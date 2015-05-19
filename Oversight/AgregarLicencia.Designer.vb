<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class AgregarLicencia
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(AgregarLicencia))
        Me.gbPreciosHistoricosTarjeta = New System.Windows.Forms.GroupBox
        Me.btnIngresarRegistro = New System.Windows.Forms.Button
        Me.lblInfo = New System.Windows.Forms.Label
        Me.btnComprarLicencia = New System.Windows.Forms.Button
        Me.btnSeguirEvaluando = New System.Windows.Forms.Button
        Me.licenseFileDialog = New System.Windows.Forms.OpenFileDialog
        Me.tmrBtnSeguirEvaluando = New System.Windows.Forms.Timer(Me.components)
        Me.NotifyIcon1 = New System.Windows.Forms.NotifyIcon(Me.components)
        Me.gbPreciosHistoricosTarjeta.SuspendLayout()
        Me.SuspendLayout()
        '
        'gbPreciosHistoricosTarjeta
        '
        Me.gbPreciosHistoricosTarjeta.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.gbPreciosHistoricosTarjeta.Controls.Add(Me.btnIngresarRegistro)
        Me.gbPreciosHistoricosTarjeta.Controls.Add(Me.lblInfo)
        Me.gbPreciosHistoricosTarjeta.Controls.Add(Me.btnComprarLicencia)
        Me.gbPreciosHistoricosTarjeta.Controls.Add(Me.btnSeguirEvaluando)
        Me.gbPreciosHistoricosTarjeta.Location = New System.Drawing.Point(4, 0)
        Me.gbPreciosHistoricosTarjeta.Name = "gbPreciosHistoricosTarjeta"
        Me.gbPreciosHistoricosTarjeta.Size = New System.Drawing.Size(662, 147)
        Me.gbPreciosHistoricosTarjeta.TabIndex = 57
        Me.gbPreciosHistoricosTarjeta.TabStop = False
        '
        'btnIngresarRegistro
        '
        Me.btnIngresarRegistro.Image = Global.Oversight.My.Resources.Resources.licence12x12
        Me.btnIngresarRegistro.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnIngresarRegistro.Location = New System.Drawing.Point(6, 60)
        Me.btnIngresarRegistro.Name = "btnIngresarRegistro"
        Me.btnIngresarRegistro.Size = New System.Drawing.Size(125, 23)
        Me.btnIngresarRegistro.TabIndex = 62
        Me.btnIngresarRegistro.Text = "&Ingresar Registro"
        Me.btnIngresarRegistro.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnIngresarRegistro.UseVisualStyleBackColor = True
        '
        'lblInfo
        '
        Me.lblInfo.AutoSize = True
        Me.lblInfo.Location = New System.Drawing.Point(7, 17)
        Me.lblInfo.MaximumSize = New System.Drawing.Size(650, 0)
        Me.lblInfo.Name = "lblInfo"
        Me.lblInfo.Size = New System.Drawing.Size(632, 26)
        Me.lblInfo.TabIndex = 60
        Me.lblInfo.Text = resources.GetString("lblInfo.Text")
        '
        'btnComprarLicencia
        '
        Me.btnComprarLicencia.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnComprarLicencia.Image = Global.Oversight.My.Resources.Resources.licence24x24
        Me.btnComprarLicencia.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnComprarLicencia.Location = New System.Drawing.Point(377, 100)
        Me.btnComprarLicencia.Name = "btnComprarLicencia"
        Me.btnComprarLicencia.Size = New System.Drawing.Size(279, 34)
        Me.btnComprarLicencia.TabIndex = 58
        Me.btnComprarLicencia.Text = "Comprar &Licencia (requiere acceso a Internet)"
        Me.btnComprarLicencia.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnComprarLicencia.UseVisualStyleBackColor = True
        '
        'btnSeguirEvaluando
        '
        Me.btnSeguirEvaluando.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnSeguirEvaluando.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnSeguirEvaluando.Image = Global.Oversight.My.Resources.Resources.cancel24x24
        Me.btnSeguirEvaluando.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnSeguirEvaluando.Location = New System.Drawing.Point(6, 100)
        Me.btnSeguirEvaluando.Name = "btnSeguirEvaluando"
        Me.btnSeguirEvaluando.Size = New System.Drawing.Size(352, 34)
        Me.btnSeguirEvaluando.TabIndex = 59
        Me.btnSeguirEvaluando.Text = "Seguir Evaluando Oversight (&Cancela la operación de Guardado)"
        Me.btnSeguirEvaluando.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnSeguirEvaluando.UseVisualStyleBackColor = True
        '
        'licenseFileDialog
        '
        Me.licenseFileDialog.DefaultExt = "*.lic"
        Me.licenseFileDialog.FileName = "*.lic"
        Me.licenseFileDialog.Filter = """Archivos de Licencia|*.lic|Todos los archivos|*.*"""
        Me.licenseFileDialog.InitialDirectory = "%USERPROFILE%"
        '
        'tmrBtnSeguirEvaluando
        '
        Me.tmrBtnSeguirEvaluando.Interval = 1000
        '
        'NotifyIcon1
        '
        Me.NotifyIcon1.Icon = CType(resources.GetObject("NotifyIcon1.Icon"), System.Drawing.Icon)
        Me.NotifyIcon1.Text = "Oversight"
        '
        'AgregarLicencia
        '
        Me.AcceptButton = Me.btnComprarLicencia
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.btnSeguirEvaluando
        Me.ClientSize = New System.Drawing.Size(672, 151)
        Me.Controls.Add(Me.gbPreciosHistoricosTarjeta)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "AgregarLicencia"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Suscripción Inválida!"
        Me.gbPreciosHistoricosTarjeta.ResumeLayout(False)
        Me.gbPreciosHistoricosTarjeta.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents gbPreciosHistoricosTarjeta As System.Windows.Forms.GroupBox
    Friend WithEvents btnComprarLicencia As System.Windows.Forms.Button
    Friend WithEvents btnSeguirEvaluando As System.Windows.Forms.Button
    Friend WithEvents lblInfo As System.Windows.Forms.Label
    Friend WithEvents btnIngresarRegistro As System.Windows.Forms.Button
    Friend WithEvents licenseFileDialog As System.Windows.Forms.OpenFileDialog
    Friend WithEvents tmrBtnSeguirEvaluando As System.Windows.Forms.Timer
    Friend WithEvents NotifyIcon1 As System.Windows.Forms.NotifyIcon
End Class
