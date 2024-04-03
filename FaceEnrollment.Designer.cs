namespace MultiFaceRec
{
    partial class FrmPrincipal
    {
        private System.ComponentModel.IContainer components = null;

        /// <param name="disposing">true si los recursos administrados se deben eliminar; false en caso contrario, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de Windows Forms

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmPrincipal));
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel4 = new System.Windows.Forms.Panel();
            this.label8 = new System.Windows.Forms.Label();
            this.panel_btn = new System.Windows.Forms.Panel();
            this.face_enroll_admin = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.imageBoxFrameGrabber = new Emgu.CV.UI.ImageBox();
            this.groupBox9 = new System.Windows.Forms.GroupBox();
            this.capture_btn = new System.Windows.Forms.Button();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.names_cmbx = new System.Windows.Forms.ComboBox();
            this.capture_groupBox = new System.Windows.Forms.GroupBox();
            this.label14 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label11 = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.image_name_txbx = new System.Windows.Forms.TextBox();
            this.enable_btn = new System.Windows.Forms.Button();
            this.enable_groupBox = new System.Windows.Forms.GroupBox();
            this.label13 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.label9 = new System.Windows.Forms.Label();
            this.num_of_device_cmbx = new System.Windows.Forms.ComboBox();
            this.video_src_cmbx = new System.Windows.Forms.ComboBox();
            this.imageBoxFrameGrabber = new Emgu.CV.UI.ImageBox();
            this.groupBox8 = new System.Windows.Forms.GroupBox();
            this.label12 = new System.Windows.Forms.Label();
            this.groupBox7 = new System.Windows.Forms.GroupBox();
            this.panel3 = new System.Windows.Forms.Panel();
            this.panel1.SuspendLayout();
            this.panel_btn.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.imageBox1)).BeginInit();
            this.groupBox9.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.capture_groupBox.SuspendLayout();
            this.panel2.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.enable_groupBox.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.imageBoxFrameGrabber)).BeginInit();
            this.groupBox7.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.panel1.Controls.Add(this.panel4);
            this.panel1.Controls.Add(this.label8);
            this.panel1.Controls.Add(this.panel_btn);
            this.panel1.Location = new System.Drawing.Point(481, 71);
            this.panel1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(549, 62);
            this.panel1.TabIndex = 39;
            // 
            // panel4
            // 
            this.panel4.BackColor = System.Drawing.Color.SteelBlue;
            this.panel4.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel4.Location = new System.Drawing.Point(0, 0);
            this.panel4.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(67, 62);
            this.panel4.TabIndex = 25;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.ForeColor = System.Drawing.Color.White;
            this.label8.Location = new System.Drawing.Point(197, 22);
            this.label8.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(190, 20);
            this.label8.TabIndex = 17;
            this.label8.Text = "FACE RECOGNITION";
            // 
            // panel_btn
            // 
            this.panel_btn.BackColor = System.Drawing.Color.SteelBlue;
            this.panel_btn.Controls.Add(this.face_enroll_admin);
            this.panel_btn.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel_btn.Location = new System.Drawing.Point(465, 0);
            this.panel_btn.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.panel_btn.Name = "panel_btn";
            this.panel_btn.Size = new System.Drawing.Size(84, 62);
            this.panel_btn.TabIndex = 25;
            // 
            // face_enroll_admin
            // 
            this.face_enroll_admin.BackColor = System.Drawing.Color.DeepSkyBlue;
            this.face_enroll_admin.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("face_enroll_admin.BackgroundImage")));
            this.face_enroll_admin.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.face_enroll_admin.Font = new System.Drawing.Font("Arial Rounded MT Bold", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.face_enroll_admin.ForeColor = System.Drawing.Color.White;
            this.face_enroll_admin.Location = new System.Drawing.Point(4, 2);
            this.face_enroll_admin.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.face_enroll_admin.Name = "face_enroll_admin";
            this.face_enroll_admin.Size = new System.Drawing.Size(75, 58);
            this.face_enroll_admin.TabIndex = 18;
            this.face_enroll_admin.UseVisualStyleBackColor = false;
            this.face_enroll_admin.Click += new System.EventHandler(this.face_enroll_admin_Click);
            this.face_enroll_admin.MouseLeave += new System.EventHandler(this.face_enroll_admin_MouseLeave);
            this.face_enroll_admin.MouseHover += new System.EventHandler(this.face_enroll_admin_MouseHover);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.BackColor = System.Drawing.Color.Transparent;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.Color.White;
            this.label5.Location = new System.Drawing.Point(29, 23);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(183, 18);
            this.label5.TabIndex = 17;
            this.label5.Text = "IDENTIFIED PERSON :";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.Cyan;
            this.label4.Location = new System.Drawing.Point(220, 19);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(90, 24);
            this.label4.TabIndex = 16;
            this.label4.Text = "Unknown";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.Red;
            this.label3.Location = new System.Drawing.Point(260, 84);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(19, 20);
            this.label3.TabIndex = 15;
            this.label3.Text = "0";
            this.label3.Visible = false;
            // 
            // groupBox2
            // 
            this.groupBox2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.groupBox2.Controls.Add(this.groupBox3);
            this.groupBox2.Controls.Add(this.groupBox9);
            this.groupBox2.Location = new System.Drawing.Point(16, 140);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox2.Size = new System.Drawing.Size(465, 711);
            this.groupBox2.TabIndex = 25;
            this.groupBox2.TabStop = false;
            // 
            // groupBox3
            // 
            this.groupBox3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.groupBox3.Controls.Add(this.imageBox1);
            this.groupBox3.Location = new System.Drawing.Point(96, 80);
            this.groupBox3.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox3.Size = new System.Drawing.Size(272, 260);
            this.groupBox3.TabIndex = 24;
            this.groupBox3.TabStop = false;
            // 
            // imageBox1
            // 
            this.imageBox1.BackColor = System.Drawing.Color.White;
            this.imageBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.imageBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.imageBox1.Location = new System.Drawing.Point(3, 10);
            this.imageBox1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.imageBox1.Name = "imageBox1";
            this.imageBox1.Size = new System.Drawing.Size(266, 246);
            this.imageBox1.TabIndex = 23;
            this.imageBox1.TabStop = false;
            // 
            // groupBox9
            // 
            this.groupBox9.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.groupBox9.Controls.Add(this.capture_btn);
            this.groupBox9.Controls.Add(this.groupBox6);
            this.groupBox9.Controls.Add(this.capture_groupBox);
            this.groupBox9.Location = new System.Drawing.Point(8, 382);
            this.groupBox9.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox9.Name = "groupBox9";
            this.groupBox9.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox9.Size = new System.Drawing.Size(449, 191);
            this.groupBox9.TabIndex = 25;
            this.groupBox9.TabStop = false;
            // 
            // capture_btn
            // 
            this.capture_btn.BackColor = System.Drawing.Color.Gainsboro;
            this.capture_btn.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("capture_btn.BackgroundImage")));
            this.capture_btn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.capture_btn.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.capture_btn.Location = new System.Drawing.Point(29, 53);
            this.capture_btn.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.capture_btn.Name = "capture_btn";
            this.capture_btn.Size = new System.Drawing.Size(95, 76);
            this.capture_btn.TabIndex = 2;
            this.capture_btn.UseVisualStyleBackColor = false;
            this.capture_btn.Click += new System.EventHandler(this.button2_Click);
            this.capture_btn.MouseLeave += new System.EventHandler(this.capture_btn_MouseLeave);
            this.capture_btn.MouseHover += new System.EventHandler(this.capture_btn_MouseHover);
            // 
            // groupBox6
            // 
            this.groupBox6.BackColor = System.Drawing.Color.DimGray;
            this.groupBox6.Controls.Add(this.label1);
            this.groupBox6.Controls.Add(this.names_cmbx);
            this.groupBox6.Location = new System.Drawing.Point(152, 34);
            this.groupBox6.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox6.Size = new System.Drawing.Size(280, 126);
            this.groupBox6.TabIndex = 41;
            this.groupBox6.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(45, 28);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(188, 18);
            this.label1.TabIndex = 8;
            this.label1.Text = "REGISTERED NAMES :";
            // 
            // names_cmbx
            // 
            this.names_cmbx.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.names_cmbx.DropDownWidth = 50;
            this.names_cmbx.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.names_cmbx.FormattingEnabled = true;
            this.names_cmbx.IntegralHeight = false;
            this.names_cmbx.Location = new System.Drawing.Point(11, 71);
            this.names_cmbx.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.names_cmbx.Name = "names_cmbx";
            this.names_cmbx.Size = new System.Drawing.Size(259, 28);
            this.names_cmbx.TabIndex = 1;
            // 
            // capture_groupBox
            // 
            this.capture_groupBox.BackColor = System.Drawing.Color.SeaGreen;
            this.capture_groupBox.Controls.Add(this.label14);
            this.capture_groupBox.Location = new System.Drawing.Point(19, 41);
            this.capture_groupBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.capture_groupBox.Name = "capture_groupBox";
            this.capture_groupBox.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.capture_groupBox.Size = new System.Drawing.Size(116, 118);
            this.capture_groupBox.TabIndex = 19;
            this.capture_groupBox.TabStop = false;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.BackColor = System.Drawing.Color.Transparent;
            this.label14.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label14.ForeColor = System.Drawing.Color.White;
            this.label14.Location = new System.Drawing.Point(17, 94);
            this.label14.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(80, 17);
            this.label14.TabIndex = 42;
            this.label14.Text = "CAPTURE";
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.panel2.Controls.Add(this.label11);
            this.panel2.Location = new System.Drawing.Point(16, 71);
            this.panel2.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(465, 62);
            this.panel2.TabIndex = 40;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.ForeColor = System.Drawing.Color.White;
            this.label11.Location = new System.Drawing.Point(168, 22);
            this.label11.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(139, 20);
            this.label11.TabIndex = 17;
            this.label11.Text = "TRAIN IMAGES";
            // 
            // timer1
            // 
            this.timer1.Interval = 2000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // groupBox5
            // 
            this.groupBox5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.groupBox5.Controls.Add(this.image_name_txbx);
            this.groupBox5.Controls.Add(this.enable_btn);
            this.groupBox5.Controls.Add(this.enable_groupBox);
            this.groupBox5.Controls.Add(this.groupBox1);
            this.groupBox5.Location = new System.Drawing.Point(481, 656);
            this.groupBox5.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox5.Size = new System.Drawing.Size(549, 196);
            this.groupBox5.TabIndex = 38;
            this.groupBox5.TabStop = false;
            // 
            // image_name_txbx
            // 
            this.image_name_txbx.Location = new System.Drawing.Point(337, 161);
            this.image_name_txbx.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.image_name_txbx.Name = "image_name_txbx";
            this.image_name_txbx.Size = new System.Drawing.Size(177, 22);
            this.image_name_txbx.TabIndex = 42;
            this.image_name_txbx.Visible = false;
            // 
            // enable_btn
            // 
            this.enable_btn.BackColor = System.Drawing.Color.Gainsboro;
            this.enable_btn.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("enable_btn.BackgroundImage")));
            this.enable_btn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.enable_btn.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.enable_btn.Location = new System.Drawing.Point(45, 49);
            this.enable_btn.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.enable_btn.Name = "enable_btn";
            this.enable_btn.Size = new System.Drawing.Size(95, 76);
            this.enable_btn.TabIndex = 0;
            this.enable_btn.UseVisualStyleBackColor = false;
            this.enable_btn.Click += new System.EventHandler(this.button1_Click);
            this.enable_btn.MouseLeave += new System.EventHandler(this.enable_btn_MouseLeave);
            this.enable_btn.MouseHover += new System.EventHandler(this.enable_btn_MouseHover);
            // 
            // enable_groupBox
            // 
            this.enable_groupBox.BackColor = System.Drawing.Color.SteelBlue;
            this.enable_groupBox.Controls.Add(this.label13);
            this.enable_groupBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.enable_groupBox.Location = new System.Drawing.Point(35, 37);
            this.enable_groupBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.enable_groupBox.Name = "enable_groupBox";
            this.enable_groupBox.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.enable_groupBox.Size = new System.Drawing.Size(116, 118);
            this.enable_groupBox.TabIndex = 19;
            this.enable_groupBox.TabStop = false;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.BackColor = System.Drawing.Color.Transparent;
            this.label13.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label13.ForeColor = System.Drawing.Color.White;
            this.label13.Location = new System.Drawing.Point(21, 95);
            this.label13.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(68, 17);
            this.label13.TabIndex = 17;
            this.label13.Text = "ENABLE";
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.Color.DimGray;
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Location = new System.Drawing.Point(172, 30);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox1.Size = new System.Drawing.Size(344, 132);
            this.groupBox1.TabIndex = 20;
            this.groupBox1.TabStop = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(39, 84);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(155, 18);
            this.label2.TabIndex = 17;
            this.label2.Text = "FACE DETECTED :";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(356, 463);
            this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(38, 17);
            this.label7.TabIndex = 30;
            this.label7.Text = "Date";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(352, 418);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(45, 17);
            this.label6.TabIndex = 29;
            this.label6.Text = "Name";
            // 
            // groupBox4
            // 
            this.groupBox4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.groupBox4.Controls.Add(this.label9);
            this.groupBox4.Controls.Add(this.num_of_device_cmbx);
            this.groupBox4.Controls.Add(this.video_src_cmbx);
            this.groupBox4.Controls.Add(this.imageBoxFrameGrabber);
            this.groupBox4.Controls.Add(this.groupBox8);
            this.groupBox4.Location = new System.Drawing.Point(481, 140);
            this.groupBox4.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox4.Size = new System.Drawing.Size(549, 510);
            this.groupBox4.TabIndex = 26;
            this.groupBox4.TabStop = false;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.BackColor = System.Drawing.Color.Transparent;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.ForeColor = System.Drawing.Color.White;
            this.label9.Location = new System.Drawing.Point(279, 480);
            this.label9.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(156, 17);
            this.label9.TabIndex = 17;
            this.label9.Text = "Capture Device (index):";
            // 
            // num_of_device_cmbx
            // 
            this.num_of_device_cmbx.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.num_of_device_cmbx.FormattingEnabled = true;
            this.num_of_device_cmbx.Location = new System.Drawing.Point(436, 476);
            this.num_of_device_cmbx.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.num_of_device_cmbx.Name = "num_of_device_cmbx";
            this.num_of_device_cmbx.Size = new System.Drawing.Size(89, 24);
            this.num_of_device_cmbx.TabIndex = 25;
            // 
            // video_src_cmbx
            // 
            this.video_src_cmbx.FormattingEnabled = true;
            this.video_src_cmbx.Location = new System.Drawing.Point(436, 476);
            this.video_src_cmbx.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.video_src_cmbx.Name = "video_src_cmbx";
            this.video_src_cmbx.Size = new System.Drawing.Size(89, 24);
            this.video_src_cmbx.TabIndex = 24;
            // 
            // imageBoxFrameGrabber
            // 
            this.imageBoxFrameGrabber.BackColor = System.Drawing.Color.White;
            this.imageBoxFrameGrabber.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.imageBoxFrameGrabber.Location = new System.Drawing.Point(60, 100);
            this.imageBoxFrameGrabber.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.imageBoxFrameGrabber.Name = "imageBoxFrameGrabber";
            this.imageBoxFrameGrabber.Size = new System.Drawing.Size(426, 295);
            this.imageBoxFrameGrabber.TabIndex = 4;
            this.imageBoxFrameGrabber.TabStop = false;
            // 
            // groupBox8
            // 
            this.groupBox8.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.groupBox8.Location = new System.Drawing.Point(57, 90);
            this.groupBox8.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox8.Name = "groupBox8";
            this.groupBox8.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox8.Size = new System.Drawing.Size(432, 310);
            this.groupBox8.TabIndex = 24;
            this.groupBox8.TabStop = false;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.ForeColor = System.Drawing.Color.White;
            this.label12.Location = new System.Drawing.Point(367, 26);
            this.label12.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(300, 24);
            this.label12.TabIndex = 17;
            this.label12.Text = "FACE ENROLLMENT SYSTEM";
            // 
            // groupBox7
            // 
            this.groupBox7.BackColor = System.Drawing.Color.Maroon;
            this.groupBox7.Controls.Add(this.label12);
            this.groupBox7.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox7.Location = new System.Drawing.Point(0, 0);
            this.groupBox7.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox7.Name = "groupBox7";
            this.groupBox7.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox7.Size = new System.Drawing.Size(1031, 66);
            this.groupBox7.TabIndex = 41;
            this.groupBox7.TabStop = false;
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.SeaGreen;
            this.panel3.Location = new System.Drawing.Point(16, 71);
            this.panel3.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(67, 62);
            this.panel3.TabIndex = 24;
            // 
            // FrmPrincipal
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ClientSize = new System.Drawing.Size(1031, 857);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.groupBox7);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.groupBox4);
            this.ForeColor = System.Drawing.Color.White;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "FrmPrincipal";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Face Recoginizer";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmPrincipal_FormClosing);
            this.Load += new System.EventHandler(this.FrmPrincipal_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel_btn.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.imageBox1)).EndInit();
            this.groupBox9.ResumeLayout(false);
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            this.capture_groupBox.ResumeLayout(false);
            this.capture_groupBox.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.enable_groupBox.ResumeLayout(false);
            this.enable_groupBox.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.imageBoxFrameGrabber)).EndInit();
            this.groupBox7.ResumeLayout(false);
            this.groupBox7.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button enable_btn;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.GroupBox groupBox4;
        private Emgu.CV.UI.ImageBox imageBoxFrameGrabber;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.ComboBox names_cmbx;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox3;
        private Emgu.CV.UI.ImageBox imageBox1;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.GroupBox enable_groupBox;
        private System.Windows.Forms.Button capture_btn;
        private System.Windows.Forms.GroupBox capture_groupBox;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox7;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Panel panel_btn;
        private System.Windows.Forms.Button face_enroll_admin;
        private System.Windows.Forms.ComboBox video_src_cmbx;
        private System.Windows.Forms.ComboBox num_of_device_cmbx;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox image_name_txbx;
        private System.Windows.Forms.GroupBox groupBox8;
        private System.Windows.Forms.GroupBox groupBox9;
    }
}

