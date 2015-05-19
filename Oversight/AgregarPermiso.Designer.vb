<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class AgregarPermiso
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(AgregarPermiso))
        Me.gbCategoria = New System.Windows.Forms.GroupBox
        Me.lblPermiso = New System.Windows.Forms.Label
        Me.txtPermiso = New System.Windows.Forms.TextBox
        Me.lblDisponibilidadUsuario = New System.Windows.Forms.Label
        Me.lblVentana = New System.Windows.Forms.Label
        Me.txtVentana = New System.Windows.Forms.TextBox
        Me.lblUsuario = New System.Windows.Forms.Label
        Me.txtUsuario = New System.Windows.Forms.TextBox
        Me.btnGuardar = New System.Windows.Forms.Button
        Me.btnCancelar = New System.Windows.Forms.Button
        Me.NotifyIcon1 = New System.Windows.Forms.NotifyIcon(Me.components)
        Me.gbCategoria.SuspendLayout()
        Me.SuspendLayout()
        '
        'gbCategoria
        '
        Me.gbCategoria.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.gbCategoria.Controls.Add(Me.lblPermiso)
        Me.gbCategoria.Controls.Add(Me.txtPermiso)
        Me.gbCategoria.Controls.Add(Me.lblDisponibilidadUsuario)
        Me.gbCategoria.Controls.Add(Me.lblVentana)
        Me.gbCategoria.Controls.Add(Me.txtVentana)
        Me.gbCategoria.Controls.Add(Me.lblUsuario)
        Me.gbCategoria.Controls.Add(Me.txtUsuario)
        Me.gbCategoria.Controls.Add(Me.btnGuardar)
        Me.gbCategoria.Controls.Add(Me.btnCancelar)
        Me.gbCategoria.Location = New System.Drawing.Point(3, -2)
        Me.gbCategoria.Name = "gbCategoria"
        Me.gbCategoria.Size = New System.Drawing.Size(453, 171)
        Me.gbCategoria.TabIndex = 72
        Me.gbCategoria.TabStop = False
        '
        'lblPermiso
        '
        Me.lblPermiso.AutoSize = True
        Me.lblPermiso.Location = New System.Drawing.Point(8, 74)
        Me.lblPermiso.Name = "lblPermiso"
        Me.lblPermiso.Size = New System.Drawing.Size(47, 13)
        Me.lblPermiso.TabIndex = 75
        Me.lblPermiso.Text = "Permiso:"
        '
        'txtPermiso
        '
        Me.txtPermiso.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtPermiso.Location = New System.Drawing.Point(88, 71)
        Me.txtPermiso.MaxLength = 500
        Me.txtPermiso.Multiline = True
        Me.txtPermiso.Name = "txtPermiso"
        Me.txtPermiso.Size = New System.Drawing.Size(279, 20)
        Me.txtPermiso.TabIndex = 4
        '
        'lblDisponibilidadUsuario
        '
        Me.lblDisponibilidadUsuario.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblDisponibilidadUsuario.AutoSize = True
        Me.lblDisponibilidadUsuario.ForeColor = System.Drawing.Color.ForestGreen
        Me.lblDisponibilidadUsuario.Location = New System.Drawing.Point(373, 22)
        Me.lblDisponibilidadUsuario.Name = "lblDisponibilidadUsuario"
        Me.lblDisponibilidadUsuario.Size = New System.Drawing.Size(56, 13)
        Me.lblDisponibilidadUsuario.TabIndex = 71
        Me.lblDisponibilidadUsuario.Text = "Disponible"
        Me.lblDisponibilidadUsuario.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        Me.lblDisponibilidadUsuario.Visible = False
        '
        'lblVentana
        '
        Me.lblVentana.AutoSize = True
        Me.lblVentana.Location = New System.Drawing.Point(8, 48)
        Me.lblVentana.Name = "lblVentana"
        Me.lblVentana.Size = New System.Drawing.Size(50, 13)
        Me.lblVentana.TabIndex = 63
        Me.lblVentana.Text = "Ventana:"
        '
        'txtVentana
        '
        Me.txtVentana.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtVentana.Location = New System.Drawing.Point(88, 45)
        Me.txtVentana.MaxLength = 300
        Me.txtVentana.Multiline = True
        Me.txtVentana.Name = "txtVentana"
        Me.txtVentana.Size = New System.Drawing.Size(279, 20)
        Me.txtVentana.TabIndex = 2
        '
        'lblUsuario
        '
        Me.lblUsuario.AutoSize = True
        Me.lblUsuario.Location = New System.Drawing.Point(8, 22)
        Me.lblUsuario.Name = "lblUsuario"
        Me.lblUsuario.Size = New System.Drawing.Size(46, 13)
        Me.lblUsuario.TabIndex = 61
        Me.lblUsuario.Text = "Usuario:"
        '
        'txtUsuario
        '
        Me.txtUsuario.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtUsuario.Location = New System.Drawing.Point(88, 19)
        Me.txtUsuario.MaxLength = 100
        Me.txtUsuario.Name = "txtUsuario"
        Me.txtUsuario.Size = New System.Drawing.Size(279, 20)
        Me.txtUsuario.TabIndex = 1
        '
        'btnGuardar
        '
        Me.btnGuardar.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnGuardar.Image = Global.Oversight.My.Resources.Resources.copyright24x24
        Me.btnGuardar.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnGuardar.Location = New System.Drawing.Point(317, 116)
        Me.btnGuardar.Name = "btnGuardar"
        Me.btnGuardar.Size = New System.Drawing.Size(130, 34)
        Me.btnGuardar.TabIndex = 6
        Me.btnGuardar.Text = "&Guardar Permiso"
        Me.btnGuardar.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnGuardar.UseVisualStyleBackColor = True
        '
        'btnCancelar
        '
        Me.btnCancelar.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnCancelar.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnCancelar.Image = Global.Oversight.My.Resources.Resources.cancel24x24
        Me.btnCancelar.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnCancelar.Location = New System.Drawing.Point(210, 116)
        Me.btnCancelar.Name = "btnCancelar"
        Me.btnCancelar.Size = New System.Drawing.Size(101, 34)
        Me.btnCancelar.TabIndex = 5
        Me.btnCancelar.Text = "&Cancelar"
        Me.btnCancelar.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnCancelar.UseVisualStyleBackColor = True
        '
        'NotifyIcon1
        '
        Me.NotifyIcon1.Icon = CType(resources.GetObject("NotifyIcon1.Icon"), System.Drawing.Icon)
        Me.NotifyIcon1.Text = "Oversight"
        '
        'AgregarPermiso
        '
        Me.AcceptButton = Me.btnGuardar
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.btnCancelar
        Me.ClientSize = New System.Drawing.Size(462, 173)
        Me.Controls.Add(Me.gbCategoria)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "AgregarPermiso"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Permiso"
        Me.gbCategoria.ResumeLayout(False)
        Me.gbCategoria.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents gbCategoria As System.Windows.Forms.GroupBox
    Friend WithEvents btnGuardar As System.Windows.Forms.Button
    Friend WithEvents btnCancelar As System.Windows.Forms.Button
    Friend WithEvents lblVentana As System.Windows.Forms.Label
    Friend WithEvents txtVentana As System.Windows.Forms.TextBox
    Friend WithEvents lblUsuario As System.Windows.Forms.Label
    Friend WithEvents txtUsuario As System.Windows.Forms.TextBox
    Friend WithEvents lblDisponibilidadUsuario As System.Windows.Forms.Label
    Friend WithEvents lblPermiso As System.Windows.Forms.Label
    Friend WithEvents txtPermiso As System.Windows.Forms.TextBox
    Friend WithEvents NotifyIcon1 As System.Windows.Forms.NotifyIcon
End Class
