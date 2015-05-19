<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class AgregarProyecto
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(AgregarProyecto))
        Me.tcProyecto = New System.Windows.Forms.TabControl
        Me.tpDatosIniciales = New System.Windows.Forms.TabPage
        Me.btnRevisiones = New System.Windows.Forms.Button
        Me.btnDireccion = New System.Windows.Forms.Button
        Me.lblmts4 = New System.Windows.Forms.Label
        Me.txtAnchoTerreno = New System.Windows.Forms.TextBox
        Me.lblAnchoTerreno = New System.Windows.Forms.Label
        Me.lblmts3 = New System.Windows.Forms.Label
        Me.lblmts2 = New System.Windows.Forms.Label
        Me.lblmts1 = New System.Windows.Forms.Label
        Me.txtAnchoVivienda = New System.Windows.Forms.TextBox
        Me.lblAnchoVivienda = New System.Windows.Forms.Label
        Me.lblLongitudTerreno = New System.Windows.Forms.Label
        Me.txtLongitudTerreno = New System.Windows.Forms.TextBox
        Me.btnClientes = New System.Windows.Forms.Button
        Me.lblNombreDelProyecto = New System.Windows.Forms.Label
        Me.txtNombreProyecto = New System.Windows.Forms.TextBox
        Me.GroupBox6 = New System.Windows.Forms.GroupBox
        Me.lblArchivoPlanos = New System.Windows.Forms.Label
        Me.txtRuta = New System.Windows.Forms.TextBox
        Me.btnAbrirCarpeta = New System.Windows.Forms.Button
        Me.lblNombreEmpresa = New System.Windows.Forms.Label
        Me.txtNombreEmpresa = New System.Windows.Forms.TextBox
        Me.lblLugar = New System.Windows.Forms.Label
        Me.rbOtro = New System.Windows.Forms.RadioButton
        Me.rbOficina = New System.Windows.Forms.RadioButton
        Me.rbCasa = New System.Windows.Forms.RadioButton
        Me.lblLongitudVivienda = New System.Windows.Forms.Label
        Me.lblTipoConstruccion = New System.Windows.Forms.Label
        Me.lblNombreCliente = New System.Windows.Forms.Label
        Me.lblFechaPresupuesto = New System.Windows.Forms.Label
        Me.txtLugar = New System.Windows.Forms.TextBox
        Me.txtLongitudVivienda = New System.Windows.Forms.TextBox
        Me.txtNombreCliente = New System.Windows.Forms.TextBox
        Me.dtFechaPresupuesto = New System.Windows.Forms.DateTimePicker
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
        Me.chkDoNOTApplyTax = New System.Windows.Forms.CheckBox
        Me.btnEliminarTarjeta = New System.Windows.Forms.Button
        Me.btnInsertarTarjeta = New System.Windows.Forms.Button
        Me.btnNuevaTarjeta = New System.Windows.Forms.Button
        Me.FlowLayoutPanel1 = New System.Windows.Forms.FlowLayoutPanel
        Me.btnRealizarObra = New System.Windows.Forms.Button
        Me.btnCostoHoy = New System.Windows.Forms.Button
        Me.lblPercentageSignHelper = New System.Windows.Forms.Label
        Me.txtPorcentajeIVA = New System.Windows.Forms.TextBox
        Me.flpAccionesResumen = New System.Windows.Forms.FlowLayoutPanel
        Me.btnActualizarPrecios = New System.Windows.Forms.Button
        Me.btnActualizarUtilidad = New System.Windows.Forms.Button
        Me.btnGenerarContratoWord = New System.Windows.Forms.Button
        Me.btnGenerarArchivoExcel = New System.Windows.Forms.Button
        Me.lblIva = New System.Windows.Forms.Label
        Me.txtIVA = New System.Windows.Forms.TextBox
        Me.lblPrecioProyectadoTotal = New System.Windows.Forms.Label
        Me.txtPrecioProyectadoTotal = New System.Windows.Forms.TextBox
        Me.txtPrecioProyectadoSubTotal = New System.Windows.Forms.TextBox
        Me.dgvResumenDeTarjetas = New System.Windows.Forms.DataGridView
        Me.tpExplosionDeInsumos = New System.Windows.Forms.TabPage
        Me.btnPresupuestosDeCompras = New System.Windows.Forms.Button
        Me.tcInsumosObra = New System.Windows.Forms.TabControl
        Me.tpExplosion = New System.Windows.Forms.TabPage
        Me.btnEliminarInsumo = New System.Windows.Forms.Button
        Me.btnInsertarInsumo = New System.Windows.Forms.Button
        Me.btnNuevoInsumo = New System.Windows.Forms.Button
        Me.dgvExplosionDeInsumos = New System.Windows.Forms.DataGridView
        Me.tpExplosionPorProveedor = New System.Windows.Forms.TabPage
        Me.dgvExplosionDeInsumosPorProveedor = New System.Windows.Forms.DataGridView
        Me.tpInsumosFacturadosNoPresupuestados = New System.Windows.Forms.TabPage
        Me.dgvInsumosFacturadosNoPresupuestados = New System.Windows.Forms.DataGridView
        Me.tpPedidos = New System.Windows.Forms.TabPage
        Me.dgvPedidosDeObra = New System.Windows.Forms.DataGridView
        Me.tpEnvios = New System.Windows.Forms.TabPage
        Me.dgvEnviosALaObra = New System.Windows.Forms.DataGridView
        Me.btnVerificarInventario = New System.Windows.Forms.Button
        Me.btnGenerarHojaControlCompras = New System.Windows.Forms.Button
        Me.btnGenerarExplosion = New System.Windows.Forms.Button
        Me.tpDetalleCostosReales = New System.Windows.Forms.TabPage
        Me.tcContables = New System.Windows.Forms.TabControl
        Me.tpFacturas = New System.Windows.Forms.TabPage
        Me.lblTotalFacturas = New System.Windows.Forms.Label
        Me.lblTotalFacturasLbl = New System.Windows.Forms.Label
        Me.dgvFacturas = New System.Windows.Forms.DataGridView
        Me.tpCombustible = New System.Windows.Forms.TabPage
        Me.lblTotalVales = New System.Windows.Forms.Label
        Me.lblTotalValeslbl = New System.Windows.Forms.Label
        Me.dgvVales = New System.Windows.Forms.DataGridView
        Me.tpGastosAdministrativosAsignados = New System.Windows.Forms.TabPage
        Me.lblSeparator = New System.Windows.Forms.Label
        Me.lblGastosAdminAsignados = New System.Windows.Forms.Label
        Me.lblFacturasNoAplicablesAProyectosDuranteMismoPeriodo = New System.Windows.Forms.Label
        Me.lblTotalFacturasNoAplicablesAProyectosDuranteMismoPeriodo = New System.Windows.Forms.Label
        Me.lblTotalFacturasNoAplicablesAProyectosDuranteMismoPeriodolbl = New System.Windows.Forms.Label
        Me.dgvFacturasNoAplicablesAProyectosDuranteMismoPeriodo = New System.Windows.Forms.DataGridView
        Me.lblTotalGastosAdminAsignados = New System.Windows.Forms.Label
        Me.btnEliminarGastoAdminAsignado = New System.Windows.Forms.Button
        Me.lblTotalGastosAdminAsignadoslbl = New System.Windows.Forms.Label
        Me.btnNuevoGastoAdminAsignado = New System.Windows.Forms.Button
        Me.dgvGastosAdminAsociados = New System.Windows.Forms.DataGridView
        Me.tpNominasYAlimentacion = New System.Windows.Forms.TabPage
        Me.lblTotalNominas = New System.Windows.Forms.Label
        Me.lblTotalNominaslbl = New System.Windows.Forms.Label
        Me.dgvNominas = New System.Windows.Forms.DataGridView
        Me.tpAnticiposYPagos = New System.Windows.Forms.TabPage
        Me.lblTotalEntradas = New System.Windows.Forms.Label
        Me.lblTotalEntradaslbl = New System.Windows.Forms.Label
        Me.dgvEntradas = New System.Windows.Forms.DataGridView
        Me.tpPagosFacturas = New System.Windows.Forms.TabPage
        Me.lblTotalSalidas = New System.Windows.Forms.Label
        Me.lblTotalSalidaslbl = New System.Windows.Forms.Label
        Me.dgvSalidas = New System.Windows.Forms.DataGridView
        Me.tpAnalisisUtilidades = New System.Windows.Forms.TabPage
        Me.gbFechas = New System.Windows.Forms.GroupBox
        Me.lblPorcentajeDeAvanceDeObraLbl = New System.Windows.Forms.Label
        Me.lblPorcentajeDeAvanceDeObra = New System.Windows.Forms.Label
        Me.lblStatusProyectoLbl = New System.Windows.Forms.Label
        Me.lblFechaInicio2 = New System.Windows.Forms.Label
        Me.dtFechaInicio2 = New System.Windows.Forms.DateTimePicker
        Me.lblFechaInicio1 = New System.Windows.Forms.Label
        Me.dtFechaInicio1 = New System.Windows.Forms.DateTimePicker
        Me.lblStatusProyecto = New System.Windows.Forms.Label
        Me.btnTerminarObra = New System.Windows.Forms.Button
        Me.dtFechaTerminoPrevista = New System.Windows.Forms.DateTimePicker
        Me.lblFechaTerminoPrevista = New System.Windows.Forms.Label
        Me.lblFechaTerminoReal = New System.Windows.Forms.Label
        Me.dtFechaTerminoReal = New System.Windows.Forms.DateTimePicker
        Me.lblPorcentajeDeComisionDeObraReal = New System.Windows.Forms.Label
        Me.txtPorcentajePorCierreDeOperacionPresupuestada = New System.Windows.Forms.TextBox
        Me.lblPorcentajePorCierreDeOperacionPresupuestada = New System.Windows.Forms.Label
        Me.txtImportePorCierreDeOperacionPresupuestada = New System.Windows.Forms.TextBox
        Me.txtPorcentajeDeComisionDeObraPresupuestada = New System.Windows.Forms.TextBox
        Me.lblPorcentajeDeComisionDeObraPresupuestada = New System.Windows.Forms.Label
        Me.txtImporteComisionDeObraPresupuestada = New System.Windows.Forms.TextBox
        Me.lblPorcentajePrevistoDeUtilidadEnObra = New System.Windows.Forms.Label
        Me.txtPorcentajePrevistoDeUtilidadEnObra = New System.Windows.Forms.TextBox
        Me.lblUtilidadPrevistaDeEjecución = New System.Windows.Forms.Label
        Me.txtUtilidadPrevistaDeEjecucion = New System.Windows.Forms.TextBox
        Me.lblUtilidadFinalPrevista = New System.Windows.Forms.Label
        Me.txtUtilidadFinalPrevista = New System.Windows.Forms.TextBox
        Me.lblCostoPresupuestadoDeLaObra = New System.Windows.Forms.Label
        Me.txtCostoPresupuestado = New System.Windows.Forms.TextBox
        Me.lblPrecioPresupuestado = New System.Windows.Forms.Label
        Me.txtPrecioPresupuestado = New System.Windows.Forms.TextBox
        Me.txtPorcentajePorCierreDeOperacionReal = New System.Windows.Forms.TextBox
        Me.lblPorcentajePorCierreDeOperacionReal = New System.Windows.Forms.Label
        Me.txtImportePorCierreDeOperacionReal = New System.Windows.Forms.TextBox
        Me.txtPorcentajeDeComisionDeObraReal = New System.Windows.Forms.TextBox
        Me.txtImporteComisiónDeObraReal = New System.Windows.Forms.TextBox
        Me.lblPorcentajeDeUtilidadEnObraReal = New System.Windows.Forms.Label
        Me.txtPorcentajeDeUtilidadEnObraReal = New System.Windows.Forms.TextBox
        Me.lblUtilidadDeEjecucionDeObraReal = New System.Windows.Forms.Label
        Me.txtUtilidadDeEjecucionDeObraReal = New System.Windows.Forms.TextBox
        Me.lblUtilidadFinalReal = New System.Windows.Forms.Label
        Me.txtUtilidadFinalReal = New System.Windows.Forms.TextBox
        Me.lblCostoRealDeLaObra = New System.Windows.Forms.Label
        Me.txtCostoRealDeLaObra = New System.Windows.Forms.TextBox
        Me.lblEntradas = New System.Windows.Forms.Label
        Me.txtEntradas = New System.Windows.Forms.TextBox
        Me.tpReportes = New System.Windows.Forms.TabPage
        Me.tcReportes = New System.Windows.Forms.TabControl
        Me.msSaveFileDialog = New System.Windows.Forms.SaveFileDialog
        Me.btnGuardar = New System.Windows.Forms.Button
        Me.btnCancelar = New System.Windows.Forms.Button
        Me.btnGuardarYCerrar = New System.Windows.Forms.Button
        Me.NotifyIcon1 = New System.Windows.Forms.NotifyIcon(Me.components)
        Me.tcProyecto.SuspendLayout()
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
        Me.tpExplosionPorProveedor.SuspendLayout()
        CType(Me.dgvExplosionDeInsumosPorProveedor, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.tpInsumosFacturadosNoPresupuestados.SuspendLayout()
        CType(Me.dgvInsumosFacturadosNoPresupuestados, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.tpPedidos.SuspendLayout()
        CType(Me.dgvPedidosDeObra, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.tpEnvios.SuspendLayout()
        CType(Me.dgvEnviosALaObra, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.tpDetalleCostosReales.SuspendLayout()
        Me.tcContables.SuspendLayout()
        Me.tpFacturas.SuspendLayout()
        CType(Me.dgvFacturas, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.tpCombustible.SuspendLayout()
        CType(Me.dgvVales, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.tpGastosAdministrativosAsignados.SuspendLayout()
        CType(Me.dgvFacturasNoAplicablesAProyectosDuranteMismoPeriodo, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.dgvGastosAdminAsociados, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.tpNominasYAlimentacion.SuspendLayout()
        CType(Me.dgvNominas, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.tpAnticiposYPagos.SuspendLayout()
        CType(Me.dgvEntradas, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.tpPagosFacturas.SuspendLayout()
        CType(Me.dgvSalidas, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.tpAnalisisUtilidades.SuspendLayout()
        Me.gbFechas.SuspendLayout()
        Me.tpReportes.SuspendLayout()
        Me.SuspendLayout()
        '
        'tcProyecto
        '
        Me.tcProyecto.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tcProyecto.Controls.Add(Me.tpDatosIniciales)
        Me.tcProyecto.Controls.Add(Me.tpCostosIndirectos)
        Me.tcProyecto.Controls.Add(Me.tpResumenTarjetas)
        Me.tcProyecto.Controls.Add(Me.tpExplosionDeInsumos)
        Me.tcProyecto.Controls.Add(Me.tpDetalleCostosReales)
        Me.tcProyecto.Controls.Add(Me.tpAnalisisUtilidades)
        Me.tcProyecto.Controls.Add(Me.tpReportes)
        Me.tcProyecto.Location = New System.Drawing.Point(5, 7)
        Me.tcProyecto.Name = "tcProyecto"
        Me.tcProyecto.SelectedIndex = 0
        Me.tcProyecto.Size = New System.Drawing.Size(786, 422)
        Me.tcProyecto.TabIndex = 18
        '
        'tpDatosIniciales
        '
        Me.tpDatosIniciales.Controls.Add(Me.btnRevisiones)
        Me.tpDatosIniciales.Controls.Add(Me.btnDireccion)
        Me.tpDatosIniciales.Controls.Add(Me.lblmts4)
        Me.tpDatosIniciales.Controls.Add(Me.txtAnchoTerreno)
        Me.tpDatosIniciales.Controls.Add(Me.lblAnchoTerreno)
        Me.tpDatosIniciales.Controls.Add(Me.lblmts3)
        Me.tpDatosIniciales.Controls.Add(Me.lblmts2)
        Me.tpDatosIniciales.Controls.Add(Me.lblmts1)
        Me.tpDatosIniciales.Controls.Add(Me.txtAnchoVivienda)
        Me.tpDatosIniciales.Controls.Add(Me.lblAnchoVivienda)
        Me.tpDatosIniciales.Controls.Add(Me.lblLongitudTerreno)
        Me.tpDatosIniciales.Controls.Add(Me.txtLongitudTerreno)
        Me.tpDatosIniciales.Controls.Add(Me.btnClientes)
        Me.tpDatosIniciales.Controls.Add(Me.lblNombreDelProyecto)
        Me.tpDatosIniciales.Controls.Add(Me.txtNombreProyecto)
        Me.tpDatosIniciales.Controls.Add(Me.GroupBox6)
        Me.tpDatosIniciales.Controls.Add(Me.lblNombreEmpresa)
        Me.tpDatosIniciales.Controls.Add(Me.txtNombreEmpresa)
        Me.tpDatosIniciales.Controls.Add(Me.lblLugar)
        Me.tpDatosIniciales.Controls.Add(Me.rbOtro)
        Me.tpDatosIniciales.Controls.Add(Me.rbOficina)
        Me.tpDatosIniciales.Controls.Add(Me.rbCasa)
        Me.tpDatosIniciales.Controls.Add(Me.lblLongitudVivienda)
        Me.tpDatosIniciales.Controls.Add(Me.lblTipoConstruccion)
        Me.tpDatosIniciales.Controls.Add(Me.lblNombreCliente)
        Me.tpDatosIniciales.Controls.Add(Me.lblFechaPresupuesto)
        Me.tpDatosIniciales.Controls.Add(Me.txtLugar)
        Me.tpDatosIniciales.Controls.Add(Me.txtLongitudVivienda)
        Me.tpDatosIniciales.Controls.Add(Me.txtNombreCliente)
        Me.tpDatosIniciales.Controls.Add(Me.dtFechaPresupuesto)
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
        'btnDireccion
        '
        Me.btnDireccion.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnDireccion.Enabled = False
        Me.btnDireccion.Image = Global.Oversight.My.Resources.Resources.note12x12
        Me.btnDireccion.Location = New System.Drawing.Point(717, 197)
        Me.btnDireccion.Name = "btnDireccion"
        Me.btnDireccion.Size = New System.Drawing.Size(27, 23)
        Me.btnDireccion.TabIndex = 0
        Me.btnDireccion.TabStop = False
        Me.btnDireccion.UseVisualStyleBackColor = True
        '
        'lblmts4
        '
        Me.lblmts4.AutoSize = True
        Me.lblmts4.Location = New System.Drawing.Point(550, 175)
        Me.lblmts4.Name = "lblmts4"
        Me.lblmts4.Size = New System.Drawing.Size(23, 13)
        Me.lblmts4.TabIndex = 63
        Me.lblmts4.Text = "mts"
        '
        'txtAnchoTerreno
        '
        Me.txtAnchoTerreno.Enabled = False
        Me.txtAnchoTerreno.Location = New System.Drawing.Point(473, 172)
        Me.txtAnchoTerreno.MaxLength = 100
        Me.txtAnchoTerreno.Name = "txtAnchoTerreno"
        Me.txtAnchoTerreno.Size = New System.Drawing.Size(71, 20)
        Me.txtAnchoTerreno.TabIndex = 12
        Me.txtAnchoTerreno.Text = "1"
        '
        'lblAnchoTerreno
        '
        Me.lblAnchoTerreno.AutoSize = True
        Me.lblAnchoTerreno.Location = New System.Drawing.Point(317, 175)
        Me.lblAnchoTerreno.Name = "lblAnchoTerreno"
        Me.lblAnchoTerreno.Size = New System.Drawing.Size(137, 13)
        Me.lblAnchoTerreno.TabIndex = 61
        Me.lblAnchoTerreno.Text = "Ancho del Terreno (Fondo):"
        '
        'lblmts3
        '
        Me.lblmts3.AutoSize = True
        Me.lblmts3.Location = New System.Drawing.Point(277, 175)
        Me.lblmts3.Name = "lblmts3"
        Me.lblmts3.Size = New System.Drawing.Size(23, 13)
        Me.lblmts3.TabIndex = 60
        Me.lblmts3.Text = "mts"
        '
        'lblmts2
        '
        Me.lblmts2.AutoSize = True
        Me.lblmts2.Location = New System.Drawing.Point(550, 149)
        Me.lblmts2.Name = "lblmts2"
        Me.lblmts2.Size = New System.Drawing.Size(23, 13)
        Me.lblmts2.TabIndex = 59
        Me.lblmts2.Text = "mts"
        '
        'lblmts1
        '
        Me.lblmts1.AutoSize = True
        Me.lblmts1.Location = New System.Drawing.Point(274, 149)
        Me.lblmts1.Name = "lblmts1"
        Me.lblmts1.Size = New System.Drawing.Size(23, 13)
        Me.lblmts1.TabIndex = 58
        Me.lblmts1.Text = "mts"
        '
        'txtAnchoVivienda
        '
        Me.txtAnchoVivienda.Enabled = False
        Me.txtAnchoVivienda.Location = New System.Drawing.Point(473, 146)
        Me.txtAnchoVivienda.MaxLength = 100
        Me.txtAnchoVivienda.Name = "txtAnchoVivienda"
        Me.txtAnchoVivienda.Size = New System.Drawing.Size(71, 20)
        Me.txtAnchoVivienda.TabIndex = 10
        Me.txtAnchoVivienda.Text = "1"
        '
        'lblAnchoVivienda
        '
        Me.lblAnchoVivienda.AutoSize = True
        Me.lblAnchoVivienda.Location = New System.Drawing.Point(317, 149)
        Me.lblAnchoVivienda.Name = "lblAnchoVivienda"
        Me.lblAnchoVivienda.Size = New System.Drawing.Size(150, 13)
        Me.lblAnchoVivienda.TabIndex = 56
        Me.lblAnchoVivienda.Text = "Ancho de la Vivienda (Fondo):"
        '
        'lblLongitudTerreno
        '
        Me.lblLongitudTerreno.AutoSize = True
        Me.lblLongitudTerreno.Location = New System.Drawing.Point(11, 175)
        Me.lblLongitudTerreno.Name = "lblLongitudTerreno"
        Me.lblLongitudTerreno.Size = New System.Drawing.Size(147, 13)
        Me.lblLongitudTerreno.TabIndex = 55
        Me.lblLongitudTerreno.Text = "Longitud del Terreno (Frente):"
        '
        'txtLongitudTerreno
        '
        Me.txtLongitudTerreno.Enabled = False
        Me.txtLongitudTerreno.Location = New System.Drawing.Point(197, 172)
        Me.txtLongitudTerreno.MaxLength = 100
        Me.txtLongitudTerreno.Name = "txtLongitudTerreno"
        Me.txtLongitudTerreno.Size = New System.Drawing.Size(71, 20)
        Me.txtLongitudTerreno.TabIndex = 11
        Me.txtLongitudTerreno.Text = "1"
        '
        'btnClientes
        '
        Me.btnClientes.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnClientes.Enabled = False
        Me.btnClientes.Image = Global.Oversight.My.Resources.Resources.clients12x12
        Me.btnClientes.Location = New System.Drawing.Point(717, 62)
        Me.btnClientes.Name = "btnClientes"
        Me.btnClientes.Size = New System.Drawing.Size(27, 23)
        Me.btnClientes.TabIndex = 4
        Me.btnClientes.UseVisualStyleBackColor = True
        '
        'lblNombreDelProyecto
        '
        Me.lblNombreDelProyecto.AutoSize = True
        Me.lblNombreDelProyecto.Location = New System.Drawing.Point(11, 41)
        Me.lblNombreDelProyecto.Name = "lblNombreDelProyecto"
        Me.lblNombreDelProyecto.Size = New System.Drawing.Size(109, 13)
        Me.lblNombreDelProyecto.TabIndex = 53
        Me.lblNombreDelProyecto.Text = "Nombre del Proyecto:"
        '
        'txtNombreProyecto
        '
        Me.txtNombreProyecto.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtNombreProyecto.Enabled = False
        Me.txtNombreProyecto.Location = New System.Drawing.Point(197, 38)
        Me.txtNombreProyecto.MaxLength = 200
        Me.txtNombreProyecto.Name = "txtNombreProyecto"
        Me.txtNombreProyecto.Size = New System.Drawing.Size(547, 20)
        Me.txtNombreProyecto.TabIndex = 2
        '
        'GroupBox6
        '
        Me.GroupBox6.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GroupBox6.Controls.Add(Me.lblArchivoPlanos)
        Me.GroupBox6.Controls.Add(Me.txtRuta)
        Me.GroupBox6.Controls.Add(Me.btnAbrirCarpeta)
        Me.GroupBox6.Location = New System.Drawing.Point(16, 259)
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
        'lblNombreEmpresa
        '
        Me.lblNombreEmpresa.AutoSize = True
        Me.lblNombreEmpresa.Location = New System.Drawing.Point(11, 93)
        Me.lblNombreEmpresa.Name = "lblNombreEmpresa"
        Me.lblNombreEmpresa.Size = New System.Drawing.Size(181, 13)
        Me.lblNombreEmpresa.TabIndex = 44
        Me.lblNombreEmpresa.Text = "Nombre de Empresa / Dependencia:"
        '
        'txtNombreEmpresa
        '
        Me.txtNombreEmpresa.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtNombreEmpresa.Enabled = False
        Me.txtNombreEmpresa.Location = New System.Drawing.Point(197, 90)
        Me.txtNombreEmpresa.MaxLength = 500
        Me.txtNombreEmpresa.Name = "txtNombreEmpresa"
        Me.txtNombreEmpresa.Size = New System.Drawing.Size(547, 20)
        Me.txtNombreEmpresa.TabIndex = 5
        '
        'lblLugar
        '
        Me.lblLugar.AutoSize = True
        Me.lblLugar.Location = New System.Drawing.Point(11, 202)
        Me.lblLugar.Name = "lblLugar"
        Me.lblLugar.Size = New System.Drawing.Size(117, 13)
        Me.lblLugar.TabIndex = 42
        Me.lblLugar.Text = "Lugar de Construcción:"
        '
        'rbOtro
        '
        Me.rbOtro.AutoSize = True
        Me.rbOtro.Enabled = False
        Me.rbOtro.Location = New System.Drawing.Point(402, 120)
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
        Me.rbOficina.Location = New System.Drawing.Point(320, 120)
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
        Me.rbCasa.Location = New System.Drawing.Point(197, 120)
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
        Me.lblLongitudVivienda.Location = New System.Drawing.Point(11, 149)
        Me.lblLongitudVivienda.Name = "lblLongitudVivienda"
        Me.lblLongitudVivienda.Size = New System.Drawing.Size(160, 13)
        Me.lblLongitudVivienda.TabIndex = 37
        Me.lblLongitudVivienda.Text = "Longitud de la Vivienda (Frente):"
        '
        'lblTipoConstruccion
        '
        Me.lblTipoConstruccion.AutoSize = True
        Me.lblTipoConstruccion.Location = New System.Drawing.Point(11, 122)
        Me.lblTipoConstruccion.Name = "lblTipoConstruccion"
        Me.lblTipoConstruccion.Size = New System.Drawing.Size(111, 13)
        Me.lblTipoConstruccion.TabIndex = 36
        Me.lblTipoConstruccion.Text = "Tipo de Construcción:"
        '
        'lblNombreCliente
        '
        Me.lblNombreCliente.AutoSize = True
        Me.lblNombreCliente.Location = New System.Drawing.Point(11, 67)
        Me.lblNombreCliente.Name = "lblNombreCliente"
        Me.lblNombreCliente.Size = New System.Drawing.Size(99, 13)
        Me.lblNombreCliente.TabIndex = 35
        Me.lblNombreCliente.Text = "Nombre del Cliente:"
        '
        'lblFechaPresupuesto
        '
        Me.lblFechaPresupuesto.AutoSize = True
        Me.lblFechaPresupuesto.Location = New System.Drawing.Point(11, 12)
        Me.lblFechaPresupuesto.Name = "lblFechaPresupuesto"
        Me.lblFechaPresupuesto.Size = New System.Drawing.Size(170, 13)
        Me.lblFechaPresupuesto.TabIndex = 34
        Me.lblFechaPresupuesto.Text = "Fecha de Presupuesto / Proyecto:"
        '
        'txtLugar
        '
        Me.txtLugar.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtLugar.Location = New System.Drawing.Point(197, 199)
        Me.txtLugar.Name = "txtLugar"
        Me.txtLugar.Size = New System.Drawing.Size(514, 20)
        Me.txtLugar.TabIndex = 14
        '
        'txtLongitudVivienda
        '
        Me.txtLongitudVivienda.Enabled = False
        Me.txtLongitudVivienda.Location = New System.Drawing.Point(197, 146)
        Me.txtLongitudVivienda.MaxLength = 100
        Me.txtLongitudVivienda.Name = "txtLongitudVivienda"
        Me.txtLongitudVivienda.Size = New System.Drawing.Size(71, 20)
        Me.txtLongitudVivienda.TabIndex = 9
        Me.txtLongitudVivienda.Text = "1"
        '
        'txtNombreCliente
        '
        Me.txtNombreCliente.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtNombreCliente.Enabled = False
        Me.txtNombreCliente.Location = New System.Drawing.Point(197, 64)
        Me.txtNombreCliente.Name = "txtNombreCliente"
        Me.txtNombreCliente.Size = New System.Drawing.Size(514, 20)
        Me.txtNombreCliente.TabIndex = 3
        Me.txtNombreCliente.TabStop = False
        '
        'dtFechaPresupuesto
        '
        Me.dtFechaPresupuesto.Enabled = False
        Me.dtFechaPresupuesto.Location = New System.Drawing.Point(197, 12)
        Me.dtFechaPresupuesto.Name = "dtFechaPresupuesto"
        Me.dtFechaPresupuesto.Size = New System.Drawing.Size(254, 20)
        Me.dtFechaPresupuesto.TabIndex = 1
        Me.dtFechaPresupuesto.TabStop = False
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
        Me.tpResumenTarjetas.Controls.Add(Me.chkDoNOTApplyTax)
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
        'chkDoNOTApplyTax
        '
        Me.chkDoNOTApplyTax.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.chkDoNOTApplyTax.AutoSize = True
        Me.chkDoNOTApplyTax.Checked = True
        Me.chkDoNOTApplyTax.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkDoNOTApplyTax.Location = New System.Drawing.Point(602, 312)
        Me.chkDoNOTApplyTax.Name = "chkDoNOTApplyTax"
        Me.chkDoNOTApplyTax.Size = New System.Drawing.Size(93, 17)
        Me.chkDoNOTApplyTax.TabIndex = 23
        Me.chkDoNOTApplyTax.Text = "No cobrar IVA"
        Me.chkDoNOTApplyTax.UseVisualStyleBackColor = True
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
        Me.FlowLayoutPanel1.Controls.Add(Me.btnRealizarObra)
        Me.FlowLayoutPanel1.Controls.Add(Me.btnCostoHoy)
        Me.FlowLayoutPanel1.Location = New System.Drawing.Point(570, 341)
        Me.FlowLayoutPanel1.Name = "FlowLayoutPanel1"
        Me.FlowLayoutPanel1.Size = New System.Drawing.Size(140, 40)
        Me.FlowLayoutPanel1.TabIndex = 62
        '
        'btnRealizarObra
        '
        Me.btnRealizarObra.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnRealizarObra.Enabled = False
        Me.btnRealizarObra.Image = Global.Oversight.My.Resources.Resources.helmet24x24
        Me.btnRealizarObra.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnRealizarObra.Location = New System.Drawing.Point(3, 3)
        Me.btnRealizarObra.Name = "btnRealizarObra"
        Me.btnRealizarObra.Size = New System.Drawing.Size(134, 34)
        Me.btnRealizarObra.TabIndex = 30
        Me.btnRealizarObra.Text = "&Realizar Obra!"
        Me.btnRealizarObra.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnRealizarObra.UseVisualStyleBackColor = True
        '
        'btnCostoHoy
        '
        Me.btnCostoHoy.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnCostoHoy.Enabled = False
        Me.btnCostoHoy.Image = Global.Oversight.My.Resources.Resources.question24x24
        Me.btnCostoHoy.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnCostoHoy.Location = New System.Drawing.Point(3, 43)
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
        Me.flpAccionesResumen.Controls.Add(Me.btnGenerarContratoWord)
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
        'btnGenerarContratoWord
        '
        Me.btnGenerarContratoWord.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnGenerarContratoWord.Enabled = False
        Me.btnGenerarContratoWord.Image = Global.Oversight.My.Resources.Resources.word24x24
        Me.btnGenerarContratoWord.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnGenerarContratoWord.Location = New System.Drawing.Point(3, 83)
        Me.btnGenerarContratoWord.Name = "btnGenerarContratoWord"
        Me.btnGenerarContratoWord.Size = New System.Drawing.Size(162, 34)
        Me.btnGenerarContratoWord.TabIndex = 28
        Me.btnGenerarContratoWord.Text = "Generar Contrato &Word"
        Me.btnGenerarContratoWord.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnGenerarContratoWord.UseVisualStyleBackColor = True
        '
        'btnGenerarArchivoExcel
        '
        Me.btnGenerarArchivoExcel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnGenerarArchivoExcel.Enabled = False
        Me.btnGenerarArchivoExcel.Image = Global.Oversight.My.Resources.Resources.excel24x24
        Me.btnGenerarArchivoExcel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnGenerarArchivoExcel.Location = New System.Drawing.Point(3, 123)
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
        Me.tpExplosionDeInsumos.Controls.Add(Me.btnPresupuestosDeCompras)
        Me.tpExplosionDeInsumos.Controls.Add(Me.tcInsumosObra)
        Me.tpExplosionDeInsumos.Controls.Add(Me.btnVerificarInventario)
        Me.tpExplosionDeInsumos.Controls.Add(Me.btnGenerarHojaControlCompras)
        Me.tpExplosionDeInsumos.Controls.Add(Me.btnGenerarExplosion)
        Me.tpExplosionDeInsumos.Location = New System.Drawing.Point(4, 22)
        Me.tpExplosionDeInsumos.Name = "tpExplosionDeInsumos"
        Me.tpExplosionDeInsumos.Size = New System.Drawing.Size(778, 396)
        Me.tpExplosionDeInsumos.TabIndex = 3
        Me.tpExplosionDeInsumos.Text = "Explosión de Insumos"
        Me.tpExplosionDeInsumos.UseVisualStyleBackColor = True
        '
        'btnPresupuestosDeCompras
        '
        Me.btnPresupuestosDeCompras.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnPresupuestosDeCompras.Enabled = False
        Me.btnPresupuestosDeCompras.Image = Global.Oversight.My.Resources.Resources.quotation24x24
        Me.btnPresupuestosDeCompras.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnPresupuestosDeCompras.Location = New System.Drawing.Point(432, 341)
        Me.btnPresupuestosDeCompras.Name = "btnPresupuestosDeCompras"
        Me.btnPresupuestosDeCompras.Size = New System.Drawing.Size(148, 42)
        Me.btnPresupuestosDeCompras.TabIndex = 59
        Me.btnPresupuestosDeCompras.Text = "Presupuestar Insumo"
        Me.btnPresupuestosDeCompras.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnPresupuestosDeCompras.UseVisualStyleBackColor = True
        '
        'tcInsumosObra
        '
        Me.tcInsumosObra.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tcInsumosObra.Controls.Add(Me.tpExplosion)
        Me.tcInsumosObra.Controls.Add(Me.tpExplosionPorProveedor)
        Me.tcInsumosObra.Controls.Add(Me.tpInsumosFacturadosNoPresupuestados)
        Me.tcInsumosObra.Controls.Add(Me.tpPedidos)
        Me.tcInsumosObra.Controls.Add(Me.tpEnvios)
        Me.tcInsumosObra.Location = New System.Drawing.Point(8, 13)
        Me.tcInsumosObra.Name = "tcInsumosObra"
        Me.tcInsumosObra.SelectedIndex = 0
        Me.tcInsumosObra.Size = New System.Drawing.Size(771, 322)
        Me.tcInsumosObra.TabIndex = 51
        '
        'tpExplosion
        '
        Me.tpExplosion.Controls.Add(Me.btnEliminarInsumo)
        Me.tpExplosion.Controls.Add(Me.btnInsertarInsumo)
        Me.tpExplosion.Controls.Add(Me.btnNuevoInsumo)
        Me.tpExplosion.Controls.Add(Me.dgvExplosionDeInsumos)
        Me.tpExplosion.Location = New System.Drawing.Point(4, 22)
        Me.tpExplosion.Name = "tpExplosion"
        Me.tpExplosion.Padding = New System.Windows.Forms.Padding(3)
        Me.tpExplosion.Size = New System.Drawing.Size(763, 296)
        Me.tpExplosion.TabIndex = 0
        Me.tpExplosion.Text = "Explosión de Insumos"
        Me.tpExplosion.UseVisualStyleBackColor = True
        '
        'btnEliminarInsumo
        '
        Me.btnEliminarInsumo.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnEliminarInsumo.Enabled = False
        Me.btnEliminarInsumo.Image = Global.Oversight.My.Resources.Resources.delete12x12
        Me.btnEliminarInsumo.Location = New System.Drawing.Point(729, 64)
        Me.btnEliminarInsumo.Name = "btnEliminarInsumo"
        Me.btnEliminarInsumo.Size = New System.Drawing.Size(28, 23)
        Me.btnEliminarInsumo.TabIndex = 58
        Me.btnEliminarInsumo.UseVisualStyleBackColor = True
        '
        'btnInsertarInsumo
        '
        Me.btnInsertarInsumo.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnInsertarInsumo.Enabled = False
        Me.btnInsertarInsumo.Image = Global.Oversight.My.Resources.Resources.insertcard12x12
        Me.btnInsertarInsumo.Location = New System.Drawing.Point(729, 35)
        Me.btnInsertarInsumo.Name = "btnInsertarInsumo"
        Me.btnInsertarInsumo.Size = New System.Drawing.Size(28, 23)
        Me.btnInsertarInsumo.TabIndex = 57
        Me.btnInsertarInsumo.UseVisualStyleBackColor = True
        '
        'btnNuevoInsumo
        '
        Me.btnNuevoInsumo.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnNuevoInsumo.Enabled = False
        Me.btnNuevoInsumo.Image = Global.Oversight.My.Resources.Resources.newcard12x12
        Me.btnNuevoInsumo.Location = New System.Drawing.Point(729, 6)
        Me.btnNuevoInsumo.Name = "btnNuevoInsumo"
        Me.btnNuevoInsumo.Size = New System.Drawing.Size(28, 23)
        Me.btnNuevoInsumo.TabIndex = 56
        Me.btnNuevoInsumo.UseVisualStyleBackColor = True
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
        'tpExplosionPorProveedor
        '
        Me.tpExplosionPorProveedor.Controls.Add(Me.dgvExplosionDeInsumosPorProveedor)
        Me.tpExplosionPorProveedor.Location = New System.Drawing.Point(4, 22)
        Me.tpExplosionPorProveedor.Name = "tpExplosionPorProveedor"
        Me.tpExplosionPorProveedor.Size = New System.Drawing.Size(763, 296)
        Me.tpExplosionPorProveedor.TabIndex = 3
        Me.tpExplosionPorProveedor.Text = "Explosion de Insumos por Proveedor"
        Me.tpExplosionPorProveedor.UseVisualStyleBackColor = True
        '
        'dgvExplosionDeInsumosPorProveedor
        '
        Me.dgvExplosionDeInsumosPorProveedor.AllowUserToAddRows = False
        Me.dgvExplosionDeInsumosPorProveedor.AllowUserToDeleteRows = False
        Me.dgvExplosionDeInsumosPorProveedor.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.dgvExplosionDeInsumosPorProveedor.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvExplosionDeInsumosPorProveedor.Location = New System.Drawing.Point(6, 6)
        Me.dgvExplosionDeInsumosPorProveedor.Name = "dgvExplosionDeInsumosPorProveedor"
        Me.dgvExplosionDeInsumosPorProveedor.RowHeadersVisible = False
        Me.dgvExplosionDeInsumosPorProveedor.Size = New System.Drawing.Size(751, 284)
        Me.dgvExplosionDeInsumosPorProveedor.TabIndex = 56
        Me.dgvExplosionDeInsumosPorProveedor.Visible = False
        '
        'tpInsumosFacturadosNoPresupuestados
        '
        Me.tpInsumosFacturadosNoPresupuestados.Controls.Add(Me.dgvInsumosFacturadosNoPresupuestados)
        Me.tpInsumosFacturadosNoPresupuestados.Location = New System.Drawing.Point(4, 22)
        Me.tpInsumosFacturadosNoPresupuestados.Name = "tpInsumosFacturadosNoPresupuestados"
        Me.tpInsumosFacturadosNoPresupuestados.Size = New System.Drawing.Size(763, 296)
        Me.tpInsumosFacturadosNoPresupuestados.TabIndex = 4
        Me.tpInsumosFacturadosNoPresupuestados.Text = "Insumos Facturados no Presupuestados"
        Me.tpInsumosFacturadosNoPresupuestados.UseVisualStyleBackColor = True
        '
        'dgvInsumosFacturadosNoPresupuestados
        '
        Me.dgvInsumosFacturadosNoPresupuestados.AllowUserToAddRows = False
        Me.dgvInsumosFacturadosNoPresupuestados.AllowUserToDeleteRows = False
        Me.dgvInsumosFacturadosNoPresupuestados.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.dgvInsumosFacturadosNoPresupuestados.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvInsumosFacturadosNoPresupuestados.Location = New System.Drawing.Point(6, 6)
        Me.dgvInsumosFacturadosNoPresupuestados.Name = "dgvInsumosFacturadosNoPresupuestados"
        Me.dgvInsumosFacturadosNoPresupuestados.RowHeadersVisible = False
        Me.dgvInsumosFacturadosNoPresupuestados.Size = New System.Drawing.Size(751, 284)
        Me.dgvInsumosFacturadosNoPresupuestados.TabIndex = 57
        Me.dgvInsumosFacturadosNoPresupuestados.Visible = False
        '
        'tpPedidos
        '
        Me.tpPedidos.Controls.Add(Me.dgvPedidosDeObra)
        Me.tpPedidos.Location = New System.Drawing.Point(4, 22)
        Me.tpPedidos.Name = "tpPedidos"
        Me.tpPedidos.Size = New System.Drawing.Size(763, 296)
        Me.tpPedidos.TabIndex = 2
        Me.tpPedidos.Text = "Pedidos"
        Me.tpPedidos.UseVisualStyleBackColor = True
        '
        'dgvPedidosDeObra
        '
        Me.dgvPedidosDeObra.AllowUserToAddRows = False
        Me.dgvPedidosDeObra.AllowUserToDeleteRows = False
        Me.dgvPedidosDeObra.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.dgvPedidosDeObra.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvPedidosDeObra.Location = New System.Drawing.Point(6, 6)
        Me.dgvPedidosDeObra.MultiSelect = False
        Me.dgvPedidosDeObra.Name = "dgvPedidosDeObra"
        Me.dgvPedidosDeObra.RowHeadersVisible = False
        Me.dgvPedidosDeObra.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgvPedidosDeObra.Size = New System.Drawing.Size(751, 284)
        Me.dgvPedidosDeObra.TabIndex = 63
        Me.dgvPedidosDeObra.Visible = False
        '
        'tpEnvios
        '
        Me.tpEnvios.Controls.Add(Me.dgvEnviosALaObra)
        Me.tpEnvios.Location = New System.Drawing.Point(4, 22)
        Me.tpEnvios.Name = "tpEnvios"
        Me.tpEnvios.Padding = New System.Windows.Forms.Padding(3)
        Me.tpEnvios.Size = New System.Drawing.Size(763, 296)
        Me.tpEnvios.TabIndex = 1
        Me.tpEnvios.Text = "Envíos"
        Me.tpEnvios.UseVisualStyleBackColor = True
        '
        'dgvEnviosALaObra
        '
        Me.dgvEnviosALaObra.AllowUserToAddRows = False
        Me.dgvEnviosALaObra.AllowUserToDeleteRows = False
        Me.dgvEnviosALaObra.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.dgvEnviosALaObra.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvEnviosALaObra.Location = New System.Drawing.Point(6, 6)
        Me.dgvEnviosALaObra.MultiSelect = False
        Me.dgvEnviosALaObra.Name = "dgvEnviosALaObra"
        Me.dgvEnviosALaObra.RowHeadersVisible = False
        Me.dgvEnviosALaObra.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgvEnviosALaObra.Size = New System.Drawing.Size(751, 284)
        Me.dgvEnviosALaObra.TabIndex = 64
        Me.dgvEnviosALaObra.Visible = False
        '
        'btnVerificarInventario
        '
        Me.btnVerificarInventario.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnVerificarInventario.Enabled = False
        Me.btnVerificarInventario.Image = Global.Oversight.My.Resources.Resources.inventory_2_24x24
        Me.btnVerificarInventario.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnVerificarInventario.Location = New System.Drawing.Point(8, 341)
        Me.btnVerificarInventario.Name = "btnVerificarInventario"
        Me.btnVerificarInventario.Size = New System.Drawing.Size(141, 42)
        Me.btnVerificarInventario.TabIndex = 60
        Me.btnVerificarInventario.Text = "&Verificar Inventario"
        Me.btnVerificarInventario.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnVerificarInventario.UseVisualStyleBackColor = True
        '
        'btnGenerarHojaControlCompras
        '
        Me.btnGenerarHojaControlCompras.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnGenerarHojaControlCompras.Enabled = False
        Me.btnGenerarHojaControlCompras.Image = Global.Oversight.My.Resources.Resources.excel24x24
        Me.btnGenerarHojaControlCompras.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnGenerarHojaControlCompras.Location = New System.Drawing.Point(155, 341)
        Me.btnGenerarHojaControlCompras.Name = "btnGenerarHojaControlCompras"
        Me.btnGenerarHojaControlCompras.Size = New System.Drawing.Size(223, 42)
        Me.btnGenerarHojaControlCompras.TabIndex = 61
        Me.btnGenerarHojaControlCompras.Text = "Generar &Hoja de Control de Compras"
        Me.btnGenerarHojaControlCompras.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnGenerarHojaControlCompras.UseVisualStyleBackColor = True
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
        'tpDetalleCostosReales
        '
        Me.tpDetalleCostosReales.Controls.Add(Me.tcContables)
        Me.tpDetalleCostosReales.Location = New System.Drawing.Point(4, 22)
        Me.tpDetalleCostosReales.Name = "tpDetalleCostosReales"
        Me.tpDetalleCostosReales.Size = New System.Drawing.Size(778, 396)
        Me.tpDetalleCostosReales.TabIndex = 5
        Me.tpDetalleCostosReales.Text = "Detalle de Costos Reales"
        Me.tpDetalleCostosReales.UseVisualStyleBackColor = True
        '
        'tcContables
        '
        Me.tcContables.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tcContables.Controls.Add(Me.tpFacturas)
        Me.tcContables.Controls.Add(Me.tpCombustible)
        Me.tcContables.Controls.Add(Me.tpGastosAdministrativosAsignados)
        Me.tcContables.Controls.Add(Me.tpNominasYAlimentacion)
        Me.tcContables.Controls.Add(Me.tpAnticiposYPagos)
        Me.tcContables.Controls.Add(Me.tpPagosFacturas)
        Me.tcContables.Location = New System.Drawing.Point(8, 14)
        Me.tcContables.Name = "tcContables"
        Me.tcContables.SelectedIndex = 0
        Me.tcContables.Size = New System.Drawing.Size(771, 340)
        Me.tcContables.TabIndex = 65
        '
        'tpFacturas
        '
        Me.tpFacturas.Controls.Add(Me.lblTotalFacturas)
        Me.tpFacturas.Controls.Add(Me.lblTotalFacturasLbl)
        Me.tpFacturas.Controls.Add(Me.dgvFacturas)
        Me.tpFacturas.Location = New System.Drawing.Point(4, 22)
        Me.tpFacturas.Name = "tpFacturas"
        Me.tpFacturas.Size = New System.Drawing.Size(763, 314)
        Me.tpFacturas.TabIndex = 2
        Me.tpFacturas.Text = "Facturas"
        Me.tpFacturas.UseVisualStyleBackColor = True
        '
        'lblTotalFacturas
        '
        Me.lblTotalFacturas.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lblTotalFacturas.AutoSize = True
        Me.lblTotalFacturas.Location = New System.Drawing.Point(49, 292)
        Me.lblTotalFacturas.MaximumSize = New System.Drawing.Size(200, 0)
        Me.lblTotalFacturas.MinimumSize = New System.Drawing.Size(200, 0)
        Me.lblTotalFacturas.Name = "lblTotalFacturas"
        Me.lblTotalFacturas.Size = New System.Drawing.Size(200, 13)
        Me.lblTotalFacturas.TabIndex = 63
        '
        'lblTotalFacturasLbl
        '
        Me.lblTotalFacturasLbl.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lblTotalFacturasLbl.AutoSize = True
        Me.lblTotalFacturasLbl.Location = New System.Drawing.Point(3, 292)
        Me.lblTotalFacturasLbl.Name = "lblTotalFacturasLbl"
        Me.lblTotalFacturasLbl.Size = New System.Drawing.Size(40, 13)
        Me.lblTotalFacturasLbl.TabIndex = 62
        Me.lblTotalFacturasLbl.Text = "Total : "
        '
        'dgvFacturas
        '
        Me.dgvFacturas.AllowUserToAddRows = False
        Me.dgvFacturas.AllowUserToDeleteRows = False
        Me.dgvFacturas.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.dgvFacturas.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvFacturas.Location = New System.Drawing.Point(6, 5)
        Me.dgvFacturas.MultiSelect = False
        Me.dgvFacturas.Name = "dgvFacturas"
        Me.dgvFacturas.RowHeadersVisible = False
        Me.dgvFacturas.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgvFacturas.Size = New System.Drawing.Size(751, 284)
        Me.dgvFacturas.TabIndex = 66
        Me.dgvFacturas.Visible = False
        '
        'tpCombustible
        '
        Me.tpCombustible.Controls.Add(Me.lblTotalVales)
        Me.tpCombustible.Controls.Add(Me.lblTotalValeslbl)
        Me.tpCombustible.Controls.Add(Me.dgvVales)
        Me.tpCombustible.Location = New System.Drawing.Point(4, 22)
        Me.tpCombustible.Name = "tpCombustible"
        Me.tpCombustible.Size = New System.Drawing.Size(763, 314)
        Me.tpCombustible.TabIndex = 4
        Me.tpCombustible.Text = "Combustible"
        Me.tpCombustible.UseVisualStyleBackColor = True
        '
        'lblTotalVales
        '
        Me.lblTotalVales.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lblTotalVales.AutoSize = True
        Me.lblTotalVales.Location = New System.Drawing.Point(49, 292)
        Me.lblTotalVales.MaximumSize = New System.Drawing.Size(200, 0)
        Me.lblTotalVales.MinimumSize = New System.Drawing.Size(200, 0)
        Me.lblTotalVales.Name = "lblTotalVales"
        Me.lblTotalVales.Size = New System.Drawing.Size(200, 13)
        Me.lblTotalVales.TabIndex = 63
        '
        'lblTotalValeslbl
        '
        Me.lblTotalValeslbl.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lblTotalValeslbl.AutoSize = True
        Me.lblTotalValeslbl.Location = New System.Drawing.Point(3, 292)
        Me.lblTotalValeslbl.Name = "lblTotalValeslbl"
        Me.lblTotalValeslbl.Size = New System.Drawing.Size(40, 13)
        Me.lblTotalValeslbl.TabIndex = 62
        Me.lblTotalValeslbl.Text = "Total : "
        '
        'dgvVales
        '
        Me.dgvVales.AllowUserToAddRows = False
        Me.dgvVales.AllowUserToDeleteRows = False
        Me.dgvVales.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.dgvVales.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvVales.Location = New System.Drawing.Point(6, 5)
        Me.dgvVales.MultiSelect = False
        Me.dgvVales.Name = "dgvVales"
        Me.dgvVales.RowHeadersVisible = False
        Me.dgvVales.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgvVales.Size = New System.Drawing.Size(751, 284)
        Me.dgvVales.TabIndex = 66
        Me.dgvVales.Visible = False
        '
        'tpGastosAdministrativosAsignados
        '
        Me.tpGastosAdministrativosAsignados.Controls.Add(Me.lblSeparator)
        Me.tpGastosAdministrativosAsignados.Controls.Add(Me.lblGastosAdminAsignados)
        Me.tpGastosAdministrativosAsignados.Controls.Add(Me.lblFacturasNoAplicablesAProyectosDuranteMismoPeriodo)
        Me.tpGastosAdministrativosAsignados.Controls.Add(Me.lblTotalFacturasNoAplicablesAProyectosDuranteMismoPeriodo)
        Me.tpGastosAdministrativosAsignados.Controls.Add(Me.lblTotalFacturasNoAplicablesAProyectosDuranteMismoPeriodolbl)
        Me.tpGastosAdministrativosAsignados.Controls.Add(Me.dgvFacturasNoAplicablesAProyectosDuranteMismoPeriodo)
        Me.tpGastosAdministrativosAsignados.Controls.Add(Me.lblTotalGastosAdminAsignados)
        Me.tpGastosAdministrativosAsignados.Controls.Add(Me.btnEliminarGastoAdminAsignado)
        Me.tpGastosAdministrativosAsignados.Controls.Add(Me.lblTotalGastosAdminAsignadoslbl)
        Me.tpGastosAdministrativosAsignados.Controls.Add(Me.btnNuevoGastoAdminAsignado)
        Me.tpGastosAdministrativosAsignados.Controls.Add(Me.dgvGastosAdminAsociados)
        Me.tpGastosAdministrativosAsignados.Location = New System.Drawing.Point(4, 22)
        Me.tpGastosAdministrativosAsignados.Name = "tpGastosAdministrativosAsignados"
        Me.tpGastosAdministrativosAsignados.Size = New System.Drawing.Size(763, 314)
        Me.tpGastosAdministrativosAsignados.TabIndex = 6
        Me.tpGastosAdministrativosAsignados.Text = "Gastos Admin Asignados"
        Me.tpGastosAdministrativosAsignados.UseVisualStyleBackColor = True
        '
        'lblSeparator
        '
        Me.lblSeparator.AutoSize = True
        Me.lblSeparator.Location = New System.Drawing.Point(398, 148)
        Me.lblSeparator.Name = "lblSeparator"
        Me.lblSeparator.Size = New System.Drawing.Size(0, 13)
        Me.lblSeparator.TabIndex = 84
        '
        'lblGastosAdminAsignados
        '
        Me.lblGastosAdminAsignados.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblGastosAdminAsignados.AutoSize = True
        Me.lblGastosAdminAsignados.Location = New System.Drawing.Point(401, 7)
        Me.lblGastosAdminAsignados.Name = "lblGastosAdminAsignados"
        Me.lblGastosAdminAsignados.Size = New System.Drawing.Size(168, 13)
        Me.lblGastosAdminAsignados.TabIndex = 83
        Me.lblGastosAdminAsignados.Text = "Gastos Administrativos Asignados:"
        Me.lblGastosAdminAsignados.Visible = False
        '
        'lblFacturasNoAplicablesAProyectosDuranteMismoPeriodo
        '
        Me.lblFacturasNoAplicablesAProyectosDuranteMismoPeriodo.AutoSize = True
        Me.lblFacturasNoAplicablesAProyectosDuranteMismoPeriodo.Location = New System.Drawing.Point(3, 7)
        Me.lblFacturasNoAplicablesAProyectosDuranteMismoPeriodo.Name = "lblFacturasNoAplicablesAProyectosDuranteMismoPeriodo"
        Me.lblFacturasNoAplicablesAProyectosDuranteMismoPeriodo.Size = New System.Drawing.Size(346, 13)
        Me.lblFacturasNoAplicablesAProyectosDuranteMismoPeriodo.TabIndex = 82
        Me.lblFacturasNoAplicablesAProyectosDuranteMismoPeriodo.Text = "Facturas no aplicables a Proyectos durante el periodo de este Proyecto:"
        Me.lblFacturasNoAplicablesAProyectosDuranteMismoPeriodo.Visible = False
        '
        'lblTotalFacturasNoAplicablesAProyectosDuranteMismoPeriodo
        '
        Me.lblTotalFacturasNoAplicablesAProyectosDuranteMismoPeriodo.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lblTotalFacturasNoAplicablesAProyectosDuranteMismoPeriodo.AutoSize = True
        Me.lblTotalFacturasNoAplicablesAProyectosDuranteMismoPeriodo.Location = New System.Drawing.Point(49, 294)
        Me.lblTotalFacturasNoAplicablesAProyectosDuranteMismoPeriodo.MaximumSize = New System.Drawing.Size(200, 0)
        Me.lblTotalFacturasNoAplicablesAProyectosDuranteMismoPeriodo.MinimumSize = New System.Drawing.Size(200, 0)
        Me.lblTotalFacturasNoAplicablesAProyectosDuranteMismoPeriodo.Name = "lblTotalFacturasNoAplicablesAProyectosDuranteMismoPeriodo"
        Me.lblTotalFacturasNoAplicablesAProyectosDuranteMismoPeriodo.Size = New System.Drawing.Size(200, 13)
        Me.lblTotalFacturasNoAplicablesAProyectosDuranteMismoPeriodo.TabIndex = 81
        Me.lblTotalFacturasNoAplicablesAProyectosDuranteMismoPeriodo.Visible = False
        '
        'lblTotalFacturasNoAplicablesAProyectosDuranteMismoPeriodolbl
        '
        Me.lblTotalFacturasNoAplicablesAProyectosDuranteMismoPeriodolbl.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lblTotalFacturasNoAplicablesAProyectosDuranteMismoPeriodolbl.AutoSize = True
        Me.lblTotalFacturasNoAplicablesAProyectosDuranteMismoPeriodolbl.Location = New System.Drawing.Point(3, 294)
        Me.lblTotalFacturasNoAplicablesAProyectosDuranteMismoPeriodolbl.Name = "lblTotalFacturasNoAplicablesAProyectosDuranteMismoPeriodolbl"
        Me.lblTotalFacturasNoAplicablesAProyectosDuranteMismoPeriodolbl.Size = New System.Drawing.Size(40, 13)
        Me.lblTotalFacturasNoAplicablesAProyectosDuranteMismoPeriodolbl.TabIndex = 80
        Me.lblTotalFacturasNoAplicablesAProyectosDuranteMismoPeriodolbl.Text = "Total : "
        Me.lblTotalFacturasNoAplicablesAProyectosDuranteMismoPeriodolbl.Visible = False
        '
        'dgvFacturasNoAplicablesAProyectosDuranteMismoPeriodo
        '
        Me.dgvFacturasNoAplicablesAProyectosDuranteMismoPeriodo.AllowUserToAddRows = False
        Me.dgvFacturasNoAplicablesAProyectosDuranteMismoPeriodo.AllowUserToDeleteRows = False
        Me.dgvFacturasNoAplicablesAProyectosDuranteMismoPeriodo.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.dgvFacturasNoAplicablesAProyectosDuranteMismoPeriodo.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvFacturasNoAplicablesAProyectosDuranteMismoPeriodo.Location = New System.Drawing.Point(6, 23)
        Me.dgvFacturasNoAplicablesAProyectosDuranteMismoPeriodo.MultiSelect = False
        Me.dgvFacturasNoAplicablesAProyectosDuranteMismoPeriodo.Name = "dgvFacturasNoAplicablesAProyectosDuranteMismoPeriodo"
        Me.dgvFacturasNoAplicablesAProyectosDuranteMismoPeriodo.RowHeadersVisible = False
        Me.dgvFacturasNoAplicablesAProyectosDuranteMismoPeriodo.Size = New System.Drawing.Size(386, 268)
        Me.dgvFacturasNoAplicablesAProyectosDuranteMismoPeriodo.TabIndex = 79
        Me.dgvFacturasNoAplicablesAProyectosDuranteMismoPeriodo.Visible = False
        '
        'lblTotalGastosAdminAsignados
        '
        Me.lblTotalGastosAdminAsignados.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblTotalGastosAdminAsignados.AutoSize = True
        Me.lblTotalGastosAdminAsignados.Location = New System.Drawing.Point(447, 294)
        Me.lblTotalGastosAdminAsignados.MaximumSize = New System.Drawing.Size(200, 0)
        Me.lblTotalGastosAdminAsignados.MinimumSize = New System.Drawing.Size(200, 0)
        Me.lblTotalGastosAdminAsignados.Name = "lblTotalGastosAdminAsignados"
        Me.lblTotalGastosAdminAsignados.Size = New System.Drawing.Size(200, 13)
        Me.lblTotalGastosAdminAsignados.TabIndex = 77
        Me.lblTotalGastosAdminAsignados.Visible = False
        '
        'btnEliminarGastoAdminAsignado
        '
        Me.btnEliminarGastoAdminAsignado.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnEliminarGastoAdminAsignado.Enabled = False
        Me.btnEliminarGastoAdminAsignado.Image = Global.Oversight.My.Resources.Resources.delete12x12
        Me.btnEliminarGastoAdminAsignado.Location = New System.Drawing.Point(729, 52)
        Me.btnEliminarGastoAdminAsignado.Name = "btnEliminarGastoAdminAsignado"
        Me.btnEliminarGastoAdminAsignado.Size = New System.Drawing.Size(28, 23)
        Me.btnEliminarGastoAdminAsignado.TabIndex = 78
        Me.btnEliminarGastoAdminAsignado.UseVisualStyleBackColor = True
        '
        'lblTotalGastosAdminAsignadoslbl
        '
        Me.lblTotalGastosAdminAsignadoslbl.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblTotalGastosAdminAsignadoslbl.AutoSize = True
        Me.lblTotalGastosAdminAsignadoslbl.Location = New System.Drawing.Point(401, 294)
        Me.lblTotalGastosAdminAsignadoslbl.Name = "lblTotalGastosAdminAsignadoslbl"
        Me.lblTotalGastosAdminAsignadoslbl.Size = New System.Drawing.Size(40, 13)
        Me.lblTotalGastosAdminAsignadoslbl.TabIndex = 76
        Me.lblTotalGastosAdminAsignadoslbl.Text = "Total : "
        Me.lblTotalGastosAdminAsignadoslbl.Visible = False
        '
        'btnNuevoGastoAdminAsignado
        '
        Me.btnNuevoGastoAdminAsignado.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnNuevoGastoAdminAsignado.Enabled = False
        Me.btnNuevoGastoAdminAsignado.Image = Global.Oversight.My.Resources.Resources.newcard12x12
        Me.btnNuevoGastoAdminAsignado.Location = New System.Drawing.Point(729, 23)
        Me.btnNuevoGastoAdminAsignado.Name = "btnNuevoGastoAdminAsignado"
        Me.btnNuevoGastoAdminAsignado.Size = New System.Drawing.Size(28, 23)
        Me.btnNuevoGastoAdminAsignado.TabIndex = 75
        Me.btnNuevoGastoAdminAsignado.UseVisualStyleBackColor = True
        '
        'dgvGastosAdminAsociados
        '
        Me.dgvGastosAdminAsociados.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.dgvGastosAdminAsociados.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvGastosAdminAsociados.Location = New System.Drawing.Point(404, 23)
        Me.dgvGastosAdminAsociados.MultiSelect = False
        Me.dgvGastosAdminAsociados.Name = "dgvGastosAdminAsociados"
        Me.dgvGastosAdminAsociados.RowHeadersVisible = False
        Me.dgvGastosAdminAsociados.Size = New System.Drawing.Size(319, 268)
        Me.dgvGastosAdminAsociados.TabIndex = 74
        Me.dgvGastosAdminAsociados.Visible = False
        '
        'tpNominasYAlimentacion
        '
        Me.tpNominasYAlimentacion.Controls.Add(Me.lblTotalNominas)
        Me.tpNominasYAlimentacion.Controls.Add(Me.lblTotalNominaslbl)
        Me.tpNominasYAlimentacion.Controls.Add(Me.dgvNominas)
        Me.tpNominasYAlimentacion.Location = New System.Drawing.Point(4, 22)
        Me.tpNominasYAlimentacion.Name = "tpNominasYAlimentacion"
        Me.tpNominasYAlimentacion.Size = New System.Drawing.Size(763, 314)
        Me.tpNominasYAlimentacion.TabIndex = 5
        Me.tpNominasYAlimentacion.Text = "Nóminas y Alimentación"
        Me.tpNominasYAlimentacion.UseVisualStyleBackColor = True
        '
        'lblTotalNominas
        '
        Me.lblTotalNominas.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lblTotalNominas.AutoSize = True
        Me.lblTotalNominas.Location = New System.Drawing.Point(49, 292)
        Me.lblTotalNominas.MaximumSize = New System.Drawing.Size(200, 0)
        Me.lblTotalNominas.MinimumSize = New System.Drawing.Size(200, 0)
        Me.lblTotalNominas.Name = "lblTotalNominas"
        Me.lblTotalNominas.Size = New System.Drawing.Size(200, 13)
        Me.lblTotalNominas.TabIndex = 63
        '
        'lblTotalNominaslbl
        '
        Me.lblTotalNominaslbl.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lblTotalNominaslbl.AutoSize = True
        Me.lblTotalNominaslbl.Location = New System.Drawing.Point(3, 292)
        Me.lblTotalNominaslbl.Name = "lblTotalNominaslbl"
        Me.lblTotalNominaslbl.Size = New System.Drawing.Size(40, 13)
        Me.lblTotalNominaslbl.TabIndex = 62
        Me.lblTotalNominaslbl.Text = "Total : "
        '
        'dgvNominas
        '
        Me.dgvNominas.AllowUserToAddRows = False
        Me.dgvNominas.AllowUserToDeleteRows = False
        Me.dgvNominas.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.dgvNominas.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvNominas.Location = New System.Drawing.Point(6, 5)
        Me.dgvNominas.MultiSelect = False
        Me.dgvNominas.Name = "dgvNominas"
        Me.dgvNominas.RowHeadersVisible = False
        Me.dgvNominas.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgvNominas.Size = New System.Drawing.Size(751, 284)
        Me.dgvNominas.TabIndex = 66
        Me.dgvNominas.Visible = False
        '
        'tpAnticiposYPagos
        '
        Me.tpAnticiposYPagos.Controls.Add(Me.lblTotalEntradas)
        Me.tpAnticiposYPagos.Controls.Add(Me.lblTotalEntradaslbl)
        Me.tpAnticiposYPagos.Controls.Add(Me.dgvEntradas)
        Me.tpAnticiposYPagos.Location = New System.Drawing.Point(4, 22)
        Me.tpAnticiposYPagos.Name = "tpAnticiposYPagos"
        Me.tpAnticiposYPagos.Padding = New System.Windows.Forms.Padding(3)
        Me.tpAnticiposYPagos.Size = New System.Drawing.Size(763, 314)
        Me.tpAnticiposYPagos.TabIndex = 0
        Me.tpAnticiposYPagos.Text = "Anticipos y Pagos del Cliente"
        Me.tpAnticiposYPagos.UseVisualStyleBackColor = True
        '
        'lblTotalEntradas
        '
        Me.lblTotalEntradas.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lblTotalEntradas.AutoSize = True
        Me.lblTotalEntradas.Location = New System.Drawing.Point(48, 292)
        Me.lblTotalEntradas.MaximumSize = New System.Drawing.Size(200, 0)
        Me.lblTotalEntradas.MinimumSize = New System.Drawing.Size(200, 0)
        Me.lblTotalEntradas.Name = "lblTotalEntradas"
        Me.lblTotalEntradas.Size = New System.Drawing.Size(200, 13)
        Me.lblTotalEntradas.TabIndex = 61
        '
        'lblTotalEntradaslbl
        '
        Me.lblTotalEntradaslbl.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lblTotalEntradaslbl.AutoSize = True
        Me.lblTotalEntradaslbl.Location = New System.Drawing.Point(2, 292)
        Me.lblTotalEntradaslbl.Name = "lblTotalEntradaslbl"
        Me.lblTotalEntradaslbl.Size = New System.Drawing.Size(40, 13)
        Me.lblTotalEntradaslbl.TabIndex = 60
        Me.lblTotalEntradaslbl.Text = "Total : "
        '
        'dgvEntradas
        '
        Me.dgvEntradas.AllowUserToAddRows = False
        Me.dgvEntradas.AllowUserToDeleteRows = False
        Me.dgvEntradas.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.dgvEntradas.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvEntradas.Location = New System.Drawing.Point(5, 5)
        Me.dgvEntradas.MultiSelect = False
        Me.dgvEntradas.Name = "dgvEntradas"
        Me.dgvEntradas.RowHeadersVisible = False
        Me.dgvEntradas.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgvEntradas.Size = New System.Drawing.Size(751, 284)
        Me.dgvEntradas.TabIndex = 68
        Me.dgvEntradas.Visible = False
        '
        'tpPagosFacturas
        '
        Me.tpPagosFacturas.Controls.Add(Me.lblTotalSalidas)
        Me.tpPagosFacturas.Controls.Add(Me.lblTotalSalidaslbl)
        Me.tpPagosFacturas.Controls.Add(Me.dgvSalidas)
        Me.tpPagosFacturas.Location = New System.Drawing.Point(4, 22)
        Me.tpPagosFacturas.Name = "tpPagosFacturas"
        Me.tpPagosFacturas.Padding = New System.Windows.Forms.Padding(3)
        Me.tpPagosFacturas.Size = New System.Drawing.Size(763, 314)
        Me.tpPagosFacturas.TabIndex = 1
        Me.tpPagosFacturas.Text = "Pagos de Facturas de Obra"
        Me.tpPagosFacturas.UseVisualStyleBackColor = True
        '
        'lblTotalSalidas
        '
        Me.lblTotalSalidas.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lblTotalSalidas.AutoSize = True
        Me.lblTotalSalidas.Location = New System.Drawing.Point(49, 292)
        Me.lblTotalSalidas.MaximumSize = New System.Drawing.Size(200, 0)
        Me.lblTotalSalidas.MinimumSize = New System.Drawing.Size(200, 0)
        Me.lblTotalSalidas.Name = "lblTotalSalidas"
        Me.lblTotalSalidas.Size = New System.Drawing.Size(200, 13)
        Me.lblTotalSalidas.TabIndex = 62
        '
        'lblTotalSalidaslbl
        '
        Me.lblTotalSalidaslbl.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lblTotalSalidaslbl.AutoSize = True
        Me.lblTotalSalidaslbl.Location = New System.Drawing.Point(3, 292)
        Me.lblTotalSalidaslbl.Name = "lblTotalSalidaslbl"
        Me.lblTotalSalidaslbl.Size = New System.Drawing.Size(40, 13)
        Me.lblTotalSalidaslbl.TabIndex = 61
        Me.lblTotalSalidaslbl.Text = "Total : "
        '
        'dgvSalidas
        '
        Me.dgvSalidas.AllowUserToAddRows = False
        Me.dgvSalidas.AllowUserToDeleteRows = False
        Me.dgvSalidas.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.dgvSalidas.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvSalidas.Location = New System.Drawing.Point(6, 5)
        Me.dgvSalidas.MultiSelect = False
        Me.dgvSalidas.Name = "dgvSalidas"
        Me.dgvSalidas.RowHeadersVisible = False
        Me.dgvSalidas.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgvSalidas.Size = New System.Drawing.Size(751, 284)
        Me.dgvSalidas.TabIndex = 69
        Me.dgvSalidas.Visible = False
        '
        'tpAnalisisUtilidades
        '
        Me.tpAnalisisUtilidades.Controls.Add(Me.gbFechas)
        Me.tpAnalisisUtilidades.Controls.Add(Me.lblPorcentajeDeComisionDeObraReal)
        Me.tpAnalisisUtilidades.Controls.Add(Me.txtPorcentajePorCierreDeOperacionPresupuestada)
        Me.tpAnalisisUtilidades.Controls.Add(Me.lblPorcentajePorCierreDeOperacionPresupuestada)
        Me.tpAnalisisUtilidades.Controls.Add(Me.txtImportePorCierreDeOperacionPresupuestada)
        Me.tpAnalisisUtilidades.Controls.Add(Me.txtPorcentajeDeComisionDeObraPresupuestada)
        Me.tpAnalisisUtilidades.Controls.Add(Me.lblPorcentajeDeComisionDeObraPresupuestada)
        Me.tpAnalisisUtilidades.Controls.Add(Me.txtImporteComisionDeObraPresupuestada)
        Me.tpAnalisisUtilidades.Controls.Add(Me.lblPorcentajePrevistoDeUtilidadEnObra)
        Me.tpAnalisisUtilidades.Controls.Add(Me.txtPorcentajePrevistoDeUtilidadEnObra)
        Me.tpAnalisisUtilidades.Controls.Add(Me.lblUtilidadPrevistaDeEjecución)
        Me.tpAnalisisUtilidades.Controls.Add(Me.txtUtilidadPrevistaDeEjecucion)
        Me.tpAnalisisUtilidades.Controls.Add(Me.lblUtilidadFinalPrevista)
        Me.tpAnalisisUtilidades.Controls.Add(Me.txtUtilidadFinalPrevista)
        Me.tpAnalisisUtilidades.Controls.Add(Me.lblCostoPresupuestadoDeLaObra)
        Me.tpAnalisisUtilidades.Controls.Add(Me.txtCostoPresupuestado)
        Me.tpAnalisisUtilidades.Controls.Add(Me.lblPrecioPresupuestado)
        Me.tpAnalisisUtilidades.Controls.Add(Me.txtPrecioPresupuestado)
        Me.tpAnalisisUtilidades.Controls.Add(Me.txtPorcentajePorCierreDeOperacionReal)
        Me.tpAnalisisUtilidades.Controls.Add(Me.lblPorcentajePorCierreDeOperacionReal)
        Me.tpAnalisisUtilidades.Controls.Add(Me.txtImportePorCierreDeOperacionReal)
        Me.tpAnalisisUtilidades.Controls.Add(Me.txtPorcentajeDeComisionDeObraReal)
        Me.tpAnalisisUtilidades.Controls.Add(Me.txtImporteComisiónDeObraReal)
        Me.tpAnalisisUtilidades.Controls.Add(Me.lblPorcentajeDeUtilidadEnObraReal)
        Me.tpAnalisisUtilidades.Controls.Add(Me.txtPorcentajeDeUtilidadEnObraReal)
        Me.tpAnalisisUtilidades.Controls.Add(Me.lblUtilidadDeEjecucionDeObraReal)
        Me.tpAnalisisUtilidades.Controls.Add(Me.txtUtilidadDeEjecucionDeObraReal)
        Me.tpAnalisisUtilidades.Controls.Add(Me.lblUtilidadFinalReal)
        Me.tpAnalisisUtilidades.Controls.Add(Me.txtUtilidadFinalReal)
        Me.tpAnalisisUtilidades.Controls.Add(Me.lblCostoRealDeLaObra)
        Me.tpAnalisisUtilidades.Controls.Add(Me.txtCostoRealDeLaObra)
        Me.tpAnalisisUtilidades.Controls.Add(Me.lblEntradas)
        Me.tpAnalisisUtilidades.Controls.Add(Me.txtEntradas)
        Me.tpAnalisisUtilidades.Location = New System.Drawing.Point(4, 22)
        Me.tpAnalisisUtilidades.Name = "tpAnalisisUtilidades"
        Me.tpAnalisisUtilidades.Size = New System.Drawing.Size(778, 396)
        Me.tpAnalisisUtilidades.TabIndex = 4
        Me.tpAnalisisUtilidades.Text = "Análisis de Utilidades"
        Me.tpAnalisisUtilidades.UseVisualStyleBackColor = True
        '
        'gbFechas
        '
        Me.gbFechas.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.gbFechas.Controls.Add(Me.lblPorcentajeDeAvanceDeObraLbl)
        Me.gbFechas.Controls.Add(Me.lblPorcentajeDeAvanceDeObra)
        Me.gbFechas.Controls.Add(Me.lblStatusProyectoLbl)
        Me.gbFechas.Controls.Add(Me.lblFechaInicio2)
        Me.gbFechas.Controls.Add(Me.dtFechaInicio2)
        Me.gbFechas.Controls.Add(Me.lblFechaInicio1)
        Me.gbFechas.Controls.Add(Me.dtFechaInicio1)
        Me.gbFechas.Controls.Add(Me.lblStatusProyecto)
        Me.gbFechas.Controls.Add(Me.btnTerminarObra)
        Me.gbFechas.Controls.Add(Me.dtFechaTerminoPrevista)
        Me.gbFechas.Controls.Add(Me.lblFechaTerminoPrevista)
        Me.gbFechas.Controls.Add(Me.lblFechaTerminoReal)
        Me.gbFechas.Controls.Add(Me.dtFechaTerminoReal)
        Me.gbFechas.Location = New System.Drawing.Point(16, 217)
        Me.gbFechas.Name = "gbFechas"
        Me.gbFechas.Size = New System.Drawing.Size(747, 159)
        Me.gbFechas.TabIndex = 90
        Me.gbFechas.TabStop = False
        Me.gbFechas.Text = "Fechas"
        '
        'lblPorcentajeDeAvanceDeObraLbl
        '
        Me.lblPorcentajeDeAvanceDeObraLbl.AutoSize = True
        Me.lblPorcentajeDeAvanceDeObraLbl.Location = New System.Drawing.Point(14, 73)
        Me.lblPorcentajeDeAvanceDeObraLbl.Name = "lblPorcentajeDeAvanceDeObraLbl"
        Me.lblPorcentajeDeAvanceDeObraLbl.Size = New System.Drawing.Size(173, 13)
        Me.lblPorcentajeDeAvanceDeObraLbl.TabIndex = 97
        Me.lblPorcentajeDeAvanceDeObraLbl.Text = "Porcentaje del Proyecto avanzado:"
        '
        'lblPorcentajeDeAvanceDeObra
        '
        Me.lblPorcentajeDeAvanceDeObra.AutoSize = True
        Me.lblPorcentajeDeAvanceDeObra.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblPorcentajeDeAvanceDeObra.Location = New System.Drawing.Point(189, 73)
        Me.lblPorcentajeDeAvanceDeObra.Name = "lblPorcentajeDeAvanceDeObra"
        Me.lblPorcentajeDeAvanceDeObra.Size = New System.Drawing.Size(21, 13)
        Me.lblPorcentajeDeAvanceDeObra.TabIndex = 96
        Me.lblPorcentajeDeAvanceDeObra.Text = "0%"
        Me.lblPorcentajeDeAvanceDeObra.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'lblStatusProyectoLbl
        '
        Me.lblStatusProyectoLbl.AutoSize = True
        Me.lblStatusProyectoLbl.Location = New System.Drawing.Point(85, 109)
        Me.lblStatusProyectoLbl.Name = "lblStatusProyectoLbl"
        Me.lblStatusProyectoLbl.Size = New System.Drawing.Size(102, 13)
        Me.lblStatusProyectoLbl.TabIndex = 95
        Me.lblStatusProyectoLbl.Text = "Status del Proyecto:"
        '
        'lblFechaInicio2
        '
        Me.lblFechaInicio2.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblFechaInicio2.AutoSize = True
        Me.lblFechaInicio2.Location = New System.Drawing.Point(498, 21)
        Me.lblFechaInicio2.Name = "lblFechaInicio2"
        Me.lblFechaInicio2.Size = New System.Drawing.Size(133, 13)
        Me.lblFechaInicio2.TabIndex = 94
        Me.lblFechaInicio2.Text = "Fecha de Inicio Prometida:"
        '
        'dtFechaInicio2
        '
        Me.dtFechaInicio2.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.dtFechaInicio2.Enabled = False
        Me.dtFechaInicio2.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.dtFechaInicio2.Location = New System.Drawing.Point(637, 15)
        Me.dtFechaInicio2.Name = "dtFechaInicio2"
        Me.dtFechaInicio2.Size = New System.Drawing.Size(104, 20)
        Me.dtFechaInicio2.TabIndex = 93
        Me.dtFechaInicio2.TabStop = False
        '
        'lblFechaInicio1
        '
        Me.lblFechaInicio1.AutoSize = True
        Me.lblFechaInicio1.Location = New System.Drawing.Point(54, 21)
        Me.lblFechaInicio1.Name = "lblFechaInicio1"
        Me.lblFechaInicio1.Size = New System.Drawing.Size(133, 13)
        Me.lblFechaInicio1.TabIndex = 91
        Me.lblFechaInicio1.Text = "Fecha de Inicio Prometida:"
        '
        'dtFechaInicio1
        '
        Me.dtFechaInicio1.Enabled = False
        Me.dtFechaInicio1.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.dtFechaInicio1.Location = New System.Drawing.Point(192, 15)
        Me.dtFechaInicio1.Name = "dtFechaInicio1"
        Me.dtFechaInicio1.Size = New System.Drawing.Size(100, 20)
        Me.dtFechaInicio1.TabIndex = 89
        Me.dtFechaInicio1.TabStop = False
        '
        'lblStatusProyecto
        '
        Me.lblStatusProyecto.AutoSize = True
        Me.lblStatusProyecto.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblStatusProyecto.Location = New System.Drawing.Point(189, 109)
        Me.lblStatusProyecto.Name = "lblStatusProyecto"
        Me.lblStatusProyecto.Size = New System.Drawing.Size(79, 13)
        Me.lblStatusProyecto.TabIndex = 64
        Me.lblStatusProyecto.Text = "StatusProyecto"
        Me.lblStatusProyecto.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'btnTerminarObra
        '
        Me.btnTerminarObra.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnTerminarObra.Enabled = False
        Me.btnTerminarObra.Image = Global.Oversight.My.Resources.Resources.check24x24
        Me.btnTerminarObra.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnTerminarObra.Location = New System.Drawing.Point(569, 94)
        Me.btnTerminarObra.Name = "btnTerminarObra"
        Me.btnTerminarObra.Size = New System.Drawing.Size(172, 42)
        Me.btnTerminarObra.TabIndex = 88
        Me.btnTerminarObra.Text = "&Dar por Terminada la Obra"
        Me.btnTerminarObra.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnTerminarObra.UseVisualStyleBackColor = True
        Me.btnTerminarObra.Visible = False
        '
        'dtFechaTerminoPrevista
        '
        Me.dtFechaTerminoPrevista.Enabled = False
        Me.dtFechaTerminoPrevista.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.dtFechaTerminoPrevista.Location = New System.Drawing.Point(192, 41)
        Me.dtFechaTerminoPrevista.Name = "dtFechaTerminoPrevista"
        Me.dtFechaTerminoPrevista.Size = New System.Drawing.Size(100, 20)
        Me.dtFechaTerminoPrevista.TabIndex = 71
        Me.dtFechaTerminoPrevista.TabStop = False
        '
        'lblFechaTerminoPrevista
        '
        Me.lblFechaTerminoPrevista.AutoSize = True
        Me.lblFechaTerminoPrevista.Location = New System.Drawing.Point(50, 47)
        Me.lblFechaTerminoPrevista.Name = "lblFechaTerminoPrevista"
        Me.lblFechaTerminoPrevista.Size = New System.Drawing.Size(137, 13)
        Me.lblFechaTerminoPrevista.TabIndex = 61
        Me.lblFechaTerminoPrevista.Text = "Fecha de Término Prevista:"
        '
        'lblFechaTerminoReal
        '
        Me.lblFechaTerminoReal.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblFechaTerminoReal.AutoSize = True
        Me.lblFechaTerminoReal.Location = New System.Drawing.Point(510, 47)
        Me.lblFechaTerminoReal.Name = "lblFechaTerminoReal"
        Me.lblFechaTerminoReal.Size = New System.Drawing.Size(121, 13)
        Me.lblFechaTerminoReal.TabIndex = 63
        Me.lblFechaTerminoReal.Text = "Fecha de Término Real:"
        '
        'dtFechaTerminoReal
        '
        Me.dtFechaTerminoReal.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.dtFechaTerminoReal.Enabled = False
        Me.dtFechaTerminoReal.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.dtFechaTerminoReal.Location = New System.Drawing.Point(637, 41)
        Me.dtFechaTerminoReal.Name = "dtFechaTerminoReal"
        Me.dtFechaTerminoReal.Size = New System.Drawing.Size(104, 20)
        Me.dtFechaTerminoReal.TabIndex = 79
        Me.dtFechaTerminoReal.TabStop = False
        '
        'lblPorcentajeDeComisionDeObraReal
        '
        Me.lblPorcentajeDeComisionDeObraReal.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblPorcentajeDeComisionDeObraReal.AutoSize = True
        Me.lblPorcentajeDeComisionDeObraReal.Location = New System.Drawing.Point(537, 122)
        Me.lblPorcentajeDeComisionDeObraReal.Name = "lblPorcentajeDeComisionDeObraReal"
        Me.lblPorcentajeDeComisionDeObraReal.Size = New System.Drawing.Size(116, 13)
        Me.lblPorcentajeDeComisionDeObraReal.TabIndex = 59
        Me.lblPorcentajeDeComisionDeObraReal.Text = "% de Comisión de Obra"
        '
        'txtPorcentajePorCierreDeOperacionPresupuestada
        '
        Me.txtPorcentajePorCierreDeOperacionPresupuestada.Enabled = False
        Me.txtPorcentajePorCierreDeOperacionPresupuestada.Location = New System.Drawing.Point(16, 145)
        Me.txtPorcentajePorCierreDeOperacionPresupuestada.MaxLength = 20
        Me.txtPorcentajePorCierreDeOperacionPresupuestada.Name = "txtPorcentajePorCierreDeOperacionPresupuestada"
        Me.txtPorcentajePorCierreDeOperacionPresupuestada.Size = New System.Drawing.Size(51, 20)
        Me.txtPorcentajePorCierreDeOperacionPresupuestada.TabIndex = 71
        Me.txtPorcentajePorCierreDeOperacionPresupuestada.Text = "0"
        '
        'lblPorcentajePorCierreDeOperacionPresupuestada
        '
        Me.lblPorcentajePorCierreDeOperacionPresupuestada.AutoSize = True
        Me.lblPorcentajePorCierreDeOperacionPresupuestada.Location = New System.Drawing.Point(74, 148)
        Me.lblPorcentajePorCierreDeOperacionPresupuestada.Name = "lblPorcentajePorCierreDeOperacionPresupuestada"
        Me.lblPorcentajePorCierreDeOperacionPresupuestada.Size = New System.Drawing.Size(129, 13)
        Me.lblPorcentajePorCierreDeOperacionPresupuestada.TabIndex = 57
        Me.lblPorcentajePorCierreDeOperacionPresupuestada.Text = "% por cierre de Operación"
        '
        'txtImportePorCierreDeOperacionPresupuestada
        '
        Me.txtImportePorCierreDeOperacionPresupuestada.Enabled = False
        Me.txtImportePorCierreDeOperacionPresupuestada.Location = New System.Drawing.Point(208, 145)
        Me.txtImportePorCierreDeOperacionPresupuestada.MaxLength = 20
        Me.txtImportePorCierreDeOperacionPresupuestada.Name = "txtImportePorCierreDeOperacionPresupuestada"
        Me.txtImportePorCierreDeOperacionPresupuestada.Size = New System.Drawing.Size(100, 20)
        Me.txtImportePorCierreDeOperacionPresupuestada.TabIndex = 79
        Me.txtImportePorCierreDeOperacionPresupuestada.TabStop = False
        '
        'txtPorcentajeDeComisionDeObraPresupuestada
        '
        Me.txtPorcentajeDeComisionDeObraPresupuestada.Enabled = False
        Me.txtPorcentajeDeComisionDeObraPresupuestada.Location = New System.Drawing.Point(16, 122)
        Me.txtPorcentajeDeComisionDeObraPresupuestada.MaxLength = 20
        Me.txtPorcentajeDeComisionDeObraPresupuestada.Name = "txtPorcentajeDeComisionDeObraPresupuestada"
        Me.txtPorcentajeDeComisionDeObraPresupuestada.Size = New System.Drawing.Size(51, 20)
        Me.txtPorcentajeDeComisionDeObraPresupuestada.TabIndex = 70
        Me.txtPorcentajeDeComisionDeObraPresupuestada.Text = "0"
        '
        'lblPorcentajeDeComisionDeObraPresupuestada
        '
        Me.lblPorcentajeDeComisionDeObraPresupuestada.AutoSize = True
        Me.lblPorcentajeDeComisionDeObraPresupuestada.Location = New System.Drawing.Point(87, 125)
        Me.lblPorcentajeDeComisionDeObraPresupuestada.Name = "lblPorcentajeDeComisionDeObraPresupuestada"
        Me.lblPorcentajeDeComisionDeObraPresupuestada.Size = New System.Drawing.Size(116, 13)
        Me.lblPorcentajeDeComisionDeObraPresupuestada.TabIndex = 54
        Me.lblPorcentajeDeComisionDeObraPresupuestada.Text = "% de Comisión de Obra"
        '
        'txtImporteComisionDeObraPresupuestada
        '
        Me.txtImporteComisionDeObraPresupuestada.Enabled = False
        Me.txtImporteComisionDeObraPresupuestada.Location = New System.Drawing.Point(208, 119)
        Me.txtImporteComisionDeObraPresupuestada.MaxLength = 20
        Me.txtImporteComisionDeObraPresupuestada.Name = "txtImporteComisionDeObraPresupuestada"
        Me.txtImporteComisionDeObraPresupuestada.Size = New System.Drawing.Size(100, 20)
        Me.txtImporteComisionDeObraPresupuestada.TabIndex = 78
        Me.txtImporteComisionDeObraPresupuestada.TabStop = False
        '
        'lblPorcentajePrevistoDeUtilidadEnObra
        '
        Me.lblPorcentajePrevistoDeUtilidadEnObra.AutoSize = True
        Me.lblPorcentajePrevistoDeUtilidadEnObra.Location = New System.Drawing.Point(54, 96)
        Me.lblPorcentajePrevistoDeUtilidadEnObra.Name = "lblPorcentajePrevistoDeUtilidadEnObra"
        Me.lblPorcentajePrevistoDeUtilidadEnObra.Size = New System.Drawing.Size(149, 13)
        Me.lblPorcentajePrevistoDeUtilidadEnObra.TabIndex = 52
        Me.lblPorcentajePrevistoDeUtilidadEnObra.Text = "% previsto de Utilidad en Obra"
        '
        'txtPorcentajePrevistoDeUtilidadEnObra
        '
        Me.txtPorcentajePrevistoDeUtilidadEnObra.Enabled = False
        Me.txtPorcentajePrevistoDeUtilidadEnObra.Location = New System.Drawing.Point(208, 93)
        Me.txtPorcentajePrevistoDeUtilidadEnObra.MaxLength = 20
        Me.txtPorcentajePrevistoDeUtilidadEnObra.Name = "txtPorcentajePrevistoDeUtilidadEnObra"
        Me.txtPorcentajePrevistoDeUtilidadEnObra.Size = New System.Drawing.Size(100, 20)
        Me.txtPorcentajePrevistoDeUtilidadEnObra.TabIndex = 77
        Me.txtPorcentajePrevistoDeUtilidadEnObra.TabStop = False
        '
        'lblUtilidadPrevistaDeEjecución
        '
        Me.lblUtilidadPrevistaDeEjecución.AutoSize = True
        Me.lblUtilidadPrevistaDeEjecución.Location = New System.Drawing.Point(15, 70)
        Me.lblUtilidadPrevistaDeEjecución.Name = "lblUtilidadPrevistaDeEjecución"
        Me.lblUtilidadPrevistaDeEjecución.Size = New System.Drawing.Size(188, 13)
        Me.lblUtilidadPrevistaDeEjecución.TabIndex = 46
        Me.lblUtilidadPrevistaDeEjecución.Text = "Utilidad prevista de Ejecución de Obra"
        '
        'txtUtilidadPrevistaDeEjecucion
        '
        Me.txtUtilidadPrevistaDeEjecucion.Enabled = False
        Me.txtUtilidadPrevistaDeEjecucion.Location = New System.Drawing.Point(208, 67)
        Me.txtUtilidadPrevistaDeEjecucion.MaxLength = 20
        Me.txtUtilidadPrevistaDeEjecucion.Name = "txtUtilidadPrevistaDeEjecucion"
        Me.txtUtilidadPrevistaDeEjecucion.Size = New System.Drawing.Size(100, 20)
        Me.txtUtilidadPrevistaDeEjecucion.TabIndex = 76
        Me.txtUtilidadPrevistaDeEjecucion.TabStop = False
        '
        'lblUtilidadFinalPrevista
        '
        Me.lblUtilidadFinalPrevista.AutoSize = True
        Me.lblUtilidadFinalPrevista.Location = New System.Drawing.Point(95, 174)
        Me.lblUtilidadFinalPrevista.Name = "lblUtilidadFinalPrevista"
        Me.lblUtilidadFinalPrevista.Size = New System.Drawing.Size(108, 13)
        Me.lblUtilidadFinalPrevista.TabIndex = 44
        Me.lblUtilidadFinalPrevista.Text = "Utilidad Final Prevista"
        '
        'txtUtilidadFinalPrevista
        '
        Me.txtUtilidadFinalPrevista.Enabled = False
        Me.txtUtilidadFinalPrevista.Location = New System.Drawing.Point(208, 171)
        Me.txtUtilidadFinalPrevista.MaxLength = 20
        Me.txtUtilidadFinalPrevista.Name = "txtUtilidadFinalPrevista"
        Me.txtUtilidadFinalPrevista.Size = New System.Drawing.Size(100, 20)
        Me.txtUtilidadFinalPrevista.TabIndex = 80
        Me.txtUtilidadFinalPrevista.TabStop = False
        '
        'lblCostoPresupuestadoDeLaObra
        '
        Me.lblCostoPresupuestadoDeLaObra.AutoSize = True
        Me.lblCostoPresupuestadoDeLaObra.Location = New System.Drawing.Point(43, 41)
        Me.lblCostoPresupuestadoDeLaObra.Name = "lblCostoPresupuestadoDeLaObra"
        Me.lblCostoPresupuestadoDeLaObra.Size = New System.Drawing.Size(160, 13)
        Me.lblCostoPresupuestadoDeLaObra.TabIndex = 42
        Me.lblCostoPresupuestadoDeLaObra.Text = "Costo Presupuestado de la Obra"
        '
        'txtCostoPresupuestado
        '
        Me.txtCostoPresupuestado.Enabled = False
        Me.txtCostoPresupuestado.Location = New System.Drawing.Point(208, 38)
        Me.txtCostoPresupuestado.MaxLength = 20
        Me.txtCostoPresupuestado.Name = "txtCostoPresupuestado"
        Me.txtCostoPresupuestado.Size = New System.Drawing.Size(100, 20)
        Me.txtCostoPresupuestado.TabIndex = 75
        Me.txtCostoPresupuestado.TabStop = False
        '
        'lblPrecioPresupuestado
        '
        Me.lblPrecioPresupuestado.AutoSize = True
        Me.lblPrecioPresupuestado.Location = New System.Drawing.Point(46, 15)
        Me.lblPrecioPresupuestado.Name = "lblPrecioPresupuestado"
        Me.lblPrecioPresupuestado.Size = New System.Drawing.Size(157, 13)
        Me.lblPrecioPresupuestado.TabIndex = 40
        Me.lblPrecioPresupuestado.Text = "Precio Presupuestado al Cliente"
        '
        'txtPrecioPresupuestado
        '
        Me.txtPrecioPresupuestado.Enabled = False
        Me.txtPrecioPresupuestado.Location = New System.Drawing.Point(208, 12)
        Me.txtPrecioPresupuestado.MaxLength = 20
        Me.txtPrecioPresupuestado.Name = "txtPrecioPresupuestado"
        Me.txtPrecioPresupuestado.Size = New System.Drawing.Size(100, 20)
        Me.txtPrecioPresupuestado.TabIndex = 74
        Me.txtPrecioPresupuestado.TabStop = False
        '
        'txtPorcentajePorCierreDeOperacionReal
        '
        Me.txtPorcentajePorCierreDeOperacionReal.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtPorcentajePorCierreDeOperacionReal.Enabled = False
        Me.txtPorcentajePorCierreDeOperacionReal.Location = New System.Drawing.Point(471, 145)
        Me.txtPorcentajePorCierreDeOperacionReal.MaxLength = 20
        Me.txtPorcentajePorCierreDeOperacionReal.Name = "txtPorcentajePorCierreDeOperacionReal"
        Me.txtPorcentajePorCierreDeOperacionReal.Size = New System.Drawing.Size(51, 20)
        Me.txtPorcentajePorCierreDeOperacionReal.TabIndex = 73
        Me.txtPorcentajePorCierreDeOperacionReal.Text = "0"
        '
        'lblPorcentajePorCierreDeOperacionReal
        '
        Me.lblPorcentajePorCierreDeOperacionReal.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblPorcentajePorCierreDeOperacionReal.AutoSize = True
        Me.lblPorcentajePorCierreDeOperacionReal.Location = New System.Drawing.Point(524, 148)
        Me.lblPorcentajePorCierreDeOperacionReal.Name = "lblPorcentajePorCierreDeOperacionReal"
        Me.lblPorcentajePorCierreDeOperacionReal.Size = New System.Drawing.Size(129, 13)
        Me.lblPorcentajePorCierreDeOperacionReal.TabIndex = 37
        Me.lblPorcentajePorCierreDeOperacionReal.Text = "% por cierre de Operación"
        '
        'txtImportePorCierreDeOperacionReal
        '
        Me.txtImportePorCierreDeOperacionReal.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtImportePorCierreDeOperacionReal.Enabled = False
        Me.txtImportePorCierreDeOperacionReal.Location = New System.Drawing.Point(659, 145)
        Me.txtImportePorCierreDeOperacionReal.MaxLength = 20
        Me.txtImportePorCierreDeOperacionReal.Name = "txtImportePorCierreDeOperacionReal"
        Me.txtImportePorCierreDeOperacionReal.Size = New System.Drawing.Size(104, 20)
        Me.txtImportePorCierreDeOperacionReal.TabIndex = 86
        Me.txtImportePorCierreDeOperacionReal.TabStop = False
        '
        'txtPorcentajeDeComisionDeObraReal
        '
        Me.txtPorcentajeDeComisionDeObraReal.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtPorcentajeDeComisionDeObraReal.Enabled = False
        Me.txtPorcentajeDeComisionDeObraReal.Location = New System.Drawing.Point(471, 122)
        Me.txtPorcentajeDeComisionDeObraReal.MaxLength = 20
        Me.txtPorcentajeDeComisionDeObraReal.Name = "txtPorcentajeDeComisionDeObraReal"
        Me.txtPorcentajeDeComisionDeObraReal.Size = New System.Drawing.Size(51, 20)
        Me.txtPorcentajeDeComisionDeObraReal.TabIndex = 72
        Me.txtPorcentajeDeComisionDeObraReal.Text = "0"
        '
        'txtImporteComisiónDeObraReal
        '
        Me.txtImporteComisiónDeObraReal.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtImporteComisiónDeObraReal.Enabled = False
        Me.txtImporteComisiónDeObraReal.Location = New System.Drawing.Point(659, 119)
        Me.txtImporteComisiónDeObraReal.MaxLength = 20
        Me.txtImporteComisiónDeObraReal.Name = "txtImporteComisiónDeObraReal"
        Me.txtImporteComisiónDeObraReal.Size = New System.Drawing.Size(104, 20)
        Me.txtImporteComisiónDeObraReal.TabIndex = 85
        Me.txtImporteComisiónDeObraReal.TabStop = False
        '
        'lblPorcentajeDeUtilidadEnObraReal
        '
        Me.lblPorcentajeDeUtilidadEnObraReal.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblPorcentajeDeUtilidadEnObraReal.AutoSize = True
        Me.lblPorcentajeDeUtilidadEnObraReal.Location = New System.Drawing.Point(544, 96)
        Me.lblPorcentajeDeUtilidadEnObraReal.Name = "lblPorcentajeDeUtilidadEnObraReal"
        Me.lblPorcentajeDeUtilidadEnObraReal.Size = New System.Drawing.Size(109, 13)
        Me.lblPorcentajeDeUtilidadEnObraReal.TabIndex = 24
        Me.lblPorcentajeDeUtilidadEnObraReal.Text = "% de Utilidad en Obra"
        '
        'txtPorcentajeDeUtilidadEnObraReal
        '
        Me.txtPorcentajeDeUtilidadEnObraReal.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtPorcentajeDeUtilidadEnObraReal.Enabled = False
        Me.txtPorcentajeDeUtilidadEnObraReal.Location = New System.Drawing.Point(659, 93)
        Me.txtPorcentajeDeUtilidadEnObraReal.MaxLength = 20
        Me.txtPorcentajeDeUtilidadEnObraReal.Name = "txtPorcentajeDeUtilidadEnObraReal"
        Me.txtPorcentajeDeUtilidadEnObraReal.Size = New System.Drawing.Size(104, 20)
        Me.txtPorcentajeDeUtilidadEnObraReal.TabIndex = 84
        Me.txtPorcentajeDeUtilidadEnObraReal.TabStop = False
        '
        'lblUtilidadDeEjecucionDeObraReal
        '
        Me.lblUtilidadDeEjecucionDeObraReal.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblUtilidadDeEjecucionDeObraReal.AutoSize = True
        Me.lblUtilidadDeEjecucionDeObraReal.Location = New System.Drawing.Point(505, 70)
        Me.lblUtilidadDeEjecucionDeObraReal.Name = "lblUtilidadDeEjecucionDeObraReal"
        Me.lblUtilidadDeEjecucionDeObraReal.Size = New System.Drawing.Size(148, 13)
        Me.lblUtilidadDeEjecucionDeObraReal.TabIndex = 14
        Me.lblUtilidadDeEjecucionDeObraReal.Text = "Utilidad de Ejecución de Obra"
        '
        'txtUtilidadDeEjecucionDeObraReal
        '
        Me.txtUtilidadDeEjecucionDeObraReal.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtUtilidadDeEjecucionDeObraReal.Enabled = False
        Me.txtUtilidadDeEjecucionDeObraReal.Location = New System.Drawing.Point(659, 67)
        Me.txtUtilidadDeEjecucionDeObraReal.MaxLength = 20
        Me.txtUtilidadDeEjecucionDeObraReal.Name = "txtUtilidadDeEjecucionDeObraReal"
        Me.txtUtilidadDeEjecucionDeObraReal.Size = New System.Drawing.Size(104, 20)
        Me.txtUtilidadDeEjecucionDeObraReal.TabIndex = 83
        Me.txtUtilidadDeEjecucionDeObraReal.TabStop = False
        '
        'lblUtilidadFinalReal
        '
        Me.lblUtilidadFinalReal.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblUtilidadFinalReal.AutoSize = True
        Me.lblUtilidadFinalReal.Location = New System.Drawing.Point(561, 174)
        Me.lblUtilidadFinalReal.Name = "lblUtilidadFinalReal"
        Me.lblUtilidadFinalReal.Size = New System.Drawing.Size(92, 13)
        Me.lblUtilidadFinalReal.TabIndex = 7
        Me.lblUtilidadFinalReal.Text = "Utilidad Final Real"
        '
        'txtUtilidadFinalReal
        '
        Me.txtUtilidadFinalReal.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtUtilidadFinalReal.Enabled = False
        Me.txtUtilidadFinalReal.Location = New System.Drawing.Point(659, 171)
        Me.txtUtilidadFinalReal.MaxLength = 20
        Me.txtUtilidadFinalReal.Name = "txtUtilidadFinalReal"
        Me.txtUtilidadFinalReal.Size = New System.Drawing.Size(104, 20)
        Me.txtUtilidadFinalReal.TabIndex = 87
        Me.txtUtilidadFinalReal.TabStop = False
        '
        'lblCostoRealDeLaObra
        '
        Me.lblCostoRealDeLaObra.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblCostoRealDeLaObra.AutoSize = True
        Me.lblCostoRealDeLaObra.Location = New System.Drawing.Point(542, 41)
        Me.lblCostoRealDeLaObra.Name = "lblCostoRealDeLaObra"
        Me.lblCostoRealDeLaObra.Size = New System.Drawing.Size(111, 13)
        Me.lblCostoRealDeLaObra.TabIndex = 5
        Me.lblCostoRealDeLaObra.Text = "Costo Real de la Obra"
        '
        'txtCostoRealDeLaObra
        '
        Me.txtCostoRealDeLaObra.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtCostoRealDeLaObra.Enabled = False
        Me.txtCostoRealDeLaObra.Location = New System.Drawing.Point(659, 38)
        Me.txtCostoRealDeLaObra.MaxLength = 20
        Me.txtCostoRealDeLaObra.Name = "txtCostoRealDeLaObra"
        Me.txtCostoRealDeLaObra.Size = New System.Drawing.Size(104, 20)
        Me.txtCostoRealDeLaObra.TabIndex = 82
        Me.txtCostoRealDeLaObra.TabStop = False
        '
        'lblEntradas
        '
        Me.lblEntradas.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblEntradas.AutoSize = True
        Me.lblEntradas.Location = New System.Drawing.Point(510, 15)
        Me.lblEntradas.Name = "lblEntradas"
        Me.lblEntradas.Size = New System.Drawing.Size(143, 13)
        Me.lblEntradas.TabIndex = 3
        Me.lblEntradas.Text = "Anticipos y Pagos del Cliente"
        '
        'txtEntradas
        '
        Me.txtEntradas.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtEntradas.Enabled = False
        Me.txtEntradas.Location = New System.Drawing.Point(659, 12)
        Me.txtEntradas.MaxLength = 20
        Me.txtEntradas.Name = "txtEntradas"
        Me.txtEntradas.Size = New System.Drawing.Size(104, 20)
        Me.txtEntradas.TabIndex = 81
        Me.txtEntradas.TabStop = False
        '
        'tpReportes
        '
        Me.tpReportes.Controls.Add(Me.tcReportes)
        Me.tpReportes.Location = New System.Drawing.Point(4, 22)
        Me.tpReportes.Name = "tpReportes"
        Me.tpReportes.Size = New System.Drawing.Size(778, 396)
        Me.tpReportes.TabIndex = 6
        Me.tpReportes.Text = "Reportes"
        Me.tpReportes.UseVisualStyleBackColor = True
        '
        'tcReportes
        '
        Me.tcReportes.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tcReportes.Location = New System.Drawing.Point(7, 8)
        Me.tcReportes.Name = "tcReportes"
        Me.tcReportes.SelectedIndex = 0
        Me.tcReportes.Size = New System.Drawing.Size(767, 379)
        Me.tcReportes.TabIndex = 71
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
        Me.btnGuardar.Text = "&Guardar Proyecto"
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
        Me.btnGuardarYCerrar.Text = "G&uardar Proyecto y Cerrar"
        Me.btnGuardarYCerrar.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnGuardarYCerrar.UseVisualStyleBackColor = True
        '
        'NotifyIcon1
        '
        Me.NotifyIcon1.Icon = CType(resources.GetObject("NotifyIcon1.Icon"), System.Drawing.Icon)
        Me.NotifyIcon1.Text = "Oversight"
        '
        'AgregarProyecto
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(799, 475)
        Me.Controls.Add(Me.btnGuardar)
        Me.Controls.Add(Me.btnCancelar)
        Me.Controls.Add(Me.btnGuardarYCerrar)
        Me.Controls.Add(Me.tcProyecto)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "AgregarProyecto"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Proyecto"
        Me.tcProyecto.ResumeLayout(False)
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
        Me.tpExplosionPorProveedor.ResumeLayout(False)
        CType(Me.dgvExplosionDeInsumosPorProveedor, System.ComponentModel.ISupportInitialize).EndInit()
        Me.tpInsumosFacturadosNoPresupuestados.ResumeLayout(False)
        CType(Me.dgvInsumosFacturadosNoPresupuestados, System.ComponentModel.ISupportInitialize).EndInit()
        Me.tpPedidos.ResumeLayout(False)
        CType(Me.dgvPedidosDeObra, System.ComponentModel.ISupportInitialize).EndInit()
        Me.tpEnvios.ResumeLayout(False)
        CType(Me.dgvEnviosALaObra, System.ComponentModel.ISupportInitialize).EndInit()
        Me.tpDetalleCostosReales.ResumeLayout(False)
        Me.tcContables.ResumeLayout(False)
        Me.tpFacturas.ResumeLayout(False)
        Me.tpFacturas.PerformLayout()
        CType(Me.dgvFacturas, System.ComponentModel.ISupportInitialize).EndInit()
        Me.tpCombustible.ResumeLayout(False)
        Me.tpCombustible.PerformLayout()
        CType(Me.dgvVales, System.ComponentModel.ISupportInitialize).EndInit()
        Me.tpGastosAdministrativosAsignados.ResumeLayout(False)
        Me.tpGastosAdministrativosAsignados.PerformLayout()
        CType(Me.dgvFacturasNoAplicablesAProyectosDuranteMismoPeriodo, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.dgvGastosAdminAsociados, System.ComponentModel.ISupportInitialize).EndInit()
        Me.tpNominasYAlimentacion.ResumeLayout(False)
        Me.tpNominasYAlimentacion.PerformLayout()
        CType(Me.dgvNominas, System.ComponentModel.ISupportInitialize).EndInit()
        Me.tpAnticiposYPagos.ResumeLayout(False)
        Me.tpAnticiposYPagos.PerformLayout()
        CType(Me.dgvEntradas, System.ComponentModel.ISupportInitialize).EndInit()
        Me.tpPagosFacturas.ResumeLayout(False)
        Me.tpPagosFacturas.PerformLayout()
        CType(Me.dgvSalidas, System.ComponentModel.ISupportInitialize).EndInit()
        Me.tpAnalisisUtilidades.ResumeLayout(False)
        Me.tpAnalisisUtilidades.PerformLayout()
        Me.gbFechas.ResumeLayout(False)
        Me.gbFechas.PerformLayout()
        Me.tpReportes.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents tcProyecto As System.Windows.Forms.TabControl
    Friend WithEvents tpDatosIniciales As System.Windows.Forms.TabPage
    Friend WithEvents tpResumenTarjetas As System.Windows.Forms.TabPage
    Friend WithEvents btnPaso2 As System.Windows.Forms.Button
    Friend WithEvents tpCostosIndirectos As System.Windows.Forms.TabPage
    Friend WithEvents dgvResumenDeTarjetas As System.Windows.Forms.DataGridView
    Friend WithEvents lblNombreEmpresa As System.Windows.Forms.Label
    Friend WithEvents txtNombreEmpresa As System.Windows.Forms.TextBox
    Friend WithEvents lblLugar As System.Windows.Forms.Label
    Friend WithEvents rbOtro As System.Windows.Forms.RadioButton
    Friend WithEvents rbOficina As System.Windows.Forms.RadioButton
    Friend WithEvents rbCasa As System.Windows.Forms.RadioButton
    Friend WithEvents lblLongitudVivienda As System.Windows.Forms.Label
    Friend WithEvents lblTipoConstruccion As System.Windows.Forms.Label
    Friend WithEvents lblNombreCliente As System.Windows.Forms.Label
    Friend WithEvents lblFechaPresupuesto As System.Windows.Forms.Label
    Friend WithEvents txtLugar As System.Windows.Forms.TextBox
    Friend WithEvents txtLongitudVivienda As System.Windows.Forms.TextBox
    Friend WithEvents txtNombreCliente As System.Windows.Forms.TextBox
    Friend WithEvents dtFechaPresupuesto As System.Windows.Forms.DateTimePicker
    Friend WithEvents lblCostoProyectadoTotal As System.Windows.Forms.Label
    Friend WithEvents txtPrecioProyectadoSubTotal As System.Windows.Forms.TextBox
    Friend WithEvents tpExplosionDeInsumos As System.Windows.Forms.TabPage
    Friend WithEvents tpAnalisisUtilidades As System.Windows.Forms.TabPage
    Friend WithEvents lblPrecioProyectadoTotal As System.Windows.Forms.Label
    Friend WithEvents txtPrecioProyectadoTotal As System.Windows.Forms.TextBox
    Friend WithEvents lblArchivoPlanos As System.Windows.Forms.Label
    Friend WithEvents txtRuta As System.Windows.Forms.TextBox
    Friend WithEvents btnAbrirCarpeta As System.Windows.Forms.Button
    Friend WithEvents dgvExplosionDeInsumos As System.Windows.Forms.DataGridView
    Friend WithEvents tpDetalleCostosReales As System.Windows.Forms.TabPage
    Friend WithEvents btnVerificarInventario As System.Windows.Forms.Button
    Friend WithEvents btnGenerarHojaControlCompras As System.Windows.Forms.Button
    Friend WithEvents btnGenerarExplosion As System.Windows.Forms.Button
    Friend WithEvents btnGenerarContratoWord As System.Windows.Forms.Button
    Friend WithEvents tcContables As System.Windows.Forms.TabControl
    Friend WithEvents tpAnticiposYPagos As System.Windows.Forms.TabPage
    Friend WithEvents tpPagosFacturas As System.Windows.Forms.TabPage
    Friend WithEvents tpFacturas As System.Windows.Forms.TabPage
    Friend WithEvents lblUtilidadFinalReal As System.Windows.Forms.Label
    Friend WithEvents txtUtilidadFinalReal As System.Windows.Forms.TextBox
    Friend WithEvents lblCostoRealDeLaObra As System.Windows.Forms.Label
    Friend WithEvents txtCostoRealDeLaObra As System.Windows.Forms.TextBox
    Friend WithEvents lblEntradas As System.Windows.Forms.Label
    Friend WithEvents txtEntradas As System.Windows.Forms.TextBox
    Friend WithEvents tcInsumosObra As System.Windows.Forms.TabControl
    Friend WithEvents tpExplosion As System.Windows.Forms.TabPage
    Friend WithEvents tpEnvios As System.Windows.Forms.TabPage
    Friend WithEvents tpPedidos As System.Windows.Forms.TabPage
    Friend WithEvents GroupBox6 As System.Windows.Forms.GroupBox
    Friend WithEvents btnCostoHoy As System.Windows.Forms.Button
    Friend WithEvents btnPaso3 As System.Windows.Forms.Button
    Friend WithEvents btnActualizarPrecios As System.Windows.Forms.Button
    Friend WithEvents dgvPedidosDeObra As System.Windows.Forms.DataGridView
    Friend WithEvents dgvEnviosALaObra As System.Windows.Forms.DataGridView
    Friend WithEvents dgvEntradas As System.Windows.Forms.DataGridView
    Friend WithEvents dgvSalidas As System.Windows.Forms.DataGridView
    Friend WithEvents dgvFacturas As System.Windows.Forms.DataGridView
    Friend WithEvents txtPorcentajePorCierreDeOperacionReal As System.Windows.Forms.TextBox
    Friend WithEvents lblPorcentajePorCierreDeOperacionReal As System.Windows.Forms.Label
    Friend WithEvents txtImportePorCierreDeOperacionReal As System.Windows.Forms.TextBox
    Friend WithEvents txtPorcentajeDeComisionDeObraReal As System.Windows.Forms.TextBox
    Friend WithEvents txtImporteComisiónDeObraReal As System.Windows.Forms.TextBox
    Friend WithEvents lblPorcentajeDeUtilidadEnObraReal As System.Windows.Forms.Label
    Friend WithEvents txtPorcentajeDeUtilidadEnObraReal As System.Windows.Forms.TextBox
    Friend WithEvents lblUtilidadDeEjecucionDeObraReal As System.Windows.Forms.Label
    Friend WithEvents txtUtilidadDeEjecucionDeObraReal As System.Windows.Forms.TextBox
    Friend WithEvents txtPorcentajePorCierreDeOperacionPresupuestada As System.Windows.Forms.TextBox
    Friend WithEvents lblPorcentajePorCierreDeOperacionPresupuestada As System.Windows.Forms.Label
    Friend WithEvents txtImportePorCierreDeOperacionPresupuestada As System.Windows.Forms.TextBox
    Friend WithEvents txtPorcentajeDeComisionDeObraPresupuestada As System.Windows.Forms.TextBox
    Friend WithEvents lblPorcentajeDeComisionDeObraPresupuestada As System.Windows.Forms.Label
    Friend WithEvents txtImporteComisionDeObraPresupuestada As System.Windows.Forms.TextBox
    Friend WithEvents lblPorcentajePrevistoDeUtilidadEnObra As System.Windows.Forms.Label
    Friend WithEvents txtPorcentajePrevistoDeUtilidadEnObra As System.Windows.Forms.TextBox
    Friend WithEvents lblUtilidadPrevistaDeEjecución As System.Windows.Forms.Label
    Friend WithEvents txtUtilidadPrevistaDeEjecucion As System.Windows.Forms.TextBox
    Friend WithEvents lblUtilidadFinalPrevista As System.Windows.Forms.Label
    Friend WithEvents txtUtilidadFinalPrevista As System.Windows.Forms.TextBox
    Friend WithEvents lblCostoPresupuestadoDeLaObra As System.Windows.Forms.Label
    Friend WithEvents txtCostoPresupuestado As System.Windows.Forms.TextBox
    Friend WithEvents lblPrecioPresupuestado As System.Windows.Forms.Label
    Friend WithEvents txtPrecioPresupuestado As System.Windows.Forms.TextBox
    Friend WithEvents lblPorcentajeDeComisionDeObraReal As System.Windows.Forms.Label
    Friend WithEvents lblNombreDelProyecto As System.Windows.Forms.Label
    Friend WithEvents txtNombreProyecto As System.Windows.Forms.TextBox
    Friend WithEvents lblIva As System.Windows.Forms.Label
    Friend WithEvents txtIVA As System.Windows.Forms.TextBox
    Friend WithEvents flpAccionesResumen As System.Windows.Forms.FlowLayoutPanel
    Friend WithEvents lblPercentageSignHelper As System.Windows.Forms.Label
    Friend WithEvents txtPorcentajeIVA As System.Windows.Forms.TextBox
    Friend WithEvents btnPresupuestosDeCompras As System.Windows.Forms.Button
    Friend WithEvents lblFechaTerminoReal As System.Windows.Forms.Label
    Friend WithEvents dtFechaTerminoReal As System.Windows.Forms.DateTimePicker
    Friend WithEvents lblFechaTerminoPrevista As System.Windows.Forms.Label
    Friend WithEvents dtFechaTerminoPrevista As System.Windows.Forms.DateTimePicker
    Friend WithEvents lblStatusProyecto As System.Windows.Forms.Label
    Friend WithEvents btnClientes As System.Windows.Forms.Button
    Friend WithEvents lblLongitudTerreno As System.Windows.Forms.Label
    Friend WithEvents txtLongitudTerreno As System.Windows.Forms.TextBox
    Friend WithEvents btnActualizarUtilidad As System.Windows.Forms.Button
    Friend WithEvents btnGenerarArchivoExcel As System.Windows.Forms.Button
    Friend WithEvents FlowLayoutPanel1 As System.Windows.Forms.FlowLayoutPanel
    Friend WithEvents btnRealizarObra As System.Windows.Forms.Button
    Friend WithEvents msSaveFileDialog As System.Windows.Forms.SaveFileDialog
    Friend WithEvents lblAnchoVivienda As System.Windows.Forms.Label
    Friend WithEvents lblmts2 As System.Windows.Forms.Label
    Friend WithEvents lblmts1 As System.Windows.Forms.Label
    Friend WithEvents txtAnchoVivienda As System.Windows.Forms.TextBox
    Friend WithEvents lblmts4 As System.Windows.Forms.Label
    Friend WithEvents txtAnchoTerreno As System.Windows.Forms.TextBox
    Friend WithEvents lblAnchoTerreno As System.Windows.Forms.Label
    Friend WithEvents lblmts3 As System.Windows.Forms.Label
    Friend WithEvents dgvCostosIndirectos As System.Windows.Forms.DataGridView
    Friend WithEvents lblTotalIndirectos As System.Windows.Forms.Label
    Friend WithEvents lblTotalIndirectosLbl As System.Windows.Forms.Label
    Friend WithEvents tpReportes As System.Windows.Forms.TabPage
    Friend WithEvents btnTerminarObra As System.Windows.Forms.Button
    Friend WithEvents btnDireccion As System.Windows.Forms.Button
    Friend WithEvents lblPorcentajeIndirectos As System.Windows.Forms.Label
    Friend WithEvents lblPorcentajeIndirectosLbl As System.Windows.Forms.Label
    Friend WithEvents txtIngresosIndirectos As System.Windows.Forms.TextBox
    Friend WithEvents lblIngresosIndirectos As System.Windows.Forms.Label
    Friend WithEvents lblTotalFacturas As System.Windows.Forms.Label
    Friend WithEvents lblTotalFacturasLbl As System.Windows.Forms.Label
    Friend WithEvents lblTotalEntradas As System.Windows.Forms.Label
    Friend WithEvents lblTotalEntradaslbl As System.Windows.Forms.Label
    Friend WithEvents lblTotalSalidas As System.Windows.Forms.Label
    Friend WithEvents lblTotalSalidaslbl As System.Windows.Forms.Label
    Friend WithEvents tcReportes As System.Windows.Forms.TabControl
    Friend WithEvents btnCancelar As System.Windows.Forms.Button
    Friend WithEvents btnGuardarYCerrar As System.Windows.Forms.Button
    Friend WithEvents btnGuardar As System.Windows.Forms.Button
    Friend WithEvents btnEliminarTarjeta As System.Windows.Forms.Button
    Friend WithEvents btnInsertarTarjeta As System.Windows.Forms.Button
    Friend WithEvents btnNuevaTarjeta As System.Windows.Forms.Button
    Friend WithEvents btnEliminarCostoIndirecto As System.Windows.Forms.Button
    Friend WithEvents btnNuevoCostoIndirecto As System.Windows.Forms.Button
    Friend WithEvents btnEliminarInsumo As System.Windows.Forms.Button
    Friend WithEvents btnInsertarInsumo As System.Windows.Forms.Button
    Friend WithEvents btnNuevoInsumo As System.Windows.Forms.Button
    Friend WithEvents chkDoNOTApplyTax As System.Windows.Forms.CheckBox
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
    Friend WithEvents dtFechaInicio1 As System.Windows.Forms.DateTimePicker
    Friend WithEvents gbFechas As System.Windows.Forms.GroupBox
    Friend WithEvents lblFechaInicio1 As System.Windows.Forms.Label
    Friend WithEvents lblFechaInicio2 As System.Windows.Forms.Label
    Friend WithEvents dtFechaInicio2 As System.Windows.Forms.DateTimePicker
    Friend WithEvents lblStatusProyectoLbl As System.Windows.Forms.Label
    Friend WithEvents lblPorcentajeDeAvanceDeObraLbl As System.Windows.Forms.Label
    Friend WithEvents lblPorcentajeDeAvanceDeObra As System.Windows.Forms.Label
    Friend WithEvents tpCombustible As System.Windows.Forms.TabPage
    Friend WithEvents lblTotalVales As System.Windows.Forms.Label
    Friend WithEvents lblTotalValeslbl As System.Windows.Forms.Label
    Friend WithEvents dgvVales As System.Windows.Forms.DataGridView
    Friend WithEvents tpNominasYAlimentacion As System.Windows.Forms.TabPage
    Friend WithEvents lblTotalNominas As System.Windows.Forms.Label
    Friend WithEvents lblTotalNominaslbl As System.Windows.Forms.Label
    Friend WithEvents dgvNominas As System.Windows.Forms.DataGridView
    Friend WithEvents tpGastosAdministrativosAsignados As System.Windows.Forms.TabPage
    Friend WithEvents lblTotalGastosAdminAsignados As System.Windows.Forms.Label
    Friend WithEvents btnEliminarGastoAdminAsignado As System.Windows.Forms.Button
    Friend WithEvents lblTotalGastosAdminAsignadoslbl As System.Windows.Forms.Label
    Friend WithEvents btnNuevoGastoAdminAsignado As System.Windows.Forms.Button
    Friend WithEvents dgvGastosAdminAsociados As System.Windows.Forms.DataGridView
    Friend WithEvents lblGastosAdminAsignados As System.Windows.Forms.Label
    Friend WithEvents lblFacturasNoAplicablesAProyectosDuranteMismoPeriodo As System.Windows.Forms.Label
    Friend WithEvents lblTotalFacturasNoAplicablesAProyectosDuranteMismoPeriodo As System.Windows.Forms.Label
    Friend WithEvents lblTotalFacturasNoAplicablesAProyectosDuranteMismoPeriodolbl As System.Windows.Forms.Label
    Friend WithEvents dgvFacturasNoAplicablesAProyectosDuranteMismoPeriodo As System.Windows.Forms.DataGridView
    Friend WithEvents lblSeparator As System.Windows.Forms.Label
    Friend WithEvents tpExplosionPorProveedor As System.Windows.Forms.TabPage
    Friend WithEvents dgvExplosionDeInsumosPorProveedor As System.Windows.Forms.DataGridView
    Friend WithEvents tpInsumosFacturadosNoPresupuestados As System.Windows.Forms.TabPage
    Friend WithEvents dgvInsumosFacturadosNoPresupuestados As System.Windows.Forms.DataGridView

End Class
