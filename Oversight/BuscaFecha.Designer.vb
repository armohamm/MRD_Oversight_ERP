<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class BuscaFecha
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(BuscaFecha))
        Me.gbFechaTermino = New System.Windows.Forms.GroupBox
        Me.dtFecha = New System.Windows.Forms.DateTimePicker
        Me.btnGuardar = New System.Windows.Forms.Button
        Me.NotifyIcon1 = New System.Windows.Forms.NotifyIcon(Me.components)
        Me.gbFechaTermino.SuspendLayout()
        Me.SuspendLayout()
        '
        'gbFechaTermino
        '
        Me.gbFechaTermino.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.gbFechaTermino.Controls.Add(Me.dtFecha)
        Me.gbFechaTermino.Controls.Add(Me.btnGuardar)
        Me.gbFechaTermino.Location = New System.Drawing.Point(5, 0)
        Me.gbFechaTermino.Name = "gbFechaTermino"
        Me.gbFechaTermino.Size = New System.Drawing.Size(234, 113)
        Me.gbFechaTermino.TabIndex = 1
        Me.gbFechaTermino.TabStop = False
        '
        'dtFecha
        '
        Me.dtFecha.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.dtFecha.Location = New System.Drawing.Point(19, 27)
        Me.dtFecha.Name = "dtFecha"
        Me.dtFecha.Size = New System.Drawing.Size(114, 20)
        Me.dtFecha.TabIndex = 61
        '
        'btnGuardar
        '
        Me.btnGuardar.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnGuardar.Image = Global.Oversight.My.Resources.Resources.save24x24
        Me.btnGuardar.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnGuardar.Location = New System.Drawing.Point(108, 68)
        Me.btnGuardar.Name = "btnGuardar"
        Me.btnGuardar.Size = New System.Drawing.Size(119, 34)
        Me.btnGuardar.TabIndex = 57
        Me.btnGuardar.Text = "&Guardar Fecha"
        Me.btnGuardar.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnGuardar.UseVisualStyleBackColor = True
        '
        'NotifyIcon1
        '
        Me.NotifyIcon1.Icon = CType(resources.GetObject("NotifyIcon1.Icon"), System.Drawing.Icon)
        Me.NotifyIcon1.Text = "Oversight"
        '
        'BuscaFecha
        '
        Me.AcceptButton = Me.btnGuardar
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(244, 119)
        Me.Controls.Add(Me.gbFechaTermino)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "BuscaFecha"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Selecciona Fecha"
        Me.gbFechaTermino.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents gbFechaTermino As System.Windows.Forms.GroupBox
    Friend WithEvents btnGuardar As System.Windows.Forms.Button
    Friend WithEvents dtFecha As System.Windows.Forms.DateTimePicker
    Friend WithEvents NotifyIcon1 As System.Windows.Forms.NotifyIcon
End Class
