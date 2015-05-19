<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Opciones
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Opciones))
        Me.gbCategoria = New System.Windows.Forms.GroupBox
        Me.btnAbrirCarpetaRutaActualizaciones = New System.Windows.Forms.Button
        Me.lblRutaActualizaciones = New System.Windows.Forms.Label
        Me.txtRutaActualizaciones = New System.Windows.Forms.TextBox
        Me.btnAbrirCarpetaRutaArchivos = New System.Windows.Forms.Button
        Me.lblRutaArchivos = New System.Windows.Forms.Label
        Me.txtRutaArchivos = New System.Windows.Forms.TextBox
        Me.lblError = New System.Windows.Forms.Label
        Me.lblIP = New System.Windows.Forms.Label
        Me.txtIP = New System.Windows.Forms.TextBox
        Me.btnGuardar = New System.Windows.Forms.Button
        Me.btnCancelar = New System.Windows.Forms.Button
        Me.fbd = New System.Windows.Forms.FolderBrowserDialog
        Me.NotifyIcon1 = New System.Windows.Forms.NotifyIcon(Me.components)
        Me.gbCategoria.SuspendLayout()
        Me.SuspendLayout()
        '
        'gbCategoria
        '
        Me.gbCategoria.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.gbCategoria.Controls.Add(Me.btnAbrirCarpetaRutaActualizaciones)
        Me.gbCategoria.Controls.Add(Me.lblRutaActualizaciones)
        Me.gbCategoria.Controls.Add(Me.txtRutaActualizaciones)
        Me.gbCategoria.Controls.Add(Me.btnAbrirCarpetaRutaArchivos)
        Me.gbCategoria.Controls.Add(Me.lblRutaArchivos)
        Me.gbCategoria.Controls.Add(Me.txtRutaArchivos)
        Me.gbCategoria.Controls.Add(Me.lblError)
        Me.gbCategoria.Controls.Add(Me.lblIP)
        Me.gbCategoria.Controls.Add(Me.txtIP)
        Me.gbCategoria.Controls.Add(Me.btnGuardar)
        Me.gbCategoria.Controls.Add(Me.btnCancelar)
        Me.gbCategoria.Location = New System.Drawing.Point(3, -2)
        Me.gbCategoria.Name = "gbCategoria"
        Me.gbCategoria.Size = New System.Drawing.Size(415, 180)
        Me.gbCategoria.TabIndex = 57
        Me.gbCategoria.TabStop = False
        '
        'btnAbrirCarpetaRutaActualizaciones
        '
        Me.btnAbrirCarpetaRutaActualizaciones.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnAbrirCarpetaRutaActualizaciones.Image = Global.Oversight.My.Resources.Resources.open12x12
        Me.btnAbrirCarpetaRutaActualizaciones.Location = New System.Drawing.Point(352, 86)
        Me.btnAbrirCarpetaRutaActualizaciones.Name = "btnAbrirCarpetaRutaActualizaciones"
        Me.btnAbrirCarpetaRutaActualizaciones.Size = New System.Drawing.Size(27, 23)
        Me.btnAbrirCarpetaRutaActualizaciones.TabIndex = 77
        Me.btnAbrirCarpetaRutaActualizaciones.UseVisualStyleBackColor = True
        '
        'lblRutaActualizaciones
        '
        Me.lblRutaActualizaciones.AutoSize = True
        Me.lblRutaActualizaciones.Location = New System.Drawing.Point(8, 91)
        Me.lblRutaActualizaciones.MaximumSize = New System.Drawing.Size(204, 0)
        Me.lblRutaActualizaciones.Name = "lblRutaActualizaciones"
        Me.lblRutaActualizaciones.Size = New System.Drawing.Size(187, 13)
        Me.lblRutaActualizaciones.TabIndex = 76
        Me.lblRutaActualizaciones.Text = "Ruta para Actualizaciones de Version:"
        '
        'txtRutaActualizaciones
        '
        Me.txtRutaActualizaciones.Enabled = False
        Me.txtRutaActualizaciones.Location = New System.Drawing.Point(210, 88)
        Me.txtRutaActualizaciones.MaxLength = 1000
        Me.txtRutaActualizaciones.Name = "txtRutaActualizaciones"
        Me.txtRutaActualizaciones.Size = New System.Drawing.Size(136, 20)
        Me.txtRutaActualizaciones.TabIndex = 75
        '
        'btnAbrirCarpetaRutaArchivos
        '
        Me.btnAbrirCarpetaRutaArchivos.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnAbrirCarpetaRutaArchivos.Image = Global.Oversight.My.Resources.Resources.open12x12
        Me.btnAbrirCarpetaRutaArchivos.Location = New System.Drawing.Point(352, 57)
        Me.btnAbrirCarpetaRutaArchivos.Name = "btnAbrirCarpetaRutaArchivos"
        Me.btnAbrirCarpetaRutaArchivos.Size = New System.Drawing.Size(27, 23)
        Me.btnAbrirCarpetaRutaArchivos.TabIndex = 74
        Me.btnAbrirCarpetaRutaArchivos.UseVisualStyleBackColor = True
        '
        'lblRutaArchivos
        '
        Me.lblRutaArchivos.AutoSize = True
        Me.lblRutaArchivos.Location = New System.Drawing.Point(8, 62)
        Me.lblRutaArchivos.MaximumSize = New System.Drawing.Size(200, 0)
        Me.lblRutaArchivos.Name = "lblRutaArchivos"
        Me.lblRutaArchivos.Size = New System.Drawing.Size(169, 13)
        Me.lblRutaArchivos.TabIndex = 73
        Me.lblRutaArchivos.Text = "Ruta Base para Guardar Archivos:"
        '
        'txtRutaArchivos
        '
        Me.txtRutaArchivos.Enabled = False
        Me.txtRutaArchivos.Location = New System.Drawing.Point(210, 59)
        Me.txtRutaArchivos.MaxLength = 1000
        Me.txtRutaArchivos.Name = "txtRutaArchivos"
        Me.txtRutaArchivos.Size = New System.Drawing.Size(136, 20)
        Me.txtRutaArchivos.TabIndex = 72
        '
        'lblError
        '
        Me.lblError.AutoSize = True
        Me.lblError.ForeColor = System.Drawing.Color.Red
        Me.lblError.Location = New System.Drawing.Point(352, 22)
        Me.lblError.Name = "lblError"
        Me.lblError.Size = New System.Drawing.Size(55, 13)
        Me.lblError.TabIndex = 71
        Me.lblError.Text = "Incorrecto"
        Me.lblError.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        Me.lblError.Visible = False
        '
        'lblIP
        '
        Me.lblIP.AutoSize = True
        Me.lblIP.Location = New System.Drawing.Point(8, 22)
        Me.lblIP.MaximumSize = New System.Drawing.Size(200, 0)
        Me.lblIP.Name = "lblIP"
        Me.lblIP.Size = New System.Drawing.Size(197, 26)
        Me.lblIP.TabIndex = 61
        Me.lblIP.Text = "IP ó Nombre de Computadora donde se encuentra la Base de Datos:"
        '
        'txtIP
        '
        Me.txtIP.Location = New System.Drawing.Point(210, 19)
        Me.txtIP.MaxLength = 100
        Me.txtIP.Name = "txtIP"
        Me.txtIP.Size = New System.Drawing.Size(136, 20)
        Me.txtIP.TabIndex = 1
        '
        'btnGuardar
        '
        Me.btnGuardar.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnGuardar.Image = Global.Oversight.My.Resources.Resources.configb24x24
        Me.btnGuardar.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnGuardar.Location = New System.Drawing.Point(227, 127)
        Me.btnGuardar.Name = "btnGuardar"
        Me.btnGuardar.Size = New System.Drawing.Size(151, 34)
        Me.btnGuardar.TabIndex = 4
        Me.btnGuardar.Text = "&Guardar Configuración"
        Me.btnGuardar.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnGuardar.UseVisualStyleBackColor = True
        '
        'btnCancelar
        '
        Me.btnCancelar.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnCancelar.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnCancelar.Image = Global.Oversight.My.Resources.Resources.cancel24x24
        Me.btnCancelar.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnCancelar.Location = New System.Drawing.Point(55, 127)
        Me.btnCancelar.Name = "btnCancelar"
        Me.btnCancelar.Size = New System.Drawing.Size(101, 34)
        Me.btnCancelar.TabIndex = 3
        Me.btnCancelar.Text = "&Cancelar"
        Me.btnCancelar.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnCancelar.UseVisualStyleBackColor = True
        '
        'NotifyIcon1
        '
        Me.NotifyIcon1.Icon = CType(resources.GetObject("NotifyIcon1.Icon"), System.Drawing.Icon)
        Me.NotifyIcon1.Text = "Oversight"
        '
        'Opciones
        '
        Me.AcceptButton = Me.btnGuardar
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.btnCancelar
        Me.ClientSize = New System.Drawing.Size(423, 181)
        Me.Controls.Add(Me.gbCategoria)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.Name = "Opciones"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Opciones de Conexión"
        Me.gbCategoria.ResumeLayout(False)
        Me.gbCategoria.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents gbCategoria As System.Windows.Forms.GroupBox
    Friend WithEvents btnGuardar As System.Windows.Forms.Button
    Friend WithEvents btnCancelar As System.Windows.Forms.Button
    Friend WithEvents lblIP As System.Windows.Forms.Label
    Friend WithEvents txtIP As System.Windows.Forms.TextBox
    Friend WithEvents lblError As System.Windows.Forms.Label
    Friend WithEvents lblRutaArchivos As System.Windows.Forms.Label
    Friend WithEvents txtRutaArchivos As System.Windows.Forms.TextBox
    Friend WithEvents btnAbrirCarpetaRutaArchivos As System.Windows.Forms.Button
    Friend WithEvents fbd As System.Windows.Forms.FolderBrowserDialog
    Friend WithEvents NotifyIcon1 As System.Windows.Forms.NotifyIcon
    Friend WithEvents btnAbrirCarpetaRutaActualizaciones As System.Windows.Forms.Button
    Friend WithEvents lblRutaActualizaciones As System.Windows.Forms.Label
    Friend WithEvents txtRutaActualizaciones As System.Windows.Forms.TextBox
End Class
