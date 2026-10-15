namespace GroceryManagement
{
    partial class RegistrationForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RegistrationForm));
            this.Login = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.btRegister = new Guna.UI2.WinForms.Guna2GradientButton();
            this.guna2DragControl1 = new Guna.UI2.WinForms.Guna2DragControl(this.components);
            this.guna2Button1 = new Guna.UI2.WinForms.Guna2Button();
            this.guna2CirclePictureBox1 = new Guna.UI2.WinForms.Guna2CirclePictureBox();
            this.register_Password = new Guna.UI2.WinForms.Guna2TextBox();
            this.register_UserName = new Guna.UI2.WinForms.Guna2TextBox();
            this.register_FullName = new Guna.UI2.WinForms.Guna2TextBox();
            this.guna2CustomGradientPanel1 = new Guna.UI2.WinForms.Guna2CustomGradientPanel();
            this.register_Address = new Guna.UI2.WinForms.Guna2TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.guna2CirclePictureBox1)).BeginInit();
            this.guna2CustomGradientPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // Login
            // 
            this.Login.AutoSize = false;
            this.Login.BackColor = System.Drawing.Color.Transparent;
            this.Login.Font = new System.Drawing.Font("Segoe UI", 22.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Login.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.Login.Location = new System.Drawing.Point(213, 174);
            this.Login.Name = "Login";
            this.Login.Size = new System.Drawing.Size(237, 69);
            this.Login.TabIndex = 11;
            this.Login.Text = "Register";
            // 
            // btRegister
            // 
            this.btRegister.Animated = true;
            this.btRegister.AutoRoundedCorners = true;
            this.btRegister.BackColor = System.Drawing.Color.Transparent;
            this.btRegister.BorderColor = System.Drawing.Color.Transparent;
            this.btRegister.BorderRadius = 21;
            this.btRegister.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btRegister.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btRegister.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btRegister.DisabledState.FillColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btRegister.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btRegister.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(101)))), ((int)(((byte)(171)))), ((int)(((byte)(59)))));
            this.btRegister.FillColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(94)))), ((int)(((byte)(32)))));
            this.btRegister.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btRegister.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.btRegister.Location = new System.Drawing.Point(121, 573);
            this.btRegister.Name = "btRegister";
            this.btRegister.Size = new System.Drawing.Size(329, 45);
            this.btRegister.TabIndex = 6;
            this.btRegister.Text = "Create Account";
            this.btRegister.UseTransparentBackground = true;
            this.btRegister.Click += new System.EventHandler(this.btRegister_Click);
            // 
            // guna2DragControl1
            // 
            this.guna2DragControl1.DockIndicatorTransparencyValue = 0.6D;
            this.guna2DragControl1.TargetControl = this;
            this.guna2DragControl1.UseTransparentDrag = true;
            // 
            // guna2Button1
            // 
            this.guna2Button1.Animated = true;
            this.guna2Button1.AutoRoundedCorners = true;
            this.guna2Button1.BackColor = System.Drawing.Color.Transparent;
            this.guna2Button1.BorderColor = System.Drawing.Color.White;
            this.guna2Button1.BorderRadius = 21;
            this.guna2Button1.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.guna2Button1.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.guna2Button1.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.guna2Button1.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.guna2Button1.FillColor = System.Drawing.Color.Transparent;
            this.guna2Button1.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold);
            this.guna2Button1.ForeColor = System.Drawing.Color.White;
            this.guna2Button1.ImageAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.guna2Button1.ImageSize = new System.Drawing.Size(50, 50);
            this.guna2Button1.Location = new System.Drawing.Point(121, 642);
            this.guna2Button1.Name = "guna2Button1";
            this.guna2Button1.Size = new System.Drawing.Size(329, 45);
            this.guna2Button1.TabIndex = 16;
            this.guna2Button1.Text = "Back To Login Form";
            this.guna2Button1.UseTransparentBackground = true;
            this.guna2Button1.Click += new System.EventHandler(this.guna2Button1_Click);
            // 
            // guna2CirclePictureBox1
            // 
            this.guna2CirclePictureBox1.BackColor = System.Drawing.Color.Transparent;
            this.guna2CirclePictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("guna2CirclePictureBox1.Image")));
            this.guna2CirclePictureBox1.ImageRotate = 0F;
            this.guna2CirclePictureBox1.Location = new System.Drawing.Point(213, 3);
            this.guna2CirclePictureBox1.Name = "guna2CirclePictureBox1";
            this.guna2CirclePictureBox1.ShadowDecoration.Mode = Guna.UI2.WinForms.Enums.ShadowMode.Circle;
            this.guna2CirclePictureBox1.Size = new System.Drawing.Size(160, 165);
            this.guna2CirclePictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.guna2CirclePictureBox1.TabIndex = 15;
            this.guna2CirclePictureBox1.TabStop = false;
            this.guna2CirclePictureBox1.UseTransparentBackground = true;
            // 
            // register_Password
            // 
            this.register_Password.Animated = true;
            this.register_Password.BorderRadius = 8;
            this.register_Password.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.register_Password.DefaultText = "";
            this.register_Password.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.register_Password.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.register_Password.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.register_Password.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.register_Password.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(229)))), ((int)(((byte)(229)))), ((int)(((byte)(229)))));
            this.register_Password.FocusedState.BorderColor = System.Drawing.Color.Cyan;
            this.register_Password.FocusedState.PlaceholderForeColor = System.Drawing.Color.Black;
            this.register_Password.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.register_Password.HoverState.BorderColor = System.Drawing.Color.Red;
            this.register_Password.IconLeft = ((System.Drawing.Image)(resources.GetObject("register_Password.IconLeft")));
            this.register_Password.Location = new System.Drawing.Point(121, 410);
            this.register_Password.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.register_Password.Name = "register_Password";
            this.register_Password.PasswordChar = '\0';
            this.register_Password.PlaceholderForeColor = System.Drawing.Color.DarkGray;
            this.register_Password.PlaceholderText = "Password";
            this.register_Password.SelectedText = "";
            this.register_Password.ShadowDecoration.Color = System.Drawing.Color.White;
            this.register_Password.Size = new System.Drawing.Size(329, 48);
            this.register_Password.TabIndex = 4;
            // 
            // register_UserName
            // 
            this.register_UserName.Animated = true;
            this.register_UserName.BorderRadius = 8;
            this.register_UserName.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.register_UserName.DefaultText = "";
            this.register_UserName.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.register_UserName.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.register_UserName.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.register_UserName.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.register_UserName.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(229)))), ((int)(((byte)(229)))), ((int)(((byte)(229)))));
            this.register_UserName.FocusedState.BorderColor = System.Drawing.Color.Cyan;
            this.register_UserName.FocusedState.PlaceholderForeColor = System.Drawing.Color.Black;
            this.register_UserName.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.register_UserName.HoverState.BorderColor = System.Drawing.Color.Red;
            this.register_UserName.IconLeft = ((System.Drawing.Image)(resources.GetObject("register_UserName.IconLeft")));
            this.register_UserName.Location = new System.Drawing.Point(121, 327);
            this.register_UserName.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.register_UserName.Name = "register_UserName";
            this.register_UserName.PasswordChar = '\0';
            this.register_UserName.PlaceholderForeColor = System.Drawing.Color.DarkGray;
            this.register_UserName.PlaceholderText = "UserName";
            this.register_UserName.SelectedText = "";
            this.register_UserName.ShadowDecoration.Color = System.Drawing.Color.White;
            this.register_UserName.Size = new System.Drawing.Size(329, 48);
            this.register_UserName.TabIndex = 3;
            // 
            // register_FullName
            // 
            this.register_FullName.Animated = true;
            this.register_FullName.BorderRadius = 8;
            this.register_FullName.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.register_FullName.DefaultText = "";
            this.register_FullName.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.register_FullName.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.register_FullName.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.register_FullName.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.register_FullName.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(229)))), ((int)(((byte)(229)))), ((int)(((byte)(229)))));
            this.register_FullName.FocusedState.BorderColor = System.Drawing.Color.Cyan;
            this.register_FullName.FocusedState.PlaceholderForeColor = System.Drawing.Color.Black;
            this.register_FullName.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.register_FullName.HoverState.BorderColor = System.Drawing.Color.Red;
            this.register_FullName.IconLeft = ((System.Drawing.Image)(resources.GetObject("register_FullName.IconLeft")));
            this.register_FullName.Location = new System.Drawing.Point(121, 250);
            this.register_FullName.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.register_FullName.Name = "register_FullName";
            this.register_FullName.PasswordChar = '\0';
            this.register_FullName.PlaceholderForeColor = System.Drawing.Color.DarkGray;
            this.register_FullName.PlaceholderText = "FullName";
            this.register_FullName.SelectedText = "";
            this.register_FullName.ShadowDecoration.Color = System.Drawing.Color.White;
            this.register_FullName.Size = new System.Drawing.Size(329, 48);
            this.register_FullName.TabIndex = 2;
            // 
            // guna2CustomGradientPanel1
            // 
            this.guna2CustomGradientPanel1.BackColor = System.Drawing.Color.Transparent;
            this.guna2CustomGradientPanel1.Controls.Add(this.register_Address);
            this.guna2CustomGradientPanel1.Controls.Add(this.guna2CirclePictureBox1);
            this.guna2CustomGradientPanel1.Controls.Add(this.guna2Button1);
            this.guna2CustomGradientPanel1.Controls.Add(this.register_FullName);
            this.guna2CustomGradientPanel1.Controls.Add(this.register_UserName);
            this.guna2CustomGradientPanel1.Controls.Add(this.Login);
            this.guna2CustomGradientPanel1.Controls.Add(this.register_Password);
            this.guna2CustomGradientPanel1.Controls.Add(this.btRegister);
            this.guna2CustomGradientPanel1.FillColor = System.Drawing.Color.Turquoise;
            this.guna2CustomGradientPanel1.FillColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(101)))), ((int)(((byte)(171)))), ((int)(((byte)(59)))));
            this.guna2CustomGradientPanel1.FillColor3 = System.Drawing.Color.FromArgb(((int)(((byte)(178)))), ((int)(((byte)(115)))), ((int)(((byte)(60)))));
            this.guna2CustomGradientPanel1.FillColor4 = System.Drawing.Color.Transparent;
            this.guna2CustomGradientPanel1.Location = new System.Drawing.Point(60, 99);
            this.guna2CustomGradientPanel1.Name = "guna2CustomGradientPanel1";
            this.guna2CustomGradientPanel1.Size = new System.Drawing.Size(576, 690);
            this.guna2CustomGradientPanel1.TabIndex = 17;
            // 
            // register_Address
            // 
            this.register_Address.Animated = true;
            this.register_Address.BorderRadius = 8;
            this.register_Address.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.register_Address.DefaultText = "";
            this.register_Address.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.register_Address.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.register_Address.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.register_Address.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.register_Address.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(229)))), ((int)(((byte)(229)))), ((int)(((byte)(229)))));
            this.register_Address.FocusedState.BorderColor = System.Drawing.Color.Cyan;
            this.register_Address.FocusedState.PlaceholderForeColor = System.Drawing.Color.Black;
            this.register_Address.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.register_Address.HoverState.BorderColor = System.Drawing.Color.Red;
            this.register_Address.IconLeft = ((System.Drawing.Image)(resources.GetObject("register_Address.IconLeft")));
            this.register_Address.Location = new System.Drawing.Point(121, 486);
            this.register_Address.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.register_Address.Name = "register_Address";
            this.register_Address.PasswordChar = '\0';
            this.register_Address.PlaceholderForeColor = System.Drawing.Color.DarkGray;
            this.register_Address.PlaceholderText = "Address";
            this.register_Address.SelectedText = "";
            this.register_Address.ShadowDecoration.Color = System.Drawing.Color.White;
            this.register_Address.Size = new System.Drawing.Size(329, 48);
            this.register_Address.TabIndex = 17;
            // 
            // RegistrationForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackColor = System.Drawing.Color.White;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.ClientSize = new System.Drawing.Size(1628, 886);
            this.Controls.Add(this.guna2CustomGradientPanel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "RegistrationForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Registration";
            ((System.ComponentModel.ISupportInitialize)(this.guna2CirclePictureBox1)).EndInit();
            this.guna2CustomGradientPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Guna.UI2.WinForms.Guna2CirclePictureBox guna2CirclePictureBox1;
        private Guna.UI2.WinForms.Guna2HtmlLabel Login;
        private Guna.UI2.WinForms.Guna2GradientButton btRegister;
        private Guna.UI2.WinForms.Guna2TextBox register_Password;
        private Guna.UI2.WinForms.Guna2TextBox register_UserName;
        private Guna.UI2.WinForms.Guna2TextBox register_FullName;
        private Guna.UI2.WinForms.Guna2DragControl guna2DragControl1;
        private Guna.UI2.WinForms.Guna2Button guna2Button1;
        private Guna.UI2.WinForms.Guna2CustomGradientPanel guna2CustomGradientPanel1;
        private Guna.UI2.WinForms.Guna2TextBox register_Address;
    }
}