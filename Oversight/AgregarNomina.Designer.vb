<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class AgregarNomina
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(AgregarNomina))
        Me.NotifyIcon1 = New System.Windows.Forms.NotifyIcon(Me.components)
        Me.tcNominas = New System.Windows.Forms.TabControl
        Me.tpDatos = New System.Windows.Forms.TabPage
        Me.btnExportarAExcel = New System.Windows.Forms.Button
        Me.btnProyecto = New System.Windows.Forms.Button
        Me.cmbTipoNomina = New System.Windows.Forms.ComboBox
        Me.cmbFrecuencia = New System.Windows.Forms.ComboBox
        Me.lblTipoNomina = New System.Windows.Forms.Label
        Me.lblFrecuencia = New System.Windows.Forms.Label
        Me.btnPaso2 = New System.Windows.Forms.Button
        Me.btnRevisiones = New System.Windows.Forms.Button
        Me.btnPersona = New System.Windows.Forms.Button
        Me.btnCancelar = New System.Windows.Forms.Button
        Me.btnGuardar = New System.Windows.Forms.Button
        Me.lblDescripcionNomina = New System.Windows.Forms.Label
        Me.txtDescripcionNomina = New System.Windows.Forms.TextBox
        Me.lblCliente = New System.Windows.Forms.Label
        Me.txtCliente = New System.Windows.Forms.TextBox
        Me.lblProyecto = New System.Windows.Forms.Label
        Me.txtProyecto = New System.Windows.Forms.TextBox
        Me.lblHasta = New System.Windows.Forms.Label
        Me.lblDesde = New System.Windows.Forms.Label
        Me.dtFechaFinNomina = New System.Windows.Forms.DateTimePicker
        Me.dtFechaInicioNomina = New System.Windows.Forms.DateTimePicker
        Me.dtFechaNomina = New System.Windows.Forms.DateTimePicker
        Me.btnInsertarPersona = New System.Windows.Forms.Button
        Me.btnEliminarPersona = New System.Windows.Forms.Button
        Me.dgvNominas = New System.Windows.Forms.DataGridView
        Me.lblSupervisor = New System.Windows.Forms.Label
        Me.btnNuevaPersona = New System.Windows.Forms.Button
        Me.lblFechaNomina = New System.Windows.Forms.Label
        Me.txtSupervisor = New System.Windows.Forms.TextBox
        Me.lblTotalNomina = New System.Windows.Forms.Label
        Me.txtTotalNomina = New System.Windows.Forms.TextBox
        Me.tpPagos = New System.Windows.Forms.TabPage
        Me.btnRevisionesPagos = New System.Windows.Forms.Button
        Me.btnInsertarPago = New System.Windows.Forms.Button
        Me.lblRestanteAPagar = New System.Windows.Forms.Label
        Me.lblSumaPagos = New System.Windows.Forms.Label
        Me.txtMontoRestante = New System.Windows.Forms.TextBox
        Me.txtSumaPagos = New System.Windows.Forms.TextBox
        Me.lblPagos = New System.Windows.Forms.Label
        Me.dgvPagos = New System.Windows.Forms.DataGridView
        Me.btnEliminarPago = New System.Windows.Forms.Button
        Me.btnAgregarPago = New System.Windows.Forms.Button
        Me.btnCancelarPago = New System.Windows.Forms.Button
        Me.btnGuardarPago = New System.Windows.Forms.Button
        Me.msSaveFileDialog = New System.Windows.Forms.SaveFileDialog
        Me.txtTotalNominaSinDescuentos = New System.Windows.Forms.TextBox
        Me.lblTotalNominaSinDescuentos = New System.Windows.Forms.Label
        Me.tcNominas.SuspendLayout()
        Me.tpDatos.SuspendLayout()
        CType(Me.dgvNominas, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.tpPagos.SuspendLayout()
        CType(Me.dgvPagos, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'NotifyIcon1
        '
        Me.NotifyIcon1.Icon = CType(resources.GetObject("NotifyIcon1.Icon"), System.Drawing.Icon)
        Me.NotifyIcon1.Text = "Oversight"
        '
        'tcNominas
        '
        Me.tcNominas.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tcNominas.Controls.Add(Me.tpDatos)
        Me.tcNominas.Controls.Add(Me.tpPagos)
        Me.tcNominas.Location = New System.Drawing.Point(5, 4)
        Me.tcNominas.Name = "tcNominas"
        Me.tcNominas.SelectedIndex = 0
        Me.tcNominas.Size = New System.Drawing.Size(616, 561)
        Me.tcNominas.TabIndex = 18
        '
        'tpDatos
        '
        Me.tpDatos.Controls.Add(Me.lblTotalNominaSinDescuentos)
        Me.tpDatos.Controls.Add(Me.txtTotalNominaSinDescuentos)
        Me.tpDatos.Controls.Add(Me.btnExportarAExcel)
        Me.tpDatos.Controls.Add(Me.btnProyecto)
        Me.tpDatos.Controls.Add(Me.cmbTipoNomina)
        Me.tpDatos.Controls.Add(Me.cmbFrecuencia)
        Me.tpDatos.Controls.Add(Me.lblTipoNomina)
        Me.tpDatos.Controls.Add(Me.lblFrecuencia)
        Me.tpDatos.Controls.Add(Me.btnPaso2)
        Me.tpDatos.Controls.Add(Me.btnRevisiones)
        Me.tpDatos.Controls.Add(Me.btnPersona)
        Me.tpDatos.Controls.Add(Me.btnCancelar)
        Me.tpDatos.Controls.Add(Me.btnGuardar)
        Me.tpDatos.Controls.Add(Me.lblDescripcionNomina)
        Me.tpDatos.Controls.Add(Me.txtDescripcionNomina)
        Me.tpDatos.Controls.Add(Me.lblCliente)
        Me.tpDatos.Controls.Add(Me.txtCliente)
        Me.tpDatos.Controls.Add(Me.lblProyecto)
        Me.tpDatos.Controls.Add(Me.txtProyecto)
        Me.tpDatos.Controls.Add(Me.lblHasta)
        Me.tpDatos.Controls.Add(Me.lblDesde)
        Me.tpDatos.Controls.Add(Me.dtFechaFinNomina)
        Me.tpDatos.Controls.Add(Me.dtFechaInicioNomina)
        Me.tpDatos.Controls.Add(Me.dtFechaNomina)
        Me.tpDatos.Controls.Add(Me.btnInsertarPersona)
        Me.tpDatos.Controls.Add(Me.btnEliminarPersona)
        Me.tpDatos.Controls.Add(Me.dgvNominas)
        Me.tpDatos.Controls.Add(Me.lblSupervisor)
        Me.tpDatos.Controls.Add(Me.btnNuevaPersona)
        Me.tpDatos.Controls.Add(Me.lblFechaNomina)
        Me.tpDatos.Controls.Add(Me.txtSupervisor)
        Me.tpDatos.Controls.Add(Me.lblTotalNomina)
        Me.tpDatos.Controls.Add(Me.txtTotalNomina)
        Me.tpDatos.Location = New System.Drawing.Point(4, 22)
        Me.tpDatos.Name = "tpDatos"
        Me.tpDatos.Padding = New System.Windows.Forms.Padding(3)
        Me.tpDatos.Size = New System.Drawing.Size(608, 535)
        Me.tpDatos.TabIndex = 0
        Me.tpDatos.Text = "Nómina"
        Me.tpDatos.UseVisualStyleBackColor = True
        '
        'btnExportarAExcel
        '
        Me.btnExportarAExcel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnExportarAExcel.Enabled = False
        Me.btnExportarAExcel.Image = Global.Oversight.My.Resources.Resources.excel24x24
        Me.btnExportarAExcel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnExportarAExcel.Location = New System.Drawing.Point(10, 452)
        Me.btnExportarAExcel.Name = "btnExportarAExcel"
        Me.btnExportarAExcel.Size = New System.Drawing.Size(118, 34)
        Me.btnExportarAExcel.TabIndex = 15
        Me.btnExportarAExcel.Text = "Exportar a Excel"
        Me.btnExportarAExcel.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnExportarAExcel.UseVisualStyleBackColor = True
        '
        'btnProyecto
        '
        Me.btnProyecto.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnProyecto.Enabled = False
        Me.btnProyecto.Image = Global.Oversight.My.Resources.Resources.helmet12x12
        Me.btnProyecto.Location = New System.Drawing.Point(466, 115)
        Me.btnProyecto.Name = "btnProyecto"
        Me.btnProyecto.Size = New System.Drawing.Size(27, 23)
        Me.btnProyecto.TabIndex = 6
        Me.btnProyecto.UseVisualStyleBackColor = True
        '
        'cmbTipoNomina
        '
        Me.cmbTipoNomina.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbTipoNomina.FormattingEnabled = True
        Me.cmbTipoNomina.Items.AddRange(New Object() {"Proyecto", "Oficina", "Aserradero", "Monte"})
        Me.cmbTipoNomina.Location = New System.Drawing.Point(146, 90)
        Me.cmbTipoNomina.Name = "cmbTipoNomina"
        Me.cmbTipoNomina.Size = New System.Drawing.Size(232, 21)
        Me.cmbTipoNomina.TabIndex = 5
        '
        'cmbFrecuencia
        '
        Me.cmbFrecuencia.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbFrecuencia.FormattingEnabled = True
        Me.cmbFrecuencia.Items.AddRange(New Object() {"Semanal", "Catorcenal", "Quincenal"})
        Me.cmbFrecuencia.Location = New System.Drawing.Point(146, 11)
        Me.cmbFrecuencia.Name = "cmbFrecuencia"
        Me.cmbFrecuencia.Size = New System.Drawing.Size(232, 21)
        Me.cmbFrecuencia.TabIndex = 1
        '
        'lblTipoNomina
        '
        Me.lblTipoNomina.AutoSize = True
        Me.lblTipoNomina.Location = New System.Drawing.Point(55, 93)
        Me.lblTipoNomina.Name = "lblTipoNomina"
        Me.lblTipoNomina.Size = New System.Drawing.Size(85, 13)
        Me.lblTipoNomina.TabIndex = 116
        Me.lblTipoNomina.Text = "Tipo de Nómina:"
        '
        'lblFrecuencia
        '
        Me.lblFrecuencia.AutoSize = True
        Me.lblFrecuencia.Location = New System.Drawing.Point(23, 14)
        Me.lblFrecuencia.Name = "lblFrecuencia"
        Me.lblFrecuencia.Size = New System.Drawing.Size(117, 13)
        Me.lblFrecuencia.TabIndex = 113
        Me.lblFrecuencia.Text = "Frecuencia de Nómina:"
        '
        'btnPaso2
        '
        Me.btnPaso2.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnPaso2.Enabled = False
        Me.btnPaso2.Image = Global.Oversight.My.Resources.Resources.next24x24
        Me.btnPaso2.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnPaso2.Location = New System.Drawing.Point(134, 492)
        Me.btnPaso2.Name = "btnPaso2"
        Me.btnPaso2.Size = New System.Drawing.Size(111, 34)
        Me.btnPaso2.TabIndex = 16
        Me.btnPaso2.Text = "&Siguiente Paso"
        Me.btnPaso2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnPaso2.UseVisualStyleBackColor = True
        '
        'btnRevisiones
        '
        Me.btnRevisiones.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnRevisiones.Enabled = False
        Me.btnRevisiones.Image = Global.Oversight.My.Resources.Resources.yes24x24
        Me.btnRevisiones.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnRevisiones.Location = New System.Drawing.Point(10, 492)
        Me.btnRevisiones.Name = "btnRevisiones"
        Me.btnRevisiones.Size = New System.Drawing.Size(118, 34)
        Me.btnRevisiones.TabIndex = 17
        Me.btnRevisiones.Text = "Revisiones"
        Me.btnRevisiones.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnRevisiones.UseVisualStyleBackColor = True
        '
        'btnPersona
        '
        Me.btnPersona.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnPersona.Enabled = False
        Me.btnPersona.Image = Global.Oversight.My.Resources.Resources.clients12x12
        Me.btnPersona.Location = New System.Drawing.Point(466, 167)
        Me.btnPersona.Name = "btnPersona"
        Me.btnPersona.Size = New System.Drawing.Size(27, 23)
        Me.btnPersona.TabIndex = 7
        Me.btnPersona.UseVisualStyleBackColor = True
        '
        'btnCancelar
        '
        Me.btnCancelar.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnCancelar.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnCancelar.Image = Global.Oversight.My.Resources.Resources.cancel24x24
        Me.btnCancelar.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnCancelar.Location = New System.Drawing.Point(380, 492)
        Me.btnCancelar.Name = "btnCancelar"
        Me.btnCancelar.Size = New System.Drawing.Size(89, 34)
        Me.btnCancelar.TabIndex = 13
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
        Me.btnGuardar.Location = New System.Drawing.Point(475, 492)
        Me.btnGuardar.Name = "btnGuardar"
        Me.btnGuardar.Size = New System.Drawing.Size(122, 34)
        Me.btnGuardar.TabIndex = 14
        Me.btnGuardar.Text = "&Guardar Nómina"
        Me.btnGuardar.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnGuardar.UseVisualStyleBackColor = True
        '
        'lblDescripcionNomina
        '
        Me.lblDescripcionNomina.AutoSize = True
        Me.lblDescripcionNomina.Location = New System.Drawing.Point(9, 198)
        Me.lblDescripcionNomina.Name = "lblDescripcionNomina"
        Me.lblDescripcionNomina.Size = New System.Drawing.Size(131, 13)
        Me.lblDescripcionNomina.TabIndex = 108
        Me.lblDescripcionNomina.Text = "Descripción de la Nomina:"
        '
        'txtDescripcionNomina
        '
        Me.txtDescripcionNomina.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtDescripcionNomina.Enabled = False
        Me.txtDescripcionNomina.Location = New System.Drawing.Point(146, 195)
        Me.txtDescripcionNomina.MaxLength = 500
        Me.txtDescripcionNomina.Name = "txtDescripcionNomina"
        Me.txtDescripcionNomina.Size = New System.Drawing.Size(314, 20)
        Me.txtDescripcionNomina.TabIndex = 8
        '
        'lblCliente
        '
        Me.lblCliente.AutoSize = True
        Me.lblCliente.Location = New System.Drawing.Point(98, 146)
        Me.lblCliente.Name = "lblCliente"
        Me.lblCliente.Size = New System.Drawing.Size(42, 13)
        Me.lblCliente.TabIndex = 106
        Me.lblCliente.Text = "Cliente:"
        '
        'txtCliente
        '
        Me.txtCliente.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtCliente.Enabled = False
        Me.txtCliente.Location = New System.Drawing.Point(146, 143)
        Me.txtCliente.MaxLength = 500
        Me.txtCliente.Name = "txtCliente"
        Me.txtCliente.Size = New System.Drawing.Size(314, 20)
        Me.txtCliente.TabIndex = 0
        Me.txtCliente.TabStop = False
        '
        'lblProyecto
        '
        Me.lblProyecto.AutoSize = True
        Me.lblProyecto.Location = New System.Drawing.Point(88, 120)
        Me.lblProyecto.Name = "lblProyecto"
        Me.lblProyecto.Size = New System.Drawing.Size(52, 13)
        Me.lblProyecto.TabIndex = 104
        Me.lblProyecto.Text = "Proyecto:"
        '
        'txtProyecto
        '
        Me.txtProyecto.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtProyecto.Enabled = False
        Me.txtProyecto.Location = New System.Drawing.Point(146, 117)
        Me.txtProyecto.MaxLength = 500
        Me.txtProyecto.Name = "txtProyecto"
        Me.txtProyecto.Size = New System.Drawing.Size(314, 20)
        Me.txtProyecto.TabIndex = 0
        Me.txtProyecto.TabStop = False
        '
        'lblHasta
        '
        Me.lblHasta.AutoSize = True
        Me.lblHasta.Location = New System.Drawing.Point(352, 70)
        Me.lblHasta.Name = "lblHasta"
        Me.lblHasta.Size = New System.Drawing.Size(39, 13)
        Me.lblHasta.TabIndex = 102
        Me.lblHasta.Text = " hasta "
        '
        'lblDesde
        '
        Me.lblDesde.AutoSize = True
        Me.lblDesde.Location = New System.Drawing.Point(99, 70)
        Me.lblDesde.Name = "lblDesde"
        Me.lblDesde.Size = New System.Drawing.Size(41, 13)
        Me.lblDesde.TabIndex = 101
        Me.lblDesde.Text = "Desde "
        '
        'dtFechaFinNomina
        '
        Me.dtFechaFinNomina.Location = New System.Drawing.Point(397, 64)
        Me.dtFechaFinNomina.Name = "dtFechaFinNomina"
        Me.dtFechaFinNomina.Size = New System.Drawing.Size(200, 20)
        Me.dtFechaFinNomina.TabIndex = 4
        '
        'dtFechaInicioNomina
        '
        Me.dtFechaInicioNomina.Location = New System.Drawing.Point(146, 64)
        Me.dtFechaInicioNomina.Name = "dtFechaInicioNomina"
        Me.dtFechaInicioNomina.Size = New System.Drawing.Size(200, 20)
        Me.dtFechaInicioNomina.TabIndex = 3
        '
        'dtFechaNomina
        '
        Me.dtFechaNomina.Location = New System.Drawing.Point(146, 38)
        Me.dtFechaNomina.Name = "dtFechaNomina"
        Me.dtFechaNomina.Size = New System.Drawing.Size(200, 20)
        Me.dtFechaNomina.TabIndex = 2
        '
        'btnInsertarPersona
        '
        Me.btnInsertarPersona.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnInsertarPersona.Enabled = False
        Me.btnInsertarPersona.Image = Global.Oversight.My.Resources.Resources.insertcard12x12
        Me.btnInsertarPersona.Location = New System.Drawing.Point(569, 250)
        Me.btnInsertarPersona.Name = "btnInsertarPersona"
        Me.btnInsertarPersona.Size = New System.Drawing.Size(28, 23)
        Me.btnInsertarPersona.TabIndex = 10
        Me.btnInsertarPersona.UseVisualStyleBackColor = True
        '
        'btnEliminarPersona
        '
        Me.btnEliminarPersona.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnEliminarPersona.Enabled = False
        Me.btnEliminarPersona.Image = Global.Oversight.My.Resources.Resources.delete12x12
        Me.btnEliminarPersona.Location = New System.Drawing.Point(569, 279)
        Me.btnEliminarPersona.Name = "btnEliminarPersona"
        Me.btnEliminarPersona.Size = New System.Drawing.Size(28, 23)
        Me.btnEliminarPersona.TabIndex = 11
        Me.btnEliminarPersona.UseVisualStyleBackColor = True
        '
        'dgvNominas
        '
        Me.dgvNominas.AllowUserToAddRows = False
        Me.dgvNominas.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.dgvNominas.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvNominas.Location = New System.Drawing.Point(10, 221)
        Me.dgvNominas.MultiSelect = False
        Me.dgvNominas.Name = "dgvNominas"
        Me.dgvNominas.RowHeadersVisible = False
        Me.dgvNominas.Size = New System.Drawing.Size(553, 197)
        Me.dgvNominas.TabIndex = 12
        '
        'lblSupervisor
        '
        Me.lblSupervisor.AutoSize = True
        Me.lblSupervisor.Location = New System.Drawing.Point(80, 172)
        Me.lblSupervisor.Name = "lblSupervisor"
        Me.lblSupervisor.Size = New System.Drawing.Size(60, 13)
        Me.lblSupervisor.TabIndex = 96
        Me.lblSupervisor.Text = "Supervisor:"
        '
        'btnNuevaPersona
        '
        Me.btnNuevaPersona.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnNuevaPersona.Enabled = False
        Me.btnNuevaPersona.Image = Global.Oversight.My.Resources.Resources.newcard12x12
        Me.btnNuevaPersona.Location = New System.Drawing.Point(569, 221)
        Me.btnNuevaPersona.Name = "btnNuevaPersona"
        Me.btnNuevaPersona.Size = New System.Drawing.Size(28, 23)
        Me.btnNuevaPersona.TabIndex = 9
        Me.btnNuevaPersona.UseVisualStyleBackColor = True
        '
        'lblFechaNomina
        '
        Me.lblFechaNomina.AutoSize = True
        Me.lblFechaNomina.Location = New System.Drawing.Point(46, 44)
        Me.lblFechaNomina.Name = "lblFechaNomina"
        Me.lblFechaNomina.Size = New System.Drawing.Size(94, 13)
        Me.lblFechaNomina.TabIndex = 90
        Me.lblFechaNomina.Text = "Fecha de Nómina:"
        '
        'txtSupervisor
        '
        Me.txtSupervisor.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtSupervisor.Enabled = False
        Me.txtSupervisor.Location = New System.Drawing.Point(146, 169)
        Me.txtSupervisor.MaxLength = 500
        Me.txtSupervisor.Name = "txtSupervisor"
        Me.txtSupervisor.Size = New System.Drawing.Size(314, 20)
        Me.txtSupervisor.TabIndex = 0
        Me.txtSupervisor.TabStop = False
        '
        'lblTotalNomina
        '
        Me.lblTotalNomina.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblTotalNomina.AutoSize = True
        Me.lblTotalNomina.Location = New System.Drawing.Point(294, 427)
        Me.lblTotalNomina.Name = "lblTotalNomina"
        Me.lblTotalNomina.Size = New System.Drawing.Size(99, 13)
        Me.lblTotalNomina.TabIndex = 91
        Me.lblTotalNomina.Text = "Total de la Nómina:"
        '
        'txtTotalNomina
        '
        Me.txtTotalNomina.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtTotalNomina.Enabled = False
        Me.txtTotalNomina.Location = New System.Drawing.Point(399, 424)
        Me.txtTotalNomina.MaxLength = 1000
        Me.txtTotalNomina.Name = "txtTotalNomina"
        Me.txtTotalNomina.Size = New System.Drawing.Size(164, 20)
        Me.txtTotalNomina.TabIndex = 88
        Me.txtTotalNomina.TabStop = False
        '
        'tpPagos
        '
        Me.tpPagos.Controls.Add(Me.btnRevisionesPagos)
        Me.tpPagos.Controls.Add(Me.btnInsertarPago)
        Me.tpPagos.Controls.Add(Me.lblRestanteAPagar)
        Me.tpPagos.Controls.Add(Me.lblSumaPagos)
        Me.tpPagos.Controls.Add(Me.txtMontoRestante)
        Me.tpPagos.Controls.Add(Me.txtSumaPagos)
        Me.tpPagos.Controls.Add(Me.lblPagos)
        Me.tpPagos.Controls.Add(Me.dgvPagos)
        Me.tpPagos.Controls.Add(Me.btnEliminarPago)
        Me.tpPagos.Controls.Add(Me.btnAgregarPago)
        Me.tpPagos.Controls.Add(Me.btnCancelarPago)
        Me.tpPagos.Controls.Add(Me.btnGuardarPago)
        Me.tpPagos.Location = New System.Drawing.Point(4, 22)
        Me.tpPagos.Name = "tpPagos"
        Me.tpPagos.Padding = New System.Windows.Forms.Padding(3)
        Me.tpPagos.Size = New System.Drawing.Size(608, 535)
        Me.tpPagos.TabIndex = 1
        Me.tpPagos.Text = "Pagos"
        Me.tpPagos.UseVisualStyleBackColor = True
        '
        'btnRevisionesPagos
        '
        Me.btnRevisionesPagos.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnRevisionesPagos.Enabled = False
        Me.btnRevisionesPagos.Image = Global.Oversight.My.Resources.Resources.yes24x24
        Me.btnRevisionesPagos.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnRevisionesPagos.Location = New System.Drawing.Point(9, 327)
        Me.btnRevisionesPagos.Name = "btnRevisionesPagos"
        Me.btnRevisionesPagos.Size = New System.Drawing.Size(111, 34)
        Me.btnRevisionesPagos.TabIndex = 25
        Me.btnRevisionesPagos.Text = "Revisiones"
        Me.btnRevisionesPagos.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnRevisionesPagos.UseVisualStyleBackColor = True
        '
        'btnInsertarPago
        '
        Me.btnInsertarPago.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnInsertarPago.Enabled = False
        Me.btnInsertarPago.Image = Global.Oversight.My.Resources.Resources.insertcard12x12
        Me.btnInsertarPago.Location = New System.Drawing.Point(574, 59)
        Me.btnInsertarPago.Name = "btnInsertarPago"
        Me.btnInsertarPago.Size = New System.Drawing.Size(28, 23)
        Me.btnInsertarPago.TabIndex = 20
        Me.btnInsertarPago.UseVisualStyleBackColor = True
        '
        'lblRestanteAPagar
        '
        Me.lblRestanteAPagar.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblRestanteAPagar.AutoSize = True
        Me.lblRestanteAPagar.Location = New System.Drawing.Point(379, 225)
        Me.lblRestanteAPagar.Name = "lblRestanteAPagar"
        Me.lblRestanteAPagar.Size = New System.Drawing.Size(83, 13)
        Me.lblRestanteAPagar.TabIndex = 122
        Me.lblRestanteAPagar.Text = "Monto Restante"
        Me.lblRestanteAPagar.Visible = False
        '
        'lblSumaPagos
        '
        Me.lblSumaPagos.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblSumaPagos.AutoSize = True
        Me.lblSumaPagos.Location = New System.Drawing.Point(370, 199)
        Me.lblSumaPagos.Name = "lblSumaPagos"
        Me.lblSumaPagos.Size = New System.Drawing.Size(82, 13)
        Me.lblSumaPagos.TabIndex = 121
        Me.lblSumaPagos.Text = "Suma de Pagos"
        Me.lblSumaPagos.Visible = False
        '
        'txtMontoRestante
        '
        Me.txtMontoRestante.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtMontoRestante.Enabled = False
        Me.txtMontoRestante.Location = New System.Drawing.Point(468, 222)
        Me.txtMontoRestante.Name = "txtMontoRestante"
        Me.txtMontoRestante.Size = New System.Drawing.Size(100, 20)
        Me.txtMontoRestante.TabIndex = 112
        Me.txtMontoRestante.TabStop = False
        Me.txtMontoRestante.Visible = False
        '
        'txtSumaPagos
        '
        Me.txtSumaPagos.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtSumaPagos.Enabled = False
        Me.txtSumaPagos.Location = New System.Drawing.Point(468, 196)
        Me.txtSumaPagos.Name = "txtSumaPagos"
        Me.txtSumaPagos.Size = New System.Drawing.Size(100, 20)
        Me.txtSumaPagos.TabIndex = 111
        Me.txtSumaPagos.TabStop = False
        Me.txtSumaPagos.Visible = False
        '
        'lblPagos
        '
        Me.lblPagos.AutoSize = True
        Me.lblPagos.Location = New System.Drawing.Point(10, 10)
        Me.lblPagos.Name = "lblPagos"
        Me.lblPagos.Size = New System.Drawing.Size(159, 13)
        Me.lblPagos.TabIndex = 120
        Me.lblPagos.Text = "Pagos aplicables a esta nómina:"
        '
        'dgvPagos
        '
        Me.dgvPagos.AllowUserToAddRows = False
        Me.dgvPagos.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.dgvPagos.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvPagos.Enabled = False
        Me.dgvPagos.Location = New System.Drawing.Point(9, 30)
        Me.dgvPagos.MultiSelect = False
        Me.dgvPagos.Name = "dgvPagos"
        Me.dgvPagos.RowHeadersVisible = False
        Me.dgvPagos.Size = New System.Drawing.Size(559, 160)
        Me.dgvPagos.TabIndex = 22
        '
        'btnEliminarPago
        '
        Me.btnEliminarPago.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnEliminarPago.Enabled = False
        Me.btnEliminarPago.Image = Global.Oversight.My.Resources.Resources.delete12x12
        Me.btnEliminarPago.Location = New System.Drawing.Point(574, 88)
        Me.btnEliminarPago.Name = "btnEliminarPago"
        Me.btnEliminarPago.Size = New System.Drawing.Size(28, 23)
        Me.btnEliminarPago.TabIndex = 21
        Me.btnEliminarPago.UseVisualStyleBackColor = True
        '
        'btnAgregarPago
        '
        Me.btnAgregarPago.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnAgregarPago.Enabled = False
        Me.btnAgregarPago.Image = Global.Oversight.My.Resources.Resources.newcard12x12
        Me.btnAgregarPago.Location = New System.Drawing.Point(574, 30)
        Me.btnAgregarPago.Name = "btnAgregarPago"
        Me.btnAgregarPago.Size = New System.Drawing.Size(28, 23)
        Me.btnAgregarPago.TabIndex = 19
        Me.btnAgregarPago.UseVisualStyleBackColor = True
        '
        'btnCancelarPago
        '
        Me.btnCancelarPago.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnCancelarPago.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnCancelarPago.Image = Global.Oversight.My.Resources.Resources.cancel24x24
        Me.btnCancelarPago.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnCancelarPago.Location = New System.Drawing.Point(349, 327)
        Me.btnCancelarPago.Name = "btnCancelarPago"
        Me.btnCancelarPago.Size = New System.Drawing.Size(89, 34)
        Me.btnCancelarPago.TabIndex = 23
        Me.btnCancelarPago.Text = "&Cancelar"
        Me.btnCancelarPago.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnCancelarPago.UseVisualStyleBackColor = True
        '
        'btnGuardarPago
        '
        Me.btnGuardarPago.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnGuardarPago.Enabled = False
        Me.btnGuardarPago.Image = Global.Oversight.My.Resources.Resources.save24x24
        Me.btnGuardarPago.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnGuardarPago.Location = New System.Drawing.Point(444, 327)
        Me.btnGuardarPago.Name = "btnGuardarPago"
        Me.btnGuardarPago.Size = New System.Drawing.Size(124, 34)
        Me.btnGuardarPago.TabIndex = 24
        Me.btnGuardarPago.Text = "&Guardar Pagos"
        Me.btnGuardarPago.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnGuardarPago.UseVisualStyleBackColor = True
        '
        'txtTotalNominaSinDescuentos
        '
        Me.txtTotalNominaSinDescuentos.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtTotalNominaSinDescuentos.Enabled = False
        Me.txtTotalNominaSinDescuentos.Location = New System.Drawing.Point(399, 450)
        Me.txtTotalNominaSinDescuentos.MaxLength = 1000
        Me.txtTotalNominaSinDescuentos.Name = "txtTotalNominaSinDescuentos"
        Me.txtTotalNominaSinDescuentos.Size = New System.Drawing.Size(164, 20)
        Me.txtTotalNominaSinDescuentos.TabIndex = 117
        Me.txtTotalNominaSinDescuentos.TabStop = False
        '
        'lblTotalNominaSinDescuentos
        '
        Me.lblTotalNominaSinDescuentos.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblTotalNominaSinDescuentos.AutoSize = True
        Me.lblTotalNominaSinDescuentos.Location = New System.Drawing.Point(216, 453)
        Me.lblTotalNominaSinDescuentos.Name = "lblTotalNominaSinDescuentos"
        Me.lblTotalNominaSinDescuentos.Size = New System.Drawing.Size(175, 13)
        Me.lblTotalNominaSinDescuentos.TabIndex = 118
        Me.lblTotalNominaSinDescuentos.Text = "Total de la Nómina sin Descuentos:"
        '
        'AgregarNomina
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(626, 570)
        Me.Controls.Add(Me.tcNominas)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "AgregarNomina"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Nómina"
        Me.tcNominas.ResumeLayout(False)
        Me.tpDatos.ResumeLayout(False)
        Me.tpDatos.PerformLayout()
        CType(Me.dgvNominas, System.ComponentModel.ISupportInitialize).EndInit()
        Me.tpPagos.ResumeLayout(False)
        Me.tpPagos.PerformLayout()
        CType(Me.dgvPagos, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents NotifyIcon1 As System.Windows.Forms.NotifyIcon
    Friend WithEvents tcNominas As System.Windows.Forms.TabControl
    Friend WithEvents tpDatos As System.Windows.Forms.TabPage
    Friend WithEvents lblFrecuencia As System.Windows.Forms.Label
    Friend WithEvents btnPaso2 As System.Windows.Forms.Button
    Friend WithEvents btnRevisiones As System.Windows.Forms.Button
    Friend WithEvents btnPersona As System.Windows.Forms.Button
    Friend WithEvents btnCancelar As System.Windows.Forms.Button
    Friend WithEvents btnGuardar As System.Windows.Forms.Button
    Friend WithEvents lblDescripcionNomina As System.Windows.Forms.Label
    Friend WithEvents txtDescripcionNomina As System.Windows.Forms.TextBox
    Friend WithEvents lblCliente As System.Windows.Forms.Label
    Friend WithEvents txtCliente As System.Windows.Forms.TextBox
    Friend WithEvents lblProyecto As System.Windows.Forms.Label
    Friend WithEvents txtProyecto As System.Windows.Forms.TextBox
    Friend WithEvents lblHasta As System.Windows.Forms.Label
    Friend WithEvents lblDesde As System.Windows.Forms.Label
    Friend WithEvents dtFechaFinNomina As System.Windows.Forms.DateTimePicker
    Friend WithEvents dtFechaInicioNomina As System.Windows.Forms.DateTimePicker
    Friend WithEvents dtFechaNomina As System.Windows.Forms.DateTimePicker
    Friend WithEvents btnInsertarPersona As System.Windows.Forms.Button
    Friend WithEvents btnEliminarPersona As System.Windows.Forms.Button
    Friend WithEvents dgvNominas As System.Windows.Forms.DataGridView
    Friend WithEvents lblSupervisor As System.Windows.Forms.Label
    Friend WithEvents btnNuevaPersona As System.Windows.Forms.Button
    Friend WithEvents lblFechaNomina As System.Windows.Forms.Label
    Friend WithEvents txtSupervisor As System.Windows.Forms.TextBox
    Friend WithEvents lblTotalNomina As System.Windows.Forms.Label
    Friend WithEvents txtTotalNomina As System.Windows.Forms.TextBox
    Friend WithEvents tpPagos As System.Windows.Forms.TabPage
    Friend WithEvents lblTipoNomina As System.Windows.Forms.Label
    Friend WithEvents cmbFrecuencia As System.Windows.Forms.ComboBox
    Friend WithEvents cmbTipoNomina As System.Windows.Forms.ComboBox
    Friend WithEvents btnProyecto As System.Windows.Forms.Button
    Friend WithEvents btnRevisionesPagos As System.Windows.Forms.Button
    Friend WithEvents btnInsertarPago As System.Windows.Forms.Button
    Friend WithEvents lblRestanteAPagar As System.Windows.Forms.Label
    Friend WithEvents lblSumaPagos As System.Windows.Forms.Label
    Friend WithEvents txtMontoRestante As System.Windows.Forms.TextBox
    Friend WithEvents txtSumaPagos As System.Windows.Forms.TextBox
    Friend WithEvents lblPagos As System.Windows.Forms.Label
    Friend WithEvents dgvPagos As System.Windows.Forms.DataGridView
    Friend WithEvents btnEliminarPago As System.Windows.Forms.Button
    Friend WithEvents btnAgregarPago As System.Windows.Forms.Button
    Friend WithEvents btnCancelarPago As System.Windows.Forms.Button
    Friend WithEvents btnGuardarPago As System.Windows.Forms.Button
    Friend WithEvents btnExportarAExcel As System.Windows.Forms.Button
    Friend WithEvents msSaveFileDialog As System.Windows.Forms.SaveFileDialog
    Friend WithEvents lblTotalNominaSinDescuentos As System.Windows.Forms.Label
    Friend WithEvents txtTotalNominaSinDescuentos As System.Windows.Forms.TextBox
End Class
