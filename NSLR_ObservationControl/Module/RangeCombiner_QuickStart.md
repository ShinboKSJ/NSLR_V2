# Range Combiner 빠른 시작 가이드

## 5분 안에 적용하기

### 📋 준비사항 확인

- [ ] RGG에서 `interpolated_UtcMs`, `interpolated_ToF` 리스트를 제공하는가?
- [ ] Event Timer가 UTC time of day (초) 단위로 시간을 전송하는가?
- [ ] `GlobalVariables.SystemDelay` 값이 설정되어 있는가?
- [ ] 네트워크 스트림 (`stream`)이 연결되어 있는가?

### 🚀 3단계 적용

#### Step 1: 필드 추가

기존 `Observation_Control.cs`에 필드 추가:

```csharp
public partial class Observation_Control : UserControl
{
    // 기존 필드들...
    
    // 추가: Range Combiner
    private RealTimeRangeCombiner _rangeCombiner;
    private CancellationTokenSource _readCts;
}
```

#### Step 2: 초기화 메서드 작성

```csharp
private void InitializeRangeCombiner()
{
    // 1. Range Combiner 생성
    _rangeCombiner = new RealTimeRangeCombiner();
    
    // 2. RGG Lookup Table 로드
    List<long> utcMs;
    List<double> tof;
    
    if (DutyModeNow.CurrentSystemObject == DutyModeNow.SystemObject.SLR)
    {
        utcMs = Subsystem.RGG_SAT_Controller.interpolated_UtcMs;
        tof = Subsystem.RGG_SAT_Controller.interpolated_ToF;
    }
    else
    {
        utcMs = Subsystem.RGG_DEB_Controller.interpolated_UtcMs;
        tof = Subsystem.RGG_DEB_Controller.interpolated_ToF;
    }
    
    _rangeCombiner.SetLookupTable(utcMs, tof);
    
    // 3. 이벤트 핸들러 등록
    _rangeCombiner.OnMeasurementBatch += OnMeasurementBatch;
    _rangeCombiner.OnHistogramUpdate += OnHistogramUpdate;
    _rangeCombiner.OnStatisticsUpdate += OnStats;
    
    // 4. 시작
    _rangeCombiner.Start();
    
    Console.WriteLine($"[INFO] Range Combiner started with {_rangeCombiner.LookupTable.Count} lookup entries");
}

// 차트 업데이트 핸들러
private void OnMeasurementBatch(List<RealTimeRangeCombiner.LaserMeasurement> measurements)
{
    if (InvokeRequired)
    {
        BeginInvoke(new Action(() => OnMeasurementBatch(measurements)));
        return;
    }
    
    var chart1 = this.Parent?.Controls.Find("chart1", true).FirstOrDefault() as Chart;
    if (chart1 == null) return;
    
    foreach (var m in measurements)
    {
        chart1.Series[0].Points.AddXY(m.EndTime, m.OminusC);
        
        // 너무 많으면 제거
        if (chart1.Series[0].Points.Count > 10000)
            chart1.Series[0].Points.RemoveAt(0);
    }
}

// 히스토그램 핸들러
private void OnHistogramUpdate(Dictionary<int, int> histogram)
{
    if (InvokeRequired)
    {
        BeginInvoke(new Action(() => OnHistogramUpdate(histogram)));
        return;
    }
    
    var chart2 = this.Parent?.Controls.Find("chart2", true).FirstOrDefault() as Chart;
    if (chart2 == null) return;
    
    chart2.Series[0].Points.Clear();
    foreach (var kvp in histogram.OrderBy(k => k.Key))
        chart2.Series[0].Points.AddXY(kvp.Key, kvp.Value);
}

// 통계 핸들러
private void OnStats(RealTimeRangeCombiner.ProcessingStatistics stats)
{
    Console.WriteLine($"Paired: {stats.TotalPaired}, Rate: {stats.PairingRate:F1}%, Latency: {stats.ProcessingLatencyMs:F2}ms");
}
```

#### Step 3: 관측 시작/중지 버튼 수정

