<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class AgregarPersona
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(AgregarPersona))
        Me.gbDatosInsumo = New System.Windows.Forms.GroupBox
        Me.btnEliminarTelefono = New System.Windows.Forms.Button
        Me.dgvTelefonos = New System.Windows.Forms.DataGridView
        Me.lblTelefonos = New System.Windows.Forms.Label
        Me.btnNuevoTelefono = New System.Windows.Forms.Button
        Me.btnDireccion = New System.Windows.Forms.Button
        Me.chkCliente = New System.Windows.Forms.CheckBox
        Me.lblSexo = New System.Windows.Forms.Label
        Me.lblNombreCompleto = New System.Windows.Forms.Label
        Me.btnCancelar = New System.Windows.Forms.Button
        Me.cmbSexo = New System.Windows.Forms.ComboBox
        Me.btnGuardar = New System.Windows.Forms.Button
        Me.txtNombreCompleto = New System.Windows.Forms.TextBox
        Me.lblDireccion = New System.Windows.Forms.Label
        Me.lblObservaciones = New System.Windows.Forms.Label
        Me.txtObservaciones = New System.Windows.Forms.TextBox
        Me.txtDireccion = New System.Windows.Forms.TextBox
        Me.txtEmail = New System.Windows.Forms.TextBox
        Me.lblEmail = New System.Windows.Forms.Label
        Me.NotifyIcon1 = New System.Windows.Forms.NotifyIcon(Me.components)
        Me.gbDatosInsumo.SuspendLayout()
        CType(Me.dgvTelefonos, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'gbDatosInsumo
        '
        Me.gbDatosInsumo.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.gbDatosInsumo.Controls.Add(Me.btnEliminarTelefono)
        Me.gbDatosInsumo.Controls.Add(Me.dgvTelefonos)
        Me.gbDatosInsumo.Controls.Add(Me.lblTelefonos)
        Me.gbDatosInsumo.Controls.Add(Me.btnNuevoTelefono)
        Me.gbDatosInsumo.Controls.Add(Me.btnDireccion)
        Me.gbDatosInsumo.Controls.Add(Me.chkCliente)
        Me.gbDatosInsumo.Controls.Add(Me.lblSexo)
        Me.gbDatosInsumo.Controls.Add(Me.lblNombreCompleto)
        Me.gbDatosInsumo.Controls.Add(Me.btnCancelar)
        Me.gbDatosInsumo.Controls.Add(Me.cmbSexo)
        Me.gbDatosInsumo.Controls.Add(Me.btnGuardar)
        Me.gbDatosInsumo.Controls.Add(Me.txtNombreCompleto)
        Me.gbDatosInsumo.Controls.Add(Me.lblDireccion)
        Me.gbDatosInsumo.Controls.Add(Me.lblObservaciones)
        Me.gbDatosInsumo.Controls.Add(Me.txtObservaciones)
        Me.gbDatosInsumo.Controls.Add(Me.txtDireccion)
        Me.gbDatosInsumo.Controls.Add(Me.txtEmail)
        Me.gbDatosInsumo.Controls.Add(Me.lblEmail)
        Me.gbDatosInsumo.Location = New System.Drawing.Point(3, -2)
        Me.gbDatosInsumo.Name = "gbDatosInsumo"
        Me.gbDatosInsumo.Size = New System.Drawing.Size(554, 344)
        Me.gbDatosInsumo.TabIndex = 0
        Me.gbDatosInsumo.TabStop = False
        '
        'btnEliminarTelefono
        '
        Me.btnEliminarTelefono.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnEliminarTelefono.Enabled = False
        Me.btnEliminarTelefono.Image = Global.Oversight.My.Resources.Resources.delete12x12
        Me.btnEliminarTelefono.Location = New System.Drawing.Point(300, 225)
        Me.btnEliminarTelefono.Name = "btnEliminarTelefono"
        Me.btnEliminarTelefono.Size = New System.Drawing.Size(28, 23)
        Me.btnEliminarTelefono.TabIndex = 7
        Me.btnEliminarTelefono.UseVisualStyleBackColor = True
        '
        'dgvTelefonos
        '
        Me.dgvTelefonos.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.dgvTelefonos.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvTelefonos.Enabled = False
        Me.dgvTelefonos.Location = New System.Drawing.Point(111, 196)
        Me.dgvTelefonos.MultiSelect = False
        Me.dgvTelefonos.Name = "dgvTelefonos"
        Me.dgvTelefonos.RowHeadersVisible = False
        Me.dgvTelefonos.Size = New System.Drawing.Size(185, 136)
        Me.dgvTelefonos.TabIndex = 8
        '
        'lblTelefonos
        '
        Me.lblTelefonos.AutoSize = True
        Me.lblTelefonos.Location = New System.Drawing.Point(5, 196)
        Me.lblTelefonos.Name = "lblTelefonos"
        Me.lblTelefonos.Size = New System.Drawing.Size(57, 13)
        Me.lblTelefonos.TabIndex = 68
        Me.lblTelefonos.Text = "Teléfonos:"
        '
        'btnNuevoTelefono
        '
        Me.btnNuevoTelefono.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnNuevoTelefono.Enabled = False
        Me.btnNuevoTelefono.Image = Global.Oversight.My.Resources.Resources.newcard12x12
        Me.btnNuevoTelefono.Location = New System.Drawing.Point(300, 196)
        Me.btnNuevoTelefono.Name = "btnNuevoTelefono"
        Me.btnNuevoTelefono.Size = New System.Drawing.Size(28, 23)
        Me.btnNuevoTelefono.TabIndex = 6
        Me.btnNuevoTelefono.UseVisualStyleBackColor = True
        '
        'btnDireccion
        '
        Me.btnDireccion.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnDireccion.Enabled = False
        Me.btnDireccion.Image = Global.Oversight.My.Resources.Resources.note24x24
        Me.btnDireccion.Location = New System.Drawing.Point(506, 66)
        Me.btnDireccion.Name = "btnDireccion"
        Me.btnDireccion.Size = New System.Drawing.Size(39, 42)
        Me.btnDireccion.TabIndex = 0
        Me.btnDireccion.TabStop = False
        Me.btnDireccion.UseVisualStyleBackColor = True
        '
        'chkCliente
        '
        Me.chkCliente.AutoSize = True
        Me.chkCliente.Enabled = False
        Me.chkCliente.Location = New System.Drawing.Point(300, 40)
        Me.chkCliente.Name = "chkCliente"
        Me.chkCliente.Size = New System.Drawing.Size(64, 17)
        Me.chkCliente.TabIndex = 0
        Me.chkCliente.TabStop = False
        Me.chkCliente.Text = "Cliente?"
        Me.chkCliente.UseVisualStyleBackColor = True
        '
        'lblSexo
        '
        Me.lblSexo.AutoSize = True
        Me.lblSexo.Location = New System.Drawing.Point(6, 42)
        Me.lblSexo.Name = "lblSexo"
        Me.lblSexo.Size = New System.Drawing.Size(34, 13)
        Me.lblSexo.TabIndex = 10
        Me.lblSexo.Text = "Sexo:"
        '
        'lblNombreCompleto
        '
        Me.lblNombreCompleto.AutoSize = True
        Me.lblNombreCompleto.Location = New System.Drawing.Point(6, 16)
        Me.lblNombreCompleto.Name = "lblNombreCompleto"
        Me.lblNombreCompleto.Size = New System.Drawing.Size(94, 13)
        Me.lblNombreCompleto.TabIndex = 1
        Me.lblNombreCompleto.Text = "Nombre Completo:"
        '
        'btnCancelar
        '
        Me.btnCancelar.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnCancelar.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnCancelar.Image = Global.Oversight.My.Resources.Resources.cancel24x24
        Me.btnCancelar.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnCancelar.Location = New System.Drawing.Point(337, 298)
        Me.btnCancelar.Name = "btnCancelar"
        Me.btnCancelar.Size = New System.Drawing.Size(89, 34)
        Me.btnCancelar.TabIndex = 9
        Me.btnCancelar.Text = "&Cancelar"
        Me.btnCancelar.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnCancelar.UseVisualStyleBackColor = True
        '
        'cmbSexo
        '
        Me.cmbSexo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbSexo.FormattingEnabled = True
        Me.cmbSexo.Items.AddRange(New Object() {"Femenino", "Masculino"})
        Me.cmbSexo.Location = New System.Drawing.Point(111, 39)
        Me.cmbSexo.Name = "cmbSexo"
        Me.cmbSexo.Size = New System.Drawing.Size(157, 21)
        Me.cmbSexo.TabIndex = 2
        '
        'btnGuardar
        '
        Me.btnGuardar.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnGuardar.Enabled = False
        Me.btnGuardar.Image = Global.Oversight.My.Resources.Resources.save24x24
        Me.btnGuardar.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnGuardar.Location = New System.Drawing.Point(432, 298)
        Me.btnGuardar.Name = "btnGuardar"
        Me.btnGuardar.Size = New System.Drawing.Size(116, 34)
        Me.btnGuardar.TabIndex = 10
        Me.btnGuardar.Text = "&Guardar Datos"
        Me.btnGuardar.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnGuardar.UseVisualStyleBackColor = True
        '
        'txtNombreCompleto
        '
        Me.txtNombreCompleto.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtNombreCompleto.Location = New System.Drawing.Point(111, 13)
        Me.txtNombreCompleto.MaxLength = 500
        Me.txtNombreCompleto.Name = "txtNombreCompleto"
        Me.txtNombreCompleto.Size = New System.Drawing.Size(434, 20)
        Me.txtNombreCompleto.TabIndex = 1
        '
        'lblDireccion
        '
        Me.lblDireccion.AutoSize = True
        Me.lblDireccion.Location = New System.Drawing.Point(6, 69)
        Me.lblDireccion.Name = "lblDireccion"
        Me.lblDireccion.Size = New System.Drawing.Size(55, 13)
        Me.lblDireccion.TabIndex = 2
        Me.lblDireccion.Text = "Dirección:"
        '
        'lblObservaciones
        '
        Me.lblObservaciones.AutoSize = True
        Me.lblObservaciones.Location = New System.Drawing.Point(6, 143)
        Me.lblObservaciones.Name = "lblObservaciones"
        Me.lblObservaciones.Size = New System.Drawing.Size(81, 13)
        Me.lblObservaciones.TabIndex = 6
        Me.lblObservaciones.Text = "Observaciones:"
        '
        'txtObservaciones
        '
        Me.txtObservaciones.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtObservaciones.Location = New System.Drawing.Point(111, 140)
        Me.txtObservaciones.MaxLength = 1000
        Me.txtObservaciones.Multiline = True
        Me.txtObservaciones.Name = "txtObservaciones"
        Me.txtObservaciones.Size = New System.Drawing.Size(434, 50)
        Me.txtObservaciones.TabIndex = 5
        '
        'txtDireccion
        '
        Me.txtDireccion.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtDireccion.Location = New System.Drawing.Point(111, 66)
        Me.txtDireccion.MaxLength = 1000
        Me.txtDireccion.Multiline = True
        Me.txtDireccion.Name = "txtDireccion"
        Me.txtDireccion.Size = New System.Drawing.Size(389, 42)
        Me.txtDireccion.TabIndex = 3
        '
        'txtEmail
        '
        Me.txtEmail.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtEmail.Location = New System.Drawing.Point(111, 114)
        Me.txtEmail.MaxLength = 500
        Me.txtEmail.Name = "txtEmail"
        Me.txtEmail.Size = New System.Drawing.Size(434, 20)
        Me.txtEmail.TabIndex = 4
        '
        'lblEmail
        '
        Me.lblEmail.AutoSize = True
        Me.lblEmail.Location = New System.Drawing.Point(6, 117)
        Me.lblEmail.Name = "lblEmail"
        Me.lblEmail.Size = New System.Drawing.Size(35, 13)
        Me.lblEmail.TabIndex = 4
        Me.lblEmail.Text = "Email:"
        '
        'NotifyIcon1
        '
        Me.NotifyIcon1.Icon = CType(resources.GetObject("NotifyIcon1.Icon"), System.Drawing.Icon)
        Me.NotifyIcon1.Text = "Oversight"
        '
        'AgregarPersona
        '
        Me.AcceptButton = Me.btnGuardar
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.btnCancelar
        Me.ClientSize = New System.Drawing.Size(560, 346)
        Me.Controls.Add(Me.gbDatosInsumo)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "AgregarPersona"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Persona"
        Me.gbDatosInsumo.ResumeLayout(False)
        Me.gbDatosInsumo.PerformLayout()
        CType(Me.dgvTelefonos, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents gbDatosInsumo As System.Windows.Forms.GroupBox
    Friend WithEvents lblNombreCompleto As System.Windows.Forms.Label
    Friend WithEvents txtNombreCompleto As System.Windows.Forms.TextBox
    Friend WithEvents lblDireccion As System.Windows.Forms.Label
    Friend WithEvents txtObservaciones As System.Windows.Forms.TextBox
    Friend WithEvents lblObservaciones As System.Windows.Forms.Label
    Friend WithEvents txtEmail As System.Windows.Forms.TextBox
    Friend WithEvents lblEmail As System.Windows.Forms.Label
    Friend WithEvents txtDireccion As System.Windows.Forms.TextBox
    Friend WithEvents btnCancelar As System.Windows.Forms.Button
    Friend WithEvents btnGuardar As System.Windows.Forms.Button
    Friend WithEvents cmbSexo As System.Windows.Forms.ComboBox
    Friend WithEvents lblSexo As System.Windows.Forms.Label
    Friend WithEvents chkCliente As System.Windows.Forms.CheckBox
    Friend WithEvents btnDireccion As System.Windows.Forms.Button
    Friend WithEvents dgvTelefonos As System.Windows.Forms.DataGridView
    Friend WithEvents lblTelefonos As System.Windows.Forms.Label
    Friend WithEvents btnEliminarTelefono As System.Windows.Forms.Button
    Friend WithEvents btnNuevoTelefono As System.Windows.Forms.Button
    Friend WithEvents NotifyIcon1 As System.Windows.Forms.NotifyIcon
End Class
