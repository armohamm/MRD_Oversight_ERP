<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class PreguntaRescate
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(PreguntaRescate))
        Me.tlpLogin = New System.Windows.Forms.TableLayoutPanel
        Me.btnLogin = New System.Windows.Forms.Button
        Me.btnCancel = New System.Windows.Forms.Button
        Me.lblRespuesta = New System.Windows.Forms.Label
        Me.txtRespuesta = New System.Windows.Forms.TextBox
        Me.lblPreguntalbl = New System.Windows.Forms.Label
        Me.lblBienvenida = New System.Windows.Forms.Label
        Me.NotifyIcon1 = New System.Windows.Forms.NotifyIcon(Me.components)
        Me.lblPregunta = New System.Windows.Forms.Label
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
        Me.btnLogin.Text = "&Verificar"
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
        'lblRespuesta
        '
        Me.lblRespuesta.AutoSize = True
        Me.lblRespuesta.Location = New System.Drawing.Point(12, 111)
        Me.lblRespuesta.Name = "lblRespuesta"
        Me.lblRespuesta.Size = New System.Drawing.Size(61, 13)
        Me.lblRespuesta.TabIndex = 9
        Me.lblRespuesta.Text = "Respuesta:"
        '
        'txtRespuesta
        '
        Me.txtRespuesta.Location = New System.Drawing.Point(15, 127)
        Me.txtRespuesta.Name = "txtRespuesta"
        Me.txtRespuesta.PasswordChar = Global.Microsoft.VisualBasic.ChrW(42)
        Me.txtRespuesta.Size = New System.Drawing.Size(236, 20)
        Me.txtRespuesta.TabIndex = 2
        Me.txtRespuesta.UseSystemPasswordChar = True
        '
        'lblPreguntalbl
        '
        Me.lblPreguntalbl.AutoSize = True
        Me.lblPreguntalbl.Location = New System.Drawing.Point(12, 57)
        Me.lblPreguntalbl.Name = "lblPreguntalbl"
        Me.lblPreguntalbl.Size = New System.Drawing.Size(53, 13)
        Me.lblPreguntalbl.TabIndex = 7
        Me.lblPreguntalbl.Text = "Pregunta:"
        '
        'lblBienvenida
        '
        Me.lblBienvenida.AutoSize = True
        Me.lblBienvenida.Location = New System.Drawing.Point(12, 19)
        Me.lblBienvenida.Name = "lblBienvenida"
        Me.lblBienvenida.Size = New System.Drawing.Size(218, 13)
        Me.lblBienvenida.TabIndex = 5
        Me.lblBienvenida.Text = "Escribe la respuesta correcta de la pregunta:"
        '
        'NotifyIcon1
        '
        Me.NotifyIcon1.Icon = CType(resources.GetObject("NotifyIcon1.Icon"), System.Drawing.Icon)
        Me.NotifyIcon1.Text = "Oversight"
        '
        'lblPregunta
        '
        Me.lblPregunta.AutoSize = True
        Me.lblPregunta.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPregunta.ForeColor = System.Drawing.Color.Red
        Me.lblPregunta.Location = New System.Drawing.Point(12, 75)
        Me.lblPregunta.MaximumSize = New System.Drawing.Size(238, 0)
        Me.lblPregunta.Name = "lblPregunta"
        Me.lblPregunta.Size = New System.Drawing.Size(238, 13)
        Me.lblPregunta.TabIndex = 10
        Me.lblPregunta.Text = "AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA"
        '
        'PreguntaRescate
        '
        Me.AcceptButton = Me.btnLogin
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.btnCancel
        Me.ClientSize = New System.Drawing.Size(268, 231)
        Me.Controls.Add(Me.lblPregunta)
        Me.Controls.Add(Me.lblRespuesta)
        Me.Controls.Add(Me.txtRespuesta)
        Me.Controls.Add(Me.lblPreguntalbl)
        Me.Controls.Add(Me.lblBienvenida)
        Me.Controls.Add(Me.tlpLogin)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "PreguntaRescate"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Pregunta de Rescate"
        Me.tlpLogin.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents tlpLogin As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents btnLogin As System.Windows.Forms.Button
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents lblRespuesta As System.Windows.Forms.Label
    Friend WithEvents txtRespuesta As System.Windows.Forms.TextBox
    Friend WithEvents lblPreguntalbl As System.Windows.Forms.Label
    Friend WithEvents lblBienvenida As System.Windows.Forms.Label
    Friend WithEvents NotifyIcon1 As System.Windows.Forms.NotifyIcon
    Friend WithEvents lblPregunta As System.Windows.Forms.Label

End Class