```csharp
// 기존 button2_Click_2 또는 관측 시작 버튼
private async void StartObservation_Click(object sender, EventArgs e)
{
    try
    {
        // 레이저 시작
        if (DutyModeNow.CurrentSystemObject == DutyModeNow.SystemObject.SLR)
        {
            Subsystem.LAS_SAT_Controller.instance.laserStart_SatTrack();
            Subsystem.RGG_SAT_Controller.instance.SetRangeMode();
        }
        else
        {
            Subsystem.LAS_DEB_Controller.instance.laserStart_DebTrack();
            Subsystem.RGG_DEB_Controller.instance.SetRangeMode();
        }
        
        // Range Combiner 초기화
        if (_rangeCombiner == null)
            InitializeRangeCombiner();
        else
            _rangeCombiner.ResetStatistics();
        
        // Event Timer 읽기 시작
        _readCts = new CancellationTokenSource();
        await ReadEventTimerAsync(_readCts.Token);
    }
    catch (Exception ex)
    {
        MessageBox.Show($"시작 실패: {ex.Message}", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
    }
}

// Event Timer 읽기
private async Task ReadEventTimerAsync(CancellationToken ct)
{
    byte[] buffer = new byte[64];
    
    while (!ct.IsCancellationRequested)
    {
        try
        {
            if (stream == null || !stream.CanRead)
            {
                await Task.Delay(100, ct);
                continue;
            }
            
            int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length, ct);
            if (bytesRead == 0) break;
            
            // Range Combiner에 전달 (복사본)
            byte[] dataCopy = new byte[bytesRead];
            Array.Copy(buffer, dataCopy, bytesRead);
            _rangeCombiner.EnqueueRawData(dataCopy, bytesRead);
        }
        catch (TaskCanceledException) { break; }
        catch (Exception ex)
        {
            Console.WriteLine($"Read error: {ex.Message}");
            await Task.Delay(100, ct);
        }
    }
}

// 중지 버튼
private void StopObservation_Click(object sender, EventArgs e)
{
    _readCts?.Cancel();
    
    if (DutyModeNow.CurrentSystemObject == DutyModeNow.SystemObject.SLR)
    {
        Subsystem.LAS_SAT_Controller.instance.laserStop_SatTrack();
        Subsystem.RGG_SAT_Controller.instance.SetStandbyMode();
    }
    else
    {
        Subsystem.LAS_DEB_Controller.instance.laserStop_DebTrack();
        Subsystem.RGG_DEB_Controller.instance.SetStandbyMode();
    }
    
    _rangeCombiner?.Stop();
    SaveMeasurements();
}

// 데이터 저장
private void SaveMeasurements()
{
    var measurements = _rangeCombiner?.GetAllMeasurements();
    if (measurements == null || measurements.Count == 0) return;
    
    string filename = $"Range_{DateTime.Now:yyyyMMdd_HHmmss}.dat";
    using (var writer = new StreamWriter(filename))
    {
        writer.WriteLine("# Laser Range Measurements");
        writer.WriteLine($"# Total: {measurements.Count}");
        writer.WriteLine("# SequenceId,StartTime(s),EndTime(s),ToF(s),O-C(ms),Az(deg),El(deg)");
        
        foreach (var m in measurements)
        {
            writer.WriteLine($"{m.SequenceId},{m.StartTime:F12},{m.EndTime:F12}," +
                           $"{m.ToF:F12},{m.OminusC:F6},{m.AzimuthDeg:F6},{m.ElevationDeg:F6}");
        }
    }
    
    MessageBox.Show($"저장 완료: {filename}\n개수: {measurements.Count}", 
        "저장", MessageBoxButtons.OK, MessageBoxIcon.Information);
}
```

---

## ✅ 확인 사항

### 1. Lookup Table이 제대로 로드되었는지 확인

```csharp
var stats = _rangeCombiner.GetStatistics();
Console.WriteLine($"Lookup Table Size: {stats.LookupTableSize}");

// 0이면 문제!
if (stats.LookupTableSize == 0)
{
    MessageBox.Show("RGG Lookup Table이 비어있습니다!", "경고");
}
```

