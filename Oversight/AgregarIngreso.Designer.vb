<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class AgregarIngreso
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(AgregarIngreso))
        Me.btnPersonas = New System.Windows.Forms.Button
        Me.txtNombrePersona = New System.Windows.Forms.TextBox
        Me.lblPersona = New System.Windows.Forms.Label
        Me.btnBancoOrigen = New System.Windows.Forms.Button
        Me.btnCuentaDestino = New System.Windows.Forms.Button
        Me.btnTipoIngreso = New System.Windows.Forms.Button
        Me.txtCuentaOrigen = New System.Windows.Forms.TextBox
        Me.lblImporte = New System.Windows.Forms.Label
        Me.txtImporte = New System.Windows.Forms.TextBox
        Me.lblBancoOrigen = New System.Windows.Forms.Label
        Me.cmbBancoOrigen = New System.Windows.Forms.ComboBox
        Me.lblCuentaDestino = New System.Windows.Forms.Label
        Me.lblCuentaOrigen = New System.Windows.Forms.Label
        Me.cmbCuentaDestino = New System.Windows.Forms.ComboBox
        Me.cmbTipoIngreso = New System.Windows.Forms.ComboBox
        Me.lblTipoIngreso = New System.Windows.Forms.Label
        Me.lblFecha = New System.Windows.Forms.Label
        Me.dtFechaIngreso = New System.Windows.Forms.DateTimePicker
        Me.lblDescripcion = New System.Windows.Forms.Label
        Me.txtDescripcionIngreso = New System.Windows.Forms.TextBox
        Me.lblReferenciaOrigen = New System.Windows.Forms.Label
        Me.txtReferenciaOrigen = New System.Windows.Forms.TextBox
        Me.btnGuardar = New System.Windows.Forms.Button
        Me.btnCancelar = New System.Windows.Forms.Button
        Me.tcIngresos = New System.Windows.Forms.TabControl
        Me.tpIngreso = New System.Windows.Forms.TabPage
        Me.tpRelaciones = New System.Windows.Forms.TabPage
        Me.btnCancelarRelacion = New System.Windows.Forms.Button
        Me.btnGuardarRelacion = New System.Windows.Forms.Button
        Me.btnEliminarRelacion = New System.Windows.Forms.Button
        Me.btnBuscarRelacion = New System.Windows.Forms.Button
        Me.lblItemsYaRelacionados = New System.Windows.Forms.Label
        Me.dgvConRelacion = New System.Windows.Forms.DataGridView
        Me.cmbTipoRelacion = New System.Windows.Forms.ComboBox
        Me.NotifyIcon1 = New System.Windows.Forms.NotifyIcon(Me.components)
        Me.btnRevisiones = New System.Windows.Forms.Button
        Me.btnRevisionesRelaciones = New System.Windows.Forms.Button
        Me.tcIngresos.SuspendLayout()
        Me.tpIngreso.SuspendLayout()
        Me.tpRelaciones.SuspendLayout()
        CType(Me.dgvConRelacion, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'btnPersonas
        '
        Me.btnPersonas.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnPersonas.Enabled = False
        Me.btnPersonas.Image = Global.Oversight.My.Resources.Resources.clients12x12
        Me.btnPersonas.Location = New System.Drawing.Point(438, 309)
        Me.btnPersonas.Name = "btnPersonas"
        Me.btnPersonas.Size = New System.Drawing.Size(27, 23)
        Me.btnPersonas.TabIndex = 13
        Me.btnPersonas.UseVisualStyleBackColor = True
        '
        'txtNombrePersona
        '
        Me.txtNombrePersona.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtNombrePersona.Enabled = False
        Me.txtNombrePersona.Location = New System.Drawing.Point(177, 311)
        Me.txtNombrePersona.Name = "txtNombrePersona"
        Me.txtNombrePersona.Size = New System.Drawing.Size(255, 20)
        Me.txtNombrePersona.TabIndex = 0
        Me.txtNombrePersona.TabStop = False
        '
        'lblPersona
        '
        Me.lblPersona.AutoSize = True
        Me.lblPersona.Location = New System.Drawing.Point(6, 314)
        Me.lblPersona.Name = "lblPersona"
        Me.lblPersona.Size = New System.Drawing.Size(70, 13)
        Me.lblPersona.TabIndex = 90
        Me.lblPersona.Text = "Recibido por:"
        '
        'btnBancoOrigen
        '
        Me.btnBancoOrigen.Enabled = False
        Me.btnBancoOrigen.Image = Global.Oversight.My.Resources.Resources.bank12x12
        Me.btnBancoOrigen.Location = New System.Drawing.Point(415, 62)
        Me.btnBancoOrigen.Name = "btnBancoOrigen"
        Me.btnBancoOrigen.Size = New System.Drawing.Size(27, 23)
        Me.btnBancoOrigen.TabIndex = 5
        Me.btnBancoOrigen.UseVisualStyleBackColor = True
        '
        'btnCuentaDestino
        '
        Me.btnCuentaDestino.Enabled = False
        Me.btnCuentaDestino.Image = Global.Oversight.My.Resources.Resources.creditcard12x12
        Me.btnCuentaDestino.Location = New System.Drawing.Point(413, 142)
        Me.btnCuentaDestino.Name = "btnCuentaDestino"
        Me.btnCuentaDestino.Size = New System.Drawing.Size(27, 23)
        Me.btnCuentaDestino.TabIndex = 9
        Me.btnCuentaDestino.UseVisualStyleBackColor = True
        '
        'btnTipoIngreso
        '
        Me.btnTipoIngreso.Enabled = False
        Me.btnTipoIngreso.Image = Global.Oversight.My.Resources.Resources.money12x12
        Me.btnTipoIngreso.Location = New System.Drawing.Point(415, 35)
        Me.btnTipoIngreso.Name = "btnTipoIngreso"
        Me.btnTipoIngreso.Size = New System.Drawing.Size(27, 23)
        Me.btnTipoIngreso.TabIndex = 3
        Me.btnTipoIngreso.UseVisualStyleBackColor = True
        '
        'txtCuentaOrigen
        '
        Me.txtCuentaOrigen.Location = New System.Drawing.Point(178, 91)
        Me.txtCuentaOrigen.Name = "txtCuentaOrigen"
        Me.txtCuentaOrigen.Size = New System.Drawing.Size(231, 20)
        Me.txtCuentaOrigen.TabIndex = 6
        '
        'lblImporte
        '
        Me.lblImporte.AutoSize = True
        Me.lblImporte.Location = New System.Drawing.Point(5, 281)
        Me.lblImporte.Name = "lblImporte"
        Me.lblImporte.Size = New System.Drawing.Size(45, 13)
        Me.lblImporte.TabIndex = 84
        Me.lblImporte.Text = "Importe:"
        '
        'txtImporte
        '
        Me.txtImporte.Location = New System.Drawing.Point(177, 278)
        Me.txtImporte.Name = "txtImporte"
        Me.txtImporte.Size = New System.Drawing.Size(121, 20)
        Me.txtImporte.TabIndex = 12
        '
        'lblBancoOrigen
        '
        Me.lblBancoOrigen.AutoSize = True
        Me.lblBancoOrigen.Location = New System.Drawing.Point(6, 67)
        Me.lblBancoOrigen.Name = "lblBancoOrigen"
        Me.lblBancoOrigen.Size = New System.Drawing.Size(75, 13)
        Me.lblBancoOrigen.TabIndex = 82
        Me.lblBancoOrigen.Text = "Banco Origen:"
        '
        'cmbBancoOrigen
        '
        Me.cmbBancoOrigen.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbBancoOrigen.FormattingEnabled = True
        Me.cmbBancoOrigen.Location = New System.Drawing.Point(178, 64)
        Me.cmbBancoOrigen.Name = "cmbBancoOrigen"
        Me.cmbBancoOrigen.Size = New System.Drawing.Size(231, 21)
        Me.cmbBancoOrigen.TabIndex = 4
        '
        'lblCuentaDestino
        '
        Me.lblCuentaDestino.AutoSize = True
        Me.lblCuentaDestino.Location = New System.Drawing.Point(6, 147)
        Me.lblCuentaDestino.Name = "lblCuentaDestino"
        Me.lblCuentaDestino.Size = New System.Drawing.Size(83, 13)
        Me.lblCuentaDestino.TabIndex = 78
        Me.lblCuentaDestino.Text = "Cuenta Destino:"
        '
        'lblCuentaOrigen
        '
        Me.lblCuentaOrigen.AutoSize = True
        Me.lblCuentaOrigen.Location = New System.Drawing.Point(6, 94)
        Me.lblCuentaOrigen.Name = "lblCuentaOrigen"
        Me.lblCuentaOrigen.Size = New System.Drawing.Size(78, 13)
        Me.lblCuentaOrigen.TabIndex = 77
        Me.lblCuentaOrigen.Text = "Cuenta Origen:"
        '
        'cmbCuentaDestino
        '
        Me.cmbCuentaDestino.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbCuentaDestino.FormattingEnabled = True
        Me.cmbCuentaDestino.Location = New System.Drawing.Point(177, 144)
        Me.cmbCuentaDestino.Name = "cmbCuentaDestino"
        Me.cmbCuentaDestino.Size = New System.Drawing.Size(230, 21)
        Me.cmbCuentaDestino.TabIndex = 8
        '
        'cmbTipoIngreso
        '
        Me.cmbTipoIngreso.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbTipoIngreso.FormattingEnabled = True
        Me.cmbTipoIngreso.Location = New System.Drawing.Point(178, 37)
        Me.cmbTipoIngreso.Name = "cmbTipoIngreso"
        Me.cmbTipoIngreso.Size = New System.Drawing.Size(231, 21)
        Me.cmbTipoIngreso.TabIndex = 2
        '
        'lblTipoIngreso
        '
        Me.lblTipoIngreso.AutoSize = True
        Me.lblTipoIngreso.Location = New System.Drawing.Point(6, 40)
        Me.lblTipoIngreso.Name = "lblTipoIngreso"
        Me.lblTipoIngreso.Size = New System.Drawing.Size(84, 13)
        Me.lblTipoIngreso.TabIndex = 74
        Me.lblTipoIngreso.Text = "Tipo de Ingreso:"
        '
        'lblFecha
        '
        Me.lblFecha.AutoSize = True
        Me.lblFecha.Location = New System.Drawing.Point(6, 11)
        Me.lblFecha.Name = "lblFecha"
        Me.lblFecha.Size = New System.Drawing.Size(163, 13)
        Me.lblFecha.TabIndex = 73
        Me.lblFecha.Text = "Fecha en la que entró el Ingreso:"
        '
        'dtFechaIngreso
        '
        Me.dtFechaIngreso.Location = New System.Drawing.Point(178, 11)
        Me.dtFechaIngreso.Name = "dtFechaIngreso"
        Me.dtFechaIngreso.Size = New System.Drawing.Size(231, 20)
        Me.dtFechaIngreso.TabIndex = 1
        '
        'lblDescripcion
        '
        Me.lblDescripcion.AutoSize = True
        Me.lblDescripcion.Location = New System.Drawing.Point(5, 182)
        Me.lblDescripcion.Name = "lblDescripcion"
        Me.lblDescripcion.Size = New System.Drawing.Size(121, 13)
        Me.lblDescripcion.TabIndex = 63
        Me.lblDescripcion.Text = "Descripción del Ingreso:"
        '
        'txtDescripcionIngreso
        '
        Me.txtDescripcionIngreso.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtDescripcionIngreso.Location = New System.Drawing.Point(177, 179)
        Me.txtDescripcionIngreso.MaxLength = 500
        Me.txtDescripcionIngreso.Multiline = True
        Me.txtDescripcionIngreso.Name = "txtDescripcionIngreso"
        Me.txtDescripcionIngreso.Size = New System.Drawing.Size(294, 88)
        Me.txtDescripcionIngreso.TabIndex = 11
        '
        'lblReferenciaOrigen
        '
        Me.lblReferenciaOrigen.AutoSize = True
        Me.lblReferenciaOrigen.Location = New System.Drawing.Point(6, 120)
        Me.lblReferenciaOrigen.Name = "lblReferenciaOrigen"
        Me.lblReferenciaOrigen.Size = New System.Drawing.Size(62, 13)
        Me.lblReferenciaOrigen.TabIndex = 61
        Me.lblReferenciaOrigen.Text = "Referencia:"
        '
        'txtReferenciaOrigen
        '
        Me.txtReferenciaOrigen.Location = New System.Drawing.Point(178, 117)
        Me.txtReferenciaOrigen.MaxLength = 10
        Me.txtReferenciaOrigen.Name = "txtReferenciaOrigen"
        Me.txtReferenciaOrigen.Size = New System.Drawing.Size(230, 20)
        Me.txtReferenciaOrigen.TabIndex = 7
        '
        'btnGuardar
        '
        Me.btnGuardar.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnGuardar.Enabled = False
        Me.btnGuardar.Image = Global.Oversight.My.Resources.Resources.income24x24
        Me.btnGuardar.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnGuardar.Location = New System.Drawing.Point(350, 376)
        Me.btnGuardar.Name = "btnGuardar"
        Me.btnGuardar.Size = New System.Drawing.Size(121, 34)
        Me.btnGuardar.TabIndex = 15
        Me.btnGuardar.Text = "&Guardar Ingreso"
        Me.btnGuardar.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnGuardar.UseVisualStyleBackColor = True
        '
        'btnCancelar
        '
        Me.btnCancelar.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnCancelar.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnCancelar.Image = Global.Oversight.My.Resources.Resources.cancel24x24
        Me.btnCancelar.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnCancelar.Location = New System.Drawing.Point(243, 376)
        Me.btnCancelar.Name = "btnCancelar"
        Me.btnCancelar.Size = New System.Drawing.Size(101, 34)
        Me.btnCancelar.TabIndex = 14
        Me.btnCancelar.Text = "&Cancelar"
        Me.btnCancelar.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnCancelar.UseVisualStyleBackColor = True
        '
        'tcIngresos
        '
        Me.tcIngresos.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tcIngresos.Controls.Add(Me.tpIngreso)
        Me.tcIngresos.Controls.Add(Me.tpRelaciones)
        Me.tcIngresos.Location = New System.Drawing.Point(5, 5)
        Me.tcIngresos.Name = "tcIngresos"
        Me.tcIngresos.SelectedIndex = 0
        Me.tcIngresos.Size = New System.Drawing.Size(519, 460)
        Me.tcIngresos.TabIndex = 58
        '
        'tpIngreso
        '
        Me.tpIngreso.Controls.Add(Me.btnRevisiones)
        Me.tpIngreso.Controls.Add(Me.btnPersonas)
        Me.tpIngreso.Controls.Add(Me.lblFecha)
        Me.tpIngreso.Controls.Add(Me.txtNombrePersona)
        Me.tpIngreso.Controls.Add(Me.btnCancelar)
        Me.tpIngreso.Controls.Add(Me.lblPersona)
        Me.tpIngreso.Controls.Add(Me.btnGuardar)
        Me.tpIngreso.Controls.Add(Me.btnBancoOrigen)
        Me.tpIngreso.Controls.Add(Me.txtReferenciaOrigen)
        Me.tpIngreso.Controls.Add(Me.btnCuentaDestino)
        Me.tpIngreso.Controls.Add(Me.lblReferenciaOrigen)
        Me.tpIngreso.Controls.Add(Me.btnTipoIngreso)
        Me.tpIngreso.Controls.Add(Me.txtDescripcionIngreso)
        Me.tpIngreso.Controls.Add(Me.lblDescripcion)
        Me.tpIngreso.Controls.Add(Me.dtFechaIngreso)
        Me.tpIngreso.Controls.Add(Me.txtCuentaOrigen)
        Me.tpIngreso.Controls.Add(Me.lblTipoIngreso)
        Me.tpIngreso.Controls.Add(Me.lblImporte)
        Me.tpIngreso.Controls.Add(Me.cmbTipoIngreso)
        Me.tpIngreso.Controls.Add(Me.txtImporte)
        Me.tpIngreso.Controls.Add(Me.cmbCuentaDestino)
        Me.tpIngreso.Controls.Add(Me.lblBancoOrigen)
        Me.tpIngreso.Controls.Add(Me.lblCuentaOrigen)
        Me.tpIngreso.Controls.Add(Me.cmbBancoOrigen)
        Me.tpIngreso.Controls.Add(Me.lblCuentaDestino)
        Me.tpIngreso.Location = New System.Drawing.Point(4, 22)
        Me.tpIngreso.Name = "tpIngreso"
        Me.tpIngreso.Padding = New System.Windows.Forms.Padding(3)
        Me.tpIngreso.Size = New System.Drawing.Size(511, 434)
        Me.tpIngreso.TabIndex = 0
        Me.tpIngreso.Text = "Ingresos"
        Me.tpIngreso.UseVisualStyleBackColor = True
        '
        'tpRelaciones
        '
        Me.tpRelaciones.Controls.Add(Me.btnRevisionesRelaciones)
        Me.tpRelaciones.Controls.Add(Me.btnCancelarRelacion)
        Me.tpRelaciones.Controls.Add(Me.btnGuardarRelacion)
        Me.tpRelaciones.Controls.Add(Me.btnEliminarRelacion)
        Me.tpRelaciones.Controls.Add(Me.btnBuscarRelacion)
        Me.tpRelaciones.Controls.Add(Me.lblItemsYaRelacionados)
        Me.tpRelaciones.Controls.Add(Me.dgvConRelacion)
        Me.tpRelaciones.Controls.Add(Me.cmbTipoRelacion)
        Me.tpRelaciones.Location = New System.Drawing.Point(4, 22)
        Me.tpRelaciones.Name = "tpRelaciones"
        Me.tpRelaciones.Padding = New System.Windows.Forms.Padding(3)
        Me.tpRelaciones.Size = New System.Drawing.Size(511, 434)
        Me.tpRelaciones.TabIndex = 1
        Me.tpRelaciones.Text = "Relacionar A"
        Me.tpRelaciones.UseVisualStyleBackColor = True
        '
        'btnCancelarRelacion
        '
        Me.btnCancelarRelacion.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnCancelarRelacion.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnCancelarRelacion.Image = Global.Oversight.My.Resources.Resources.cancel24x24
        Me.btnCancelarRelacion.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnCancelarRelacion.Location = New System.Drawing.Point(243, 377)
        Me.btnCancelarRelacion.Name = "btnCancelarRelacion"
        Me.btnCancelarRelacion.Size = New System.Drawing.Size(101, 34)
        Me.btnCancelarRelacion.TabIndex = 31
        Me.btnCancelarRelacion.Text = "&Cancelar"
        Me.btnCancelarRelacion.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnCancelarRelacion.UseVisualStyleBackColor = True
        '
        'btnGuardarRelacion
        '
        Me.btnGuardarRelacion.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnGuardarRelacion.Enabled = False
        Me.btnGuardarRelacion.Image = Global.Oversight.My.Resources.Resources.income24x24
        Me.btnGuardarRelacion.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnGuardarRelacion.Location = New System.Drawing.Point(350, 377)
        Me.btnGuardarRelacion.Name = "btnGuardarRelacion"
        Me.btnGuardarRelacion.Size = New System.Drawing.Size(121, 34)
        Me.btnGuardarRelacion.TabIndex = 32
        Me.btnGuardarRelacion.Text = "&Guardar Ingreso"
        Me.btnGuardarRelacion.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnGuardarRelacion.UseVisualStyleBackColor = True
        '
        'btnEliminarRelacion
        '
        Me.btnEliminarRelacion.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnEliminarRelacion.Enabled = False
        Me.btnEliminarRelacion.Image = Global.Oversight.My.Resources.Resources.delete12x12
        Me.btnEliminarRelacion.Location = New System.Drawing.Point(474, 28)
        Me.btnEliminarRelacion.Name = "btnEliminarRelacion"
        Me.btnEliminarRelacion.Size = New System.Drawing.Size(28, 23)
        Me.btnEliminarRelacion.TabIndex = 28
        Me.btnEliminarRelacion.UseVisualStyleBackColor = True
        '
        'btnBuscarRelacion
        '
        Me.btnBuscarRelacion.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnBuscarRelacion.Enabled = False
        Me.btnBuscarRelacion.Image = Global.Oversight.My.Resources.Resources.search12x12
        Me.btnBuscarRelacion.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnBuscarRelacion.Location = New System.Drawing.Point(338, 203)
        Me.btnBuscarRelacion.Name = "btnBuscarRelacion"
        Me.btnBuscarRelacion.Size = New System.Drawing.Size(130, 23)
        Me.btnBuscarRelacion.TabIndex = 30
        Me.btnBuscarRelacion.Text = "Buscar y Relacionar"
        Me.btnBuscarRelacion.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnBuscarRelacion.UseVisualStyleBackColor = True
        '
        'lblItemsYaRelacionados
        '
        Me.lblItemsYaRelacionados.AutoSize = True
        Me.lblItemsYaRelacionados.Location = New System.Drawing.Point(6, 12)
        Me.lblItemsYaRelacionados.Name = "lblItemsYaRelacionados"
        Me.lblItemsYaRelacionados.Size = New System.Drawing.Size(112, 13)
        Me.lblItemsYaRelacionados.TabIndex = 25
        Me.lblItemsYaRelacionados.Text = "Ingreso relacionado a:"
        '
        'dgvConRelacion
        '
        Me.dgvConRelacion.AllowUserToAddRows = False
        Me.dgvConRelacion.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.dgvConRelacion.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvConRelacion.Location = New System.Drawing.Point(5, 28)
        Me.dgvConRelacion.MultiSelect = False
        Me.dgvConRelacion.Name = "dgvConRelacion"
        Me.dgvConRelacion.RowHeadersVisible = False
        Me.dgvConRelacion.Size = New System.Drawing.Size(463, 160)
        Me.dgvConRelacion.TabIndex = 27
        Me.dgvConRelacion.Visible = False
        '
        'cmbTipoRelacion
        '
        Me.cmbTipoRelacion.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.cmbTipoRelacion.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbTipoRelacion.FormattingEnabled = True
        Me.cmbTipoRelacion.Items.AddRange(New Object() {"Relacionar a Activo", "Relacionar a Proyecto"})
        Me.cmbTipoRelacion.Location = New System.Drawing.Point(5, 205)
        Me.cmbTipoRelacion.Name = "cmbTipoRelacion"
        Me.cmbTipoRelacion.Size = New System.Drawing.Size(327, 21)
        Me.cmbTipoRelacion.TabIndex = 29
        '
        'NotifyIcon1
        '
        Me.NotifyIcon1.Icon = CType(resources.GetObject("NotifyIcon1.Icon"), System.Drawing.Icon)
        Me.NotifyIcon1.Text = "Oversight"
        '
        'btnRevisiones
        '
        Me.btnRevisiones.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnRevisiones.Enabled = False
        Me.btnRevisiones.Image = Global.Oversight.My.Resources.Resources.yes24x24
        Me.btnRevisiones.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnRevisiones.Location = New System.Drawing.Point(9, 376)
        Me.btnRevisiones.Name = "btnRevisiones"
        Me.btnRevisiones.Size = New System.Drawing.Size(111, 34)
        Me.btnRevisiones.TabIndex = 91
        Me.btnRevisiones.Text = "Revisiones"
        Me.btnRevisiones.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnRevisiones.UseVisualStyleBackColor = True
        '
        'btnRevisionesRelaciones
        '
        Me.btnRevisionesRelaciones.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnRevisionesRelaciones.Enabled = False
        Me.btnRevisionesRelaciones.Image = Global.Oversight.My.Resources.Resources.yes24x24
        Me.btnRevisionesRelaciones.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnRevisionesRelaciones.Location = New System.Drawing.Point(5, 377)
        Me.btnRevisionesRelaciones.Name = "btnRevisionesRelaciones"
        Me.btnRevisionesRelaciones.Size = New System.Drawing.Size(111, 34)
        Me.btnRevisionesRelaciones.TabIndex = 92
        Me.btnRevisionesRelaciones.Text = "Revisiones"
        Me.btnRevisionesRelaciones.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnRevisionesRelaciones.UseVisualStyleBackColor = True
        '
        'AgregarIngreso
        '
        Me.AcceptButton = Me.btnGuardar
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.btnCancelar
        Me.ClientSize = New System.Drawing.Size(527, 467)
        Me.Controls.Add(Me.tcIngresos)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "AgregarIngreso"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Ingreso"
        Me.tcIngresos.ResumeLayout(False)
        Me.tpIngreso.ResumeLayout(False)
        Me.tpIngreso.PerformLayout()
        Me.tpRelaciones.ResumeLayout(False)
        Me.tpRelaciones.PerformLayout()
        CType(Me.dgvConRelacion, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents btnGuardar As System.Windows.Forms.Button
    Friend WithEvents btnCancelar As System.Windows.Forms.Button
    Friend WithEvents lblDescripcion As System.Windows.Forms.Label
    Friend WithEvents txtDescripcionIngreso As System.Windows.Forms.TextBox
    Friend WithEvents lblReferenciaOrigen As System.Windows.Forms.Label
    Friend WithEvents txtReferenciaOrigen As System.Windows.Forms.TextBox
    Friend WithEvents txtCuentaOrigen As System.Windows.Forms.TextBox
    Friend WithEvents lblImporte As System.Windows.Forms.Label
    Friend WithEvents txtImporte As System.Windows.Forms.TextBox
    Friend WithEvents lblBancoOrigen As System.Windows.Forms.Label
    Friend WithEvents cmbBancoOrigen As System.Windows.Forms.ComboBox
    Friend WithEvents lblCuentaDestino As System.Windows.Forms.Label
    Friend WithEvents lblCuentaOrigen As System.Windows.Forms.Label
    Friend WithEvents cmbCuentaDestino As System.Windows.Forms.ComboBox
    Friend WithEvents cmbTipoIngreso As System.Windows.Forms.ComboBox
    Friend WithEvents lblTipoIngreso As System.Windows.Forms.Label
    Friend WithEvents lblFecha As System.Windows.Forms.Label
    Friend WithEvents dtFechaIngreso As System.Windows.Forms.DateTimePicker
    Friend WithEvents btnTipoIngreso As System.Windows.Forms.Button
    Friend WithEvents btnBancoOrigen As System.Windows.Forms.Button
    Friend WithEvents btnCuentaDestino As System.Windows.Forms.Button
    Friend WithEvents btnPersonas As System.Windows.Forms.Button
    Friend WithEvents txtNombrePersona As System.Windows.Forms.TextBox
    Friend WithEvents lblPersona As System.Windows.Forms.Label
    Friend WithEvents tcIngresos As System.Windows.Forms.TabControl
    Friend WithEvents tpIngreso As System.Windows.Forms.TabPage
    Friend WithEvents tpRelaciones As System.Windows.Forms.TabPage
    Friend WithEvents btnCancelarRelacion As System.Windows.Forms.Button
    Friend WithEvents btnGuardarRelacion As System.Windows.Forms.Button
    Friend WithEvents btnEliminarRelacion As System.Windows.Forms.Button
    Friend WithEvents btnBuscarRelacion As System.Windows.Forms.Button
    Friend WithEvents lblItemsYaRelacionados As System.Windows.Forms.Label
    Friend WithEvents dgvConRelacion As System.Windows.Forms.DataGridView
    Friend WithEvents cmbTipoRelacion As System.Windows.Forms.ComboBox
    Friend WithEvents NotifyIcon1 As System.Windows.Forms.NotifyIcon
    Friend WithEvents btnRevisiones As System.Windows.Forms.Button
    Friend WithEvents btnRevisionesRelaciones As System.Windows.Forms.Button
End Class
