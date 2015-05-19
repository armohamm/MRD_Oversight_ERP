<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class AgregarProveedor
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(AgregarProveedor))
        Me.gbDatosProveedor = New System.Windows.Forms.GroupBox
        Me.gbPersonas = New System.Windows.Forms.GroupBox
        Me.lblPersonas = New System.Windows.Forms.Label
        Me.dgvPersonas = New System.Windows.Forms.DataGridView
        Me.btnInsertarPersona = New System.Windows.Forms.Button
        Me.btnNuevaPersona = New System.Windows.Forms.Button
        Me.btnEliminarPersona = New System.Windows.Forms.Button
        Me.gbTelefonos = New System.Windows.Forms.GroupBox
        Me.lblTelefonos = New System.Windows.Forms.Label
        Me.dgvTelefonos = New System.Windows.Forms.DataGridView
        Me.btnNuevoTelefono = New System.Windows.Forms.Button
        Me.btnEliminarTelefono = New System.Windows.Forms.Button
        Me.gbDatosFiscales = New System.Windows.Forms.GroupBox
        Me.btnCopiarDatos = New System.Windows.Forms.Button
        Me.lblNombreFiscal = New System.Windows.Forms.Label
        Me.txtNombreFiscal = New System.Windows.Forms.TextBox
        Me.lblRFC = New System.Windows.Forms.Label
        Me.txtDireccionFiscal = New System.Windows.Forms.TextBox
        Me.txtRFC = New System.Windows.Forms.TextBox
        Me.lblDireccionFiscal = New System.Windows.Forms.Label
        Me.gbDatos = New System.Windows.Forms.GroupBox
        Me.Label1 = New System.Windows.Forms.Label
        Me.txtNombreComercial = New System.Windows.Forms.TextBox
        Me.lblEmail = New System.Windows.Forms.Label
        Me.txtEmail = New System.Windows.Forms.TextBox
        Me.txtDireccion = New System.Windows.Forms.TextBox
        Me.txtObservaciones = New System.Windows.Forms.TextBox
        Me.lblObservaciones = New System.Windows.Forms.Label
        Me.lblDireccion = New System.Windows.Forms.Label
        Me.btnCopiarPersona = New System.Windows.Forms.Button
        Me.lblNombreComercial = New System.Windows.Forms.Label
        Me.btnCancelar = New System.Windows.Forms.Button
        Me.btnGuardar = New System.Windows.Forms.Button
        Me.NotifyIcon1 = New System.Windows.Forms.NotifyIcon(Me.components)
        Me.gbDatosProveedor.SuspendLayout()
        Me.gbPersonas.SuspendLayout()
        CType(Me.dgvPersonas, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.gbTelefonos.SuspendLayout()
        CType(Me.dgvTelefonos, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.gbDatosFiscales.SuspendLayout()
        Me.gbDatos.SuspendLayout()
        Me.SuspendLayout()
        '
        'gbDatosProveedor
        '
        Me.gbDatosProveedor.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.gbDatosProveedor.Controls.Add(Me.gbPersonas)
        Me.gbDatosProveedor.Controls.Add(Me.gbTelefonos)
        Me.gbDatosProveedor.Controls.Add(Me.gbDatosFiscales)
        Me.gbDatosProveedor.Controls.Add(Me.gbDatos)
        Me.gbDatosProveedor.Controls.Add(Me.btnCancelar)
        Me.gbDatosProveedor.Controls.Add(Me.btnGuardar)
        Me.gbDatosProveedor.Location = New System.Drawing.Point(3, -2)
        Me.gbDatosProveedor.Name = "gbDatosProveedor"
        Me.gbDatosProveedor.Size = New System.Drawing.Size(935, 519)
        Me.gbDatosProveedor.TabIndex = 0
        Me.gbDatosProveedor.TabStop = False
        '
        'gbPersonas
        '
        Me.gbPersonas.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.gbPersonas.Controls.Add(Me.lblPersonas)
        Me.gbPersonas.Controls.Add(Me.dgvPersonas)
        Me.gbPersonas.Controls.Add(Me.btnInsertarPersona)
        Me.gbPersonas.Controls.Add(Me.btnNuevaPersona)
        Me.gbPersonas.Controls.Add(Me.btnEliminarPersona)
        Me.gbPersonas.Location = New System.Drawing.Point(475, 291)
        Me.gbPersonas.Name = "gbPersonas"
        Me.gbPersonas.Size = New System.Drawing.Size(450, 179)
        Me.gbPersonas.TabIndex = 87
        Me.gbPersonas.TabStop = False
        '
        'lblPersonas
        '
        Me.lblPersonas.AutoSize = True
        Me.lblPersonas.Location = New System.Drawing.Point(6, 16)
        Me.lblPersonas.MaximumSize = New System.Drawing.Size(110, 0)
        Me.lblPersonas.Name = "lblPersonas"
        Me.lblPersonas.Size = New System.Drawing.Size(75, 26)
        Me.lblPersonas.TabIndex = 72
        Me.lblPersonas.Text = "Personas Relacionadas:"
        '
        'dgvPersonas
        '
        Me.dgvPersonas.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.dgvPersonas.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvPersonas.Enabled = False
        Me.dgvPersonas.Location = New System.Drawing.Point(87, 16)
        Me.dgvPersonas.MultiSelect = False
        Me.dgvPersonas.Name = "dgvPersonas"
        Me.dgvPersonas.RowHeadersVisible = False
        Me.dgvPersonas.Size = New System.Drawing.Size(323, 151)
        Me.dgvPersonas.TabIndex = 16
        '
        'btnInsertarPersona
        '
        Me.btnInsertarPersona.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnInsertarPersona.Enabled = False
        Me.btnInsertarPersona.Image = Global.Oversight.My.Resources.Resources.insertcard12x12
        Me.btnInsertarPersona.Location = New System.Drawing.Point(416, 45)
        Me.btnInsertarPersona.Name = "btnInsertarPersona"
        Me.btnInsertarPersona.Size = New System.Drawing.Size(28, 23)
        Me.btnInsertarPersona.TabIndex = 14
        Me.btnInsertarPersona.UseVisualStyleBackColor = True
        '
        'btnNuevaPersona
        '
        Me.btnNuevaPersona.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnNuevaPersona.Enabled = False
        Me.btnNuevaPersona.Image = Global.Oversight.My.Resources.Resources.newcard12x12
        Me.btnNuevaPersona.Location = New System.Drawing.Point(416, 16)
        Me.btnNuevaPersona.Name = "btnNuevaPersona"
        Me.btnNuevaPersona.Size = New System.Drawing.Size(28, 23)
        Me.btnNuevaPersona.TabIndex = 13
        Me.btnNuevaPersona.UseVisualStyleBackColor = True
        '
        'btnEliminarPersona
        '
        Me.btnEliminarPersona.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnEliminarPersona.Enabled = False
        Me.btnEliminarPersona.Image = Global.Oversight.My.Resources.Resources.delete12x12
        Me.btnEliminarPersona.Location = New System.Drawing.Point(416, 74)
        Me.btnEliminarPersona.Name = "btnEliminarPersona"
        Me.btnEliminarPersona.Size = New System.Drawing.Size(28, 23)
        Me.btnEliminarPersona.TabIndex = 15
        Me.btnEliminarPersona.UseVisualStyleBackColor = True
        '
        'gbTelefonos
        '
        Me.gbTelefonos.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.gbTelefonos.Controls.Add(Me.lblTelefonos)
        Me.gbTelefonos.Controls.Add(Me.dgvTelefonos)
        Me.gbTelefonos.Controls.Add(Me.btnNuevoTelefono)
        Me.gbTelefonos.Controls.Add(Me.btnEliminarTelefono)
        Me.gbTelefonos.Location = New System.Drawing.Point(9, 291)
        Me.gbTelefonos.Name = "gbTelefonos"
        Me.gbTelefonos.Size = New System.Drawing.Size(460, 179)
        Me.gbTelefonos.TabIndex = 86
        Me.gbTelefonos.TabStop = False
        '
        'lblTelefonos
        '
        Me.lblTelefonos.AutoSize = True
        Me.lblTelefonos.Location = New System.Drawing.Point(6, 16)
        Me.lblTelefonos.Name = "lblTelefonos"
        Me.lblTelefonos.Size = New System.Drawing.Size(57, 13)
        Me.lblTelefonos.TabIndex = 68
        Me.lblTelefonos.Text = "Teléfonos:"
        '
        'dgvTelefonos
        '
        Me.dgvTelefonos.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.dgvTelefonos.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvTelefonos.Enabled = False
        Me.dgvTelefonos.Location = New System.Drawing.Point(69, 16)
        Me.dgvTelefonos.MultiSelect = False
        Me.dgvTelefonos.Name = "dgvTelefonos"
        Me.dgvTelefonos.RowHeadersVisible = False
        Me.dgvTelefonos.Size = New System.Drawing.Size(354, 151)
        Me.dgvTelefonos.TabIndex = 12
        '
        'btnNuevoTelefono
        '
        Me.btnNuevoTelefono.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnNuevoTelefono.Enabled = False
        Me.btnNuevoTelefono.Image = Global.Oversight.My.Resources.Resources.newcard12x12
        Me.btnNuevoTelefono.Location = New System.Drawing.Point(429, 16)
        Me.btnNuevoTelefono.Name = "btnNuevoTelefono"
        Me.btnNuevoTelefono.Size = New System.Drawing.Size(28, 23)
        Me.btnNuevoTelefono.TabIndex = 10
        Me.btnNuevoTelefono.UseVisualStyleBackColor = True
        '
        'btnEliminarTelefono
        '
        Me.btnEliminarTelefono.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnEliminarTelefono.Enabled = False
        Me.btnEliminarTelefono.Image = Global.Oversight.My.Resources.Resources.delete12x12
        Me.btnEliminarTelefono.Location = New System.Drawing.Point(429, 74)
        Me.btnEliminarTelefono.Name = "btnEliminarTelefono"
        Me.btnEliminarTelefono.Size = New System.Drawing.Size(28, 23)
        Me.btnEliminarTelefono.TabIndex = 11
        Me.btnEliminarTelefono.UseVisualStyleBackColor = True
        '
        'gbDatosFiscales
        '
        Me.gbDatosFiscales.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.gbDatosFiscales.Controls.Add(Me.btnCopiarDatos)
        Me.gbDatosFiscales.Controls.Add(Me.lblNombreFiscal)
        Me.gbDatosFiscales.Controls.Add(Me.txtNombreFiscal)
        Me.gbDatosFiscales.Controls.Add(Me.lblRFC)
        Me.gbDatosFiscales.Controls.Add(Me.txtDireccionFiscal)
        Me.gbDatosFiscales.Controls.Add(Me.txtRFC)
        Me.gbDatosFiscales.Controls.Add(Me.lblDireccionFiscal)
        Me.gbDatosFiscales.Location = New System.Drawing.Point(475, 10)
        Me.gbDatosFiscales.Name = "gbDatosFiscales"
        Me.gbDatosFiscales.Size = New System.Drawing.Size(450, 275)
        Me.gbDatosFiscales.TabIndex = 80
        Me.gbDatosFiscales.TabStop = False
        '
        'btnCopiarDatos
        '
        Me.btnCopiarDatos.Enabled = False
        Me.btnCopiarDatos.Image = Global.Oversight.My.Resources.Resources.next24x24
        Me.btnCopiarDatos.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnCopiarDatos.Location = New System.Drawing.Point(124, 20)
        Me.btnCopiarDatos.Name = "btnCopiarDatos"
        Me.btnCopiarDatos.Size = New System.Drawing.Size(241, 34)
        Me.btnCopiarDatos.TabIndex = 0
        Me.btnCopiarDatos.TabStop = False
        Me.btnCopiarDatos.Text = "U&sar los mismos Datos para la parte Fiscal"
        Me.btnCopiarDatos.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnCopiarDatos.UseVisualStyleBackColor = True
        '
        'lblNombreFiscal
        '
        Me.lblNombreFiscal.AutoSize = True
        Me.lblNombreFiscal.Location = New System.Drawing.Point(6, 68)
        Me.lblNombreFiscal.MaximumSize = New System.Drawing.Size(70, 0)
        Me.lblNombreFiscal.Name = "lblNombreFiscal"
        Me.lblNombreFiscal.Size = New System.Drawing.Size(47, 26)
        Me.lblNombreFiscal.TabIndex = 70
        Me.lblNombreFiscal.Text = "Nombre Fiscal:"
        '
        'txtNombreFiscal
        '
        Me.txtNombreFiscal.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtNombreFiscal.Location = New System.Drawing.Point(79, 73)
        Me.txtNombreFiscal.MaxLength = 500
        Me.txtNombreFiscal.Name = "txtNombreFiscal"
        Me.txtNombreFiscal.Size = New System.Drawing.Size(354, 20)
        Me.txtNombreFiscal.TabIndex = 5
        '
        'lblRFC
        '
        Me.lblRFC.AutoSize = True
        Me.lblRFC.Location = New System.Drawing.Point(6, 164)
        Me.lblRFC.Name = "lblRFC"
        Me.lblRFC.Size = New System.Drawing.Size(31, 13)
        Me.lblRFC.TabIndex = 78
        Me.lblRFC.Text = "RFC:"
        '
        'txtDireccionFiscal
        '
        Me.txtDireccionFiscal.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtDireccionFiscal.Location = New System.Drawing.Point(79, 99)
        Me.txtDireccionFiscal.Multiline = True
        Me.txtDireccionFiscal.Name = "txtDireccionFiscal"
        Me.txtDireccionFiscal.Size = New System.Drawing.Size(354, 56)
        Me.txtDireccionFiscal.TabIndex = 6
        Me.txtDireccionFiscal.Text = "Tuxtla Gutierrez, Chiapas"
        '
        'txtRFC
        '
        Me.txtRFC.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtRFC.Location = New System.Drawing.Point(79, 161)
        Me.txtRFC.MaxLength = 15
        Me.txtRFC.Name = "txtRFC"
        Me.txtRFC.Size = New System.Drawing.Size(354, 20)
        Me.txtRFC.TabIndex = 7
        '
        'lblDireccionFiscal
        '
        Me.lblDireccionFiscal.AutoSize = True
        Me.lblDireccionFiscal.Location = New System.Drawing.Point(6, 113)
        Me.lblDireccionFiscal.MaximumSize = New System.Drawing.Size(70, 0)
        Me.lblDireccionFiscal.Name = "lblDireccionFiscal"
        Me.lblDireccionFiscal.Size = New System.Drawing.Size(55, 26)
        Me.lblDireccionFiscal.TabIndex = 76
        Me.lblDireccionFiscal.Text = "Dirección Fiscal:"
        '
        'gbDatos
        '
        Me.gbDatos.Controls.Add(Me.Label1)
        Me.gbDatos.Controls.Add(Me.txtNombreComercial)
        Me.gbDatos.Controls.Add(Me.lblEmail)
        Me.gbDatos.Controls.Add(Me.txtEmail)
        Me.gbDatos.Controls.Add(Me.txtDireccion)
        Me.gbDatos.Controls.Add(Me.txtObservaciones)
        Me.gbDatos.Controls.Add(Me.lblObservaciones)
        Me.gbDatos.Controls.Add(Me.lblDireccion)
        Me.gbDatos.Controls.Add(Me.btnCopiarPersona)
        Me.gbDatos.Controls.Add(Me.lblNombreComercial)
        Me.gbDatos.Location = New System.Drawing.Point(9, 10)
        Me.gbDatos.Name = "gbDatos"
        Me.gbDatos.Size = New System.Drawing.Size(460, 275)
        Me.gbDatos.TabIndex = 79
        Me.gbDatos.TabStop = False
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(250, 31)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(152, 13)
        Me.Label1.TabIndex = 9
        Me.Label1.Text = "ó registra un nuevo proveedor:"
        '
        'txtNombreComercial
        '
        Me.txtNombreComercial.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtNombreComercial.Location = New System.Drawing.Point(97, 73)
        Me.txtNombreComercial.MaxLength = 400
        Me.txtNombreComercial.Name = "txtNombreComercial"
        Me.txtNombreComercial.Size = New System.Drawing.Size(348, 20)
        Me.txtNombreComercial.TabIndex = 1
        '
        'lblEmail
        '
        Me.lblEmail.AutoSize = True
        Me.lblEmail.Location = New System.Drawing.Point(13, 164)
        Me.lblEmail.Name = "lblEmail"
        Me.lblEmail.Size = New System.Drawing.Size(35, 13)
        Me.lblEmail.TabIndex = 4
        Me.lblEmail.Text = "Email:"
        '
        'txtEmail
        '
        Me.txtEmail.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtEmail.Location = New System.Drawing.Point(97, 161)
        Me.txtEmail.MaxLength = 500
        Me.txtEmail.Name = "txtEmail"
        Me.txtEmail.Size = New System.Drawing.Size(348, 20)
        Me.txtEmail.TabIndex = 3
        '
        'txtDireccion
        '
        Me.txtDireccion.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtDireccion.Location = New System.Drawing.Point(97, 99)
        Me.txtDireccion.Multiline = True
        Me.txtDireccion.Name = "txtDireccion"
        Me.txtDireccion.Size = New System.Drawing.Size(348, 56)
        Me.txtDireccion.TabIndex = 2
        Me.txtDireccion.Text = "Tuxtla Gutierrez, Chiapas"
        '
        'txtObservaciones
        '
        Me.txtObservaciones.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtObservaciones.Location = New System.Drawing.Point(97, 187)
        Me.txtObservaciones.MaxLength = 1000
        Me.txtObservaciones.Multiline = True
        Me.txtObservaciones.Name = "txtObservaciones"
        Me.txtObservaciones.Size = New System.Drawing.Size(348, 76)
        Me.txtObservaciones.TabIndex = 4
        '
        'lblObservaciones
        '
        Me.lblObservaciones.AutoSize = True
        Me.lblObservaciones.Location = New System.Drawing.Point(13, 190)
        Me.lblObservaciones.Name = "lblObservaciones"
        Me.lblObservaciones.Size = New System.Drawing.Size(81, 13)
        Me.lblObservaciones.TabIndex = 6
        Me.lblObservaciones.Text = "Observaciones:"
        '
        'lblDireccion
        '
        Me.lblDireccion.AutoSize = True
        Me.lblDireccion.Location = New System.Drawing.Point(13, 121)
        Me.lblDireccion.Name = "lblDireccion"
        Me.lblDireccion.Size = New System.Drawing.Size(55, 13)
        Me.lblDireccion.TabIndex = 2
        Me.lblDireccion.Text = "Dirección:"
        '
        'btnCopiarPersona
        '
        Me.btnCopiarPersona.Enabled = False
        Me.btnCopiarPersona.Image = Global.Oversight.My.Resources.Resources.clients24x24
        Me.btnCopiarPersona.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnCopiarPersona.Location = New System.Drawing.Point(52, 20)
        Me.btnCopiarPersona.Name = "btnCopiarPersona"
        Me.btnCopiarPersona.Size = New System.Drawing.Size(192, 34)
        Me.btnCopiarPersona.TabIndex = 0
        Me.btnCopiarPersona.TabStop = False
        Me.btnCopiarPersona.Text = "&Ya está registrada como Persona (trae los datos a esta ventana)"
        Me.btnCopiarPersona.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnCopiarPersona.UseVisualStyleBackColor = True
        '
        'lblNombreComercial
        '
        Me.lblNombreComercial.AutoSize = True
        Me.lblNombreComercial.Location = New System.Drawing.Point(13, 68)
        Me.lblNombreComercial.MaximumSize = New System.Drawing.Size(70, 0)
        Me.lblNombreComercial.Name = "lblNombreComercial"
        Me.lblNombreComercial.Size = New System.Drawing.Size(56, 26)
        Me.lblNombreComercial.TabIndex = 1
        Me.lblNombreComercial.Text = "Nombre Comercial:"
        '
        'btnCancelar
        '
        Me.btnCancelar.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnCancelar.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnCancelar.Image = Global.Oversight.My.Resources.Resources.cancel24x24
        Me.btnCancelar.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnCancelar.Location = New System.Drawing.Point(714, 476)
        Me.btnCancelar.Name = "btnCancelar"
        Me.btnCancelar.Size = New System.Drawing.Size(89, 34)
        Me.btnCancelar.TabIndex = 17
        Me.btnCancelar.Text = "&Cancelar"
        Me.btnCancelar.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnCancelar.UseVisualStyleBackColor = True
        '
        'btnGuardar
        '
        Me.btnGuardar.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnGuardar.Enabled = False
        Me.btnGuardar.Image = Global.Oversight.My.Resources.Resources.save24x24
        Me.btnGuardar.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnGuardar.Location = New System.Drawing.Point(809, 476)
        Me.btnGuardar.Name = "btnGuardar"
        Me.btnGuardar.Size = New System.Drawing.Size(116, 34)
        Me.btnGuardar.TabIndex = 18
        Me.btnGuardar.Text = "&Guardar Datos"
        Me.btnGuardar.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnGuardar.UseVisualStyleBackColor = True
        '
        'NotifyIcon1
        '
        Me.NotifyIcon1.Icon = CType(resources.GetObject("NotifyIcon1.Icon"), System.Drawing.Icon)
        Me.NotifyIcon1.Text = "Oversight"
        '
        'AgregarProveedor
        '
        Me.AcceptButton = Me.btnGuardar
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.btnCancelar
        Me.ClientSize = New System.Drawing.Size(943, 520)
        Me.Controls.Add(Me.gbDatosProveedor)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "AgregarProveedor"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Proveedor"
        Me.gbDatosProveedor.ResumeLayout(False)
        Me.gbPersonas.ResumeLayout(False)
        Me.gbPersonas.PerformLayout()
        CType(Me.dgvPersonas, System.ComponentModel.ISupportInitialize).EndInit()
        Me.gbTelefonos.ResumeLayout(False)
        Me.gbTelefonos.PerformLayout()
        CType(Me.dgvTelefonos, System.ComponentModel.ISupportInitialize).EndInit()
        Me.gbDatosFiscales.ResumeLayout(False)
        Me.gbDatosFiscales.PerformLayout()
        Me.gbDatos.ResumeLayout(False)
        Me.gbDatos.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents gbDatosProveedor As System.Windows.Forms.GroupBox
    Friend WithEvents lblNombreComercial As System.Windows.Forms.Label
    Friend WithEvents txtNombreComercial As System.Windows.Forms.TextBox
    Friend WithEvents lblDireccion As System.Windows.Forms.Label
    Friend WithEvents txtObservaciones As System.Windows.Forms.TextBox
    Friend WithEvents lblObservaciones As System.Windows.Forms.Label
    Friend WithEvents txtEmail As System.Windows.Forms.TextBox
    Friend WithEvents lblEmail As System.Windows.Forms.Label
    Friend WithEvents txtDireccion As System.Windows.Forms.TextBox
    Friend WithEvents btnCancelar As System.Windows.Forms.Button
    Friend WithEvents btnGuardar As System.Windows.Forms.Button
    Friend WithEvents dgvTelefonos As System.Windows.Forms.DataGridView
    Friend WithEvents lblTelefonos As System.Windows.Forms.Label
    Friend WithEvents lblNombreFiscal As System.Windows.Forms.Label
    Friend WithEvents txtNombreFiscal As System.Windows.Forms.TextBox
    Friend WithEvents dgvPersonas As System.Windows.Forms.DataGridView
    Friend WithEvents lblPersonas As System.Windows.Forms.Label
    Friend WithEvents btnCopiarPersona As System.Windows.Forms.Button
    Friend WithEvents txtDireccionFiscal As System.Windows.Forms.TextBox
    Friend WithEvents lblRFC As System.Windows.Forms.Label
    Friend WithEvents txtRFC As System.Windows.Forms.TextBox
    Friend WithEvents lblDireccionFiscal As System.Windows.Forms.Label
    Friend WithEvents gbDatosFiscales As System.Windows.Forms.GroupBox
    Friend WithEvents gbDatos As System.Windows.Forms.GroupBox
    Friend WithEvents btnCopiarDatos As System.Windows.Forms.Button
    Friend WithEvents btnEliminarPersona As System.Windows.Forms.Button
    Friend WithEvents btnNuevaPersona As System.Windows.Forms.Button
    Friend WithEvents btnEliminarTelefono As System.Windows.Forms.Button
    Friend WithEvents btnNuevoTelefono As System.Windows.Forms.Button
    Friend WithEvents btnInsertarPersona As System.Windows.Forms.Button
    Friend WithEvents NotifyIcon1 As System.Windows.Forms.NotifyIcon
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents gbPersonas As System.Windows.Forms.GroupBox
    Friend WithEvents gbTelefonos As System.Windows.Forms.GroupBox
End Class
