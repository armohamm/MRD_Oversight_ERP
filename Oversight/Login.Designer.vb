<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Login
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Login))
        Me.tlpLogin = New System.Windows.Forms.TableLayoutPanel
        Me.btnLogin = New System.Windows.Forms.Button
        Me.btnCancel = New System.Windows.Forms.Button
        Me.lblPassword = New System.Windows.Forms.Label
        Me.txtPassword = New System.Windows.Forms.TextBox
        Me.lblUsuario = New System.Windows.Forms.Label
        Me.txtUsername = New System.Windows.Forms.TextBox
        Me.lblBienvenida = New System.Windows.Forms.Label
        Me.NotifyIcon1 = New System.Windows.Forms.NotifyIcon(Me.components)
        Me.lnkForgotPassword = New System.Windows.Forms.LinkLabel
        Me.tlpLogin.SuspendLayout()
        Me.SuspendLayout()
        '
        'tlpLogin
        '
        Me.tlpLogin.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tlpLogin.ColumnCount = 2
        Me.tlpLogin.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.tlpLogin.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.tlpLogin.Controls.Add(Me.btnLogin, 0, 0)
        Me.tlpLogin.Controls.Add(Me.btnCancel, 1, 0)
        Me.tlpLogin.Location = New System.Drawing.Point(110, 190)
        Me.tlpLogin.Name = "tlpLogin"
        Me.tlpLogin.RowCount = 1
        Me.tlpLogin.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.tlpLogin.Size = New System.Drawing.Size(146, 29)
        Me.tlpLogin.TabIndex = 0
        '
        'btnLogin
        '
        Me.btnLogin.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.btnLogin.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.btnLogin.Location = New System.Drawing.Point(3, 3)
        Me.btnLogin.Name = "btnLogin"
        Me.btnLogin.Size = New System.Drawing.Size(67, 23)
        Me.btnLogin.TabIndex = 3
        Me.btnLogin.Text = "&Login"
        '
        'btnCancel
        '
        Me.btnCancel.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnCancel.Location = New System.Drawing.Point(76, 3)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(67, 23)
        Me.btnCancel.TabIndex = 4
        Me.btnCancel.Text = "&Cancelar"
        '
        'lblPassword
        '
        Me.lblPassword.AutoSize = True
        Me.lblPassword.Location = New System.Drawing.Point(12, 111)
        Me.lblPassword.Name = "lblPassword"
        Me.lblPassword.Size = New System.Drawing.Size(64, 13)
        Me.lblPassword.TabIndex = 9
        Me.lblPassword.Text = "Contraseña:"
        '
        'txtPassword
        '
        Me.txtPassword.Location = New System.Drawing.Point(15, 127)
        Me.txtPassword.Name = "txtPassword"
        Me.txtPassword.PasswordChar = Global.Microsoft.VisualBasic.ChrW(42)
        Me.txtPassword.Size = New System.Drawing.Size(236, 20)
        Me.txtPassword.TabIndex = 2
        Me.txtPassword.UseSystemPasswordChar = True
        '
        'lblUsuario
        '
        Me.lblUsuario.AutoSize = True
        Me.lblUsuario.Location = New System.Drawing.Point(12, 57)
        Me.lblUsuario.Name = "lblUsuario"
        Me.lblUsuario.Size = New System.Drawing.Size(46, 13)
        Me.lblUsuario.TabIndex = 7
        Me.lblUsuario.Text = "Usuario:"
        '
        'txtUsername
        '
        Me.txtUsername.CharacterCasing = System.Windows.Forms.CharacterCasing.Lower
        Me.txtUsername.Location = New System.Drawing.Point(15, 73)
        Me.txtUsername.MaxLength = 15
        Me.txtUsername.Name = "txtUsername"
        Me.txtUsername.Size = New System.Drawing.Size(236, 20)
        Me.txtUsername.TabIndex = 1
        '
        'lblBienvenida
        '
        Me.lblBienvenida.AutoSize = True
        Me.lblBienvenida.Location = New System.Drawing.Point(12, 19)
        Me.lblBienvenida.Name = "lblBienvenida"
        Me.lblBienvenida.Size = New System.Drawing.Size(239, 13)
        Me.lblBienvenida.TabIndex = 5
        Me.lblBienvenida.Text = "Escribe tu usuario y contraseña para identificarte:"
        '
        'NotifyIcon1
        '
        Me.NotifyIcon1.Icon = CType(resources.GetObject("NotifyIcon1.Icon"), System.Drawing.Icon)
        Me.NotifyIcon1.Text = "Oversight"
        '
        'lnkForgotPassword
        '
        Me.lnkForgotPassword.AutoSize = True
        Me.lnkForgotPassword.Location = New System.Drawing.Point(108, 150)
        Me.lnkForgotPassword.Name = "lnkForgotPassword"
        Me.lnkForgotPassword.Size = New System.Drawing.Size(143, 13)
        Me.lnkForgotPassword.TabIndex = 10
        Me.lnkForgotPassword.TabStop = True
        Me.lnkForgotPassword.Text = "¿Se te olvidó tu contraseña?"
        '
        'Login
        '
        Me.AcceptButton = Me.btnLogin
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.btnCancel
        Me.ClientSize = New System.Drawing.Size(268, 231)
        Me.Controls.Add(Me.lnkForgotPassword)
        Me.Controls.Add(Me.lblPassword)
        Me.Controls.Add(Me.txtPassword)
        Me.Controls.Add(Me.lblUsuario)
        Me.Controls.Add(Me.txtUsername)
        Me.Controls.Add(Me.lblBienvenida)
        Me.Controls.Add(Me.tlpLogin)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "Login"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Login - Oversight"
        Me.tlpLogin.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents tlpLogin As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents btnLogin As System.Windows.Forms.Button
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents lblPassword As System.Windows.Forms.Label
    Friend WithEvents txtPassword As System.Windows.Forms.TextBox
    Friend WithEvents lblUsuario As System.Windows.Forms.Label
    Friend WithEvents txtUsername As System.Windows.Forms.TextBox
    Friend WithEvents lblBienvenida As System.Windows.Forms.Label
    Friend WithEvents NotifyIcon1 As System.Windows.Forms.NotifyIcon
    Friend WithEvents lnkForgotPassword As System.Windows.Forms.LinkLabel

End Class
