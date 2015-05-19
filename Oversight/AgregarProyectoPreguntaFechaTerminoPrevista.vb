Public Class AgregarProyectoPreguntaFechaTerminoPrevista

    Private fDone As Boolean = False

    Public susername As String = ""
    Public bactive As Boolean = False
    Public bonline As Boolean = False
    Public suserfullname As String = ""
    Public suseremail As String = ""
    Public susersession As Integer = 0
    Public susermachinename As String = ""
    Public suserip As String = "0.0.0.0"

    Public iprojectid As Integer = 0

    Public fechaprevista As Date = Now


    Private Sub AgregarProyectoPreguntaFechaTerminoPrevista_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing

        executeSQLCommand(0, "INSERT IGNORE INTO logs VALUES (" & getMySQLDate() & ", '" & getAppTime() & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Cerró la Ventana para definir Fecha de Término Prevista del proyecto " & iprojectid & " (Realizar Obra)', 'OK')")

    End Sub


    Private Sub AgregarProyectoPreguntaFechaTerminoPrevista_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown

        If e.KeyCode = Keys.F5 Then

            If My.Computer.Info.OSFullName.StartsWith("Microsoft Windows 7") = True Then
                NotifyIcon1.Icon = Oversight.My.Resources.winmineVista16x16
            Else
                NotifyIcon1.Icon = Oversight.My.Resources.winmineXP16x16
            End If

            NotifyIcon1.Text = "Buscaminas"

            NotifyIcon1.Visible = True

            Me.Visible = False
            Do While Not fDone
                System.Windows.Forms.Application.DoEvents()
            Loop
            fDone = False
            Me.Visible = True

        End If

    End Sub


    Private Sub PreguntaFechaTerminoPrevista_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        Me.KeyPreview = True

        dtFechaTerminoPrevista.Select()
        dtFechaTerminoPrevista.Focus()

        Me.AcceptButton = btnGuardar

        executeSQLCommand(0, "INSERT IGNORE INTO logs VALUES (" & getMySQLDate() & ", '" & getAppTime() & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Abrió la Ventana para definir Fecha de Término Prevista del proyecto " & iprojectid & " (Realizar Obra)', 'OK')")

        Cursor.Current = System.Windows.Forms.Cursors.Default

    End Sub


    Private Sub NotifyIcon1_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles NotifyIcon1.DoubleClick

        Dim n As New Loader

        n.isEdit = True

        n.ShowDialog()

        If n.DialogResult = Windows.Forms.DialogResult.OK Then

            fDone = True

        End If

    End Sub


    Private Sub btnGuardar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnGuardar.Click

        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        Dim selecteddate As String = ""
        selecteddate = convertDDdashMMdashYYYYspaceHHcolonMMcolonSStoYYYYdashMMdashDDspaceHHcolonMMcolonSS(dtFechaTerminoPrevista.Value)
        selecteddate = selecteddate.Substring(0, selecteddate.Length - 9)

        If CDbl(selecteddate.Replace("/", "").Replace("-", "")) < CDbl(getMySQLDate()) Then

            Cursor.Current = System.Windows.Forms.Cursors.Default
            MsgBox("¿Puedes corregir la fecha McFly? Es anterior a hoy y eso no es posible...", MsgBoxStyle.OkOnly, "Fecha incorrecta")
            Exit Sub

        ElseIf selecteddate.Replace("/", "").Replace("-", "") = getMySQLDate() Then

            Cursor.Current = System.Windows.Forms.Cursors.Default

            If MsgBox("¿Seguro que la fecha de término es HOY?", MsgBoxStyle.YesNo, "Confirmación de Fecha Correcta") = MsgBoxResult.Yes Then

                fechaprevista = dtFechaTerminoPrevista.Value

                executeSQLCommand(0, "INSERT IGNORE INTO logs VALUES (" & getMySQLDate() & ", '" & getAppTime() & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Guardó la Fecha de Término Prevista para el proyecto " & iprojectid & " (Realizar Obra)', 'OK')")

                Me.DialogResult = Windows.Forms.DialogResult.OK
                Me.Close()

            Else

                Exit Sub

            End If

        Else

            fechaprevista = dtFechaTerminoPrevista.Value

            executeSQLCommand(0, "INSERT IGNORE INTO logs VALUES (" & getMySQLDate() & ", '" & getAppTime() & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Guardó la Fecha de Término Prevista para el proyecto " & iprojectid & " (Realizar Obra)', 'OK')")

            Me.DialogResult = Windows.Forms.DialogResult.OK
            Me.Close()

        End If

        Cursor.Current = System.Windows.Forms.Cursors.Default

    End Sub


End Class