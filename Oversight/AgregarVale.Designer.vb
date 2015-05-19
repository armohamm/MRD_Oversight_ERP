<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class AgregarVale
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(AgregarVale))
        Me.lblImporteVale = New System.Windows.Forms.Label
        Me.lblTipoGas = New System.Windows.Forms.Label
        Me.cmbTipoGas = New System.Windows.Forms.ComboBox
        Me.txtImporteVale = New System.Windows.Forms.TextBox
        Me.dtFecha = New System.Windows.Forms.DateTimePicker
        Me.lblFecha = New System.Windows.Forms.Label
        Me.btnPersona = New System.Windows.Forms.Button
        Me.btnDestino = New System.Windows.Forms.Button
        Me.lblPersona = New System.Windows.Forms.Label
        Me.txtPersona = New System.Windows.Forms.TextBox
        Me.lblLitros = New System.Windows.Forms.Label
        Me.txtLitros = New System.Windows.Forms.TextBox
        Me.lblDestino = New System.Windows.Forms.Label
        Me.lblKms = New System.Windows.Forms.Label
        Me.txtDestino = New System.Windows.Forms.TextBox
        Me.txtKms = New System.Windows.Forms.TextBox
        Me.txtAuto = New System.Windows.Forms.TextBox
        Me.btnAuto = New System.Windows.Forms.Button
        Me.lblCarro = New System.Windows.Forms.Label
        Me.lblComentarios = New System.Windows.Forms.Label
        Me.txtComentarios = New System.Windows.Forms.TextBox
        Me.btnGuardar = New System.Windows.Forms.Button
        Me.btnCancelar = New System.Windows.Forms.Button
        Me.NotifyIcon1 = New System.Windows.Forms.NotifyIcon(Me.components)
        Me.tcVales = New System.Windows.Forms.TabControl
        Me.tpDatos = New System.Windows.Forms.TabPage
        Me.lblFolio = New System.Windows.Forms.Label
        Me.txtFolio = New System.Windows.Forms.TextBox
        Me.btnRevisiones = New System.Windows.Forms.Button
        Me.tpRelaciones = New System.Windows.Forms.TabPage
        Me.btnRevisionesRelaciones = New System.Windows.Forms.Button
        Me.btnCancelarRelaciones = New System.Windows.Forms.Button
        Me.btnGuardarRelaciones = New System.Windows.Forms.Button
        Me.btnEliminarRelacion = New System.Windows.Forms.Button
        Me.btnBuscarRelacion = New System.Windows.Forms.Button
        Me.lblLitrosYaRelacionados = New System.Windows.Forms.Label
        Me.dgvDetalleSinRelacion = New System.Windows.Forms.DataGridView
        Me.dgvDetalleConRelacion = New System.Windows.Forms.DataGridView
        Me.lblLitrosSinRelacion = New System.Windows.Forms.Label
        Me.cmbTipoRelacion = New System.Windows.Forms.ComboBox
        Me.tcVales.SuspendLayout()
        Me.tpDatos.SuspendLayout()
        Me.tpRelaciones.SuspendLayout()
        CType(Me.dgvDetalleSinRelacion, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.dgvDetalleConRelacion, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'lblImporteVale
        '
        Me.lblImporteVale.AutoSize = True
        Me.lblImporteVale.Location = New System.Drawing.Point(6, 222)
        Me.lblImporteVale.Name = "lblImporteVale"
        Me.lblImporteVale.Size = New System.Drawing.Size(69, 13)
        Me.lblImporteVale.TabIndex = 90
        Me.lblImporteVale.Text = "Importe Vale:"
        '
        'lblTipoGas
        '
        Me.lblTipoGas.AutoSize = True
        Me.lblTipoGas.Location = New System.Drawing.Point(6, 193)
        Me.lblTipoGas.Name = "lblTipoGas"
        Me.lblTipoGas.Size = New System.Drawing.Size(53, 13)
        Me.lblTipoGas.TabIndex = 89
        Me.lblTipoGas.Text = "Tipo Gas:"
        '
        'cmbTipoGas
        '
        Me.cmbTipoGas.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbTipoGas.FormattingEnabled = True
        Me.cmbTipoGas.Location = New System.Drawing.Point(78, 190)
        Me.cmbTipoGas.Name = "cmbTipoGas"
        Me.cmbTipoGas.Size = New System.Drawing.Size(139, 21)
        Me.cmbTipoGas.TabIndex = 7
        '
        'txtImporteVale
        '
        Me.txtImporteVale.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtImporteVale.Location = New System.Drawing.Point(78, 219)
        Me.txtImporteVale.MaxLength = 500
        Me.txtImporteVale.Multiline = True
        Me.txtImporteVale.Name = "txtImporteVale"
        Me.txtImporteVale.Size = New System.Drawing.Size(200, 20)
        Me.txtImporteVale.TabIndex = 8
        '
        'dtFecha
        '
        Me.dtFecha.Location = New System.Drawing.Point(78, 14)
        Me.dtFecha.Name = "dtFecha"
        Me.dtFecha.Size = New System.Drawing.Size(200, 20)
        Me.dtFecha.TabIndex = 1
        '
        'lblFecha
        '
        Me.lblFecha.AutoSize = True
        Me.lblFecha.Location = New System.Drawing.Point(6, 14)
        Me.lblFecha.Name = "lblFecha"
        Me.lblFecha.Size = New System.Drawing.Size(40, 13)
        Me.lblFecha.TabIndex = 85
        Me.lblFecha.Text = "Fecha:"
        '
        'btnPersona
        '
        Me.btnPersona.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnPersona.Enabled = False
        Me.btnPersona.Image = Global.Oversight.My.Resources.Resources.clients12x12
        Me.btnPersona.Location = New System.Drawing.Point(284, 246)
        Me.btnPersona.Name = "btnPersona"
        Me.btnPersona.Size = New System.Drawing.Size(28, 23)
        Me.btnPersona.TabIndex = 9
        Me.btnPersona.UseVisualStyleBackColor = True
        '
        'btnDestino
        '
        Me.btnDestino.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnDestino.Enabled = False
        Me.btnDestino.Image = Global.Oversight.My.Resources.Resources.note12x12
        Me.btnDestino.Location = New System.Drawing.Point(284, 132)
        Me.btnDestino.Name = "btnDestino"
        Me.btnDestino.Size = New System.Drawing.Size(28, 23)
        Me.btnDestino.TabIndex = 5
        Me.btnDestino.UseVisualStyleBackColor = True
        '
        'lblPersona
        '
        Me.lblPersona.AutoSize = True
        Me.lblPersona.Location = New System.Drawing.Point(6, 251)
        Me.lblPersona.Name = "lblPersona"
        Me.lblPersona.Size = New System.Drawing.Size(49, 13)
        Me.lblPersona.TabIndex = 84
        Me.lblPersona.Text = "Persona:"
        '
        'txtPersona
        '
        Me.txtPersona.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtPersona.Enabled = False
        Me.txtPersona.Location = New System.Drawing.Point(78, 247)
        Me.txtPersona.MaxLength = 500
        Me.txtPersona.Multiline = True
        Me.txtPersona.Name = "txtPersona"
        Me.txtPersona.Size = New System.Drawing.Size(200, 20)
        Me.txtPersona.TabIndex = 8
        Me.txtPersona.TabStop = False
        '
        'lblLitros
        '
        Me.lblLitros.AutoSize = True
        Me.lblLitros.Location = New System.Drawing.Point(6, 165)
        Me.lblLitros.Name = "lblLitros"
        Me.lblLitros.Size = New System.Drawing.Size(24, 13)
        Me.lblLitros.TabIndex = 82
        Me.lblLitros.Text = "Lts:"
        '
        'txtLitros
        '
        Me.txtLitros.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtLitros.Location = New System.Drawing.Point(78, 162)
        Me.txtLitros.MaxLength = 500
        Me.txtLitros.Multiline = True
        Me.txtLitros.Name = "txtLitros"
        Me.txtLitros.Size = New System.Drawing.Size(200, 20)
        Me.txtLitros.TabIndex = 6
        '
        'lblDestino
        '
        Me.lblDestino.AutoSize = True
        Me.lblDestino.Location = New System.Drawing.Point(6, 137)
        Me.lblDestino.Name = "lblDestino"
        Me.lblDestino.Size = New System.Drawing.Size(46, 13)
        Me.lblDestino.TabIndex = 80
        Me.lblDestino.Text = "Destino:"
        '
        'lblKms
        '
        Me.lblKms.AutoSize = True
        Me.lblKms.Location = New System.Drawing.Point(6, 109)
        Me.lblKms.Name = "lblKms"
        Me.lblKms.Size = New System.Drawing.Size(25, 13)
        Me.lblKms.TabIndex = 79
        Me.lblKms.Text = "Km:"
        '
        'txtDestino
        '
        Me.txtDestino.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtDestino.Location = New System.Drawing.Point(78, 134)
        Me.txtDestino.MaxLength = 500
        Me.txtDestino.Multiline = True
        Me.txtDestino.Name = "txtDestino"
        Me.txtDestino.Size = New System.Drawing.Size(200, 20)
        Me.txtDestino.TabIndex = 4
        '
        'txtKms
        '
        Me.txtKms.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtKms.Location = New System.Drawing.Point(78, 106)
        Me.txtKms.MaxLength = 500
        Me.txtKms.Multiline = True
        Me.txtKms.Name = "txtKms"
        Me.txtKms.Size = New System.Drawing.Size(200, 20)
        Me.txtKms.TabIndex = 4
        '
        'txtAuto
        '
        Me.txtAuto.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtAuto.Enabled = False
        Me.txtAuto.Location = New System.Drawing.Point(78, 73)
        Me.txtAuto.Name = "txtAuto"
        Me.txtAuto.Size = New System.Drawing.Size(200, 20)
        Me.txtAuto.TabIndex = 2
        Me.txtAuto.TabStop = False
        '
        'btnAuto
        '
        Me.btnAuto.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnAuto.Enabled = False
        Me.btnAuto.Image = Global.Oversight.My.Resources.Resources.truckb24x24
        Me.btnAuto.Location = New System.Drawing.Point(284, 71)
        Me.btnAuto.Name = "btnAuto"
        Me.btnAuto.Size = New System.Drawing.Size(28, 23)
        Me.btnAuto.TabIndex = 3
        Me.btnAuto.UseVisualStyleBackColor = True
        '
        'lblCarro
        '
        Me.lblCarro.AutoSize = True
        Me.lblCarro.Location = New System.Drawing.Point(6, 76)
        Me.lblCarro.Name = "lblCarro"
        Me.lblCarro.Size = New System.Drawing.Size(32, 13)
        Me.lblCarro.TabIndex = 64
        Me.lblCarro.Text = "Auto:"
        '
        'lblComentarios
        '
        Me.lblComentarios.AutoSize = True
        Me.lblComentarios.Location = New System.Drawing.Point(7, 281)
        Me.lblComentarios.Name = "lblComentarios"
        Me.lblComentarios.Size = New System.Drawing.Size(68, 13)
        Me.lblComentarios.TabIndex = 63
        Me.lblComentarios.Text = "Comentarios:"
        '
        'txtComentarios
        '
        Me.txtComentarios.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtComentarios.Enabled = False
        Me.txtComentarios.Location = New System.Drawing.Point(78, 281)
        Me.txtComentarios.MaxLength = 500
        Me.txtComentarios.Multiline = True
        Me.txtComentarios.Name = "txtComentarios"
        Me.txtComentarios.Size = New System.Drawing.Size(362, 49)
        Me.txtComentarios.TabIndex = 10
        '
        'btnGuardar
        '
        Me.btnGuardar.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnGuardar.Enabled = False
        Me.btnGuardar.Image = Global.Oversight.My.Resources.Resources.creditcard24x24
        Me.btnGuardar.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnGuardar.Location = New System.Drawing.Point(316, 350)
        Me.btnGuardar.Name = "btnGuardar"
        Me.btnGuardar.Size = New System.Drawing.Size(124, 34)
        Me.btnGuardar.TabIndex = 12
        Me.btnGuardar.Text = "&Guardar Vale"
        Me.btnGuardar.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnGuardar.UseVisualStyleBackColor = True
        '
        'btnCancelar
        '
        Me.btnCancelar.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnCancelar.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnCancelar.Image = Global.Oversight.My.Resources.Resources.cancel24x24
        Me.btnCancelar.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnCancelar.Location = New System.Drawing.Point(209, 350)
        Me.btnCancelar.Name = "btnCancelar"
        Me.btnCancelar.Size = New System.Drawing.Size(101, 34)
        Me.btnCancelar.TabIndex = 11
        Me.btnCancelar.Text = "&Cancelar"
        Me.btnCancelar.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnCancelar.UseVisualStyleBackColor = True
        '
        'NotifyIcon1
        '
        Me.NotifyIcon1.Icon = CType(resources.GetObject("NotifyIcon1.Icon"), System.Drawing.Icon)
        Me.NotifyIcon1.Text = "Oversight"
        '
        'tcVales
        '
        Me.tcVales.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tcVales.Controls.Add(Me.tpDatos)
        Me.tcVales.Controls.Add(Me.tpRelaciones)
        Me.tcVales.Location = New System.Drawing.Point(5, 5)
        Me.tcVales.Name = "tcVales"
        Me.tcVales.SelectedIndex = 0
        Me.tcVales.Size = New System.Drawing.Size(462, 427)
        Me.tcVales.TabIndex = 14
        '
        'tpDatos
        '
        Me.tpDatos.Controls.Add(Me.lblFolio)
        Me.tpDatos.Controls.Add(Me.txtFolio)
        Me.tpDatos.Controls.Add(Me.btnRevisiones)
        Me.tpDatos.Controls.Add(Me.lblFecha)
        Me.tpDatos.Controls.Add(Me.lblImporteVale)
        Me.tpDatos.Controls.Add(Me.btnCancelar)
        Me.tpDatos.Controls.Add(Me.lblTipoGas)
        Me.tpDatos.Controls.Add(Me.btnGuardar)
        Me.tpDatos.Controls.Add(Me.cmbTipoGas)
        Me.tpDatos.Controls.Add(Me.txtComentarios)
        Me.tpDatos.Controls.Add(Me.txtImporteVale)
        Me.tpDatos.Controls.Add(Me.lblComentarios)
        Me.tpDatos.Controls.Add(Me.dtFecha)
        Me.tpDatos.Controls.Add(Me.lblCarro)
        Me.tpDatos.Controls.Add(Me.btnAuto)
        Me.tpDatos.Controls.Add(Me.btnPersona)
        Me.tpDatos.Controls.Add(Me.txtAuto)
        Me.tpDatos.Controls.Add(Me.btnDestino)
        Me.tpDatos.Controls.Add(Me.txtKms)
        Me.tpDatos.Controls.Add(Me.lblPersona)
        Me.tpDatos.Controls.Add(Me.txtDestino)
        Me.tpDatos.Controls.Add(Me.txtPersona)
        Me.tpDatos.Controls.Add(Me.lblKms)
        Me.tpDatos.Controls.Add(Me.lblLitros)
        Me.tpDatos.Controls.Add(Me.lblDestino)
        Me.tpDatos.Controls.Add(Me.txtLitros)
        Me.tpDatos.Location = New System.Drawing.Point(4, 22)
        Me.tpDatos.Name = "tpDatos"
        Me.tpDatos.Padding = New System.Windows.Forms.Padding(3)
        Me.tpDatos.Size = New System.Drawing.Size(454, 401)
        Me.tpDatos.TabIndex = 0
        Me.tpDatos.Text = "Datos"
        Me.tpDatos.UseVisualStyleBackColor = True
        '
        'lblFolio
        '
        Me.lblFolio.AutoSize = True
        Me.lblFolio.Location = New System.Drawing.Point(6, 50)
        Me.lblFolio.Name = "lblFolio"
        Me.lblFolio.Size = New System.Drawing.Size(32, 13)
        Me.lblFolio.TabIndex = 93
        Me.lblFolio.Text = "Folio:"
        '
        'txtFolio
        '
        Me.txtFolio.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtFolio.Location = New System.Drawing.Point(78, 47)
        Me.txtFolio.Name = "txtFolio"
        Me.txtFolio.Size = New System.Drawing.Size(95, 20)
        Me.txtFolio.TabIndex = 2
        '
        'btnRevisiones
        '
        Me.btnRevisiones.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnRevisiones.Enabled = False
        Me.btnRevisiones.Image = Global.Oversight.My.Resources.Resources.yes24x24
        Me.btnRevisiones.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnRevisiones.Location = New System.Drawing.Point(9, 350)
        Me.btnRevisiones.Name = "btnRevisiones"
        Me.btnRevisiones.Size = New System.Drawing.Size(111, 34)
        Me.btnRevisiones.TabIndex = 13
        Me.btnRevisiones.Text = "Revisiones"
        Me.btnRevisiones.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnRevisiones.UseVisualStyleBackColor = True
        '
        'tpRelaciones
        '
        Me.tpRelaciones.Controls.Add(Me.btnRevisionesRelaciones)
        Me.tpRelaciones.Controls.Add(Me.btnCancelarRelaciones)
        Me.tpRelaciones.Controls.Add(Me.btnGuardarRelaciones)
        Me.tpRelaciones.Controls.Add(Me.btnEliminarRelacion)
        Me.tpRelaciones.Controls.Add(Me.btnBuscarRelacion)
        Me.tpRelaciones.Controls.Add(Me.lblLitrosYaRelacionados)
        Me.tpRelaciones.Controls.Add(Me.dgvDetalleSinRelacion)
        Me.tpRelaciones.Controls.Add(Me.dgvDetalleConRelacion)
        Me.tpRelaciones.Controls.Add(Me.lblLitrosSinRelacion)
        Me.tpRelaciones.Controls.Add(Me.cmbTipoRelacion)
        Me.tpRelaciones.Location = New System.Drawing.Point(4, 22)
        Me.tpRelaciones.Name = "tpRelaciones"
        Me.tpRelaciones.Padding = New System.Windows.Forms.Padding(3)
        Me.tpRelaciones.Size = New System.Drawing.Size(454, 401)
        Me.tpRelaciones.TabIndex = 1
        Me.tpRelaciones.Text = "Relaciones"
        Me.tpRelaciones.UseVisualStyleBackColor = True
        '
        'btnRevisionesRelaciones
        '
        Me.btnRevisionesRelaciones.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnRevisionesRelaciones.Enabled = False
        Me.btnRevisionesRelaciones.Image = Global.Oversight.My.Resources.Resources.yes24x24
        Me.btnRevisionesRelaciones.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnRevisionesRelaciones.Location = New System.Drawing.Point(8, 335)
        Me.btnRevisionesRelaciones.Name = "btnRevisionesRelaciones"
        Me.btnRevisionesRelaciones.Size = New System.Drawing.Size(111, 34)
        Me.btnRevisionesRelaciones.TabIndex = 22
        Me.btnRevisionesRelaciones.Text = "Revisiones"
        Me.btnRevisionesRelaciones.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnRevisionesRelaciones.UseVisualStyleBackColor = True
        '
        'btnCancelarRelaciones
        '
        Me.btnCancelarRelaciones.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnCancelarRelaciones.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnCancelarRelaciones.Image = Global.Oversight.My.Resources.Resources.cancel24x24
        Me.btnCancelarRelaciones.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnCancelarRelaciones.Location = New System.Drawing.Point(199, 335)
        Me.btnCancelarRelaciones.Name = "btnCancelarRelaciones"
        Me.btnCancelarRelaciones.Size = New System.Drawing.Size(101, 34)
        Me.btnCancelarRelaciones.TabIndex = 20
        Me.btnCancelarRelaciones.Text = "&Cancelar"
        Me.btnCancelarRelaciones.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnCancelarRelaciones.UseVisualStyleBackColor = True
        '
        'btnGuardarRelaciones
        '
        Me.btnGuardarRelaciones.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnGuardarRelaciones.Enabled = False
        Me.btnGuardarRelaciones.Image = Global.Oversight.My.Resources.Resources.creditcard24x24
        Me.btnGuardarRelaciones.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnGuardarRelaciones.Location = New System.Drawing.Point(306, 335)
        Me.btnGuardarRelaciones.Name = "btnGuardarRelaciones"
        Me.btnGuardarRelaciones.Size = New System.Drawing.Size(141, 34)
        Me.btnGuardarRelaciones.TabIndex = 21
        Me.btnGuardarRelaciones.Text = "&Guardar Relaciones"
        Me.btnGuardarRelaciones.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnGuardarRelaciones.UseVisualStyleBackColor = True
        '
        'btnEliminarRelacion
        '
        Me.btnEliminarRelacion.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnEliminarRelacion.Enabled = False
        Me.btnEliminarRelacion.Image = Global.Oversight.My.Resources.Resources.delete12x12
        Me.btnEliminarRelacion.Location = New System.Drawing.Point(419, 197)
        Me.btnEliminarRelacion.Name = "btnEliminarRelacion"
        Me.btnEliminarRelacion.Size = New System.Drawing.Size(28, 23)
        Me.btnEliminarRelacion.TabIndex = 33
        Me.btnEliminarRelacion.UseVisualStyleBackColor = True
        '
        'btnBuscarRelacion
        '
        Me.btnBuscarRelacion.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnBuscarRelacion.Enabled = False
        Me.btnBuscarRelacion.Image = Global.Oversight.My.Resources.Resources.search12x12
        Me.btnBuscarRelacion.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnBuscarRelacion.Location = New System.Drawing.Point(219, 141)
        Me.btnBuscarRelacion.Name = "btnBuscarRelacion"
        Me.btnBuscarRelacion.Size = New System.Drawing.Size(130, 23)
        Me.btnBuscarRelacion.TabIndex = 18
        Me.btnBuscarRelacion.Text = "Buscar y Relacionar"
        Me.btnBuscarRelacion.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnBuscarRelacion.UseVisualStyleBackColor = True
        '
        'lblLitrosYaRelacionados
        '
        Me.lblLitrosYaRelacionados.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lblLitrosYaRelacionados.AutoSize = True
        Me.lblLitrosYaRelacionados.Location = New System.Drawing.Point(9, 181)
        Me.lblLitrosYaRelacionados.Name = "lblLitrosYaRelacionados"
        Me.lblLitrosYaRelacionados.Size = New System.Drawing.Size(115, 13)
        Me.lblLitrosYaRelacionados.TabIndex = 28
        Me.lblLitrosYaRelacionados.Text = "Litros YA relacionados:"
        '
        'dgvDetalleSinRelacion
        '
        Me.dgvDetalleSinRelacion.AllowUserToAddRows = False
        Me.dgvDetalleSinRelacion.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.dgvDetalleSinRelacion.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvDetalleSinRelacion.Location = New System.Drawing.Point(8, 27)
        Me.dgvDetalleSinRelacion.MultiSelect = False
        Me.dgvDetalleSinRelacion.Name = "dgvDetalleSinRelacion"
        Me.dgvDetalleSinRelacion.RowHeadersVisible = False
        Me.dgvDetalleSinRelacion.Size = New System.Drawing.Size(439, 92)
        Me.dgvDetalleSinRelacion.TabIndex = 16
        '
        'dgvDetalleConRelacion
        '
        Me.dgvDetalleConRelacion.AllowUserToAddRows = False
        Me.dgvDetalleConRelacion.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.dgvDetalleConRelacion.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvDetalleConRelacion.Location = New System.Drawing.Point(8, 197)
        Me.dgvDetalleConRelacion.MultiSelect = False
        Me.dgvDetalleConRelacion.Name = "dgvDetalleConRelacion"
        Me.dgvDetalleConRelacion.RowHeadersVisible = False
        Me.dgvDetalleConRelacion.Size = New System.Drawing.Size(405, 92)
        Me.dgvDetalleConRelacion.TabIndex = 19
        Me.dgvDetalleConRelacion.Visible = False
        '
        'lblLitrosSinRelacion
        '
        Me.lblLitrosSinRelacion.AutoSize = True
        Me.lblLitrosSinRelacion.Location = New System.Drawing.Point(9, 11)
        Me.lblLitrosSinRelacion.Name = "lblLitrosSinRelacion"
        Me.lblLitrosSinRelacion.Size = New System.Drawing.Size(183, 13)
        Me.lblLitrosSinRelacion.TabIndex = 27
        Me.lblLitrosSinRelacion.Text = "Litros del Vale SIN relación (todavía):"
        '
        'cmbTipoRelacion
        '
        Me.cmbTipoRelacion.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.cmbTipoRelacion.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbTipoRelacion.FormattingEnabled = True
        Me.cmbTipoRelacion.Items.AddRange(New Object() {"Relacionar a Proyecto"})
        Me.cmbTipoRelacion.Location = New System.Drawing.Point(8, 143)
        Me.cmbTipoRelacion.Name = "cmbTipoRelacion"
        Me.cmbTipoRelacion.Size = New System.Drawing.Size(205, 21)
        Me.cmbTipoRelacion.TabIndex = 17
        '
        'AgregarVale
        '
        Me.AcceptButton = Me.btnGuardar
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.btnCancelar
        Me.ClientSize = New System.Drawing.Size(471, 435)
        Me.Controls.Add(Me.tcVales)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "AgregarVale"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Vale de Gasolina"
        Me.tcVales.ResumeLayout(False)
        Me.tpDatos.ResumeLayout(False)
        Me.tpDatos.PerformLayout()
        Me.tpRelaciones.ResumeLayout(False)
        Me.tpRelaciones.PerformLayout()
        CType(Me.dgvDetalleSinRelacion, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.dgvDetalleConRelacion, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents btnGuardar As System.Windows.Forms.Button
    Friend WithEvents btnCancelar As System.Windows.Forms.Button
    Friend WithEvents lblComentarios As System.Windows.Forms.Label
    Friend WithEvents txtComentarios As System.Windows.Forms.TextBox
    Friend WithEvents lblCarro As System.Windows.Forms.Label
    Friend WithEvents btnAuto As System.Windows.Forms.Button
    Friend WithEvents btnPersona As System.Windows.Forms.Button
    Friend WithEvents btnDestino As System.Windows.Forms.Button
    Friend WithEvents lblPersona As System.Windows.Forms.Label
    Friend WithEvents txtPersona As System.Windows.Forms.TextBox
    Friend WithEvents lblLitros As System.Windows.Forms.Label
    Friend WithEvents txtLitros As System.Windows.Forms.TextBox
    Friend WithEvents lblDestino As System.Windows.Forms.Label
    Friend WithEvents lblKms As System.Windows.Forms.Label
    Friend WithEvents txtDestino As System.Windows.Forms.TextBox
    Friend WithEvents txtKms As System.Windows.Forms.TextBox
    Friend WithEvents txtAuto As System.Windows.Forms.TextBox
    Friend WithEvents dtFecha As System.Windows.Forms.DateTimePicker
    Friend WithEvents lblFecha As System.Windows.Forms.Label
    Friend WithEvents txtImporteVale As System.Windows.Forms.TextBox
    Friend WithEvents lblImporteVale As System.Windows.Forms.Label
    Friend WithEvents lblTipoGas As System.Windows.Forms.Label
    Friend WithEvents cmbTipoGas As System.Windows.Forms.ComboBox
    Friend WithEvents NotifyIcon1 As System.Windows.Forms.NotifyIcon
    Friend WithEvents tcVales As System.Windows.Forms.TabControl
    Friend WithEvents tpDatos As System.Windows.Forms.TabPage
    Friend WithEvents tpRelaciones As System.Windows.Forms.TabPage
    Friend WithEvents btnEliminarRelacion As System.Windows.Forms.Button
    Friend WithEvents btnBuscarRelacion As System.Windows.Forms.Button
    Friend WithEvents lblLitrosYaRelacionados As System.Windows.Forms.Label
    Friend WithEvents dgvDetalleSinRelacion As System.Windows.Forms.DataGridView
    Friend WithEvents dgvDetalleConRelacion As System.Windows.Forms.DataGridView
    Friend WithEvents lblLitrosSinRelacion As System.Windows.Forms.Label
    Friend WithEvents cmbTipoRelacion As System.Windows.Forms.ComboBox
    Friend WithEvents btnCancelarRelaciones As System.Windows.Forms.Button
    Friend WithEvents btnGuardarRelaciones As System.Windows.Forms.Button
    Friend WithEvents btnRevisiones As System.Windows.Forms.Button
    Friend WithEvents btnRevisionesRelaciones As System.Windows.Forms.Button
    Friend WithEvents lblFolio As System.Windows.Forms.Label
    Friend WithEvents txtFolio As System.Windows.Forms.TextBox
End Class
