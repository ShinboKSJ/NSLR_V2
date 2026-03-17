namespace NSLR_ObservationControl.Module
{
    partial class RecordManagement_AccessHistory
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
            this.dataGridView_AccessHistory = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_AccessHistory)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridView_AccessHistory
            // 
            this.dataGridView_AccessHistory.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.DisplayedHeaders;
            this.dataGridView_AccessHistory.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.dataGridView_AccessHistory.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView_AccessHistory.Location = new System.Drawing.Point(3, 2);
            this.dataGridView_AccessHistory.Name = "dataGridView_AccessHistory";
            this.dataGridView_AccessHistory.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders;
            this.dataGridView_AccessHistory.RowTemplate.Height = 23;
            this.dataGridView_AccessHistory.Size = new System.Drawing.Size(2629, 1514);
            this.dataGridView_AccessHistory.TabIndex = 0;
            // 
            // RecordManagement_AccessHistory
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.Controls.Add(this.dataGridView_AccessHistory);
            this.Name = "RecordManagement_AccessHistory";
            this.Size = new System.Drawing.Size(3611, 2040);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_AccessHistory)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView_AccessHistory;
    }
}
