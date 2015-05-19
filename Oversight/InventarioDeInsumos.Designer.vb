<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class InventarioDeInsumos
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(InventarioDeInsumos))
        Me.ToolStripStatusLabel1 = New System.Windows.Forms.ToolStripStatusLabel
        Me.ToolStripProgressBar1 = New System.Windows.Forms.ToolStripProgressBar
        Me.msSaveFileDialog = New System.Windows.Forms.SaveFileDialog
        Me.gbInventario = New System.Windows.Forms.GroupBox
        Me.btnCerrar = New System.Windows.Forms.Button
        Me.btnRehacerInventarioMostrado = New System.Windows.Forms.Button
        Me.btnEliminarInsumo = New System.Windows.Forms.Button
        Me.btnInsertarInsumo = New System.Windows.Forms.Button
        Me.btnNuevoInsumo = New System.Windows.Forms.Button
        Me.lblBuscar = New System.Windows.Forms.Label
        Me.txtBuscar = New System.Windows.Forms.TextBox
        Me.tcInventario = New System.Windows.Forms.TabControl
        Me.tpInventario = New System.Windows.Forms.TabPage
        Me.dgvInventario = New System.Windows.Forms.DataGridView
        Me.tpEntradas = New System.Windows.Forms.TabPage
        Me.dgvEntradas = New System.Windows.Forms.DataGridView
        Me.tpSalidas = New System.Windows.Forms.TabPage
        Me.dgvSalidas = New System.Windows.Forms.DataGridView
        Me.btnExportarExcel = New System.Windows.Forms.Button
        Me.btnGenerarHojaConfirmacion = New System.Windows.Forms.Button
        Me.NotifyIcon1 = New System.Windows.Forms.NotifyIcon(Me.components)
        Me.btnRevisiones = New System.Windows.Forms.Button
        Me.gbInventario.SuspendLayout()
        Me.tcInventario.SuspendLayout()
        Me.tpInventario.SuspendLayout()
        CType(Me.dgvInventario, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.tpEntradas.SuspendLayout()
        CType(Me.dgvEntradas, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.tpSalidas.SuspendLayout()
        CType(Me.dgvSalidas, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
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
        'gbInventario
        '
        Me.gbInventario.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.gbInventario.Controls.Add(Me.btnRevisiones)
        Me.gbInventario.Controls.Add(Me.btnCerrar)
        Me.gbInventario.Controls.Add(Me.btnRehacerInventarioMostrado)
        Me.gbInventario.Controls.Add(Me.btnEliminarInsumo)
        Me.gbInventario.Controls.Add(Me.btnInsertarInsumo)
        Me.gbInventario.Controls.Add(Me.btnNuevoInsumo)
        Me.gbInventario.Controls.Add(Me.lblBuscar)
        Me.gbInventario.Controls.Add(Me.txtBuscar)
        Me.gbInventario.Controls.Add(Me.tcInventario)
        Me.gbInventario.Controls.Add(Me.btnExportarExcel)
        Me.gbInventario.Controls.Add(Me.btnGenerarHojaConfirmacion)
        Me.gbInventario.Location = New System.Drawing.Point(4, -2)
        Me.gbInventario.Name = "gbInventario"
        Me.gbInventario.Size = New System.Drawing.Size(689, 555)
        Me.gbInventario.TabIndex = 14
        Me.gbInventario.TabStop = False
        '
        'btnCerrar
        '
        Me.btnCerrar.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnCerrar.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnCerrar.Image = Global.Oversight.My.Resources.Resources.saveclose33x24
        Me.btnCerrar.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnCerrar.Location = New System.Drawing.Point(485, 419)
        Me.btnCerrar.Name = "btnCerrar"
        Me.btnCerrar.Size = New System.Drawing.Size(163, 34)
        Me.btnCerrar.TabIndex = 71
        Me.btnCerrar.Text = "&Cerrar Inventario"
        Me.btnCerrar.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnCerrar.UseVisualStyleBackColor = True
        '
        'btnRehacerInventarioMostrado
        '
        Me.btnRehacerInventarioMostrado.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnRehacerInventarioMostrado.Enabled = False
        Me.btnRehacerInventarioMostrado.Image = Global.Oversight.My.Resources.Resources.inventory_2_24x24
        Me.btnRehacerInventarioMostrado.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnRehacerInventarioMostrado.Location = New System.Drawing.Point(10, 419)
        Me.btnRehacerInventarioMostrado.Name = "btnRehacerInventarioMostrado"
        Me.btnRehacerInventarioMostrado.Size = New System.Drawing.Size(320, 38)
        Me.btnRehacerInventarioMostrado.TabIndex = 70
        Me.btnRehacerInventarioMostrado.Text = "Rehacer SOLO el Inventario de los productos mostrados"
        Me.btnRehacerInventarioMostrado.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnRehacerInventarioMostrado.UseVisualStyleBackColor = True
        '
        'btnEliminarInsumo
        '
        Me.btnEliminarInsumo.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnEliminarInsumo.Enabled = False
        Me.btnEliminarInsumo.Image = Global.Oversight.My.Resources.Resources.delete12x12
        Me.btnEliminarInsumo.Location = New System.Drawing.Point(650, 130)
        Me.btnEliminarInsumo.Name = "btnEliminarInsumo"
        Me.btnEliminarInsumo.Size = New System.Drawing.Size(28, 23)
        Me.btnEliminarInsumo.TabIndex = 69
        Me.btnEliminarInsumo.UseVisualStyleBackColor = True
        '
        'btnInsertarInsumo
        '
        Me.btnInsertarInsumo.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnInsertarInsumo.Enabled = False
        Me.btnInsertarInsumo.Image = Global.Oversight.My.Resources.Resources.insertcard12x12
        Me.btnInsertarInsumo.Location = New System.Drawing.Point(650, 101)
        Me.btnInsertarInsumo.Name = "btnInsertarInsumo"
        Me.btnInsertarInsumo.Size = New System.Drawing.Size(28, 23)
        Me.btnInsertarInsumo.TabIndex = 68
        Me.btnInsertarInsumo.UseVisualStyleBackColor = True
        '
        'btnNuevoInsumo
        '
        Me.btnNuevoInsumo.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnNuevoInsumo.Enabled = False
        Me.btnNuevoInsumo.Image = Global.Oversight.My.Resources.Resources.newcard12x12
        Me.btnNuevoInsumo.Location = New System.Drawing.Point(650, 72)
        Me.btnNuevoInsumo.Name = "btnNuevoInsumo"
        Me.btnNuevoInsumo.Size = New System.Drawing.Size(28, 23)
        Me.btnNuevoInsumo.TabIndex = 67
        Me.btnNuevoInsumo.UseVisualStyleBackColor = True
        '
        'lblBuscar
        '
        Me.lblBuscar.AutoSize = True
        Me.lblBuscar.Location = New System.Drawing.Point(7, 16)
        Me.lblBuscar.Name = "lblBuscar"
        Me.lblBuscar.Size = New System.Drawing.Size(73, 13)
        Me.lblBuscar.TabIndex = 66
        Me.lblBuscar.Text = "Buscar/Filtrar:"
        '
        'txtBuscar
        '
        Me.txtBuscar.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtBuscar.Location = New System.Drawing.Point(86, 13)
        Me.txtBuscar.Name = "txtBuscar"
        Me.txtBuscar.Size = New System.Drawing.Size(562, 20)
        Me.txtBuscar.TabIndex = 1
        '
        'tcInventario
        '
        Me.tcInventario.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tcInventario.Controls.Add(Me.tpInventario)
        Me.tcInventario.Controls.Add(Me.tpEntradas)
        Me.tcInventario.Controls.Add(Me.tpSalidas)
        Me.tcInventario.Location = New System.Drawing.Point(10, 46)
        Me.tcInventario.Name = "tcInventario"
        Me.tcInventario.SelectedIndex = 0
        Me.tcInventario.Size = New System.Drawing.Size(638, 367)
        Me.tcInventario.TabIndex = 2
        '
        'tpInventario
        '
        Me.tpInventario.Controls.Add(Me.dgvInventario)
        Me.tpInventario.Location = New System.Drawing.Point(4, 22)
        Me.tpInventario.Name = "tpInventario"
        Me.tpInventario.Padding = New System.Windows.Forms.Padding(3)
        Me.tpInventario.Size = New System.Drawing.Size(630, 341)
        Me.tpInventario.TabIndex = 0
        Me.tpInventario.Text = "Inventario"
        Me.tpInventario.UseVisualStyleBackColor = True
        '
        'dgvInventario
        '
        Me.dgvInventario.AllowUserToAddRows = False
        Me.dgvInventario.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.dgvInventario.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvInventario.Location = New System.Drawing.Point(6, 6)
        Me.dgvInventario.Name = "dgvInventario"
        Me.dgvInventario.RowHeadersVisible = False
        Me.dgvInventario.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect
        Me.dgvInventario.Size = New System.Drawing.Size(618, 332)
        Me.dgvInventario.TabIndex = 3
        Me.dgvInventario.Visible = False
        '
        'tpEntradas
        '
        Me.tpEntradas.Controls.Add(Me.dgvEntradas)
        Me.tpEntradas.Location = New System.Drawing.Point(4, 22)
        Me.tpEntradas.Name = "tpEntradas"
        Me.tpEntradas.Size = New System.Drawing.Size(630, 341)
        Me.tpEntradas.TabIndex = 2
        Me.tpEntradas.Text = "Entradas de Inventario"
        Me.tpEntradas.UseVisualStyleBackColor = True
        '
        'dgvEntradas
        '
        Me.dgvEntradas.AllowUserToAddRows = False
        Me.dgvEntradas.AllowUserToDeleteRows = False
        Me.dgvEntradas.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.dgvEntradas.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvEntradas.Location = New System.Drawing.Point(6, 6)
        Me.dgvEntradas.MultiSelect = False
        Me.dgvEntradas.Name = "dgvEntradas"
        Me.dgvEntradas.RowHeadersVisible = False
        Me.dgvEntradas.Size = New System.Drawing.Size(618, 332)
        Me.dgvEntradas.TabIndex = 7
        Me.dgvEntradas.Visible = False
        '
        'tpSalidas
        '
        Me.tpSalidas.Controls.Add(Me.dgvSalidas)
        Me.tpSalidas.Location = New System.Drawing.Point(4, 22)
        Me.tpSalidas.Name = "tpSalidas"
        Me.tpSalidas.Padding = New System.Windows.Forms.Padding(3)
        Me.tpSalidas.Size = New System.Drawing.Size(630, 341)
        Me.tpSalidas.TabIndex = 1
        Me.tpSalidas.Text = "Salidas de Inventario"
        Me.tpSalidas.UseVisualStyleBackColor = True
        '
        'dgvSalidas
        '
        Me.dgvSalidas.AllowUserToAddRows = False
        Me.dgvSalidas.AllowUserToDeleteRows = False
        Me.dgvSalidas.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.dgvSalidas.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvSalidas.Location = New System.Drawing.Point(6, 6)
        Me.dgvSalidas.MultiSelect = False
        Me.dgvSalidas.Name = "dgvSalidas"
        Me.dgvSalidas.RowHeadersVisible = False
        Me.dgvSalidas.Size = New System.Drawing.Size(618, 332)
        Me.dgvSalidas.TabIndex = 8
        Me.dgvSalidas.Visible = False
        '
        'btnExportarExcel
        '
        Me.btnExportarExcel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnExportarExcel.Enabled = False
        Me.btnExportarExcel.Image = Global.Oversight.My.Resources.Resources.excel24x24
        Me.btnExportarExcel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnExportarExcel.Location = New System.Drawing.Point(10, 507)
        Me.btnExportarExcel.Name = "btnExportarExcel"
        Me.btnExportarExcel.Size = New System.Drawing.Size(319, 38)
        Me.btnExportarExcel.TabIndex = 6
        Me.btnExportarExcel.Text = "Exportar Inventario Mostrado a Excel"
        Me.btnExportarExcel.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnExportarExcel.UseVisualStyleBackColor = True
        '
        'btnGenerarHojaConfirmacion
        '
        Me.btnGenerarHojaConfirmacion.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnGenerarHojaConfirmacion.Enabled = False
        Me.btnGenerarHojaConfirmacion.Image = Global.Oversight.My.Resources.Resources.word24x24
        Me.btnGenerarHojaConfirmacion.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnGenerarHojaConfirmacion.Location = New System.Drawing.Point(10, 463)
        Me.btnGenerarHojaConfirmacion.Name = "btnGenerarHojaConfirmacion"
        Me.btnGenerarHojaConfirmacion.Size = New System.Drawing.Size(320, 38)
        Me.btnGenerarHojaConfirmacion.TabIndex = 5
        Me.btnGenerarHojaConfirmacion.Text = "Generar Hoja de Confirmación de Inventario"
        Me.btnGenerarHojaConfirmacion.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnGenerarHojaConfirmacion.UseVisualStyleBackColor = True
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
        Me.btnRevisiones.Location = New System.Drawing.Point(485, 509)
        Me.btnRevisiones.Name = "btnRevisiones"
        Me.btnRevisiones.Size = New System.Drawing.Size(163, 34)
        Me.btnRevisiones.TabIndex = 72
        Me.btnRevisiones.Text = "Revisiones"
        Me.btnRevisiones.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnRevisiones.UseVisualStyleBackColor = True
        '
        'InventarioDeInsumos
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(698, 557)
        Me.Controls.Add(Me.gbInventario)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "InventarioDeInsumos"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Inventario de Insumos"
        Me.gbInventario.ResumeLayout(False)
        Me.gbInventario.PerformLayout()
        Me.tcInventario.ResumeLayout(False)
        Me.tpInventario.ResumeLayout(False)
        CType(Me.dgvInventario, System.ComponentModel.ISupportInitialize).EndInit()
        Me.tpEntradas.ResumeLayout(False)
        CType(Me.dgvEntradas, System.ComponentModel.ISupportInitialize).EndInit()
        Me.tpSalidas.ResumeLayout(False)
        CType(Me.dgvSalidas, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents ToolStripStatusLabel1 As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents ToolStripProgressBar1 As System.Windows.Forms.ToolStripProgressBar
    Friend WithEvents msSaveFileDialog As System.Windows.Forms.SaveFileDialog
    Friend WithEvents gbInventario As System.Windows.Forms.GroupBox
    Friend WithEvents tcInventario As System.Windows.Forms.TabControl
    Friend WithEvents tpInventario As System.Windows.Forms.TabPage
    Friend WithEvents dgvInventario As System.Windows.Forms.DataGridView
    Friend WithEvents tpEntradas As System.Windows.Forms.TabPage
    Friend WithEvents dgvEntradas As System.Windows.Forms.DataGridView
    Friend WithEvents tpSalidas As System.Windows.Forms.TabPage
    Friend WithEvents dgvSalidas As System.Windows.Forms.DataGridView
    Friend WithEvents btnExportarExcel As System.Windows.Forms.Button
    Friend WithEvents btnGenerarHojaConfirmacion As System.Windows.Forms.Button
    Friend WithEvents lblBuscar As System.Windows.Forms.Label
    Friend WithEvents txtBuscar As System.Windows.Forms.TextBox
    Friend WithEvents btnEliminarInsumo As System.Windows.Forms.Button
    Friend WithEvents btnInsertarInsumo As System.Windows.Forms.Button
    Friend WithEvents btnNuevoInsumo As System.Windows.Forms.Button
    Friend WithEvents btnRehacerInventarioMostrado As System.Windows.Forms.Button
    Friend WithEvents btnCerrar As System.Windows.Forms.Button
    Friend WithEvents NotifyIcon1 As System.Windows.Forms.NotifyIcon
    Friend WithEvents btnRevisiones As System.Windows.Forms.Button
End Class
