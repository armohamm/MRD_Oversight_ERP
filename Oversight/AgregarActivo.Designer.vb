<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class AgregarActivo
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(AgregarActivo))
        Me.gbCategoria = New System.Windows.Forms.GroupBox
        Me.btnTipoActivo = New System.Windows.Forms.Button
        Me.cmbTipoActivo = New System.Windows.Forms.ComboBox
        Me.lblTipoActivo = New System.Windows.Forms.Label
        Me.lblDescripcion = New System.Windows.Forms.Label
        Me.txtDescripcion = New System.Windows.Forms.TextBox
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
        Me.gbCategoria.Controls.Add(Me.btnTipoActivo)
        Me.gbCategoria.Controls.Add(Me.cmbTipoActivo)
        Me.gbCategoria.Controls.Add(Me.lblTipoActivo)
        Me.gbCategoria.Controls.Add(Me.lblDescripcion)
        Me.gbCategoria.Controls.Add(Me.txtDescripcion)
        Me.gbCategoria.Controls.Add(Me.btnGuardar)
        Me.gbCategoria.Controls.Add(Me.btnCancelar)
        Me.gbCategoria.Location = New System.Drawing.Point(3, -2)
        Me.gbCategoria.Name = "gbCategoria"
        Me.gbCategoria.Size = New System.Drawing.Size(451, 195)
        Me.gbCategoria.TabIndex = 57
        Me.gbCategoria.TabStop = False
        '
        'btnTipoActivo
        '
        Me.btnTipoActivo.Enabled = False
        Me.btnTipoActivo.Image = Global.Oversight.My.Resources.Resources.asset12x12
        Me.btnTipoActivo.Location = New System.Drawing.Point(319, 11)
        Me.btnTipoActivo.Name = "btnTipoActivo"
        Me.btnTipoActivo.Size = New System.Drawing.Size(28, 23)
        Me.btnTipoActivo.TabIndex = 75
        Me.btnTipoActivo.UseVisualStyleBackColor = True
        '
        'cmbTipoActivo
        '
        Me.cmbTipoActivo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbTipoActivo.FormattingEnabled = True
        Me.cmbTipoActivo.Location = New System.Drawing.Point(81, 13)
        Me.cmbTipoActivo.Name = "cmbTipoActivo"
        Me.cmbTipoActivo.Size = New System.Drawing.Size(232, 21)
        Me.cmbTipoActivo.TabIndex = 65
        '
        'lblTipoActivo
        '
        Me.lblTipoActivo.AutoSize = True
        Me.lblTipoActivo.Location = New System.Drawing.Point(9, 16)
        Me.lblTipoActivo.Name = "lblTipoActivo"
        Me.lblTipoActivo.Size = New System.Drawing.Size(64, 13)
        Me.lblTipoActivo.TabIndex = 64
        Me.lblTipoActivo.Text = "Tipo Activo:"
        '
        'lblDescripcion
        '
        Me.lblDescripcion.AutoSize = True
        Me.lblDescripcion.Location = New System.Drawing.Point(9, 47)
        Me.lblDescripcion.Name = "lblDescripcion"
        Me.lblDescripcion.Size = New System.Drawing.Size(66, 13)
        Me.lblDescripcion.TabIndex = 63
        Me.lblDescripcion.Text = "Descripción:"
        '
        'txtDescripcion
        '
        Me.txtDescripcion.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtDescripcion.Location = New System.Drawing.Point(81, 45)
        Me.txtDescripcion.MaxLength = 500
        Me.txtDescripcion.Multiline = True
        Me.txtDescripcion.Name = "txtDescripcion"
        Me.txtDescripcion.Size = New System.Drawing.Size(362, 100)
        Me.txtDescripcion.TabIndex = 2
        '
        'btnGuardar
        '
        Me.btnGuardar.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnGuardar.Enabled = False
        Me.btnGuardar.Image = Global.Oversight.My.Resources.Resources.asset24x24
        Me.btnGuardar.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnGuardar.Location = New System.Drawing.Point(329, 151)
        Me.btnGuardar.Name = "btnGuardar"
        Me.btnGuardar.Size = New System.Drawing.Size(114, 34)
        Me.btnGuardar.TabIndex = 4
        Me.btnGuardar.Text = "&Guardar Activo"
        Me.btnGuardar.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnGuardar.UseVisualStyleBackColor = True
        '
        'btnCancelar
        '
        Me.btnCancelar.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnCancelar.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnCancelar.Image = Global.Oversight.My.Resources.Resources.cancel24x24
        Me.btnCancelar.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnCancelar.Location = New System.Drawing.Point(222, 151)
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
        'AgregarActivo
        '
        Me.AcceptButton = Me.btnGuardar
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.btnCancelar
        Me.ClientSize = New System.Drawing.Size(458, 197)
        Me.Controls.Add(Me.gbCategoria)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "AgregarActivo"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Activo"
        Me.gbCategoria.ResumeLayout(False)
        Me.gbCategoria.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents gbCategoria As System.Windows.Forms.GroupBox
    Friend WithEvents btnGuardar As System.Windows.Forms.Button
    Friend WithEvents btnCancelar As System.Windows.Forms.Button
    Friend WithEvents lblDescripcion As System.Windows.Forms.Label
    Friend WithEvents txtDescripcion As System.Windows.Forms.TextBox
    Friend WithEvents cmbTipoActivo As System.Windows.Forms.ComboBox
    Friend WithEvents lblTipoActivo As System.Windows.Forms.Label
    Friend WithEvents btnTipoActivo As System.Windows.Forms.Button
    Friend WithEvents NotifyIcon1 As System.Windows.Forms.NotifyIcon
End Class
