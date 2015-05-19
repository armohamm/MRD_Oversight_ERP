<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class BuscaArchivosCerradosIncorrectamente
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(BuscaArchivosCerradosIncorrectamente))
        Me.gbBuscar = New System.Windows.Forms.GroupBox
        Me.lblInstructions1 = New System.Windows.Forms.Label
        Me.dgvArchivos = New System.Windows.Forms.DataGridView
        Me.btnCerrarBorrar = New System.Windows.Forms.Button
        Me.NotifyIcon1 = New System.Windows.Forms.NotifyIcon(Me.components)
        Me.gbBuscar.SuspendLayout()
        CType(Me.dgvArchivos, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'gbBuscar
        '
        Me.gbBuscar.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.gbBuscar.Controls.Add(Me.lblInstructions1)
        Me.gbBuscar.Controls.Add(Me.dgvArchivos)
        Me.gbBuscar.Controls.Add(Me.btnCerrarBorrar)
        Me.gbBuscar.Location = New System.Drawing.Point(7, 2)
        Me.gbBuscar.Name = "gbBuscar"
        Me.gbBuscar.Size = New System.Drawing.Size(544, 235)
        Me.gbBuscar.TabIndex = 29
        Me.gbBuscar.TabStop = False
        '
        'lblInstructions1
        '
        Me.lblInstructions1.AutoSize = True
        Me.lblInstructions1.Location = New System.Drawing.Point(7, 12)
        Me.lblInstructions1.MaximumSize = New System.Drawing.Size(525, 0)
        Me.lblInstructions1.Name = "lblInstructions1"
        Me.lblInstructions1.Size = New System.Drawing.Size(340, 13)
        Me.lblInstructions1.TabIndex = 32
        Me.lblInstructions1.Text = "Haz doble click sobre los archivos que intentaremos recuperar para tí. "
        '
        'dgvArchivos
        '
        Me.dgvArchivos.AllowUserToAddRows = False
        Me.dgvArchivos.AllowUserToDeleteRows = False
        Me.dgvArchivos.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.dgvArchivos.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText
        Me.dgvArchivos.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvArchivos.Location = New System.Drawing.Point(10, 34)
        Me.dgvArchivos.MultiSelect = False
        Me.dgvArchivos.Name = "dgvArchivos"
        Me.dgvArchivos.ReadOnly = True
        Me.dgvArchivos.RowHeadersVisible = False
        Me.dgvArchivos.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgvArchivos.Size = New System.Drawing.Size(527, 152)
        Me.dgvArchivos.TabIndex = 1
        '
        'btnCerrarBorrar
        '
        Me.btnCerrarBorrar.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnCerrarBorrar.Image = Global.Oversight.My.Resources.Resources.delete24x24
        Me.btnCerrarBorrar.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnCerrarBorrar.Location = New System.Drawing.Point(366, 192)
        Me.btnCerrarBorrar.Name = "btnCerrarBorrar"
        Me.btnCerrarBorrar.Size = New System.Drawing.Size(171, 34)
        Me.btnCerrarBorrar.TabIndex = 29
        Me.btnCerrarBorrar.Text = "Borrar Archivos Restantes"
        Me.btnCerrarBorrar.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnCerrarBorrar.UseVisualStyleBackColor = True
        '
        'NotifyIcon1
        '
        Me.NotifyIcon1.Icon = CType(resources.GetObject("NotifyIcon1.Icon"), System.Drawing.Icon)
        Me.NotifyIcon1.Text = "Oversight"
        '
        'BuscaArchivosCerradosIncorrectamente
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(558, 242)
        Me.Controls.Add(Me.gbBuscar)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "BuscaArchivosCerradosIncorrectamente"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Archivos con Posible Recuperación"
        Me.gbBuscar.ResumeLayout(False)
        Me.gbBuscar.PerformLayout()
        CType(Me.dgvArchivos, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents gbBuscar As System.Windows.Forms.GroupBox
    Friend WithEvents btnCerrarBorrar As System.Windows.Forms.Button
    Friend WithEvents dgvArchivos As System.Windows.Forms.DataGridView
    Friend WithEvents lblInstructions1 As System.Windows.Forms.Label
    Friend WithEvents NotifyIcon1 As System.Windows.Forms.NotifyIcon
End Class
