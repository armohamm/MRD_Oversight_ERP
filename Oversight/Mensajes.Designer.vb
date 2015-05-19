<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Mensajes
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Mensajes))
        Me.dgvMensajes = New System.Windows.Forms.DataGridView
        Me.NotifyIcon1 = New System.Windows.Forms.NotifyIcon(Me.components)
        Me.btnLeido = New System.Windows.Forms.Button
        Me.btnNuevo = New System.Windows.Forms.Button
        Me.Timer1 = New System.Windows.Forms.Timer(Me.components)
        Me.lblNota = New System.Windows.Forms.Label
        CType(Me.dgvMensajes, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'dgvMensajes
        '
        Me.dgvMensajes.AllowUserToAddRows = False
        Me.dgvMensajes.AllowUserToDeleteRows = False
        Me.dgvMensajes.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.dgvMensajes.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText
        Me.dgvMensajes.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvMensajes.Location = New System.Drawing.Point(5, 34)
        Me.dgvMensajes.MultiSelect = False
        Me.dgvMensajes.Name = "dgvMensajes"
        Me.dgvMensajes.ReadOnly = True
        Me.dgvMensajes.RowHeadersVisible = False
        Me.dgvMensajes.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgvMensajes.Size = New System.Drawing.Size(794, 80)
        Me.dgvMensajes.TabIndex = 1
        '
        'NotifyIcon1
        '
        Me.NotifyIcon1.Icon = CType(resources.GetObject("NotifyIcon1.Icon"), System.Drawing.Icon)
        Me.NotifyIcon1.Text = "Oversight"
        '
        'btnLeido
        '
        Me.btnLeido.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnLeido.Enabled = False
        Me.btnLeido.Image = Global.Oversight.My.Resources.Resources.yes12x12
        Me.btnLeido.Location = New System.Drawing.Point(805, 34)
        Me.btnLeido.Name = "btnLeido"
        Me.btnLeido.Size = New System.Drawing.Size(28, 23)
        Me.btnLeido.TabIndex = 64
        Me.btnLeido.UseVisualStyleBackColor = True
        '
        'btnNuevo
        '
        Me.btnNuevo.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnNuevo.Enabled = False
        Me.btnNuevo.Location = New System.Drawing.Point(663, 5)
        Me.btnNuevo.Name = "btnNuevo"
        Me.btnNuevo.Size = New System.Drawing.Size(136, 23)
        Me.btnNuevo.TabIndex = 65
        Me.btnNuevo.Text = "Enviar Nuevo Mensaje"
        Me.btnNuevo.UseVisualStyleBackColor = True
        '
        'Timer1
        '
        Me.Timer1.Interval = 10000
        '
        'lblNota
        '
        Me.lblNota.AutoSize = True
        Me.lblNota.Location = New System.Drawing.Point(2, 10)
        Me.lblNota.Name = "lblNota"
        Me.lblNota.Size = New System.Drawing.Size(262, 13)
        Me.lblNota.TabIndex = 66
        Me.lblNota.Text = "Esta lista de Mensajes se actualiza cada 10 segundos"
        '
        'Mensajes
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(840, 125)
        Me.Controls.Add(Me.lblNota)
        Me.Controls.Add(Me.btnNuevo)
        Me.Controls.Add(Me.btnLeido)
        Me.Controls.Add(Me.dgvMensajes)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "Mensajes"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.Manual
        Me.Text = "Mensajes"
        CType(Me.dgvMensajes, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents dgvMensajes As System.Windows.Forms.DataGridView
    Friend WithEvents NotifyIcon1 As System.Windows.Forms.NotifyIcon
    Friend WithEvents btnLeido As System.Windows.Forms.Button
    Friend WithEvents btnNuevo As System.Windows.Forms.Button
    Friend WithEvents Timer1 As System.Windows.Forms.Timer
    Friend WithEvents lblNota As System.Windows.Forms.Label
End Class
