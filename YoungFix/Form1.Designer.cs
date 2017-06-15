namespace YoungFix
{
    partial class Form1
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

        #region Windows Form 디자이너에서 생성한 코드

        /// <summary>
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마세요.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.btnConnection = new System.Windows.Forms.Button();
            this.btnDisconnect = new System.Windows.Forms.Button();
            this.labelConnectionFeedStatusValue = new System.Windows.Forms.Label();
            this.labelConnectionBrokerStatusValue = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.button1 = new System.Windows.Forms.Button();
            this.progressBarLoadSymbols = new System.Windows.Forms.ProgressBar();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.button3 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.lblSavePath = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.lblTargetSymbol = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.listViewMarketDepth = new System.Windows.Forms.ListView();
            this.cmbStp = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.cmbOpt = new System.Windows.Forms.ComboBox();
            this.cmbExp = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.btnBuyLimit = new System.Windows.Forms.Button();
            this.btnSellLimit = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.lblBuyPrice = new System.Windows.Forms.Label();
            this.lblBuyQty = new System.Windows.Forms.Label();
            this.lblSellQty = new System.Windows.Forms.Label();
            this.lblSellPrice = new System.Windows.Forms.Label();
            this.listViewOrders = new System.Windows.Forms.ListView();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnConnection
            // 
            this.btnConnection.Location = new System.Drawing.Point(7, 106);
            this.btnConnection.Name = "btnConnection";
            this.btnConnection.Size = new System.Drawing.Size(122, 23);
            this.btnConnection.TabIndex = 0;
            this.btnConnection.Text = "Connect";
            this.btnConnection.UseVisualStyleBackColor = true;
            this.btnConnection.Click += new System.EventHandler(this.btnConnection_Click);
            // 
            // btnDisconnect
            // 
            this.btnDisconnect.Location = new System.Drawing.Point(146, 106);
            this.btnDisconnect.Name = "btnDisconnect";
            this.btnDisconnect.Size = new System.Drawing.Size(124, 23);
            this.btnDisconnect.TabIndex = 1;
            this.btnDisconnect.Text = "Disconnect";
            this.btnDisconnect.UseVisualStyleBackColor = true;
            this.btnDisconnect.Click += new System.EventHandler(this.btnDisconnect_Click);
            // 
            // labelConnectionFeedStatusValue
            // 
            this.labelConnectionFeedStatusValue.Location = new System.Drawing.Point(9, 26);
            this.labelConnectionFeedStatusValue.Name = "labelConnectionFeedStatusValue";
            this.labelConnectionFeedStatusValue.Size = new System.Drawing.Size(261, 19);
            this.labelConnectionFeedStatusValue.TabIndex = 4;
            // 
            // labelConnectionBrokerStatusValue
            // 
            this.labelConnectionBrokerStatusValue.Location = new System.Drawing.Point(9, 54);
            this.labelConnectionBrokerStatusValue.Name = "labelConnectionBrokerStatusValue";
            this.labelConnectionBrokerStatusValue.Size = new System.Drawing.Size(261, 19);
            this.labelConnectionBrokerStatusValue.TabIndex = 5;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnConnection);
            this.groupBox1.Controls.Add(this.labelConnectionBrokerStatusValue);
            this.groupBox1.Controls.Add(this.btnDisconnect);
            this.groupBox1.Controls.Add(this.labelConnectionFeedStatusValue);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(276, 161);
            this.groupBox1.TabIndex = 6;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Connection Manager";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(8, 77);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(133, 23);
            this.button1.TabIndex = 6;
            this.button1.Text = "Start Loading";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // progressBarLoadSymbols
            // 
            this.progressBarLoadSymbols.Location = new System.Drawing.Point(8, 133);
            this.progressBarLoadSymbols.Name = "progressBarLoadSymbols";
            this.progressBarLoadSymbols.Size = new System.Drawing.Size(297, 22);
            this.progressBarLoadSymbols.TabIndex = 8;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.button3);
            this.groupBox2.Controls.Add(this.button2);
            this.groupBox2.Controls.Add(this.lblSavePath);
            this.groupBox2.Controls.Add(this.progressBarLoadSymbols);
            this.groupBox2.Controls.Add(this.button1);
            this.groupBox2.Location = new System.Drawing.Point(294, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(314, 161);
            this.groupBox2.TabIndex = 9;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Task 1";
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(8, 106);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(133, 23);
            this.button3.TabIndex = 10;
            this.button3.Text = "Generate Reports";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(8, 48);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(133, 23);
            this.button2.TabIndex = 9;
            this.button2.Text = "Set Save Destination";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // lblSavePath
            // 
            this.lblSavePath.AutoSize = true;
            this.lblSavePath.Font = new System.Drawing.Font("굴림", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblSavePath.Location = new System.Drawing.Point(6, 26);
            this.lblSavePath.Name = "lblSavePath";
            this.lblSavePath.Size = new System.Drawing.Size(33, 9);
            this.lblSavePath.TabIndex = 6;
            this.lblSavePath.Text = "Path : ";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.groupBox5);
            this.groupBox3.Controls.Add(this.groupBox4);
            this.groupBox3.Location = new System.Drawing.Point(13, 183);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(595, 385);
            this.groupBox3.TabIndex = 10;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Task 2";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.lblTargetSymbol);
            this.groupBox4.Controls.Add(this.label5);
            this.groupBox4.Controls.Add(this.listViewMarketDepth);
            this.groupBox4.Controls.Add(this.cmbStp);
            this.groupBox4.Controls.Add(this.label2);
            this.groupBox4.Controls.Add(this.cmbOpt);
            this.groupBox4.Controls.Add(this.cmbExp);
            this.groupBox4.Controls.Add(this.label4);
            this.groupBox4.Controls.Add(this.label1);
            this.groupBox4.Controls.Add(this.label3);
            this.groupBox4.Location = new System.Drawing.Point(6, 20);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(580, 103);
            this.groupBox4.TabIndex = 9;
            this.groupBox4.TabStop = false;
            // 
            // lblTargetSymbol
            // 
            this.lblTargetSymbol.AutoSize = true;
            this.lblTargetSymbol.Font = new System.Drawing.Font("굴림", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblTargetSymbol.Location = new System.Drawing.Point(125, 77);
            this.lblTargetSymbol.Name = "lblTargetSymbol";
            this.lblTargetSymbol.Size = new System.Drawing.Size(0, 9);
            this.lblTargetSymbol.TabIndex = 15;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("굴림", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label5.Location = new System.Drawing.Point(9, 77);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(113, 9);
            this.label5.TabIndex = 14;
            this.label5.Text = "Selected Target Symbol : ";
            // 
            // listViewMarketDepth
            // 
            this.listViewMarketDepth.FullRowSelect = true;
            this.listViewMarketDepth.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.listViewMarketDepth.Location = new System.Drawing.Point(381, 30);
            this.listViewMarketDepth.Name = "listViewMarketDepth";
            this.listViewMarketDepth.Size = new System.Drawing.Size(193, 67);
            this.listViewMarketDepth.TabIndex = 13;
            this.listViewMarketDepth.UseCompatibleStateImageBehavior = false;
            this.listViewMarketDepth.View = System.Windows.Forms.View.Details;
            // 
            // cmbStp
            // 
            this.cmbStp.FormattingEnabled = true;
            this.cmbStp.Location = new System.Drawing.Point(256, 30);
            this.cmbStp.Name = "cmbStp";
            this.cmbStp.Size = new System.Drawing.Size(119, 20);
            this.cmbStp.TabIndex = 11;
            this.cmbStp.SelectedIndexChanged += new System.EventHandler(this.cmbStp_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("굴림", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label2.Location = new System.Drawing.Point(254, 17);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 9);
            this.label2.TabIndex = 7;
            this.label2.Text = "Strike Price";
            // 
            // cmbOpt
            // 
            this.cmbOpt.FormattingEnabled = true;
            this.cmbOpt.Location = new System.Drawing.Point(131, 30);
            this.cmbOpt.Name = "cmbOpt";
            this.cmbOpt.Size = new System.Drawing.Size(119, 20);
            this.cmbOpt.TabIndex = 12;
            this.cmbOpt.SelectedIndexChanged += new System.EventHandler(this.cmbOpt_SelectedIndexChanged);
            // 
            // cmbExp
            // 
            this.cmbExp.FormattingEnabled = true;
            this.cmbExp.Location = new System.Drawing.Point(7, 30);
            this.cmbExp.Name = "cmbExp";
            this.cmbExp.Size = new System.Drawing.Size(118, 20);
            this.cmbExp.TabIndex = 10;
            this.cmbExp.SelectedIndexChanged += new System.EventHandler(this.cmbExp_SelectedIndexChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("굴림", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label4.Location = new System.Drawing.Point(384, 17);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(31, 9);
            this.label4.TabIndex = 9;
            this.label4.Text = "Prices";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("굴림", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label1.Location = new System.Drawing.Point(6, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(69, 9);
            this.label1.TabIndex = 6;
            this.label1.Text = "Expiration Date";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("굴림", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label3.Location = new System.Drawing.Point(129, 17);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(37, 9);
            this.label3.TabIndex = 8;
            this.label3.Text = "Options";
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.listViewOrders);
            this.groupBox5.Controls.Add(this.lblSellPrice);
            this.groupBox5.Controls.Add(this.lblSellQty);
            this.groupBox5.Controls.Add(this.lblBuyQty);
            this.groupBox5.Controls.Add(this.lblBuyPrice);
            this.groupBox5.Controls.Add(this.label9);
            this.groupBox5.Controls.Add(this.label8);
            this.groupBox5.Controls.Add(this.label7);
            this.groupBox5.Controls.Add(this.label6);
            this.groupBox5.Controls.Add(this.btnSellLimit);
            this.groupBox5.Controls.Add(this.btnBuyLimit);
            this.groupBox5.Location = new System.Drawing.Point(6, 129);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(574, 242);
            this.groupBox5.TabIndex = 10;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Task 3";
            // 
            // btnBuyLimit
            // 
            this.btnBuyLimit.Location = new System.Drawing.Point(8, 20);
            this.btnBuyLimit.Name = "btnBuyLimit";
            this.btnBuyLimit.Size = new System.Drawing.Size(75, 23);
            this.btnBuyLimit.TabIndex = 0;
            this.btnBuyLimit.Text = "Buy Limit";
            this.btnBuyLimit.UseVisualStyleBackColor = true;
            this.btnBuyLimit.Click += new System.EventHandler(this.btnBuyLimit_Click);
            // 
            // btnSellLimit
            // 
            this.btnSellLimit.Location = new System.Drawing.Point(8, 49);
            this.btnSellLimit.Name = "btnSellLimit";
            this.btnSellLimit.Size = new System.Drawing.Size(75, 23);
            this.btnSellLimit.TabIndex = 1;
            this.btnSellLimit.Text = "Sell Limit";
            this.btnSellLimit.UseVisualStyleBackColor = true;
            this.btnSellLimit.Click += new System.EventHandler(this.btnSellLimit_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("굴림", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label6.Location = new System.Drawing.Point(99, 27);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(32, 9);
            this.label6.TabIndex = 16;
            this.label6.Text = "Price :";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("굴림", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label7.Location = new System.Drawing.Point(99, 56);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(32, 9);
            this.label7.TabIndex = 17;
            this.label7.Text = "Price :";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("굴림", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label8.Location = new System.Drawing.Point(209, 27);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(26, 9);
            this.label8.TabIndex = 18;
            this.label8.Text = "Qty :";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("굴림", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label9.Location = new System.Drawing.Point(209, 56);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(26, 9);
            this.label9.TabIndex = 19;
            this.label9.Text = "Qty :";
            // 
            // lblBuyPrice
            // 
            this.lblBuyPrice.AutoSize = true;
            this.lblBuyPrice.Font = new System.Drawing.Font("굴림", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblBuyPrice.Location = new System.Drawing.Point(137, 27);
            this.lblBuyPrice.Name = "lblBuyPrice";
            this.lblBuyPrice.Size = new System.Drawing.Size(0, 9);
            this.lblBuyPrice.TabIndex = 20;
            // 
            // lblBuyQty
            // 
            this.lblBuyQty.AutoSize = true;
            this.lblBuyQty.Font = new System.Drawing.Font("굴림", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblBuyQty.Location = new System.Drawing.Point(241, 27);
            this.lblBuyQty.Name = "lblBuyQty";
            this.lblBuyQty.Size = new System.Drawing.Size(0, 9);
            this.lblBuyQty.TabIndex = 21;
            // 
            // lblSellQty
            // 
            this.lblSellQty.AutoSize = true;
            this.lblSellQty.Font = new System.Drawing.Font("굴림", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblSellQty.Location = new System.Drawing.Point(241, 56);
            this.lblSellQty.Name = "lblSellQty";
            this.lblSellQty.Size = new System.Drawing.Size(0, 9);
            this.lblSellQty.TabIndex = 22;
            // 
            // lblSellPrice
            // 
            this.lblSellPrice.AutoSize = true;
            this.lblSellPrice.Font = new System.Drawing.Font("굴림", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblSellPrice.Location = new System.Drawing.Point(137, 56);
            this.lblSellPrice.Name = "lblSellPrice";
            this.lblSellPrice.Size = new System.Drawing.Size(0, 9);
            this.lblSellPrice.TabIndex = 23;
            // 
            // listViewOrders
            // 
            this.listViewOrders.FullRowSelect = true;
            this.listViewOrders.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.listViewOrders.Location = new System.Drawing.Point(11, 78);
            this.listViewOrders.MultiSelect = false;
            this.listViewOrders.Name = "listViewOrders";
            this.listViewOrders.Size = new System.Drawing.Size(557, 136);
            this.listViewOrders.TabIndex = 24;
            this.listViewOrders.UseCompatibleStateImageBehavior = false;
            this.listViewOrders.View = System.Windows.Forms.View.Details;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(618, 583);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "YoungFix";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnConnection;
        private System.Windows.Forms.Button btnDisconnect;
        private System.Windows.Forms.Label labelConnectionFeedStatusValue;
        private System.Windows.Forms.Label labelConnectionBrokerStatusValue;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ProgressBar progressBarLoadSymbols;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label lblSavePath;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ListView listViewMarketDepth;
        private System.Windows.Forms.ComboBox cmbOpt;
        private System.Windows.Forms.ComboBox cmbStp;
        private System.Windows.Forms.ComboBox cmbExp;
        private System.Windows.Forms.Label lblTargetSymbol;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.ListView listViewOrders;
        private System.Windows.Forms.Label lblSellPrice;
        private System.Windows.Forms.Label lblSellQty;
        private System.Windows.Forms.Label lblBuyQty;
        private System.Windows.Forms.Label lblBuyPrice;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button btnSellLimit;
        private System.Windows.Forms.Button btnBuyLimit;
    }
}

