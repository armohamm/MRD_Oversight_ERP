<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class LicenciaValida
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(LicenciaValida))
        Me.gbPreciosHistoricosTarjeta = New System.Windows.Forms.GroupBox
        Me.Label4 = New System.Windows.Forms.Label
        Me.Label3 = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.Label1 = New System.Windows.Forms.Label
        Me.lblInfo = New System.Windows.Forms.Label
        Me.btnAcepto = New System.Windows.Forms.Button
        Me.NotifyIcon1 = New System.Windows.Forms.NotifyIcon(Me.components)
        Me.gbPreciosHistoricosTarjeta.SuspendLayout()
        Me.SuspendLayout()
        '
        'gbPreciosHistoricosTarjeta
        '
        Me.gbPreciosHistoricosTarjeta.Controls.Add(Me.Label4)
        Me.gbPreciosHistoricosTarjeta.Controls.Add(Me.Label3)
        Me.gbPreciosHistoricosTarjeta.Controls.Add(Me.Label2)
        Me.gbPreciosHistoricosTarjeta.Controls.Add(Me.Label1)
        Me.gbPreciosHistoricosTarjeta.Controls.Add(Me.lblInfo)
        Me.gbPreciosHistoricosTarjeta.Controls.Add(Me.btnAcepto)
        Me.gbPreciosHistoricosTarjeta.Location = New System.Drawing.Point(4, 0)
        Me.gbPreciosHistoricosTarjeta.Name = "gbPreciosHistoricosTarjeta"
        Me.gbPreciosHistoricosTarjeta.Size = New System.Drawing.Size(613, 188)
        Me.gbPreciosHistoricosTarjeta.TabIndex = 57
        Me.gbPreciosHistoricosTarjeta.TabStop = False
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(6, 60)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(418, 13)
        Me.Label4.TabIndex = 64
        Me.Label4.Text = "Ninguna información, salvo datos de contacto, será enviada a Oversight Technologi" & _
            "es."
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(22, 139)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(427, 13)
        Me.Label3.TabIndex = 63
        Me.Label3.Text = "* Si no se activa en los siguientes 30 días, el producto regresará a funcionalida" & _
            "d Limitada"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(8, 99)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(120, 13)
        Me.Label2.TabIndex = 62
        Me.Label2.Text = "Bienvenido a Oversight!"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(6, 38)
        Me.Label1.MaximumSize = New System.Drawing.Size(650, 0)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(595, 13)
        Me.Label1.TabIndex = 61
        Me.Label1.Text = "Su producto será registrado para su activación con Oversight Technologies en cuan" & _
            "to se detecte una conexión a Internet.  *"
        '
        'lblInfo
        '
        Me.lblInfo.AutoSize = True
        Me.lblInfo.Location = New System.Drawing.Point(6, 16)
        Me.lblInfo.MaximumSize = New System.Drawing.Size(650, 0)
        Me.lblInfo.Name = "lblInfo"
        Me.lblInfo.Size = New System.Drawing.Size(474, 13)
        Me.lblInfo.TabIndex = 60
        Me.lblInfo.Text = "Felicidades! Se ha decidido por Oversight, el mejor programa para supervisión de " & _
            "obras en México. "
        '
        'btnAcepto
        '
        Me.btnAcepto.Image = Global.Oversight.My.Resources.Resources.next24x24
        Me.btnAcepto.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnAcepto.Location = New System.Drawing.Point(518, 143)
        Me.btnAcepto.Name = "btnAcepto"
        Me.btnAcepto.Size = New System.Drawing.Size(89, 34)
        Me.btnAcepto.TabIndex = 58
        Me.btnAcepto.Text = "&Continuar"
        Me.btnAcepto.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnAcepto.UseVisualStyleBackColor = True
        '
        'NotifyIcon1
        '
        Me.NotifyIcon1.Icon = CType(resources.GetObject("NotifyIcon1.Icon"), System.Drawing.Icon)
        Me.NotifyIcon1.Text = "Oversight"
        '
        'LicenciaValida
        '
        Me.AcceptButton = Me.btnAcepto
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(625, 194)
        Me.ControlBox = False
        Me.Controls.Add(Me.gbPreciosHistoricosTarjeta)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "LicenciaValida"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Felicidades!"
        Me.gbPreciosHistoricosTarjeta.ResumeLayout(False)
        Me.gbPreciosHistoricosTarjeta.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents gbPreciosHistoricosTarjeta As System.Windows.Forms.GroupBox
    Friend WithEvents btnAcepto As System.Windows.Forms.Button
    Friend WithEvents lblInfo As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents NotifyIcon1 As System.Windows.Forms.NotifyIcon
End Class
