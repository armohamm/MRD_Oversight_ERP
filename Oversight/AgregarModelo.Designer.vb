<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class AgregarModelo
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(AgregarModelo))
        Me.tcModelo = New System.Windows.Forms.TabControl
        Me.tpDatosIniciales = New System.Windows.Forms.TabPage
        Me.btnRevisiones = New System.Windows.Forms.Button
        Me.lblmts2 = New System.Windows.Forms.Label
        Me.lblmts1 = New System.Windows.Forms.Label
        Me.txtAnchoVivienda = New System.Windows.Forms.TextBox
        Me.lblAnchoVivienda = New System.Windows.Forms.Label
        Me.lblNombreDelModelo = New System.Windows.Forms.Label
        Me.txtNombreModelo = New System.Windows.Forms.TextBox
        Me.GroupBox6 = New System.Windows.Forms.GroupBox
        Me.lblArchivoPlanos = New System.Windows.Forms.Label
        Me.txtRuta = New System.Windows.Forms.TextBox
        Me.btnAbrirCarpeta = New System.Windows.Forms.Button
        Me.rbOtro = New System.Windows.Forms.RadioButton
        Me.rbOficina = New System.Windows.Forms.RadioButton
        Me.rbCasa = New System.Windows.Forms.RadioButton
        Me.lblLongitudVivienda = New System.Windows.Forms.Label
        Me.lblTipoConstruccion = New System.Windows.Forms.Label
        Me.lblFechaPresupuesto = New System.Windows.Forms.Label
        Me.txtLongitudVivienda = New System.Windows.Forms.TextBox
        Me.dtFechaModificacion = New System.Windows.Forms.DateTimePicker
        Me.btnPaso2 = New System.Windows.Forms.Button
        Me.tpCostosIndirectos = New System.Windows.Forms.TabPage
        Me.btnEliminarCostoIndirecto = New System.Windows.Forms.Button
        Me.btnNuevoCostoIndirecto = New System.Windows.Forms.Button
        Me.lblPorcentajeIndirectos = New System.Windows.Forms.Label
        Me.lblPorcentajeIndirectosLbl = New System.Windows.Forms.Label
        Me.txtIngresosIndirectos = New System.Windows.Forms.TextBox
        Me.lblIngresosIndirectos = New System.Windows.Forms.Label
        Me.lblTotalIndirectos = New System.Windows.Forms.Label
        Me.lblTotalIndirectosLbl = New System.Windows.Forms.Label
        Me.dgvCostosIndirectos = New System.Windows.Forms.DataGridView
        Me.btnPaso3 = New System.Windows.Forms.Button
        Me.tpResumenTarjetas = New System.Windows.Forms.TabPage
        Me.lblIndirectosTotal = New System.Windows.Forms.Label
        Me.lblIndirectosSubTotal = New System.Windows.Forms.Label
        Me.txtIndirectosTotal = New System.Windows.Forms.TextBox
        Me.txtIndirectosSubtotal = New System.Windows.Forms.TextBox
        Me.lblCostoProyectadoTotal = New System.Windows.Forms.Label
        Me.lblAsteriscoUtilidades = New System.Windows.Forms.Label
        Me.lblAsteriscoIndirectos = New System.Windows.Forms.Label
        Me.txtPorcentajeUtilidadDefault = New System.Windows.Forms.TextBox
        Me.lblPorcentajeUtilidadPorDefault = New System.Windows.Forms.Label
        Me.txtPorcentajeIndirectosDefault = New System.Windows.Forms.TextBox
        Me.lblPorcentajeIndirectosPorDefault = New System.Windows.Forms.Label
        Me.btnEliminarTarjeta = New System.Windows.Forms.Button
        Me.btnInsertarTarjeta = New System.Windows.Forms.Button
        Me.btnNuevaTarjeta = New System.Windows.Forms.Button
        Me.FlowLayoutPanel1 = New System.Windows.Forms.FlowLayoutPanel
        Me.btnCostoHoy = New System.Windows.Forms.Button
        Me.lblPercentageSignHelper = New System.Windows.Forms.Label
        Me.txtPorcentajeIVA = New System.Windows.Forms.TextBox
        Me.flpAccionesResumen = New System.Windows.Forms.FlowLayoutPanel
        Me.btnActualizarPrecios = New System.Windows.Forms.Button
        Me.btnActualizarUtilidad = New System.Windows.Forms.Button
        Me.btnGenerarArchivoExcel = New System.Windows.Forms.Button
        Me.lblIva = New System.Windows.Forms.Label
        Me.txtIVA = New System.Windows.Forms.TextBox
        Me.lblPrecioProyectadoTotal = New System.Windows.Forms.Label
        Me.txtPrecioProyectadoTotal = New System.Windows.Forms.TextBox
        Me.txtPrecioProyectadoSubTotal = New System.Windows.Forms.TextBox
        Me.dgvResumenDeTarjetas = New System.Windows.Forms.DataGridView
        Me.tpExplosionDeInsumos = New System.Windows.Forms.TabPage
        Me.tcInsumosObra = New System.Windows.Forms.TabControl
        Me.tpExplosion = New System.Windows.Forms.TabPage
        Me.dgvExplosionDeInsumos = New System.Windows.Forms.DataGridView
        Me.btnGenerarExplosion = New System.Windows.Forms.Button
        Me.msSaveFileDialog = New System.Windows.Forms.SaveFileDialog
        Me.btnGuardar = New System.Windows.Forms.Button
        Me.btnCancelar = New System.Windows.Forms.Button
        Me.btnGuardarYCerrar = New System.Windows.Forms.Button
        Me.NotifyIcon1 = New System.Windows.Forms.NotifyIcon(Me.components)
        Me.tcModelo.SuspendLayout()
        Me.tpDatosIniciales.SuspendLayout()
        Me.GroupBox6.SuspendLayout()
        Me.tpCostosIndirectos.SuspendLayout()
        CType(Me.dgvCostosIndirectos, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.tpResumenTarjetas.SuspendLayout()
        Me.FlowLayoutPanel1.SuspendLayout()
        Me.flpAccionesResumen.SuspendLayout()
        CType(Me.dgvResumenDeTarjetas, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.tpExplosionDeInsumos.SuspendLayout()
        Me.tcInsumosObra.SuspendLayout()
        Me.tpExplosion.SuspendLayout()
        CType(Me.dgvExplosionDeInsumos, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'tcModelo
        '
        Me.tcModelo.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tcModelo.Controls.Add(Me.tpDatosIniciales)
        Me.tcModelo.Controls.Add(Me.tpCostosIndirectos)
        Me.tcModelo.Controls.Add(Me.tpResumenTarjetas)
        Me.tcModelo.Controls.Add(Me.tpExplosionDeInsumos)
        Me.tcModelo.Location = New System.Drawing.Point(5, 7)
        Me.tcModelo.Name = "tcModelo"
        Me.tcModelo.SelectedIndex = 0
        Me.tcModelo.Size = New System.Drawing.Size(786, 422)
        Me.tcModelo.TabIndex = 18
        '
        'tpDatosIniciales
        '
        Me.tpDatosIniciales.Controls.Add(Me.btnRevisiones)
        Me.tpDatosIniciales.Controls.Add(Me.lblmts2)
        Me.tpDatosIniciales.Controls.Add(Me.lblmts1)
        Me.tpDatosIniciales.Controls.Add(Me.txtAnchoVivienda)
        Me.tpDatosIniciales.Controls.Add(Me.lblAnchoVivienda)
        Me.tpDatosIniciales.Controls.Add(Me.lblNombreDelModelo)
        Me.tpDatosIniciales.Controls.Add(Me.txtNombreModelo)
        Me.tpDatosIniciales.Controls.Add(Me.GroupBox6)
        Me.tpDatosIniciales.Controls.Add(Me.rbOtro)
        Me.tpDatosIniciales.Controls.Add(Me.rbOficina)
        Me.tpDatosIniciales.Controls.Add(Me.rbCasa)
        Me.tpDatosIniciales.Controls.Add(Me.lblLongitudVivienda)
        Me.tpDatosIniciales.Controls.Add(Me.lblTipoConstruccion)
        Me.tpDatosIniciales.Controls.Add(Me.lblFechaPresupuesto)
        Me.tpDatosIniciales.Controls.Add(Me.txtLongitudVivienda)
        Me.tpDatosIniciales.Controls.Add(Me.dtFechaModificacion)
        Me.tpDatosIniciales.Controls.Add(Me.btnPaso2)
        Me.tpDatosIniciales.Location = New System.Drawing.Point(4, 22)
        Me.tpDatosIniciales.Name = "tpDatosIniciales"
        Me.tpDatosIniciales.Padding = New System.Windows.Forms.Padding(3)
        Me.tpDatosIniciales.Size = New System.Drawing.Size(778, 396)
        Me.tpDatosIniciales.TabIndex = 0
        Me.tpDatosIniciales.Text = "Datos Iniciales"
        Me.tpDatosIniciales.UseVisualStyleBackColor = True
        '
        'btnRevisiones
        '
        Me.btnRevisiones.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnRevisiones.Enabled = False
        Me.btnRevisiones.Image = Global.Oversight.My.Resources.Resources.yes24x24
        Me.btnRevisiones.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnRevisiones.Location = New System.Drawing.Point(17, 352)
        Me.btnRevisiones.Name = "btnRevisiones"
        Me.btnRevisiones.Size = New System.Drawing.Size(111, 34)
        Me.btnRevisiones.TabIndex = 64
        Me.btnRevisiones.Text = "Revisiones"
        Me.btnRevisiones.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnRevisiones.UseVisualStyleBackColor = True
        '
        'lblmts2
        '
        Me.lblmts2.AutoSize = True
        Me.lblmts2.Location = New System.Drawing.Point(550, 93)
        Me.lblmts2.Name = "lblmts2"
        Me.lblmts2.Size = New System.Drawing.Size(23, 13)
        Me.lblmts2.TabIndex = 59
        Me.lblmts2.Text = "mts"
        '
        'lblmts1
        '
        Me.lblmts1.AutoSize = True
        Me.lblmts1.Location = New System.Drawing.Point(274, 93)
        Me.lblmts1.Name = "lblmts1"
        Me.lblmts1.Size = New System.Drawing.Size(23, 13)
        Me.lblmts1.TabIndex = 58
        Me.lblmts1.Text = "mts"
        '
        'txtAnchoVivienda
        '
        Me.txtAnchoVivienda.Enabled = False
        Me.txtAnchoVivienda.Location = New System.Drawing.Point(473, 90)
        Me.txtAnchoVivienda.MaxLength = 100
        Me.txtAnchoVivienda.Name = "txtAnchoVivienda"
        Me.txtAnchoVivienda.Size = New System.Drawing.Size(71, 20)
        Me.txtAnchoVivienda.TabIndex = 10
        Me.txtAnchoVivienda.Text = "1"
        '
        'lblAnchoVivienda
        '
        Me.lblAnchoVivienda.AutoSize = True
        Me.lblAnchoVivienda.Location = New System.Drawing.Point(317, 93)
        Me.lblAnchoVivienda.Name = "lblAnchoVivienda"
        Me.lblAnchoVivienda.Size = New System.Drawing.Size(150, 13)
        Me.lblAnchoVivienda.TabIndex = 56
        Me.lblAnchoVivienda.Text = "Ancho de la Vivienda (Fondo):"
        '
        'lblNombreDelModelo
        '
        Me.lblNombreDelModelo.AutoSize = True
        Me.lblNombreDelModelo.Location = New System.Drawing.Point(11, 41)
        Me.lblNombreDelModelo.Name = "lblNombreDelModelo"
        Me.lblNombreDelModelo.Size = New System.Drawing.Size(109, 13)
        Me.lblNombreDelModelo.TabIndex = 53
        Me.lblNombreDelModelo.Text = "Nombre del Proyecto:"
        '
        'txtNombreModelo
        '
        Me.txtNombreModelo.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtNombreModelo.Enabled = False
        Me.txtNombreModelo.Location = New System.Drawing.Point(197, 38)
        Me.txtNombreModelo.MaxLength = 200
        Me.txtNombreModelo.Name = "txtNombreModelo"
        Me.txtNombreModelo.Size = New System.Drawing.Size(547, 20)
        Me.txtNombreModelo.TabIndex = 2
        '
        'GroupBox6
        '
        Me.GroupBox6.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GroupBox6.Controls.Add(Me.lblArchivoPlanos)
        Me.GroupBox6.Controls.Add(Me.txtRuta)
        Me.GroupBox6.Controls.Add(Me.btnAbrirCarpeta)
        Me.GroupBox6.Location = New System.Drawing.Point(17, 138)
        Me.GroupBox6.Name = "GroupBox6"
        Me.GroupBox6.Size = New System.Drawing.Size(719, 71)
        Me.GroupBox6.TabIndex = 51
        Me.GroupBox6.TabStop = False
        Me.GroupBox6.Text = "Archivos de Proyecto:"
        '
        'lblArchivoPlanos
        '
        Me.lblArchivoPlanos.AutoSize = True
        Me.lblArchivoPlanos.Location = New System.Drawing.Point(19, 32)
        Me.lblArchivoPlanos.Name = "lblArchivoPlanos"
        Me.lblArchivoPlanos.Size = New System.Drawing.Size(33, 13)
        Me.lblArchivoPlanos.TabIndex = 46
        Me.lblArchivoPlanos.Text = "Ruta:"
        '
        'txtRuta
        '
        Me.txtRuta.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtRuta.Enabled = False
        Me.txtRuta.Location = New System.Drawing.Point(58, 29)
        Me.txtRuta.MaxLength = 1000
        Me.txtRuta.Name = "txtRuta"
        Me.txtRuta.Size = New System.Drawing.Size(622, 20)
        Me.txtRuta.TabIndex = 13
        '
        'btnAbrirCarpeta
        '
        Me.btnAbrirCarpeta.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnAbrirCarpeta.Enabled = False
        Me.btnAbrirCarpeta.Image = Global.Oversight.My.Resources.Resources.open12x12
        Me.btnAbrirCarpeta.Location = New System.Drawing.Point(686, 27)
        Me.btnAbrirCarpeta.Name = "btnAbrirCarpeta"
        Me.btnAbrirCarpeta.Size = New System.Drawing.Size(27, 23)
        Me.btnAbrirCarpeta.TabIndex = 16
        Me.btnAbrirCarpeta.UseVisualStyleBackColor = True
        '
        'rbOtro
        '
        Me.rbOtro.AutoSize = True
        Me.rbOtro.Enabled = False
        Me.rbOtro.Location = New System.Drawing.Point(402, 64)
        Me.rbOtro.Name = "rbOtro"
        Me.rbOtro.Size = New System.Drawing.Size(45, 17)
        Me.rbOtro.TabIndex = 8
        Me.rbOtro.Text = "Otro"
        Me.rbOtro.UseVisualStyleBackColor = True
        '
        'rbOficina
        '
        Me.rbOficina.AutoSize = True
        Me.rbOficina.Enabled = False
        Me.rbOficina.Location = New System.Drawing.Point(320, 64)
        Me.rbOficina.Name = "rbOficina"
        Me.rbOficina.Size = New System.Drawing.Size(58, 17)
        Me.rbOficina.TabIndex = 7
        Me.rbOficina.Text = "Oficina"
        Me.rbOficina.UseVisualStyleBackColor = True
        '
        'rbCasa
        '
        Me.rbCasa.AutoSize = True
        Me.rbCasa.Checked = True
        Me.rbCasa.Enabled = False
        Me.rbCasa.Location = New System.Drawing.Point(197, 64)
        Me.rbCasa.Name = "rbCasa"
        Me.rbCasa.Size = New System.Drawing.Size(103, 17)
        Me.rbCasa.TabIndex = 6
        Me.rbCasa.TabStop = True
        Me.rbCasa.Text = "Casa Habitación"
        Me.rbCasa.UseVisualStyleBackColor = True
        '
        'lblLongitudVivienda
        '
        Me.lblLongitudVivienda.AutoSize = True
        Me.lblLongitudVivienda.Location = New System.Drawing.Point(11, 93)
        Me.lblLongitudVivienda.Name = "lblLongitudVivienda"
        Me.lblLongitudVivienda.Size = New System.Drawing.Size(160, 13)
        Me.lblLongitudVivienda.TabIndex = 37
        Me.lblLongitudVivienda.Text = "Longitud de la Vivienda (Frente):"
        '
        'lblTipoConstruccion
        '
        Me.lblTipoConstruccion.AutoSize = True
        Me.lblTipoConstruccion.Location = New System.Drawing.Point(11, 66)
        Me.lblTipoConstruccion.Name = "lblTipoConstruccion"
        Me.lblTipoConstruccion.Size = New System.Drawing.Size(111, 13)
        Me.lblTipoConstruccion.TabIndex = 36
        Me.lblTipoConstruccion.Text = "Tipo de Construcción:"
        '
        'lblFechaPresupuesto
        '
        Me.lblFechaPresupuesto.AutoSize = True
        Me.lblFechaPresupuesto.Location = New System.Drawing.Point(11, 12)
        Me.lblFechaPresupuesto.Name = "lblFechaPresupuesto"
        Me.lblFechaPresupuesto.Size = New System.Drawing.Size(150, 13)
        Me.lblFechaPresupuesto.TabIndex = 34
        Me.lblFechaPresupuesto.Text = "Fecha de Ultima Modificacion:"
        '
        'txtLongitudVivienda
        '
        Me.txtLongitudVivienda.Enabled = False
        Me.txtLongitudVivienda.Location = New System.Drawing.Point(197, 90)
        Me.txtLongitudVivienda.MaxLength = 100
        Me.txtLongitudVivienda.Name = "txtLongitudVivienda"
        Me.txtLongitudVivienda.Size = New System.Drawing.Size(71, 20)
        Me.txtLongitudVivienda.TabIndex = 9
        Me.txtLongitudVivienda.Text = "1"
        '
        'dtFechaModificacion
        '
        Me.dtFechaModificacion.Enabled = False
        Me.dtFechaModificacion.Location = New System.Drawing.Point(197, 12)
        Me.dtFechaModificacion.Name = "dtFechaModificacion"
        Me.dtFechaModificacion.Size = New System.Drawing.Size(254, 20)
        Me.dtFechaModificacion.TabIndex = 1
        Me.dtFechaModificacion.TabStop = False
        '
        'btnPaso2
        '
        Me.btnPaso2.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnPaso2.Enabled = False
        Me.btnPaso2.Image = Global.Oversight.My.Resources.Resources.next24x24
        Me.btnPaso2.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnPaso2.Location = New System.Drawing.Point(633, 352)
        Me.btnPaso2.Name = "btnPaso2"
        Me.btnPaso2.Size = New System.Drawing.Size(111, 34)
        Me.btnPaso2.TabIndex = 17
        Me.btnPaso2.Text = "&Siguiente Paso"
        Me.btnPaso2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnPaso2.UseVisualStyleBackColor = True
        '
        'tpCostosIndirectos
        '
        Me.tpCostosIndirectos.Controls.Add(Me.btnEliminarCostoIndirecto)
        Me.tpCostosIndirectos.Controls.Add(Me.btnNuevoCostoIndirecto)
        Me.tpCostosIndirectos.Controls.Add(Me.lblPorcentajeIndirectos)
        Me.tpCostosIndirectos.Controls.Add(Me.lblPorcentajeIndirectosLbl)
        Me.tpCostosIndirectos.Controls.Add(Me.txtIngresosIndirectos)
        Me.tpCostosIndirectos.Controls.Add(Me.lblIngresosIndirectos)
        Me.tpCostosIndirectos.Controls.Add(Me.lblTotalIndirectos)
        Me.tpCostosIndirectos.Controls.Add(Me.lblTotalIndirectosLbl)
        Me.tpCostosIndirectos.Controls.Add(Me.dgvCostosIndirectos)
        Me.tpCostosIndirectos.Controls.Add(Me.btnPaso3)
        Me.tpCostosIndirectos.Location = New System.Drawing.Point(4, 22)
        Me.tpCostosIndirectos.Name = "tpCostosIndirectos"
        Me.tpCostosIndirectos.Size = New System.Drawing.Size(778, 396)
        Me.tpCostosIndirectos.TabIndex = 2
        Me.tpCostosIndirectos.Text = "Costos Indirectos"
        Me.tpCostosIndirectos.UseVisualStyleBackColor = True
        '
        'btnEliminarCostoIndirecto
        '
        Me.btnEliminarCostoIndirecto.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnEliminarCostoIndirecto.Enabled = False
        Me.btnEliminarCostoIndirecto.Image = Global.Oversight.My.Resources.Resources.delete12x12
        Me.btnEliminarCostoIndirecto.Location = New System.Drawing.Point(688, 32)
        Me.btnEliminarCostoIndirecto.Name = "btnEliminarCostoIndirecto"
        Me.btnEliminarCostoIndirecto.Size = New System.Drawing.Size(28, 23)
        Me.btnEliminarCostoIndirecto.TabIndex = 68
        Me.btnEliminarCostoIndirecto.UseVisualStyleBackColor = True
        '
        'btnNuevoCostoIndirecto
        '
        Me.btnNuevoCostoIndirecto.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnNuevoCostoIndirecto.Enabled = False
        Me.btnNuevoCostoIndirecto.Image = Global.Oversight.My.Resources.Resources.newcard12x12
        Me.btnNuevoCostoIndirecto.Location = New System.Drawing.Point(688, 3)
        Me.btnNuevoCostoIndirecto.Name = "btnNuevoCostoIndirecto"
        Me.btnNuevoCostoIndirecto.Size = New System.Drawing.Size(28, 23)
        Me.btnNuevoCostoIndirecto.TabIndex = 66
        Me.btnNuevoCostoIndirecto.UseVisualStyleBackColor = True
        '
        'lblPorcentajeIndirectos
        '
        Me.lblPorcentajeIndirectos.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lblPorcentajeIndirectos.AutoSize = True
        Me.lblPorcentajeIndirectos.Location = New System.Drawing.Point(186, 366)
        Me.lblPorcentajeIndirectos.MaximumSize = New System.Drawing.Size(113, 0)
        Me.lblPorcentajeIndirectos.MinimumSize = New System.Drawing.Size(113, 0)
        Me.lblPorcentajeIndirectos.Name = "lblPorcentajeIndirectos"
        Me.lblPorcentajeIndirectos.Size = New System.Drawing.Size(113, 13)
        Me.lblPorcentajeIndirectos.TabIndex = 57
        Me.lblPorcentajeIndirectos.Visible = False
        '
        'lblPorcentajeIndirectosLbl
        '
        Me.lblPorcentajeIndirectosLbl.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lblPorcentajeIndirectosLbl.AutoSize = True
        Me.lblPorcentajeIndirectosLbl.Location = New System.Drawing.Point(4, 365)
        Me.lblPorcentajeIndirectosLbl.Name = "lblPorcentajeIndirectosLbl"
        Me.lblPorcentajeIndirectosLbl.Size = New System.Drawing.Size(176, 13)
        Me.lblPorcentajeIndirectosLbl.TabIndex = 56
        Me.lblPorcentajeIndirectosLbl.Text = "Porcentaje de Indirectos Propuesto:"
        Me.lblPorcentajeIndirectosLbl.Visible = False
        '
        'txtIngresosIndirectos
        '
        Me.txtIngresosIndirectos.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.txtIngresosIndirectos.Location = New System.Drawing.Point(138, 343)
        Me.txtIngresosIndirectos.MaxLength = 20
        Me.txtIngresosIndirectos.Name = "txtIngresosIndirectos"
        Me.txtIngresosIndirectos.Size = New System.Drawing.Size(112, 20)
        Me.txtIngresosIndirectos.TabIndex = 20
        Me.txtIngresosIndirectos.Text = "0"
        Me.txtIngresosIndirectos.Visible = False
        '
        'lblIngresosIndirectos
        '
        Me.lblIngresosIndirectos.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lblIngresosIndirectos.AutoSize = True
        Me.lblIngresosIndirectos.Location = New System.Drawing.Point(4, 346)
        Me.lblIngresosIndirectos.Name = "lblIngresosIndirectos"
        Me.lblIngresosIndirectos.Size = New System.Drawing.Size(128, 13)
        Me.lblIngresosIndirectos.TabIndex = 54
        Me.lblIngresosIndirectos.Text = "Ingresos Totales al Mes : "
        Me.lblIngresosIndirectos.Visible = False
        '
        'lblTotalIndirectos
        '
        Me.lblTotalIndirectos.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lblTotalIndirectos.AutoSize = True
        Me.lblTotalIndirectos.Location = New System.Drawing.Point(50, 329)
        Me.lblTotalIndirectos.MaximumSize = New System.Drawing.Size(200, 0)
        Me.lblTotalIndirectos.MinimumSize = New System.Drawing.Size(200, 0)
        Me.lblTotalIndirectos.Name = "lblTotalIndirectos"
        Me.lblTotalIndirectos.Size = New System.Drawing.Size(200, 13)
        Me.lblTotalIndirectos.TabIndex = 53
        Me.lblTotalIndirectos.Visible = False
        '
        'lblTotalIndirectosLbl
        '
        Me.lblTotalIndirectosLbl.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lblTotalIndirectosLbl.AutoSize = True
        Me.lblTotalIndirectosLbl.Location = New System.Drawing.Point(4, 329)
        Me.lblTotalIndirectosLbl.Name = "lblTotalIndirectosLbl"
        Me.lblTotalIndirectosLbl.Size = New System.Drawing.Size(40, 13)
        Me.lblTotalIndirectosLbl.TabIndex = 52
        Me.lblTotalIndirectosLbl.Text = "Total : "
        Me.lblTotalIndirectosLbl.Visible = False
        '
        'dgvCostosIndirectos
        '
        Me.dgvCostosIndirectos.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.dgvCostosIndirectos.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvCostosIndirectos.Location = New System.Drawing.Point(3, 3)
        Me.dgvCostosIndirectos.MultiSelect = False
        Me.dgvCostosIndirectos.Name = "dgvCostosIndirectos"
        Me.dgvCostosIndirectos.RowHeadersVisible = False
        Me.dgvCostosIndirectos.Size = New System.Drawing.Size(679, 323)
        Me.dgvCostosIndirectos.TabIndex = 19
        Me.dgvCostosIndirectos.Visible = False
        '
        'btnPaso3
        '
        Me.btnPaso3.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnPaso3.Image = Global.Oversight.My.Resources.Resources.next24x24
        Me.btnPaso3.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnPaso3.Location = New System.Drawing.Point(571, 343)
        Me.btnPaso3.Name = "btnPaso3"
        Me.btnPaso3.Size = New System.Drawing.Size(111, 34)
        Me.btnPaso3.TabIndex = 21
        Me.btnPaso3.Text = "S&iguiente Paso"
        Me.btnPaso3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnPaso3.UseVisualStyleBackColor = True
        '
        'tpResumenTarjetas
        '
        Me.tpResumenTarjetas.Controls.Add(Me.lblIndirectosTotal)
        Me.tpResumenTarjetas.Controls.Add(Me.lblIndirectosSubTotal)
        Me.tpResumenTarjetas.Controls.Add(Me.txtIndirectosTotal)
        Me.tpResumenTarjetas.Controls.Add(Me.txtIndirectosSubtotal)
        Me.tpResumenTarjetas.Controls.Add(Me.lblCostoProyectadoTotal)
        Me.tpResumenTarjetas.Controls.Add(Me.lblAsteriscoUtilidades)
        Me.tpResumenTarjetas.Controls.Add(Me.lblAsteriscoIndirectos)
        Me.tpResumenTarjetas.Controls.Add(Me.txtPorcentajeUtilidadDefault)
        Me.tpResumenTarjetas.Controls.Add(Me.lblPorcentajeUtilidadPorDefault)
        Me.tpResumenTarjetas.Controls.Add(Me.txtPorcentajeIndirectosDefault)
        Me.tpResumenTarjetas.Controls.Add(Me.lblPorcentajeIndirectosPorDefault)
        Me.tpResumenTarjetas.Controls.Add(Me.btnEliminarTarjeta)
        Me.tpResumenTarjetas.Controls.Add(Me.btnInsertarTarjeta)
        Me.tpResumenTarjetas.Controls.Add(Me.btnNuevaTarjeta)
        Me.tpResumenTarjetas.Controls.Add(Me.FlowLayoutPanel1)
        Me.tpResumenTarjetas.Controls.Add(Me.lblPercentageSignHelper)
        Me.tpResumenTarjetas.Controls.Add(Me.txtPorcentajeIVA)
        Me.tpResumenTarjetas.Controls.Add(Me.flpAccionesResumen)
        Me.tpResumenTarjetas.Controls.Add(Me.lblIva)
        Me.tpResumenTarjetas.Controls.Add(Me.txtIVA)
        Me.tpResumenTarjetas.Controls.Add(Me.lblPrecioProyectadoTotal)
        Me.tpResumenTarjetas.Controls.Add(Me.txtPrecioProyectadoTotal)
        Me.tpResumenTarjetas.Controls.Add(Me.txtPrecioProyectadoSubTotal)
        Me.tpResumenTarjetas.Controls.Add(Me.dgvResumenDeTarjetas)
        Me.tpResumenTarjetas.Location = New System.Drawing.Point(4, 22)
        Me.tpResumenTarjetas.Name = "tpResumenTarjetas"
        Me.tpResumenTarjetas.Padding = New System.Windows.Forms.Padding(3)
        Me.tpResumenTarjetas.Size = New System.Drawing.Size(778, 396)
        Me.tpResumenTarjetas.TabIndex = 1
        Me.tpResumenTarjetas.Text = "Resumen de Tarjetas"
        Me.tpResumenTarjetas.UseVisualStyleBackColor = True
        '
        'lblIndirectosTotal
        '
        Me.lblIndirectosTotal.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblIndirectosTotal.AutoSize = True
        Me.lblIndirectosTotal.Location = New System.Drawing.Point(200, 260)
        Me.lblIndirectosTotal.Name = "lblIndirectosTotal"
        Me.lblIndirectosTotal.Size = New System.Drawing.Size(145, 13)
        Me.lblIndirectosTotal.TabIndex = 87
        Me.lblIndirectosTotal.Text = "Indirectos Proyectados Total:"
        '
        'lblIndirectosSubTotal
        '
        Me.lblIndirectosSubTotal.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblIndirectosSubTotal.AutoSize = True
        Me.lblIndirectosSubTotal.Location = New System.Drawing.Point(185, 234)
        Me.lblIndirectosSubTotal.Name = "lblIndirectosSubTotal"
        Me.lblIndirectosSubTotal.Size = New System.Drawing.Size(160, 13)
        Me.lblIndirectosSubTotal.TabIndex = 86
        Me.lblIndirectosSubTotal.Text = "Indirectos Proyectados Subtotal:"
        '
        'txtIndirectosTotal
        '
        Me.txtIndirectosTotal.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtIndirectosTotal.Enabled = False
        Me.txtIndirectosTotal.Location = New System.Drawing.Point(351, 257)
        Me.txtIndirectosTotal.MaxLength = 20
        Me.txtIndirectosTotal.Name = "txtIndirectosTotal"
        Me.txtIndirectosTotal.Size = New System.Drawing.Size(100, 20)
        Me.txtIndirectosTotal.TabIndex = 85
        Me.txtIndirectosTotal.TabStop = False
        '
        'txtIndirectosSubtotal
        '
        Me.txtIndirectosSubtotal.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtIndirectosSubtotal.Enabled = False
        Me.txtIndirectosSubtotal.Location = New System.Drawing.Point(351, 231)
        Me.txtIndirectosSubtotal.MaxLength = 20
        Me.txtIndirectosSubtotal.Name = "txtIndirectosSubtotal"
        Me.txtIndirectosSubtotal.Size = New System.Drawing.Size(100, 20)
        Me.txtIndirectosSubtotal.TabIndex = 84
        Me.txtIndirectosSubtotal.TabStop = False
        '
        'lblCostoProyectadoTotal
        '
        Me.lblCostoProyectadoTotal.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblCostoProyectadoTotal.AutoSize = True
        Me.lblCostoProyectadoTotal.Location = New System.Drawing.Point(457, 234)
        Me.lblCostoProyectadoTotal.Name = "lblCostoProyectadoTotal"
        Me.lblCostoProyectadoTotal.Size = New System.Drawing.Size(139, 13)
        Me.lblCostoProyectadoTotal.TabIndex = 41
        Me.lblCostoProyectadoTotal.Text = "Precio Proyectado Subtotal:"
        '
        'lblAsteriscoUtilidades
        '
        Me.lblAsteriscoUtilidades.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblAsteriscoUtilidades.AutoSize = True
        Me.lblAsteriscoUtilidades.Location = New System.Drawing.Point(457, 313)
        Me.lblAsteriscoUtilidades.Name = "lblAsteriscoUtilidades"
        Me.lblAsteriscoUtilidades.Size = New System.Drawing.Size(15, 13)
        Me.lblAsteriscoUtilidades.TabIndex = 83
        Me.lblAsteriscoUtilidades.Text = "%"
        '
        'lblAsteriscoIndirectos
        '
        Me.lblAsteriscoIndirectos.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblAsteriscoIndirectos.AutoSize = True
        Me.lblAsteriscoIndirectos.Location = New System.Drawing.Point(457, 286)
        Me.lblAsteriscoIndirectos.Name = "lblAsteriscoIndirectos"
        Me.lblAsteriscoIndirectos.Size = New System.Drawing.Size(15, 13)
        Me.lblAsteriscoIndirectos.TabIndex = 82
        Me.lblAsteriscoIndirectos.Text = "%"
        '
        'txtPorcentajeUtilidadDefault
        '
        Me.txtPorcentajeUtilidadDefault.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtPorcentajeUtilidadDefault.Enabled = False
        Me.txtPorcentajeUtilidadDefault.Location = New System.Drawing.Point(348, 309)
        Me.txtPorcentajeUtilidadDefault.MaxLength = 20
        Me.txtPorcentajeUtilidadDefault.Name = "txtPorcentajeUtilidadDefault"
        Me.txtPorcentajeUtilidadDefault.Size = New System.Drawing.Size(103, 20)
        Me.txtPorcentajeUtilidadDefault.TabIndex = 25
        Me.txtPorcentajeUtilidadDefault.Text = "0.00"
        '
        'lblPorcentajeUtilidadPorDefault
        '
        Me.lblPorcentajeUtilidadPorDefault.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblPorcentajeUtilidadPorDefault.AutoSize = True
        Me.lblPorcentajeUtilidadPorDefault.Location = New System.Drawing.Point(225, 312)
        Me.lblPorcentajeUtilidadPorDefault.Name = "lblPorcentajeUtilidadPorDefault"
        Me.lblPorcentajeUtilidadPorDefault.Size = New System.Drawing.Size(109, 13)
        Me.lblPorcentajeUtilidadPorDefault.TabIndex = 78
        Me.lblPorcentajeUtilidadPorDefault.Text = "% Utilidad por default:"
        '
        'txtPorcentajeIndirectosDefault
        '
        Me.txtPorcentajeIndirectosDefault.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtPorcentajeIndirectosDefault.Enabled = False
        Me.txtPorcentajeIndirectosDefault.Location = New System.Drawing.Point(348, 283)
        Me.txtPorcentajeIndirectosDefault.MaxLength = 20
        Me.txtPorcentajeIndirectosDefault.Name = "txtPorcentajeIndirectosDefault"
        Me.txtPorcentajeIndirectosDefault.Size = New System.Drawing.Size(103, 20)
        Me.txtPorcentajeIndirectosDefault.TabIndex = 24
        Me.txtPorcentajeIndirectosDefault.Text = "0.00"
        '
        'lblPorcentajeIndirectosPorDefault
        '
        Me.lblPorcentajeIndirectosPorDefault.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblPorcentajeIndirectosPorDefault.AutoSize = True
        Me.lblPorcentajeIndirectosPorDefault.Location = New System.Drawing.Point(225, 286)
        Me.lblPorcentajeIndirectosPorDefault.Name = "lblPorcentajeIndirectosPorDefault"
        Me.lblPorcentajeIndirectosPorDefault.Size = New System.Drawing.Size(120, 13)
        Me.lblPorcentajeIndirectosPorDefault.TabIndex = 77
        Me.lblPorcentajeIndirectosPorDefault.Text = "% Indirectos por default:"
        '
        'btnEliminarTarjeta
        '
        Me.btnEliminarTarjeta.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnEliminarTarjeta.Enabled = False
        Me.btnEliminarTarjeta.Image = Global.Oversight.My.Resources.Resources.delete12x12
        Me.btnEliminarTarjeta.Location = New System.Drawing.Point(716, 63)
        Me.btnEliminarTarjeta.Name = "btnEliminarTarjeta"
        Me.btnEliminarTarjeta.Size = New System.Drawing.Size(28, 23)
        Me.btnEliminarTarjeta.TabIndex = 65
        Me.btnEliminarTarjeta.UseVisualStyleBackColor = True
        '
        'btnInsertarTarjeta
        '
        Me.btnInsertarTarjeta.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnInsertarTarjeta.Enabled = False
        Me.btnInsertarTarjeta.Image = Global.Oversight.My.Resources.Resources.insertcard12x12
        Me.btnInsertarTarjeta.Location = New System.Drawing.Point(716, 34)
        Me.btnInsertarTarjeta.Name = "btnInsertarTarjeta"
        Me.btnInsertarTarjeta.Size = New System.Drawing.Size(28, 23)
        Me.btnInsertarTarjeta.TabIndex = 64
        Me.btnInsertarTarjeta.UseVisualStyleBackColor = True
        '
        'btnNuevaTarjeta
        '
        Me.btnNuevaTarjeta.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnNuevaTarjeta.Enabled = False
        Me.btnNuevaTarjeta.Image = Global.Oversight.My.Resources.Resources.newcard12x12
        Me.btnNuevaTarjeta.Location = New System.Drawing.Point(716, 5)
        Me.btnNuevaTarjeta.Name = "btnNuevaTarjeta"
        Me.btnNuevaTarjeta.Size = New System.Drawing.Size(28, 23)
        Me.btnNuevaTarjeta.TabIndex = 63
        Me.btnNuevaTarjeta.UseVisualStyleBackColor = True
        '
        'FlowLayoutPanel1
        '
        Me.FlowLayoutPanel1.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.FlowLayoutPanel1.Controls.Add(Me.btnCostoHoy)
        Me.FlowLayoutPanel1.Location = New System.Drawing.Point(570, 341)
        Me.FlowLayoutPanel1.Name = "FlowLayoutPanel1"
        Me.FlowLayoutPanel1.Size = New System.Drawing.Size(140, 40)
        Me.FlowLayoutPanel1.TabIndex = 62
        '
        'btnCostoHoy
        '
        Me.btnCostoHoy.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnCostoHoy.Enabled = False
        Me.btnCostoHoy.Image = Global.Oversight.My.Resources.Resources.question24x24
        Me.btnCostoHoy.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnCostoHoy.Location = New System.Drawing.Point(3, 3)
        Me.btnCostoHoy.Name = "btnCostoHoy"
        Me.btnCostoHoy.Size = New System.Drawing.Size(134, 34)
        Me.btnCostoHoy.TabIndex = 31
        Me.btnCostoHoy.Text = "&Si se hiciera hoy..."
        Me.btnCostoHoy.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnCostoHoy.UseVisualStyleBackColor = True
        '
        'lblPercentageSignHelper
        '
        Me.lblPercentageSignHelper.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblPercentageSignHelper.AutoSize = True
        Me.lblPercentageSignHelper.Location = New System.Drawing.Point(581, 260)
        Me.lblPercentageSignHelper.Name = "lblPercentageSignHelper"
        Me.lblPercentageSignHelper.Size = New System.Drawing.Size(15, 13)
        Me.lblPercentageSignHelper.TabIndex = 61
        Me.lblPercentageSignHelper.Text = "%"
        '
        'txtPorcentajeIVA
        '
        Me.txtPorcentajeIVA.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtPorcentajeIVA.Enabled = False
        Me.txtPorcentajeIVA.Location = New System.Drawing.Point(534, 257)
        Me.txtPorcentajeIVA.MaxLength = 20
        Me.txtPorcentajeIVA.Name = "txtPorcentajeIVA"
        Me.txtPorcentajeIVA.Size = New System.Drawing.Size(41, 20)
        Me.txtPorcentajeIVA.TabIndex = 22
        Me.txtPorcentajeIVA.Text = "0.00"
        '
        'flpAccionesResumen
        '
        Me.flpAccionesResumen.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.flpAccionesResumen.Controls.Add(Me.btnActualizarPrecios)
        Me.flpAccionesResumen.Controls.Add(Me.btnActualizarUtilidad)
        Me.flpAccionesResumen.Controls.Add(Me.btnGenerarArchivoExcel)
        Me.flpAccionesResumen.Location = New System.Drawing.Point(5, 231)
        Me.flpAccionesResumen.Name = "flpAccionesResumen"
        Me.flpAccionesResumen.Size = New System.Drawing.Size(170, 159)
        Me.flpAccionesResumen.TabIndex = 58
        '
        'btnActualizarPrecios
        '
        Me.btnActualizarPrecios.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnActualizarPrecios.Enabled = False
        Me.btnActualizarPrecios.Image = Global.Oversight.My.Resources.Resources.money24x24
        Me.btnActualizarPrecios.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnActualizarPrecios.Location = New System.Drawing.Point(3, 3)
        Me.btnActualizarPrecios.Name = "btnActualizarPrecios"
        Me.btnActualizarPrecios.Size = New System.Drawing.Size(162, 34)
        Me.btnActualizarPrecios.TabIndex = 26
        Me.btnActualizarPrecios.Text = "&Actualizar Precios"
        Me.btnActualizarPrecios.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnActualizarPrecios.UseVisualStyleBackColor = True
        '
        'btnActualizarUtilidad
        '
        Me.btnActualizarUtilidad.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnActualizarUtilidad.Enabled = False
        Me.btnActualizarUtilidad.Image = Global.Oversight.My.Resources.Resources.percent24x24
        Me.btnActualizarUtilidad.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnActualizarUtilidad.Location = New System.Drawing.Point(3, 43)
        Me.btnActualizarUtilidad.Name = "btnActualizarUtilidad"
        Me.btnActualizarUtilidad.Size = New System.Drawing.Size(162, 34)
        Me.btnActualizarUtilidad.TabIndex = 27
        Me.btnActualizarUtilidad.Text = "Cambiar U&tilidad e Ind"
        Me.btnActualizarUtilidad.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnActualizarUtilidad.UseVisualStyleBackColor = True
        '
        'btnGenerarArchivoExcel
        '
        Me.btnGenerarArchivoExcel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnGenerarArchivoExcel.Enabled = False
        Me.btnGenerarArchivoExcel.Image = Global.Oversight.My.Resources.Resources.excel24x24
        Me.btnGenerarArchivoExcel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnGenerarArchivoExcel.Location = New System.Drawing.Point(3, 83)
        Me.btnGenerarArchivoExcel.Name = "btnGenerarArchivoExcel"
        Me.btnGenerarArchivoExcel.Size = New System.Drawing.Size(162, 34)
        Me.btnGenerarArchivoExcel.TabIndex = 29
        Me.btnGenerarArchivoExcel.Text = "Generar Archivo &Excel"
        Me.btnGenerarArchivoExcel.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnGenerarArchivoExcel.UseVisualStyleBackColor = True
        '
        'lblIva
        '
        Me.lblIva.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblIva.AutoSize = True
        Me.lblIva.Location = New System.Drawing.Point(501, 260)
        Me.lblIva.Name = "lblIva"
        Me.lblIva.Size = New System.Drawing.Size(27, 13)
        Me.lblIva.TabIndex = 57
        Me.lblIva.Text = "IVA:"
        '
        'txtIVA
        '
        Me.txtIVA.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtIVA.Enabled = False
        Me.txtIVA.Location = New System.Drawing.Point(602, 257)
        Me.txtIVA.MaxLength = 20
        Me.txtIVA.Name = "txtIVA"
        Me.txtIVA.Size = New System.Drawing.Size(108, 20)
        Me.txtIVA.TabIndex = 27
        Me.txtIVA.TabStop = False
        '
        'lblPrecioProyectadoTotal
        '
        Me.lblPrecioProyectadoTotal.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblPrecioProyectadoTotal.AutoSize = True
        Me.lblPrecioProyectadoTotal.Location = New System.Drawing.Point(472, 286)
        Me.lblPrecioProyectadoTotal.MaximumSize = New System.Drawing.Size(130, 0)
        Me.lblPrecioProyectadoTotal.Name = "lblPrecioProyectadoTotal"
        Me.lblPrecioProyectadoTotal.Size = New System.Drawing.Size(124, 13)
        Me.lblPrecioProyectadoTotal.TabIndex = 43
        Me.lblPrecioProyectadoTotal.Text = "Precio Proyectado Total:"
        '
        'txtPrecioProyectadoTotal
        '
        Me.txtPrecioProyectadoTotal.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtPrecioProyectadoTotal.Enabled = False
        Me.txtPrecioProyectadoTotal.Location = New System.Drawing.Point(602, 283)
        Me.txtPrecioProyectadoTotal.MaxLength = 20
        Me.txtPrecioProyectadoTotal.Name = "txtPrecioProyectadoTotal"
        Me.txtPrecioProyectadoTotal.Size = New System.Drawing.Size(108, 20)
        Me.txtPrecioProyectadoTotal.TabIndex = 28
        Me.txtPrecioProyectadoTotal.TabStop = False
        '
        'txtPrecioProyectadoSubTotal
        '
        Me.txtPrecioProyectadoSubTotal.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtPrecioProyectadoSubTotal.Enabled = False
        Me.txtPrecioProyectadoSubTotal.Location = New System.Drawing.Point(602, 231)
        Me.txtPrecioProyectadoSubTotal.MaxLength = 20
        Me.txtPrecioProyectadoSubTotal.Name = "txtPrecioProyectadoSubTotal"
        Me.txtPrecioProyectadoSubTotal.Size = New System.Drawing.Size(108, 20)
        Me.txtPrecioProyectadoSubTotal.TabIndex = 26
        Me.txtPrecioProyectadoSubTotal.TabStop = False
        '
        'dgvResumenDeTarjetas
        '
        Me.dgvResumenDeTarjetas.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.dgvResumenDeTarjetas.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvResumenDeTarjetas.Location = New System.Drawing.Point(5, 6)
        Me.dgvResumenDeTarjetas.MultiSelect = False
        Me.dgvResumenDeTarjetas.Name = "dgvResumenDeTarjetas"
        Me.dgvResumenDeTarjetas.RowHeadersVisible = False
        Me.dgvResumenDeTarjetas.Size = New System.Drawing.Size(705, 219)
        Me.dgvResumenDeTarjetas.TabIndex = 23
        Me.dgvResumenDeTarjetas.Visible = False
        '
        'tpExplosionDeInsumos
        '
        Me.tpExplosionDeInsumos.Controls.Add(Me.tcInsumosObra)
        Me.tpExplosionDeInsumos.Controls.Add(Me.btnGenerarExplosion)
        Me.tpExplosionDeInsumos.Location = New System.Drawing.Point(4, 22)
        Me.tpExplosionDeInsumos.Name = "tpExplosionDeInsumos"
        Me.tpExplosionDeInsumos.Size = New System.Drawing.Size(778, 396)
        Me.tpExplosionDeInsumos.TabIndex = 3
        Me.tpExplosionDeInsumos.Text = "Explosión de Insumos"
        Me.tpExplosionDeInsumos.UseVisualStyleBackColor = True
        '
        'tcInsumosObra
        '
        Me.tcInsumosObra.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tcInsumosObra.Controls.Add(Me.tpExplosion)
        Me.tcInsumosObra.Location = New System.Drawing.Point(8, 13)
        Me.tcInsumosObra.Name = "tcInsumosObra"
        Me.tcInsumosObra.SelectedIndex = 0
        Me.tcInsumosObra.Size = New System.Drawing.Size(771, 322)
        Me.tcInsumosObra.TabIndex = 51
        '
        'tpExplosion
        '
        Me.tpExplosion.Controls.Add(Me.dgvExplosionDeInsumos)
        Me.tpExplosion.Location = New System.Drawing.Point(4, 22)
        Me.tpExplosion.Name = "tpExplosion"
        Me.tpExplosion.Padding = New System.Windows.Forms.Padding(3)
        Me.tpExplosion.Size = New System.Drawing.Size(763, 296)
        Me.tpExplosion.TabIndex = 0
        Me.tpExplosion.Text = "Explosión de Insumos"
        Me.tpExplosion.UseVisualStyleBackColor = True
        '
        'dgvExplosionDeInsumos
        '
        Me.dgvExplosionDeInsumos.AllowUserToAddRows = False
        Me.dgvExplosionDeInsumos.AllowUserToDeleteRows = False
        Me.dgvExplosionDeInsumos.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.dgvExplosionDeInsumos.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvExplosionDeInsumos.Location = New System.Drawing.Point(6, 6)
        Me.dgvExplosionDeInsumos.Name = "dgvExplosionDeInsumos"
        Me.dgvExplosionDeInsumos.RowHeadersVisible = False
        Me.dgvExplosionDeInsumos.Size = New System.Drawing.Size(717, 284)
        Me.dgvExplosionDeInsumos.TabIndex = 55
        Me.dgvExplosionDeInsumos.Visible = False
        '
        'btnGenerarExplosion
        '
        Me.btnGenerarExplosion.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnGenerarExplosion.Enabled = False
        Me.btnGenerarExplosion.Image = Global.Oversight.My.Resources.Resources.explosion24x24
        Me.btnGenerarExplosion.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnGenerarExplosion.Location = New System.Drawing.Point(586, 341)
        Me.btnGenerarExplosion.Name = "btnGenerarExplosion"
        Me.btnGenerarExplosion.Size = New System.Drawing.Size(183, 42)
        Me.btnGenerarExplosion.TabIndex = 62
        Me.btnGenerarExplosion.Text = "Exportar E&xplosión de Insumos"
        Me.btnGenerarExplosion.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnGenerarExplosion.UseVisualStyleBackColor = True
        '
        'btnGuardar
        '
        Me.btnGuardar.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnGuardar.Enabled = False
        Me.btnGuardar.Image = CType(resources.GetObject("btnGuardar.Image"), System.Drawing.Image)
        Me.btnGuardar.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnGuardar.Location = New System.Drawing.Point(478, 435)
        Me.btnGuardar.Name = "btnGuardar"
        Me.btnGuardar.Size = New System.Drawing.Size(131, 34)
        Me.btnGuardar.TabIndex = 21
        Me.btnGuardar.Text = "&Guardar Modelo"
        Me.btnGuardar.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnGuardar.UseVisualStyleBackColor = True
        '
        'btnCancelar
        '
        Me.btnCancelar.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnCancelar.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnCancelar.Image = Global.Oversight.My.Resources.Resources.cancel24x24
        Me.btnCancelar.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnCancelar.Location = New System.Drawing.Point(379, 435)
        Me.btnCancelar.Name = "btnCancelar"
        Me.btnCancelar.Size = New System.Drawing.Size(93, 34)
        Me.btnCancelar.TabIndex = 19
        Me.btnCancelar.Text = "&Cancelar"
        Me.btnCancelar.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnCancelar.UseVisualStyleBackColor = True
        '
        'btnGuardarYCerrar
        '
        Me.btnGuardarYCerrar.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnGuardarYCerrar.Enabled = False
        Me.btnGuardarYCerrar.Image = Global.Oversight.My.Resources.Resources.saveclose33x24
        Me.btnGuardarYCerrar.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnGuardarYCerrar.Location = New System.Drawing.Point(615, 435)
        Me.btnGuardarYCerrar.Name = "btnGuardarYCerrar"
        Me.btnGuardarYCerrar.Size = New System.Drawing.Size(176, 34)
        Me.btnGuardarYCerrar.TabIndex = 20
        Me.btnGuardarYCerrar.Text = "G&uardar Modelo y Cerrar"
        Me.btnGuardarYCerrar.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnGuardarYCerrar.UseVisualStyleBackColor = True
        '
        'NotifyIcon1
        '
        Me.NotifyIcon1.Icon = CType(resources.GetObject("NotifyIcon1.Icon"), System.Drawing.Icon)
        Me.NotifyIcon1.Text = "Oversight"
        '
        'AgregarModelo
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(799, 475)
        Me.Controls.Add(Me.btnGuardar)
        Me.Controls.Add(Me.btnCancelar)
        Me.Controls.Add(Me.btnGuardarYCerrar)
        Me.Controls.Add(Me.tcModelo)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "AgregarModelo"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Modelo"
        Me.tcModelo.ResumeLayout(False)
        Me.tpDatosIniciales.ResumeLayout(False)
        Me.tpDatosIniciales.PerformLayout()
        Me.GroupBox6.ResumeLayout(False)
        Me.GroupBox6.PerformLayout()
        Me.tpCostosIndirectos.ResumeLayout(False)
        Me.tpCostosIndirectos.PerformLayout()
        CType(Me.dgvCostosIndirectos, System.ComponentModel.ISupportInitialize).EndInit()
        Me.tpResumenTarjetas.ResumeLayout(False)
        Me.tpResumenTarjetas.PerformLayout()
        Me.FlowLayoutPanel1.ResumeLayout(False)
        Me.flpAccionesResumen.ResumeLayout(False)
        CType(Me.dgvResumenDeTarjetas, System.ComponentModel.ISupportInitialize).EndInit()
        Me.tpExplosionDeInsumos.ResumeLayout(False)
        Me.tcInsumosObra.ResumeLayout(False)
        Me.tpExplosion.ResumeLayout(False)
        CType(Me.dgvExplosionDeInsumos, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents tcModelo As System.Windows.Forms.TabControl
    Friend WithEvents tpDatosIniciales As System.Windows.Forms.TabPage
    Friend WithEvents tpResumenTarjetas As System.Windows.Forms.TabPage
    Friend WithEvents btnPaso2 As System.Windows.Forms.Button
    Friend WithEvents tpCostosIndirectos As System.Windows.Forms.TabPage
    Friend WithEvents dgvResumenDeTarjetas As System.Windows.Forms.DataGridView
    Friend WithEvents rbOtro As System.Windows.Forms.RadioButton
    Friend WithEvents rbOficina As System.Windows.Forms.RadioButton
    Friend WithEvents rbCasa As System.Windows.Forms.RadioButton
    Friend WithEvents lblLongitudVivienda As System.Windows.Forms.Label
    Friend WithEvents lblTipoConstruccion As System.Windows.Forms.Label
    Friend WithEvents lblFechaPresupuesto As System.Windows.Forms.Label
    Friend WithEvents txtLongitudVivienda As System.Windows.Forms.TextBox
    Friend WithEvents dtFechaModificacion As System.Windows.Forms.DateTimePicker
    Friend WithEvents lblCostoProyectadoTotal As System.Windows.Forms.Label
    Friend WithEvents txtPrecioProyectadoSubTotal As System.Windows.Forms.TextBox
    Friend WithEvents tpExplosionDeInsumos As System.Windows.Forms.TabPage
    Friend WithEvents lblPrecioProyectadoTotal As System.Windows.Forms.Label
    Friend WithEvents txtPrecioProyectadoTotal As System.Windows.Forms.TextBox
    Friend WithEvents lblArchivoPlanos As System.Windows.Forms.Label
    Friend WithEvents txtRuta As System.Windows.Forms.TextBox
    Friend WithEvents btnAbrirCarpeta As System.Windows.Forms.Button
    Friend WithEvents dgvExplosionDeInsumos As System.Windows.Forms.DataGridView
    Friend WithEvents btnGenerarExplosion As System.Windows.Forms.Button
    Friend WithEvents tcInsumosObra As System.Windows.Forms.TabControl
    Friend WithEvents tpExplosion As System.Windows.Forms.TabPage
    Friend WithEvents GroupBox6 As System.Windows.Forms.GroupBox
    Friend WithEvents btnCostoHoy As System.Windows.Forms.Button
    Friend WithEvents btnPaso3 As System.Windows.Forms.Button
    Friend WithEvents btnActualizarPrecios As System.Windows.Forms.Button
    Friend WithEvents lblNombreDelModelo As System.Windows.Forms.Label
    Friend WithEvents txtNombreModelo As System.Windows.Forms.TextBox
    Friend WithEvents lblIva As System.Windows.Forms.Label
    Friend WithEvents txtIVA As System.Windows.Forms.TextBox
    Friend WithEvents flpAccionesResumen As System.Windows.Forms.FlowLayoutPanel
    Friend WithEvents lblPercentageSignHelper As System.Windows.Forms.Label
    Friend WithEvents txtPorcentajeIVA As System.Windows.Forms.TextBox
    Friend WithEvents btnActualizarUtilidad As System.Windows.Forms.Button
    Friend WithEvents btnGenerarArchivoExcel As System.Windows.Forms.Button
    Friend WithEvents FlowLayoutPanel1 As System.Windows.Forms.FlowLayoutPanel
    Friend WithEvents msSaveFileDialog As System.Windows.Forms.SaveFileDialog
    Friend WithEvents lblAnchoVivienda As System.Windows.Forms.Label
    Friend WithEvents lblmts2 As System.Windows.Forms.Label
    Friend WithEvents lblmts1 As System.Windows.Forms.Label
    Friend WithEvents txtAnchoVivienda As System.Windows.Forms.TextBox
    Friend WithEvents dgvCostosIndirectos As System.Windows.Forms.DataGridView
    Friend WithEvents lblTotalIndirectos As System.Windows.Forms.Label
    Friend WithEvents lblTotalIndirectosLbl As System.Windows.Forms.Label
    Friend WithEvents lblPorcentajeIndirectos As System.Windows.Forms.Label
    Friend WithEvents lblPorcentajeIndirectosLbl As System.Windows.Forms.Label
    Friend WithEvents txtIngresosIndirectos As System.Windows.Forms.TextBox
    Friend WithEvents lblIngresosIndirectos As System.Windows.Forms.Label
    Friend WithEvents btnCancelar As System.Windows.Forms.Button
    Friend WithEvents btnGuardarYCerrar As System.Windows.Forms.Button
    Friend WithEvents btnGuardar As System.Windows.Forms.Button
    Friend WithEvents btnEliminarTarjeta As System.Windows.Forms.Button
    Friend WithEvents btnInsertarTarjeta As System.Windows.Forms.Button
    Friend WithEvents btnNuevaTarjeta As System.Windows.Forms.Button
    Friend WithEvents btnEliminarCostoIndirecto As System.Windows.Forms.Button
    Friend WithEvents btnNuevoCostoIndirecto As System.Windows.Forms.Button
    Friend WithEvents txtPorcentajeUtilidadDefault As System.Windows.Forms.TextBox
    Friend WithEvents lblPorcentajeUtilidadPorDefault As System.Windows.Forms.Label
    Friend WithEvents txtPorcentajeIndirectosDefault As System.Windows.Forms.TextBox
    Friend WithEvents lblPorcentajeIndirectosPorDefault As System.Windows.Forms.Label
    Friend WithEvents lblAsteriscoUtilidades As System.Windows.Forms.Label
    Friend WithEvents lblAsteriscoIndirectos As System.Windows.Forms.Label
    Friend WithEvents lblIndirectosSubTotal As System.Windows.Forms.Label
    Friend WithEvents txtIndirectosTotal As System.Windows.Forms.TextBox
    Friend WithEvents txtIndirectosSubtotal As System.Windows.Forms.TextBox
    Friend WithEvents lblIndirectosTotal As System.Windows.Forms.Label
    Friend WithEvents NotifyIcon1 As System.Windows.Forms.NotifyIcon
    Friend WithEvents btnRevisiones As System.Windows.Forms.Button

End Class
