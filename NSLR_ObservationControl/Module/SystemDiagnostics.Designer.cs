using System.Windows.Forms;

namespace NSLR_ObservationControl.Module
{
    partial class SystemDiagnostics
    {
        /// <summary> 
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 구성 요소 디자이너에서 생성한 코드

        /// <summary> 
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마세요.
        /// </summary>
        private void InitializeComponent()
        {
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.systemDiagnostic_RGG_SAT1 = new NSLR_ObservationControl.Module.SystemDiagnostic_RGG_SAT();
            this.DOM = new NSLR_ObservationControl.Module.SystemDiagnostic_DOM();
            this.LAS_DEB = new NSLR_ObservationControl.Module.SystemDiagnostic_LAS_DEB();
            this.AID = new NSLR_ObservationControl.Module.SystemDiagnostic_AID();
            this.LAS_SAT = new NSLR_ObservationControl.Module.SystemDiagnostic_LAS_SAT();
            this.systemDiagnostic_RGG_DEB1 = new NSLR_ObservationControl.Module.SystemDiagnostic_RGG_DEB();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 19.34211F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 19.57895F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 61.07895F));
            this.tableLayoutPanel1.Controls.Add(this.systemDiagnostic_RGG_SAT1, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.DOM, 2, 1);
            this.tableLayoutPanel1.Controls.Add(this.LAS_DEB, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.AID, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.LAS_SAT, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.systemDiagnostic_RGG_DEB1, 1, 1);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(4, 2);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 47.65013F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 52.34987F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(3800, 1532);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // systemDiagnostic_RGG_SAT1
            // 
            this.systemDiagnostic_RGG_SAT1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(60)))));
            this.systemDiagnostic_RGG_SAT1.Location = new System.Drawing.Point(738, 3);
            this.systemDiagnostic_RGG_SAT1.message = null;
            this.systemDiagnostic_RGG_SAT1.Name = "systemDiagnostic_RGG_SAT1";
            this.systemDiagnostic_RGG_SAT1.seq_num_rx = ((ulong)(0ul));
            this.systemDiagnostic_RGG_SAT1.seq_num_tx = ((ulong)(0ul));
            this.systemDiagnostic_RGG_SAT1.Size = new System.Drawing.Size(723, 724);
            this.systemDiagnostic_RGG_SAT1.TabIndex = 4;
            // 
            // DOM
            // 
            this.DOM.BackColor = System.Drawing.Color.Transparent;
            this.DOM.Location = new System.Drawing.Point(1482, 733);
            this.DOM.Name = "DOM";
            this.DOM.Size = new System.Drawing.Size(722, 732);
            this.DOM.TabIndex = 2;
            // 
            // LAS_DEB
            // 
            this.LAS_DEB.BackColor = System.Drawing.Color.Transparent;
            this.LAS_DEB.laserOpState = 1;
            this.LAS_DEB.laserStartStop = 0;
            this.LAS_DEB.Location = new System.Drawing.Point(3, 734);
            this.LAS_DEB.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.LAS_DEB.Name = "LAS_DEB";
            this.LAS_DEB.Size = new System.Drawing.Size(722, 731);
            this.LAS_DEB.TabIndex = 1;
            this.LAS_DEB.TX_LaserMode = "00";
            this.LAS_DEB.TX_LaserOpEnd = "00";
            this.LAS_DEB.TX_LaserOpMode = "00";
            this.LAS_DEB.TX_LaserStartStop = "00";
            this.LAS_DEB.TX_PACKET = "70717883010200020000000083787170";
            // 
            // AID
            // 
            this.AID.AID_state_ADSBSync = ((byte)(0));
            this.AID.AID_state_CBITresult = ((byte)(0));
            this.AID.AID_state_IBITresult = ((byte)(0));
            this.AID.AID_state_OpMode = ((byte)(0));
            this.AID.AID_state_PBITresult = ((byte)(0));
            this.AID.AID_state_RadarSync = ((byte)(0));
            this.AID.BackColor = System.Drawing.Color.Transparent;
            this.AID.Location = new System.Drawing.Point(1482, 3);
            this.AID.Name = "AID";
            this.AID.Size = new System.Drawing.Size(726, 719);
            this.AID.TabIndex = 3;
            // 
            // LAS_SAT
            // 
            this.LAS_SAT.BackColor = System.Drawing.Color.Transparent;
            this.LAS_SAT.laserOpState = 1;
            this.LAS_SAT.laserStartStop = 0;
            this.LAS_SAT.Location = new System.Drawing.Point(3, 4);
            this.LAS_SAT.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.LAS_SAT.Name = "LAS_SAT";
            this.LAS_SAT.Size = new System.Drawing.Size(722, 717);
            this.LAS_SAT.TabIndex = 1;
            this.LAS_SAT.TX_LaserMode = "00";
            this.LAS_SAT.TX_LaserOpEnd = "00";
            this.LAS_SAT.TX_LaserOpMode = "00";
            this.LAS_SAT.TX_LaserStartStop = "00";
            this.LAS_SAT.TX_PACKET = "70717883010200020000000083787170";
            // 
            // systemDiagnostic_RGG_DEB1
            // 
            this.systemDiagnostic_RGG_DEB1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(60)))));
            this.systemDiagnostic_RGG_DEB1.Location = new System.Drawing.Point(738, 733);
            this.systemDiagnostic_RGG_DEB1.message = null;
            this.systemDiagnostic_RGG_DEB1.Name = "systemDiagnostic_RGG_DEB1";
            this.systemDiagnostic_RGG_DEB1.seq_num_rx = ((ulong)(0ul));
            this.systemDiagnostic_RGG_DEB1.seq_num_tx = ((ulong)(0ul));
            this.systemDiagnostic_RGG_DEB1.Size = new System.Drawing.Size(723, 731);
            this.systemDiagnostic_RGG_DEB1.TabIndex = 5;
            // 
            // SystemDiagnostics
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(60)))));
            this.Controls.Add(this.tableLayoutPanel1);
            this.DoubleBuffered = true;
            this.Name = "SystemDiagnostics";
            this.Size = new System.Drawing.Size(2228, 1479);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private TableLayoutPanel tableLayoutPanel1;
        private SystemDiagnostic_LAS_DEB LAS_DEB;
        private SystemDiagnostic_DOM DOM;
        private SystemDiagnostic_AID AID;
        private SystemDiagnostic_LAS_SAT LAS_SAT;
        private SystemDiagnostic_RGG_SAT systemDiagnostic_RGG_SAT1;
        private SystemDiagnostic_RGG_DEB systemDiagnostic_RGG_DEB1;
    }
}
