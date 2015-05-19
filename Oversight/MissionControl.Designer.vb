<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class MissionControl
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(MissionControl))
        Me.msMain = New System.Windows.Forms.MenuStrip
        Me.mnuFile = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuFiletsmiNew = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuFiletsmiOpen = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuFiletss1 = New System.Windows.Forms.ToolStripSeparator
        Me.mnuFiletsmiSave = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuFiletss2 = New System.Windows.Forms.ToolStripSeparator
        Me.mnuFiletsmiPrint = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuFiletsmiPrintPreview = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuFiletsmiPrintSetup = New System.Windows.Forms.ToolStripMenuItem
        Me.tsmiRecentlyOpenedFiles = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuFiletss3 = New System.Windows.Forms.ToolStripSeparator
        Me.mnuFiletsmiLogout = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuFiletsmiExit = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuEdit = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuEdittsmiUndo = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuEdittsmiRedo = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuEdittss1 = New System.Windows.Forms.ToolStripSeparator
        Me.mnuEdittsmiCut = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuEdittsmiCopy = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuEdittsmiPaste = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuEdittss2 = New System.Windows.Forms.ToolStripSeparator
        Me.mnuEdittsmiSelectAll = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuView = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuViewtsmiGeneralBar = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuViewtsmiStatusBar = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuTools = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuToolstsmiModifyMyUser = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuToolstsmiOptions = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuWindows = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuWindowtsmiNewWindow = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuWindowtsmiCascade = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuWindowtsmiTileVertical = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuWindowtsmiTileHorizontal = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuWindowtsmiCloseAll = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuWindowtsmiArrangeIcons = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuHelp = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuHelptsmiContents = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuHelptsmiIndex = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuHelptsmiSearch = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuHelptss1 = New System.Windows.Forms.ToolStripSeparator
        Me.mnuHelptsmiAbout = New System.Windows.Forms.ToolStripMenuItem
        Me.tsGeneral = New System.Windows.Forms.ToolStrip
        Me.tsGeneraltsbNuevo = New System.Windows.Forms.ToolStripButton
        Me.tsGeneraltsbAbrir = New System.Windows.Forms.ToolStripButton
        Me.tsGeneraltsbGuardar = New System.Windows.Forms.ToolStripButton
        Me.tsGeneraltss1 = New System.Windows.Forms.ToolStripSeparator
        Me.tsGeneraltsbImprimir = New System.Windows.Forms.ToolStripButton
        Me.tsGeneraltsbVistaPrevia = New System.Windows.Forms.ToolStripButton
        Me.tsGeneraltss2 = New System.Windows.Forms.ToolStripSeparator
        Me.tsGeneraltsbAyuda = New System.Windows.Forms.ToolStripButton
        Me.ssMain = New System.Windows.Forms.StatusStrip
        Me.tsStatusLabel = New System.Windows.Forms.ToolStripStatusLabel
        Me.ttMain = New System.Windows.Forms.ToolTip(Me.components)
        Me.tsProyectos = New System.Windows.Forms.ToolStrip
        Me.tsbProyectos = New System.Windows.Forms.ToolStripDropDownButton
        Me.tsmiVerProyectos = New System.Windows.Forms.ToolStripMenuItem
        Me.tsmiNuevoProyecto = New System.Windows.Forms.ToolStripMenuItem
        Me.tsmiNuevoProyectoDesdeModelo = New System.Windows.Forms.ToolStripMenuItem
        Me.tsmiDuplicarProyecto = New System.Windows.Forms.ToolStripMenuItem
        Me.tsmiEliminarProyecto = New System.Windows.Forms.ToolStripMenuItem
        Me.tsbModelos = New System.Windows.Forms.ToolStripDropDownButton
        Me.tsmiVerModelos = New System.Windows.Forms.ToolStripMenuItem
        Me.tsmiNuevoModelo = New System.Windows.Forms.ToolStripMenuItem
        Me.tsmiEliminarModelo = New System.Windows.Forms.ToolStripMenuItem
        Me.tsbPresupuestosBase = New System.Windows.Forms.ToolStripDropDownButton
        Me.tsmiVerPresupuestosBase = New System.Windows.Forms.ToolStripMenuItem
        Me.tsmiNuevoPresupuestoBase = New System.Windows.Forms.ToolStripMenuItem
        Me.tsmiEliminarPresupuestoBase = New System.Windows.Forms.ToolStripMenuItem
        Me.tsbMateriales = New System.Windows.Forms.ToolStripDropDownButton
        Me.tsmiVerMaterialesPrecios = New System.Windows.Forms.ToolStripMenuItem
        Me.tsmiNuevoMaterial = New System.Windows.Forms.ToolStripMenuItem
        Me.tsmiEliminarMaterial = New System.Windows.Forms.ToolStripMenuItem
        Me.tsbCotizacionesMateriales = New System.Windows.Forms.ToolStripDropDownButton
        Me.tsmiVerCotizacionesDeMateriales = New System.Windows.Forms.ToolStripMenuItem
        Me.tsmiNuevaCotizacionDeMateriales = New System.Windows.Forms.ToolStripMenuItem
        Me.tsbPedidos = New System.Windows.Forms.ToolStripDropDownButton
        Me.tsmiVerPedidosDeMateriales = New System.Windows.Forms.ToolStripMenuItem
        Me.tsmiNuevoPedidoDeMaterial = New System.Windows.Forms.ToolStripMenuItem
        Me.tsmiEliminarPedidosDeMaterial = New System.Windows.Forms.ToolStripMenuItem
        Me.tsbEnvios = New System.Windows.Forms.ToolStripDropDownButton
        Me.tsmiVerEnvíosDeMaterial = New System.Windows.Forms.ToolStripMenuItem
        Me.tsmiNuevoEnvíoDeMaterial = New System.Windows.Forms.ToolStripMenuItem
        Me.tsmiEliminarEnvíoDeMaterial = New System.Windows.Forms.ToolStripMenuItem
        Me.tsbActivos = New System.Windows.Forms.ToolStripDropDownButton
        Me.tsmiVerActivos = New System.Windows.Forms.ToolStripMenuItem
        Me.tsmiNuevoActivo = New System.Windows.Forms.ToolStripMenuItem
        Me.tsmiEliminarActivo = New System.Windows.Forms.ToolStripMenuItem
        Me.tsbInventarios = New System.Windows.Forms.ToolStripDropDownButton
        Me.tsmiVerInventarioDeMateriales = New System.Windows.Forms.ToolStripMenuItem
        Me.tsmiVerInventarioDeActivos = New System.Windows.Forms.ToolStripMenuItem
        Me.tsbDirectorio = New System.Windows.Forms.ToolStripDropDownButton
        Me.tsmiVerDirectorio = New System.Windows.Forms.ToolStripMenuItem
        Me.tsmiNuevaPersonaProveedor = New System.Windows.Forms.ToolStripMenuItem
        Me.tsmiEliminarPersonaProveedor = New System.Windows.Forms.ToolStripMenuItem
        Me.tsbProveedores = New System.Windows.Forms.ToolStripDropDownButton
        Me.tsmiVerFacturasDeProveedores = New System.Windows.Forms.ToolStripMenuItem
        Me.tsmiNuevaFacturaDeProveedor = New System.Windows.Forms.ToolStripMenuItem
        Me.tsmiEliminarFacturaDeProveedor = New System.Windows.Forms.ToolStripMenuItem
        Me.ToolStripSeparator1 = New System.Windows.Forms.ToolStripSeparator
        Me.tsmiRevisarFacturasDeProveedores = New System.Windows.Forms.ToolStripMenuItem
        Me.tsbVales = New System.Windows.Forms.ToolStripDropDownButton
        Me.tsmiVerValesDeGasolina = New System.Windows.Forms.ToolStripMenuItem
        Me.tsmiNuevoValeDeGasolina = New System.Windows.Forms.ToolStripMenuItem
        Me.tsmiEliminarValeDeGasolina = New System.Windows.Forms.ToolStripMenuItem
        Me.tsVales = New System.Windows.Forms.ToolStripSeparator
        Me.tsmiVerFacturaCombustibleVales = New System.Windows.Forms.ToolStripMenuItem
        Me.tsmiNuevaFacturaCombustibleVales = New System.Windows.Forms.ToolStripMenuItem
        Me.tsmiEliminarFacturaCombustibleVales = New System.Windows.Forms.ToolStripMenuItem
        Me.tsbCasetas = New System.Windows.Forms.ToolStripDropDownButton
        Me.tsmiVerGastosPorCasetas = New System.Windows.Forms.ToolStripMenuItem
        Me.tsmiNuevoGastoPorCaseta = New System.Windows.Forms.ToolStripMenuItem
        Me.tsmiEliminarGastoPorCaseta = New System.Windows.Forms.ToolStripMenuItem
        Me.tsbNominas = New System.Windows.Forms.ToolStripDropDownButton
        Me.tsmiVerNominas = New System.Windows.Forms.ToolStripMenuItem
        Me.tsmiNuevaNomina = New System.Windows.Forms.ToolStripMenuItem
        Me.tsmiEliminarNomina = New System.Windows.Forms.ToolStripMenuItem
        Me.tsbIngresos = New System.Windows.Forms.ToolStripDropDownButton
        Me.tsmiVerIngresos = New System.Windows.Forms.ToolStripMenuItem
        Me.tsmiNuevoIngreso = New System.Windows.Forms.ToolStripMenuItem
        Me.tsmiEliminarIngreso = New System.Windows.Forms.ToolStripMenuItem
        Me.tsbIngresoSep1 = New System.Windows.Forms.ToolStripSeparator
        Me.tsmiVerPagos = New System.Windows.Forms.ToolStripMenuItem
        Me.tsmiNuevoPago = New System.Windows.Forms.ToolStripMenuItem
        Me.tsmiEliminarPago = New System.Windows.Forms.ToolStripMenuItem
        Me.tsbPolizas = New System.Windows.Forms.ToolStripDropDownButton
        Me.tsmiVerPolizas = New System.Windows.Forms.ToolStripMenuItem
        Me.tsmiNuevaPoliza = New System.Windows.Forms.ToolStripMenuItem
        Me.tsmiEliminarPoliza = New System.Windows.Forms.ToolStripMenuItem
        Me.tsbCuentas = New System.Windows.Forms.ToolStripDropDownButton
        Me.tsmiVerCuentas = New System.Windows.Forms.ToolStripMenuItem
        Me.tsmiNuevaCuenta = New System.Windows.Forms.ToolStripMenuItem
        Me.tsmiEliminarCuenta = New System.Windows.Forms.ToolStripMenuItem
        Me.tsContabilidadtss3 = New System.Windows.Forms.ToolStripSeparator
        Me.tsmiVerSaldoEnCuentas = New System.Windows.Forms.ToolStripMenuItem
        Me.tsbFacturasMRD = New System.Windows.Forms.ToolStripDropDownButton
        Me.tsmiVerFacturasEmitidas = New System.Windows.Forms.ToolStripMenuItem
        Me.tsmiNuevaFactura = New System.Windows.Forms.ToolStripMenuItem
        Me.tsmiEliminarFactura = New System.Windows.Forms.ToolStripMenuItem
        Me.tsbReportes = New System.Windows.Forms.ToolStripDropDownButton
        Me.tsmiVerReportes = New System.Windows.Forms.ToolStripMenuItem
        Me.tsbMensajes = New System.Windows.Forms.ToolStripDropDownButton
        Me.tsmiVerMensajes = New System.Windows.Forms.ToolStripMenuItem
        Me.tsmiNuevoMensaje = New System.Windows.Forms.ToolStripMenuItem
        Me.tsbUsuarios = New System.Windows.Forms.ToolStripDropDownButton
        Me.tsmiVerUsuarios = New System.Windows.Forms.ToolStripMenuItem
        Me.tsmiNuevoUsuario = New System.Windows.Forms.ToolStripMenuItem
        Me.tsmiEliminarUsuario = New System.Windows.Forms.ToolStripMenuItem
        Me.tsbUnidades = New System.Windows.Forms.ToolStripDropDownButton
        Me.tsmiVerUnidades = New System.Windows.Forms.ToolStripMenuItem
        Me.tsmiNuevaUnidad = New System.Windows.Forms.ToolStripMenuItem
        Me.tsmiEliminarUnidad = New System.Windows.Forms.ToolStripMenuItem
        Me.NotifyIcon1 = New System.Windows.Forms.NotifyIcon(Me.components)
        Me.msMain.SuspendLayout()
        Me.tsGeneral.SuspendLayout()
        Me.ssMain.SuspendLayout()
        Me.tsProyectos.SuspendLayout()
        Me.SuspendLayout()
        '
        'msMain
        '
        Me.msMain.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuFile, Me.mnuEdit, Me.mnuView, Me.mnuTools, Me.mnuWindows, Me.mnuHelp})
        Me.msMain.Location = New System.Drawing.Point(0, 0)
        Me.msMain.MdiWindowListItem = Me.mnuWindows
        Me.msMain.Name = "msMain"
        Me.msMain.Size = New System.Drawing.Size(840, 24)
        Me.msMain.TabIndex = 5
        Me.msMain.Text = "MenuStrip"
        '
        'mnuFile
        '
        Me.mnuFile.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuFiletsmiNew, Me.mnuFiletsmiOpen, Me.mnuFiletss1, Me.mnuFiletsmiSave, Me.mnuFiletss2, Me.mnuFiletsmiPrint, Me.mnuFiletsmiPrintPreview, Me.mnuFiletsmiPrintSetup, Me.tsmiRecentlyOpenedFiles, Me.mnuFiletss3, Me.mnuFiletsmiLogout, Me.mnuFiletsmiExit})
        Me.mnuFile.ImageTransparentColor = System.Drawing.SystemColors.ActiveBorder
        Me.mnuFile.Name = "mnuFile"
        Me.mnuFile.Size = New System.Drawing.Size(55, 20)
        Me.mnuFile.Text = "&Archivo"
        '
        'mnuFiletsmiNew
        '
        Me.mnuFiletsmiNew.Image = CType(resources.GetObject("mnuFiletsmiNew.Image"), System.Drawing.Image)
        Me.mnuFiletsmiNew.ImageTransparentColor = System.Drawing.Color.Black
        Me.mnuFiletsmiNew.Name = "mnuFiletsmiNew"
        Me.mnuFiletsmiNew.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.N), System.Windows.Forms.Keys)
        Me.mnuFiletsmiNew.Size = New System.Drawing.Size(199, 22)
        Me.mnuFiletsmiNew.Text = "&Nuevo"
        Me.mnuFiletsmiNew.Visible = False
        '
        'mnuFiletsmiOpen
        '
        Me.mnuFiletsmiOpen.Image = CType(resources.GetObject("mnuFiletsmiOpen.Image"), System.Drawing.Image)
        Me.mnuFiletsmiOpen.ImageTransparentColor = System.Drawing.Color.Black
        Me.mnuFiletsmiOpen.Name = "mnuFiletsmiOpen"
        Me.mnuFiletsmiOpen.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.O), System.Windows.Forms.Keys)
        Me.mnuFiletsmiOpen.Size = New System.Drawing.Size(199, 22)
        Me.mnuFiletsmiOpen.Text = "&Abrir"
        Me.mnuFiletsmiOpen.Visible = False
        '
        'mnuFiletss1
        '
        Me.mnuFiletss1.Name = "mnuFiletss1"
        Me.mnuFiletss1.Size = New System.Drawing.Size(196, 6)
        Me.mnuFiletss1.Visible = False
        '
        'mnuFiletsmiSave
        '
        Me.mnuFiletsmiSave.Image = CType(resources.GetObject("mnuFiletsmiSave.Image"), System.Drawing.Image)
        Me.mnuFiletsmiSave.ImageTransparentColor = System.Drawing.Color.Black
        Me.mnuFiletsmiSave.Name = "mnuFiletsmiSave"
        Me.mnuFiletsmiSave.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.S), System.Windows.Forms.Keys)
        Me.mnuFiletsmiSave.Size = New System.Drawing.Size(199, 22)
        Me.mnuFiletsmiSave.Text = "&Guardar"
        Me.mnuFiletsmiSave.Visible = False
        '
        'mnuFiletss2
        '
        Me.mnuFiletss2.Name = "mnuFiletss2"
        Me.mnuFiletss2.Size = New System.Drawing.Size(196, 6)
        Me.mnuFiletss2.Visible = False
        '
        'mnuFiletsmiPrint
        '
        Me.mnuFiletsmiPrint.Image = CType(resources.GetObject("mnuFiletsmiPrint.Image"), System.Drawing.Image)
        Me.mnuFiletsmiPrint.ImageTransparentColor = System.Drawing.Color.Black
        Me.mnuFiletsmiPrint.Name = "mnuFiletsmiPrint"
        Me.mnuFiletsmiPrint.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.P), System.Windows.Forms.Keys)
        Me.mnuFiletsmiPrint.Size = New System.Drawing.Size(199, 22)
        Me.mnuFiletsmiPrint.Text = "&Imprimir"
        Me.mnuFiletsmiPrint.Visible = False
        '
        'mnuFiletsmiPrintPreview
        '
        Me.mnuFiletsmiPrintPreview.Image = CType(resources.GetObject("mnuFiletsmiPrintPreview.Image"), System.Drawing.Image)
        Me.mnuFiletsmiPrintPreview.ImageTransparentColor = System.Drawing.Color.Black
        Me.mnuFiletsmiPrintPreview.Name = "mnuFiletsmiPrintPreview"
        Me.mnuFiletsmiPrintPreview.Size = New System.Drawing.Size(199, 22)
        Me.mnuFiletsmiPrintPreview.Text = "&Vista Previa"
        Me.mnuFiletsmiPrintPreview.Visible = False
        '
        'mnuFiletsmiPrintSetup
        '
        Me.mnuFiletsmiPrintSetup.Name = "mnuFiletsmiPrintSetup"
        Me.mnuFiletsmiPrintSetup.Size = New System.Drawing.Size(199, 22)
        Me.mnuFiletsmiPrintSetup.Text = "&Configuración Impresora"
        Me.mnuFiletsmiPrintSetup.Visible = False
        '
        'tsmiRecentlyOpenedFiles
        '
        Me.tsmiRecentlyOpenedFiles.Name = "tsmiRecentlyOpenedFiles"
        Me.tsmiRecentlyOpenedFiles.Size = New System.Drawing.Size(199, 22)
        Me.tsmiRecentlyOpenedFiles.Text = "Ver Archivos Recientes"
        '
        'mnuFiletss3
        '
        Me.mnuFiletss3.Name = "mnuFiletss3"
        Me.mnuFiletss3.Size = New System.Drawing.Size(196, 6)
        Me.mnuFiletss3.Visible = False
        '
        'mnuFiletsmiLogout
        '
        Me.mnuFiletsmiLogout.Name = "mnuFiletsmiLogout"
        Me.mnuFiletsmiLogout.Size = New System.Drawing.Size(199, 22)
        Me.mnuFiletsmiLogout.Text = "&Salir de Sesión"
        '
        'mnuFiletsmiExit
        '
        Me.mnuFiletsmiExit.Name = "mnuFiletsmiExit"
        Me.mnuFiletsmiExit.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.Q), System.Windows.Forms.Keys)
        Me.mnuFiletsmiExit.Size = New System.Drawing.Size(199, 22)
        Me.mnuFiletsmiExit.Text = "Salir de Aplicación"
        '
        'mnuEdit
        '
        Me.mnuEdit.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuEdittsmiUndo, Me.mnuEdittsmiRedo, Me.mnuEdittss1, Me.mnuEdittsmiCut, Me.mnuEdittsmiCopy, Me.mnuEdittsmiPaste, Me.mnuEdittss2, Me.mnuEdittsmiSelectAll})
        Me.mnuEdit.Name = "mnuEdit"
        Me.mnuEdit.Size = New System.Drawing.Size(52, 20)
        Me.mnuEdit.Text = "&Edición"
        Me.mnuEdit.Visible = False
        '
        'mnuEdittsmiUndo
        '
        Me.mnuEdittsmiUndo.Image = CType(resources.GetObject("mnuEdittsmiUndo.Image"), System.Drawing.Image)
        Me.mnuEdittsmiUndo.ImageTransparentColor = System.Drawing.Color.Black
        Me.mnuEdittsmiUndo.Name = "mnuEdittsmiUndo"
        Me.mnuEdittsmiUndo.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.Z), System.Windows.Forms.Keys)
        Me.mnuEdittsmiUndo.Size = New System.Drawing.Size(193, 22)
        Me.mnuEdittsmiUndo.Text = "&Deshacer"
        Me.mnuEdittsmiUndo.Visible = False
        '
        'mnuEdittsmiRedo
        '
        Me.mnuEdittsmiRedo.Image = CType(resources.GetObject("mnuEdittsmiRedo.Image"), System.Drawing.Image)
        Me.mnuEdittsmiRedo.ImageTransparentColor = System.Drawing.Color.Black
        Me.mnuEdittsmiRedo.Name = "mnuEdittsmiRedo"
        Me.mnuEdittsmiRedo.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.Y), System.Windows.Forms.Keys)
        Me.mnuEdittsmiRedo.Size = New System.Drawing.Size(193, 22)
        Me.mnuEdittsmiRedo.Text = "&Rehacer"
        Me.mnuEdittsmiRedo.Visible = False
        '
        'mnuEdittss1
        '
        Me.mnuEdittss1.Name = "mnuEdittss1"
        Me.mnuEdittss1.Size = New System.Drawing.Size(190, 6)
        '
        'mnuEdittsmiCut
        '
        Me.mnuEdittsmiCut.Image = CType(resources.GetObject("mnuEdittsmiCut.Image"), System.Drawing.Image)
        Me.mnuEdittsmiCut.ImageTransparentColor = System.Drawing.Color.Black
        Me.mnuEdittsmiCut.Name = "mnuEdittsmiCut"
        Me.mnuEdittsmiCut.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.X), System.Windows.Forms.Keys)
        Me.mnuEdittsmiCut.Size = New System.Drawing.Size(193, 22)
        Me.mnuEdittsmiCut.Text = "Cor&tar"
        Me.mnuEdittsmiCut.Visible = False
        '
        'mnuEdittsmiCopy
        '
        Me.mnuEdittsmiCopy.Image = CType(resources.GetObject("mnuEdittsmiCopy.Image"), System.Drawing.Image)
        Me.mnuEdittsmiCopy.ImageTransparentColor = System.Drawing.Color.Black
        Me.mnuEdittsmiCopy.Name = "mnuEdittsmiCopy"
        Me.mnuEdittsmiCopy.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.C), System.Windows.Forms.Keys)
        Me.mnuEdittsmiCopy.Size = New System.Drawing.Size(193, 22)
        Me.mnuEdittsmiCopy.Text = "&Copiar"
        Me.mnuEdittsmiCopy.Visible = False
        '
        'mnuEdittsmiPaste
        '
        Me.mnuEdittsmiPaste.Image = CType(resources.GetObject("mnuEdittsmiPaste.Image"), System.Drawing.Image)
        Me.mnuEdittsmiPaste.ImageTransparentColor = System.Drawing.Color.Black
        Me.mnuEdittsmiPaste.Name = "mnuEdittsmiPaste"
        Me.mnuEdittsmiPaste.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.V), System.Windows.Forms.Keys)
        Me.mnuEdittsmiPaste.Size = New System.Drawing.Size(193, 22)
        Me.mnuEdittsmiPaste.Text = "&Pegar"
        Me.mnuEdittsmiPaste.Visible = False
        '
        'mnuEdittss2
        '
        Me.mnuEdittss2.Name = "mnuEdittss2"
        Me.mnuEdittss2.Size = New System.Drawing.Size(190, 6)
        '
        'mnuEdittsmiSelectAll
        '
        Me.mnuEdittsmiSelectAll.Name = "mnuEdittsmiSelectAll"
        Me.mnuEdittsmiSelectAll.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.E), System.Windows.Forms.Keys)
        Me.mnuEdittsmiSelectAll.Size = New System.Drawing.Size(193, 22)
        Me.mnuEdittsmiSelectAll.Text = "Seleccion&ar Todo"
        Me.mnuEdittsmiSelectAll.Visible = False
        '
        'mnuView
        '
        Me.mnuView.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuViewtsmiGeneralBar, Me.mnuViewtsmiStatusBar})
        Me.mnuView.Name = "mnuView"
        Me.mnuView.Size = New System.Drawing.Size(35, 20)
        Me.mnuView.Text = "Ve&r"
        Me.mnuView.Visible = False
        '
        'mnuViewtsmiGeneralBar
        '
        Me.mnuViewtsmiGeneralBar.CheckOnClick = True
        Me.mnuViewtsmiGeneralBar.Name = "mnuViewtsmiGeneralBar"
        Me.mnuViewtsmiGeneralBar.Size = New System.Drawing.Size(222, 22)
        Me.mnuViewtsmiGeneralBar.Text = "&Barra de Herramientas General"
        Me.mnuViewtsmiGeneralBar.Visible = False
        '
        'mnuViewtsmiStatusBar
        '
        Me.mnuViewtsmiStatusBar.Checked = True
        Me.mnuViewtsmiStatusBar.CheckOnClick = True
        Me.mnuViewtsmiStatusBar.CheckState = System.Windows.Forms.CheckState.Checked
        Me.mnuViewtsmiStatusBar.Name = "mnuViewtsmiStatusBar"
        Me.mnuViewtsmiStatusBar.Size = New System.Drawing.Size(222, 22)
        Me.mnuViewtsmiStatusBar.Text = "&Barra de Estado"
        Me.mnuViewtsmiStatusBar.Visible = False
        '
        'mnuTools
        '
        Me.mnuTools.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuToolstsmiModifyMyUser, Me.mnuToolstsmiOptions})
        Me.mnuTools.Name = "mnuTools"
        Me.mnuTools.Size = New System.Drawing.Size(83, 20)
        Me.mnuTools.Text = "&Herramientas"
        '
        'mnuToolstsmiModifyMyUser
        '
        Me.mnuToolstsmiModifyMyUser.Name = "mnuToolstsmiModifyMyUser"
        Me.mnuToolstsmiModifyMyUser.Size = New System.Drawing.Size(290, 22)
        Me.mnuToolstsmiModifyMyUser.Text = "Cambiar mi Contraseña / Modificar mi Usuario"
        '
        'mnuToolstsmiOptions
        '
        Me.mnuToolstsmiOptions.Name = "mnuToolstsmiOptions"
        Me.mnuToolstsmiOptions.Size = New System.Drawing.Size(290, 22)
        Me.mnuToolstsmiOptions.Text = "&Opciones"
        '
        'mnuWindows
        '
        Me.mnuWindows.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuWindowtsmiNewWindow, Me.mnuWindowtsmiCascade, Me.mnuWindowtsmiTileVertical, Me.mnuWindowtsmiTileHorizontal, Me.mnuWindowtsmiCloseAll, Me.mnuWindowtsmiArrangeIcons})
        Me.mnuWindows.Name = "mnuWindows"
        Me.mnuWindows.Size = New System.Drawing.Size(59, 20)
        Me.mnuWindows.Text = "&Ventana"
        Me.mnuWindows.Visible = False
        '
        'mnuWindowtsmiNewWindow
        '
        Me.mnuWindowtsmiNewWindow.Name = "mnuWindowtsmiNewWindow"
        Me.mnuWindowtsmiNewWindow.Size = New System.Drawing.Size(195, 22)
        Me.mnuWindowtsmiNewWindow.Text = "&Nueva Ventana"
        Me.mnuWindowtsmiNewWindow.Visible = False
        '
        'mnuWindowtsmiCascade
        '
        Me.mnuWindowtsmiCascade.Name = "mnuWindowtsmiCascade"
        Me.mnuWindowtsmiCascade.Size = New System.Drawing.Size(195, 22)
        Me.mnuWindowtsmiCascade.Text = "Ordenar En &Cascada"
        Me.mnuWindowtsmiCascade.Visible = False
        '
        'mnuWindowtsmiTileVertical
        '
        Me.mnuWindowtsmiTileVertical.Name = "mnuWindowtsmiTileVertical"
        Me.mnuWindowtsmiTileVertical.Size = New System.Drawing.Size(195, 22)
        Me.mnuWindowtsmiTileVertical.Text = "Ordenar &Verticalmente"
        Me.mnuWindowtsmiTileVertical.Visible = False
        '
        'mnuWindowtsmiTileHorizontal
        '
        Me.mnuWindowtsmiTileHorizontal.Name = "mnuWindowtsmiTileHorizontal"
        Me.mnuWindowtsmiTileHorizontal.Size = New System.Drawing.Size(195, 22)
        Me.mnuWindowtsmiTileHorizontal.Text = "Ordenar &Horizontalmente"
        Me.mnuWindowtsmiTileHorizontal.Visible = False
        '
        'mnuWindowtsmiCloseAll
        '
        Me.mnuWindowtsmiCloseAll.Name = "mnuWindowtsmiCloseAll"
        Me.mnuWindowtsmiCloseAll.Size = New System.Drawing.Size(195, 22)
        Me.mnuWindowtsmiCloseAll.Text = "&Cerrar Todo"
        Me.mnuWindowtsmiCloseAll.Visible = False
        '
        'mnuWindowtsmiArrangeIcons
        '
        Me.mnuWindowtsmiArrangeIcons.Name = "mnuWindowtsmiArrangeIcons"
        Me.mnuWindowtsmiArrangeIcons.Size = New System.Drawing.Size(195, 22)
        Me.mnuWindowtsmiArrangeIcons.Text = "&Ordenar Iconos"
        Me.mnuWindowtsmiArrangeIcons.Visible = False
        '
        'mnuHelp
        '
        Me.mnuHelp.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuHelptsmiContents, Me.mnuHelptsmiIndex, Me.mnuHelptsmiSearch, Me.mnuHelptss1, Me.mnuHelptsmiAbout})
        Me.mnuHelp.Name = "mnuHelp"
        Me.mnuHelp.Size = New System.Drawing.Size(50, 20)
        Me.mnuHelp.Text = "A&yuda"
        '
        'mnuHelptsmiContents
        '
        Me.mnuHelptsmiContents.Name = "mnuHelptsmiContents"
        Me.mnuHelptsmiContents.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.F1), System.Windows.Forms.Keys)
        Me.mnuHelptsmiContents.Size = New System.Drawing.Size(167, 22)
        Me.mnuHelptsmiContents.Text = "&Contenido"
        Me.mnuHelptsmiContents.Visible = False
        '
        'mnuHelptsmiIndex
        '
        Me.mnuHelptsmiIndex.Image = CType(resources.GetObject("mnuHelptsmiIndex.Image"), System.Drawing.Image)
        Me.mnuHelptsmiIndex.ImageTransparentColor = System.Drawing.Color.Black
        Me.mnuHelptsmiIndex.Name = "mnuHelptsmiIndex"
        Me.mnuHelptsmiIndex.Size = New System.Drawing.Size(167, 22)
        Me.mnuHelptsmiIndex.Text = "&Indice"
        Me.mnuHelptsmiIndex.Visible = False
        '
        'mnuHelptsmiSearch
        '
        Me.mnuHelptsmiSearch.Image = CType(resources.GetObject("mnuHelptsmiSearch.Image"), System.Drawing.Image)
        Me.mnuHelptsmiSearch.ImageTransparentColor = System.Drawing.Color.Black
        Me.mnuHelptsmiSearch.Name = "mnuHelptsmiSearch"
        Me.mnuHelptsmiSearch.Size = New System.Drawing.Size(167, 22)
        Me.mnuHelptsmiSearch.Text = "&Buscar"
        Me.mnuHelptsmiSearch.Visible = False
        '
        'mnuHelptss1
        '
        Me.mnuHelptss1.Name = "mnuHelptss1"
        Me.mnuHelptss1.Size = New System.Drawing.Size(164, 6)
        Me.mnuHelptss1.Visible = False
        '
        'mnuHelptsmiAbout
        '
        Me.mnuHelptsmiAbout.Name = "mnuHelptsmiAbout"
        Me.mnuHelptsmiAbout.Size = New System.Drawing.Size(167, 22)
        Me.mnuHelptsmiAbout.Text = "&Sobre Oversight"
        '
        'tsGeneral
        '
        Me.tsGeneral.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.tsGeneraltsbNuevo, Me.tsGeneraltsbAbrir, Me.tsGeneraltsbGuardar, Me.tsGeneraltss1, Me.tsGeneraltsbImprimir, Me.tsGeneraltsbVistaPrevia, Me.tsGeneraltss2, Me.tsGeneraltsbAyuda})
        Me.tsGeneral.Location = New System.Drawing.Point(0, 49)
        Me.tsGeneral.Name = "tsGeneral"
        Me.tsGeneral.Size = New System.Drawing.Size(716, 25)
        Me.tsGeneral.TabIndex = 6
        Me.tsGeneral.Text = "General Bar"
        Me.tsGeneral.Visible = False
        '
        'tsGeneraltsbNuevo
        '
        Me.tsGeneraltsbNuevo.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.tsGeneraltsbNuevo.Image = CType(resources.GetObject("tsGeneraltsbNuevo.Image"), System.Drawing.Image)
        Me.tsGeneraltsbNuevo.ImageTransparentColor = System.Drawing.Color.Black
        Me.tsGeneraltsbNuevo.Name = "tsGeneraltsbNuevo"
        Me.tsGeneraltsbNuevo.Size = New System.Drawing.Size(23, 22)
        Me.tsGeneraltsbNuevo.Text = "Nuevo"
        '
        'tsGeneraltsbAbrir
        '
        Me.tsGeneraltsbAbrir.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.tsGeneraltsbAbrir.Image = CType(resources.GetObject("tsGeneraltsbAbrir.Image"), System.Drawing.Image)
        Me.tsGeneraltsbAbrir.ImageTransparentColor = System.Drawing.Color.Black
        Me.tsGeneraltsbAbrir.Name = "tsGeneraltsbAbrir"
        Me.tsGeneraltsbAbrir.Size = New System.Drawing.Size(23, 22)
        Me.tsGeneraltsbAbrir.Text = "Abrir"
        '
        'tsGeneraltsbGuardar
        '
        Me.tsGeneraltsbGuardar.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.tsGeneraltsbGuardar.Image = CType(resources.GetObject("tsGeneraltsbGuardar.Image"), System.Drawing.Image)
        Me.tsGeneraltsbGuardar.ImageTransparentColor = System.Drawing.Color.Black
        Me.tsGeneraltsbGuardar.Name = "tsGeneraltsbGuardar"
        Me.tsGeneraltsbGuardar.Size = New System.Drawing.Size(23, 22)
        Me.tsGeneraltsbGuardar.Text = "Guardar"
        '
        'tsGeneraltss1
        '
        Me.tsGeneraltss1.Name = "tsGeneraltss1"
        Me.tsGeneraltss1.Size = New System.Drawing.Size(6, 25)
        '
        'tsGeneraltsbImprimir
        '
        Me.tsGeneraltsbImprimir.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.tsGeneraltsbImprimir.Image = CType(resources.GetObject("tsGeneraltsbImprimir.Image"), System.Drawing.Image)
        Me.tsGeneraltsbImprimir.ImageTransparentColor = System.Drawing.Color.Black
        Me.tsGeneraltsbImprimir.Name = "tsGeneraltsbImprimir"
        Me.tsGeneraltsbImprimir.Size = New System.Drawing.Size(23, 22)
        Me.tsGeneraltsbImprimir.Text = "Imprimir"
        '
        'tsGeneraltsbVistaPrevia
        '
        Me.tsGeneraltsbVistaPrevia.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.tsGeneraltsbVistaPrevia.Image = CType(resources.GetObject("tsGeneraltsbVistaPrevia.Image"), System.Drawing.Image)
        Me.tsGeneraltsbVistaPrevia.ImageTransparentColor = System.Drawing.Color.Black
        Me.tsGeneraltsbVistaPrevia.Name = "tsGeneraltsbVistaPrevia"
        Me.tsGeneraltsbVistaPrevia.Size = New System.Drawing.Size(23, 22)
        Me.tsGeneraltsbVistaPrevia.Text = "Vista Previa"
        '
        'tsGeneraltss2
        '
        Me.tsGeneraltss2.Name = "tsGeneraltss2"
        Me.tsGeneraltss2.Size = New System.Drawing.Size(6, 25)
        '
        'tsGeneraltsbAyuda
        '
        Me.tsGeneraltsbAyuda.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.tsGeneraltsbAyuda.Image = CType(resources.GetObject("tsGeneraltsbAyuda.Image"), System.Drawing.Image)
        Me.tsGeneraltsbAyuda.ImageTransparentColor = System.Drawing.Color.Black
        Me.tsGeneraltsbAyuda.Name = "tsGeneraltsbAyuda"
        Me.tsGeneraltsbAyuda.Size = New System.Drawing.Size(23, 22)
        Me.tsGeneraltsbAyuda.Text = "Ayuda"
        '
        'ssMain
        '
        Me.ssMain.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.tsStatusLabel})
        Me.ssMain.Location = New System.Drawing.Point(0, 474)
        Me.ssMain.Name = "ssMain"
        Me.ssMain.Size = New System.Drawing.Size(840, 22)
        Me.ssMain.TabIndex = 7
        Me.ssMain.Text = "StatusStrip"
        '
        'tsStatusLabel
        '
        Me.tsStatusLabel.Name = "tsStatusLabel"
        Me.tsStatusLabel.Size = New System.Drawing.Size(29, 17)
        Me.tsStatusLabel.Text = "Listo"
        '
        'tsProyectos
        '
        Me.tsProyectos.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.tsbProyectos, Me.tsbModelos, Me.tsbPresupuestosBase, Me.tsbMateriales, Me.tsbCotizacionesMateriales, Me.tsbPedidos, Me.tsbEnvios, Me.tsbActivos, Me.tsbInventarios, Me.tsbDirectorio, Me.tsbProveedores, Me.tsbVales, Me.tsbCasetas, Me.tsbNominas, Me.tsbIngresos, Me.tsbPolizas, Me.tsbCuentas, Me.tsbFacturasMRD, Me.tsbReportes, Me.tsbMensajes, Me.tsbUsuarios, Me.tsbUnidades})
        Me.tsProyectos.Location = New System.Drawing.Point(0, 24)
        Me.tsProyectos.Name = "tsProyectos"
        Me.tsProyectos.Size = New System.Drawing.Size(840, 25)
        Me.tsProyectos.TabIndex = 9
        Me.tsProyectos.Text = "Proyectos"
        '
        'tsbProyectos
        '
        Me.tsbProyectos.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.tsbProyectos.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.tsmiVerProyectos, Me.tsmiNuevoProyecto, Me.tsmiNuevoProyectoDesdeModelo, Me.tsmiDuplicarProyecto, Me.tsmiEliminarProyecto})
        Me.tsbProyectos.Image = Global.Oversight.My.Resources.Resources.helmet24x24
        Me.tsbProyectos.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.tsbProyectos.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbProyectos.Name = "tsbProyectos"
        Me.tsbProyectos.Size = New System.Drawing.Size(37, 31)
        Me.tsbProyectos.Text = "Proyectos"
        Me.tsbProyectos.Visible = False
        '
        'tsmiVerProyectos
        '
        Me.tsmiVerProyectos.Name = "tsmiVerProyectos"
        Me.tsmiVerProyectos.ShortcutKeys = CType((System.Windows.Forms.Keys.Alt Or System.Windows.Forms.Keys.F1), System.Windows.Forms.Keys)
        Me.tsmiVerProyectos.Size = New System.Drawing.Size(220, 22)
        Me.tsmiVerProyectos.Text = "Ver Proyectos / Obras"
        Me.tsmiVerProyectos.Visible = False
        '
        'tsmiNuevoProyecto
        '
        Me.tsmiNuevoProyecto.Name = "tsmiNuevoProyecto"
        Me.tsmiNuevoProyecto.ShortcutKeys = System.Windows.Forms.Keys.F1
        Me.tsmiNuevoProyecto.Size = New System.Drawing.Size(220, 22)
        Me.tsmiNuevoProyecto.Text = "Nuevo Proyecto"
        Me.tsmiNuevoProyecto.Visible = False
        '
        'tsmiNuevoProyectoDesdeModelo
        '
        Me.tsmiNuevoProyectoDesdeModelo.Name = "tsmiNuevoProyectoDesdeModelo"
        Me.tsmiNuevoProyectoDesdeModelo.Size = New System.Drawing.Size(220, 22)
        Me.tsmiNuevoProyectoDesdeModelo.Text = "Nuevo Proyecto desde Modelo"
        Me.tsmiNuevoProyectoDesdeModelo.Visible = False
        '
        'tsmiDuplicarProyecto
        '
        Me.tsmiDuplicarProyecto.Name = "tsmiDuplicarProyecto"
        Me.tsmiDuplicarProyecto.Size = New System.Drawing.Size(220, 22)
        Me.tsmiDuplicarProyecto.Text = "Duplicar Proyecto"
        Me.tsmiDuplicarProyecto.Visible = False
        '
        'tsmiEliminarProyecto
        '
        Me.tsmiEliminarProyecto.Name = "tsmiEliminarProyecto"
        Me.tsmiEliminarProyecto.Size = New System.Drawing.Size(220, 22)
        Me.tsmiEliminarProyecto.Text = "Eliminar Proyecto"
        Me.tsmiEliminarProyecto.Visible = False
        '
        'tsbModelos
        '
        Me.tsbModelos.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.tsbModelos.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.tsmiVerModelos, Me.tsmiNuevoModelo, Me.tsmiEliminarModelo})
        Me.tsbModelos.Image = Global.Oversight.My.Resources.Resources.home24x24
        Me.tsbModelos.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.tsbModelos.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbModelos.Name = "tsbModelos"
        Me.tsbModelos.Size = New System.Drawing.Size(37, 31)
        Me.tsbModelos.Text = "Modelos"
        Me.tsbModelos.Visible = False
        '
        'tsmiVerModelos
        '
        Me.tsmiVerModelos.Name = "tsmiVerModelos"
        Me.tsmiVerModelos.ShortcutKeys = System.Windows.Forms.Keys.F2
        Me.tsmiVerModelos.Size = New System.Drawing.Size(151, 22)
        Me.tsmiVerModelos.Text = "Ver Modelos"
        Me.tsmiVerModelos.Visible = False
        '
        'tsmiNuevoModelo
        '
        Me.tsmiNuevoModelo.Name = "tsmiNuevoModelo"
        Me.tsmiNuevoModelo.Size = New System.Drawing.Size(151, 22)
        Me.tsmiNuevoModelo.Text = "Nuevo Modelo"
        Me.tsmiNuevoModelo.Visible = False
        '
        'tsmiEliminarModelo
        '
        Me.tsmiEliminarModelo.Name = "tsmiEliminarModelo"
        Me.tsmiEliminarModelo.Size = New System.Drawing.Size(151, 22)
        Me.tsmiEliminarModelo.Text = "Eliminar Modelo"
        Me.tsmiEliminarModelo.Visible = False
        '
        'tsbPresupuestosBase
        '
        Me.tsbPresupuestosBase.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.tsbPresupuestosBase.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.tsmiVerPresupuestosBase, Me.tsmiNuevoPresupuestoBase, Me.tsmiEliminarPresupuestoBase})
        Me.tsbPresupuestosBase.Image = Global.Oversight.My.Resources.Resources.attachments24x27
        Me.tsbPresupuestosBase.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.tsbPresupuestosBase.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbPresupuestosBase.Name = "tsbPresupuestosBase"
        Me.tsbPresupuestosBase.Size = New System.Drawing.Size(37, 31)
        Me.tsbPresupuestosBase.Text = "Presupuestos Base"
        Me.tsbPresupuestosBase.Visible = False
        '
        'tsmiVerPresupuestosBase
        '
        Me.tsmiVerPresupuestosBase.Name = "tsmiVerPresupuestosBase"
        Me.tsmiVerPresupuestosBase.ShortcutKeys = System.Windows.Forms.Keys.F3
        Me.tsmiVerPresupuestosBase.Size = New System.Drawing.Size(204, 22)
        Me.tsmiVerPresupuestosBase.Text = "Ver Presupuestos Base"
        Me.tsmiVerPresupuestosBase.Visible = False
        '
        'tsmiNuevoPresupuestoBase
        '
        Me.tsmiNuevoPresupuestoBase.Name = "tsmiNuevoPresupuestoBase"
        Me.tsmiNuevoPresupuestoBase.Size = New System.Drawing.Size(204, 22)
        Me.tsmiNuevoPresupuestoBase.Text = "Nuevo Presupuesto Base"
        Me.tsmiNuevoPresupuestoBase.Visible = False
        '
        'tsmiEliminarPresupuestoBase
        '
        Me.tsmiEliminarPresupuestoBase.Name = "tsmiEliminarPresupuestoBase"
        Me.tsmiEliminarPresupuestoBase.Size = New System.Drawing.Size(204, 22)
        Me.tsmiEliminarPresupuestoBase.Text = "Eliminar Presupuestos Base"
        Me.tsmiEliminarPresupuestoBase.Visible = False
        '
        'tsbMateriales
        '
        Me.tsbMateriales.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.tsbMateriales.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.tsmiVerMaterialesPrecios, Me.tsmiNuevoMaterial, Me.tsmiEliminarMaterial})
        Me.tsbMateriales.Image = Global.Oversight.My.Resources.Resources.bricks24x24
        Me.tsbMateriales.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.tsbMateriales.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbMateriales.Name = "tsbMateriales"
        Me.tsbMateriales.Size = New System.Drawing.Size(37, 28)
        Me.tsbMateriales.Text = "Materiales"
        Me.tsbMateriales.Visible = False
        '
        'tsmiVerMaterialesPrecios
        '
        Me.tsmiVerMaterialesPrecios.Name = "tsmiVerMaterialesPrecios"
        Me.tsmiVerMaterialesPrecios.ShortcutKeys = System.Windows.Forms.Keys.F4
        Me.tsmiVerMaterialesPrecios.Size = New System.Drawing.Size(256, 22)
        Me.tsmiVerMaterialesPrecios.Text = "Ver Materiales / Precios Generales"
        Me.tsmiVerMaterialesPrecios.Visible = False
        '
        'tsmiNuevoMaterial
        '
        Me.tsmiNuevoMaterial.Name = "tsmiNuevoMaterial"
        Me.tsmiNuevoMaterial.Size = New System.Drawing.Size(256, 22)
        Me.tsmiNuevoMaterial.Text = "Nuevo Material"
        Me.tsmiNuevoMaterial.Visible = False
        '
        'tsmiEliminarMaterial
        '
        Me.tsmiEliminarMaterial.Name = "tsmiEliminarMaterial"
        Me.tsmiEliminarMaterial.Size = New System.Drawing.Size(256, 22)
        Me.tsmiEliminarMaterial.Text = "Eliminar Material"
        Me.tsmiEliminarMaterial.Visible = False
        '
        'tsbCotizacionesMateriales
        '
        Me.tsbCotizacionesMateriales.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.tsbCotizacionesMateriales.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.tsmiVerCotizacionesDeMateriales, Me.tsmiNuevaCotizacionDeMateriales})
        Me.tsbCotizacionesMateriales.Image = Global.Oversight.My.Resources.Resources.db24x24
        Me.tsbCotizacionesMateriales.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.tsbCotizacionesMateriales.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbCotizacionesMateriales.Name = "tsbCotizacionesMateriales"
        Me.tsbCotizacionesMateriales.Size = New System.Drawing.Size(37, 28)
        Me.tsbCotizacionesMateriales.Text = "Cotizaciones de Materiales"
        Me.tsbCotizacionesMateriales.Visible = False
        '
        'tsmiVerCotizacionesDeMateriales
        '
        Me.tsmiVerCotizacionesDeMateriales.Name = "tsmiVerCotizacionesDeMateriales"
        Me.tsmiVerCotizacionesDeMateriales.Size = New System.Drawing.Size(224, 22)
        Me.tsmiVerCotizacionesDeMateriales.Text = "Ver Cotizaciones de Materiales"
        Me.tsmiVerCotizacionesDeMateriales.Visible = False
        '
        'tsmiNuevaCotizacionDeMateriales
        '
        Me.tsmiNuevaCotizacionDeMateriales.Name = "tsmiNuevaCotizacionDeMateriales"
        Me.tsmiNuevaCotizacionDeMateriales.Size = New System.Drawing.Size(224, 22)
        Me.tsmiNuevaCotizacionDeMateriales.Text = "Nueva Cotizacion de Materiales"
        Me.tsmiNuevaCotizacionDeMateriales.Visible = False
        '
        'tsbPedidos
        '
        Me.tsbPedidos.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.tsbPedidos.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.tsmiVerPedidosDeMateriales, Me.tsmiNuevoPedidoDeMaterial, Me.tsmiEliminarPedidosDeMaterial})
        Me.tsbPedidos.Enabled = False
        Me.tsbPedidos.Image = Global.Oversight.My.Resources.Resources.check24x24
        Me.tsbPedidos.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.tsbPedidos.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbPedidos.Name = "tsbPedidos"
        Me.tsbPedidos.Size = New System.Drawing.Size(37, 28)
        Me.tsbPedidos.Text = "Órdenes de Materiales"
        Me.tsbPedidos.Visible = False
        '
        'tsmiVerPedidosDeMateriales
        '
        Me.tsmiVerPedidosDeMateriales.Name = "tsmiVerPedidosDeMateriales"
        Me.tsmiVerPedidosDeMateriales.Size = New System.Drawing.Size(206, 22)
        Me.tsmiVerPedidosDeMateriales.Text = "Ver Pedidos de Materiales"
        Me.tsmiVerPedidosDeMateriales.Visible = False
        '
        'tsmiNuevoPedidoDeMaterial
        '
        Me.tsmiNuevoPedidoDeMaterial.Name = "tsmiNuevoPedidoDeMaterial"
        Me.tsmiNuevoPedidoDeMaterial.Size = New System.Drawing.Size(206, 22)
        Me.tsmiNuevoPedidoDeMaterial.Text = "Nuevo Pedido de Material"
        Me.tsmiNuevoPedidoDeMaterial.Visible = False
        '
        'tsmiEliminarPedidosDeMaterial
        '
        Me.tsmiEliminarPedidosDeMaterial.Name = "tsmiEliminarPedidosDeMaterial"
        Me.tsmiEliminarPedidosDeMaterial.Size = New System.Drawing.Size(206, 22)
        Me.tsmiEliminarPedidosDeMaterial.Text = "Eliminar Pedidos de Material"
        Me.tsmiEliminarPedidosDeMaterial.Visible = False
        '
        'tsbEnvios
        '
        Me.tsbEnvios.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.tsbEnvios.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.tsmiVerEnvíosDeMaterial, Me.tsmiNuevoEnvíoDeMaterial, Me.tsmiEliminarEnvíoDeMaterial})
        Me.tsbEnvios.Enabled = False
        Me.tsbEnvios.Image = Global.Oversight.My.Resources.Resources.package24x24
        Me.tsbEnvios.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.tsbEnvios.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbEnvios.Name = "tsbEnvios"
        Me.tsbEnvios.Size = New System.Drawing.Size(37, 28)
        Me.tsbEnvios.Text = "Envíos de Materiales"
        Me.tsbEnvios.Visible = False
        '
        'tsmiVerEnvíosDeMaterial
        '
        Me.tsmiVerEnvíosDeMaterial.Name = "tsmiVerEnvíosDeMaterial"
        Me.tsmiVerEnvíosDeMaterial.Size = New System.Drawing.Size(195, 22)
        Me.tsmiVerEnvíosDeMaterial.Text = "Ver Envíos de Material"
        Me.tsmiVerEnvíosDeMaterial.Visible = False
        '
        'tsmiNuevoEnvíoDeMaterial
        '
        Me.tsmiNuevoEnvíoDeMaterial.Name = "tsmiNuevoEnvíoDeMaterial"
        Me.tsmiNuevoEnvíoDeMaterial.Size = New System.Drawing.Size(195, 22)
        Me.tsmiNuevoEnvíoDeMaterial.Text = "Nuevo Envío de Material"
        Me.tsmiNuevoEnvíoDeMaterial.Visible = False
        '
        'tsmiEliminarEnvíoDeMaterial
        '
        Me.tsmiEliminarEnvíoDeMaterial.Name = "tsmiEliminarEnvíoDeMaterial"
        Me.tsmiEliminarEnvíoDeMaterial.Size = New System.Drawing.Size(195, 22)
        Me.tsmiEliminarEnvíoDeMaterial.Text = "Eliminar Envío de Material"
        Me.tsmiEliminarEnvíoDeMaterial.Visible = False
        '
        'tsbActivos
        '
        Me.tsbActivos.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.tsbActivos.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.tsmiVerActivos, Me.tsmiNuevoActivo, Me.tsmiEliminarActivo})
        Me.tsbActivos.Image = Global.Oversight.My.Resources.Resources.hammer24x24
        Me.tsbActivos.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.tsbActivos.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbActivos.Name = "tsbActivos"
        Me.tsbActivos.Size = New System.Drawing.Size(37, 28)
        Me.tsbActivos.Text = "Activos y Herramientas"
        Me.tsbActivos.Visible = False
        '
        'tsmiVerActivos
        '
        Me.tsmiVerActivos.Name = "tsmiVerActivos"
        Me.tsmiVerActivos.Size = New System.Drawing.Size(143, 22)
        Me.tsmiVerActivos.Text = "Ver Activos"
        Me.tsmiVerActivos.Visible = False
        '
        'tsmiNuevoActivo
        '
        Me.tsmiNuevoActivo.Name = "tsmiNuevoActivo"
        Me.tsmiNuevoActivo.Size = New System.Drawing.Size(143, 22)
        Me.tsmiNuevoActivo.Text = "Nuevo Activo"
        Me.tsmiNuevoActivo.Visible = False
        '
        'tsmiEliminarActivo
        '
        Me.tsmiEliminarActivo.Name = "tsmiEliminarActivo"
        Me.tsmiEliminarActivo.Size = New System.Drawing.Size(143, 22)
        Me.tsmiEliminarActivo.Text = "Eliminar Activo"
        Me.tsmiEliminarActivo.Visible = False
        '
        'tsbInventarios
        '
        Me.tsbInventarios.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.tsbInventarios.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.tsmiVerInventarioDeMateriales, Me.tsmiVerInventarioDeActivos})
        Me.tsbInventarios.Image = Global.Oversight.My.Resources.Resources.inventory_2_24x24
        Me.tsbInventarios.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.tsbInventarios.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbInventarios.Name = "tsbInventarios"
        Me.tsbInventarios.Size = New System.Drawing.Size(37, 28)
        Me.tsbInventarios.Text = "Inventarios"
        Me.tsbInventarios.Visible = False
        '
        'tsmiVerInventarioDeMateriales
        '
        Me.tsmiVerInventarioDeMateriales.Name = "tsmiVerInventarioDeMateriales"
        Me.tsmiVerInventarioDeMateriales.Size = New System.Drawing.Size(210, 22)
        Me.tsmiVerInventarioDeMateriales.Text = "Ver Inventario de Materiales"
        Me.tsmiVerInventarioDeMateriales.Visible = False
        '
        'tsmiVerInventarioDeActivos
        '
        Me.tsmiVerInventarioDeActivos.Name = "tsmiVerInventarioDeActivos"
        Me.tsmiVerInventarioDeActivos.Size = New System.Drawing.Size(210, 22)
        Me.tsmiVerInventarioDeActivos.Text = "Ver Inventario de Activos"
        Me.tsmiVerInventarioDeActivos.Visible = False
        '
        'tsbDirectorio
        '
        Me.tsbDirectorio.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.tsbDirectorio.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.tsmiVerDirectorio, Me.tsmiNuevaPersonaProveedor, Me.tsmiEliminarPersonaProveedor})
        Me.tsbDirectorio.Image = Global.Oversight.My.Resources.Resources.clients24x24
        Me.tsbDirectorio.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.tsbDirectorio.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbDirectorio.Name = "tsbDirectorio"
        Me.tsbDirectorio.Size = New System.Drawing.Size(37, 28)
        Me.tsbDirectorio.Text = "Directorio"
        Me.tsbDirectorio.Visible = False
        '
        'tsmiVerDirectorio
        '
        Me.tsmiVerDirectorio.Name = "tsmiVerDirectorio"
        Me.tsmiVerDirectorio.ShortcutKeys = System.Windows.Forms.Keys.F6
        Me.tsmiVerDirectorio.Size = New System.Drawing.Size(206, 22)
        Me.tsmiVerDirectorio.Text = "Ver Directorio"
        Me.tsmiVerDirectorio.Visible = False
        '
        'tsmiNuevaPersonaProveedor
        '
        Me.tsmiNuevaPersonaProveedor.Name = "tsmiNuevaPersonaProveedor"
        Me.tsmiNuevaPersonaProveedor.Size = New System.Drawing.Size(206, 22)
        Me.tsmiNuevaPersonaProveedor.Text = "Nueva Persona/Proveedor"
        Me.tsmiNuevaPersonaProveedor.Visible = False
        '
        'tsmiEliminarPersonaProveedor
        '
        Me.tsmiEliminarPersonaProveedor.Name = "tsmiEliminarPersonaProveedor"
        Me.tsmiEliminarPersonaProveedor.Size = New System.Drawing.Size(206, 22)
        Me.tsmiEliminarPersonaProveedor.Text = "Eliminar Persona/Proveedor"
        Me.tsmiEliminarPersonaProveedor.Visible = False
        '
        'tsbProveedores
        '
        Me.tsbProveedores.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.tsbProveedores.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.tsmiVerFacturasDeProveedores, Me.tsmiNuevaFacturaDeProveedor, Me.tsmiEliminarFacturaDeProveedor, Me.ToolStripSeparator1, Me.tsmiRevisarFacturasDeProveedores})
        Me.tsbProveedores.Image = Global.Oversight.My.Resources.Resources.empresa24x24
        Me.tsbProveedores.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.tsbProveedores.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbProveedores.Name = "tsbProveedores"
        Me.tsbProveedores.Size = New System.Drawing.Size(37, 28)
        Me.tsbProveedores.Text = "Proveedores"
        Me.tsbProveedores.Visible = False
        '
        'tsmiVerFacturasDeProveedores
        '
        Me.tsmiVerFacturasDeProveedores.Name = "tsmiVerFacturasDeProveedores"
        Me.tsmiVerFacturasDeProveedores.ShortcutKeys = System.Windows.Forms.Keys.F7
        Me.tsmiVerFacturasDeProveedores.Size = New System.Drawing.Size(234, 22)
        Me.tsmiVerFacturasDeProveedores.Text = "Ver Facturas de Proveedores"
        Me.tsmiVerFacturasDeProveedores.Visible = False
        '
        'tsmiNuevaFacturaDeProveedor
        '
        Me.tsmiNuevaFacturaDeProveedor.Name = "tsmiNuevaFacturaDeProveedor"
        Me.tsmiNuevaFacturaDeProveedor.Size = New System.Drawing.Size(234, 22)
        Me.tsmiNuevaFacturaDeProveedor.Text = "Nueva Factura de Proveedor"
        Me.tsmiNuevaFacturaDeProveedor.Visible = False
        '
        'tsmiEliminarFacturaDeProveedor
        '
        Me.tsmiEliminarFacturaDeProveedor.Name = "tsmiEliminarFacturaDeProveedor"
        Me.tsmiEliminarFacturaDeProveedor.Size = New System.Drawing.Size(234, 22)
        Me.tsmiEliminarFacturaDeProveedor.Text = "Eliminar Factura de Proveedor"
        Me.tsmiEliminarFacturaDeProveedor.Visible = False
        '
        'ToolStripSeparator1
        '
        Me.ToolStripSeparator1.Name = "ToolStripSeparator1"
        Me.ToolStripSeparator1.Size = New System.Drawing.Size(231, 6)
        '
        'tsmiRevisarFacturasDeProveedores
        '
        Me.tsmiRevisarFacturasDeProveedores.Name = "tsmiRevisarFacturasDeProveedores"
        Me.tsmiRevisarFacturasDeProveedores.Size = New System.Drawing.Size(234, 22)
        Me.tsmiRevisarFacturasDeProveedores.Text = "Revisar Facturas de Proveedores"
        Me.tsmiRevisarFacturasDeProveedores.Visible = False
        '
        'tsbVales
        '
        Me.tsbVales.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.tsbVales.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.tsmiVerValesDeGasolina, Me.tsmiNuevoValeDeGasolina, Me.tsmiEliminarValeDeGasolina, Me.tsVales, Me.tsmiVerFacturaCombustibleVales, Me.tsmiNuevaFacturaCombustibleVales, Me.tsmiEliminarFacturaCombustibleVales})
        Me.tsbVales.Image = Global.Oversight.My.Resources.Resources.gas24x22
        Me.tsbVales.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.tsbVales.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbVales.Name = "tsbVales"
        Me.tsbVales.Size = New System.Drawing.Size(37, 28)
        Me.tsbVales.Text = "Vales de Gasolina"
        Me.tsbVales.Visible = False
        '
        'tsmiVerValesDeGasolina
        '
        Me.tsmiVerValesDeGasolina.Name = "tsmiVerValesDeGasolina"
        Me.tsmiVerValesDeGasolina.ShortcutKeys = System.Windows.Forms.Keys.F8
        Me.tsmiVerValesDeGasolina.Size = New System.Drawing.Size(262, 22)
        Me.tsmiVerValesDeGasolina.Text = "Ver Vales de Gasolina"
        Me.tsmiVerValesDeGasolina.Visible = False
        '
        'tsmiNuevoValeDeGasolina
        '
        Me.tsmiNuevoValeDeGasolina.Name = "tsmiNuevoValeDeGasolina"
        Me.tsmiNuevoValeDeGasolina.Size = New System.Drawing.Size(262, 22)
        Me.tsmiNuevoValeDeGasolina.Text = "Nuevo Vale de Gasolina"
        Me.tsmiNuevoValeDeGasolina.Visible = False
        '
        'tsmiEliminarValeDeGasolina
        '
        Me.tsmiEliminarValeDeGasolina.Name = "tsmiEliminarValeDeGasolina"
        Me.tsmiEliminarValeDeGasolina.Size = New System.Drawing.Size(262, 22)
        Me.tsmiEliminarValeDeGasolina.Text = "Eliminar Vale de Gasolina"
        Me.tsmiEliminarValeDeGasolina.Visible = False
        '
        'tsVales
        '
        Me.tsVales.Name = "tsVales"
        Me.tsVales.Size = New System.Drawing.Size(259, 6)
        '
        'tsmiVerFacturaCombustibleVales
        '
        Me.tsmiVerFacturaCombustibleVales.Name = "tsmiVerFacturaCombustibleVales"
        Me.tsmiVerFacturaCombustibleVales.Size = New System.Drawing.Size(262, 22)
        Me.tsmiVerFacturaCombustibleVales.Text = "Ver Facturas de Combustible (Vales)"
        Me.tsmiVerFacturaCombustibleVales.Visible = False
        '
        'tsmiNuevaFacturaCombustibleVales
        '
        Me.tsmiNuevaFacturaCombustibleVales.Name = "tsmiNuevaFacturaCombustibleVales"
        Me.tsmiNuevaFacturaCombustibleVales.Size = New System.Drawing.Size(262, 22)
        Me.tsmiNuevaFacturaCombustibleVales.Text = "Nueva Factura de Combustible (Vales)"
        Me.tsmiNuevaFacturaCombustibleVales.Visible = False
        '
        'tsmiEliminarFacturaCombustibleVales
        '
        Me.tsmiEliminarFacturaCombustibleVales.Name = "tsmiEliminarFacturaCombustibleVales"
        Me.tsmiEliminarFacturaCombustibleVales.Size = New System.Drawing.Size(262, 22)
        Me.tsmiEliminarFacturaCombustibleVales.Text = "Eliminar Factura de Combustible (Vales)"
        Me.tsmiEliminarFacturaCombustibleVales.Visible = False
        '
        'tsbCasetas
        '
        Me.tsbCasetas.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.tsbCasetas.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.tsmiVerGastosPorCasetas, Me.tsmiNuevoGastoPorCaseta, Me.tsmiEliminarGastoPorCaseta})
        Me.tsbCasetas.Image = Global.Oversight.My.Resources.Resources.truckb24x24
        Me.tsbCasetas.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.tsbCasetas.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbCasetas.Name = "tsbCasetas"
        Me.tsbCasetas.Size = New System.Drawing.Size(37, 28)
        Me.tsbCasetas.Text = "Casetas"
        Me.tsbCasetas.Visible = False
        '
        'tsmiVerGastosPorCasetas
        '
        Me.tsmiVerGastosPorCasetas.Name = "tsmiVerGastosPorCasetas"
        Me.tsmiVerGastosPorCasetas.Size = New System.Drawing.Size(197, 22)
        Me.tsmiVerGastosPorCasetas.Text = "Ver Gastos por Casetas"
        Me.tsmiVerGastosPorCasetas.Visible = False
        '
        'tsmiNuevoGastoPorCaseta
        '
        Me.tsmiNuevoGastoPorCaseta.Name = "tsmiNuevoGastoPorCaseta"
        Me.tsmiNuevoGastoPorCaseta.Size = New System.Drawing.Size(197, 22)
        Me.tsmiNuevoGastoPorCaseta.Text = "Nuevo Gasto por Caseta"
        Me.tsmiNuevoGastoPorCaseta.Visible = False
        '
        'tsmiEliminarGastoPorCaseta
        '
        Me.tsmiEliminarGastoPorCaseta.Name = "tsmiEliminarGastoPorCaseta"
        Me.tsmiEliminarGastoPorCaseta.Size = New System.Drawing.Size(197, 22)
        Me.tsmiEliminarGastoPorCaseta.Text = "Eliminar Gasto por Caseta"
        Me.tsmiEliminarGastoPorCaseta.Visible = False
        '
        'tsbNominas
        '
        Me.tsbNominas.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.tsbNominas.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.tsmiVerNominas, Me.tsmiNuevaNomina, Me.tsmiEliminarNomina})
        Me.tsbNominas.Image = Global.Oversight.My.Resources.Resources.payrollSolutions24x24
        Me.tsbNominas.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.tsbNominas.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbNominas.Name = "tsbNominas"
        Me.tsbNominas.Size = New System.Drawing.Size(37, 28)
        Me.tsbNominas.Text = "Nóminas"
        Me.tsbNominas.Visible = False
        '
        'tsmiVerNominas
        '
        Me.tsmiVerNominas.Name = "tsmiVerNominas"
        Me.tsmiVerNominas.Size = New System.Drawing.Size(148, 22)
        Me.tsmiVerNominas.Text = "Ver Nóminas"
        Me.tsmiVerNominas.Visible = False
        '
        'tsmiNuevaNomina
        '
        Me.tsmiNuevaNomina.Name = "tsmiNuevaNomina"
        Me.tsmiNuevaNomina.Size = New System.Drawing.Size(148, 22)
        Me.tsmiNuevaNomina.Text = "Nueva Nómina"
        Me.tsmiNuevaNomina.Visible = False
        '
        'tsmiEliminarNomina
        '
        Me.tsmiEliminarNomina.Name = "tsmiEliminarNomina"
        Me.tsmiEliminarNomina.Size = New System.Drawing.Size(148, 22)
        Me.tsmiEliminarNomina.Text = "Eliminar Nómina"
        Me.tsmiEliminarNomina.Visible = False
        '
        'tsbIngresos
        '
        Me.tsbIngresos.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.tsbIngresos.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.tsmiVerIngresos, Me.tsmiNuevoIngreso, Me.tsmiEliminarIngreso, Me.tsbIngresoSep1, Me.tsmiVerPagos, Me.tsmiNuevoPago, Me.tsmiEliminarPago})
        Me.tsbIngresos.Image = Global.Oversight.My.Resources.Resources.money24x24
        Me.tsbIngresos.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.tsbIngresos.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbIngresos.Name = "tsbIngresos"
        Me.tsbIngresos.Size = New System.Drawing.Size(37, 28)
        Me.tsbIngresos.Text = "Ingresos y Pagos"
        Me.tsbIngresos.Visible = False
        '
        'tsmiVerIngresos
        '
        Me.tsmiVerIngresos.Name = "tsmiVerIngresos"
        Me.tsmiVerIngresos.ShortcutKeys = System.Windows.Forms.Keys.F9
        Me.tsmiVerIngresos.Size = New System.Drawing.Size(154, 22)
        Me.tsmiVerIngresos.Text = "Ver Ingresos"
        Me.tsmiVerIngresos.Visible = False
        '
        'tsmiNuevoIngreso
        '
        Me.tsmiNuevoIngreso.Name = "tsmiNuevoIngreso"
        Me.tsmiNuevoIngreso.Size = New System.Drawing.Size(154, 22)
        Me.tsmiNuevoIngreso.Text = "Nuevo Ingreso"
        Me.tsmiNuevoIngreso.Visible = False
        '
        'tsmiEliminarIngreso
        '
        Me.tsmiEliminarIngreso.Name = "tsmiEliminarIngreso"
        Me.tsmiEliminarIngreso.Size = New System.Drawing.Size(154, 22)
        Me.tsmiEliminarIngreso.Text = "Eliminar Ingreso"
        Me.tsmiEliminarIngreso.Visible = False
        '
        'tsbIngresoSep1
        '
        Me.tsbIngresoSep1.Name = "tsbIngresoSep1"
        Me.tsbIngresoSep1.Size = New System.Drawing.Size(151, 6)
        '
        'tsmiVerPagos
        '
        Me.tsmiVerPagos.Name = "tsmiVerPagos"
        Me.tsmiVerPagos.ShortcutKeys = System.Windows.Forms.Keys.F10
        Me.tsmiVerPagos.Size = New System.Drawing.Size(154, 22)
        Me.tsmiVerPagos.Text = "Ver Pagos"
        Me.tsmiVerPagos.Visible = False
        '
        'tsmiNuevoPago
        '
        Me.tsmiNuevoPago.Name = "tsmiNuevoPago"
        Me.tsmiNuevoPago.Size = New System.Drawing.Size(154, 22)
        Me.tsmiNuevoPago.Text = "Nuevo Pago"
        Me.tsmiNuevoPago.Visible = False
        '
        'tsmiEliminarPago
        '
        Me.tsmiEliminarPago.Name = "tsmiEliminarPago"
        Me.tsmiEliminarPago.Size = New System.Drawing.Size(154, 22)
        Me.tsmiEliminarPago.Text = "Eliminar Pago"
        Me.tsmiEliminarPago.Visible = False
        '
        'tsbPolizas
        '
        Me.tsbPolizas.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.tsbPolizas.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.tsmiVerPolizas, Me.tsmiNuevaPoliza, Me.tsmiEliminarPoliza})
        Me.tsbPolizas.Image = Global.Oversight.My.Resources.Resources.note24x24
        Me.tsbPolizas.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.tsbPolizas.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbPolizas.Name = "tsbPolizas"
        Me.tsbPolizas.Size = New System.Drawing.Size(37, 28)
        Me.tsbPolizas.Text = "Pólizas"
        Me.tsbPolizas.Visible = False
        '
        'tsmiVerPolizas
        '
        Me.tsmiVerPolizas.Name = "tsmiVerPolizas"
        Me.tsmiVerPolizas.Size = New System.Drawing.Size(140, 22)
        Me.tsmiVerPolizas.Text = "Ver Pólizas"
        Me.tsmiVerPolizas.Visible = False
        '
        'tsmiNuevaPoliza
        '
        Me.tsmiNuevaPoliza.Name = "tsmiNuevaPoliza"
        Me.tsmiNuevaPoliza.Size = New System.Drawing.Size(140, 22)
        Me.tsmiNuevaPoliza.Text = "Nueva Póliza"
        Me.tsmiNuevaPoliza.Visible = False
        '
        'tsmiEliminarPoliza
        '
        Me.tsmiEliminarPoliza.Name = "tsmiEliminarPoliza"
        Me.tsmiEliminarPoliza.Size = New System.Drawing.Size(140, 22)
        Me.tsmiEliminarPoliza.Text = "Eliminar Póliza"
        Me.tsmiEliminarPoliza.Visible = False
        '
        'tsbCuentas
        '
        Me.tsbCuentas.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.tsbCuentas.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.tsmiVerCuentas, Me.tsmiNuevaCuenta, Me.tsmiEliminarCuenta, Me.tsContabilidadtss3, Me.tsmiVerSaldoEnCuentas})
        Me.tsbCuentas.Image = Global.Oversight.My.Resources.Resources.expenses24x24
        Me.tsbCuentas.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.tsbCuentas.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbCuentas.Name = "tsbCuentas"
        Me.tsbCuentas.Size = New System.Drawing.Size(37, 28)
        Me.tsbCuentas.Text = "Cuentas y Saldos"
        Me.tsbCuentas.Visible = False
        '
        'tsmiVerCuentas
        '
        Me.tsmiVerCuentas.Name = "tsmiVerCuentas"
        Me.tsmiVerCuentas.ShortcutKeys = System.Windows.Forms.Keys.F11
        Me.tsmiVerCuentas.Size = New System.Drawing.Size(177, 22)
        Me.tsmiVerCuentas.Text = "Ver Cuentas"
        Me.tsmiVerCuentas.Visible = False
        '
        'tsmiNuevaCuenta
        '
        Me.tsmiNuevaCuenta.Name = "tsmiNuevaCuenta"
        Me.tsmiNuevaCuenta.Size = New System.Drawing.Size(177, 22)
        Me.tsmiNuevaCuenta.Text = "Nueva Cuenta"
        Me.tsmiNuevaCuenta.Visible = False
        '
        'tsmiEliminarCuenta
        '
        Me.tsmiEliminarCuenta.Name = "tsmiEliminarCuenta"
        Me.tsmiEliminarCuenta.Size = New System.Drawing.Size(177, 22)
        Me.tsmiEliminarCuenta.Text = "Eliminar Cuenta"
        Me.tsmiEliminarCuenta.Visible = False
        '
        'tsContabilidadtss3
        '
        Me.tsContabilidadtss3.Name = "tsContabilidadtss3"
        Me.tsContabilidadtss3.Size = New System.Drawing.Size(174, 6)
        '
        'tsmiVerSaldoEnCuentas
        '
        Me.tsmiVerSaldoEnCuentas.Name = "tsmiVerSaldoEnCuentas"
        Me.tsmiVerSaldoEnCuentas.Size = New System.Drawing.Size(177, 22)
        Me.tsmiVerSaldoEnCuentas.Text = "Ver Saldo en Cuentas"
        Me.tsmiVerSaldoEnCuentas.Visible = False
        '
        'tsbFacturasMRD
        '
        Me.tsbFacturasMRD.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.tsbFacturasMRD.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.tsmiVerFacturasEmitidas, Me.tsmiNuevaFactura, Me.tsmiEliminarFactura})
        Me.tsbFacturasMRD.Enabled = False
        Me.tsbFacturasMRD.Image = Global.Oversight.My.Resources.Resources.invoiceFinance24x24
        Me.tsbFacturasMRD.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.tsbFacturasMRD.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbFacturasMRD.Name = "tsbFacturasMRD"
        Me.tsbFacturasMRD.Size = New System.Drawing.Size(37, 28)
        Me.tsbFacturasMRD.Text = "Facturas MRD"
        Me.tsbFacturasMRD.Visible = False
        '
        'tsmiVerFacturasEmitidas
        '
        Me.tsmiVerFacturasEmitidas.Name = "tsmiVerFacturasEmitidas"
        Me.tsmiVerFacturasEmitidas.Size = New System.Drawing.Size(177, 22)
        Me.tsmiVerFacturasEmitidas.Text = "Ver Facturas emitidas"
        Me.tsmiVerFacturasEmitidas.Visible = False
        '
        'tsmiNuevaFactura
        '
        Me.tsmiNuevaFactura.Name = "tsmiNuevaFactura"
        Me.tsmiNuevaFactura.Size = New System.Drawing.Size(177, 22)
        Me.tsmiNuevaFactura.Text = "Nueva Factura"
        Me.tsmiNuevaFactura.Visible = False
        '
        'tsmiEliminarFactura
        '
        Me.tsmiEliminarFactura.Name = "tsmiEliminarFactura"
        Me.tsmiEliminarFactura.Size = New System.Drawing.Size(177, 22)
        Me.tsmiEliminarFactura.Text = "Eliminar Factura"
        Me.tsmiEliminarFactura.Visible = False
        '
        'tsbReportes
        '
        Me.tsbReportes.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.tsbReportes.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.tsmiVerReportes})
        Me.tsbReportes.Image = Global.Oversight.My.Resources.Resources.report24x24
        Me.tsbReportes.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.tsbReportes.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbReportes.Name = "tsbReportes"
        Me.tsbReportes.Size = New System.Drawing.Size(37, 28)
        Me.tsbReportes.Text = "Reportes"
        Me.tsbReportes.Visible = False
        '
        'tsmiVerReportes
        '
        Me.tsmiVerReportes.Name = "tsmiVerReportes"
        Me.tsmiVerReportes.Size = New System.Drawing.Size(137, 22)
        Me.tsmiVerReportes.Text = "Ver Reportes"
        Me.tsmiVerReportes.Visible = False
        '
        'tsbMensajes
        '
        Me.tsbMensajes.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.tsbMensajes.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.tsmiVerMensajes, Me.tsmiNuevoMensaje})
        Me.tsbMensajes.Image = Global.Oversight.My.Resources.Resources.message24x24
        Me.tsbMensajes.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.tsbMensajes.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbMensajes.Name = "tsbMensajes"
        Me.tsbMensajes.Size = New System.Drawing.Size(37, 28)
        Me.tsbMensajes.Text = "Mensajes"
        Me.tsbMensajes.Visible = False
        '
        'tsmiVerMensajes
        '
        Me.tsmiVerMensajes.Name = "tsmiVerMensajes"
        Me.tsmiVerMensajes.Size = New System.Drawing.Size(171, 22)
        Me.tsmiVerMensajes.Text = "Ver Mensajes Leídos"
        Me.tsmiVerMensajes.Visible = False
        '
        'tsmiNuevoMensaje
        '
        Me.tsmiNuevoMensaje.Name = "tsmiNuevoMensaje"
        Me.tsmiNuevoMensaje.Size = New System.Drawing.Size(171, 22)
        Me.tsmiNuevoMensaje.Text = "Nuevo Mensaje"
        Me.tsmiNuevoMensaje.Visible = False
        '
        'tsbUsuarios
        '
        Me.tsbUsuarios.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.tsbUsuarios.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.tsmiVerUsuarios, Me.tsmiNuevoUsuario, Me.tsmiEliminarUsuario})
        Me.tsbUsuarios.Image = Global.Oversight.My.Resources.Resources.user24x24
        Me.tsbUsuarios.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.tsbUsuarios.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbUsuarios.Name = "tsbUsuarios"
        Me.tsbUsuarios.Size = New System.Drawing.Size(37, 28)
        Me.tsbUsuarios.Text = "Usuarios"
        Me.tsbUsuarios.Visible = False
        '
        'tsmiVerUsuarios
        '
        Me.tsmiVerUsuarios.Name = "tsmiVerUsuarios"
        Me.tsmiVerUsuarios.Size = New System.Drawing.Size(149, 22)
        Me.tsmiVerUsuarios.Text = "Ver Usuarios"
        Me.tsmiVerUsuarios.Visible = False
        '
        'tsmiNuevoUsuario
        '
        Me.tsmiNuevoUsuario.Name = "tsmiNuevoUsuario"
        Me.tsmiNuevoUsuario.Size = New System.Drawing.Size(149, 22)
        Me.tsmiNuevoUsuario.Text = "Nuevo Usuario"
        Me.tsmiNuevoUsuario.Visible = False
        '
        'tsmiEliminarUsuario
        '
        Me.tsmiEliminarUsuario.Name = "tsmiEliminarUsuario"
        Me.tsmiEliminarUsuario.Size = New System.Drawing.Size(149, 22)
        Me.tsmiEliminarUsuario.Text = "Eliminar Usuario"
        Me.tsmiEliminarUsuario.Visible = False
        '
        'tsbUnidades
        '
        Me.tsbUnidades.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.tsbUnidades.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.tsmiVerUnidades, Me.tsmiNuevaUnidad, Me.tsmiEliminarUnidad})
        Me.tsbUnidades.Image = Global.Oversight.My.Resources.Resources.unit24x24
        Me.tsbUnidades.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.tsbUnidades.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbUnidades.Name = "tsbUnidades"
        Me.tsbUnidades.Size = New System.Drawing.Size(37, 28)
        Me.tsbUnidades.Text = "Unidades"
        Me.tsbUnidades.Visible = False
        '
        'tsmiVerUnidades
        '
        Me.tsmiVerUnidades.Name = "tsmiVerUnidades"
        Me.tsmiVerUnidades.Size = New System.Drawing.Size(146, 22)
        Me.tsmiVerUnidades.Text = "Ver Unidades"
        Me.tsmiVerUnidades.Visible = False
        '
        'tsmiNuevaUnidad
        '
        Me.tsmiNuevaUnidad.Name = "tsmiNuevaUnidad"
        Me.tsmiNuevaUnidad.Size = New System.Drawing.Size(146, 22)
        Me.tsmiNuevaUnidad.Text = "Nueva Unidad"
        Me.tsmiNuevaUnidad.Visible = False
        '
        'tsmiEliminarUnidad
        '
        Me.tsmiEliminarUnidad.Name = "tsmiEliminarUnidad"
        Me.tsmiEliminarUnidad.Size = New System.Drawing.Size(146, 22)
        Me.tsmiEliminarUnidad.Text = "Eliminar Unidad"
        Me.tsmiEliminarUnidad.Visible = False
        '
        'NotifyIcon1
        '
        Me.NotifyIcon1.Icon = CType(resources.GetObject("NotifyIcon1.Icon"), System.Drawing.Icon)
        Me.NotifyIcon1.Text = "Oversight"
        '
        'MissionControl
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(840, 496)
        Me.Controls.Add(Me.tsGeneral)
        Me.Controls.Add(Me.tsProyectos)
        Me.Controls.Add(Me.msMain)
        Me.Controls.Add(Me.ssMain)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.IsMdiContainer = True
        Me.MainMenuStrip = Me.msMain
        Me.Name = "MissionControl"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Oversight"
        Me.msMain.ResumeLayout(False)
        Me.msMain.PerformLayout()
        Me.tsGeneral.ResumeLayout(False)
        Me.tsGeneral.PerformLayout()
        Me.ssMain.ResumeLayout(False)
        Me.ssMain.PerformLayout()
        Me.tsProyectos.ResumeLayout(False)
        Me.tsProyectos.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents mnuHelptsmiContents As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuHelp As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuHelptsmiIndex As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuHelptsmiSearch As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuHelptss1 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents mnuHelptsmiAbout As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuWindowtsmiArrangeIcons As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuWindowtsmiCloseAll As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuWindowtsmiNewWindow As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuWindows As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuWindowtsmiCascade As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuWindowtsmiTileVertical As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuWindowtsmiTileHorizontal As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuToolstsmiOptions As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents tsGeneraltsbAyuda As System.Windows.Forms.ToolStripButton
    Friend WithEvents tsGeneraltss2 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents tsGeneraltsbVistaPrevia As System.Windows.Forms.ToolStripButton
    Friend WithEvents ttMain As System.Windows.Forms.ToolTip
    Friend WithEvents tsStatusLabel As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents ssMain As System.Windows.Forms.StatusStrip
    Friend WithEvents tsGeneraltsbImprimir As System.Windows.Forms.ToolStripButton
    Friend WithEvents tsGeneraltsbNuevo As System.Windows.Forms.ToolStripButton
    Friend WithEvents tsGeneral As System.Windows.Forms.ToolStrip
    Friend WithEvents tsGeneraltsbAbrir As System.Windows.Forms.ToolStripButton
    Friend WithEvents tsGeneraltsbGuardar As System.Windows.Forms.ToolStripButton
    Friend WithEvents tsGeneraltss1 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents mnuFiletsmiPrintPreview As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuFiletsmiPrint As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuFiletss2 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents mnuFiletsmiExit As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuFiletss3 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents mnuFiletsmiPrintSetup As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuFiletsmiNew As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuFile As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuFiletsmiOpen As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuFiletss1 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents mnuFiletsmiSave As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents msMain As System.Windows.Forms.MenuStrip
    Friend WithEvents mnuEdit As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuEdittsmiUndo As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuEdittsmiRedo As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuEdittss1 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents mnuEdittsmiCut As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuEdittsmiCopy As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuEdittsmiPaste As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuEdittss2 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents mnuEdittsmiSelectAll As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuView As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuViewtsmiGeneralBar As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuViewtsmiStatusBar As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuTools As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents tsProyectos As System.Windows.Forms.ToolStrip
    Friend WithEvents mnuFiletsmiLogout As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents tsbMateriales As System.Windows.Forms.ToolStripDropDownButton
    Friend WithEvents tsmiVerMaterialesPrecios As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents tsmiNuevoMaterial As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents tsmiEliminarMaterial As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents tsbProyectos As System.Windows.Forms.ToolStripDropDownButton
    Friend WithEvents tsmiVerProyectos As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents tsmiNuevoProyecto As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents tsmiNuevoProyectoDesdeModelo As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents tsmiEliminarProyecto As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents tsbModelos As System.Windows.Forms.ToolStripDropDownButton
    Friend WithEvents tsmiVerModelos As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents tsmiNuevoModelo As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents tsmiEliminarModelo As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents tsbPresupuestosBase As System.Windows.Forms.ToolStripDropDownButton
    Friend WithEvents tsmiVerPresupuestosBase As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents tsmiNuevoPresupuestoBase As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents tsmiEliminarPresupuestoBase As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuToolstsmiModifyMyUser As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents NotifyIcon1 As System.Windows.Forms.NotifyIcon
    Friend WithEvents tsmiDuplicarProyecto As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents tsbActivos As System.Windows.Forms.ToolStripDropDownButton
    Friend WithEvents tsmiVerActivos As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents tsmiNuevoActivo As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents tsmiEliminarActivo As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents tsbProveedores As System.Windows.Forms.ToolStripDropDownButton
    Friend WithEvents tsmiVerFacturasDeProveedores As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents tsmiNuevaFacturaDeProveedor As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents tsmiEliminarFacturaDeProveedor As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents tsbVales As System.Windows.Forms.ToolStripDropDownButton
    Friend WithEvents tsmiVerValesDeGasolina As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents tsmiNuevoValeDeGasolina As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents tsmiEliminarValeDeGasolina As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents tsbInventarios As System.Windows.Forms.ToolStripDropDownButton
    Friend WithEvents tsmiVerInventarioDeMateriales As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents tsbIngresos As System.Windows.Forms.ToolStripDropDownButton
    Friend WithEvents tsmiVerIngresos As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents tsmiNuevoIngreso As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents tsmiEliminarIngreso As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents tsbCuentas As System.Windows.Forms.ToolStripDropDownButton
    Friend WithEvents tsmiVerCuentas As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents tsmiNuevaCuenta As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents tsmiEliminarCuenta As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents tsContabilidadtss3 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents tsmiVerSaldoEnCuentas As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents tsbUsuarios As System.Windows.Forms.ToolStripDropDownButton
    Friend WithEvents tsmiVerUsuarios As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents tsmiNuevoUsuario As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents tsmiEliminarUsuario As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents tsbUnidades As System.Windows.Forms.ToolStripDropDownButton
    Friend WithEvents tsmiVerUnidades As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents tsmiNuevaUnidad As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents tsmiEliminarUnidad As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents tsbCotizacionesMateriales As System.Windows.Forms.ToolStripDropDownButton
    Friend WithEvents tsmiNuevaCotizacionDeMateriales As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents tsbPedidos As System.Windows.Forms.ToolStripDropDownButton
    Friend WithEvents tsmiVerPedidosDeMateriales As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents tsmiNuevoPedidoDeMaterial As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents tsmiEliminarPedidosDeMaterial As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents tsbEnvios As System.Windows.Forms.ToolStripDropDownButton
    Friend WithEvents tsmiVerEnvíosDeMaterial As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents tsmiNuevoEnvíoDeMaterial As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents tsmiEliminarEnvíoDeMaterial As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents tsbFacturasMRD As System.Windows.Forms.ToolStripDropDownButton
    Friend WithEvents tsmiVerFacturasEmitidas As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents tsmiNuevaFactura As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents tsmiEliminarFactura As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents tsmiVerInventarioDeActivos As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator1 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents tsmiRevisarFacturasDeProveedores As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents tsbNominas As System.Windows.Forms.ToolStripDropDownButton
    Friend WithEvents tsmiVerNominas As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents tsmiNuevaNomina As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents tsmiEliminarNomina As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents tsVales As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents tsmiNuevaFacturaCombustibleVales As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents tsmiVerFacturaCombustibleVales As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents tsmiEliminarFacturaCombustibleVales As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents tsbIngresoSep1 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents tsmiVerPagos As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents tsmiNuevoPago As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents tsmiEliminarPago As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents tsmiVerCotizacionesDeMateriales As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents tsbDirectorio As System.Windows.Forms.ToolStripDropDownButton
    Friend WithEvents tsmiVerDirectorio As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents tsmiNuevaPersonaProveedor As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents tsmiEliminarPersonaProveedor As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents tsmiRecentlyOpenedFiles As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents tsbPolizas As System.Windows.Forms.ToolStripDropDownButton
    Friend WithEvents tsmiVerPolizas As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents tsmiNuevaPoliza As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents tsmiEliminarPoliza As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents tsbMensajes As System.Windows.Forms.ToolStripDropDownButton
    Friend WithEvents tsmiNuevoMensaje As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents tsmiVerMensajes As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents tsbReportes As System.Windows.Forms.ToolStripDropDownButton
    Friend WithEvents tsmiVerReportes As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents tsbCasetas As System.Windows.Forms.ToolStripDropDownButton
    Friend WithEvents tsmiVerGastosPorCasetas As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents tsmiNuevoGastoPorCaseta As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents tsmiEliminarGastoPorCaseta As System.Windows.Forms.ToolStripMenuItem

End Class