### 2. 페어링이 잘 되는지 확인

관측 시작 후 10초 뒤:
```csharp
var stats = _rangeCombiner.GetStatistics();
Console.WriteLine($"Pairing Rate: {stats.PairingRate:F1}%");

// 80% 이하면 문제 가능성
if (stats.PairingRate < 80.0)
{
    Console.WriteLine($"Lookup Misses: {stats.LookupMisses}");
}
```

### 3. 시간 범위 확인

```csharp
var utcMs = RGG_SAT_Controller.interpolated_UtcMs;
if (utcMs != null && utcMs.Count > 0)
{
    Console.WriteLine($"Lookup Time Range: {utcMs[0]} ~ {utcMs.Last()} ms");
    
    // 현재 시간과 비교
    long nowMs = (long)DateTime.UtcNow.TimeOfDay.TotalMilliseconds;
    Console.WriteLine($"Current Time: {nowMs} ms");
    
    if (nowMs < utcMs[0] || nowMs > utcMs.Last())
    {
        Console.WriteLine("[WARNING] Current time is outside lookup range!");
    }
}
```

---

## 🐛 문제 해결

### 문제 1: LookupTableSize가 0

**원인**: RGG에 예측 궤도 데이터가 전송되지 않음

**해결**:
```csharp
// RGG에 데이터 전송 확인
if (RGG_SAT_Controller.interpolated_UtcMs == null || 
    RGG_SAT_Controller.interpolated_UtcMs.Count == 0)
{
    MessageBox.Show("먼저 RGG에 예측 궤도 데이터를 전송하세요!");
}
```

### 문제 2: Pairing Rate가 낮음 (< 50%)

**원인**: Lookup Miss가 많거나 시간 동기화 문제

**해결**:
```csharp
var stats = _rangeCombiner.GetStatistics();

if (stats.LookupMisses > stats.TotalReceived * 0.1)
{
    Console.WriteLine("Lookup Miss가 10% 이상입니다!");
    Console.WriteLine("→ Event Timer와 RGG의 시간 기준이 다를 수 있습니다.");
    Console.WriteLine("→ Lookup Table의 시간 범위를 확인하세요.");
}
```

### 문제 3: 처리 지연이 큼 (> 10ms)

**원인**: CPU 부하 또는 UI 블로킹

**해결**:
- 다른 프로세스 종료
- UI 업데이트 주기 증가 (50ms → 100ms)
- 차트 포인트 개수 제한 (10000개 → 5000개)

---

## 📊 실시간 모니터링

Form에 Label을 추가하여 실시간 통계 표시:

```csharp
private void OnStats(RealTimeRangeCombiner.ProcessingStatistics stats)
{
    if (InvokeRequired)
    {
        BeginInvoke(new Action(() => OnStats(stats)));
        return;
    }
    
    // Label 업데이트 (Form Designer에서 추가)
    lblReceived.Text = $"Received: {stats.TotalReceived}";
    lblPaired.Text = $"Paired: {stats.TotalPaired}";
    lblRate.Text = $"Rate: {stats.PairingRate:F1}%";
    lblLatency.Text = $"Latency: {stats.ProcessingLatencyMs:F2}ms";
    lblLookupMisses.Text = $"Lookup Misses: {stats.LookupMisses}";
    
    // 경고 표시
    if (stats.PairingRate < 80)
        lblRate.ForeColor = Color.Red;
    else
        lblRate.ForeColor = Color.Green;
}
```

---

## 🎯 완료!

이제 관측 시작 버튼을 누르면:
1. RGG Lookup Table이 자동으로 로드됨
2. Event Timer 데이터가 실시간으로 처리됨
3. 차트가 60Hz가 아닌 20Hz로 부드럽게 업데이트됨
4. CPU 사용률이 낮아지고 실시간성이 향상됨

더 자세한 내용은 `RangeCombiner_README.md`를 참조하세요!
