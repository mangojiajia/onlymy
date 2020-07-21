namespace ICcard
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.textBoxTruckId = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.textBoxOrder = new System.Windows.Forms.TextBox();
            this.buttonWriteCard = new System.Windows.Forms.Button();
            this.textBoxDetail = new System.Windows.Forms.TextBox();
            this.button_Valid = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // textBoxTruckId
            // 
            this.textBoxTruckId.Font = new System.Drawing.Font("宋体", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.textBoxTruckId.Location = new System.Drawing.Point(308, 57);
            this.textBoxTruckId.Margin = new System.Windows.Forms.Padding(4);
            this.textBoxTruckId.Name = "textBoxTruckId";
            this.textBoxTruckId.Size = new System.Drawing.Size(376, 53);
            this.textBoxTruckId.TabIndex = 0;
            this.textBoxTruckId.Text = "闽";
            this.textBoxTruckId.TextChanged += new System.EventHandler(this.textBoxTruckId_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("宋体", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(49, 60);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(157, 40);
            this.label1.TabIndex = 1;
            this.label1.Text = "车牌号:";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("宋体", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.Location = new System.Drawing.Point(49, 181);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(157, 40);
            this.label2.TabIndex = 1;
            this.label2.Text = "预约号:";
            this.label2.Click += new System.EventHandler(this.label2_Click);
            // 
            // textBoxOrder
            // 
            this.textBoxOrder.Font = new System.Drawing.Font("宋体", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.textBoxOrder.Location = new System.Drawing.Point(308, 178);
            this.textBoxOrder.Margin = new System.Windows.Forms.Padding(4);
            this.textBoxOrder.Name = "textBoxOrder";
            this.textBoxOrder.Size = new System.Drawing.Size(376, 53);
            this.textBoxOrder.TabIndex = 0;
            this.textBoxOrder.TextChanged += new System.EventHandler(this.textBoxOrder_TextChanged_1);
            // 
            // buttonWriteCard
            // 
            this.buttonWriteCard.Enabled = false;
            this.buttonWriteCard.Font = new System.Drawing.Font("宋体", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.buttonWriteCard.Location = new System.Drawing.Point(421, 681);
            this.buttonWriteCard.Margin = new System.Windows.Forms.Padding(4);
            this.buttonWriteCard.Name = "buttonWriteCard";
            this.buttonWriteCard.Size = new System.Drawing.Size(343, 85);
            this.buttonWriteCard.TabIndex = 2;
            this.buttonWriteCard.Text = "写入IC卡";
            this.buttonWriteCard.UseVisualStyleBackColor = true;
            this.buttonWriteCard.Click += new System.EventHandler(this.button1_Click);
            // 
            // textBoxDetail
            // 
            this.textBoxDetail.Font = new System.Drawing.Font("宋体", 36F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.textBoxDetail.Location = new System.Drawing.Point(308, 266);
            this.textBoxDetail.Margin = new System.Windows.Forms.Padding(4);
            this.textBoxDetail.Multiline = true;
            this.textBoxDetail.Name = "textBoxDetail";
            this.textBoxDetail.ReadOnly = true;
            this.textBoxDetail.Size = new System.Drawing.Size(667, 382);
            this.textBoxDetail.TabIndex = 3;
            this.textBoxDetail.TextChanged += new System.EventHandler(this.textBoxDetail_TextChanged);
            this.textBoxDetail.DoubleClick += new System.EventHandler(this.button_read_Click);
            // 
            // button_Valid
            // 
            this.button_Valid.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button_Valid.Location = new System.Drawing.Point(783, 114);
            this.button_Valid.Name = "button_Valid";
            this.button_Valid.Size = new System.Drawing.Size(162, 57);
            this.button_Valid.TabIndex = 6;
            this.button_Valid.Text = "验证预约";
            this.button_Valid.UseVisualStyleBackColor = true;
            this.button_Valid.Click += new System.EventHandler(this.button_Valid_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("宋体", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label3.Location = new System.Drawing.Point(19, 291);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(197, 40);
            this.label3.TabIndex = 1;
            this.label3.Text = "预约详情:";
            this.label3.Click += new System.EventHandler(this.label3_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1069, 808);
            this.Controls.Add(this.button_Valid);
            this.Controls.Add(this.textBoxDetail);
            this.Controls.Add(this.buttonWriteCard);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBoxOrder);
            this.Controls.Add(this.textBoxTruckId);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "Form1";
            this.Text = "永荣智能物流IC卡";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBoxTruckId;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBoxOrder;
        private System.Windows.Forms.Button buttonWriteCard;
        private System.Windows.Forms.TextBox textBoxDetail;
        private System.Windows.Forms.Button button_Valid;
        private System.Windows.Forms.Label label3;
    }
}

