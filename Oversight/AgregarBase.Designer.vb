<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class AgregarBase
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(AgregarBase))
        Me.tcBase = New System.Windows.Forms.TabControl
        Me.tpCostosIndirectos = New System.Windows.Forms.TabPage
        Me.btnEliminarCostoIndirecto = New System.Windows.Forms.Button
        Me.btnNuevoCostoIndirecto = New System.Windows.Forms.Button
        Me.lblFechaPresupuesto = New System.Windows.Forms.Label
        Me.dtFechaPresupuesto = New System.Windows.Forms.DateTimePicker
        Me.lblPorcentajeIndirectos = New System.Windows.Forms.Label
        Me.lblPorcentajeIndirectosLbl = New System.Windows.Forms.Label
        Me.txtIngresosIndirectos = New System.Windows.Forms.TextBox
        Me.lblIngresosIndirectos = New System.Windows.Forms.Label
        Me.lblTotalIndirectos = New System.Windows.Forms.Label
        Me.lblTotalIndirectosLbl = New System.Windows.Forms.Label
        Me.dgvCostosIndirectos = New System.Windows.Forms.DataGridView
        Me.btnPaso3 = New System.Windows.Forms.Button
        Me.tpResumenTarjetas = New System.Windows.Forms.TabPage
        Me.btnEliminarTarjeta = New System.Windows.Forms.Button
        Me.lblAsteriscoUtilidades = New System.Windows.Forms.Label
        Me.lblAsteriscoIndirectos = New System.Windows.Forms.Label
        Me.lblNotaAsteriscos = New System.Windows.Forms.Label
        Me.btnInsertarTarjeta = New System.Windows.Forms.Button
        Me.btnNuevaTarjeta = New System.Windows.Forms.Button
        Me.txtPorcentajeUtilidadDefault = New System.Windows.Forms.TextBox
        Me.lblPorcentajeUtilidadPorDefault = New System.Windows.Forms.Label
        Me.txtPorcentajeIndirectosDefault = New System.Windows.Forms.TextBox
        Me.lblPorcentajeIndirectosPorDefault = New System.Windows.Forms.Label
        Me.lblPercentageSignHelper = New System.Windows.Forms.Label
        Me.lblPorcentajeIVA = New System.Windows.Forms.Label
        Me.txtPorcentajeIVA = New System.Windows.Forms.TextBox
        Me.flpAccionesResumen = New System.Windows.Forms.FlowLayoutPanel
        Me.btnActualizarUtilidad = New System.Windows.Forms.Button
        Me.btnGenerarArchivoExcel = New System.Windows.Forms.Button
        Me.dgvResumenDeTarjetas = New System.Windows.Forms.DataGridView
        Me.ToolStripStatusLabel1 = New System.Windows.Forms.ToolStripStatusLabel
        Me.ToolStripProgressBar1 = New System.Windows.Forms.ToolStripProgressBar
        Me.msSaveFileDialog = New System.Windows.Forms.SaveFileDialog
        Me.btnGuardar = New System.Windows.Forms.Button
        Me.btnCancelar = New System.Windows.Forms.Button
        Me.btnGuardarYCerrar = New System.Windows.Forms.Button
        Me.NotifyIcon1 = New System.Windows.Forms.NotifyIcon(Me.components)
        Me.btnRevisiones = New System.Windows.Forms.Button
        Me.tcBase.SuspendLayout()
        Me.tpCostosIndirectos.SuspendLayout()
        CType(Me.dgvCostosIndirectos, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.tpResumenTarjetas.SuspendLayout()
        Me.flpAccionesResumen.SuspendLayout()
        CType(Me.dgvResumenDeTarjetas, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'tcBase
        '
        Me.tcBase.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tcBase.Controls.Add(Me.tpCostosIndirectos)
        Me.tcBase.Controls.Add(Me.tpResumenTarjetas)
        Me.tcBase.Location = New System.Drawing.Point(5, 7)
        Me.tcBase.Name = "tcBase"
        Me.tcBase.SelectedIndex = 0
        Me.tcBase.Size = New System.Drawing.Size(695, 422)
        Me.tcBase.TabIndex = 18
        '
        'tpCostosIndirectos
        '
        Me.tpCostosIndirectos.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.tpCostosIndirectos.Controls.Add(Me.btnRevisiones)
        Me.tpCostosIndirectos.Controls.Add(Me.btnEliminarCostoIndirecto)
        Me.tpCostosIndirectos.Controls.Add(Me.btnNuevoCostoIndirecto)
        Me.tpCostosIndirectos.Controls.Add(Me.lblFechaPresupuesto)
        Me.tpCostosIndirectos.Controls.Add(Me.dtFechaPresupuesto)
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
        Me.tpCostosIndirectos.Size = New System.Drawing.Size(687, 396)
        Me.tpCostosIndirectos.TabIndex = 2
        Me.tpCostosIndirectos.Text = "Costos Indirectos"
        Me.tpCostosIndirectos.UseVisualStyleBackColor = True
        '
        'btnEliminarCostoIndirecto
        '
        Me.btnEliminarCostoIndirecto.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnEliminarCostoIndirecto.Enabled = False
        Me.btnEliminarCostoIndirecto.Image = Global.Oversight.My.Resources.Resources.delete12x12
        Me.btnEliminarCostoIndirecto.Location = New System.Drawing.Point(656, 71)
        Me.btnEliminarCostoIndirecto.Name = "btnEliminarCostoIndirecto"
        Me.btnEliminarCostoIndirecto.Size = New System.Drawing.Size(28, 23)
        Me.btnEliminarCostoIndirecto.TabIndex = 70
        Me.btnEliminarCostoIndirecto.UseVisualStyleBackColor = True
        '
        'btnNuevoCostoIndirecto
        '
        Me.btnNuevoCostoIndirecto.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnNuevoCostoIndirecto.Enabled = False
        Me.btnNuevoCostoIndirecto.Image = Global.Oversight.My.Resources.Resources.newcard12x12
        Me.btnNuevoCostoIndirecto.Location = New System.Drawing.Point(656, 42)
        Me.btnNuevoCostoIndirecto.Name = "btnNuevoCostoIndirecto"
        Me.btnNuevoCostoIndirecto.Size = New System.Drawing.Size(28, 23)
        Me.btnNuevoCostoIndirecto.TabIndex = 69
        Me.btnNuevoCostoIndirecto.UseVisualStyleBackColor = True
        '
        'lblFechaPresupuesto
        '
        Me.lblFechaPresupuesto.AutoSize = True
        Me.lblFechaPresupuesto.Location = New System.Drawing.Point(4, 13)
        Me.lblFechaPresupuesto.Name = "lblFechaPresupuesto"
        Me.lblFechaPresupuesto.Size = New System.Drawing.Size(144, 13)
        Me.lblFechaPresupuesto.TabIndex = 59
        Me.lblFechaPresupuesto.Text = "Fecha de Presupuesto Base:"
        '
        'dtFechaPresupuesto
        '
        Me.dtFechaPresupuesto.Enabled = False
        Me.dtFechaPresupuesto.Location = New System.Drawing.Point(154, 9)
        Me.dtFechaPresupuesto.Name = "dtFechaPresupuesto"
        Me.dtFechaPresupuesto.Size = New System.Drawing.Size(254, 20)
        Me.dtFechaPresupuesto.TabIndex = 58
        Me.dtFechaPresupuesto.TabStop = False
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
        Me.lblTotalIndirectos.MaximumSize = New System.Drawing.Size(197, 0)
        Me.lblTotalIndirectos.MinimumSize = New System.Drawing.Size(197, 0)
        Me.lblTotalIndirectos.Name = "lblTotalIndirectos"
        Me.lblTotalIndirectos.Size = New System.Drawing.Size(197, 13)
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
        Me.dgvCostosIndirectos.Location = New System.Drawing.Point(3, 42)
        Me.dgvCostosIndirectos.MultiSelect = False
        Me.dgvCostosIndirectos.Name = "dgvCostosIndirectos"
        Me.dgvCostosIndirectos.ReadOnly = True
        Me.dgvCostosIndirectos.RowHeadersVisible = False
        Me.dgvCostosIndirectos.Size = New System.Drawing.Size(647, 274)
        Me.dgvCostosIndirectos.TabIndex = 19
        Me.dgvCostosIndirectos.Visible = False
        '
        'btnPaso3
        '
        Me.btnPaso3.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnPaso3.Image = Global.Oversight.My.Resources.Resources.next24x24
        Me.btnPaso3.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnPaso3.Location = New System.Drawing.Point(539, 335)
        Me.btnPaso3.Name = "btnPaso3"
        Me.btnPaso3.Size = New System.Drawing.Size(111, 34)
        Me.btnPaso3.TabIndex = 21
        Me.btnPaso3.Text = "&Siguiente Paso"
        Me.btnPaso3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnPaso3.UseVisualStyleBackColor = True
        '
        'tpResumenTarjetas
        '
        Me.tpResumenTarjetas.Controls.Add(Me.btnEliminarTarjeta)
        Me.tpResumenTarjetas.Controls.Add(Me.lblAsteriscoUtilidades)
        Me.tpResumenTarjetas.Controls.Add(Me.lblAsteriscoIndirectos)
        Me.tpResumenTarjetas.Controls.Add(Me.lblNotaAsteriscos)
        Me.tpResumenTarjetas.Controls.Add(Me.btnInsertarTarjeta)
        Me.tpResumenTarjetas.Controls.Add(Me.btnNuevaTarjeta)
        Me.tpResumenTarjetas.Controls.Add(Me.txtPorcentajeUtilidadDefault)
        Me.tpResumenTarjetas.Controls.Add(Me.lblPorcentajeUtilidadPorDefault)
        Me.tpResumenTarjetas.Controls.Add(Me.txtPorcentajeIndirectosDefault)
        Me.tpResumenTarjetas.Controls.Add(Me.lblPorcentajeIndirectosPorDefault)
        Me.tpResumenTarjetas.Controls.Add(Me.lblPercentageSignHelper)
        Me.tpResumenTarjetas.Controls.Add(Me.lblPorcentajeIVA)
        Me.tpResumenTarjetas.Controls.Add(Me.txtPorcentajeIVA)
        Me.tpResumenTarjetas.Controls.Add(Me.flpAccionesResumen)
        Me.tpResumenTarjetas.Controls.Add(Me.dgvResumenDeTarjetas)
        Me.tpResumenTarjetas.Location = New System.Drawing.Point(4, 22)
        Me.tpResumenTarjetas.Name = "tpResumenTarjetas"
        Me.tpResumenTarjetas.Padding = New System.Windows.Forms.Padding(3)
        Me.tpResumenTarjetas.Size = New System.Drawing.Size(687, 396)
        Me.tpResumenTarjetas.TabIndex = 1
        Me.tpResumenTarjetas.Text = "Resumen de Tarjetas"
        Me.tpResumenTarjetas.UseVisualStyleBackColor = True
        '
        'btnEliminarTarjeta
        '
        Me.btnEliminarTarjeta.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnEliminarTarjeta.Enabled = False
        Me.btnEliminarTarjeta.Image = Global.Oversight.My.Resources.Resources.delete12x12
        Me.btnEliminarTarjeta.Location = New System.Drawing.Point(656, 64)
        Me.btnEliminarTarjeta.Name = "btnEliminarTarjeta"
        Me.btnEliminarTarjeta.Size = New System.Drawing.Size(28, 23)
        Me.btnEliminarTarjeta.TabIndex = 77
        Me.btnEliminarTarjeta.UseVisualStyleBackColor = True
        '
        'lblAsteriscoUtilidades
        '
        Me.lblAsteriscoUtilidades.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblAsteriscoUtilidades.AutoSize = True
        Me.lblAsteriscoUtilidades.Location = New System.Drawing.Point(635, 285)
        Me.lblAsteriscoUtilidades.Name = "lblAsteriscoUtilidades"
        Me.lblAsteriscoUtilidades.Size = New System.Drawing.Size(11, 13)
        Me.lblAsteriscoUtilidades.TabIndex = 76
        Me.lblAsteriscoUtilidades.Text = "*"
        '
        'lblAsteriscoIndirectos
        '
        Me.lblAsteriscoIndirectos.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblAsteriscoIndirectos.AutoSize = True
        Me.lblAsteriscoIndirectos.Location = New System.Drawing.Point(635, 259)
        Me.lblAsteriscoIndirectos.Name = "lblAsteriscoIndirectos"
        Me.lblAsteriscoIndirectos.Size = New System.Drawing.Size(11, 13)
        Me.lblAsteriscoIndirectos.TabIndex = 75
        Me.lblAsteriscoIndirectos.Text = "*"
        '
        'lblNotaAsteriscos
        '
        Me.lblNotaAsteriscos.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblNotaAsteriscos.AutoSize = True
        Me.lblNotaAsteriscos.Location = New System.Drawing.Point(519, 321)
        Me.lblNotaAsteriscos.Name = "lblNotaAsteriscos"
        Me.lblNotaAsteriscos.Size = New System.Drawing.Size(110, 13)
        Me.lblNotaAsteriscos.TabIndex = 74
        Me.lblNotaAsteriscos.Text = "* para nuevas tarjetas"
        '
        'btnInsertarTarjeta
        '
        Me.btnInsertarTarjeta.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnInsertarTarjeta.Enabled = False
        Me.btnInsertarTarjeta.Image = Global.Oversight.My.Resources.Resources.insertcard12x12
        Me.btnInsertarTarjeta.Location = New System.Drawing.Point(656, 35)
        Me.btnInsertarTarjeta.Name = "btnInsertarTarjeta"
        Me.btnInsertarTarjeta.Size = New System.Drawing.Size(28, 23)
        Me.btnInsertarTarjeta.TabIndex = 73
        Me.btnInsertarTarjeta.UseVisualStyleBackColor = True
        Me.btnInsertarTarjeta.Visible = False
        '
        'btnNuevaTarjeta
        '
        Me.btnNuevaTarjeta.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnNuevaTarjeta.Enabled = False
        Me.btnNuevaTarjeta.Image = Global.Oversight.My.Resources.Resources.newcard12x12
        Me.btnNuevaTarjeta.Location = New System.Drawing.Point(656, 6)
        Me.btnNuevaTarjeta.Name = "btnNuevaTarjeta"
        Me.btnNuevaTarjeta.Size = New System.Drawing.Size(28, 23)
        Me.btnNuevaTarjeta.TabIndex = 72
        Me.btnNuevaTarjeta.UseVisualStyleBackColor = True
        '
        'txtPorcentajeUtilidadDefault
        '
        Me.txtPorcentajeUtilidadDefault.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtPorcentajeUtilidadDefault.Enabled = False
        Me.txtPorcentajeUtilidadDefault.Location = New System.Drawing.Point(578, 282)
        Me.txtPorcentajeUtilidadDefault.MaxLength = 20
        Me.txtPorcentajeUtilidadDefault.Name = "txtPorcentajeUtilidadDefault"
        Me.txtPorcentajeUtilidadDefault.Size = New System.Drawing.Size(51, 20)
        Me.txtPorcentajeUtilidadDefault.TabIndex = 71
        Me.txtPorcentajeUtilidadDefault.Text = "0"
        '
        'lblPorcentajeUtilidadPorDefault
        '
        Me.lblPorcentajeUtilidadPorDefault.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblPorcentajeUtilidadPorDefault.AutoSize = True
        Me.lblPorcentajeUtilidadPorDefault.Location = New System.Drawing.Point(463, 285)
        Me.lblPorcentajeUtilidadPorDefault.Name = "lblPorcentajeUtilidadPorDefault"
        Me.lblPorcentajeUtilidadPorDefault.Size = New System.Drawing.Size(109, 13)
        Me.lblPorcentajeUtilidadPorDefault.TabIndex = 69
        Me.lblPorcentajeUtilidadPorDefault.Text = "% Utilidad por default:"
        '
        'txtPorcentajeIndirectosDefault
        '
        Me.txtPorcentajeIndirectosDefault.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtPorcentajeIndirectosDefault.Enabled = False
        Me.txtPorcentajeIndirectosDefault.Location = New System.Drawing.Point(578, 256)
        Me.txtPorcentajeIndirectosDefault.MaxLength = 20
        Me.txtPorcentajeIndirectosDefault.Name = "txtPorcentajeIndirectosDefault"
        Me.txtPorcentajeIndirectosDefault.Size = New System.Drawing.Size(51, 20)
        Me.txtPorcentajeIndirectosDefault.TabIndex = 70
        Me.txtPorcentajeIndirectosDefault.Text = "0"
        '
        'lblPorcentajeIndirectosPorDefault
        '
        Me.lblPorcentajeIndirectosPorDefault.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblPorcentajeIndirectosPorDefault.AutoSize = True
        Me.lblPorcentajeIndirectosPorDefault.Location = New System.Drawing.Point(452, 259)
        Me.lblPorcentajeIndirectosPorDefault.Name = "lblPorcentajeIndirectosPorDefault"
        Me.lblPorcentajeIndirectosPorDefault.Size = New System.Drawing.Size(120, 13)
        Me.lblPorcentajeIndirectosPorDefault.TabIndex = 68
        Me.lblPorcentajeIndirectosPorDefault.Text = "% Indirectos por default:"
        '
        'lblPercentageSignHelper
        '
        Me.lblPercentageSignHelper.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblPercentageSignHelper.AutoSize = True
        Me.lblPercentageSignHelper.Location = New System.Drawing.Point(635, 234)
        Me.lblPercentageSignHelper.Name = "lblPercentageSignHelper"
        Me.lblPercentageSignHelper.Size = New System.Drawing.Size(15, 13)
        Me.lblPercentageSignHelper.TabIndex = 61
        Me.lblPercentageSignHelper.Text = "%"
        '
        'lblPorcentajeIVA
        '
        Me.lblPorcentajeIVA.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblPorcentajeIVA.AutoSize = True
        Me.lblPorcentajeIVA.Location = New System.Drawing.Point(431, 234)
        Me.lblPorcentajeIVA.Name = "lblPorcentajeIVA"
        Me.lblPorcentajeIVA.Size = New System.Drawing.Size(141, 13)
        Me.lblPorcentajeIVA.TabIndex = 60
        Me.lblPorcentajeIVA.Text = "Porcentaje de IVA aplicable:"
        '
        'txtPorcentajeIVA
        '
        Me.txtPorcentajeIVA.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtPorcentajeIVA.Enabled = False
        Me.txtPorcentajeIVA.Location = New System.Drawing.Point(578, 231)
        Me.txtPorcentajeIVA.MaxLength = 20
        Me.txtPorcentajeIVA.Name = "txtPorcentajeIVA"
        Me.txtPorcentajeIVA.Size = New System.Drawing.Size(51, 20)
        Me.txtPorcentajeIVA.TabIndex = 22
        Me.txtPorcentajeIVA.Text = "0"
        '
        'flpAccionesResumen
        '
        Me.flpAccionesResumen.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.flpAccionesResumen.Controls.Add(Me.btnActualizarUtilidad)
        Me.flpAccionesResumen.Controls.Add(Me.btnGenerarArchivoExcel)
        Me.flpAccionesResumen.Location = New System.Drawing.Point(5, 231)
        Me.flpAccionesResumen.Name = "flpAccionesResumen"
        Me.flpAccionesResumen.Size = New System.Drawing.Size(170, 82)
        Me.flpAccionesResumen.TabIndex = 58
        '
        'btnActualizarUtilidad
        '
        Me.btnActualizarUtilidad.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnActualizarUtilidad.Enabled = False
        Me.btnActualizarUtilidad.Image = Global.Oversight.My.Resources.Resources.percent24x24
        Me.btnActualizarUtilidad.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnActualizarUtilidad.Location = New System.Drawing.Point(3, 3)
        Me.btnActualizarUtilidad.Name = "btnActualizarUtilidad"
        Me.btnActualizarUtilidad.Size = New System.Drawing.Size(162, 34)
        Me.btnActualizarUtilidad.TabIndex = 25
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
        Me.btnGenerarArchivoExcel.Location = New System.Drawing.Point(3, 43)
        Me.btnGenerarArchivoExcel.Name = "btnGenerarArchivoExcel"
        Me.btnGenerarArchivoExcel.Size = New System.Drawing.Size(162, 34)
        Me.btnGenerarArchivoExcel.TabIndex = 27
        Me.btnGenerarArchivoExcel.Text = "Generar Archivo &Excel"
        Me.btnGenerarArchivoExcel.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnGenerarArchivoExcel.UseVisualStyleBackColor = True
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
        Me.dgvResumenDeTarjetas.Size = New System.Drawing.Size(645, 219)
        Me.dgvResumenDeTarjetas.TabIndex = 23
        Me.dgvResumenDeTarjetas.Visible = False
        '
        'ToolStripStatusLabel1
        '
        Me.ToolStripStatusLabel1.Name = "ToolStripStatusLabel1"
        Me.ToolStripStatusLabel1.Size = New System.Drawing.Size(244, 17)
        Me.ToolStripStatusLabel1.Text = "% de Información de Proyecto Completada : "
        '
        'ToolStripProgressBar1
        '
        Me.ToolStripProgressBar1.Name = "ToolStripProgressBar1"
        Me.ToolStripProgressBar1.Size = New System.Drawing.Size(100, 16)
        '
        'btnGuardar
        '
        Me.btnGuardar.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnGuardar.Enabled = False
        Me.btnGuardar.Image = CType(resources.GetObject("btnGuardar.Image"), System.Drawing.Image)
        Me.btnGuardar.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnGuardar.Location = New System.Drawing.Point(303, 435)
        Me.btnGuardar.Name = "btnGuardar"
        Me.btnGuardar.Size = New System.Drawing.Size(171, 34)
        Me.btnGuardar.TabIndex = 24
        Me.btnGuardar.Text = "&Guardar Presupuesto Base"
        Me.btnGuardar.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnGuardar.UseVisualStyleBackColor = True
        '
        'btnCancelar
        '
        Me.btnCancelar.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnCancelar.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnCancelar.Image = Global.Oversight.My.Resources.Resources.cancel24x24
        Me.btnCancelar.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnCancelar.Location = New System.Drawing.Point(204, 435)
        Me.btnCancelar.Name = "btnCancelar"
        Me.btnCancelar.Size = New System.Drawing.Size(93, 34)
        Me.btnCancelar.TabIndex = 22
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
        Me.btnGuardarYCerrar.Location = New System.Drawing.Point(480, 435)
        Me.btnGuardarYCerrar.Name = "btnGuardarYCerrar"
        Me.btnGuardarYCerrar.Size = New System.Drawing.Size(220, 34)
        Me.btnGuardarYCerrar.TabIndex = 23
        Me.btnGuardarYCerrar.Text = "G&uardar Presupuesto Base y Cerrar"
        Me.btnGuardarYCerrar.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnGuardarYCerrar.UseVisualStyleBackColor = True
        '
        'NotifyIcon1
        '
        Me.NotifyIcon1.Icon = CType(resources.GetObject("NotifyIcon1.Icon"), System.Drawing.Icon)
        Me.NotifyIcon1.Text = "Oversight"
        '
        'btnRevisiones
        '
        Me.btnRevisiones.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnRevisiones.Enabled = False
        Me.btnRevisiones.Image = Global.Oversight.My.Resources.Resources.yes24x24
        Me.btnRevisiones.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnRevisiones.Location = New System.Drawing.Point(422, 335)
        Me.btnRevisiones.Name = "btnRevisiones"
        Me.btnRevisiones.Size = New System.Drawing.Size(111, 34)
        Me.btnRevisiones.TabIndex = 71
        Me.btnRevisiones.Text = "Revisiones"
        Me.btnRevisiones.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnRevisiones.UseVisualStyleBackColor = True
        '
        'AgregarBase
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(708, 475)
        Me.Controls.Add(Me.btnGuardar)
        Me.Controls.Add(Me.btnCancelar)
        Me.Controls.Add(Me.btnGuardarYCerrar)
        Me.Controls.Add(Me.tcBase)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "AgregarBase"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Presupuesto Base"
        Me.tcBase.ResumeLayout(False)
        Me.tpCostosIndirectos.ResumeLayout(False)
        Me.tpCostosIndirectos.PerformLayout()
        CType(Me.dgvCostosIndirectos, System.ComponentModel.ISupportInitialize).EndInit()
        Me.tpResumenTarjetas.ResumeLayout(False)
        Me.tpResumenTarjetas.PerformLayout()
        Me.flpAccionesResumen.ResumeLayout(False)
        CType(Me.dgvResumenDeTarjetas, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents tcBase As System.Windows.Forms.TabControl
    Friend WithEvents tpResumenTarjetas As System.Windows.Forms.TabPage
    Friend WithEvents tpCostosIndirectos As System.Windows.Forms.TabPage
    Friend WithEvents ToolStripStatusLabel1 As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents ToolStripProgressBar1 As System.Windows.Forms.ToolStripProgressBar
    Friend WithEvents dgvResumenDeTarjetas As System.Windows.Forms.DataGridView
    Friend WithEvents btnPaso3 As System.Windows.Forms.Button
    Friend WithEvents flpAccionesResumen As System.Windows.Forms.FlowLayoutPanel
    Friend WithEvents lblPercentageSignHelper As System.Windows.Forms.Label
    Friend WithEvents lblPorcentajeIVA As System.Windows.Forms.Label
    Friend WithEvents txtPorcentajeIVA As System.Windows.Forms.TextBox
    Friend WithEvents btnActualizarUtilidad As System.Windows.Forms.Button
    Friend WithEvents btnGenerarArchivoExcel As System.Windows.Forms.Button
    Friend WithEvents msSaveFileDialog As System.Windows.Forms.SaveFileDialog
    Friend WithEvents dgvCostosIndirectos As System.Windows.Forms.DataGridView
    Friend WithEvents lblTotalIndirectos As System.Windows.Forms.Label
    Friend WithEvents lblTotalIndirectosLbl As System.Windows.Forms.Label
    Friend WithEvents lblPorcentajeIndirectos As System.Windows.Forms.Label
    Friend WithEvents lblPorcentajeIndirectosLbl As System.Windows.Forms.Label
    Friend WithEvents txtIngresosIndirectos As System.Windows.Forms.TextBox
    Friend WithEvents lblIngresosIndirectos As System.Windows.Forms.Label
    Friend WithEvents lblFechaPresupuesto As System.Windows.Forms.Label
    Friend WithEvents dtFechaPresupuesto As System.Windows.Forms.DateTimePicker
    Friend WithEvents txtPorcentajeUtilidadDefault As System.Windows.Forms.TextBox
    Friend WithEvents lblPorcentajeUtilidadPorDefault As System.Windows.Forms.Label
    Friend WithEvents txtPorcentajeIndirectosDefault As System.Windows.Forms.TextBox
    Friend WithEvents lblPorcentajeIndirectosPorDefault As System.Windows.Forms.Label
    Friend WithEvents btnGuardar As System.Windows.Forms.Button
    Friend WithEvents btnCancelar As System.Windows.Forms.Button
    Friend WithEvents btnGuardarYCerrar As System.Windows.Forms.Button
    Friend WithEvents btnEliminarCostoIndirecto As System.Windows.Forms.Button
    Friend WithEvents btnNuevoCostoIndirecto As System.Windows.Forms.Button
    Friend WithEvents btnInsertarTarjeta As System.Windows.Forms.Button
    Friend WithEvents btnNuevaTarjeta As System.Windows.Forms.Button
    Friend WithEvents lblAsteriscoUtilidades As System.Windows.Forms.Label
    Friend WithEvents lblAsteriscoIndirectos As System.Windows.Forms.Label
    Friend WithEvents lblNotaAsteriscos As System.Windows.Forms.Label
    Friend WithEvents btnEliminarTarjeta As System.Windows.Forms.Button
    Friend WithEvents NotifyIcon1 As System.Windows.Forms.NotifyIcon
    Friend WithEvents btnRevisiones As System.Windows.Forms.Button
End Class
