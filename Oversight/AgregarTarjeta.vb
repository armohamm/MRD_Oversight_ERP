
Public Class AgregarTarjeta

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
    Public icardid As Integer = 0

    Public scardlegacyid As String = ""
    Public scarddescription As String = ""

    Public IsEdit As Boolean = False
    Public IsBase As Boolean = False
    Public IsModel As Boolean = False
    Public IsHistoric As Boolean = False

    Public iselectedinputid As Integer = 0
    Public sselectedinputdescription As String = ""
    Public sselectedunit As String = ""
    Public dselectedinputqty As Double = 0.0

    Public ihistoricprojectid As Integer = 0
    Public ihistoriccardid As Integer = 0

    Private WithEvents txtCantidadDgvConceptosTarjeta As TextBox
    Private WithEvents txtNombreDgvConceptosTarjeta As TextBox

    Private txtCantidadDgvConceptosTarjeta_OldText As String = ""
    Private txtNombreDgvConceptosTarjeta_OldText As String = ""

    Private isFormReadyForAction As Boolean = False
    Private isUpdatingPercentages As Boolean = False
    Private isConceptosTarjetaDGVReady As Boolean = False

    Public wasCreated As Boolean = False

    Private saveCardPermission As Boolean = False

    Private modifyInputQtyPermission As Boolean = False

    Private openInputPermission As Boolean = False
    Private newInputPermission As Boolean = False
    Private insertInputPermission As Boolean = False
    Private deleteInputPermission As Boolean = False

    Public messagesWindowIsAlreadyOpen As Boolean = False



    Private Sub checkMessages(ByVal username As String, ByVal x As Integer, ByVal y As Integer)

        Dim unreadmessagecount As Integer = 0
        unreadmessagecount = getSQLQueryAsInteger(0, "SELECT COUNT(*) FROM messages where susername = '" & username & "' AND bread = 0")

        If unreadmessagecount > 0 And messagesWindowIsAlreadyOpen = False Then

            Dim msg As New Mensajes
            Dim pt As Point

            msg.susername = username
            msg.bactive = bactive
            msg.bonline = bonline
            msg.suserfullname = suserfullname
            msg.suseremail = suseremail
            msg.susersession = susersession
            msg.susermachinename = susermachinename
            msg.suserip = suserip

            msg.StartPosition = FormStartPosition.Manual

            Dim tamañoPantalla As Integer = Screen.GetWorkingArea(Me).Height

            Dim tmpPt1 As Point = New Point(Me.Location.X, (tamañoPantalla - Me.Size.Height - msg.Size.Height) / 2) 'msg window
            Dim tmpPt2 As Point = New Point(Me.Location.X, tmpPt1.Y + msg.Size.Height) 'me

            If tmpPt1.Y > Screen.GetWorkingArea(Me).Location.Y Then

                pt = New Point(Me.Location.X, tmpPt1.Y)
                Me.Location = New Point(Me.Location.X, tmpPt2.Y)

            Else

                pt = New Point(x, y)

            End If

            msg.Location = pt
            msg.bAlreadyOpen = True

            messagesWindowIsAlreadyOpen = True

            msg.Show()

        End If

    End Sub


    Private Sub checkForKickoutsAndTimedOuts()

        Dim queryMySession As String = ""
        Dim dsMySession As DataSet

        queryMySession = "SELECT * FROM sessions s WHERE s.susername = '" & susername & "' AND s.susersession = '" & susersession & "' ORDER BY s.ilogindate DESC, s.slogintime DESC LIMIT 1 "

        dsMySession = getSQLQueryAsDataset(0, queryMySession)

        If dsMySession.Tables(0).Rows.Count > 0 Then

            If dsMySession.Tables(0).Rows(0).Item("btimedout") = "1" Then

                MsgBox("Tu sesión ha expirado. Es necesario que entres de nuevo al sistema con tu usuario y contraseña", MsgBoxStyle.Critical, "Sesión expirada")

                susername = ""
                bactive = False
                bonline = False
                suserfullname = ""
                suseremail = ""
                susersession = 0
                susermachinename = ""
                suserip = "0.0.0.0"

                Dim l As New Login

                l.isEdit = True

                l.ShowDialog(Me)

                If l.DialogResult <> Windows.Forms.DialogResult.OK Then

                    MsgBox("Cerrando Aplicación SIN Guardar...", MsgBoxStyle.Critical, "Intento Fallido")
                    System.Environment.Exit(0)

                End If

            End If

            If dsMySession.Tables(0).Rows(0).Item("bkickedout") = "1" Then

                MsgBox("Has sido sacado del sistema. Para continuar es necesario que entres de nuevo al sistema con tu usuario y contraseña", MsgBoxStyle.Critical, "Logged out")

                susername = ""
                bactive = False
                bonline = False
                suserfullname = ""
                suseremail = ""
                susersession = 0
                susermachinename = ""
                suserip = "0.0.0.0"

                Dim l As New Login

                l.isEdit = True

                l.ShowDialog(Me)

                If l.DialogResult <> Windows.Forms.DialogResult.OK Then

                    MsgBox("Cerrando Aplicación SIN Guardar...", MsgBoxStyle.Critical, "Intento Fallido")
                    System.Environment.Exit(0)

                End If

            End If

        End If

    End Sub


    Private Sub setControlsByPermissions(ByVal windowname As String, ByVal username As String)

        'Check for specific permissions on every window, but only for that unique window permissions, not the entire list!!

        Dim dsPermissions As DataSet

        Dim permission As String

        Dim viewPermission As Boolean = False

        dsPermissions = getSQLQueryAsDataset(0, "SELECT * FROM userpermissions WHERE susername = '" & username & "' AND swindowname = '" & windowname & "'")

        For j = 0 To dsPermissions.Tables(0).Rows.Count - 1

            Try

                permission = dsPermissions.Tables(0).Rows(j).Item("spermission")

                If permission = "Ver" Then
                    viewPermission = True
                End If

                If permission = "Modificar" Then
                    saveCardPermission = True
                    btnGuardar.Enabled = True
                End If

                If permission = "Abrir Categorias de Tarjeta" Then
                    btnCategorias.Enabled = True
                End If

                If permission = "Abrir Insumo" Then
                    openInputPermission = True
                End If

                If permission = "Nuevo Insumo" Then
                    newInputPermission = True
                End If

                If permission = "Insertar Insumo" Then
                    insertInputPermission = True
                End If

                If permission = "Modificar Cantidad de Insumo" Then
                    modifyInputQtyPermission = True
                End If

                If permission = "Eliminar Insumo" Then
                    deleteInputPermission = True
                End If

            Catch ex As Exception

            End Try

            permission = ""

        Next j


        If viewPermission = False Then

            Dim fecha As Integer = 0
            Dim hora As String = "00:00:00"

            fecha = getMySQLDate()
            hora = getAppTime()

            executeSQLCommand(0, "INSERT IGNORE INTO logs VALUES (" & fecha & ", '" & hora & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Acceso denegado a la ventana de Agregar/Modificar Tarjeta', 'OK')")

            Dim dsUsuariosSysAdmin As DataSet

            dsUsuariosSysAdmin = getSQLQueryAsDataset(0, "SELECT susername FROM userspecialattributes WHERE bsysadmin = 1")

            If dsUsuariosSysAdmin.Tables(0).Rows.Count > 0 Then

                For i = 0 To dsUsuariosSysAdmin.Tables(0).Rows.Count - 1
                    executeSQLCommand(0, "INSERT INTO messages (susername, susersession, smessage, bread, imessagedatetime, smessagecreatorusername, iupdatedatetime, supdateusername) VALUES ('" & dsUsuariosSysAdmin.Tables(0).Rows(i).Item(0) & "', 0, 'Un usuario ha intentado propasar sus permisos. ¿Podrías revisar?', 0, '" & convertYYYYMMDDtoYYYYhyphenMMhyphenDD(fecha) & " " & hora & "', 'SYSTEM', '" & convertYYYYMMDDtoYYYYhyphenMMhyphenDD(fecha) & " " & hora & "', 'SYSTEM')")
                Next i

            End If

            MsgBox("No tienes los permisos necesarios para abrir esta Ventana. Este intento ha sido notificado al administrador.", MsgBoxStyle.Exclamation, "Access Denied")
            Me.DialogResult = Windows.Forms.DialogResult.Cancel
            Me.Close()

        End If

    End Sub


    Private Sub AgregarTarjeta_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing

        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        If validaTarjeta(True, False) = False And Me.DialogResult <> Windows.Forms.DialogResult.OK And IsHistoric = False Then

            Cursor.Current = System.Windows.Forms.Cursors.Default

            If MsgBox("Esta Tarjeta no está completa. Si sales ahora, se perderán los cambios que hayas hecho." & Chr(13) & "¿Realmente deseas Salir de esta ventana y Cancelar esta Tarjeta?", MsgBoxStyle.YesNo, "Confirmación Eliminación de Tarjeta Parcial") = MsgBoxResult.Yes Then

                Dim timesCardIsOpen As Integer = 1

                timesCardIsOpen = getSQLQueryAsInteger(0, "SELECT COUNT(*) FROM information_schema.TABLES WHERE TABLE_NAME LIKE '%CardAux" & icardid & "'")

                If timesCardIsOpen > 1 And IsEdit = True Then

                    Cursor.Current = System.Windows.Forms.Cursors.Default

                    If MsgBox("Otro usuario tiene abierta la misma Tarjeta. Esto podría causar que alguno de ustedes perdiera los cambios que hiciera. ¿Deseas seguir guardando la Tarjeta?", MsgBoxStyle.YesNo, "Confirmación Guardado") = MsgBoxResult.No Then

                        e.Cancel = True
                        Exit Sub

                    Else

                        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

                    End If

                ElseIf timesCardIsOpen > 1 And IsEdit = False Then

                    Dim newIdAddition As Integer = 1

                    Do While getSQLQueryAsInteger(0, "SELECT COUNT(*) FROM information_schema.TABLES WHERE TABLE_NAME LIKE '%CardAux" & icardid + newIdAddition & "'") > 1 And IsEdit = False
                        newIdAddition = newIdAddition + 1
                    Loop

                    'I got the new id (previousId + newIdAddition)

                    Dim queriesNewId(4) As String

                    If IsBase = True Then

                        queriesNewId(0) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardAux" & icardid
                        queriesNewId(1) = "UPDATE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardInputs SET icardid = " & icardid + newIdAddition & " WHERE icardid = " & icardid

                    Else

                        If IsModel = True Then

                            queriesNewId(0) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardAux" & icardid
                            queriesNewId(1) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "CardAux" & icardid
                            queriesNewId(2) = "UPDATE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardInputs SET icardid = " & icardid + newIdAddition & " WHERE icardid = " & icardid
                            queriesNewId(3) = "UPDATE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "CardInputs SET icardid = " & icardid + newIdAddition & " WHERE icardid = " & icardid

                        Else

                            queriesNewId(0) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardAux" & icardid
                            queriesNewId(1) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "CardAux" & icardid
                            queriesNewId(2) = "UPDATE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardInputs SET icardid = " & icardid + newIdAddition & " WHERE icardid = " & icardid
                            queriesNewId(3) = "UPDATE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "CardInputs SET icardid = " & icardid + newIdAddition & " WHERE icardid = " & icardid

                        End If

                    End If

                    If executeTransactedSQLCommand(0, queriesNewId) = True Then
                        icardid = icardid + newIdAddition
                    End If

                End If


                Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

                Dim conteoTarjetas As Integer = 0

                Dim baseid As Integer = 0
                baseid = getSQLQueryAsInteger(0, "SELECT ibaseid FROM base ORDER BY iupdatedate DESC, supdatetime DESC LIMIT 1")

                If baseid = 0 Then
                    baseid = 99999
                End If

                If IsEdit = True Then

                    If IsBase = True Then

                        Dim queriesDelete(4) As String

                        queriesDelete(0) = "DELETE FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardCompoundInputs WHERE ibaseid = " & iprojectid & " AND icardid = " & icardid
                        queriesDelete(1) = "DELETE FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardInputs WHERE ibaseid = " & iprojectid & " AND icardid = " & icardid
                        queriesDelete(2) = "DELETE FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Cards WHERE ibaseid = " & iprojectid & " AND icardid = " & icardid

                        conteoTarjetas = getSQLQueryAsInteger(0, "SELECT COUNT(*) FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Cards WHERE ibaseid = " & iprojectid)

                        If conteoTarjetas < 1 Then

                            queriesDelete(3) = "DELETE FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Prices WHERE ibaseid = " & iprojectid

                        End If

                        executeTransactedSQLCommand(0, queriesDelete)

                    Else

                        If IsModel = True Then

                            Dim queriesDelete(4) As String

                            queriesDelete(0) = "DELETE FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "CardCompoundInputs WHERE imodelid = " & iprojectid & " AND icardid = " & icardid
                            queriesDelete(1) = "DELETE FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "CardInputs WHERE imodelid = " & iprojectid & " AND icardid = " & icardid
                            queriesDelete(2) = "DELETE FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "Cards WHERE imodelid = " & iprojectid & " AND icardid = " & icardid

                            conteoTarjetas = getSQLQueryAsInteger(0, "SELECT COUNT(*) FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "Cards WHERE imodelid = " & iprojectid)

                            If conteoTarjetas < 1 Then

                                queriesDelete(3) = "DELETE FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "Prices WHERE imodelid = " & iprojectid

                            End If

                            executeTransactedSQLCommand(0, queriesDelete)

                        Else

                            Dim queriesDelete(4) As String

                            queriesDelete(0) = "DELETE FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "CardCompoundInputs WHERE iprojectid = " & iprojectid & " AND icardid = " & icardid
                            queriesDelete(1) = "DELETE FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "CardInputs WHERE iprojectid = " & iprojectid & " AND icardid = " & icardid
                            queriesDelete(2) = "DELETE FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "Cards WHERE iprojectid = " & iprojectid & " AND icardid = " & icardid

                            conteoTarjetas = getSQLQueryAsInteger(0, "SELECT COUNT(*) FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "Cards WHERE iprojectid = " & iprojectid)

                            If conteoTarjetas < 1 Then

                                queriesDelete(3) = "DELETE FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "Prices WHERE iprojectid = " & iprojectid

                            End If

                            executeTransactedSQLCommand(0, queriesDelete)

                        End If


                    End If


                Else


                    Dim queriesDelete(8) As String

                    queriesDelete(0) = "DELETE FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardCompoundInputs WHERE ibaseid = " & baseid & " AND icardid = " & icardid
                    queriesDelete(1) = "DELETE FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardInputs WHERE ibaseid = " & baseid & " AND icardid = " & icardid
                    queriesDelete(2) = "DELETE FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Cards WHERE ibaseid = " & baseid & " AND icardid = " & icardid

                    conteoTarjetas = getSQLQueryAsInteger(0, "SELECT COUNT(*) FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Cards WHERE ibaseid = " & baseid)

                    If conteoTarjetas < 1 Then

                        queriesDelete(3) = "DELETE FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Prices WHERE ibaseid = " & baseid

                    End If

                    executeTransactedSQLCommand(0, queriesDelete)


                    If IsModel = True Then

                        queriesDelete(4) = "DELETE FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "CardCompoundInputs WHERE imodelid = " & iprojectid & " AND icardid = " & icardid
                        queriesDelete(5) = "DELETE FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "CardInputs WHERE imodelid = " & iprojectid & " AND icardid = " & icardid
                        queriesDelete(6) = "DELETE FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "Cards WHERE imodelid = " & iprojectid & " AND icardid = " & icardid

                        conteoTarjetas = getSQLQueryAsInteger(0, "SELECT COUNT(*) FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "Cards WHERE imodelid = " & iprojectid)

                        If conteoTarjetas < 1 Then

                            queriesDelete(7) = "DELETE FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "Prices WHERE imodelid = " & iprojectid

                        End If

                        executeTransactedSQLCommand(0, queriesDelete)

                    Else

                        queriesDelete(4) = "DELETE FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "CardCompoundInputs WHERE iprojectid = " & iprojectid & " AND icardid = " & icardid
                        queriesDelete(5) = "DELETE FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "CardInputs WHERE iprojectid = " & iprojectid & " AND icardid = " & icardid
                        queriesDelete(6) = "DELETE FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "Cards WHERE iprojectid = " & iprojectid & " AND icardid = " & icardid

                        conteoTarjetas = getSQLQueryAsInteger(0, "SELECT COUNT(*) FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "Cards WHERE iprojectid = " & iprojectid)

                        If conteoTarjetas < 1 Then

                            queriesDelete(7) = "DELETE FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "Prices WHERE iprojectid = " & iprojectid

                        End If

                        executeTransactedSQLCommand(0, queriesDelete)

                    End If


                End If

                executeSQLCommand(0, "INSERT IGNORE INTO logs VALUES (" & getMySQLDate() & ", '" & getAppTime() & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Guardó la Tarjeta " & icardid & " : " & txtNombreDeLaTarjeta.Text.Replace("'", "").Replace("--", "") & "', 'OK')")

            Else

                Cursor.Current = System.Windows.Forms.Cursors.Default
                e.Cancel = True
                Exit Sub

            End If

        End If

        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        Dim queriesDrop(3) As String

        If IsBase = True Then

            queriesDrop(0) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardAux" & icardid
            queriesDrop(1) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardAux0"
            queriesDrop(2) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Card0"

        Else

            If IsModel = True Then

                queriesDrop(0) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "CardAux" & icardid
                queriesDrop(1) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "CardAux0"
                queriesDrop(2) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "Card0"

            Else

                queriesDrop(0) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "CardAux" & icardid
                queriesDrop(1) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "CardAux0"
                queriesDrop(2) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "Card0"

            End If

        End If

        executeTransactedSQLCommand(0, queriesDrop)

        Dim fecha As Integer = getMySQLDate()
        Dim hora As String = getAppTime()

        executeSQLCommand(0, "INSERT IGNORE INTO logs VALUES (" & fecha & ", '" & hora & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Cerró la Tarjeta " & icardid & " : " & txtNombreDeLaTarjeta.Text.Replace("'", "").Replace("--", "") & "', 'OK')")
        'executeSQLCommand(0, "INSERT INTO recentlyopenedfiles VALUES ('" & susername & "', '" & susersession & "', 'CardAux', 'Tarjeta', '" & icardid & "', '', 0, " & fecha & ", '" & hora & "', '" & susername & "')")

        verifySuspiciousData()

        Cursor.Current = System.Windows.Forms.Cursors.Default

    End Sub


    Private Sub AgregarTarjeta_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown

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


    Private Sub AgregarTarjeta_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        Me.KeyPreview = True

        isFormReadyForAction = False

        closeTimedOutConnections()
        checkForKickoutsAndTimedOuts()
        checkMessages(susername, Me.Location.X, Me.Location.Y)
        setControlsByPermissions(Me.Name, susername)

        Dim timesCardIsOpen As Integer = 0

        timesCardIsOpen = getSQLQueryAsInteger(0, "SELECT COUNT(*) FROM information_schema.TABLES WHERE TABLE_NAME LIKE '%CardAux" & icardid & "'")

        If timesCardIsOpen > 0 And IsEdit = True Then

            Cursor.Current = System.Windows.Forms.Cursors.Default

            If MsgBox("Otro usuario tiene abierta la misma Tarjeta. Esto podría causar que alguno de ustedes perdiera los cambios que hiciera. ¿Deseas seguir abriendo la Tarjeta?", MsgBoxStyle.YesNo, "Confirmación Apertura") = MsgBoxResult.No Then

                Me.DialogResult = Windows.Forms.DialogResult.Cancel
                Me.Close()

                Cursor.Current = System.Windows.Forms.Cursors.Default
                Exit Sub

            Else

                Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

            End If

        End If

        cmbCategoriaDeLaTarjeta.DataSource = getSQLQueryAsDataTable(0, "SELECT scardlegacycategoryid, CONCAT(scardlegacycategoryid, ' - ', scardlegacycategorydescription) AS scardlegacycategorydescription FROM cardlegacycategories")
        cmbCategoriaDeLaTarjeta.DisplayMember = "scardlegacycategorydescription"
        cmbCategoriaDeLaTarjeta.ValueMember = "scardlegacycategoryid"
        cmbCategoriaDeLaTarjeta.SelectedIndex = -1

        updateEverything()

        If IsEdit = True Then

            If IsHistoric = True Then

                cmbCategoriaDeLaTarjeta.Enabled = False
                txtCodigoDeLaTarjeta.Enabled = False
                txtUnidadDeMedida.Enabled = False
                txtUltimaModificacion.Enabled = False
                txtNombreDeLaTarjeta.Enabled = False
                dgvConceptosTarjeta.ReadOnly = True
                txtCostoDirecto.Enabled = False
                txtPorcentajeIndirectos.Enabled = False
                txtIndirectos.Enabled = False
                txtSuma.Enabled = False
                txtPorcentajeUtilidades.Enabled = False
                txtUtilidades.Enabled = False
                txtSubTotal.Enabled = False
                txtIVA.Enabled = False
                txtTotal.Enabled = False

            Else

                dgvConceptosTarjeta.Enabled = True
                btnNuevoInsumo.Enabled = True
                btnInsertarInsumo.Enabled = True
                btnEliminarInsumo.Enabled = True

            End If

        Else

            Dim queryIVATarjeta As String = ""
            Dim dsDatos As DataSet
            Dim iva As Double = 0.0
            Dim porcentajeIndirectos As Double = 0.0
            Dim porcentajeUtilidad As Double = 0.0

            If IsBase = True Then
                queryIVATarjeta = "SELECT dbaseindirectpercentagedefault, dbasegainpercentagedefault, dbaseIVApercentage FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & " WHERE ibaseid = " & iprojectid
            Else
                If IsModel = True Then
                    queryIVATarjeta = "SELECT dmodelindirectpercentagedefault, dmodelgainpercentagedefault, dmodelIVApercentage FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & " WHERE imodelid = " & iprojectid
                Else
                    queryIVATarjeta = "SELECT dprojectindirectpercentagedefault, dprojectgainpercentagedefault, dprojectIVApercentage FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & " WHERE iprojectid = " & iprojectid
                End If
            End If

            dsDatos = getSQLQueryAsDataset(0, queryIVATarjeta)

            Try

                If IsBase = True Then

                    iva = dsDatos.Tables(0).Rows(0).Item("dbaseIVApercentage")
                    porcentajeIndirectos = dsDatos.Tables(0).Rows(0).Item("dbaseindirectpercentagedefault")
                    porcentajeUtilidad = dsDatos.Tables(0).Rows(0).Item("dbasegainpercentagedefault")

                Else

                    If IsModel = True Then

                        iva = dsDatos.Tables(0).Rows(0).Item("dmodelIVApercentage")
                        porcentajeIndirectos = dsDatos.Tables(0).Rows(0).Item("dmodelindirectpercentagedefault")
                        porcentajeUtilidad = dsDatos.Tables(0).Rows(0).Item("dmodelgainpercentagedefault")

                    Else

                        iva = dsDatos.Tables(0).Rows(0).Item("dprojectIVApercentage")
                        porcentajeIndirectos = dsDatos.Tables(0).Rows(0).Item("dprojectindirectpercentagedefault")
                        porcentajeUtilidad = dsDatos.Tables(0).Rows(0).Item("dprojectgainpercentagedefault")

                    End If

                End If

            Catch ex As Exception

                iva = 0.0
                porcentajeIndirectos = 0.0
                porcentajeUtilidad = 0.0

            End Try

            lblPorcentajeIVA.Text = FormatCurrency(iva * 100, 2, TriState.True, TriState.False, TriState.True).Replace("$", "") & " %"
            txtPorcentajeIndirectos.Text = FormatCurrency(porcentajeIndirectos * 100, 2, TriState.True, TriState.False, TriState.True).Replace("$", "")
            txtPorcentajeUtilidades.Text = FormatCurrency(porcentajeUtilidad * 100, 2, TriState.True, TriState.False, TriState.True).Replace("$", "")

        End If

        isConceptosTarjetaDGVReady = True

        isFormReadyForAction = True

        If IsEdit = True Then
            updateTotals()
        End If

        Dim fecha As Integer = getMySQLDate()
        Dim hora As String = getAppTime()

        executeSQLCommand(0, "INSERT IGNORE INTO logs VALUES (" & getMySQLDate() & ", '" & getAppTime() & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Abrió la Tarjeta " & icardid & " : " & txtNombreDeLaTarjeta.Text.Replace("'", "").Replace("--", "") & "', 'OK')")
        'executeSQLCommand(0, "INSERT INTO recentlyopenedfiles VALUES ('" & susername & "', '" & susersession & "', 'CardAux', 'Tarjeta', '" & icardid & "', '', 1, " & fecha & ", '" & hora & "', '" & susername & "')")

        cmbCategoriaDeLaTarjeta.Select()
        cmbCategoriaDeLaTarjeta.Focus()

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


    Private Sub updateEverything()

        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        Dim queryInsumosTarjetas As String = ""

        Dim queriesCreation(2) As String


        If IsBase = True Then

            queriesCreation(0) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardAux" & icardid
            queriesCreation(1) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardAux" & icardid & " (   iinputid varchar(11) COLLATE latin1_spanish_ci NOT NULL,   sinputdescription varchar(300) CHARACTER SET latin1 NOT NULL,   scardinputunit varchar(100) CHARACTER SET latin1 NOT NULL,   unitprice varchar(50) CHARACTER SET latin1 NOT NULL,   dcardinputqty varchar(50) COLLATE latin1_spanish_ci NOT NULL,   amount varchar(50) COLLATE latin1_spanish_ci NOT NULL ) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"

        Else

            If IsModel = True Then

                queriesCreation(0) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "CardAux" & icardid
                queriesCreation(1) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "CardAux" & icardid & " (   iinputid varchar(11) COLLATE latin1_spanish_ci NOT NULL,   sinputdescription varchar(300) CHARACTER SET latin1 NOT NULL,   scardinputunit varchar(100) CHARACTER SET latin1 NOT NULL,   unitprice varchar(50) CHARACTER SET latin1 NOT NULL,   dcardinputqty varchar(50) COLLATE latin1_spanish_ci NOT NULL,   amount varchar(50) COLLATE latin1_spanish_ci NOT NULL ) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"

            Else

                queriesCreation(0) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "CardAux" & icardid
                queriesCreation(1) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "CardAux" & icardid & " (   iinputid varchar(11) COLLATE latin1_spanish_ci NOT NULL,   sinputdescription varchar(300) CHARACTER SET latin1 NOT NULL,   scardinputunit varchar(100) CHARACTER SET latin1 NOT NULL,   unitprice varchar(50) CHARACTER SET latin1 NOT NULL,   dcardinputqty varchar(50) COLLATE latin1_spanish_ci NOT NULL,   amount varchar(50) COLLATE latin1_spanish_ci NOT NULL ) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"

            End If

        End If

        executeTransactedSQLCommand(0, queriesCreation)

        If IsBase = True Then

            queryInsumosTarjetas = ""

            If getSQLQueryAsBoolean(0, "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardInputs WHERE ibaseid = " & iprojectid & " AND icardid = " & icardid) = True Then

                'Sí hay Insumos para mostrar

                queryInsumosTarjetas = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardAux" & icardid & " "

                If getSQLQueryAsBoolean(0, "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardInputs btfi JOIN inputtypes it ON btfi.iinputid = it.iinputid WHERE ibaseid = " & iprojectid & " AND icardid = " & icardid & " AND it.sinputtypedescription = 'MATERIALES'") = True Then

                    'Materiales

                    queryInsumosTarjetas &= "SELECT '' AS iinputid, 'MATERIALES' AS sinputdescription, '' AS scardinputunit, '' AS unitprice, " & _
                    "'' AS dcardinputqty, '' AS amount " & _
                    "UNION " & _
                    "(SELECT btfi.iinputid, i.sinputdescription, btfi.scardinputunit, IF(bp.dinputfinalprice IS NULL, cibp.dinputfinalprice, bp.dinputfinalprice) AS unitprice, " & _
                    "btfi.dcardinputqty, IF(SUM(btfi.dcardinputqty*bp.dinputfinalprice) IS NULL, SUM(btfi.dcardinputqty*cibp.dinputfinalprice), SUM(btfi.dcardinputqty*bp.dinputfinalprice)) AS amount " & _
                    "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardInputs btfi " & _
                    "JOIN inputs i ON btfi.iinputid = i.iinputid " & _
                    "JOIN inputtypes it ON i.iinputid = it.iinputid " & _
                    "LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) bp GROUP BY iinputid, ibaseid) bp ON btfi.ibaseid = bp.ibaseid AND i.iinputid = bp.iinputid " & _
                    "LEFT JOIN (SELECT btfci.ibaseid, btfci.iinputid, cibp.iupdatedate, cibp.supdatetime, SUM(btfci.dcompoundinputqty*cibp.dinputfinalprice) AS dinputpricewithoutIVA, 0.00000 AS dinputprotectionpercentage, SUM(btfci.dcompoundinputqty*cibp.dinputfinalprice) AS dinputfinalprice FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardCompoundInputs btfci JOIN inputs i" & icardid & " ON i" & icardid & ".iinputid = btfci.icompoundinputid LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) cibp GROUP BY iinputid, ibaseid) cibp ON cibp.ibaseid = btfci.ibaseid AND cibp.iinputid = btfci.icompoundinputid WHERE btfci.ibaseid = " & iprojectid & " AND btfci.icardid = " & icardid & " GROUP BY btfci.ibaseid, btfci.icardid, btfci.iinputid) cibp ON btfi.ibaseid = cibp.ibaseid AND i.iinputid = cibp.iinputid " & _
                    "WHERE btfi.ibaseid = " & iprojectid & " AND btfi.icardid = " & icardid & " AND it.sinputtypedescription = 'MATERIALES' " & _
                    "GROUP BY btfi.ibaseid, btfi.iinputid " & _
                    "ORDER BY 2, 3, 4) " & _
                    "UNION " & _
                    "SELECT '', '', '', '', 'SUMA DE MATERIALES', IF(SUM(btfi.dcardinputqty*bp.dinputfinalprice)IS NULL, 0, SUM(btfi.dcardinputqty*bp.dinputfinalprice))+IF(SUM(btfi.dcardinputqty*cibp.dinputfinalprice) IS NULL, 0, SUM(btfi.dcardinputqty*cibp.dinputfinalprice)) AS amount " & _
                    "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardInputs btfi " & _
                    "JOIN inputs i ON btfi.iinputid = i.iinputid " & _
                    "JOIN inputtypes it ON i.iinputid = it.iinputid " & _
                    "LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) bp GROUP BY iinputid, ibaseid) bp ON btfi.ibaseid = bp.ibaseid AND i.iinputid = bp.iinputid " & _
                    "LEFT JOIN (SELECT btfci.ibaseid, btfci.iinputid, cibp.iupdatedate, cibp.supdatetime, SUM(btfci.dcompoundinputqty*cibp.dinputfinalprice) AS dinputpricewithoutIVA, 0.00000 AS dinputprotectionpercentage, SUM(btfci.dcompoundinputqty*cibp.dinputfinalprice) AS dinputfinalprice FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardCompoundInputs btfci JOIN inputs i" & icardid & " ON i" & icardid & ".iinputid = btfci.icompoundinputid LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) cibp GROUP BY iinputid, ibaseid) cibp ON cibp.ibaseid = btfci.ibaseid AND cibp.iinputid = btfci.icompoundinputid WHERE btfci.ibaseid = " & iprojectid & " AND btfci.icardid = " & icardid & " GROUP BY btfci.ibaseid, btfci.icardid, btfci.iinputid) cibp ON btfi.ibaseid = cibp.ibaseid AND i.iinputid = cibp.iinputid " & _
                    "WHERE btfi.ibaseid = " & iprojectid & " AND btfi.icardid = " & icardid & " AND it.sinputtypedescription = 'MATERIALES' "

                    If getSQLQueryAsBoolean(0, "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardInputs btfi JOIN inputtypes it ON btfi.iinputid = it.iinputid WHERE ibaseid = " & iprojectid & " AND icardid = " & icardid & " AND it.sinputtypedescription = 'MANO DE OBRA'") = True Then

                        queryInsumosTarjetas &= "UNION "

                    End If

                End If

                If getSQLQueryAsBoolean(0, "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardInputs btfi JOIN inputtypes it ON btfi.iinputid = it.iinputid WHERE ibaseid = " & iprojectid & " AND icardid = " & icardid & " AND it.sinputtypedescription = 'MANO DE OBRA'") = True Then

                    'Mano de Obra

                    queryInsumosTarjetas &= "SELECT '' AS iinputid, 'MANO DE OBRA' AS sinputdescription, '' AS scardinputunit, '' AS unitprice, " & _
                    "'' AS dcardinputqty, '' AS amount " & _
                    "UNION " & _
                    "(SELECT btfi.iinputid, i.sinputdescription, btfi.scardinputunit, bp.dinputfinalprice AS unitprice, " & _
                    "btfi.dcardinputqty, IF(SUM(btfi.dcardinputqty*bp.dinputfinalprice) IS NULL, 0, SUM(btfi.dcardinputqty*bp.dinputfinalprice)) AS amount " & _
                    "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardInputs btfi " & _
                    "JOIN inputs i ON btfi.iinputid = i.iinputid " & _
                    "JOIN inputtypes it ON i.iinputid = it.iinputid " & _
                    "LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) bp GROUP BY iinputid, ibaseid) bp ON btfi.ibaseid = bp.ibaseid AND i.iinputid = bp.iinputid " & _
                    "WHERE btfi.ibaseid = " & iprojectid & " AND btfi.icardid = " & icardid & " AND it.sinputtypedescription = 'MANO DE OBRA' " & _
                    "GROUP BY btfi.ibaseid, btfi.iinputid " & _
                    "ORDER BY 2, 3, 4) " & _
                    "UNION " & _
                    "SELECT '', '', '', '', 'SUMA DE MANO DE OBRA', IF(SUM(btfi.dcardinputqty*bp.dinputfinalprice) IS NULL, 0, SUM(btfi.dcardinputqty*bp.dinputfinalprice)) AS amount " & _
                    "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardInputs btfi " & _
                    "JOIN inputs i ON btfi.iinputid = i.iinputid " & _
                    "JOIN inputtypes it ON i.iinputid = it.iinputid " & _
                    "LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) bp GROUP BY iinputid, ibaseid) bp ON btfi.ibaseid = bp.ibaseid AND i.iinputid = bp.iinputid " & _
                    "WHERE btfi.ibaseid = " & iprojectid & " AND btfi.icardid = " & icardid & " AND it.sinputtypedescription = 'MANO DE OBRA' "

                    If getSQLQueryAsBoolean(0, "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardInputs btfi JOIN inputtypes it ON btfi.iinputid = it.iinputid WHERE ibaseid = " & iprojectid & " AND icardid = " & icardid & " AND it.sinputtypedescription = 'EQUIPO Y HERRAMIENTA'") = True Then

                        queryInsumosTarjetas &= "UNION "

                    End If

                End If

                If getSQLQueryAsBoolean(0, "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardInputs btfi JOIN inputtypes it ON btfi.iinputid = it.iinputid WHERE ibaseid = " & iprojectid & " AND icardid = " & icardid & " AND it.sinputtypedescription = 'EQUIPO Y HERRAMIENTA'") = True Then

                    'Equipo y Herramienta

                    queryInsumosTarjetas &= "SELECT '' AS iinputid, 'EQUIPO Y HERRAMIENTA' AS sinputdescription, '' AS scardinputunit, '' AS unitprice, " & _
                    "'' AS dcardinputqty, '' AS amount " & _
                    "UNION " & _
                    "(SELECT btfi.iinputid, 'EQUIPO Y HERRAMIENTA', CONCAT(btfi.scardinputunit, ' M.O.') AS scardinputunit, " & _
                    "IF(eq.amount IS NULL, 0, eq.amount) AS unitprice, btfi.dcardinputqty, IF(SUM(btfi.dcardinputqty*eq.amount) IS NULL, 0, SUM(btfi.dcardinputqty*eq.amount)) AS amount " & _
                    "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardInputs btfi " & _
                    "LEFT JOIN (SELECT btfi.ibaseid, 0 AS iinputid, SUM(btfi.dcardinputqty*bp.dinputfinalprice) AS amount FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardInputs btfi JOIN inputs i ON btfi.iinputid = i.iinputid JOIN inputtypes it ON i.iinputid = it.iinputid LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) bp GROUP BY iinputid, ibaseid) bp ON btfi.ibaseid = bp.ibaseid AND i.iinputid = bp.iinputid WHERE btfi.ibaseid = " & iprojectid & " AND btfi.icardid = " & icardid & " AND it.sinputtypedescription = 'MANO DE OBRA' GROUP BY btfi.ibaseid, btfi.iinputid) eq ON btfi.ibaseid = eq.ibaseid AND btfi.iinputid = eq.iinputid " & _
                    "WHERE btfi.ibaseid = " & iprojectid & " AND btfi.icardid = " & icardid & " and btfi.iinputid = 0 " & _
                    "GROUP BY btfi.ibaseid, btfi.iinputid " & _
                    "ORDER BY 2, 3, 4) " & _
                    "UNION " & _
                    "SELECT '', '', '', '', 'SUMA DE EQUIPO Y HERRAMIENTA', IF(SUM(btfi.dcardinputqty*eq.amount) IS NULL, 0, SUM(btfi.dcardinputqty*eq.amount)) AS amount " & _
                    "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardInputs btfi " & _
                    "JOIN (SELECT btfi.ibaseid, 0 AS iinputid, SUM(btfi.dcardinputqty*bp.dinputfinalprice) AS amount FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardInputs btfi JOIN inputs i ON btfi.iinputid = i.iinputid JOIN inputtypes it ON i.iinputid = it.iinputid LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) bp GROUP BY iinputid, ibaseid) bp ON btfi.ibaseid = bp.ibaseid AND i.iinputid = bp.iinputid WHERE btfi.ibaseid = " & iprojectid & " AND btfi.icardid = " & icardid & " AND it.sinputtypedescription = 'MANO DE OBRA' GROUP BY btfi.ibaseid, btfi.iinputid) eq ON btfi.ibaseid = eq.ibaseid AND btfi.iinputid = eq.iinputid " & _
                    "WHERE btfi.ibaseid = " & iprojectid & " AND btfi.icardid = " & icardid & " "

                End If

            Else

                'Regresamos En Blanco el Grid

                queryInsumosTarjetas &= "SELECT btfi.iinputid, i.sinputdescription, btfi.scardinputunit, IF(bp.dinputfinalprice IS NULL, cibp.dinputfinalprice, bp.dinputfinalprice) AS unitprice, " & _
                "btfi.dcardinputqty, IF(SUM(btfi.dcardinputqty*bp.dinputfinalprice) IS NULL, SUM(btfi.dcardinputqty*cibp.dinputfinalprice), SUM(btfi.dcardinputqty*bp.dinputfinalprice)) AS amount " & _
                "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardInputs btfi " & _
                "JOIN inputs i ON btfi.iinputid = i.iinputid " & _
                "JOIN inputtypes it ON i.iinputid = it.iinputid " & _
                "LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) bp GROUP BY iinputid, ibaseid) bp ON btfi.ibaseid = bp.ibaseid AND i.iinputid = bp.iinputid " & _
                "LEFT JOIN (SELECT btfci.ibaseid, btfci.iinputid, cibp.iupdatedate, cibp.supdatetime, SUM(btfci.dcompoundinputqty*cibp.dinputfinalprice) AS dinputpricewithoutIVA, 0.00000 AS dinputprotectionpercentage, SUM(btfci.dcompoundinputqty*cibp.dinputfinalprice) AS dinputfinalprice FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardCompoundInputs btfci JOIN inputs i" & icardid & " ON i" & icardid & ".iinputid = btfci.icompoundinputid LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) cibp GROUP BY iinputid, ibaseid) cibp ON cibp.ibaseid = btfci.ibaseid AND cibp.iinputid = btfci.icompoundinputid WHERE btfci.ibaseid = " & iprojectid & " AND btfci.icardid = " & icardid & " GROUP BY btfci.ibaseid, btfci.icardid, btfci.iinputid) cibp ON btfi.ibaseid = cibp.ibaseid AND i.iinputid = cibp.iinputid " & _
                "WHERE btfi.ibaseid = " & iprojectid & " AND btfi.icardid = " & icardid & " AND it.sinputtypedescription = 'MATERIALES' " & _
                "GROUP BY btfi.ibaseid, btfi.iinputid "

            End If






        Else

            If IsModel = True Then

                queryInsumosTarjetas = ""

                If getSQLQueryAsBoolean(0, "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "CardInputs WHERE imodelid = " & iprojectid & " AND icardid = " & icardid) = True Then

                    'Sí hay Insumos para mostrar

                    queryInsumosTarjetas = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "CardAux" & icardid & " "

                    If getSQLQueryAsBoolean(0, "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "CardInputs mtfi JOIN inputtypes it ON mtfi.iinputid = it.iinputid WHERE imodelid = " & iprojectid & " AND icardid = " & icardid & " AND it.sinputtypedescription = 'MATERIALES'") = True Then

                        'Materiales

                        queryInsumosTarjetas &= "SELECT '' AS iinputid, 'MATERIALES' AS sinputdescription, '' AS scardinputunit, '' AS unitprice, " & _
                        "'' AS dcardinputqty, '' AS amount " & _
                        "UNION " & _
                        "(SELECT mtfi.iinputid, i.sinputdescription, mtfi.scardinputunit, IF(bp.dinputfinalprice IS NULL, cibp.dinputfinalprice, bp.dinputfinalprice) AS unitprice, " & _
                        "mtfi.dcardinputqty, IF(SUM(mtfi.dcardinputqty*bp.dinputfinalprice) IS NULL, SUM(mtfi.dcardinputqty*cibp.dinputfinalprice), SUM(mtfi.dcardinputqty*bp.dinputfinalprice)) AS amount " & _
                        "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "CardInputs mtfi " & _
                        "JOIN inputs i ON mtfi.iinputid = i.iinputid " & _
                        "JOIN inputtypes it ON i.iinputid = it.iinputid " & _
                        "LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) bp GROUP BY iinputid, imodelid) bp ON mtfi.imodelid = bp.imodelid AND i.iinputid = bp.iinputid " & _
                        "LEFT JOIN (SELECT mtfci.imodelid, mtfci.iinputid, cibp.iupdatedate, cibp.supdatetime, SUM(mtfci.dcompoundinputqty*cibp.dinputfinalprice) AS dinputpricewithoutIVA, 0.00000 AS dinputprotectionpercentage, SUM(mtfci.dcompoundinputqty*cibp.dinputfinalprice) AS dinputfinalprice FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "CardCompoundInputs mtfci JOIN inputs i" & icardid & " ON i" & icardid & ".iinputid = mtfci.icompoundinputid LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) cibp GROUP BY iinputid, imodelid) cibp ON cibp.imodelid = mtfci.imodelid AND cibp.iinputid = mtfci.icompoundinputid WHERE mtfci.imodelid = " & iprojectid & " AND mtfci.icardid = " & icardid & " GROUP BY mtfci.imodelid, mtfci.icardid, mtfci.iinputid) cibp ON mtfi.imodelid = cibp.imodelid AND i.iinputid = cibp.iinputid " & _
                        "WHERE mtfi.imodelid = " & iprojectid & " AND mtfi.icardid = " & icardid & " AND it.sinputtypedescription = 'MATERIALES' " & _
                        "GROUP BY mtfi.imodelid, mtfi.iinputid " & _
                        "ORDER BY 2, 3, 4) " & _
                        "UNION " & _
                        "SELECT '', '', '', '', 'SUMA DE MATERIALES', IF(SUM(mtfi.dcardinputqty*bp.dinputfinalprice)IS NULL, 0, SUM(mtfi.dcardinputqty*bp.dinputfinalprice))+IF(SUM(mtfi.dcardinputqty*cibp.dinputfinalprice) IS NULL, 0, SUM(mtfi.dcardinputqty*cibp.dinputfinalprice)) AS amount " & _
                        "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "CardInputs mtfi " & _
                        "JOIN inputs i ON mtfi.iinputid = i.iinputid " & _
                        "JOIN inputtypes it ON i.iinputid = it.iinputid " & _
                        "LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) bp GROUP BY iinputid, imodelid) bp ON mtfi.imodelid = bp.imodelid AND i.iinputid = bp.iinputid " & _
                        "LEFT JOIN (SELECT mtfci.imodelid, mtfci.iinputid, cibp.iupdatedate, cibp.supdatetime, SUM(mtfci.dcompoundinputqty*cibp.dinputfinalprice) AS dinputpricewithoutIVA, 0.00000 AS dinputprotectionpercentage, SUM(mtfci.dcompoundinputqty*cibp.dinputfinalprice) AS dinputfinalprice FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "CardCompoundInputs mtfci JOIN inputs i" & icardid & " ON i" & icardid & ".iinputid = mtfci.icompoundinputid LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) cibp GROUP BY iinputid, imodelid) cibp ON cibp.imodelid = mtfci.imodelid AND cibp.iinputid = mtfci.icompoundinputid WHERE mtfci.imodelid = " & iprojectid & " AND mtfci.icardid = " & icardid & " GROUP BY mtfci.imodelid, mtfci.icardid, mtfci.iinputid) cibp ON mtfi.imodelid = cibp.imodelid AND i.iinputid = cibp.iinputid " & _
                        "WHERE mtfi.imodelid = " & iprojectid & " AND mtfi.icardid = " & icardid & " AND it.sinputtypedescription = 'MATERIALES' "

                        If getSQLQueryAsBoolean(0, "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "CardInputs mtfi JOIN inputtypes it ON mtfi.iinputid = it.iinputid WHERE imodelid = " & iprojectid & " AND icardid = " & icardid & " AND it.sinputtypedescription = 'MANO DE OBRA'") = True Then

                            queryInsumosTarjetas &= "UNION "

                        End If

                    End If

                    If getSQLQueryAsBoolean(0, "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "CardInputs mtfi JOIN inputtypes it ON mtfi.iinputid = it.iinputid WHERE imodelid = " & iprojectid & " AND icardid = " & icardid & " AND it.sinputtypedescription = 'MANO DE OBRA'") = True Then

                        'Mano de Obra

                        queryInsumosTarjetas &= "SELECT '' AS iinputid, 'MANO DE OBRA' AS sinputdescription, '' AS scardinputunit, '' AS unitprice, " & _
                        "'' AS dcardinputqty, '' AS amount " & _
                        "UNION " & _
                        "(SELECT mtfi.iinputid, i.sinputdescription, mtfi.scardinputunit, bp.dinputfinalprice AS unitprice, " & _
                        "mtfi.dcardinputqty, IF(SUM(mtfi.dcardinputqty*bp.dinputfinalprice) IS NULL, 0, SUM(mtfi.dcardinputqty*bp.dinputfinalprice)) AS amount " & _
                        "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "CardInputs mtfi " & _
                        "JOIN inputs i ON mtfi.iinputid = i.iinputid " & _
                        "JOIN inputtypes it ON i.iinputid = it.iinputid " & _
                        "LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) bp GROUP BY iinputid, imodelid) bp ON mtfi.imodelid = bp.imodelid AND i.iinputid = bp.iinputid " & _
                        "WHERE mtfi.imodelid = " & iprojectid & " AND mtfi.icardid = " & icardid & " AND it.sinputtypedescription = 'MANO DE OBRA' " & _
                        "GROUP BY mtfi.imodelid, mtfi.iinputid " & _
                        "ORDER BY 2, 3, 4) " & _
                        "UNION " & _
                        "SELECT '', '', '', '', 'SUMA DE MANO DE OBRA', IF(SUM(mtfi.dcardinputqty*bp.dinputfinalprice) IS NULL, 0, SUM(mtfi.dcardinputqty*bp.dinputfinalprice)) AS amount " & _
                        "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "CardInputs mtfi " & _
                        "JOIN inputs i ON mtfi.iinputid = i.iinputid " & _
                        "JOIN inputtypes it ON i.iinputid = it.iinputid " & _
                        "LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) bp GROUP BY iinputid, imodelid) bp ON mtfi.imodelid = bp.imodelid AND i.iinputid = bp.iinputid " & _
                        "WHERE mtfi.imodelid = " & iprojectid & " AND mtfi.icardid = " & icardid & " AND it.sinputtypedescription = 'MANO DE OBRA' "

                        If getSQLQueryAsBoolean(0, "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "CardInputs mtfi JOIN inputtypes it ON mtfi.iinputid = it.iinputid WHERE imodelid = " & iprojectid & " AND icardid = " & icardid & " AND it.sinputtypedescription = 'EQUIPO Y HERRAMIENTA'") = True Then

                            queryInsumosTarjetas &= "UNION "

                        End If

                    End If


                    If getSQLQueryAsBoolean(0, "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "CardInputs mtfi JOIN inputtypes it ON mtfi.iinputid = it.iinputid WHERE imodelid = " & iprojectid & " AND icardid = " & icardid & " AND it.sinputtypedescription = 'EQUIPO Y HERRAMIENTA'") = True Then

                        'Equipo y Herramienta

                        queryInsumosTarjetas &= "SELECT '' AS iinputid, 'EQUIPO Y HERRAMIENTA' AS sinputdescription, '' AS scardinputunit, '' AS unitprice, " & _
                        "'' AS dcardinputqty, '' AS amount " & _
                        "UNION " & _
                        "(SELECT mtfi.iinputid, 'EQUIPO Y HERRAMIENTA', CONCAT(mtfi.scardinputunit, ' M.O.') AS scardinputunit, " & _
                        "IF(eq.amount IS NULL, 0, eq.amount) AS unitprice, mtfi.dcardinputqty, IF(SUM(mtfi.dcardinputqty*eq.amount) IS NULL, 0, SUM(mtfi.dcardinputqty*eq.amount)) AS amount " & _
                        "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "CardInputs mtfi " & _
                        "LEFT JOIN (SELECT mtfi.imodelid, 0 AS iinputid, SUM(mtfi.dcardinputqty*bp.dinputfinalprice) AS amount FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "CardInputs mtfi JOIN inputs i ON mtfi.iinputid = i.iinputid JOIN inputtypes it ON i.iinputid = it.iinputid LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) bp GROUP BY iinputid, imodelid) bp ON mtfi.imodelid = bp.imodelid AND i.iinputid = bp.iinputid WHERE mtfi.imodelid = " & iprojectid & " AND mtfi.icardid = " & icardid & " AND it.sinputtypedescription = 'MANO DE OBRA' GROUP BY mtfi.imodelid, mtfi.iinputid) eq ON mtfi.imodelid = eq.imodelid AND mtfi.iinputid = eq.iinputid " & _
                        "WHERE mtfi.imodelid = " & iprojectid & " AND mtfi.icardid = " & icardid & " and mtfi.iinputid = 0 " & _
                        "GROUP BY mtfi.imodelid, mtfi.iinputid " & _
                        "ORDER BY 2, 3, 4) " & _
                        "UNION " & _
                        "SELECT '', '', '', '', 'SUMA DE EQUIPO Y HERRAMIENTA', IF(SUM(mtfi.dcardinputqty*eq.amount) IS NULL, 0, SUM(mtfi.dcardinputqty*eq.amount)) AS amount " & _
                        "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "CardInputs mtfi " & _
                        "JOIN (SELECT mtfi.imodelid, 0 AS iinputid, SUM(mtfi.dcardinputqty*bp.dinputfinalprice) AS amount FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "CardInputs mtfi JOIN inputs i ON mtfi.iinputid = i.iinputid JOIN inputtypes it ON i.iinputid = it.iinputid LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) bp GROUP BY iinputid, imodelid) bp ON mtfi.imodelid = bp.imodelid AND i.iinputid = bp.iinputid WHERE mtfi.imodelid = " & iprojectid & " AND mtfi.icardid = " & icardid & " AND it.sinputtypedescription = 'MANO DE OBRA' GROUP BY mtfi.imodelid, mtfi.iinputid) eq ON mtfi.imodelid = eq.imodelid AND mtfi.iinputid = eq.iinputid " & _
                        "WHERE mtfi.imodelid = " & iprojectid & " AND mtfi.icardid = " & icardid & " "

                    End If


                Else


                    'Regresamos En Blanco el Grid
                    queryInsumosTarjetas &= "SELECT mtfi.iinputid, i.sinputdescription, mtfi.scardinputunit, IF(bp.dinputfinalprice IS NULL, cibp.dinputfinalprice, bp.dinputfinalprice) AS unitprice, " & _
                    "mtfi.dcardinputqty, IF(SUM(mtfi.dcardinputqty*bp.dinputfinalprice) IS NULL, SUM(mtfi.dcardinputqty*cibp.dinputfinalprice), SUM(mtfi.dcardinputqty*bp.dinputfinalprice)) AS amount " & _
                    "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "CardInputs mtfi " & _
                    "JOIN inputs i ON mtfi.iinputid = i.iinputid " & _
                    "JOIN inputtypes it ON i.iinputid = it.iinputid " & _
                    "LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) bp GROUP BY iinputid, imodelid) bp ON mtfi.imodelid = bp.imodelid AND i.iinputid = bp.iinputid " & _
                    "LEFT JOIN (SELECT mtfci.imodelid, mtfci.iinputid, cibp.iupdatedate, cibp.supdatetime, SUM(mtfci.dcompoundinputqty*cibp.dinputfinalprice) AS dinputpricewithoutIVA, 0.00000 AS dinputprotectionpercentage, SUM(mtfci.dcompoundinputqty*cibp.dinputfinalprice) AS dinputfinalprice FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "CardCompoundInputs mtfci JOIN inputs i" & icardid & " ON i" & icardid & ".iinputid = mtfci.icompoundinputid LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) cibp GROUP BY iinputid, imodelid) cibp ON cibp.imodelid = mtfci.imodelid AND cibp.iinputid = mtfci.icompoundinputid WHERE mtfci.imodelid = " & iprojectid & " AND mtfci.icardid = " & icardid & " GROUP BY mtfci.imodelid, mtfci.icardid, mtfci.iinputid) cibp ON mtfi.imodelid = cibp.imodelid AND i.iinputid = cibp.iinputid " & _
                    "WHERE mtfi.imodelid = " & iprojectid & " AND mtfi.icardid = " & icardid & " AND it.sinputtypedescription = 'MATERIALES' " & _
                    "GROUP BY mtfi.imodelid, mtfi.iinputid "


                End If

            Else

                queryInsumosTarjetas = ""

                If getSQLQueryAsBoolean(0, "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "CardInputs WHERE iprojectid = " & iprojectid & " AND icardid = " & icardid) = True Then

                    'Sí hay Insumos para mostrar

                    queryInsumosTarjetas &= "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "CardAux" & icardid & " "

                    If getSQLQueryAsBoolean(0, "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "CardInputs ptfi JOIN inputtypes it ON ptfi.iinputid = it.iinputid WHERE iprojectid = " & iprojectid & " AND icardid = " & icardid & " AND it.sinputtypedescription = 'MATERIALES'") = True Then

                        'Materiales

                        queryInsumosTarjetas &= "SELECT '' AS iinputid, 'MATERIALES' AS sinputdescription, '' AS scardinputunit, '' AS unitprice, " & _
                        "'' AS dcardinputqty, '' AS amount " & _
                        "UNION " & _
                        "(SELECT ptfi.iinputid, i.sinputdescription, ptfi.scardinputunit, IF(pp.dinputfinalprice IS NULL, cipp.dinputfinalprice, pp.dinputfinalprice) AS unitprice, " & _
                        "ptfi.dcardinputqty, IF(SUM(ptfi.dcardinputqty*pp.dinputfinalprice) IS NULL, SUM(ptfi.dcardinputqty*cipp.dinputfinalprice), SUM(ptfi.dcardinputqty*pp.dinputfinalprice)) AS amount " & _
                        "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "CardInputs ptfi " & _
                        "JOIN inputs i ON ptfi.iinputid = i.iinputid " & _
                        "JOIN inputtypes it ON i.iinputid = it.iinputid " & _
                        "LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) pp GROUP BY iinputid, iprojectid) pp ON ptfi.iprojectid = pp.iprojectid AND i.iinputid = pp.iinputid " & _
                        "LEFT JOIN (SELECT ptfci.iprojectid, ptfci.iinputid, cipp.iupdatedate, cipp.supdatetime, SUM(ptfci.dcompoundinputqty*cipp.dinputfinalprice) AS dinputpricewithoutIVA, 0.00000 AS dinputprotectionpercentage, SUM(ptfci.dcompoundinputqty*cipp.dinputfinalprice) AS dinputfinalprice FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "CardCompoundInputs ptfci JOIN inputs i" & icardid & " ON i" & icardid & ".iinputid = ptfci.icompoundinputid LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) cipp GROUP BY iinputid, iprojectid) cipp ON cipp.iprojectid = ptfci.iprojectid AND cipp.iinputid = ptfci.icompoundinputid WHERE ptfci.iprojectid = " & iprojectid & " AND ptfci.icardid = " & icardid & " GROUP BY ptfci.iprojectid, ptfci.icardid, ptfci.iinputid) cipp ON ptfi.iprojectid = cipp.iprojectid AND i.iinputid = cipp.iinputid " & _
                        "WHERE ptfi.iprojectid = " & iprojectid & " AND ptfi.icardid = " & icardid & " AND it.sinputtypedescription = 'MATERIALES' " & _
                        "GROUP BY ptfi.iprojectid, ptfi.iinputid " & _
                        "ORDER BY 2, 3, 4) " & _
                        "UNION " & _
                        "SELECT '', '', '', '', 'SUMA DE MATERIALES', IF(SUM(ptfi.dcardinputqty*pp.dinputfinalprice) IS NULL, 0, SUM(ptfi.dcardinputqty*pp.dinputfinalprice))+IF(SUM(ptfi.dcardinputqty*cipp.dinputfinalprice) IS NULL, 0, SUM(ptfi.dcardinputqty*cipp.dinputfinalprice)) AS amount " & _
                        "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "CardInputs ptfi " & _
                        "JOIN inputs i ON ptfi.iinputid = i.iinputid " & _
                        "JOIN inputtypes it ON i.iinputid = it.iinputid " & _
                        "LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) pp GROUP BY iinputid, iprojectid) pp ON ptfi.iprojectid = pp.iprojectid AND i.iinputid = pp.iinputid " & _
                        "LEFT JOIN (SELECT ptfci.iprojectid, ptfci.iinputid, cipp.iupdatedate, cipp.supdatetime, SUM(ptfci.dcompoundinputqty*cipp.dinputfinalprice) AS dinputpricewithoutIVA, 0.00000 AS dinputprotectionpercentage, SUM(ptfci.dcompoundinputqty*cipp.dinputfinalprice) AS dinputfinalprice FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "CardCompoundInputs ptfci JOIN inputs i" & icardid & " ON i" & icardid & ".iinputid = ptfci.icompoundinputid LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) cipp GROUP BY iinputid, iprojectid) cipp ON cipp.iprojectid = ptfci.iprojectid AND cipp.iinputid = ptfci.icompoundinputid WHERE ptfci.iprojectid = " & iprojectid & " AND ptfci.icardid = " & icardid & " GROUP BY ptfci.iprojectid, ptfci.icardid, ptfci.iinputid) cipp ON ptfi.iprojectid = cipp.iprojectid AND i.iinputid = cipp.iinputid " & _
                        "WHERE ptfi.iprojectid = " & iprojectid & " AND ptfi.icardid = " & icardid & " AND it.sinputtypedescription = 'MATERIALES' "

                        If getSQLQueryAsBoolean(0, "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "CardInputs ptfi JOIN inputtypes it ON ptfi.iinputid = it.iinputid WHERE iprojectid = " & iprojectid & " AND icardid = " & icardid & " AND it.sinputtypedescription = 'MANO DE OBRA'") = True Then

                            queryInsumosTarjetas &= "UNION "

                        End If

                    End If


                    If getSQLQueryAsBoolean(0, "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "CardInputs ptfi JOIN inputtypes it ON ptfi.iinputid = it.iinputid WHERE iprojectid = " & iprojectid & " AND icardid = " & icardid & " AND it.sinputtypedescription = 'MANO DE OBRA'") = True Then

                        'Mano de Obra

                        queryInsumosTarjetas &= "SELECT '' AS iinputid, 'MANO DE OBRA' AS sinputdescription, '' AS scardinputunit, '' AS unitprice, " & _
                        "'' AS dcardinputqty, '' AS amount " & _
                        "UNION " & _
                        "(SELECT ptfi.iinputid, i.sinputdescription, ptfi.scardinputunit, pp.dinputfinalprice AS unitprice, " & _
                        "ptfi.dcardinputqty, IF(SUM(ptfi.dcardinputqty*pp.dinputfinalprice) IS NULL, 0, SUM(ptfi.dcardinputqty*pp.dinputfinalprice)) AS amount " & _
                        "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "CardInputs ptfi " & _
                        "JOIN inputs i ON ptfi.iinputid = i.iinputid " & _
                        "JOIN inputtypes it ON i.iinputid = it.iinputid " & _
                        "LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) pp GROUP BY iinputid, iprojectid) pp ON ptfi.iprojectid = pp.iprojectid AND i.iinputid = pp.iinputid " & _
                        "WHERE ptfi.iprojectid = " & iprojectid & " AND ptfi.icardid = " & icardid & " AND it.sinputtypedescription = 'MANO DE OBRA' " & _
                        "GROUP BY ptfi.iprojectid, ptfi.iinputid " & _
                        "ORDER BY 2, 3, 4) " & _
                        "UNION " & _
                        "SELECT '', '', '', '', 'SUMA DE MANO DE OBRA', IF(SUM(ptfi.dcardinputqty*pp.dinputfinalprice) IS NULL, 0, SUM(ptfi.dcardinputqty*pp.dinputfinalprice)) AS amount " & _
                        "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "CardInputs ptfi " & _
                        "JOIN inputs i ON ptfi.iinputid = i.iinputid " & _
                        "JOIN inputtypes it ON i.iinputid = it.iinputid " & _
                        "LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) pp GROUP BY iinputid, iprojectid) pp ON ptfi.iprojectid = pp.iprojectid AND i.iinputid = pp.iinputid " & _
                        "WHERE ptfi.iprojectid = " & iprojectid & " AND ptfi.icardid = " & icardid & " AND it.sinputtypedescription = 'MANO DE OBRA' "

                        If getSQLQueryAsBoolean(0, "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "CardInputs ptfi JOIN inputtypes it ON ptfi.iinputid = it.iinputid WHERE iprojectid = " & iprojectid & " AND icardid = " & icardid & " AND it.sinputtypedescription = 'EQUIPO Y HERRAMIENTA'") = True Then

                            queryInsumosTarjetas &= "UNION "

                        End If

                    End If


                    If getSQLQueryAsBoolean(0, "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "CardInputs ptfi JOIN inputtypes it ON ptfi.iinputid = it.iinputid WHERE iprojectid = " & iprojectid & " AND icardid = " & icardid & " AND it.sinputtypedescription = 'EQUIPO Y HERRAMIENTA'") = True Then

                        'Equipo y Herramienta

                        queryInsumosTarjetas &= "SELECT '' AS iinputid, 'EQUIPO Y HERRAMIENTA' AS sinputdescription, '' AS scardinputunit, '' AS unitprice, " & _
                        "'' AS dcardinputqty, '' AS amount " & _
                        "UNION " & _
                        "(SELECT ptfi.iinputid, 'EQUIPO Y HERRAMIENTA', CONCAT(ptfi.scardinputunit, ' M.O.') AS scardinputunit, " & _
                        "IF(eq.amount IS NULL, 0, eq.amount) AS unitprice, ptfi.dcardinputqty, IF(SUM(ptfi.dcardinputqty*eq.amount) IS NULL, 0, SUM(ptfi.dcardinputqty*eq.amount)) AS amount " & _
                        "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "CardInputs ptfi " & _
                        "LEFT JOIN (SELECT ptfi.iprojectid, 0 AS iinputid, SUM(ptfi.dcardinputqty*pp.dinputfinalprice) AS amount FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "CardInputs ptfi JOIN inputs i ON ptfi.iinputid = i.iinputid JOIN inputtypes it ON i.iinputid = it.iinputid LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) pp GROUP BY iinputid, iprojectid) pp ON ptfi.iprojectid = pp.iprojectid AND i.iinputid = pp.iinputid WHERE ptfi.iprojectid = " & iprojectid & " AND ptfi.icardid = " & icardid & " AND it.sinputtypedescription = 'MANO DE OBRA' GROUP BY ptfi.iprojectid, ptfi.iinputid) eq ON ptfi.iprojectid = eq.iprojectid AND ptfi.iinputid = eq.iinputid " & _
                        "WHERE ptfi.iprojectid = " & iprojectid & " AND ptfi.icardid = " & icardid & " and ptfi.iinputid = 0 " & _
                        "GROUP BY ptfi.iprojectid, ptfi.iinputid " & _
                        "ORDER BY 2, 3, 4) " & _
                        "UNION " & _
                        "SELECT '', '', '', '', 'SUMA DE EQUIPO Y HERRAMIENTA', IF(SUM(ptfi.dcardinputqty*eq.amount) IS NULL, 0, SUM(ptfi.dcardinputqty*eq.amount)) AS amount " & _
                        "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "CardInputs ptfi " & _
                        "JOIN (SELECT ptfi.iprojectid, 0 AS iinputid, SUM(ptfi.dcardinputqty*pp.dinputfinalprice) AS amount FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "CardInputs ptfi JOIN inputs i ON ptfi.iinputid = i.iinputid JOIN inputtypes it ON i.iinputid = it.iinputid LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) pp GROUP BY iinputid, iprojectid) pp ON ptfi.iprojectid = pp.iprojectid AND i.iinputid = pp.iinputid WHERE ptfi.iprojectid = " & iprojectid & " AND ptfi.icardid = " & icardid & " AND it.sinputtypedescription = 'MANO DE OBRA' GROUP BY ptfi.iprojectid, ptfi.iinputid) eq ON ptfi.iprojectid = eq.iprojectid AND ptfi.iinputid = eq.iinputid " & _
                        "WHERE ptfi.iprojectid = " & iprojectid & " AND ptfi.icardid = " & icardid

                    End If


                Else

                    'Regresamos En Blanco el Grid
                    queryInsumosTarjetas &= "SELECT ptfi.iinputid, i.sinputdescription, ptfi.scardinputunit, IF(pp.dinputfinalprice IS NULL, cipp.dinputfinalprice, pp.dinputfinalprice) AS unitprice, " & _
                    "ptfi.dcardinputqty, IF(SUM(ptfi.dcardinputqty*pp.dinputfinalprice) IS NULL, SUM(ptfi.dcardinputqty*cipp.dinputfinalprice), SUM(ptfi.dcardinputqty*pp.dinputfinalprice)) AS amount " & _
                    "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "CardInputs ptfi " & _
                    "JOIN inputs i ON ptfi.iinputid = i.iinputid " & _
                    "JOIN inputtypes it ON i.iinputid = it.iinputid " & _
                    "LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) pp GROUP BY iinputid, iprojectid) pp ON ptfi.iprojectid = pp.iprojectid AND i.iinputid = pp.iinputid " & _
                    "LEFT JOIN (SELECT ptfci.iprojectid, ptfci.iinputid, cipp.iupdatedate, cipp.supdatetime, SUM(ptfci.dcompoundinputqty*cipp.dinputfinalprice) AS dinputpricewithoutIVA, 0.00000 AS dinputprotectionpercentage, SUM(ptfci.dcompoundinputqty*cipp.dinputfinalprice) AS dinputfinalprice FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "CardCompoundInputs ptfci JOIN inputs i" & icardid & " ON i" & icardid & ".iinputid = ptfci.icompoundinputid LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) cipp GROUP BY iinputid, iprojectid) cipp ON cipp.iprojectid = ptfci.iprojectid AND cipp.iinputid = ptfci.icompoundinputid WHERE ptfci.iprojectid = " & iprojectid & " AND ptfci.icardid = " & icardid & " GROUP BY ptfci.iprojectid, ptfci.icardid, ptfci.iinputid) cipp ON ptfi.iprojectid = cipp.iprojectid AND i.iinputid = cipp.iinputid " & _
                    "WHERE ptfi.iprojectid = " & iprojectid & " AND ptfi.icardid = " & icardid & " AND it.sinputtypedescription = 'MATERIALES' " & _
                    "GROUP BY ptfi.iprojectid, ptfi.iinputid "

                End If


            End If

        End If

        executeSQLCommand(0, queryInsumosTarjetas)

        If IsBase = True Then

            setDataGridView(dgvConceptosTarjeta, "SELECT iinputid, sinputdescription AS 'Insumo', scardinputunit AS 'Unidad', IF(iinputid <> '', FORMAT(unitprice, 2), '') AS 'Precio Unitario', IF(iinputid <> '', FORMAT(dcardinputqty, 3), '') AS 'Cantidad', IF(iinputid <> '', FORMAT(amount, 2), '') AS 'Importe' FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardAux" & icardid, False)

        Else

            If IsModel = True Then

                setDataGridView(dgvConceptosTarjeta, "SELECT iinputid, sinputdescription AS 'Insumo', scardinputunit AS 'Unidad', IF(iinputid <> '', FORMAT(unitprice, 2), '') AS 'Precio Unitario', IF(iinputid <> '', FORMAT(dcardinputqty, 3), '') AS 'Cantidad', IF(iinputid <> '', FORMAT(amount, 2), '') AS 'Importe' FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "CardAux" & icardid, False)

            Else

                setDataGridView(dgvConceptosTarjeta, "SELECT iinputid, sinputdescription AS 'Insumo', scardinputunit AS 'Unidad', IF(iinputid <> '', FORMAT(unitprice, 2), '') AS 'Precio Unitario', IF(iinputid <> '', FORMAT(dcardinputqty, 3), '') AS 'Cantidad', IF(iinputid <> '', FORMAT(amount, 2), '') AS 'Importe' FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "CardAux" & icardid, False)

            End If

        End If

        dgvConceptosTarjeta.Columns(0).Visible = False
        dgvConceptosTarjeta.Columns(2).ReadOnly = True
        dgvConceptosTarjeta.Columns(3).ReadOnly = True
        dgvConceptosTarjeta.Columns(5).ReadOnly = True

        If IsHistoric = True Then

            dgvConceptosTarjeta.Columns(1).ReadOnly = True
            dgvConceptosTarjeta.Columns(4).ReadOnly = True

            dgvConceptosTarjeta.ReadOnly = True

            dgvConceptosTarjeta.Enabled = True
            btnNuevoInsumo.Enabled = False
            btnInsertarInsumo.Enabled = False
            btnEliminarInsumo.Enabled = False

        End If


        dgvConceptosTarjeta.Columns(0).Width = 30
        dgvConceptosTarjeta.Columns(1).Width = 150
        dgvConceptosTarjeta.Columns(2).Width = 70
        dgvConceptosTarjeta.Columns(3).Width = 70
        dgvConceptosTarjeta.Columns(4).Width = 70
        dgvConceptosTarjeta.Columns(5).Width = 70


        Dim queryDatosTarjeta As String = ""
        Dim NombreProyecto As String = ""
        Dim dsDatosTarjeta As DataSet


        If IsBase = True Then

            NombreProyecto = "PRESUPUESTO BASE " & getSQLQueryAsString(0, "SELECT STR_TO_DATE(CONCAT(iupdatedate, ' ', supdatetime), '%Y%c%d %T') AS fechahora FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & " WHERE ibaseid = " & iprojectid)

            queryDatosTarjeta = "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Cards WHERE ibaseid = " & iprojectid & " AND icardid = " & icardid

        Else

            If IsModel = True Then

                NombreProyecto = "Modelo " & getSQLQueryAsString(0, "SELECT smodelname FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & " WHERE imodelid = " & iprojectid)

                queryDatosTarjeta = "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "Cards WHERE imodelid = " & iprojectid & " AND icardid = " & icardid

            Else

                NombreProyecto = "Proyecto " & getSQLQueryAsString(0, "SELECT sprojectname FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & " WHERE iprojectid = " & iprojectid)

                queryDatosTarjeta = "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "Cards WHERE iprojectid = " & iprojectid & " AND icardid = " & icardid

            End If

        End If

        dsDatosTarjeta = getSQLQueryAsDataset(0, queryDatosTarjeta)

        If IsEdit = True Then

            If IsHistoric = True Then
                Me.Text = "Tarjeta Histórica " & dsDatosTarjeta.Tables(0).Rows(0).Item("scardlegacyid") & " - " & NombreProyecto
            Else
                Me.Text = "Tarjeta " & dsDatosTarjeta.Tables(0).Rows(0).Item("scardlegacyid") & " - " & NombreProyecto
            End If

        Else

            Me.Text = "Tarjeta Nueva - " & NombreProyecto

        End If

        If IsEdit = True Then

            cmbCategoriaDeLaTarjeta.SelectedValue = dsDatosTarjeta.Tables(0).Rows(0).Item("scardlegacycategoryid")
            txtCodigoDeLaTarjeta.Text = dsDatosTarjeta.Tables(0).Rows(0).Item("scardlegacyid")
            txtUnidadDeMedida.Text = dsDatosTarjeta.Tables(0).Rows(0).Item("scardunit")
            txtUltimaModificacion.Text = convertYYYYMMDDtoDDhyphenMMhyphenYYYY(dsDatosTarjeta.Tables(0).Rows(0).Item("iupdatedate")) & " " & dsDatosTarjeta.Tables(0).Rows(0).Item("supdatetime")
            txtUltimaModificacion.Enabled = False
            txtNombreDeLaTarjeta.Text = dsDatosTarjeta.Tables(0).Rows(0).Item("scarddescription")

        Else

            txtUltimaModificacion.Text = convertYYYYMMDDtoDDhyphenMMhyphenYYYY(getMySQLDate()) & " " & getAppTime()
            txtUltimaModificacion.Enabled = False

        End If


        If isFormReadyForAction = True Then
            updateTotals()
        End If


        If IsEdit = True Then


            If IsHistoric = True Then

                cmbCategoriaDeLaTarjeta.Enabled = False
                txtUnidadDeMedida.Enabled = False
                txtCodigoDeLaTarjeta.Enabled = False
                txtUltimaModificacion.Enabled = False
                txtNombreDeLaTarjeta.Enabled = False
                txtPorcentajeIndirectos.Enabled = False
                txtPorcentajeUtilidades.Enabled = False
                txtCostoDirecto.Enabled = False
                txtIndirectos.Enabled = False
                txtSuma.Enabled = False
                txtUtilidades.Enabled = False
                txtSubTotal.Enabled = False
                txtIVA.Enabled = False
                txtTotal.Enabled = False

            End If


            'Dim queryCardHistoricPrices As String = ""

            'Dim chart As NCartesianChart = nccPreciosHistoricosTarjeta.Charts(0)
            'chart.Axis(StandardAxis.Depth).Visible = False

            'Dim line As NLineSeries = chart.Series.Add(SeriesType.Line)
            'line.Name = "Values"
            'line.InflateMargins = True
            'line.DataLabelStyle.Format = "<value>"
            'line.MarkerStyle.Visible = True
            'line.MarkerStyle.PointShape = PointShape.Cylinder
            'line.MarkerStyle.Width = New NLength(1.5F, NRelativeUnit.ParentPercentage)
            'line.MarkerStyle.Height = New NLength(1.5F, NRelativeUnit.ParentPercentage)
            'line.ShadowStyle.Type = ShadowType.GaussianBlur
            'line.ShadowStyle.Offset = New NPointL(3, 3)
            'line.ShadowStyle.FadeLength = New NLength(5)
            'line.ShadowStyle.Color = Color.FromArgb(55, 0, 0, 0)

            'line.Values.Clear()

            'If IsBase = True Then

            '    queryCardHistoricPrices = "" & _
            '    "SELECT iupdatedate, dinputfinalprice " & _
            '    "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Prices " & _
            '    "WHERE iinputid = 7"

            'Else

            '    If IsModel = True Then

            '        queryCardHistoricPrices = "" & _
            '        "SELECT iupdatedate, dinputfinalprice " & _
            '        "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "Prices " & _
            '        "WHERE iinputid = 7"

            '    Else

            '        queryCardHistoricPrices = "" & _
            '        "SELECT iupdatedate, dinputfinalprice " & _
            '        "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "Prices " & _
            '        "WHERE iinputid = 7"

            '    End If

            'End If

            'Dim dtCardHistoricPrices As DataTable
            'dtCardHistoricPrices = getSQLQueryAsDataTable(0, queryCardHistoricPrices)

            'line.Values.FillFromDataTable(dtCardHistoricPrices, "Total")

            'Dim maxvalues As String = "" & _
            '"SELECT STR_TO_DATE(CONCAT(MAX(iupdatedate), ' ', MAX(supdatetime)), '%Y%c%d %T') AS dupdatedate AS dinputfinalprice FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "Prices " & _
            '"WHERE iinputid = 7"

            'Dim minvalues As String = "" & _
            '"SELECT STR_TO_DATE(CONCAT(MIN(iupdatedate), ' ', MIN(supdatetime)), '%Y%c%d %T') AS dupdatedate AS dinputfinalprice FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "Prices " & _
            '"WHERE iinputid = 7"

            'Dim dsMax As DataSet = getSQLQueryAsDataset(0, maxvalues)
            'Dim dsMin As DataSet = getSQLQueryAsDataset(0, minvalues)


            'chart.Axis(StandardAxis.PrimaryY).View = New NRangeAxisView(New NRange1DD(dsMin.Tables(0).Rows(0).Item("dupdatedate"), dsMax.Tables(0).Rows(0).Item("dupdatedate")), True, True)


        End If


        If IsHistoric = True Then
            btnGuardar.Visible = False
        Else
            btnGuardar.Visible = True
        End If


        Cursor.Current = System.Windows.Forms.Cursors.Default


    End Sub


    Private Sub updateTotals()

        If isFormReadyForAction = True Then

            'chequeo de si hay o no hay suficientes insumos para hacer el calculo: ej. mat+mo+eq
            'Dim chequeoMatTarjeta As Double = 0.0
            Dim chequeoMOTarjeta As Double = 0.0
            Dim chequeoEQTarjeta As Double = 0.0

            If IsBase = True Then

                'chequeoMatTarjeta = getSQLQueryAsDouble(0, "SELECT COUNT(*) FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardInputs ptfi JOIN inputtypes it ON ptfi.iinputid = it.iinputid WHERE ptfi.ibaseid = " & iprojectid & " AND ptfi.icardid = " & icardid & " AND it.sinputtypedescription = 'MATERIALES'")
                chequeoMOTarjeta = getSQLQueryAsDouble(0, "SELECT COUNT(*) FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardInputs ptfi JOIN inputtypes it ON ptfi.iinputid = it.iinputid WHERE ptfi.ibaseid = " & iprojectid & " AND ptfi.icardid = " & icardid & " AND it.sinputtypedescription = 'MANO DE OBRA'")
                chequeoEQTarjeta = getSQLQueryAsDouble(0, "SELECT COUNT(*) FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardInputs ptfi JOIN inputtypes it ON ptfi.iinputid = it.iinputid WHERE ptfi.ibaseid = " & iprojectid & " AND ptfi.icardid = " & icardid & " AND it.sinputtypedescription = 'EQUIPO Y HERRAMIENTA'")

            Else

                If IsModel = True Then

                    'chequeoMatTarjeta = getSQLQueryAsDouble(0, "SELECT COUNT(*) FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "CardInputs ptfi JOIN inputtypes it ON ptfi.iinputid = it.iinputid WHERE ptfi.imodelid = " & iprojectid & " AND ptfi.icardid = " & icardid & " AND it.sinputtypedescription = 'MATERIALES'")
                    chequeoMOTarjeta = getSQLQueryAsDouble(0, "SELECT COUNT(*) FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "CardInputs ptfi JOIN inputtypes it ON ptfi.iinputid = it.iinputid WHERE ptfi.imodelid = " & iprojectid & " AND ptfi.icardid = " & icardid & " AND it.sinputtypedescription = 'MANO DE OBRA'")
                    chequeoEQTarjeta = getSQLQueryAsDouble(0, "SELECT COUNT(*) FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "CardInputs ptfi JOIN inputtypes it ON ptfi.iinputid = it.iinputid WHERE ptfi.imodelid = " & iprojectid & " AND ptfi.icardid = " & icardid & " AND it.sinputtypedescription = 'EQUIPO Y HERRAMIENTA'")

                Else

                    'chequeoMatTarjeta = getSQLQueryAsDouble(0, "SELECT COUNT(*) FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "CardInputs ptfi JOIN inputtypes it ON ptfi.iinputid = it.iinputid WHERE ptfi.iprojectid = " & iprojectid & " AND ptfi.icardid = " & icardid & " AND it.sinputtypedescription = 'MATERIALES'")
                    chequeoMOTarjeta = getSQLQueryAsDouble(0, "SELECT COUNT(*) FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "CardInputs ptfi JOIN inputtypes it ON ptfi.iinputid = it.iinputid WHERE ptfi.iprojectid = " & iprojectid & " AND ptfi.icardid = " & icardid & " AND it.sinputtypedescription = 'MANO DE OBRA'")
                    chequeoEQTarjeta = getSQLQueryAsDouble(0, "SELECT COUNT(*) FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "CardInputs ptfi JOIN inputtypes it ON ptfi.iinputid = it.iinputid WHERE ptfi.iprojectid = " & iprojectid & " AND ptfi.icardid = " & icardid & " AND it.sinputtypedescription = 'EQUIPO Y HERRAMIENTA'")

                End If

            End If


            'If chequeoMatTarjeta > 0.0 Then

            If chequeoMOTarjeta > 0.0 Then

                If chequeoEQTarjeta > 0.0 Then
                    'Continue
                Else
                    Exit Sub
                End If

            Else
                Exit Sub
            End If

            'Else
            '   Exit Sub
            'End If

            Dim queryCalculosTarjeta As String = ""
            Dim queryIVATarjeta As String = ""

            If IsBase = True Then

                queryCalculosTarjeta = "" & _
                "SELECT btf.ibaseid, btf.icardid, SUM(amountMO+amountMAT+amountEQ) AS costodirecto, (SUM(amountMO+amountMAT+amountEQ)*btf.dcardindirectcostspercentage) AS costoindirecto, " & _
                "(SUM(amountMO+amountMAT+amountEQ)+(SUM(amountMO+amountMAT+amountEQ)*btf.dcardindirectcostspercentage)) AS suma, " & _
                "((SUM(amountMO+amountMAT+amountEQ)+(SUM(amountMO+amountMAT+amountEQ)*btf.dcardindirectcostspercentage))*dcardgainpercentage) AS utilidad, " & _
                "(((SUM(amountMO+amountMAT+amountEQ)+(SUM(amountMO+amountMAT+amountEQ)*btf.dcardindirectcostspercentage))*dcardgainpercentage)+(SUM(amountMO+amountMAT+amountEQ)+(SUM(amountMO+amountMAT+amountEQ)*btf.dcardindirectcostspercentage))) AS subtotal, " & _
                "((((SUM(amountMO+amountMAT+amountEQ)+(SUM(amountMO+amountMAT+amountEQ)*btf.dcardindirectcostspercentage))*dcardgainpercentage)+(SUM(amountMO+amountMAT+amountEQ)+(SUM(amountMO+amountMAT+amountEQ)*btf.dcardindirectcostspercentage)))*b.dbaseIVApercentage) AS IVA, " & _
                "(((((SUM(amountMO+amountMAT+amountEQ)+(SUM(amountMO+amountMAT+amountEQ)*btf.dcardindirectcostspercentage))*dcardgainpercentage)+(SUM(amountMO+amountMAT+amountEQ)+(SUM(amountMO+amountMAT+amountEQ)*btf.dcardindirectcostspercentage)))*b.dbaseIVApercentage)+(((SUM(amountMO+amountMAT+amountEQ)+(SUM(amountMO+amountMAT+amountEQ)*btf.dcardindirectcostspercentage))*dcardgainpercentage)+(SUM(amountMO+amountMAT+amountEQ)+(SUM(amountMO+amountMAT+amountEQ)*btf.dcardindirectcostspercentage)))) AS total " & _
                "FROM " & _
                " ( " & _
                "  SELECT ibaseid, icardid, IF(SUM(amountMAT) IS NULL, 0, SUM(amountMAT)) AS amountMAT, IF(SUM(amountMO) IS NULL, 0, SUM(amountMO)) AS amountMO, IF(SUM(amountEQ) IS NULL, 0, SUM(amountEQ)) AS amountEQ FROM " & _
                "  (SELECT " & iprojectid & " AS ibaseid, " & icardid & " AS icardid, 'SUMA DE MATERIALES', IF(SUM(btfi.dcardinputqty*bp.dinputfinalprice) IS NULL, 0, SUM(btfi.dcardinputqty*bp.dinputfinalprice))+IF(SUM(btfi.dcardinputqty*cibp.dinputfinalprice) IS NULL, 0, SUM(btfi.dcardinputqty*cibp.dinputfinalprice)) AS amountMAT, 0 AS amountMO, 0 AS amountEQ " & _
                "  FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardInputs btfi " & _
                "  JOIN inputs i ON btfi.iinputid = i.iinputid " & _
                "  JOIN inputtypes it ON it.iinputid = i.iinputid " & _
                "  LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) bp GROUP BY iinputid, ibaseid) bp ON btfi.ibaseid = bp.ibaseid AND i.iinputid = bp.iinputid " & _
                "  LEFT JOIN (SELECT btfci.ibaseid, btfci.iinputid, cibp.iupdatedate, cibp.supdatetime, SUM(btfci.dcompoundinputqty*cibp.dinputfinalprice) AS dinputpricewithoutIVA, 0.00000 AS dinputprotectionpercentage, SUM(btfci.dcompoundinputqty*cibp.dinputfinalprice) AS dinputfinalprice FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardCompoundInputs btfci JOIN inputs i" & icardid & " ON i" & icardid & ".iinputid = btfci.icompoundinputid LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) cibp GROUP BY iinputid, ibaseid) cibp ON cibp.ibaseid = btfci.ibaseid AND cibp.iinputid = btfci.icompoundinputid WHERE btfci.ibaseid = " & iprojectid & " AND btfci.icardid = " & icardid & " GROUP BY btfci.ibaseid, btfci.icardid, btfci.iinputid) cibp ON btfi.ibaseid = cibp.ibaseid AND i.iinputid = cibp.iinputid " & _
                "  WHERE btfi.ibaseid = " & iprojectid & " AND btfi.icardid =" & icardid & " AND it.sinputtypedescription = 'MATERIALES' " & _
                "  UNION " & _
                "  SELECT " & iprojectid & " AS ibaseid, " & icardid & " AS icardid, 'SUMA DE MANO DE OBRA', 0 AS amountMAT, IF(SUM(btfi.dcardinputqty*bp.dinputfinalprice) IS NULL, 0, SUM(btfi.dcardinputqty*bp.dinputfinalprice)) AS amountMO, 0 AS amountEQ " & _
                "  FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardInputs btfi " & _
                "  JOIN inputs i ON btfi.iinputid = i.iinputid " & _
                "  JOIN inputtypes it ON it.iinputid = i.iinputid " & _
                "  LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) bp GROUP BY iinputid, ibaseid) bp ON btfi.ibaseid = bp.ibaseid AND i.iinputid = bp.iinputid " & _
                "  WHERE btfi.ibaseid = " & iprojectid & " AND btfi.icardid =" & icardid & " AND it.sinputtypedescription = 'MANO DE OBRA' " & _
                "  UNION " & _
                "  SELECT " & iprojectid & " AS ibaseid, " & icardid & " AS icardid, 'SUMA DE EQUIPO Y HERRAMIENTA', 0 AS amountMAT, 0 AS amountMO, IF(SUM(btfi.dcardinputqty*eq.amount) IS NULL, 0, SUM(btfi.dcardinputqty*eq.amount)) AS amountEQ " & _
                "  FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardInputs btfi " & _
                "  JOIN (SELECT btfi.ibaseid,  btfi.icardid, 0 AS iinputid, SUM(btfi.dcardinputqty*bp.dinputfinalprice) AS amount FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardInputs btfi JOIN inputs i ON btfi.iinputid = i.iinputid JOIN inputtypes it ON i.iinputid = it.iinputid LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) bp GROUP BY iinputid, ibaseid) bp ON btfi.ibaseid = bp.ibaseid AND i.iinputid = bp.iinputid WHERE btfi.ibaseid = " & iprojectid & " AND btfi.icardid =" & icardid & " AND it.sinputtypedescription = 'MANO DE OBRA' GROUP BY btfi.ibaseid, btfi.iinputid) eq ON btfi.ibaseid = eq.ibaseid AND btfi.iinputid = eq.iinputid " & _
                "  WHERE btfi.ibaseid = " & iprojectid & " AND btfi.icardid =" & icardid & " AND btfi.iinputid = 0 " & _
                "  ) AS costodirectoinner " & _
                "  GROUP BY 1,2 " & _
                " ) AS costodirectoouter " & _
                "JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Cards btf ON costodirectoouter.ibaseid = btf.ibaseid AND costodirectoouter.icardid = btf.icardid " & _
                "JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & " b ON btf.ibaseid = b.ibaseid " & _
                "WHERE b.ibaseid = " & iprojectid & " "

                queryIVATarjeta = "SELECT dbaseIVApercentage FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & " WHERE ibaseid = " & iprojectid

            Else

                If IsModel = True Then

                    queryCalculosTarjeta = "" & _
                    "SELECT mtf.imodelid, mtf.icardid, SUM(amountMO+amountMAT+amountEQ) AS costodirecto, (SUM(amountMO+amountMAT+amountEQ)*mtf.dcardindirectcostspercentage) AS costoindirecto, " & _
                    "(SUM(amountMO+amountMAT+amountEQ)+(SUM(amountMO+amountMAT+amountEQ)*mtf.dcardindirectcostspercentage)) AS suma, " & _
                    "((SUM(amountMO+amountMAT+amountEQ)+(SUM(amountMO+amountMAT+amountEQ)*mtf.dcardindirectcostspercentage))*dcardgainpercentage) AS utilidad, " & _
                    "(((SUM(amountMO+amountMAT+amountEQ)+(SUM(amountMO+amountMAT+amountEQ)*mtf.dcardindirectcostspercentage))*dcardgainpercentage)+(SUM(amountMO+amountMAT+amountEQ)+(SUM(amountMO+amountMAT+amountEQ)*mtf.dcardindirectcostspercentage))) AS subtotal, " & _
                    "((((SUM(amountMO+amountMAT+amountEQ)+(SUM(amountMO+amountMAT+amountEQ)*mtf.dcardindirectcostspercentage))*dcardgainpercentage)+(SUM(amountMO+amountMAT+amountEQ)+(SUM(amountMO+amountMAT+amountEQ)*mtf.dcardindirectcostspercentage)))*m.dmodelIVApercentage) AS IVA, " & _
                    "(((((SUM(amountMO+amountMAT+amountEQ)+(SUM(amountMO+amountMAT+amountEQ)*mtf.dcardindirectcostspercentage))*dcardgainpercentage)+(SUM(amountMO+amountMAT+amountEQ)+(SUM(amountMO+amountMAT+amountEQ)*mtf.dcardindirectcostspercentage)))*m.dmodelIVApercentage)+(((SUM(amountMO+amountMAT+amountEQ)+(SUM(amountMO+amountMAT+amountEQ)*mtf.dcardindirectcostspercentage))*dcardgainpercentage)+(SUM(amountMO+amountMAT+amountEQ)+(SUM(amountMO+amountMAT+amountEQ)*mtf.dcardindirectcostspercentage)))) AS total " & _
                    "FROM " & _
                    " ( " & _
                    "  SELECT imodelid, icardid, IF(SUM(amountMAT) IS NULL, 0, SUM(amountMAT)) AS amountMAT, IF(SUM(amountMO) IS NULL, 0, SUM(amountMO)) AS amountMO, IF(SUM(amountEQ) IS NULL, 0, SUM(amountEQ)) AS amountEQ FROM " & _
                    "  (SELECT " & iprojectid & " AS imodelid, " & icardid & " AS icardid, 'SUMA DE MATERIALES', IF(SUM(mtfi.dcardinputqty*mp.dinputfinalprice) IS NULL, 0, SUM(mtfi.dcardinputqty*mp.dinputfinalprice))+IF(SUM(mtfi.dcardinputqty*cimp.dinputfinalprice) IS NULL, 0, SUM(mtfi.dcardinputqty*cimp.dinputfinalprice)) AS amountMAT, 0 AS amountMO, 0 AS amountEQ " & _
                    "  FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "CardInputs mtfi " & _
                    "  JOIN inputs i ON mtfi.iinputid = i.iinputid " & _
                    "  JOIN inputtypes it ON it.iinputid = i.iinputid " & _
                    "  LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) mp GROUP BY iinputid, imodelid) mp ON mtfi.imodelid = mp.imodelid AND i.iinputid = mp.iinputid " & _
                    "  LEFT JOIN (SELECT mtfci.imodelid, mtfci.iinputid, cimp.iupdatedate, cimp.supdatetime, SUM(mtfci.dcompoundinputqty*cimp.dinputfinalprice) AS dinputpricewithoutIVA, 0.00000 AS dinputprotectionpercentage, SUM(mtfci.dcompoundinputqty*cimp.dinputfinalprice) AS dinputfinalprice FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "CardCompoundInputs mtfci JOIN inputs i" & icardid & " ON i" & icardid & ".iinputid = mtfci.icompoundinputid LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) cimp GROUP BY iinputid, imodelid) cimp ON cimp.imodelid = mtfci.imodelid AND cimp.iinputid = mtfci.icompoundinputid WHERE mtfci.imodelid = " & iprojectid & " AND mtfci.icardid = " & icardid & " GROUP BY mtfci.imodelid, mtfci.icardid, mtfci.iinputid) cimp ON mtfi.imodelid = cimp.imodelid AND i.iinputid = cimp.iinputid " & _
                    "  WHERE mtfi.imodelid = " & iprojectid & " AND mtfi.icardid =" & icardid & " AND it.sinputtypedescription = 'MATERIALES' " & _
                    "  UNION " & _
                    "  SELECT " & iprojectid & " AS imodelid, " & icardid & " AS icardid, 'SUMA DE MANO DE OBRA', 0 AS amountMAT, IF(SUM(mtfi.dcardinputqty*mp.dinputfinalprice) IS NULL, 0, SUM(mtfi.dcardinputqty*mp.dinputfinalprice)) AS amountMO, 0 AS amountEQ " & _
                    "  FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "CardInputs mtfi " & _
                    "  JOIN inputs i ON mtfi.iinputid = i.iinputid " & _
                    "  JOIN inputtypes it ON it.iinputid = i.iinputid " & _
                    "  LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) mp GROUP BY iinputid, imodelid) mp ON mtfi.imodelid = mp.imodelid AND i.iinputid = mp.iinputid " & _
                    "  WHERE mtfi.imodelid = " & iprojectid & " AND mtfi.icardid =" & icardid & " AND it.sinputtypedescription = 'MANO DE OBRA' " & _
                    "  UNION " & _
                    "  SELECT " & iprojectid & " AS imodelid, " & icardid & " AS icardid, 'SUMA DE EQUIPO Y HERRAMIENTA', 0 AS amountMAT, 0 AS amountMO, IF(SUM(mtfi.dcardinputqty*eq.amount) IS NULL, 0, SUM(mtfi.dcardinputqty*eq.amount)) AS amountEQ " & _
                    "  FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "CardInputs mtfi " & _
                    "  JOIN (SELECT mtfi.imodelid,  mtfi.icardid, 0 AS iinputid, SUM(mtfi.dcardinputqty*mp.dinputfinalprice) AS amount FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "CardInputs mtfi JOIN inputs i ON mtfi.iinputid = i.iinputid JOIN inputtypes it ON i.iinputid = it.iinputid LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) mp GROUP BY iinputid, imodelid) mp ON mtfi.imodelid = mp.imodelid AND i.iinputid = mp.iinputid WHERE mtfi.imodelid = " & iprojectid & " AND mtfi.icardid =" & icardid & " AND it.sinputtypedescription = 'MANO DE OBRA' GROUP BY mtfi.imodelid, mtfi.iinputid) eq ON mtfi.imodelid = eq.imodelid AND mtfi.iinputid = eq.iinputid " & _
                    "  WHERE mtfi.imodelid = " & iprojectid & " AND mtfi.icardid =" & icardid & " AND mtfi.iinputid = 0 " & _
                    "  ) AS costodirectoinner " & _
                    "  GROUP BY 1,2 " & _
                    " ) AS costodirectoouter " & _
                    "JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "Cards mtf ON costodirectoouter.imodelid = mtf.imodelid AND costodirectoouter.icardid = mtf.icardid " & _
                    "JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & " m ON mtf.imodelid = m.imodelid " & _
                    "WHERE m.imodelid = " & iprojectid & " "

                    queryIVATarjeta = "SELECT dmodelIVApercentage FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & " WHERE imodelid = " & iprojectid

                Else

                    queryCalculosTarjeta = "" & _
                    "SELECT ptf.iprojectid, ptf.icardid, SUM(amountMO+amountMAT+amountEQ) AS costodirecto, (SUM(amountMO+amountMAT+amountEQ)*ptf.dcardindirectcostspercentage) AS costoindirecto, " & _
                    "(SUM(amountMO+amountMAT+amountEQ)+(SUM(amountMO+amountMAT+amountEQ)*ptf.dcardindirectcostspercentage)) AS suma, " & _
                    "((SUM(amountMO+amountMAT+amountEQ)+(SUM(amountMO+amountMAT+amountEQ)*ptf.dcardindirectcostspercentage))*dcardgainpercentage) AS utilidad, " & _
                    "(((SUM(amountMO+amountMAT+amountEQ)+(SUM(amountMO+amountMAT+amountEQ)*ptf.dcardindirectcostspercentage))*dcardgainpercentage)+(SUM(amountMO+amountMAT+amountEQ)+(SUM(amountMO+amountMAT+amountEQ)*ptf.dcardindirectcostspercentage))) AS subtotal, " & _
                    "((((SUM(amountMO+amountMAT+amountEQ)+(SUM(amountMO+amountMAT+amountEQ)*ptf.dcardindirectcostspercentage))*dcardgainpercentage)+(SUM(amountMO+amountMAT+amountEQ)+(SUM(amountMO+amountMAT+amountEQ)*ptf.dcardindirectcostspercentage)))*p.dprojectIVApercentage) AS IVA, " & _
                    "(((((SUM(amountMO+amountMAT+amountEQ)+(SUM(amountMO+amountMAT+amountEQ)*ptf.dcardindirectcostspercentage))*dcardgainpercentage)+(SUM(amountMO+amountMAT+amountEQ)+(SUM(amountMO+amountMAT+amountEQ)*ptf.dcardindirectcostspercentage)))*p.dprojectIVApercentage)+(((SUM(amountMO+amountMAT+amountEQ)+(SUM(amountMO+amountMAT+amountEQ)*ptf.dcardindirectcostspercentage))*dcardgainpercentage)+(SUM(amountMO+amountMAT+amountEQ)+(SUM(amountMO+amountMAT+amountEQ)*ptf.dcardindirectcostspercentage)))) AS total " & _
                    "FROM " & _
                    " ( " & _
                    "  SELECT iprojectid, icardid, IF(SUM(amountMAT) IS NULL, 0, SUM(amountMAT)) AS amountMAT, IF(SUM(amountMO) IS NULL, 0, SUM(amountMO)) AS amountMO, IF(SUM(amountEQ) IS NULL, 0, SUM(amountEQ)) AS amountEQ FROM " & _
                    "  (SELECT " & iprojectid & " AS iprojectid, " & icardid & " AS icardid, 'SUMA DE MATERIALES', IF(SUM(ptfi.dcardinputqty*pp.dinputfinalprice) IS NULL, 0, SUM(ptfi.dcardinputqty*pp.dinputfinalprice))+IF(SUM(ptfi.dcardinputqty*cipp.dinputfinalprice) IS NULL, 0, SUM(ptfi.dcardinputqty*cipp.dinputfinalprice)) AS amountMAT, 0 AS amountMO, 0 AS amountEQ " & _
                    "  FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "CardInputs ptfi " & _
                    "  JOIN inputs i ON ptfi.iinputid = i.iinputid " & _
                    "  JOIN inputtypes it ON it.iinputid = i.iinputid " & _
                    "  LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) pp GROUP BY iinputid, iprojectid) pp ON ptfi.iprojectid = pp.iprojectid AND i.iinputid = pp.iinputid " & _
                    "  LEFT JOIN (SELECT ptfci.iprojectid, ptfci.iinputid, cipp.iupdatedate, cipp.supdatetime, SUM(ptfci.dcompoundinputqty*cipp.dinputfinalprice) AS dinputpricewithoutIVA, 0.00000 AS dinputprotectionpercentage, SUM(ptfci.dcompoundinputqty*cipp.dinputfinalprice) AS dinputfinalprice FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "CardCompoundInputs ptfci JOIN inputs i" & icardid & " ON i" & icardid & ".iinputid = ptfci.icompoundinputid LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) cipp GROUP BY iinputid, iprojectid) cipp ON cipp.iprojectid = ptfci.iprojectid AND cipp.iinputid = ptfci.icompoundinputid WHERE ptfci.iprojectid = " & iprojectid & " AND ptfci.icardid = " & icardid & " GROUP BY ptfci.iprojectid, ptfci.icardid, ptfci.iinputid) cipp ON ptfi.iprojectid = cipp.iprojectid AND i.iinputid = cipp.iinputid " & _
                    "  WHERE ptfi.iprojectid = " & iprojectid & " AND ptfi.icardid = " & icardid & " AND it.sinputtypedescription = 'MATERIALES' " & _
                    "  UNION " & _
                    "  SELECT " & iprojectid & " AS iprojectid, " & icardid & " AS icardid, 'SUMA DE MANO DE OBRA', 0 AS amountMAT, IF(SUM(ptfi.dcardinputqty*pp.dinputfinalprice) IS NULL, 0, SUM(ptfi.dcardinputqty*pp.dinputfinalprice)) AS amountMO, 0 AS amountEQ " & _
                    "  FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "CardInputs ptfi " & _
                    "  JOIN inputs i ON ptfi.iinputid = i.iinputid " & _
                    "  JOIN inputtypes it ON it.iinputid = i.iinputid " & _
                    "  LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) pp GROUP BY iinputid, iprojectid) pp ON ptfi.iprojectid = pp.iprojectid AND i.iinputid = pp.iinputid " & _
                    "  WHERE ptfi.iprojectid = " & iprojectid & " AND ptfi.icardid = " & icardid & " AND it.sinputtypedescription = 'MANO DE OBRA' " & _
                    "  UNION " & _
                    "  SELECT " & iprojectid & " AS iprojectid, " & icardid & " AS icardid, 'SUMA DE EQUIPO Y HERRAMIENTA', 0 AS amountMAT, 0 AS amountMO, IF(SUM(ptfi.dcardinputqty*eq.amount) IS NULL, 0, SUM(ptfi.dcardinputqty*eq.amount)) AS amountEQ " & _
                    "  FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "CardInputs ptfi " & _
                    "  JOIN (SELECT ptfi.iprojectid,  ptfi.icardid, 0 AS iinputid, SUM(ptfi.dcardinputqty*pp.dinputfinalprice) AS amount FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "CardInputs ptfi JOIN inputs i ON ptfi.iinputid = i.iinputid JOIN inputtypes it ON i.iinputid = it.iinputid LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) pp GROUP BY iinputid, iprojectid) pp ON ptfi.iprojectid = pp.iprojectid AND i.iinputid = pp.iinputid WHERE ptfi.iprojectid = " & iprojectid & " AND ptfi.icardid = " & icardid & " AND it.sinputtypedescription = 'MANO DE OBRA' GROUP BY ptfi.iprojectid, ptfi.iinputid) eq ON ptfi.iprojectid = eq.iprojectid AND ptfi.iinputid = eq.iinputid " & _
                    "  WHERE ptfi.iprojectid = " & iprojectid & " AND ptfi.icardid = " & icardid & " AND ptfi.iinputid = 0 " & _
                    "  ) AS costodirectoinner " & _
                    "  GROUP BY 1,2 " & _
                    " ) AS costodirectoouter " & _
                    "JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "Cards ptf ON costodirectoouter.iprojectid = ptf.iprojectid AND costodirectoouter.icardid = ptf.icardid " & _
                    "JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & " p ON ptf.iprojectid = p.iprojectid " & _
                    "WHERE p.iprojectid = " & iprojectid & " "

                    queryIVATarjeta = "SELECT dprojectIVApercentage FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & " WHERE iprojectid = " & iprojectid

                End If

            End If


            Dim dsCalculosTarjetas As DataSet
            Dim dsDatosTarjeta As DataSet
            Dim queryDatosTarjeta As String

            Dim iva As Double = 0.0

            iva = getSQLQueryAsDouble(0, queryIVATarjeta)

            dsCalculosTarjetas = getSQLQueryAsDataset(0, queryCalculosTarjeta)


            If IsBase = True Then

                queryDatosTarjeta = "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Cards WHERE ibaseid = " & iprojectid & " AND icardid = " & icardid

            Else

                If IsModel = True Then

                    queryDatosTarjeta = "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "Cards WHERE imodelid = " & iprojectid & " AND icardid = " & icardid

                Else

                    queryDatosTarjeta = "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "Cards WHERE iprojectid = " & iprojectid & " AND icardid = " & icardid

                End If

            End If

            dsDatosTarjeta = getSQLQueryAsDataset(0, queryDatosTarjeta)


            lblPorcentajeIVA.Text = FormatCurrency(iva * 100, 2, TriState.True, TriState.False, TriState.True).Replace("$", "") & " %"

            Dim porcentajeindirectos As Double = 0.0
            Dim porcentajeutilidades As Double = 0.0

            Try
                porcentajeindirectos = CDbl(dsDatosTarjeta.Tables(0).Rows(0).Item("dcardindirectcostspercentage"))
            Catch ex As Exception

            End Try

            Try
                porcentajeutilidades = CDbl(dsDatosTarjeta.Tables(0).Rows(0).Item("dcardgainpercentage"))
            Catch ex As Exception

            End Try

            If isUpdatingPercentages = False Then
                txtPorcentajeIndirectos.Text = FormatCurrency(porcentajeindirectos * 100, 2, TriState.True, TriState.False, TriState.True).Replace("$", "")
                txtPorcentajeUtilidades.Text = FormatCurrency(porcentajeutilidades * 100, 2, TriState.True, TriState.False, TriState.True).Replace("$", "")
            End If

            Try

                txtCostoDirecto.Text = FormatCurrency(dsCalculosTarjetas.Tables(0).Rows(0).Item("costodirecto"), 2, TriState.True, TriState.False, TriState.True).Replace("$", "")
                txtIndirectos.Text = FormatCurrency(dsCalculosTarjetas.Tables(0).Rows(0).Item("costoindirecto"), 2, TriState.True, TriState.False, TriState.True).Replace("$", "")
                txtSuma.Text = FormatCurrency(dsCalculosTarjetas.Tables(0).Rows(0).Item("suma"), 2, TriState.True, TriState.False, TriState.True).Replace("$", "")
                txtUtilidades.Text = FormatCurrency(dsCalculosTarjetas.Tables(0).Rows(0).Item("utilidad"), 2, TriState.True, TriState.False, TriState.True).Replace("$", "")
                txtSubTotal.Text = FormatCurrency(dsCalculosTarjetas.Tables(0).Rows(0).Item("subtotal"), 2, TriState.True, TriState.False, TriState.True).Replace("$", "")

                txtIVA.Text = FormatCurrency(dsCalculosTarjetas.Tables(0).Rows(0).Item("IVA"), 2, TriState.True, TriState.False, TriState.True).Replace("$", "")
                txtTotal.Text = FormatCurrency(dsCalculosTarjetas.Tables(0).Rows(0).Item("total"), 2, TriState.True, TriState.False, TriState.True).Replace("$", "")

            Catch ex As Exception

            End Try

        End If

    End Sub


    Private Sub cmbCategoriaDeLaTarjeta_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbCategoriaDeLaTarjeta.SelectedIndexChanged

        If isFormReadyForAction = True And txtCodigoDeLaTarjeta.Text.Trim = "" Then

            txtCodigoDeLaTarjeta.Text = cmbCategoriaDeLaTarjeta.SelectedValue.ToString

        End If

        txtCodigoDeLaTarjeta.Focus()
        txtCodigoDeLaTarjeta.SelectionStart() = txtCodigoDeLaTarjeta.Text.Length

    End Sub


    Private Sub btnCategorias_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCategorias.Click

        Dim bci As New BuscaCategoriasInsumo
        bci.susername = susername
        bci.bactive = bactive
        bci.bonline = bonline
        bci.suserfullname = suserfullname

        bci.suseremail = suseremail
        bci.susersession = susersession
        bci.susermachinename = susermachinename
        bci.suserip = suserip

        bci.isEdit = False

        If Me.WindowState = FormWindowState.Maximized Then
            bci.WindowState = FormWindowState.Maximized
        End If

        Me.Visible = False
        bci.ShowDialog(Me)
        Me.Visible = True

        If bci.DialogResult = Windows.Forms.DialogResult.OK Then

            cmbCategoriaDeLaTarjeta.DataSource = getSQLQueryAsDataTable(0, "SELECT scardlegacycategoryid, CONCAT(scardlegacycategoryid, ' - ', scardlegacycategorydescription) AS scardlegacycategorydescription FROM cardlegacycategories")
            cmbCategoriaDeLaTarjeta.DisplayMember = "scardlegacycategorydescription"
            cmbCategoriaDeLaTarjeta.ValueMember = "scardlegacycategoryid"
            cmbCategoriaDeLaTarjeta.SelectedIndex = -1

        End If

        Cursor.Current = System.Windows.Forms.Cursors.Default

    End Sub


    Private Sub txtCodigoDeLaTarjeta_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtCodigoDeLaTarjeta.KeyUp

        Dim strcaracteresprohibidos As String = "|°!#$%&/()=?¡*¨[]_:;,-{}+´¿'¬^`~@\<>"
        Dim arrayCaractProhib As Char() = strcaracteresprohibidos.ToCharArray
        Dim resultado As Boolean = False

        For carp = 0 To arrayCaractProhib.Length - 1

            If txtCodigoDeLaTarjeta.Text.Contains(arrayCaractProhib(carp)) Then
                txtCodigoDeLaTarjeta.Text = txtCodigoDeLaTarjeta.Text.Replace(arrayCaractProhib(carp), "")
                resultado = True
            End If

        Next carp

        If resultado = True Then
            txtCodigoDeLaTarjeta.Select(txtCodigoDeLaTarjeta.Text.Length, 0)
        End If

    End Sub


    Private Sub txtNombreDeLaTarjeta_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtNombreDeLaTarjeta.KeyUp

        Dim strcaracteresprohibidos As String = "|@'\"""
        Dim arrayCaractProhib As Char() = strcaracteresprohibidos.ToCharArray
        Dim resultado As Boolean = False

        For carp = 0 To arrayCaractProhib.Length - 1

            If txtNombreDeLaTarjeta.Text.Contains(arrayCaractProhib(carp)) Then
                txtNombreDeLaTarjeta.Text = txtNombreDeLaTarjeta.Text.Replace(arrayCaractProhib(carp), "")
                resultado = True
            End If

        Next carp

        txtNombreDeLaTarjeta.Text = txtNombreDeLaTarjeta.Text.Replace("--", "").Replace("  ", " ").Replace("'", " PIES").Replace("\""", " PULGADAS")

        If resultado = True Then
            txtNombreDeLaTarjeta.Select(txtNombreDeLaTarjeta.Text.Length, 0)
        End If

    End Sub


    Private Sub txtUnidadDeMedida_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtUnidadDeMedida.KeyUp

        Dim strcaracteresprohibidos As String = "|°!#$%&/()=?¡*¨[]_:;,-{}+´¿'¬^`~@\<>"
        Dim arrayCaractProhib As Char() = strcaracteresprohibidos.ToCharArray
        Dim resultado As Boolean = False

        For carp = 0 To arrayCaractProhib.Length - 1

            If txtUnidadDeMedida.Text.Contains(arrayCaractProhib(carp)) Then
                txtUnidadDeMedida.Text = txtUnidadDeMedida.Text.Replace(arrayCaractProhib(carp), "")
                resultado = True
            End If

        Next carp

        If resultado = True Then
            txtUnidadDeMedida.Select(txtUnidadDeMedida.Text.Length, 0)
        End If

    End Sub


    Private Sub txtCodigoDeLaTarjeta_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtCodigoDeLaTarjeta.LostFocus

        txtCodigoDeLaTarjeta.Text = txtCodigoDeLaTarjeta.Text.ToUpper

    End Sub


    Private Sub txtUnidadDeMedida_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtUnidadDeMedida.LostFocus

        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        If IsHistoric = True Then
            Exit Sub
        End If

        Dim unitfound As Boolean = False
        Dim strcaracteresprohibidos As String = "|°!#$%&/()=?¡*¨[]_:;,.-{}+´¿'¬^`~@\<> "

        txtUnidadDeMedida.Text = txtUnidadDeMedida.Text.Trim.Trim(strcaracteresprohibidos.ToCharArray)
        txtUnidadDeMedida.Text = txtUnidadDeMedida.Text.ToUpper

        unitfound = getSQLQueryAsBoolean(0, "SELECT count(*) FROM transformationunits WHERE soriginunit = '" & txtUnidadDeMedida.Text.Replace("--", "") & "'")

        If unitfound = False Then

            MsgBox("¡No encontré esa unidad de Medida! ¿Podrías seleccionar una de la lista?", MsgBoxStyle.OkOnly, "Unidad de Medida No Encontrada")

            Dim bu As New BuscaUnidades
            bu.susername = susername
            bu.bactive = bactive
            bu.bonline = bonline
            bu.suserfullname = suserfullname

            bu.suseremail = suseremail
            bu.susersession = susersession
            bu.susermachinename = susermachinename
            bu.suserip = suserip

            bu.querystring = txtUnidadDeMedida.Text

            bu.isEdit = False

            If Me.WindowState = FormWindowState.Maximized Then
                bu.WindowState = FormWindowState.Maximized
            End If

            Me.Visible = False
            bu.ShowDialog(Me)
            Me.Visible = True

            If bu.DialogResult = Windows.Forms.DialogResult.OK Then

                txtUnidadDeMedida.Text = bu.sunit1
                unitfound = True

            Else

                txtUnidadDeMedida.Text = ""
                txtUnidadDeMedida.Focus()

            End If

        End If

        Cursor.Current = System.Windows.Forms.Cursors.Default

    End Sub


    Private Sub txtUnidadDeMedida_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtUnidadDeMedida.TextChanged

        If saveCardPermission = False Then
            Exit Sub
        End If

        If isFormReadyForAction = False Then
            Exit Sub
        End If

        If txtUnidadDeMedida.Text.Contains("'") = True Then
            txtUnidadDeMedida.Text = txtUnidadDeMedida.Text.Replace("'", "")
        End If

        If validaDatosInicialesTarjeta(True, False) = False Then
            Cursor.Current = System.Windows.Forms.Cursors.Default
            Exit Sub
        Else
            If icardid <> 0 Then
                dgvConceptosTarjeta.Enabled = True
                btnNuevoInsumo.Enabled = True
                btnInsertarInsumo.Enabled = True
                btnEliminarInsumo.Enabled = True
            End If
        End If

        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        Dim valorIndirectos As Double = 0.0
        Try
            valorIndirectos = CDbl(txtPorcentajeIndirectos.Text)
        Catch ex As Exception

        End Try

        Dim valorUtilidades As Double = 0.0
        Try
            valorUtilidades = CDbl(txtPorcentajeUtilidades.Text)
        Catch ex As Exception

        End Try


        Dim baseid As Integer = 0
        baseid = getSQLQueryAsInteger(0, "SELECT ibaseid FROM base ORDER BY iupdatedate DESC, supdatetime DESC LIMIT 1")

        If baseid = 0 Then
            baseid = 99999
        End If

        Dim fecha As Integer = 0
        Dim hora As String = ""

        fecha = getMySQLDate()
        hora = getAppTime()

        If IsEdit = True Then

            If IsBase = True Then

                executeSQLCommand(0, "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Cards SET iupdatedate = " & fecha & ", supdatetime = '" & hora & "', supdateusername = '" & susername & "', scardlegacycategoryid = '" & cmbCategoriaDeLaTarjeta.SelectedValue.ToString & "', scardlegacyid = '" & txtCodigoDeLaTarjeta.Text & "', scarddescription = '" & txtNombreDeLaTarjeta.Text & "', scardunit = '" & txtUnidadDeMedida.Text & "', dcardindirectcostspercentage = " & (valorIndirectos / 100) & ", dcardgainpercentage = " & (valorUtilidades / 100) & " WHERE ibaseid = " & iprojectid & " AND icardid = " & icardid)

            Else

                If IsModel = True Then

                    executeSQLCommand(0, "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "Cards SET iupdatedate = " & fecha & ", supdatetime = '" & hora & "', supdateusername = '" & susername & "', scardlegacycategoryid = '" & cmbCategoriaDeLaTarjeta.SelectedValue.ToString & "', scardlegacyid = '" & txtCodigoDeLaTarjeta.Text & "', scarddescription = '" & txtNombreDeLaTarjeta.Text & "', scardunit = '" & txtUnidadDeMedida.Text & "', dcardindirectcostspercentage = " & (valorIndirectos / 100) & ", dcardgainpercentage = " & (valorUtilidades / 100) & " WHERE imodelid = " & iprojectid & " AND icardid = " & icardid)

                Else

                    executeSQLCommand(0, "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "Cards SET iupdatedate = " & fecha & ", supdatetime = '" & hora & "', supdateusername = '" & susername & "', scardlegacycategoryid = '" & cmbCategoriaDeLaTarjeta.SelectedValue.ToString & "', scardlegacyid = '" & txtCodigoDeLaTarjeta.Text & "', scarddescription = '" & txtNombreDeLaTarjeta.Text & "', scardunit = '" & txtUnidadDeMedida.Text & "', dcardindirectcostspercentage = " & (valorIndirectos / 100) & ", dcardgainpercentage = " & (valorUtilidades / 100) & " WHERE iprojectid = " & iprojectid & " AND icardid = " & icardid)

                End If

            End If

        Else

            Dim checkIfItsOnlyTextUpdate As Boolean = False

            checkIfItsOnlyTextUpdate = getSQLQueryAsBoolean(0, "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Cards WHERE ibaseid = " & baseid & " AND icardid = " & icardid)

            If checkIfItsOnlyTextUpdate = True Then

                Dim queriesUpdate(2) As String

                If IsBase = True Then

                    queriesUpdate(0) = "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Cards SET iupdatedate = " & fecha & ", supdatetime = '" & hora & "', supdateusername = '" & susername & "', scardlegacycategoryid = '" & cmbCategoriaDeLaTarjeta.SelectedValue.ToString & "', scardlegacyid = '" & txtCodigoDeLaTarjeta.Text & "', scarddescription = '" & txtNombreDeLaTarjeta.Text & "', scardunit = '" & txtUnidadDeMedida.Text & "', dcardindirectcostspercentage = " & (valorIndirectos / 100) & ", dcardgainpercentage = " & (valorUtilidades / 100) & " WHERE ibaseid = " & baseid & " AND icardid = " & icardid

                Else

                    If IsModel = True Then

                        queriesUpdate(0) = "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Cards SET iupdatedate = " & fecha & ", supdatetime = '" & hora & "', supdateusername = '" & susername & "', scardlegacycategoryid = '" & cmbCategoriaDeLaTarjeta.SelectedValue.ToString & "', scardlegacyid = '" & txtCodigoDeLaTarjeta.Text & "', scarddescription = '" & txtNombreDeLaTarjeta.Text & "', scardunit = '" & txtUnidadDeMedida.Text & "', dcardindirectcostspercentage = " & (valorIndirectos / 100) & ", dcardgainpercentage = " & (valorUtilidades / 100) & " WHERE ibaseid = " & baseid & " AND icardid = " & icardid
                        queriesUpdate(1) = "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "Cards SET iupdatedate = " & fecha & ", supdatetime = '" & hora & "', supdateusername = '" & susername & "', scardlegacycategoryid = '" & cmbCategoriaDeLaTarjeta.SelectedValue.ToString & "', scardlegacyid = '" & txtCodigoDeLaTarjeta.Text & "', scarddescription = '" & txtNombreDeLaTarjeta.Text & "', scardunit = '" & txtUnidadDeMedida.Text & "', dcardindirectcostspercentage = " & (valorIndirectos / 100) & ", dcardgainpercentage = " & (valorUtilidades / 100) & " WHERE imodelid = " & iprojectid & " AND icardid = " & icardid

                    Else

                        queriesUpdate(0) = "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Cards SET iupdatedate = " & fecha & ", supdatetime = '" & hora & "', supdateusername = '" & susername & "', scardlegacycategoryid = '" & cmbCategoriaDeLaTarjeta.SelectedValue.ToString & "', scardlegacyid = '" & txtCodigoDeLaTarjeta.Text & "', scarddescription = '" & txtNombreDeLaTarjeta.Text & "', scardunit = '" & txtUnidadDeMedida.Text & "', dcardindirectcostspercentage = " & (valorIndirectos / 100) & ", dcardgainpercentage = " & (valorUtilidades / 100) & " WHERE ibaseid = " & baseid & " AND icardid = " & icardid
                        queriesUpdate(1) = "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "Cards SET iupdatedate = " & fecha & ", supdatetime = '" & hora & "', supdateusername = '" & susername & "', scardlegacycategoryid = '" & cmbCategoriaDeLaTarjeta.SelectedValue.ToString & "', scardlegacyid = '" & txtCodigoDeLaTarjeta.Text & "', scarddescription = '" & txtNombreDeLaTarjeta.Text & "', scardunit = '" & txtUnidadDeMedida.Text & "', dcardindirectcostspercentage = " & (valorIndirectos / 100) & ", dcardgainpercentage = " & (valorUtilidades / 100) & " WHERE iprojectid = " & iprojectid & " AND icardid = " & icardid

                    End If

                End If

                executeTransactedSQLCommand(0, queriesUpdate)

            Else

                icardid = getSQLQueryAsInteger(0, "SELECT IF(MAX(icardid) + 1 IS NULL, 1, MAX(icardid) + 1) AS icardid FROM basecards WHERE ibaseid = " & baseid)
                scardlegacyid = txtCodigoDeLaTarjeta.Text
                scarddescription = txtNombreDeLaTarjeta.Text

                Dim queriesInsert(2) As String

                If IsBase = True Then

                    queriesInsert(0) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Cards VALUES (" & baseid & ", " & icardid & ", '" & cmbCategoriaDeLaTarjeta.SelectedValue & "', '" & txtCodigoDeLaTarjeta.Text & "', '" & txtNombreDeLaTarjeta.Text & "', '" & txtUnidadDeMedida.Text & "', 1, " & (valorIndirectos / 100) & ", " & (valorUtilidades / 100) & ", " & fecha & ", '" & hora & "', '" & susername & "')"

                Else

                    If IsModel = True Then

                        queriesInsert(0) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Cards VALUES (" & baseid & ", " & icardid & ", '" & cmbCategoriaDeLaTarjeta.SelectedValue & "', '" & txtCodigoDeLaTarjeta.Text & "', '" & txtNombreDeLaTarjeta.Text & "', '" & txtUnidadDeMedida.Text & "', 1, " & (valorIndirectos / 100) & ", " & (valorUtilidades / 100) & ", " & fecha & ", '" & hora & "', '" & susername & "')"
                        queriesInsert(1) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "Cards VALUES (" & iprojectid & ", " & icardid & ", '" & cmbCategoriaDeLaTarjeta.SelectedValue & "', '" & txtCodigoDeLaTarjeta.Text & "', '" & txtNombreDeLaTarjeta.Text & "', '" & txtUnidadDeMedida.Text & "', 1, " & (valorIndirectos / 100) & ", " & (valorUtilidades / 100) & ", " & fecha & ", '" & hora & "', '" & susername & "')"

                    Else

                        queriesInsert(0) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Cards VALUES (" & baseid & ", " & icardid & ", '" & cmbCategoriaDeLaTarjeta.SelectedValue & "', '" & txtCodigoDeLaTarjeta.Text & "', '" & txtNombreDeLaTarjeta.Text & "', '" & txtUnidadDeMedida.Text & "', 1, " & (valorIndirectos / 100) & ", " & (valorUtilidades / 100) & ", " & fecha & ", '" & hora & "', '" & susername & "')"
                        queriesInsert(1) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "Cards VALUES (" & iprojectid & ", " & icardid & ", '" & cmbCategoriaDeLaTarjeta.SelectedValue & "', '" & txtCodigoDeLaTarjeta.Text & "', '" & txtNombreDeLaTarjeta.Text & "', '" & txtUnidadDeMedida.Text & "', 1, " & (valorIndirectos / 100) & ", " & (valorUtilidades / 100) & ", " & fecha & ", '" & hora & "', '" & susername & "')"

                    End If

                End If

                executeTransactedSQLCommand(0, queriesInsert)

            End If

        End If

        Cursor.Current = System.Windows.Forms.Cursors.Default

    End Sub


    Private Sub txtCodigoDeLaTarjeta_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtCodigoDeLaTarjeta.TextChanged

        Cursor.Current = System.Windows.Forms.Cursors.Default

        If txtCodigoDeLaTarjeta.Text.Contains("'") = True Then
            txtCodigoDeLaTarjeta.Text = txtCodigoDeLaTarjeta.Text.Replace("'", "")
        End If

        If txtCodigoDeLaTarjeta.Text.Length > 0 Then

            lblAvailability.Visible = True

            Dim queryCheck As String = ""

            If IsBase = True Then
                queryCheck = "SELECT scardlegacyid FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Cards WHERE icardid = " & icardid
            Else

                If IsModel = True Then
                    queryCheck = "SELECT scardlegacyid FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "Cards WHERE icardid = " & icardid
                Else
                    queryCheck = "SELECT scardlegacyid FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "Cards WHERE icardid = " & icardid
                End If

            End If


            If IsEdit = False And getSQLQueryAsInteger(0, "SELECT COUNT(*) FROM basecards WHERE scardlegacyid = '" & txtCodigoDeLaTarjeta.Text.ToUpper & "'") >= 1 Then

                lblAvailability.Text = "No Disponible"
                lblAvailability.ForeColor = Color.Red

            ElseIf IsEdit = False And getSQLQueryAsInteger(0, "SELECT COUNT(*) FROM basecards WHERE scardlegacyid = '" & txtCodigoDeLaTarjeta.Text.ToUpper & "'") = 0 Then

                Dim variableQuery As String = ""

                If IsBase = True Then
                    variableQuery = "SELECT COUNT(*) FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Cards WHERE scardlegacyid = '" & txtCodigoDeLaTarjeta.Text.ToUpper & "'"
                Else
                    If IsModel = True Then
                        variableQuery = "SELECT COUNT(*) FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "Cards WHERE scardlegacyid = '" & txtCodigoDeLaTarjeta.Text.ToUpper & "'"
                    Else
                        variableQuery = "SELECT COUNT(*) FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "Cards WHERE scardlegacyid = '" & txtCodigoDeLaTarjeta.Text.ToUpper & "'"
                    End If
                End If

                If getSQLQueryAsInteger(0, variableQuery) >= 1 Then

                    lblAvailability.Text = "No Disponible"
                    lblAvailability.ForeColor = Color.Red
                Else

                    lblAvailability.Text = "Disponible"
                    lblAvailability.ForeColor = Color.ForestGreen

                End If

            ElseIf IsEdit = True And txtCodigoDeLaTarjeta.Text <> getSQLQueryAsString(0, queryCheck) Then

                If getSQLQueryAsInteger(0, "SELECT COUNT(*) FROM basecards WHERE scardlegacyid = '" & txtCodigoDeLaTarjeta.Text.ToUpper & "'") >= 1 Then

                    lblAvailability.Text = "No Disponible"
                    lblAvailability.ForeColor = Color.Red

                ElseIf getSQLQueryAsInteger(0, "SELECT COUNT(*) FROM basecards WHERE scardlegacyid = '" & txtCodigoDeLaTarjeta.Text.ToUpper & "'") = 0 Then

                    Dim variableQuery As String = ""

                    If IsBase = True Then
                        variableQuery = "SELECT COUNT(*) FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Cards WHERE scardlegacyid = '" & txtCodigoDeLaTarjeta.Text.ToUpper & "'"
                    Else
                        If IsModel = True Then
                            variableQuery = "SELECT COUNT(*) FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "Cards WHERE scardlegacyid = '" & txtCodigoDeLaTarjeta.Text.ToUpper & "'"
                        Else
                            variableQuery = "SELECT COUNT(*) FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "Cards WHERE scardlegacyid = '" & txtCodigoDeLaTarjeta.Text.ToUpper & "'"
                        End If
                    End If

                    If getSQLQueryAsInteger(0, variableQuery) >= 1 Then

                        lblAvailability.Text = "No Disponible"
                        lblAvailability.ForeColor = Color.Red
                    Else

                        lblAvailability.Text = "Disponible"
                        lblAvailability.ForeColor = Color.ForestGreen

                    End If

                End If

            ElseIf IsEdit = True And txtCodigoDeLaTarjeta.Text = getSQLQueryAsString(0, queryCheck) Then

                lblAvailability.Visible = False

            End If

        Else

            lblAvailability.Visible = False

        End If


        Cursor.Current = System.Windows.Forms.Cursors.Default


        'Inicia Save


        If saveCardPermission = False Then
            Exit Sub
        End If

        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        If isFormReadyForAction = False Then
            Exit Sub
        End If

        If validaDatosInicialesTarjeta(True, False) = False Then
            Cursor.Current = System.Windows.Forms.Cursors.Default
            Exit Sub
        Else

            If icardid <> 0 Then
                dgvConceptosTarjeta.Enabled = True
                btnNuevoInsumo.Enabled = True
                btnInsertarInsumo.Enabled = True
                btnEliminarInsumo.Enabled = True
            End If

        End If


        Dim valorIndirectos As Double = 0.0
        Try
            valorIndirectos = CDbl(txtPorcentajeIndirectos.Text)
        Catch ex As Exception

        End Try

        Dim valorUtilidades As Double = 0.0
        Try
            valorUtilidades = CDbl(txtPorcentajeUtilidades.Text)
        Catch ex As Exception

        End Try


        Dim baseid As Integer = 0
        baseid = getSQLQueryAsInteger(0, "SELECT ibaseid FROM base ORDER BY iupdatedate DESC, supdatetime DESC LIMIT 1")

        If baseid = 0 Then
            baseid = 99999
        End If

        Dim fecha As Integer = 0
        Dim hora As String = ""

        fecha = getMySQLDate()
        hora = getAppTime()

        If IsEdit = True Then

            If IsBase = True Then

                executeSQLCommand(0, "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Cards SET iupdatedate = " & fecha & ", supdatetime = '" & hora & "', supdateusername = '" & susername & "', scardlegacycategoryid = '" & cmbCategoriaDeLaTarjeta.SelectedValue.ToString & "', scardlegacyid = '" & txtCodigoDeLaTarjeta.Text & "', scarddescription = '" & txtNombreDeLaTarjeta.Text & "', scardunit = '" & txtUnidadDeMedida.Text & "', dcardindirectcostspercentage = " & (valorIndirectos / 100) & ", dcardgainpercentage = " & (valorUtilidades / 100) & " WHERE ibaseid = " & iprojectid & " AND icardid = " & icardid)

            Else

                If IsModel = True Then

                    executeSQLCommand(0, "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "Cards SET iupdatedate = " & fecha & ", supdatetime = '" & hora & "', supdateusername = '" & susername & "', scardlegacycategoryid = '" & cmbCategoriaDeLaTarjeta.SelectedValue.ToString & "', scardlegacyid = '" & txtCodigoDeLaTarjeta.Text & "', scarddescription = '" & txtNombreDeLaTarjeta.Text & "', scardunit = '" & txtUnidadDeMedida.Text & "', dcardindirectcostspercentage = " & (valorIndirectos / 100) & ", dcardgainpercentage = " & (valorUtilidades / 100) & " WHERE imodelid = " & iprojectid & " AND icardid = " & icardid)

                Else

                    executeSQLCommand(0, "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "Cards SET iupdatedate = " & fecha & ", supdatetime = '" & hora & "', supdateusername = '" & susername & "', scardlegacycategoryid = '" & cmbCategoriaDeLaTarjeta.SelectedValue.ToString & "', scardlegacyid = '" & txtCodigoDeLaTarjeta.Text & "', scarddescription = '" & txtNombreDeLaTarjeta.Text & "', scardunit = '" & txtUnidadDeMedida.Text & "', dcardindirectcostspercentage = " & (valorIndirectos / 100) & ", dcardgainpercentage = " & (valorUtilidades / 100) & " WHERE iprojectid = " & iprojectid & " AND icardid = " & icardid)

                End If

            End If

        Else

            Dim checkIfItsOnlyTextUpdate As Boolean = False

            checkIfItsOnlyTextUpdate = getSQLQueryAsBoolean(0, "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Cards WHERE ibaseid = " & baseid & " AND icardid = " & icardid)

            If checkIfItsOnlyTextUpdate = True Then

                Dim queriesUpdate(2) As String

                If IsBase = True Then

                    queriesUpdate(0) = "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Cards SET iupdatedate = " & fecha & ", supdatetime = '" & hora & "', supdateusername = '" & susername & "', scardlegacycategoryid = '" & cmbCategoriaDeLaTarjeta.SelectedValue.ToString & "', scardlegacyid = '" & txtCodigoDeLaTarjeta.Text & "', scarddescription = '" & txtNombreDeLaTarjeta.Text & "', scardunit = '" & txtUnidadDeMedida.Text & "', dcardindirectcostspercentage = " & (valorIndirectos / 100) & ", dcardgainpercentage = " & (valorUtilidades / 100) & " WHERE ibaseid = " & baseid & " AND icardid = " & icardid

                Else

                    If IsModel = True Then

                        queriesUpdate(0) = "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Cards SET iupdatedate = " & fecha & ", supdatetime = '" & hora & "', supdateusername = '" & susername & "', scardlegacycategoryid = '" & cmbCategoriaDeLaTarjeta.SelectedValue.ToString & "', scardlegacyid = '" & txtCodigoDeLaTarjeta.Text & "', scarddescription = '" & txtNombreDeLaTarjeta.Text & "', scardunit = '" & txtUnidadDeMedida.Text & "', dcardindirectcostspercentage = " & (valorIndirectos / 100) & ", dcardgainpercentage = " & (valorUtilidades / 100) & " WHERE ibaseid = " & baseid & " AND icardid = " & icardid
                        queriesUpdate(1) = "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "Cards SET iupdatedate = " & fecha & ", supdatetime = '" & hora & "', supdateusername = '" & susername & "', scardlegacycategoryid = '" & cmbCategoriaDeLaTarjeta.SelectedValue.ToString & "', scardlegacyid = '" & txtCodigoDeLaTarjeta.Text & "', scarddescription = '" & txtNombreDeLaTarjeta.Text & "', scardunit = '" & txtUnidadDeMedida.Text & "', dcardindirectcostspercentage = " & (valorIndirectos / 100) & ", dcardgainpercentage = " & (valorUtilidades / 100) & " WHERE imodelid = " & iprojectid & " AND icardid = " & icardid

                    Else

                        queriesUpdate(0) = "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Cards SET iupdatedate = " & fecha & ", supdatetime = '" & hora & "', supdateusername = '" & susername & "', scardlegacycategoryid = '" & cmbCategoriaDeLaTarjeta.SelectedValue.ToString & "', scardlegacyid = '" & txtCodigoDeLaTarjeta.Text & "', scarddescription = '" & txtNombreDeLaTarjeta.Text & "', scardunit = '" & txtUnidadDeMedida.Text & "', dcardindirectcostspercentage = " & (valorIndirectos / 100) & ", dcardgainpercentage = " & (valorUtilidades / 100) & " WHERE ibaseid = " & baseid & " AND icardid = " & icardid
                        queriesUpdate(1) = "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "Cards SET iupdatedate = " & fecha & ", supdatetime = '" & hora & "', supdateusername = '" & susername & "', scardlegacycategoryid = '" & cmbCategoriaDeLaTarjeta.SelectedValue.ToString & "', scardlegacyid = '" & txtCodigoDeLaTarjeta.Text & "', scarddescription = '" & txtNombreDeLaTarjeta.Text & "', scardunit = '" & txtUnidadDeMedida.Text & "', dcardindirectcostspercentage = " & (valorIndirectos / 100) & ", dcardgainpercentage = " & (valorUtilidades / 100) & " WHERE iprojectid = " & iprojectid & " AND icardid = " & icardid

                    End If

                End If

                executeTransactedSQLCommand(0, queriesUpdate)

            Else

                icardid = getSQLQueryAsInteger(0, "SELECT IF(MAX(icardid) + 1 IS NULL, 1, MAX(icardid) + 1) AS icardid FROM basecards WHERE ibaseid = " & baseid)
                scardlegacyid = txtCodigoDeLaTarjeta.Text
                scarddescription = txtNombreDeLaTarjeta.Text

                Dim queriesInsert(2) As String

                If IsBase = True Then

                    queriesInsert(0) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Cards VALUES (" & baseid & ", " & icardid & ", '" & cmbCategoriaDeLaTarjeta.SelectedValue & "', '" & txtCodigoDeLaTarjeta.Text & "', '" & txtNombreDeLaTarjeta.Text & "', '" & txtUnidadDeMedida.Text & "', 1, " & (valorIndirectos / 100) & ", " & (valorUtilidades / 100) & ", " & fecha & ", '" & hora & "', '" & susername & "')"

                Else

                    If IsModel = True Then

                        queriesInsert(0) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Cards VALUES (" & baseid & ", " & icardid & ", '" & cmbCategoriaDeLaTarjeta.SelectedValue & "', '" & txtCodigoDeLaTarjeta.Text & "', '" & txtNombreDeLaTarjeta.Text & "', '" & txtUnidadDeMedida.Text & "', 1, " & (valorIndirectos / 100) & ", " & (valorUtilidades / 100) & ", " & fecha & ", '" & hora & "', '" & susername & "')"
                        queriesInsert(1) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "Cards VALUES (" & iprojectid & ", " & icardid & ", '" & cmbCategoriaDeLaTarjeta.SelectedValue & "', '" & txtCodigoDeLaTarjeta.Text & "', '" & txtNombreDeLaTarjeta.Text & "', '" & txtUnidadDeMedida.Text & "', 1, " & (valorIndirectos / 100) & ", " & (valorUtilidades / 100) & ", " & fecha & ", '" & hora & "', '" & susername & "')"

                    Else

                        queriesInsert(0) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Cards VALUES (" & baseid & ", " & icardid & ", '" & cmbCategoriaDeLaTarjeta.SelectedValue & "', '" & txtCodigoDeLaTarjeta.Text & "', '" & txtNombreDeLaTarjeta.Text & "', '" & txtUnidadDeMedida.Text & "', 1, " & (valorIndirectos / 100) & ", " & (valorUtilidades / 100) & ", " & fecha & ", '" & hora & "', '" & susername & "')"
                        queriesInsert(1) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "Cards VALUES (" & iprojectid & ", " & icardid & ", '" & cmbCategoriaDeLaTarjeta.SelectedValue & "', '" & txtCodigoDeLaTarjeta.Text & "', '" & txtNombreDeLaTarjeta.Text & "', '" & txtUnidadDeMedida.Text & "', 1, " & (valorIndirectos / 100) & ", " & (valorUtilidades / 100) & ", " & fecha & ", '" & hora & "', '" & susername & "')"

                    End If

                End If

                executeTransactedSQLCommand(0, queriesInsert)

            End If

        End If

        Cursor.Current = System.Windows.Forms.Cursors.Default


    End Sub


    Private Sub txtNombreDeLaTarjeta_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtNombreDeLaTarjeta.TextChanged

        If saveCardPermission = False Then
            Exit Sub
        End If

        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        If isFormReadyForAction = False Then
            Exit Sub
        End If

        If validaDatosInicialesTarjeta(True, False) = False Then
            Cursor.Current = System.Windows.Forms.Cursors.Default
            Exit Sub
        Else
            If icardid <> 0 Then
                dgvConceptosTarjeta.Enabled = True
                btnNuevoInsumo.Enabled = True
                btnInsertarInsumo.Enabled = True
                btnEliminarInsumo.Enabled = True
            End If
        End If

        Dim valorIndirectos As Double = 0.0
        Try
            valorIndirectos = CDbl(txtPorcentajeIndirectos.Text)
        Catch ex As Exception

        End Try

        Dim valorUtilidades As Double = 0.0
        Try
            valorUtilidades = CDbl(txtPorcentajeUtilidades.Text)
        Catch ex As Exception

        End Try


        Dim baseid As Integer = 0
        baseid = getSQLQueryAsInteger(0, "SELECT ibaseid FROM base ORDER BY iupdatedate DESC, supdatetime DESC LIMIT 1")

        If baseid = 0 Then
            baseid = 99999
        End If

        Dim fecha As Integer = 0
        Dim hora As String = ""

        fecha = getMySQLDate()
        hora = getAppTime()

        If IsEdit = True Then

            If IsBase = True Then

                executeSQLCommand(0, "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Cards SET iupdatedate = " & fecha & ", supdatetime = '" & hora & "', supdateusername = '" & susername & "', scardlegacycategoryid = '" & cmbCategoriaDeLaTarjeta.SelectedValue.ToString & "', scardlegacyid = '" & txtCodigoDeLaTarjeta.Text & "', scarddescription = '" & txtNombreDeLaTarjeta.Text.Replace("'", " PIES").Replace("\""", " PULGADAS") & "', scardunit = '" & txtUnidadDeMedida.Text & "', dcardindirectcostspercentage = " & (valorIndirectos / 100) & ", dcardgainpercentage = " & (valorUtilidades / 100) & " WHERE ibaseid = " & iprojectid & " AND icardid = " & icardid)

            Else

                If IsModel = True Then

                    executeSQLCommand(0, "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "Cards SET iupdatedate = " & fecha & ", supdatetime = '" & hora & "', supdateusername = '" & susername & "', scardlegacycategoryid = '" & cmbCategoriaDeLaTarjeta.SelectedValue.ToString & "', scardlegacyid = '" & txtCodigoDeLaTarjeta.Text & "', scarddescription = '" & txtNombreDeLaTarjeta.Text.Replace("'", " PIES").Replace("\""", " PULGADAS") & "', scardunit = '" & txtUnidadDeMedida.Text & "', dcardindirectcostspercentage = " & (valorIndirectos / 100) & ", dcardgainpercentage = " & (valorUtilidades / 100) & " WHERE imodelid = " & iprojectid & " AND icardid = " & icardid)

                Else

                    executeSQLCommand(0, "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "Cards SET iupdatedate = " & fecha & ", supdatetime = '" & hora & "', supdateusername = '" & susername & "', scardlegacycategoryid = '" & cmbCategoriaDeLaTarjeta.SelectedValue.ToString & "', scardlegacyid = '" & txtCodigoDeLaTarjeta.Text & "', scarddescription = '" & txtNombreDeLaTarjeta.Text & "', scardunit = '" & txtUnidadDeMedida.Text.Replace("'", " PIES").Replace("\""", " PULGADAS") & "', dcardindirectcostspercentage = " & (valorIndirectos / 100) & ", dcardgainpercentage = " & (valorUtilidades / 100) & " WHERE iprojectid = " & iprojectid & " AND icardid = " & icardid)

                End If

            End If

        Else

            Dim checkIfItsOnlyTextUpdate As Boolean = False

            checkIfItsOnlyTextUpdate = getSQLQueryAsBoolean(0, "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Cards WHERE ibaseid = " & baseid & " AND icardid = " & icardid)

            If checkIfItsOnlyTextUpdate = True Then

                Dim queriesUpdate(2) As String

                If IsBase = True Then

                    queriesUpdate(0) = "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Cards SET iupdatedate = " & fecha & ", supdatetime = '" & hora & "', supdateusername = '" & susername & "', scardlegacycategoryid = '" & cmbCategoriaDeLaTarjeta.SelectedValue.ToString & "', scardlegacyid = '" & txtCodigoDeLaTarjeta.Text & "', scarddescription = '" & txtNombreDeLaTarjeta.Text & "', scardunit = '" & txtUnidadDeMedida.Text & "', dcardindirectcostspercentage = " & (valorIndirectos / 100) & ", dcardgainpercentage = " & (valorUtilidades / 100) & " WHERE ibaseid = " & baseid & " AND icardid = " & icardid

                Else

                    If IsModel = True Then

                        queriesUpdate(0) = "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Cards SET iupdatedate = " & fecha & ", supdatetime = '" & hora & "', supdateusername = '" & susername & "', scardlegacycategoryid = '" & cmbCategoriaDeLaTarjeta.SelectedValue.ToString & "', scardlegacyid = '" & txtCodigoDeLaTarjeta.Text & "', scarddescription = '" & txtNombreDeLaTarjeta.Text & "', scardunit = '" & txtUnidadDeMedida.Text & "', dcardindirectcostspercentage = " & (valorIndirectos / 100) & ", dcardgainpercentage = " & (valorUtilidades / 100) & " WHERE ibaseid = " & baseid & " AND icardid = " & icardid
                        queriesUpdate(1) = "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "Cards SET iupdatedate = " & fecha & ", supdatetime = '" & hora & "', supdateusername = '" & susername & "', scardlegacycategoryid = '" & cmbCategoriaDeLaTarjeta.SelectedValue.ToString & "', scardlegacyid = '" & txtCodigoDeLaTarjeta.Text & "', scarddescription = '" & txtNombreDeLaTarjeta.Text & "', scardunit = '" & txtUnidadDeMedida.Text & "', dcardindirectcostspercentage = " & (valorIndirectos / 100) & ", dcardgainpercentage = " & (valorUtilidades / 100) & " WHERE imodelid = " & iprojectid & " AND icardid = " & icardid

                    Else

                        queriesUpdate(0) = "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Cards SET iupdatedate = " & fecha & ", supdatetime = '" & hora & "', supdateusername = '" & susername & "', scardlegacycategoryid = '" & cmbCategoriaDeLaTarjeta.SelectedValue.ToString & "', scardlegacyid = '" & txtCodigoDeLaTarjeta.Text & "', scarddescription = '" & txtNombreDeLaTarjeta.Text & "', scardunit = '" & txtUnidadDeMedida.Text & "', dcardindirectcostspercentage = " & (valorIndirectos / 100) & ", dcardgainpercentage = " & (valorUtilidades / 100) & " WHERE ibaseid = " & baseid & " AND icardid = " & icardid
                        queriesUpdate(1) = "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "Cards SET iupdatedate = " & fecha & ", supdatetime = '" & hora & "', supdateusername = '" & susername & "', scardlegacycategoryid = '" & cmbCategoriaDeLaTarjeta.SelectedValue.ToString & "', scardlegacyid = '" & txtCodigoDeLaTarjeta.Text & "', scarddescription = '" & txtNombreDeLaTarjeta.Text & "', scardunit = '" & txtUnidadDeMedida.Text & "', dcardindirectcostspercentage = " & (valorIndirectos / 100) & ", dcardgainpercentage = " & (valorUtilidades / 100) & " WHERE iprojectid = " & iprojectid & " AND icardid = " & icardid

                    End If

                End If

                executeTransactedSQLCommand(0, queriesUpdate)

            Else

                icardid = getSQLQueryAsInteger(0, "SELECT IF(MAX(icardid) + 1 IS NULL, 1, MAX(icardid) + 1) AS icardid FROM basecards WHERE ibaseid = " & baseid)
                scardlegacyid = txtCodigoDeLaTarjeta.Text
                scarddescription = txtNombreDeLaTarjeta.Text

                Dim queriesInsert(2) As String

                If IsBase = True Then

                    queriesInsert(0) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Cards VALUES (" & baseid & ", " & icardid & ", '" & cmbCategoriaDeLaTarjeta.SelectedValue & "', '" & txtCodigoDeLaTarjeta.Text & "', '" & txtNombreDeLaTarjeta.Text & "', '" & txtUnidadDeMedida.Text & "', 1, " & (valorIndirectos / 100) & ", " & (valorUtilidades / 100) & ", " & fecha & ", '" & hora & "', '" & susername & "')"

                Else

                    If IsModel = True Then

                        queriesInsert(0) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Cards VALUES (" & baseid & ", " & icardid & ", '" & cmbCategoriaDeLaTarjeta.SelectedValue & "', '" & txtCodigoDeLaTarjeta.Text & "', '" & txtNombreDeLaTarjeta.Text & "', '" & txtUnidadDeMedida.Text & "', 1, " & (valorIndirectos / 100) & ", " & (valorUtilidades / 100) & ", " & fecha & ", '" & hora & "', '" & susername & "')"
                        queriesInsert(1) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "Cards VALUES (" & iprojectid & ", " & icardid & ", '" & cmbCategoriaDeLaTarjeta.SelectedValue & "', '" & txtCodigoDeLaTarjeta.Text & "', '" & txtNombreDeLaTarjeta.Text & "', '" & txtUnidadDeMedida.Text & "', 1, " & (valorIndirectos / 100) & ", " & (valorUtilidades / 100) & ", " & fecha & ", '" & hora & "', '" & susername & "')"

                    Else

                        queriesInsert(0) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Cards VALUES (" & baseid & ", " & icardid & ", '" & cmbCategoriaDeLaTarjeta.SelectedValue & "', '" & txtCodigoDeLaTarjeta.Text & "', '" & txtNombreDeLaTarjeta.Text & "', '" & txtUnidadDeMedida.Text & "', 1, " & (valorIndirectos / 100) & ", " & (valorUtilidades / 100) & ", " & fecha & ", '" & hora & "', '" & susername & "')"
                        queriesInsert(1) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "Cards VALUES (" & iprojectid & ", " & icardid & ", '" & cmbCategoriaDeLaTarjeta.SelectedValue & "', '" & txtCodigoDeLaTarjeta.Text & "', '" & txtNombreDeLaTarjeta.Text & "', '" & txtUnidadDeMedida.Text & "', 1, " & (valorIndirectos / 100) & ", " & (valorUtilidades / 100) & ", " & fecha & ", '" & hora & "', '" & susername & "')"

                    End If

                End If

                executeTransactedSQLCommand(0, queriesInsert)

            End If

        End If

        Cursor.Current = System.Windows.Forms.Cursors.Default


    End Sub


    Private Function validaDatosInicialesTarjeta(ByVal silent As Boolean, ByVal save As Boolean) As Boolean

        Dim strcaracteresprohibidos As String = "|°!#$%&/()=?¡*¨[]_:;,.-{}+´¿'¬^`~@\<> "
        Dim strcaracteresprohibidos2 As String = "|°!#$%&/()=?¡*¨[]_:;,.-{}+´¿'¬^`~@\<> "
        Dim strcaracteresprohibidos3 As String = "|'\"

        txtUnidadDeMedida.Text = txtUnidadDeMedida.Text.Trim(strcaracteresprohibidos.ToCharArray)
        txtCodigoDeLaTarjeta.Text = txtCodigoDeLaTarjeta.Text.Trim(strcaracteresprohibidos2.ToCharArray)
        txtNombreDeLaTarjeta.Text = txtNombreDeLaTarjeta.Text.Trim(strcaracteresprohibidos3.ToCharArray).Replace("--", "")

        Dim strcaracteresprohibidos4 As String = "abcdefghijklmnñopqrstuvwxyz|°!#$%&/()=?¡*¨[]_:;,-{}+´¿'¬^`~@\<> "
        Dim arrayCaractProhib As Char() = strcaracteresprohibidos4.ToCharArray
        Dim resultado As Boolean = False


        For carp = 0 To arrayCaractProhib.Length - 1

            If txtPorcentajeIndirectos.Text.Contains(arrayCaractProhib(carp)) Then
                txtPorcentajeIndirectos.Text = txtPorcentajeIndirectos.Text.Replace(arrayCaractProhib(carp), "")
                resultado = True
            End If

        Next carp

        If txtPorcentajeIndirectos.Text.Contains(".") Then

            Dim comparaPuntos As Char() = txtPorcentajeIndirectos.Text.ToCharArray
            Dim cuantosPuntos As Integer = 0


            For letra = 0 To comparaPuntos.Length - 1

                If comparaPuntos(letra) = "." Then
                    cuantosPuntos = cuantosPuntos + 1
                End If

            Next

            If cuantosPuntos > 1 Then

                For cantidad = 1 To cuantosPuntos
                    Dim lugar As Integer = txtPorcentajeIndirectos.Text.LastIndexOf(".")
                    Dim longitud As Integer = txtPorcentajeIndirectos.Text.Length

                    If longitud > (lugar + 1) Then
                        txtPorcentajeIndirectos.Text = txtPorcentajeIndirectos.Text.Substring(0, lugar) & txtPorcentajeIndirectos.Text.Substring(lugar + 1)
                        resultado = True
                        Exit For
                    Else
                        txtPorcentajeIndirectos.Text = txtPorcentajeIndirectos.Text.Substring(0, lugar)
                        resultado = True
                        Exit For
                    End If
                Next

            End If

        End If


        Dim valorIndirectos As Double = 0.0
        Try
            valorIndirectos = CDbl(txtPorcentajeIndirectos.Text)
        Catch ex As Exception

        End Try



        Dim arrayCaractProhib2 As Char() = strcaracteresprohibidos4.ToCharArray
        Dim resultado2 As Boolean = False

        For carp = 0 To arrayCaractProhib2.Length - 1

            If txtPorcentajeUtilidades.Text.Contains(arrayCaractProhib2(carp)) Then
                txtPorcentajeUtilidades.Text = txtPorcentajeUtilidades.Text.Replace(arrayCaractProhib2(carp), "")
                resultado2 = True
            End If

        Next carp

        If txtPorcentajeUtilidades.Text.Contains(".") Then

            Dim comparaPuntos As Char() = txtPorcentajeUtilidades.Text.ToCharArray
            Dim cuantosPuntos As Integer = 0


            For letra = 0 To comparaPuntos.Length - 1

                If comparaPuntos(letra) = "." Then
                    cuantosPuntos = cuantosPuntos + 1
                End If

            Next

            If cuantosPuntos > 1 Then

                For cantidad = 1 To cuantosPuntos
                    Dim lugar As Integer = txtPorcentajeUtilidades.Text.LastIndexOf(".")
                    Dim longitud As Integer = txtPorcentajeUtilidades.Text.Length

                    If longitud > (lugar + 1) Then
                        txtPorcentajeUtilidades.Text = txtPorcentajeUtilidades.Text.Substring(0, lugar) & txtPorcentajeUtilidades.Text.Substring(lugar + 1)
                        resultado2 = True
                        Exit For
                    Else
                        txtPorcentajeUtilidades.Text = txtPorcentajeUtilidades.Text.Substring(0, lugar)
                        resultado2 = True
                        Exit For
                    End If
                Next

            End If

        End If


        Dim valorUtilidades As Double = 0.0
        Try
            valorUtilidades = CDbl(txtPorcentajeUtilidades.Text)
        Catch ex As Exception

        End Try

        If cmbCategoriaDeLaTarjeta.SelectedIndex = -1 Then

            If silent = False Then
                MsgBox("¿Podrías escoger una categoría para la Tarjeta?", MsgBoxStyle.OkOnly, "Dato Faltante")
            End If

            Return False

        End If

        If txtUnidadDeMedida.Text = "" Then

            If silent = False Then
                MsgBox("¿Podrías poner una unidad de medida para la Tarjeta?", MsgBoxStyle.OkOnly, "Dato Faltante")
            End If

            Return False

        End If

        If txtCodigoDeLaTarjeta.Text = "" Then

            If silent = False Then
                MsgBox("¿Podrías poner un código para la Tarjeta? Así será más fácil usarla en algún proyecto...", MsgBoxStyle.OkOnly, "Dato Faltante")
            End If

            Return False

        End If

        If IsEdit = False And getSQLQueryAsInteger(0, "SELECT COUNT(*) FROM basecards WHERE scardlegacyid = " & txtCodigoDeLaTarjeta.Text) >= 1 Then

            Dim dsSuggestions As DataSet
            Dim suggestions As String = "("

            If IsBase = True Then
                dsSuggestions = getSQLQueryAsDataset(0, "SELECT CONCAT(SUBSTRING(MAX(scardlegacyid), 1, LENGTH(MAX(scardlegacyid)) - 1), SUBSTRING(MAX(scardlegacyid), LENGTH(MAX(scardlegacyid))) + 1) as sug FROM basecards GROUP BY scardlegacycategoryid")
            Else
                If IsModel = True Then
                    dsSuggestions = getSQLQueryAsDataset(0, "SELECT CONCAT(SUBSTRING(MAX(scardlegacyid), 1, LENGTH(MAX(scardlegacyid)) - 1), SUBSTRING(MAX(scardlegacyid), LENGTH(MAX(scardlegacyid))) + 1) as sug FROM modelvards GROUP BY scardlegacycategoryid")
                Else
                    dsSuggestions = getSQLQueryAsDataset(0, "SELECT CONCAT(SUBSTRING(MAX(scardlegacyid), 1, LENGTH(MAX(scardlegacyid)) - 1), SUBSTRING(MAX(scardlegacyid), LENGTH(MAX(scardlegacyid))) + 1) as sug FROM projectcards GROUP BY scardlegacycategoryid")
                End If
            End If

            For i = 0 To dsSuggestions.Tables(0).Rows.Count - 1

                suggestions = suggestions & dsSuggestions.Tables(0).Rows(i).Item(0) & ", "

            Next i

            suggestions = suggestions & ")"
            suggestions = suggestions.Replace(", )", ")").Replace("(", "").Replace(")", "")

            If silent = False Then
                MsgBox("El código de esta tarjeta ya se encuentra utilizado por otra tarjeta. Intenta con otro diferente." & Chr(13) & "Sugerencias: " & suggestions, MsgBoxStyle.OkOnly, "Dato Faltante")
            End If

            Return False

        ElseIf IsEdit = True And txtCodigoDeLaTarjeta.Text <> getSQLQueryAsString(0, "SELECT scardlegacyid FROM basecards WHERE icardid = " & icardid) Then

            If getSQLQueryAsInteger(0, "SELECT COUNT(*) FROM basecards WHERE scardlegacyid = " & txtCodigoDeLaTarjeta.Text) >= 1 Then

                Dim dsSuggestions As DataSet
                Dim suggestions As String = "("

                If IsBase = True Then
                    dsSuggestions = getSQLQueryAsDataset(0, "SELECT CONCAT(SUBSTRING(MAX(scardlegacyid), 1, LENGTH(MAX(scardlegacyid)) - 1), SUBSTRING(MAX(scardlegacyid), LENGTH(MAX(scardlegacyid))) + 1) as sug FROM basecards GROUP BY scardlegacycategoryid")
                Else
                    If IsModel = True Then
                        dsSuggestions = getSQLQueryAsDataset(0, "SELECT CONCAT(SUBSTRING(MAX(scardlegacyid), 1, LENGTH(MAX(scardlegacyid)) - 1), SUBSTRING(MAX(scardlegacyid), LENGTH(MAX(scardlegacyid))) + 1) as sug FROM modelcards GROUP BY scardlegacycategoryid")
                    Else
                        dsSuggestions = getSQLQueryAsDataset(0, "SELECT CONCAT(SUBSTRING(MAX(scardlegacyid), 1, LENGTH(MAX(scardlegacyid)) - 1), SUBSTRING(MAX(scardlegacyid), LENGTH(MAX(scardlegacyid))) + 1) as sug FROM projectcards GROUP BY scardlegacycategoryid")
                    End If
                End If

                For i = 0 To dsSuggestions.Tables(0).Rows.Count - 1

                    suggestions = suggestions & dsSuggestions.Tables(0).Rows(i).Item(0) & ", "

                Next i

                suggestions = suggestions & ")"
                suggestions = suggestions.Replace(", )", ")").Replace("(", "").Replace(")", "")

                If silent = False Then
                    MsgBox("El código de esta tarjeta ya se encuentra utilizado por otra tarjeta. Intenta con otro diferente." & Chr(13) & "Sugerencias: " & suggestions, MsgBoxStyle.OkOnly, "Dato Faltante")
                End If

                Return False

            End If

        End If

        If txtNombreDeLaTarjeta.Text = "" Then

            If silent = False Then
                MsgBox("¿Podrías poner una descripción a la Tarjeta?", MsgBoxStyle.OkOnly, "Dato Faltante")
            End If

            Return False

        End If

        If txtPorcentajeIndirectos.Text = "" Then

            If silent = False Then
                MsgBox("¿Podrías poner un Porcentaje de Indirectos para la Tarjeta?", MsgBoxStyle.OkOnly, "Dato Faltante")
            End If

            Return False

        End If

        If txtPorcentajeUtilidades.Text = "" Then

            If silent = False Then
                MsgBox("¿Podrías poner un Porcentaje de Utilidades para la Tarjeta?", MsgBoxStyle.OkOnly, "Dato Faltante")
            End If

            Return False

        End If

        If txtPorcentajeIndirectos.Text = "0" Or txtPorcentajeIndirectos.Text = "0.0" Then

            If silent = False Then

                If save = True Then

                    If MsgBox("Seguro que deseas utilizar 0% como Porcentaje de Indirectos de la Tarjeta?", MsgBoxStyle.YesNo, "Confirmación Porcentaje en Cero") = MsgBoxResult.Yes Then
                        'Continue
                    Else
                        Return False
                    End If

                End If

            End If

        End If

        If txtPorcentajeUtilidades.Text = "" Or txtPorcentajeUtilidades.Text = "0" Or txtPorcentajeUtilidades.Text = "0.0" Then

            If silent = False Then

                If save = True Then

                    If MsgBox("Seguro que deseas utilizar 0% como Porcentaje de Utilidades de la Tarjeta?", MsgBoxStyle.YesNo, "Confirmación Porcentaje en Cero") = MsgBoxResult.Yes Then
                        'Continue
                    Else
                        Return False
                    End If

                End If

            End If

        End If

        Return True

    End Function


    Private Sub dgvConceptosTarjeta_CellClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgvConceptosTarjeta.CellClick

        If validaDatosInicialesTarjeta(True, False) = False Then
            Exit Sub
        End If

        Try

            If dgvConceptosTarjeta.CurrentRow.IsNewRow Then
                Exit Sub
            End If

            iselectedinputid = CInt(dgvConceptosTarjeta.Rows(e.RowIndex).Cells(0).Value())
            sselectedinputdescription = dgvConceptosTarjeta.Rows(e.RowIndex).Cells(1).Value()
            sselectedunit = dgvConceptosTarjeta.Rows(e.RowIndex).Cells(2).Value()
            dselectedinputqty = CDbl(dgvConceptosTarjeta.Rows(e.RowIndex).Cells(4).Value())

        Catch ex As Exception

            iselectedinputid = 0
            sselectedinputdescription = ""
            sselectedunit = ""
            dselectedinputqty = 1.0

        End Try

    End Sub


    Private Sub dgvConceptosTarjeta_CellContentClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgvConceptosTarjeta.CellContentClick

        If validaDatosInicialesTarjeta(True, False) = False Then
            Exit Sub
        End If

        Try

            If dgvConceptosTarjeta.CurrentRow.IsNewRow Then
                Exit Sub
            End If

            iselectedinputid = CInt(dgvConceptosTarjeta.Rows(e.RowIndex).Cells(0).Value())
            sselectedinputdescription = dgvConceptosTarjeta.Rows(e.RowIndex).Cells(1).Value()
            sselectedunit = dgvConceptosTarjeta.Rows(e.RowIndex).Cells(2).Value()
            dselectedinputqty = CDbl(dgvConceptosTarjeta.Rows(e.RowIndex).Cells(4).Value())

        Catch ex As Exception

            iselectedinputid = 0
            sselectedinputdescription = ""
            sselectedunit = ""
            dselectedinputqty = 1.0

        End Try

    End Sub


    Private Sub dgvConceptosTarjeta_SelectionChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgvConceptosTarjeta.SelectionChanged

        txtCantidadDgvConceptosTarjeta = Nothing
        txtCantidadDgvConceptosTarjeta_OldText = Nothing
        txtNombreDgvConceptosTarjeta = Nothing
        txtNombreDgvConceptosTarjeta_OldText = Nothing

        If isFormReadyForAction = False Then
            Exit Sub
        End If

        If isConceptosTarjetaDGVReady = False Then
            Exit Sub
        End If

        Try

            If dgvConceptosTarjeta.CurrentRow.IsNewRow Then
                Exit Sub
            End If

            iselectedinputid = CInt(dgvConceptosTarjeta.CurrentRow.Cells(0).Value())
            sselectedinputdescription = dgvConceptosTarjeta.CurrentRow.Cells(1).Value()
            sselectedunit = dgvConceptosTarjeta.CurrentRow.Cells(2).Value()
            dselectedinputqty = CDbl(dgvConceptosTarjeta.CurrentRow.Cells(4).Value())

        Catch ex As Exception

            iselectedinputid = 0
            sselectedinputdescription = ""
            sselectedunit = ""
            dselectedinputqty = 1.0

        End Try

    End Sub


    Private Sub dgvConceptosTarjeta_CellContentDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgvConceptosTarjeta.CellContentDoubleClick

        If openInputPermission = False Then
            Exit Sub
        End If

        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        Try

            If dgvConceptosTarjeta.CurrentRow.IsNewRow Then

                Cursor.Current = System.Windows.Forms.Cursors.Default
                Exit Sub

            End If

            iselectedinputid = CInt(dgvConceptosTarjeta.Rows(e.RowIndex).Cells(0).Value())
            sselectedinputdescription = dgvConceptosTarjeta.Rows(e.RowIndex).Cells(1).Value()
            sselectedunit = dgvConceptosTarjeta.Rows(e.RowIndex).Cells(2).Value()
            dselectedinputqty = CDbl(dgvConceptosTarjeta.Rows(e.RowIndex).Cells(4).Value())

        Catch ex As Exception

            iselectedinputid = 0
            sselectedinputdescription = ""
            sselectedunit = ""
            dselectedinputqty = 1.0

        End Try

        If iselectedinputid = 0 Then
            Exit Sub
        End If

        Dim conteoCompound As Integer = 0

        'conteoCompound = getSQLQueryAsInteger(0, "SELECT COUNT(*) FROM basecardcompoundinputs WHERE iinputid = " & iselectedinputid)
        conteoCompound = getSQLQueryAsInteger(0, "SELECT COUNT(*) FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardCompoundInputs WHERE iinputid = " & iselectedinputid)

        If conteoCompound > 0 Then

            'Es un material compuesto
            Dim aic As New AgregarInsumoCompuesto

            aic.susername = susername
            aic.bactive = bactive
            aic.bonline = bonline
            aic.suserfullname = suserfullname
            aic.suseremail = suseremail
            aic.susersession = susersession
            aic.susermachinename = susermachinename
            aic.suserip = suserip

            aic.iprojectid = iprojectid
            aic.icardid = icardid
            aic.iinputid = iselectedinputid

            aic.IsHistoric = IsHistoric

            aic.IsBase = IsBase
            aic.IsModel = IsModel

            aic.IsEdit = True

            If Me.WindowState = FormWindowState.Maximized Then
                aic.WindowState = FormWindowState.Maximized
            End If

            Me.Visible = False
            aic.ShowDialog(Me)
            Me.Visible = True

            If aic.DialogResult = Windows.Forms.DialogResult.OK Then

                updateEverything()

                isConceptosTarjetaDGVReady = True

            End If

        Else

            'NO Es un material compuesto
            Dim ai As New AgregarInsumo

            ai.susername = susername
            ai.bactive = bactive
            ai.bonline = bonline
            ai.suserfullname = suserfullname
            ai.suseremail = suseremail
            ai.susersession = susersession
            ai.susermachinename = susermachinename
            ai.suserip = suserip

            ai.iprojectid = iprojectid
            ai.icardid = icardid
            ai.iinputid = iselectedinputid

            ai.IsHistoric = IsHistoric

            ai.IsBase = IsBase
            ai.IsModel = IsModel

            ai.IsEdit = True

            If Me.WindowState = FormWindowState.Maximized Then
                ai.WindowState = FormWindowState.Maximized
            End If

            Me.Visible = False
            ai.ShowDialog(Me)
            Me.Visible = True

            If ai.DialogResult = Windows.Forms.DialogResult.OK Then

                updateEverything()

                isConceptosTarjetaDGVReady = True

            End If

        End If

        Cursor.Current = System.Windows.Forms.Cursors.Default

    End Sub


    Private Sub dgvConceptosTarjeta_CellDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgvConceptosTarjeta.CellDoubleClick

        If openInputPermission = False Then
            Exit Sub
        End If

        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        Try

            If dgvConceptosTarjeta.CurrentRow.IsNewRow Then
                Cursor.Current = System.Windows.Forms.Cursors.Default
                Exit Sub
            End If

            iselectedinputid = CInt(dgvConceptosTarjeta.Rows(e.RowIndex).Cells(0).Value())
            sselectedinputdescription = dgvConceptosTarjeta.Rows(e.RowIndex).Cells(1).Value()
            sselectedunit = dgvConceptosTarjeta.Rows(e.RowIndex).Cells(2).Value()
            dselectedinputqty = CDbl(dgvConceptosTarjeta.Rows(e.RowIndex).Cells(4).Value())

        Catch ex As Exception

            iselectedinputid = 0
            sselectedinputdescription = ""
            sselectedunit = ""
            dselectedinputqty = 1.0

        End Try

        If iselectedinputid = 0 Then
            Exit Sub
        End If

        Dim conteoCompound As Integer = 0

        'conteoCompound = getSQLQueryAsInteger(0, "SELECT COUNT(*) FROM basecardcompoundinputs WHERE iinputid = " & iselectedinputid)
        conteoCompound = getSQLQueryAsInteger(0, "SELECT COUNT(*) FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardCompoundInputs WHERE iinputid = " & iselectedinputid)

        If conteoCompound > 0 Then

            'Es un material compuesto
            Dim aic As New AgregarInsumoCompuesto

            aic.susername = susername
            aic.bactive = bactive
            aic.bonline = bonline
            aic.suserfullname = suserfullname
            aic.suseremail = suseremail
            aic.susersession = susersession
            aic.susermachinename = susermachinename
            aic.suserip = suserip

            aic.iprojectid = iprojectid
            aic.icardid = icardid
            aic.iinputid = iselectedinputid

            aic.IsHistoric = IsHistoric

            aic.IsBase = IsBase
            aic.IsModel = IsModel

            aic.IsEdit = True

            If Me.WindowState = FormWindowState.Maximized Then
                aic.WindowState = FormWindowState.Maximized
            End If

            Me.Visible = False
            aic.ShowDialog(Me)
            Me.Visible = True

            If aic.DialogResult = Windows.Forms.DialogResult.OK Then

                updateEverything()

                isConceptosTarjetaDGVReady = True

            End If

        Else

            'NO Es un material compuesto
            Dim ai As New AgregarInsumo

            ai.susername = susername
            ai.bactive = bactive
            ai.bonline = bonline
            ai.suserfullname = suserfullname
            ai.suseremail = suseremail
            ai.susersession = susersession
            ai.susermachinename = susermachinename
            ai.suserip = suserip

            ai.iprojectid = iprojectid
            ai.icardid = icardid
            ai.iinputid = iselectedinputid

            ai.IsHistoric = IsHistoric

            ai.IsBase = IsBase
            ai.IsModel = IsModel

            ai.IsEdit = True

            If Me.WindowState = FormWindowState.Maximized Then
                ai.WindowState = FormWindowState.Maximized
            End If

            Me.Visible = False
            ai.ShowDialog(Me)
            Me.Visible = True

            If ai.DialogResult = Windows.Forms.DialogResult.OK Then

                updateEverything()

                isConceptosTarjetaDGVReady = True

            End If

        End If

        Cursor.Current = System.Windows.Forms.Cursors.Default

    End Sub


    Private Sub dgvConceptosTarjeta_CellEndEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgvConceptosTarjeta.CellEndEdit

        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        updateEverything()

        Cursor.Current = System.Windows.Forms.Cursors.Default

    End Sub


    Private Sub dgvConceptosTarjeta_CellValueChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgvConceptosTarjeta.CellValueChanged

        If modifyInputQtyPermission = False Then
            Exit Sub
        End If

        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        'LAS COLUMNAS EDITABLES SON 1, 2 Y 4: sinputdescription, scardinputunit, dcardinputqty

        If isConceptosTarjetaDGVReady = False Then
            Exit Sub
        End If


        Dim fecha As Integer = 0
        Dim hora As String = ""

        fecha = getMySQLDate()
        hora = getAppTime()

        Dim baseid As Integer = 0
        baseid = getSQLQueryAsInteger(0, "SELECT ibaseid FROM base ORDER BY iupdatedate DESC, supdatetime DESC LIMIT 1")

        If baseid = 0 Then
            baseid = 99999
        End If

        Dim firstTimeCompoundAddedAlert As Boolean = False


        If e.ColumnIndex = 1 Then

            'sinputdescription

            If dgvConceptosTarjeta.Rows(e.RowIndex).Cells(e.ColumnIndex).Value Is DBNull.Value Then

                If MsgBox("¿Estás seguro de que quieres eliminar el Insumo " & sselectedinputdescription & " de la Tarjeta?", MsgBoxStyle.YesNo, "Confirmación de Eliminación de Insumo de la Tarjeta") = MsgBoxResult.Yes Then

                    Dim conteoMO As Integer = 0
                    Dim conteoEQ As Integer = 0

                    Dim busquedaMO As String = ""
                    Dim busquedaEQ As String = ""

                    If IsBase = True Then

                        busquedaMO = "" & _
                        "SELECT COUNT(*) " & _
                        "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardInputs btfi " & _
                        "JOIN inputtypes it ON it.iinputid = btfi.iinputid " & _
                        "WHERE btfi.ibaseid = " & iprojectid & " AND btfi.icardid = " & icardid & " " & _
                        "AND it.sinputtypedescription = 'MANO DE OBRA' "

                        busquedaEQ = "" & _
                        "SELECT COUNT(*) " & _
                        "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardInputs btfi " & _
                        "JOIN inputtypes it ON it.iinputid = btfi.iinputiconteoEQ = getSQLQueryAsDouble(0, busquedaEQ)d " & _
                        "WHERE btfi.ibaseid = " & iprojectid & " AND btfi.icardid = " & icardid & " " & _
                        "AND it.sinputtypedescription = 'EQUIPO Y HERRAMIENTA' "

                        conteoMO = getSQLQueryAsDouble(0, busquedaMO)
                        conteoEQ = getSQLQueryAsDouble(0, busquedaEQ)

                        Dim queriesDelete(2) As String

                        If conteoMO = 0 Then
                            'Continue
                        ElseIf conteoMO > 1 Then
                            queriesDelete(0) = "DELETE FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardInputs WHERE ibaseid = " & iprojectid & " AND icardid = " & icardid & " AND iinputid = " & iselectedinputid
                        Else
                            MsgBox("No es posible dejar una Tarjeta sin Mano de Obra", MsgBoxStyle.OkOnly, "Mano de Obra Requerida")
                            Exit Sub
                        End If

                        If conteoEQ = 0 Then
                            'Continue
                        ElseIf conteoEQ > 1 Then
                            queriesDelete(0) = "DELETE FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardInputs WHERE ibaseid = " & iprojectid & " AND icardid = " & icardid & " AND iinputid = " & iselectedinputid
                        Else
                            MsgBox("No es posible dejar una Tarjeta sin Equipo y Herramientas", MsgBoxStyle.OkOnly, "Equipo y Herramientas Requeridos")
                            Exit Sub
                        End If

                        queriesDelete(0) = "DELETE FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardInputs WHERE ibaseid = " & iprojectid & " AND icardid = " & icardid & " AND iinputid = " & iselectedinputid

                        Dim conteoCompound As Integer = 0

                        conteoCompound = getSQLQueryAsInteger(0, "SELECT COUNT(*) FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardCompoundInputs WHERE iinputid = " & iselectedinputid)

                        If conteoCompound > 0 Then
                            queriesDelete(1) = "DELETE FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardCompoundInputs WHERE ibaseid = " & iprojectid & " AND icardid = " & icardid & " AND iinputid = " & iselectedinputid
                        End If

                        executeTransactedSQLCommand(0, queriesDelete)

                    Else

                        If IsModel = True Then

                            busquedaMO = "" & _
                            "SELECT COUNT(*) " & _
                            "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "CardInputs mtfi " & _
                            "JOIN inputtypes it ON it.iinputid = mtfi.iinputid " & _
                            "WHERE mtfi.imodelid = " & iprojectid & " AND mtfi.icardid = " & icardid & " " & _
                            "AND it.sinputtypedescription = 'MANO DE OBRA' "

                            busquedaEQ = "" & _
                            "SELECT COUNT(*) " & _
                            "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "CardInputs mtfi " & _
                            "JOIN inputtypes it ON it.iinputid = mtfi.iinputid " & _
                            "WHERE mtfi.imodelid = " & iprojectid & " AND mtfi.icardid = " & icardid & " " & _
                            "AND it.sinputtypedescription = 'EQUIPO Y HERRAMIENTA' "

                            conteoMO = getSQLQueryAsDouble(0, busquedaMO)
                            conteoEQ = getSQLQueryAsDouble(0, busquedaEQ)

                            Dim queriesDelete(2) As String

                            If conteoMO = 0 Then
                                'Continue
                            ElseIf conteoMO > 1 Then
                                queriesDelete(0) = "DELETE FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "CardInputs WHERE imodelid = " & iprojectid & " AND icardid = " & icardid & " AND iinputid = " & iselectedinputid
                            Else
                                MsgBox("No es posible dejar una Tarjeta sin Mano de Obra", MsgBoxStyle.OkOnly, "Mano de Obra Requerida")
                                Exit Sub
                            End If

                            If conteoEQ = 0 Then
                                'Continue
                            ElseIf conteoEQ > 1 Then
                                queriesDelete(0) = "DELETE FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "CardInputs WHERE imodelid = " & iprojectid & " AND icardid = " & icardid & " AND iinputid = " & iselectedinputid
                            Else
                                MsgBox("No es posible dejar una Tarjeta sin Equipo y Herramientas", MsgBoxStyle.OkOnly, "Equipo y Herramientas Requeridos")
                                Exit Sub
                            End If

                            queriesDelete(0) = "DELETE FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "CardInputs WHERE imodelid = " & iprojectid & " AND icardid = " & icardid & " AND iinputid = " & iselectedinputid

                            Dim conteoCompound As Integer = 0

                            conteoCompound = getSQLQueryAsInteger(0, "SELECT COUNT(*) FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardCompoundInputs WHERE iinputid = " & iselectedinputid)

                            If conteoCompound > 0 Then
                                queriesDelete(1) = "DELETE FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "CardCompoundInputs WHERE imodelid = " & iprojectid & " AND icardid = " & icardid & " AND iinputid = " & iselectedinputid
                            End If

                            executeTransactedSQLCommand(0, queriesDelete)

                        Else

                            busquedaMO = "" & _
                            "SELECT COUNT(*) " & _
                            "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "CardInputs ptfi " & _
                            "JOIN inputtypes it ON it.iinputid = ptfi.iinputid " & _
                            "WHERE ptfi.iprojectid = " & iprojectid & " AND ptfi.icardid = " & icardid & " " & _
                            "AND it.sinputtypedescription = 'MANO DE OBRA' "

                            busquedaEQ = "" & _
                            "SELECT COUNT(*) " & _
                            "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "CardInputs ptfi " & _
                            "JOIN inputtypes it ON it.iinputid = ptfi.iinputid " & _
                            "WHERE ptfi.iprojectid = " & iprojectid & " AND ptfi.icardid = " & icardid & " " & _
                            "AND it.sinputtypedescription = 'EQUIPO Y HERRAMIENTA' "

                            conteoMO = getSQLQueryAsDouble(0, busquedaMO)
                            conteoEQ = getSQLQueryAsDouble(0, busquedaEQ)

                            Dim queriesDelete(2) As String

                            If conteoMO = 0 Then
                                'Continue
                            ElseIf conteoMO > 1 Then
                                queriesDelete(0) = "DELETE FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "CardInputs WHERE iprojectid = " & iprojectid & " AND icardid = " & icardid & " AND iinputid = " & iselectedinputid
                            Else
                                MsgBox("No es posible dejar una Tarjeta sin Mano de Obra", MsgBoxStyle.OkOnly, "Mano de Obra Requerida")
                                Exit Sub
                            End If

                            If conteoEQ = 0 Then
                                'Continue
                            ElseIf conteoEQ > 1 Then
                                queriesDelete(0) = "DELETE FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "CardInputs WHERE iprojectid = " & iprojectid & " AND icardid = " & icardid & " AND iinputid = " & iselectedinputid
                            Else
                                MsgBox("No es posible dejar una Tarjeta sin Equipo y Herramientas", MsgBoxStyle.OkOnly, "Equipo y Herramientas Requeridos")
                                Exit Sub
                            End If

                            queriesDelete(0) = "DELETE FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "CardInputs WHERE iprojectid = " & iprojectid & " AND icardid = " & icardid & " AND iinputid = " & iselectedinputid

                            Dim conteoCompound As Integer = 0

                            conteoCompound = getSQLQueryAsInteger(0, "SELECT COUNT(*) FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardCompoundInputs WHERE iinputid = " & iselectedinputid)

                            If conteoCompound > 0 Then
                                queriesDelete(1) = "DELETE FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "CardCompoundInputs WHERE iprojectid = " & iprojectid & " AND icardid = " & icardid & " AND iinputid = " & iselectedinputid
                            End If

                            executeTransactedSQLCommand(0, queriesDelete)

                        End If

                    End If

                Else

                    'dgvConceptosTarjeta.Rows(e.RowIndex).Cells(e.ColumnIndex).Value = sselectedinputdescription

                End If

            Else

                'Si pone un texto, e.g. una descripcion de otro insumo

                dgvConceptosTarjeta.Rows(e.RowIndex).Cells(e.ColumnIndex).Value = dgvConceptosTarjeta.Rows(e.RowIndex).Cells(e.ColumnIndex).Value.ToString.Trim.Replace("'", "").Replace("--", "").Replace("@", "")

                Dim bi As New BuscaInsumos
                bi.susername = susername
                bi.bactive = bactive
                bi.bonline = bonline
                bi.suserfullname = suserfullname

                bi.suseremail = suseremail
                bi.susersession = susersession
                bi.susermachinename = susermachinename
                bi.suserip = suserip


                bi.IsEdit = IsEdit
                bi.IsBase = IsBase
                bi.IsModel = IsModel
                bi.IsHistoric = IsHistoric

                bi.iprojectid = iprojectid
                bi.icardid = icardid

                bi.querystring = dgvConceptosTarjeta.CurrentCell.EditedFormattedValue

                bi.IsEdit = False

                If Me.WindowState = FormWindowState.Maximized Then
                    bi.WindowState = FormWindowState.Maximized
                End If

                Me.Visible = False
                bi.ShowDialog(Me)
                Me.Visible = True

                If bi.DialogResult = Windows.Forms.DialogResult.OK Then

                    Dim isCompound As Boolean

                    isCompound = getSQLQueryAsBoolean(0, "SELECT * FROM basecardcompoundinputs WHERE iinputid = " & bi.iinputid)

                    If bi.wasCreated = True And isCompound = True Then
                        Exit Sub
                    End If

                    Dim dsBusquedaInsumosRepetidos As DataSet

                    If IsBase = True Then

                        dsBusquedaInsumosRepetidos = getSQLQueryAsDataset(0, "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardInputs WHERE ibaseid = " & iprojectid & " AND icardid = " & icardid & " AND iinputid = " & bi.iinputid)

                    Else

                        If IsModel = True Then

                            dsBusquedaInsumosRepetidos = getSQLQueryAsDataset(0, "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "CardInputs WHERE imodelid = " & iprojectid & " AND icardid = " & icardid & " AND iinputid = " & bi.iinputid)

                        Else

                            dsBusquedaInsumosRepetidos = getSQLQueryAsDataset(0, "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "CardInputs WHERE iprojectid = " & iprojectid & " AND icardid = " & icardid & " AND iinputid = " & bi.iinputid)

                        End If

                    End If


                    If dsBusquedaInsumosRepetidos.Tables(0).Rows.Count > 0 Then

                        MsgBox("Ya tienes ese Insumo insertado en esta Tarjeta. ¿Podrías buscarlo en la lista y cambiar la cantidad si así lo deseas?", MsgBoxStyle.OkOnly, "Insumo Repetido")
                        Exit Sub

                    End If

                    Dim cantidaddeinsumo As Double = 1.0

                    Try
                        cantidaddeinsumo = CDbl(dgvConceptosTarjeta.Rows(e.RowIndex).Cells(4).Value)
                    Catch ex As Exception

                    End Try


                    Dim isManoDeObra As Boolean = False
                    Dim isEqYHerr As Boolean = False

                    Dim conteoMO As Integer = 0
                    Dim conteoEQ As Integer = 0

                    Dim busquedaMO As String = ""
                    Dim busquedaEQ As String = ""

                    isManoDeObra = getSQLQueryAsBoolean(0, "SELECT * FROM inputs i JOIN inputtypes it ON i.iinputid = it.iinputid WHERE it.sinputtypedescription = 'MANO DE OBRA' AND i.iinputid = " & bi.iinputid)
                    isEqYHerr = getSQLQueryAsBoolean(0, "SELECT * FROM inputs i JOIN inputtypes it ON i.iinputid = it.iinputid WHERE it.sinputtypedescription = 'EQUIPO Y HERRAMIENTA' AND i.iinputid = " & bi.iinputid)

                    If isEqYHerr = True Then
                        dgvConceptosTarjeta.Rows(e.RowIndex).Cells(e.ColumnIndex).Value = sselectedinputdescription
                        Exit Sub
                    End If

                    If IsEdit = True Then


                        If IsBase = True Then

                            Dim queriesInsert(5) As String

                            If isManoDeObra = False Then

                                queriesInsert(0) = "DELETE FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardInputs WHERE ibaseid = " & iprojectid & " AND icardid = " & icardid & " AND iinputid = " & iselectedinputid
                                queriesInsert(1) = "DELETE FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardCompoundInputs WHERE ibaseid = " & iprojectid & " AND icardid = " & icardid & " AND iinputid = " & iselectedinputid

                            End If

                            queriesInsert(2) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardInputs SELECT " & iprojectid & ", " & icardid & ", iinputid, sinputunit, " & cantidaddeinsumo & ", " & fecha & ", '" & hora & "', '" & susername & "' FROM inputs WHERE iinputid = " & bi.iinputid

                            If isManoDeObra = True Then

                                busquedaMO = "" & _
                                "SELECT COUNT(*) " & _
                                "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardInputs btfi " & _
                                "JOIN inputtypes it ON it.iinputid = btfi.iinputid " & _
                                "WHERE btfi.ibaseid = " & iprojectid & " AND btfi.icardid = " & icardid & " " & _
                                "AND it.sinputtypedescription = 'MANO DE OBRA' "

                                conteoMO = getSQLQueryAsDouble(0, busquedaMO)

                                If conteoMO = 0 Then
                                    queriesInsert(3) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardInputs SELECT " & iprojectid & ", " & icardid & ", 0, '%', 1, " & fecha & ", '" & hora & "', '" & susername & "'"
                                End If

                            End If

                            If isCompound = True Then

                                If getSQLQueryAsBoolean(0, "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardInputs WHERE ibaseid = " & iprojectid & " AND iinputid = " & bi.iinputid) = True Then
                                    queriesInsert(4) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardCompoundInputs SELECT * FROM (SELECT " & iprojectid & " AS projectid, " & icardid & " AS cardid, " & bi.iinputid & " AS iinputid, bcci.icompoundinputid, i.sinputunit, bcci.dcompoundinputqty AS qty, " & fecha & ", '" & hora & "', '" & susername & "' FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardInputs bci JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardCompoundInputs bcci ON bci.ibaseid = bcci.ibaseid AND bci.icardid = bcci.icardid AND bcci.iinputid = bcci.iinputid JOIN inputs i ON i.iinputid = bcci.icompoundinputid WHERE bci.iinputid = " & bi.iinputid & " ORDER BY bci.iupdatedate DESC, bci.supdatetime DESC) cid GROUP BY icompoundinputid "
                                Else
                                    If getSQLQueryAsBoolean(0, "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardInputs WHERE ibaseid = " & baseid & " AND iinputid = " & bi.iinputid) = True Then
                                        queriesInsert(4) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardCompoundInputs SELECT * FROM (SELECT " & iprojectid & " AS projectid, " & icardid & " AS cardid, " & bi.iinputid & " AS iinputid, bcci.icompoundinputid, i.sinputunit, bcci.dcompoundinputqty AS qty, " & fecha & ", '" & hora & "', '" & susername & "' FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardInputs bci JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardCompoundInputs bcci ON bci.ibaseid = bcci.ibaseid AND bci.icardid = bcci.icardid AND bcci.iinputid = bcci.iinputid JOIN inputs i ON i.iinputid = bcci.icompoundinputid WHERE bci.ibaseid = " & baseid & " AND bci.iinputid = " & bi.iinputid & " ORDER BY bci.iupdatedate DESC, bci.supdatetime DESC) cid GROUP BY icompoundinputid "
                                    Else
                                        queriesInsert(4) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardCompoundInputs SELECT * FROM (SELECT " & iprojectid & " AS projectid, " & icardid & " AS cardid, " & bi.iinputid & " AS iinputid, bcci.icompoundinputid, i.sinputunit, 1 AS qty, " & fecha & ", '" & hora & "', '" & susername & "' FROM inputs i WHERE i.iinputid = " & bi.iinputid & " ORDER BY i.iupdatedate DESC, i.supdatetime DESC) cid GROUP BY icompoundinputid "
                                    End If
                                    firstTimeCompoundAddedAlert = True
                                End If

                            End If

                            executeTransactedSQLCommand(0, queriesInsert)

                        Else

                            If IsModel = True Then

                                Dim queriesInsert(5) As String

                                If isManoDeObra = False Then

                                    queriesInsert(0) = "DELETE FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "CardInputs WHERE imodelid = " & iprojectid & " AND icardid = " & icardid & " AND iinputid = " & iselectedinputid
                                    queriesInsert(1) = "DELETE FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "CardCompoundInputs WHERE imodelid = " & iprojectid & " AND icardid = " & icardid & " AND iinputid = " & iselectedinputid

                                End If

                                queriesInsert(2) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "CardInputs SELECT " & iprojectid & ", " & icardid & ", iinputid, sinputunit, " & cantidaddeinsumo & ", " & fecha & ", '" & hora & "', '" & susername & "' FROM inputs WHERE iinputid = " & bi.iinputid

                                If isManoDeObra = True Then

                                    busquedaMO = "" & _
                                    "SELECT COUNT(*) " & _
                                    "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "CardInputs mtfi " & _
                                    "JOIN inputtypes it ON it.iinputid = mtfi.iinputid " & _
                                    "WHERE mtfi.imodelid = " & iprojectid & " AND mtfi.icardid = " & icardid & " " & _
                                    "AND it.sinputtypedescription = 'MANO DE OBRA' "

                                    conteoMO = getSQLQueryAsDouble(0, busquedaMO)

                                    If conteoMO = 0 Then
                                        queriesInsert(3) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "CardInputs SELECT " & iprojectid & ", " & icardid & ", 0, '%', 1, " & fecha & ", '" & hora & "', '" & susername & "'"
                                    End If

                                End If

                                If isCompound = True Then

                                    If getSQLQueryAsBoolean(0, "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "CardInputs WHERE imodelid = " & iprojectid & " AND iinputid = " & bi.iinputid) = True Then
                                        queriesInsert(4) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "CardCompoundInputs SELECT * FROM (SELECT " & iprojectid & " AS projectid, " & icardid & " AS cardid, " & bi.iinputid & " AS inputid, bcci.icompoundinputid, i.sinputunit, bcci.dcompoundinputqty AS qty, " & fecha & ", '" & hora & "', '" & susername & "' FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "CardInputs bci JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "CardCompoundInputs bcci ON bci.imodelid = bcci.imodelid AND bci.icardid = bcci.icardid AND bci.iinputid = bcci.iinputid JOIN inputs i ON i.iinputid = bcci.icompoundinputid WHERE bci.iinputid = " & bi.iinputid & " ORDER BY bci.iupdatedate DESC, bci.supdatetime DESC) cid GROUP BY icompoundinputid "
                                    Else
                                        If getSQLQueryAsBoolean(0, "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardInputs WHERE ibaseid = " & baseid & " AND iinputid = " & bi.iinputid) = True Then
                                            queriesInsert(4) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "CardCompoundInputs SELECT * FROM (SELECT " & iprojectid & " AS projectid, " & icardid & " AS cardid, " & bi.iinputid & " AS inputid, bcci.icompoundinputid, i.sinputunit, bcci.dcompoundinputqty AS qty, " & fecha & ", '" & hora & "', '" & susername & "' FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardInputs bci JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardCompoundInputs bcci ON bci.ibaseid = bcci.ibaseid AND bci.icardid = bcci.icardid AND bci.iinputid = bcci.iinputid JOIN inputs i ON i.iinputid = bcci.icompoundinputid WHERE bci.ibaseid = " & baseid & " AND bci.iinputid = " & bi.iinputid & " ORDER BY bci.iupdatedate DESC, bci.supdatetime DESC) cid GROUP BY icompoundinputid "
                                        Else
                                            queriesInsert(4) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "CardCompoundInputs SELECT * FROM (SELECT " & iprojectid & " AS projectid, " & icardid & " AS cardid, " & bi.iinputid & " AS inputid, bcci.icompoundinputid, i.sinputunit, 1 AS qty, " & fecha & ", '" & hora & "', '" & susername & "' FROM inputs i WHERE i.iinputid = " & bi.iinputid & " ORDER BY i.iupdatedate DESC, i.supdatetime DESC) cid GROUP BY icompoundinputid "
                                        End If
                                        firstTimeCompoundAddedAlert = True
                                    End If

                                End If

                                executeTransactedSQLCommand(0, queriesInsert)

                            Else

                                Dim queriesInsert(5) As String

                                If isManoDeObra = False Then

                                    queriesInsert(0) = "DELETE FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "CardInputs WHERE iprojectid = " & iprojectid & " AND icardid = " & icardid & " AND iinputid = " & iselectedinputid
                                    queriesInsert(1) = "DELETE FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "CardCompoundInputs WHERE iprojectid = " & iprojectid & " AND icardid = " & icardid & " AND iinputid = " & iselectedinputid

                                End If

                                queriesInsert(2) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "CardInputs SELECT " & iprojectid & ", " & icardid & ", iinputid, sinputunit, " & cantidaddeinsumo & ", " & fecha & ", '" & hora & "', '" & susername & "' FROM inputs WHERE iinputid = " & bi.iinputid

                                If isManoDeObra = True Then

                                    busquedaMO = "" & _
                                    "SELECT COUNT(*) " & _
                                    "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "CardInputs ptfi " & _
                                    "JOIN inputtypes it ON it.iinputid = ptfi.iinputid " & _
                                    "WHERE ptfi.iprojectid = " & iprojectid & " AND ptfi.icardid = " & icardid & " " & _
                                    "AND it.sinputtypedescription = 'MANO DE OBRA' "

                                    conteoMO = getSQLQueryAsDouble(0, busquedaMO)

                                    If conteoMO = 0 Then
                                        queriesInsert(3) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "CardInputs SELECT " & iprojectid & ", " & icardid & ", 0, '%', 1, " & fecha & ", '" & hora & "', '" & susername & "'"
                                    End If

                                End If

                                If isCompound = True Then

                                    If getSQLQueryAsBoolean(0, "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "CardInputs WHERE iprojectid = " & iprojectid & " AND iinputid = " & bi.iinputid) = True Then
                                        queriesInsert(4) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "CardCompoundInputs SELECT * FROM (SELECT " & iprojectid & " AS projectid, " & icardid & " AS cardid, " & bi.iinputid & " AS inputid, bcci.icompoundinputid, i.sinputunit, bcci.dcompoundinputqty AS qty, " & fecha & ", '" & hora & "', '" & susername & "' FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "CardInputs bci JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "CardCompoundInputs bcci ON bci.iprojectid = bcci.iprojectid AND bci.icardid = bcci.icardid AND bci.iinputid = bcci.iinputid JOIN inputs i ON i.iinputid = bcci.icompoundinputid WHERE bci.iinputid = " & bi.iinputid & " ORDER BY bci.iupdatedate DESC, bci.supdatetime DESC) cid GROUP BY icompoundinputid "
                                    Else
                                        If getSQLQueryAsBoolean(0, "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardInputs WHERE ibaseid = " & baseid & " AND iinputid = " & bi.iinputid) = True Then
                                            queriesInsert(4) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "CardCompoundInputs SELECT * FROM (SELECT " & iprojectid & " AS projectid, " & icardid & " AS cardid, " & bi.iinputid & " AS inputid, bcci.icompoundinputid, i.sinputunit, bcci.dcompoundinputqty AS qty, " & fecha & ", '" & hora & "', '" & susername & "' FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardInputs bci JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardCompoundInputs bcci ON bci.ibaseid = bcci.ibaseid AND bci.icardid = bcci.icardid AND bci.iinputid = bcci.iinputid JOIN inputs i ON i.iinputid = bcci.icompoundinputid WHERE bci.ibaseid = " & baseid & " AND bci.iinputid = " & bi.iinputid & " ORDER BY bci.iupdatedate DESC, bci.supdatetime DESC) cid GROUP BY icompoundinputid "
                                        Else
                                            queriesInsert(4) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "CardCompoundInputs SELECT * FROM (SELECT " & iprojectid & " AS projectid, " & icardid & " AS cardid, " & bi.iinputid & " AS inputid, bcci.icompoundinputid, i.sinputunit, 1 AS qty, " & fecha & ", '" & hora & "', '" & susername & "' FROM inputs i WHERE i.iinputid = " & bi.iinputid & " ORDER BY i.iupdatedate DESC, i.supdatetime DESC) cid GROUP BY icompoundinputid "
                                        End If
                                        firstTimeCompoundAddedAlert = True
                                    End If

                                End If

                                executeTransactedSQLCommand(0, queriesInsert)

                            End If

                        End If


                    Else

                        Dim queriesInsert(10) As String

                        If IsBase = True Then

                            If isManoDeObra = False Then
                                queriesInsert(0) = "DELETE FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardInputs WHERE ibaseid = " & iprojectid & " AND icardid = " & icardid & " AND iinputid = " & iselectedinputid
                                queriesInsert(1) = "DELETE FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardCompoundInputs WHERE ibaseid = " & iprojectid & " AND icardid = " & icardid & " AND iinputid = " & iselectedinputid
                            End If

                            queriesInsert(2) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardInputs SELECT " & baseid & ", " & icardid & ", iinputid, sinputunit, " & cantidaddeinsumo & ", " & fecha & ", '" & hora & "', '" & susername & "' FROM inputs WHERE iinputid = " & bi.iinputid

                            If isManoDeObra = True Then

                                busquedaMO = "" & _
                                "SELECT COUNT(*) " & _
                                "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardInputs btfi " & _
                                "JOIN inputtypes it ON it.iinputid = btfi.iinputid " & _
                                "WHERE btfi.ibaseid = " & baseid & " AND btfi.icardid = " & icardid & " " & _
                                "AND it.sinputtypedescription = 'MANO DE OBRA' "

                                conteoMO = getSQLQueryAsDouble(0, busquedaMO)

                                If conteoMO = 0 Then
                                    queriesInsert(3) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardInputs SELECT " & baseid & ", " & icardid & ", 0, '%', 1, " & fecha & ", '" & hora & "', '" & susername & "'"
                                End If

                            End If

                            If isCompound = True Then

                                If getSQLQueryAsBoolean(0, "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardInputs WHERE ibaseid = " & iprojectid & " AND iinputid = " & bi.iinputid) = True Then
                                    queriesInsert(4) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardCompoundInputs SELECT * FROM (SELECT " & iprojectid & " AS projectid, " & icardid & " AS cardid, " & bi.iinputid & " AS inputid, bcci.icompoundinputid, i.sinputunit, bcci.dcompoundinputqty AS qty, " & fecha & ", '" & hora & "', '" & susername & "' FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardInputs bci JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardCompoundInputs bcci ON bci.ibaseid = bcci.ibaseid AND bci.icardid = bcci.icardid AND bci.iinputid = bcci.iinputid JOIN inputs i ON i.iinputid = bcci.icompoundinputid WHERE bci.iinputid = " & bi.iinputid & " ORDER BY bci.iupdatedate DESC, bci.supdatetime DESC) cid GROUP BY icompoundinputid "
                                Else
                                    If getSQLQueryAsBoolean(0, "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardInputs WHERE ibaseid = " & baseid & " AND iinputid = " & bi.iinputid) = True Then
                                        queriesInsert(4) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardCompoundInputs SELECT * FROM (SELECT " & iprojectid & " AS projectid, " & icardid & " AS cardid, " & bi.iinputid & " AS inputid, bcci.icompoundinputid, i.sinputunit, bcci.dcompoundinputqty AS qty, " & fecha & ", '" & hora & "', '" & susername & "' FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardInputs bci JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardCompoundInputs bcci ON bci.ibaseid = bcci.ibaseid AND bci.icardid = bcci.icardid AND bci.iinputid = bcci.iinputid JOIN inputs i ON i.iinputid = bcci.icompoundinputid WHERE bci.ibaseid = " & baseid & " AND bci.iinputid = " & bi.iinputid & " ORDER BY bci.iupdatedate DESC, bci.supdatetime DESC) cid GROUP BY icompoundinputid "
                                    Else
                                        queriesInsert(4) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardCompoundInputs SELECT * FROM (SELECT " & iprojectid & " AS projectid, " & icardid & " AS cardid, " & bi.iinputid & " AS inputid, bcci.icompoundinputid, i.sinputunit, 1 AS qty, " & fecha & ", '" & hora & "', '" & susername & "' FROM inputs i WHERE i.iinputid = " & bi.iinputid & " ORDER BY i.iupdatedate DESC, i.supdatetime DESC) cid GROUP BY icompoundinputid "
                                    End If
                                    firstTimeCompoundAddedAlert = True
                                End If

                            End If

                            executeTransactedSQLCommand(0, queriesInsert)

                        ElseIf IsModel = True Then

                            If isManoDeObra = False Then
                                queriesInsert(0) = "DELETE FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardInputs WHERE ibaseid = " & iprojectid & " AND icardid = " & icardid & " AND iinputid = " & iselectedinputid
                                queriesInsert(1) = "DELETE FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardCompoundInputs WHERE ibaseid = " & iprojectid & " AND icardid = " & icardid & " AND iinputid = " & iselectedinputid
                            End If

                            queriesInsert(2) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardInputs SELECT " & baseid & ", " & icardid & ", iinputid, sinputunit, " & cantidaddeinsumo & ", " & fecha & ", '" & hora & "', '" & susername & "' FROM inputs WHERE iinputid = " & bi.iinputid

                            If isManoDeObra = True Then

                                busquedaMO = "" & _
                                "SELECT COUNT(*) " & _
                                "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardInputs btfi " & _
                                "JOIN inputtypes it ON it.iinputid = btfi.iinputid " & _
                                "WHERE btfi.ibaseid = " & baseid & " AND btfi.icardid = " & icardid & " " & _
                                "AND it.sinputtypedescription = 'MANO DE OBRA' "

                                conteoMO = getSQLQueryAsDouble(0, busquedaMO)

                                If conteoMO = 0 Then
                                    queriesInsert(3) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardInputs SELECT " & baseid & ", " & icardid & ", 0, '%', 1, " & fecha & ", '" & hora & "', '" & susername & "'"
                                End If

                            End If

                            If isCompound = True Then

                                If getSQLQueryAsBoolean(0, "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardInputs WHERE ibaseid = " & iprojectid & " AND iinputid = " & bi.iinputid) = True Then
                                    queriesInsert(4) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardCompoundInputs SELECT * FROM (SELECT " & iprojectid & " AS projectid, " & icardid & " AS cardid, " & bi.iinputid & " AS inputid, bcci.icompoundinputid, i.sinputunit, bcci.dcompoundinputqty AS qty, " & fecha & ", '" & hora & "', '" & susername & "' FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardInputs bci JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardCompoundInputs bcci ON bci.ibaseid = bcci.ibaseid AND bci.icardid = bcci.icardid AND bci.iinputid = bcci.iinputid JOIN inputs i ON i.iinputid = bcci.icompoundinputid WHERE bci.iinputid = " & bi.iinputid & " ORDER BY bci.iupdatedate DESC, bci.supdatetime DESC) cid GROUP BY icompoundinputid "
                                Else
                                    If getSQLQueryAsBoolean(0, "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardInputs WHERE ibaseid = " & baseid & " AND iinputid = " & bi.iinputid) = True Then
                                        queriesInsert(4) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardCompoundInputs SELECT * FROM (SELECT " & iprojectid & " AS projectid, " & icardid & " AS cardid, " & bi.iinputid & " AS inputid, bcci.icompoundinputid, i.sinputunit, bcci.dcompoundinputqty AS qty, " & fecha & ", '" & hora & "', '" & susername & "' FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardInputs bci JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardCompoundInputs bcci ON bci.ibaseid = bcci.ibaseid AND bci.icardid = bcci.icardid AND bci.iinputid = bcci.iinputid JOIN inputs i ON i.iinputid = bcci.icompoundinputid WHERE bci.ibaseid = " & baseid & " AND bci.iinputid = " & bi.iinputid & " ORDER BY bci.iupdatedate DESC, bci.supdatetime DESC) cid GROUP BY icompoundinputid "
                                    Else
                                        queriesInsert(4) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardCompoundInputs SELECT * FROM (SELECT " & iprojectid & " AS projectid, " & icardid & " AS cardid, " & bi.iinputid & " AS inputid, bcci.icompoundinputid, i.sinputunit, 1 AS qty, " & fecha & ", '" & hora & "', '" & susername & "' FROM inputs i WHERE i.iinputid = " & bi.iinputid & " ORDER BY i.iupdatedate DESC, i.supdatetime DESC) cid GROUP BY icompoundinputid "
                                    End If
                                    firstTimeCompoundAddedAlert = True
                                End If

                            End If

                            If isManoDeObra = False Then
                                queriesInsert(5) = "DELETE FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "CardInputs WHERE imodelid = " & iprojectid & " AND icardid = " & icardid & " AND iinputid = " & iselectedinputid
                                queriesInsert(6) = "DELETE FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "CardCompoundInputs WHERE imodelid = " & iprojectid & " AND icardid = " & icardid & " AND iinputid = " & iselectedinputid
                            End If

                            queriesInsert(7) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "CardInputs SELECT " & iprojectid & ", " & icardid & ", iinputid, sinputunit, " & cantidaddeinsumo & ", " & fecha & ", '" & hora & "', '" & susername & "' FROM inputs WHERE iinputid = " & bi.iinputid

                            If isManoDeObra = True Then

                                busquedaMO = "" & _
                                "SELECT COUNT(*) " & _
                                "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "CardInputs mtfi " & _
                                "JOIN inputtypes it ON it.iinputid = mtfi.iinputid " & _
                                "WHERE mtfi.imodelid = " & iprojectid & " AND mtfi.icardid = " & icardid & " " & _
                                "AND it.sinputtypedescription = 'MANO DE OBRA' "

                                conteoMO = getSQLQueryAsDouble(0, busquedaMO)

                                If conteoMO = 0 Then
                                    queriesInsert(8) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "CardInputs SELECT " & iprojectid & ", " & icardid & ", 0, '%', 1, " & fecha & ", '" & hora & "', '" & susername & "'"
                                End If

                            End If

                            If isCompound = True Then

                                If getSQLQueryAsBoolean(0, "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "CardInputs WHERE imodelid = " & iprojectid & " AND iinputid = " & bi.iinputid) = True Then
                                    queriesInsert(9) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "CardCompoundInputs SELECT * FROM (SELECT " & iprojectid & " AS projectid, " & icardid & " AS cardid, " & bi.iinputid & " AS inputid, bcci.icompoundinputid, i.sinputunit, bcci.dcompoundinputqty AS qty, " & fecha & ", '" & hora & "', '" & susername & "' FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "CardInputs bci JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "CardCompoundInputs bcci ON bci.imodelid = bcci.imodelid AND bci.icardid = bcci.icardid AND bci.iinputid = bcci.iinputid JOIN inputs i ON i.iinputid = bcci.icompoundinputid WHERE bci.iinputid = " & bi.iinputid & " ORDER BY bci.iupdatedate DESC, bci.supdatetime DESC) cid GROUP BY icompoundinputid "
                                Else
                                    If getSQLQueryAsBoolean(0, "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardInputs WHERE ibaseid = " & baseid & " AND iinputid = " & bi.iinputid) = True Then
                                        queriesInsert(9) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "CardCompoundInputs SELECT * FROM (SELECT " & iprojectid & " AS projectid, " & icardid & " AS cardid, " & bi.iinputid & " AS inputid, bcci.icompoundinputid, i.sinputunit, bcci.dcompoundinputqty AS qty, " & fecha & ", '" & hora & "', '" & susername & "' FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardInputs bci JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardCompoundInputs bcci ON bci.ibaseid = bcci.ibaseid AND bci.icardid = bcci.icardid AND bci.iinputid = bcci.iinputid JOIN inputs i ON i.iinputid = bcci.icompoundinputid WHERE bci.ibaseid = " & baseid & " AND bci.iinputid = " & bi.iinputid & " ORDER BY bci.iupdatedate DESC, bci.supdatetime DESC) cid GROUP BY icompoundinputid "
                                    Else
                                        queriesInsert(9) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "CardCompoundInputs SELECT * FROM (SELECT " & iprojectid & " AS projectid, " & icardid & " AS cardid, " & bi.iinputid & " AS inputid, bcci.icompoundinputid, i.sinputunit, 1 AS qty, " & fecha & ", '" & hora & "', '" & susername & "' FROM inputs i WHERE i.iinputid = " & bi.iinputid & " ORDER BY i.iupdatedate DESC, i.supdatetime DESC) cid GROUP BY icompoundinputid "
                                    End If
                                    firstTimeCompoundAddedAlert = True
                                End If

                            End If

                            executeTransactedSQLCommand(0, queriesInsert)

                        Else

                            If isManoDeObra = False Then
                                queriesInsert(0) = "DELETE FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardInputs WHERE ibaseid = " & iprojectid & " AND icardid = " & icardid & " AND iinputid = " & iselectedinputid
                                queriesInsert(1) = "DELETE FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardCompoundInputs WHERE ibaseid = " & iprojectid & " AND icardid = " & icardid & " AND iinputid = " & iselectedinputid
                            End If

                            queriesInsert(2) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardInputs SELECT " & baseid & ", " & icardid & ", iinputid, sinputunit, " & cantidaddeinsumo & ", " & fecha & ", '" & hora & "', '" & susername & "' FROM inputs WHERE iinputid = " & bi.iinputid

                            If isManoDeObra = True Then

                                busquedaMO = "" & _
                                "SELECT COUNT(*) " & _
                                "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardInputs btfi " & _
                                "JOIN inputtypes it ON it.iinputid = btfi.iinputid " & _
                                "WHERE btfi.ibaseid = " & baseid & " AND btfi.icardid = " & icardid & " " & _
                                "AND it.sinputtypedescription = 'MANO DE OBRA' "

                                conteoMO = getSQLQueryAsDouble(0, busquedaMO)

                                If conteoMO = 0 Then
                                    queriesInsert(3) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardInputs SELECT " & baseid & ", " & icardid & ", 0, '%', 1, " & fecha & ", '" & hora & "', '" & susername & "'"
                                End If

                            End If

                            If isCompound = True Then

                                If getSQLQueryAsBoolean(0, "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardInputs WHERE ibaseid = " & iprojectid & " AND iinputid = " & bi.iinputid) = True Then
                                    queriesInsert(4) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardCompoundInputs SELECT * FROM (SELECT " & iprojectid & " AS projectid, " & icardid & " AS cardid, " & bi.iinputid & " AS inputid, bcci.icompoundinputid, i.sinputunit, bcci.dcompoundinputqty AS qty, " & fecha & ", '" & hora & "', '" & susername & "' FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardInputs bci JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardCompoundInputs bcci ON bci.ibaseid = bcci.ibaseid AND bci.icardid = bcci.icardid AND bci.iinputid = bcci.iinputid JOIN inputs i ON i.iinputid = bcci.icompoundinputid WHERE bci.iinputid = " & bi.iinputid & " ORDER BY bci.iupdatedate DESC, bci.supdatetime DESC) cid GROUP BY icompoundinputid "
                                Else
                                    If getSQLQueryAsBoolean(0, "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardInputs WHERE ibaseid = " & baseid & " AND iinputid = " & bi.iinputid) = True Then
                                        queriesInsert(4) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardCompoundInputs SELECT * FROM (SELECT " & iprojectid & " AS projectid, " & icardid & " AS cardid, " & bi.iinputid & " AS inputid, bcci.icompoundinputid, i.sinputunit, bcci.dcompoundinputqty AS qty, " & fecha & ", '" & hora & "', '" & susername & "' FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardInputs bci JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardCompoundInputs bcci ON bci.ibaseid = bcci.ibaseid AND bci.icardid = bcci.icardid AND bci.iinputid = bcci.iinputid JOIN inputs i ON i.iinputid = bcci.icompoundinputid WHERE bci.ibaseid = " & baseid & " AND bci.iinputid = " & bi.iinputid & " ORDER BY bci.iupdatedate DESC, bci.supdatetime DESC) cid GROUP BY icompoundinputid "
                                    Else
                                        queriesInsert(4) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardCompoundInputs SELECT * FROM (SELECT " & iprojectid & " AS projectid, " & icardid & " AS cardid, " & bi.iinputid & " AS inputid, bcci.icompoundinputid, i.sinputunit, 1 AS qty, " & fecha & ", '" & hora & "', '" & susername & "' FROM inputs i WHERE i.iinputid = " & bi.iinputid & " ORDER BY i.iupdatedate DESC, i.supdatetime DESC) cid GROUP BY icompoundinputid "
                                    End If
                                    firstTimeCompoundAddedAlert = True
                                End If

                            End If

                            If isManoDeObra = False Then
                                queriesInsert(5) = "DELETE FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "CardInputs WHERE iprojectid = " & iprojectid & " AND icardid = " & icardid & " AND iinputid = " & iselectedinputid
                                queriesInsert(6) = "DELETE FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "CardCompoundInputs WHERE iprojectid = " & iprojectid & " AND icardid = " & icardid & " AND iinputid = " & iselectedinputid
                            End If

                            queriesInsert(7) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "CardInputs SELECT " & iprojectid & ", " & icardid & ", iinputid, sinputunit, " & cantidaddeinsumo & ", " & fecha & ", '" & hora & "', '" & susername & "' FROM inputs WHERE iinputid = " & bi.iinputid

                            If isManoDeObra = True Then

                                busquedaMO = "" & _
                                "SELECT COUNT(*) " & _
                                "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "CardInputs ptfi " & _
                                "JOIN inputtypes it ON it.iinputid = ptfi.iinputid " & _
                                "WHERE ptfi.iprojectid = " & iprojectid & " AND ptfi.icardid = " & icardid & " " & _
                                "AND it.sinputtypedescription = 'MANO DE OBRA' "

                                conteoMO = getSQLQueryAsDouble(0, busquedaMO)

                                If conteoMO = 0 Then
                                    queriesInsert(8) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "CardInputs SELECT " & iprojectid & ", " & icardid & ", 0, '%', 1, " & fecha & ", '" & hora & "', '" & susername & "'"
                                End If

                            End If

                            If isCompound = True Then

                                If getSQLQueryAsBoolean(0, "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "CardInputs WHERE iprojectid = " & iprojectid & " AND iinputid = " & bi.iinputid) = True Then
                                    queriesInsert(9) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "CardCompoundInputs SELECT * FROM (SELECT " & iprojectid & " AS projectid, " & icardid & " AS cardid, " & bi.iinputid & " AS inputid, bcci.icompoundinputid, i.sinputunit, bcci.dcompoundinputqty AS qty, " & fecha & ", '" & hora & "', '" & susername & "' FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "CardInputs bci JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "CardCompoundInputs bcci ON bci.iprojectid = bcci.iprojectid AND bci.icardid = bcci.icardid AND bci.iinputid = bcci.iinputid JOIN inputs i ON i.iinputid = bcci.icompoundinputid WHERE bci.iinputid = " & bi.iinputid & " ORDER BY bci.iupdatedate DESC, bci.supdatetime DESC) cid GROUP BY icompoundinputid "
                                Else
                                    If getSQLQueryAsBoolean(0, "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardInputs WHERE ibaseid = " & baseid & " AND iinputid = " & bi.iinputid) = True Then
                                        queriesInsert(9) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "CardCompoundInputs SELECT * FROM (SELECT " & iprojectid & " AS projectid, " & icardid & " AS cardid, " & bi.iinputid & " AS inputid, bcci.icompoundinputid, i.sinputunit, bcci.dcompoundinputqty AS qty, " & fecha & ", '" & hora & "', '" & susername & "' FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardInputs bci JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardCompoundInputs bcci ON bci.ibaseid = bcci.ibaseid AND bci.icardid = bcci.icardid AND bci.iinputid = bcci.iinputid JOIN inputs i ON i.iinputid = bcci.icompoundinputid WHERE bci.ibaseid = " & baseid & " AND bci.iinputid = " & bi.iinputid & " ORDER BY bci.iupdatedate DESC, bci.supdatetime DESC) cid GROUP BY icompoundinputid "
                                    Else
                                        queriesInsert(9) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "CardCompoundInputs SELECT * FROM (SELECT " & iprojectid & " AS projectid, " & icardid & " AS cardid, " & bi.iinputid & " AS inputid, bcci.icompoundinputid, i.sinputunit, 1 AS qty, " & fecha & ", '" & hora & "', '" & susername & "' FROM inputs i WHERE i.iinputid = " & bi.iinputid & " ORDER BY i.iupdatedate DESC, i.supdatetime DESC) cid GROUP BY icompoundinputid "
                                    End If
                                    firstTimeCompoundAddedAlert = True
                                End If

                            End If

                            executeTransactedSQLCommand(0, queriesInsert)

                        End If


                    End If

                Else

                    'Si cancela el reemplazo de insumo
                    'dgvConceptosTarjeta.Rows(e.RowIndex).Cells(e.ColumnIndex).Value = sselectedinputdescription

                End If

            End If


        ElseIf e.ColumnIndex = 4 Then

            'dcardinputqty

            If dgvConceptosTarjeta.Rows(e.RowIndex).Cells(e.ColumnIndex).Value Is DBNull.Value Then

                If MsgBox("¿Estás seguro de que quieres eliminar el Insumo " & sselectedinputdescription & " de la Tarjeta?", MsgBoxStyle.YesNo, "Confirmación de Eliminación de Insumo de la Tarjeta") = MsgBoxResult.Yes Then

                    Dim conteoMO As Integer = 0
                    Dim conteoEQ As Integer = 0

                    Dim busquedaMO As String = ""
                    Dim busquedaEQ As String = ""

                    If IsBase = True Then

                        busquedaMO = "" & _
                        "SELECT COUNT(*) " & _
                        "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardInputs btfi " & _
                        "JOIN inputtypes it ON it.iinputid = btfi.iinputid " & _
                        "WHERE btfi.ibaseid = " & iprojectid & " AND btfi.icardid = " & icardid & " " & _
                        "AND it.sinputtypedescription = 'MANO DE OBRA' "

                        busquedaEQ = "" & _
                        "SELECT COUNT(*) " & _
                        "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardInputs btfi " & _
                        "JOIN inputtypes it ON it.iinputid = btfi.iinputiconteoEQ = getSQLQueryAsDouble(0, busquedaEQ)d " & _
                        "WHERE btfi.ibaseid = " & iprojectid & " AND btfi.icardid = " & icardid & " " & _
                        "AND it.sinputtypedescription = 'EQUIPO Y HERRAMIENTA' "

                        conteoMO = getSQLQueryAsDouble(0, busquedaMO)
                        conteoEQ = getSQLQueryAsDouble(0, busquedaEQ)

                        Dim queriesDelete(2) As String

                        If conteoMO = 0 Then
                            'Continue
                        ElseIf conteoMO > 1 Then
                            queriesDelete(0) = "DELETE FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardInputs WHERE ibaseid = " & iprojectid & " AND icardid = " & icardid & " AND iinputid = " & iselectedinputid
                        Else
                            MsgBox("No es posible dejar una Tarjeta sin Mano de Obra", MsgBoxStyle.OkOnly, "Mano de Obra Requerida")
                            Exit Sub
                        End If

                        If conteoEQ = 0 Then
                            'Continue
                        ElseIf conteoEQ > 1 Then
                            queriesDelete(0) = "DELETE FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardInputs WHERE ibaseid = " & iprojectid & " AND icardid = " & icardid & " AND iinputid = " & iselectedinputid
                        Else
                            MsgBox("No es posible dejar una Tarjeta sin Equipo y Herramientas", MsgBoxStyle.OkOnly, "Equipo y Herramientas Requeridos")
                            Exit Sub
                        End If

                        queriesDelete(0) = "DELETE FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardInputs WHERE ibaseid = " & iprojectid & " AND icardid = " & icardid & " AND iinputid = " & iselectedinputid

                        Dim conteoCompound As Integer = 0

                        conteoCompound = getSQLQueryAsInteger(0, "SELECT COUNT(*) FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardCompoundInputs WHERE iinputid = " & iselectedinputid)

                        If conteoCompound > 0 Then
                            queriesDelete(1) = "DELETE FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardCompoundInputs WHERE ibaseid = " & iprojectid & " AND icardid = " & icardid & " AND iinputid = " & iselectedinputid
                        End If

                        executeTransactedSQLCommand(0, queriesDelete)

                    Else

                        If IsModel = True Then

                            busquedaMO = "" & _
                            "SELECT COUNT(*) " & _
                            "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "CardInputs mtfi " & _
                            "JOIN inputtypes it ON it.iinputid = mtfi.iinputid " & _
                            "WHERE mtfi.imodelid = " & iprojectid & " AND mtfi.icardid = " & icardid & " " & _
                            "AND it.sinputtypedescription = 'MANO DE OBRA' "

                            busquedaEQ = "" & _
                            "SELECT COUNT(*) " & _
                            "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "CardInputs mtfi " & _
                            "JOIN inputtypes it ON it.iinputid = mtfi.iinputid " & _
                            "WHERE mtfi.imodelid = " & iprojectid & " AND mtfi.icardid = " & icardid & " " & _
                            "AND it.sinputtypedescription = 'EQUIPO Y HERRAMIENTA' "

                            conteoMO = getSQLQueryAsDouble(0, busquedaMO)
                            conteoEQ = getSQLQueryAsDouble(0, busquedaEQ)

                            Dim queriesDelete(2) As String

                            If conteoMO = 0 Then
                                'Continue
                            ElseIf conteoMO > 1 Then
                                queriesDelete(0) = "DELETE FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "CardInputs WHERE imodelid = " & iprojectid & " AND icardid = " & icardid & " AND iinputid = " & iselectedinputid
                            Else
                                MsgBox("No es posible dejar una Tarjeta sin Mano de Obra", MsgBoxStyle.OkOnly, "Mano de Obra Requerida")
                                Exit Sub
                            End If

                            If conteoEQ = 0 Then
                                'Continue
                            ElseIf conteoEQ > 1 Then
                                queriesDelete(0) = "DELETE FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "CardInputs WHERE imodelid = " & iprojectid & " AND icardid = " & icardid & " AND iinputid = " & iselectedinputid
                            Else
                                MsgBox("No es posible dejar una Tarjeta sin Equipo y Herramientas", MsgBoxStyle.OkOnly, "Equipo y Herramientas Requeridos")
                                Exit Sub
                            End If

                            queriesDelete(0) = "DELETE FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "CardInputs WHERE imodelid = " & iprojectid & " AND icardid = " & icardid & " AND iinputid = " & iselectedinputid

                            Dim conteoCompound As Integer = 0

                            conteoCompound = getSQLQueryAsInteger(0, "SELECT COUNT(*) FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardCompoundInputs WHERE iinputid = " & iselectedinputid)

                            If conteoCompound > 0 Then
                                queriesDelete(1) = "DELETE FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "CardCompoundInputs WHERE imodelid = " & iprojectid & " AND icardid = " & icardid & " AND iinputid = " & iselectedinputid
                            End If

                            executeTransactedSQLCommand(0, queriesDelete)

                        Else

                            busquedaMO = "" & _
                            "SELECT COUNT(*) " & _
                            "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "CardInputs ptfi " & _
                            "JOIN inputtypes it ON it.iinputid = ptfi.iinputid " & _
                            "WHERE ptfi.iprojectid = " & iprojectid & " AND ptfi.icardid = " & icardid & " " & _
                            "AND it.sinputtypedescription = 'MANO DE OBRA' "

                            busquedaEQ = "" & _
                            "SELECT COUNT(*) " & _
                            "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "CardInputs ptfi " & _
                            "JOIN inputtypes it ON it.iinputid = ptfi.iinputid " & _
                            "WHERE ptfi.iprojectid = " & iprojectid & " AND ptfi.icardid = " & icardid & " " & _
                            "AND it.sinputtypedescription = 'EQUIPO Y HERRAMIENTA' "

                            conteoMO = getSQLQueryAsDouble(0, busquedaMO)
                            conteoEQ = getSQLQueryAsDouble(0, busquedaEQ)

                            Dim queriesDelete(2) As String

                            If conteoMO = 0 Then
                                'Continue
                            ElseIf conteoMO > 1 Then
                                queriesDelete(0) = "DELETE FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "CardInputs WHERE iprojectid = " & iprojectid & " AND icardid = " & icardid & " AND iinputid = " & iselectedinputid
                            Else
                                MsgBox("No es posible dejar una Tarjeta sin Mano de Obra", MsgBoxStyle.OkOnly, "Mano de Obra Requerida")
                                Exit Sub
                            End If

                            If conteoEQ = 0 Then
                                'Continue
                            ElseIf conteoEQ > 1 Then
                                queriesDelete(0) = "DELETE FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "CardInputs WHERE iprojectid = " & iprojectid & " AND icardid = " & icardid & " AND iinputid = " & iselectedinputid
                            Else
                                MsgBox("No es posible dejar una Tarjeta sin Equipo y Herramientas", MsgBoxStyle.OkOnly, "Equipo y Herramientas Requeridos")
                                Exit Sub
                            End If

                            queriesDelete(0) = "DELETE FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "CardInputs WHERE iprojectid = " & iprojectid & " AND icardid = " & icardid & " AND iinputid = " & iselectedinputid

                            Dim conteoCompound As Integer = 0

                            conteoCompound = getSQLQueryAsInteger(0, "SELECT COUNT(*) FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardCompoundInputs WHERE iinputid = " & iselectedinputid)

                            If conteoCompound > 0 Then
                                queriesDelete(1) = "DELETE FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "CardCompoundInputs WHERE iprojectid = " & iprojectid & " AND icardid = " & icardid & " AND iinputid = " & iselectedinputid
                            End If

                            executeTransactedSQLCommand(0, queriesDelete)

                        End If

                    End If

                Else

                    'dgvConceptosTarjeta.Rows(e.RowIndex).Cells(e.ColumnIndex).Value = dselectedinputqty

                End If

            ElseIf dgvConceptosTarjeta.Rows(e.RowIndex).Cells(e.ColumnIndex).Value = 0 Then

                If MsgBox("¿Estás seguro de que quieres eliminar el Insumo " & sselectedinputdescription & " de la Tarjeta?", MsgBoxStyle.YesNo, "Confirmación de Eliminación de Insumo de la Tarjeta") = MsgBoxResult.Yes Then

                    Dim conteoMO As Integer = 0
                    Dim conteoEQ As Integer = 0

                    Dim busquedaMO As String = ""
                    Dim busquedaEQ As String = ""

                    If IsBase = True Then

                        busquedaMO = "" & _
                        "SELECT COUNT(*) " & _
                        "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardInputs btfi " & _
                        "JOIN inputtypes it ON it.iinputid = btfi.iinputid " & _
                        "WHERE btfi.ibaseid = " & iprojectid & " AND btfi.icardid = " & icardid & " " & _
                        "AND it.sinputtypedescription = 'MANO DE OBRA' "

                        busquedaEQ = "" & _
                        "SELECT COUNT(*) " & _
                        "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardInputs btfi " & _
                        "JOIN inputtypes it ON it.iinputid = btfi.iinputiconteoEQ = getSQLQueryAsDouble(0, busquedaEQ)d " & _
                        "WHERE btfi.ibaseid = " & iprojectid & " AND btfi.icardid = " & icardid & " " & _
                        "AND it.sinputtypedescription = 'EQUIPO Y HERRAMIENTA' "

                        conteoMO = getSQLQueryAsDouble(0, busquedaMO)
                        conteoEQ = getSQLQueryAsDouble(0, busquedaEQ)

                        Dim queriesDelete(2) As String

                        If conteoMO = 0 Then
                            'Continue
                        ElseIf conteoMO > 1 Then
                            queriesDelete(0) = "DELETE FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardInputs WHERE ibaseid = " & iprojectid & " AND icardid = " & icardid & " AND iinputid = " & iselectedinputid
                        Else
                            MsgBox("No es posible dejar una Tarjeta sin Mano de Obra", MsgBoxStyle.OkOnly, "Mano de Obra Requerida")
                            Exit Sub
                        End If

                        If conteoEQ = 0 Then
                            'Continue
                        ElseIf conteoEQ > 1 Then
                            queriesDelete(0) = "DELETE FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardInputs WHERE ibaseid = " & iprojectid & " AND icardid = " & icardid & " AND iinputid = " & iselectedinputid
                        Else
                            MsgBox("No es posible dejar una Tarjeta sin Equipo y Herramientas", MsgBoxStyle.OkOnly, "Equipo y Herramientas Requeridos")
                            Exit Sub
                        End If

                        queriesDelete(0) = "DELETE FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardInputs WHERE ibaseid = " & iprojectid & " AND icardid = " & icardid & " AND iinputid = " & iselectedinputid

                        Dim conteoCompound As Integer = 0

                        conteoCompound = getSQLQueryAsInteger(0, "SELECT COUNT(*) FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardCompoundInputs WHERE iinputid = " & iselectedinputid)

                        If conteoCompound > 0 Then
                            queriesDelete(1) = "DELETE FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardCompoundInputs WHERE ibaseid = " & iprojectid & " AND icardid = " & icardid & " AND iinputid = " & iselectedinputid
                        End If

                        executeTransactedSQLCommand(0, queriesDelete)

                    Else

                        If IsModel = True Then

                            busquedaMO = "" & _
                            "SELECT COUNT(*) " & _
                            "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "CardInputs mtfi " & _
                            "JOIN inputtypes it ON it.iinputid = mtfi.iinputid " & _
                            "WHERE mtfi.imodelid = " & iprojectid & " AND mtfi.icardid = " & icardid & " " & _
                            "AND it.sinputtypedescription = 'MANO DE OBRA' "

                            busquedaEQ = "" & _
                            "SELECT COUNT(*) " & _
                            "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "CardInputs mtfi " & _
                            "JOIN inputtypes it ON it.iinputid = mtfi.iinputid " & _
                            "WHERE mtfi.imodelid = " & iprojectid & " AND mtfi.icardid = " & icardid & " " & _
                            "AND it.sinputtypedescription = 'EQUIPO Y HERRAMIENTA' "

                            conteoMO = getSQLQueryAsDouble(0, busquedaMO)
                            conteoEQ = getSQLQueryAsDouble(0, busquedaEQ)

                            Dim queriesDelete(2) As String

                            If conteoMO = 0 Then
                                'Continue
                            ElseIf conteoMO > 1 Then
                                queriesDelete(0) = "DELETE FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "CardInputs WHERE imodelid = " & iprojectid & " AND icardid = " & icardid & " AND iinputid = " & iselectedinputid
                            Else
                                MsgBox("No es posible dejar una Tarjeta sin Mano de Obra", MsgBoxStyle.OkOnly, "Mano de Obra Requerida")
                                Exit Sub
                            End If

                            If conteoEQ = 0 Then
                                'Continue
                            ElseIf conteoEQ > 1 Then
                                queriesDelete(0) = "DELETE FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "CardInputs WHERE imodelid = " & iprojectid & " AND icardid = " & icardid & " AND iinputid = " & iselectedinputid
                            Else
                                MsgBox("No es posible dejar una Tarjeta sin Equipo y Herramientas", MsgBoxStyle.OkOnly, "Equipo y Herramientas Requeridos")
                                Exit Sub
                            End If

                            queriesDelete(0) = "DELETE FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "CardInputs WHERE imodelid = " & iprojectid & " AND icardid = " & icardid & " AND iinputid = " & iselectedinputid

                            Dim conteoCompound As Integer = 0

                            conteoCompound = getSQLQueryAsInteger(0, "SELECT COUNT(*) FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardCompoundInputs WHERE iinputid = " & iselectedinputid)

                            If conteoCompound > 0 Then
                                queriesDelete(1) = "DELETE FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "CardCompoundInputs WHERE imodelid = " & iprojectid & " AND icardid = " & icardid & " AND iinputid = " & iselectedinputid
                            End If

                            executeTransactedSQLCommand(0, queriesDelete)

                        Else

                            busquedaMO = "" & _
                            "SELECT COUNT(*) " & _
                            "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "CardInputs ptfi " & _
                            "JOIN inputtypes it ON it.iinputid = ptfi.iinputid " & _
                            "WHERE ptfi.iprojectid = " & iprojectid & " AND ptfi.icardid = " & icardid & " " & _
                            "AND it.sinputtypedescription = 'MANO DE OBRA' "

                            busquedaEQ = "" & _
                            "SELECT COUNT(*) " & _
                            "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "CardInputs ptfi " & _
                            "JOIN inputtypes it ON it.iinputid = ptfi.iinputid " & _
                            "WHERE ptfi.iprojectid = " & iprojectid & " AND ptfi.icardid = " & icardid & " " & _
                            "AND it.sinputtypedescription = 'EQUIPO Y HERRAMIENTA' "

                            conteoMO = getSQLQueryAsDouble(0, busquedaMO)
                            conteoEQ = getSQLQueryAsDouble(0, busquedaEQ)

                            Dim queriesDelete(2) As String

                            If conteoMO = 0 Then
                                'Continue
                            ElseIf conteoMO > 1 Then
                                queriesDelete(0) = "DELETE FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "CardInputs WHERE iprojectid = " & iprojectid & " AND icardid = " & icardid & " AND iinputid = " & iselectedinputid
                            Else
                                MsgBox("No es posible dejar una Tarjeta sin Mano de Obra", MsgBoxStyle.OkOnly, "Mano de Obra Requerida")
                                Exit Sub
                            End If

                            If conteoEQ = 0 Then
                                'Continue
                            ElseIf conteoEQ > 1 Then
                                queriesDelete(0) = "DELETE FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "CardInputs WHERE iprojectid = " & iprojectid & " AND icardid = " & icardid & " AND iinputid = " & iselectedinputid
                            Else
                                MsgBox("No es posible dejar una Tarjeta sin Equipo y Herramientas", MsgBoxStyle.OkOnly, "Equipo y Herramientas Requeridos")
                                Exit Sub
                            End If

                            queriesDelete(0) = "DELETE FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "CardInputs WHERE iprojectid = " & iprojectid & " AND icardid = " & icardid & " AND iinputid = " & iselectedinputid

                            Dim conteoCompound As Integer = 0

                            conteoCompound = getSQLQueryAsInteger(0, "SELECT COUNT(*) FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardCompoundInputs WHERE iinputid = " & iselectedinputid)

                            If conteoCompound > 0 Then
                                queriesDelete(1) = "DELETE FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "CardCompoundInputs WHERE iprojectid = " & iprojectid & " AND icardid = " & icardid & " AND iinputid = " & iselectedinputid
                            End If

                            executeTransactedSQLCommand(0, queriesDelete)

                        End If

                    End If

                Else

                    'dgvConceptosTarjeta.Rows(e.RowIndex).Cells(e.ColumnIndex).Value = dselectedinputqty

                End If

            Else

                'Si pone un número

                Dim cantidaddeinsumo As Double = dselectedinputqty

                Try
                    cantidaddeinsumo = CDbl(dgvConceptosTarjeta.Rows(e.RowIndex).Cells(4).Value)
                Catch ex As Exception
                    cantidaddeinsumo = dselectedinputqty
                End Try

                If IsEdit = True Then


                    If IsBase = True Then

                        executeSQLCommand(0, "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardInputs SET dcardinputqty = " & cantidaddeinsumo & ", iupdatedate = " & fecha & ", supdatetime = '" & hora & "', supdateusername = '" & susername & "' WHERE ibaseid = " & iprojectid & " AND icardid = " & icardid & " AND iinputid = " & iselectedinputid)

                    Else

                        If IsModel = True Then

                            executeSQLCommand(0, "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "CardInputs SET dcardinputqty = " & cantidaddeinsumo & ", iupdatedate = " & fecha & ", supdatetime = '" & hora & "', supdateusername = '" & susername & "' WHERE imodelid = " & iprojectid & " AND icardid = " & icardid & " AND iinputid = " & iselectedinputid)

                        Else

                            executeSQLCommand(0, "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "CardInputs SET dcardinputqty = " & cantidaddeinsumo & ", iupdatedate = " & fecha & ", supdatetime = '" & hora & "', supdateusername = '" & susername & "' WHERE iprojectid = " & iprojectid & " AND icardid = " & icardid & " AND iinputid = " & iselectedinputid)

                        End If

                    End If


                Else

                    Dim queriesUpdate(2) As String

                    If IsBase = True Then

                        queriesUpdate(0) = "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardInputs SET dcardinputqty = " & cantidaddeinsumo & ", iupdatedate = " & fecha & ", supdatetime = '" & hora & "', supdateusername = '" & susername & "' WHERE ibaseid = " & baseid & " AND icardid = " & icardid & " AND iinputid = " & iselectedinputid

                    ElseIf IsModel = True Then

                        queriesUpdate(0) = "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardInputs SET dcardinputqty = " & cantidaddeinsumo & ", iupdatedate = " & fecha & ", supdatetime = '" & hora & "', supdateusername = '" & susername & "' WHERE ibaseid = " & baseid & " AND icardid = " & icardid & " AND iinputid = " & iselectedinputid
                        queriesUpdate(1) = "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "CardInputs SET dcardinputqty = " & cantidaddeinsumo & ", iupdatedate = " & fecha & ", supdatetime = '" & hora & "', supdateusername = '" & susername & "' WHERE imodelid = " & iprojectid & " AND icardid = " & icardid & " AND iinputid = " & iselectedinputid

                    Else

                        queriesUpdate(0) = "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardInputs SET dcardinputqty = " & cantidaddeinsumo & ", iupdatedate = " & fecha & ", supdatetime = '" & hora & "', supdateusername = '" & susername & "' WHERE ibaseid = " & baseid & " AND icardid = " & icardid & " AND iinputid = " & iselectedinputid
                        queriesUpdate(1) = "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "CardInputs SET dcardinputqty = " & cantidaddeinsumo & ", iupdatedate = " & fecha & ", supdatetime = '" & hora & "', supdateusername = '" & susername & "' WHERE iprojectid = " & iprojectid & " AND icardid = " & icardid & " AND iinputid = " & iselectedinputid

                    End If

                    executeTransactedSQLCommand(0, queriesUpdate)

                End If

            End If

            If firstTimeCompoundAddedAlert = True Then
                MsgBox("Como es la primera vez que insertas este Insumo Compuesto a este proyecto, te sugiero que revises sus rendimientos y cantidades", MsgBoxStyle.OkOnly Or MsgBoxStyle.Information, "Establecer rendimientos correctos")
            End If

        End If

        Cursor.Current = System.Windows.Forms.Cursors.Default

    End Sub


    Private Sub dgvConceptosTarjeta_EditingControlShowing(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewEditingControlShowingEventArgs) Handles dgvConceptosTarjeta.EditingControlShowing

        If dgvConceptosTarjeta.CurrentCell.ColumnIndex = 1 Then

            txtNombreDgvConceptosTarjeta = CType(e.Control, TextBox)
            txtNombreDgvConceptosTarjeta_OldText = txtNombreDgvConceptosTarjeta.Text

        ElseIf dgvConceptosTarjeta.CurrentCell.ColumnIndex = 4 Then

            txtCantidadDgvConceptosTarjeta = CType(e.Control, TextBox)
            txtCantidadDgvConceptosTarjeta_OldText = txtCantidadDgvConceptosTarjeta.Text

        Else
            txtCantidadDgvConceptosTarjeta = Nothing
            txtCantidadDgvConceptosTarjeta_OldText = Nothing
            txtNombreDgvConceptosTarjeta = Nothing
            txtNombreDgvConceptosTarjeta_OldText = Nothing
        End If

    End Sub


    Private Sub txtNombredgvConceptosTarjeta_KeyUp(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtNombreDgvConceptosTarjeta.KeyUp

        txtNombreDgvConceptosTarjeta.Text = txtNombreDgvConceptosTarjeta.Text.Replace("'", "").Replace("--", "").Replace("@", "").Replace("|", "")

        Dim strForbidden1 As String = "|@'\"""
        Dim arrayForbidden1 As Char() = strForbidden1.ToCharArray

        For fc = 0 To arrayForbidden1.Length - 1

            If txtNombreDgvConceptosTarjeta.Text.Contains(arrayForbidden1(fc)) Then
                txtNombreDgvConceptosTarjeta.Text = txtNombreDgvConceptosTarjeta.Text.Replace(arrayForbidden1(fc), "")
            End If

        Next fc

    End Sub


    Private Sub txtCantidaddgvConceptosTarjeta_KeyUp(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtCantidadDgvConceptosTarjeta.KeyUp

        If Not IsNumeric(txtCantidadDgvConceptosTarjeta.Text) Then

            Dim strForbidden2 As String = "abcdefghijklmnopqrstuvwxyzñABCDEFGHIJKLMNOPQRSTUVWXYZÑ|°!#$%&/()=?¡*¨[]_:;,-{}+´¿'¬^`~@\<>"
            Dim arrayForbidden2 As Char() = strForbidden2.ToCharArray

            For cp = 0 To arrayForbidden2.Length - 1

                If txtCantidadDgvConceptosTarjeta.Text.Contains(arrayForbidden2(cp)) Then
                    txtCantidadDgvConceptosTarjeta.Text = txtCantidadDgvConceptosTarjeta.Text.Replace(arrayForbidden2(cp), "")
                End If

            Next cp

            If txtCantidadDgvConceptosTarjeta.Text.Contains(".") Then

                Dim comparaPuntos As Char() = txtCantidadDgvConceptosTarjeta.Text.ToCharArray
                Dim cuantosPuntos As Integer = 0


                For letra = 0 To comparaPuntos.Length - 1

                    If comparaPuntos(letra) = "." Then
                        cuantosPuntos = cuantosPuntos + 1
                    End If

                Next

                If cuantosPuntos > 1 Then

                    For cantidad = 1 To cuantosPuntos
                        Dim lugar As Integer = txtCantidadDgvConceptosTarjeta.Text.LastIndexOf(".")
                        Dim longitud As Integer = txtCantidadDgvConceptosTarjeta.Text.Length

                        If longitud > (lugar + 1) Then
                            txtCantidadDgvConceptosTarjeta.Text = txtCantidadDgvConceptosTarjeta.Text.Substring(0, lugar) & txtCantidadDgvConceptosTarjeta.Text.Substring(lugar + 1)
                            Exit For
                        Else
                            txtCantidadDgvConceptosTarjeta.Text = txtCantidadDgvConceptosTarjeta.Text.Substring(0, lugar)
                            Exit For
                        End If
                    Next

                End If

            End If

            txtCantidadDgvConceptosTarjeta.Text = txtCantidadDgvConceptosTarjeta.Text.Replace("--", "").Replace("'", "")
            txtCantidadDgvConceptosTarjeta.Text = txtCantidadDgvConceptosTarjeta.Text.Trim

        Else
            txtCantidadDgvConceptosTarjeta_OldText = txtCantidadDgvConceptosTarjeta.Text
        End If

    End Sub


    Private Sub dgvConceptosTarjeta_UserAddedRow(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewRowEventArgs) Handles dgvConceptosTarjeta.UserAddedRow

        If insertInputPermission = False Then
            Exit Sub
        End If

        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        isConceptosTarjetaDGVReady = False

        Dim firstTimeCompoundAddedAlert As Boolean = False

        Dim fecha As Integer = 0
        Dim hora As String = ""

        fecha = getMySQLDate()
        hora = getAppTime()

        Dim baseid As Integer = 0
        baseid = getSQLQueryAsInteger(0, "SELECT ibaseid FROM base ORDER BY iupdatedate DESC, supdatetime DESC LIMIT 1")

        If baseid = 0 Then
            baseid = 99999
        End If

        Dim chequeoPrimeraVezQueSeInsertaAlgo As Integer = 0

        If IsBase = True Then
            chequeoPrimeraVezQueSeInsertaAlgo = getSQLQueryAsInteger(0, "SELECT COUNT(*) FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Prices WHERE ibaseid = " & iprojectid)
        Else

            If IsModel = True Then
                chequeoPrimeraVezQueSeInsertaAlgo = getSQLQueryAsInteger(0, "SELECT COUNT(*) FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "Prices WHERE imodelid = " & iprojectid)
            Else
                chequeoPrimeraVezQueSeInsertaAlgo = getSQLQueryAsInteger(0, "SELECT COUNT(*) FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "Prices WHERE iprojectid = " & iprojectid)
            End If

        End If

        If chequeoPrimeraVezQueSeInsertaAlgo = 0 Then

            If IsBase = True Then

                Dim queriesPrimeraVez(2) As String
                queriesPrimeraVez(0) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Prices SELECT " & iprojectid & ", iinputid, dinputpricewithoutIVA, dinputprotectionpercentage, dinputfinalprice, " & fecha & ", '" & hora & "', '" & susername & "' FROM (SELECT * FROM baseprices WHERE ibaseid = " & baseid & " ORDER BY iupdatedate DESC, supdatetime DESC) pp GROUP BY iinputid"
                queriesPrimeraVez(1) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Timber SELECT " & iprojectid & ", bt.iinputid, bt.dinputtimberespesor, bt.dinputtimberancho, bt.dinputtimberlargo, bt.dinputtimberpreciopiecubico, " & fecha & ", '" & hora & "', '" & susername & "' FROM basetimber bt where ibaseid = " & baseid
                executeTransactedSQLCommand(0, queriesPrimeraVez)

            Else

                If IsModel = True Then

                    Dim queriesPrimeraVez(2) As String
                    queriesPrimeraVez(0) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "Prices SELECT " & iprojectid & ", iinputid, dinputpricewithoutIVA, dinputprotectionpercentage, dinputfinalprice, " & fecha & ", '" & hora & "', '" & susername & "' FROM (SELECT * FROM baseprices WHERE ibaseid = " & baseid & " ORDER BY iupdatedate DESC, supdatetime DESC) pp GROUP BY iinputid"
                    queriesPrimeraVez(1) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "Timber SELECT " & iprojectid & ", bt.iinputid, bt.dinputtimberespesor, bt.dinputtimberancho, bt.dinputtimberlargo, bt.dinputtimberpreciopiecubico, " & fecha & ", '" & hora & "', '" & susername & "' FROM basetimber bt where ibaseid = " & baseid
                    executeTransactedSQLCommand(0, queriesPrimeraVez)

                Else

                    Dim queriesPrimeraVez(2) As String
                    queriesPrimeraVez(0) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "Prices SELECT " & iprojectid & ", iinputid, dinputpricewithoutIVA, dinputprotectionpercentage, dinputfinalprice, " & fecha & ", '" & hora & "', '" & susername & "' FROM (SELECT * FROM baseprices WHERE ibaseid = " & baseid & " ORDER BY iupdatedate DESC, supdatetime DESC) pp GROUP BY iinputid"
                    queriesPrimeraVez(1) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "Timber SELECT " & iprojectid & ", bt.iinputid, bt.dinputtimberespesor, bt.dinputtimberancho, bt.dinputtimberlargo, bt.dinputtimberpreciopiecubico, " & fecha & ", '" & hora & "', '" & susername & "' FROM basetimber bt WHERE ibaseid = " & baseid
                    executeTransactedSQLCommand(0, queriesPrimeraVez)

                End If

            End If

        End If

        Dim bi As New BuscaInsumos
        bi.susername = susername
        bi.bactive = bactive
        bi.bonline = bonline
        bi.suserfullname = suserfullname

        bi.suseremail = suseremail
        bi.susersession = susersession
        bi.susermachinename = susermachinename
        bi.suserip = suserip

        bi.IsEdit = IsEdit
        bi.IsBase = IsBase
        bi.IsModel = IsModel
        bi.IsHistoric = IsHistoric

        bi.iprojectid = iprojectid
        bi.icardid = icardid

        bi.querystring = dgvConceptosTarjeta.CurrentCell.EditedFormattedValue

        bi.IsEdit = False

        If Me.WindowState = FormWindowState.Maximized Then
            bi.WindowState = FormWindowState.Maximized
        End If

        Me.Visible = False
        bi.ShowDialog(Me)
        Me.Visible = True

        If bi.DialogResult = Windows.Forms.DialogResult.OK Then

            Dim isCompound As Boolean

            isCompound = getSQLQueryAsBoolean(0, "SELECT * FROM basecardcompoundinputs WHERE iinputid = " & bi.iinputid)

            If bi.wasCreated = True And isCompound = True Then

                dgvConceptosTarjeta.EndEdit()
                isConceptosTarjetaDGVReady = True
                Exit Sub

            End If

            Dim dsBusquedaInsumosRepetidos As DataSet

            If IsBase = True Then

                dsBusquedaInsumosRepetidos = getSQLQueryAsDataset(0, "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardInputs WHERE ibaseid = " & iprojectid & " AND icardid = " & icardid & " AND iinputid = " & bi.iinputid)

            Else

                If IsModel = True Then

                    dsBusquedaInsumosRepetidos = getSQLQueryAsDataset(0, "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "CardInputs WHERE imodelid = " & iprojectid & " AND icardid = " & icardid & " AND iinputid = " & bi.iinputid)

                Else

                    dsBusquedaInsumosRepetidos = getSQLQueryAsDataset(0, "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "CardInputs WHERE iprojectid = " & iprojectid & " AND icardid = " & icardid & " AND iinputid = " & bi.iinputid)

                End If

            End If

            If dsBusquedaInsumosRepetidos.Tables(0).Rows.Count > 0 Then

                MsgBox("Ya tienes ese Insumo insertado en esta Tarjeta. ¿Podrías buscarlo en la lista y cambiar la cantidad si así lo deseas?", MsgBoxStyle.OkOnly, "Insumo Repetido")
                dgvConceptosTarjeta.EndEdit()
                Exit Sub

            End If

            Dim isManoDeObra As Boolean = False
            Dim conteoMO As Integer = 0
            Dim busquedaMO As String = ""

            isManoDeObra = getSQLQueryAsBoolean(0, "SELECT * FROM inputs i JOIN inputtypes it ON i.iinputid = it.iinputid WHERE it.sinputtypedescription = 'MANO DE OBRA' AND i.iinputid = " & bi.iinputid)


            If IsEdit = True Then

                If IsBase = True Then

                    Dim queriesInsert(3) As String

                    queriesInsert(0) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardInputs SELECT " & iprojectid & ", " & icardid & ", " & bi.iinputid & ", sinputunit, 1, " & fecha & ", '" & hora & "', '" & susername & "' FROM inputs WHERE iinputid = " & bi.iinputid

                    If isManoDeObra = True Then

                        busquedaMO = "" & _
                        "SELECT COUNT(*) " & _
                        "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardInputs btfi " & _
                        "JOIN inputtypes it ON it.iinputid = btfi.iinputid " & _
                        "WHERE btfi.ibaseid = " & iprojectid & " AND btfi.icardid = " & icardid & " " & _
                        "AND it.sinputtypedescription = 'MANO DE OBRA' "

                        conteoMO = getSQLQueryAsDouble(0, busquedaMO)

                        If conteoMO = 0 Then
                            queriesInsert(1) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardInputs SELECT " & iprojectid & ", " & icardid & ", 0, '%', 1, " & fecha & ", '" & hora & "', '" & susername & "'"
                        End If

                    End If

                    If isCompound = True Then

                        If getSQLQueryAsBoolean(0, "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardInputs WHERE ibaseid = " & iprojectid & " AND iinputid = " & bi.iinputid) = True Then
                            queriesInsert(2) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardCompoundInputs SELECT * FROM (SELECT " & iprojectid & " AS projectid, " & icardid & " AS cardid, " & bi.iinputid & " AS inputid, bcci.icompoundinputid, i.sinputunit, bcci.dcompoundinputqty AS qty, " & fecha & ", '" & hora & "', '" & susername & "' FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardInputs bci JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardCompoundInputs bcci ON bci.ibaseid = bcci.ibaseid AND bci.icardid = bcci.icardid AND bci.iinputid = bcci.iinputid JOIN inputs i ON i.iinputid = bcci.icompoundinputid WHERE bci.ibaseid = " & iprojectid & " AND bci.iinputid = " & bi.iinputid & " ORDER BY bci.iupdatedate DESC, bci.supdatetime DESC) cid GROUP BY icompoundinputid "
                        Else

                            If getSQLQueryAsBoolean(0, "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardInputs WHERE ibaseid = " & baseid & " AND iinputid = " & bi.iinputid) = True Then
                                queriesInsert(2) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardCompoundInputs SELECT * FROM (SELECT " & iprojectid & " AS projectid, " & icardid & " AS cardid, " & bi.iinputid & " AS inputid, bcci.icompoundinputid, i.sinputunit, bcci.dcompoundinputqty AS qty, " & fecha & ", '" & hora & "', '" & susername & "' FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardInputs bci JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardCompoundInputs bcci ON bci.ibaseid = bcci.ibaseid AND bci.icardid = bcci.icardid AND bci.iinputid = bcci.iinputid JOIN inputs i ON i.iinputid = bcci.icompoundinputid WHERE bci.ibaseid = " & baseid & " AND bci.iinputid = " & bi.iinputid & " ORDER BY bci.iupdatedate DESC, bci.supdatetime DESC) cid GROUP BY icompoundinputid "
                            Else
                                queriesInsert(2) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardCompoundInputs SELECT * FROM (SELECT " & iprojectid & " AS projectid, " & icardid & " AS cardid, " & bi.iinputid & " AS inputid, bcci.icompoundinputid, i.sinputunit, 1 AS qty, " & fecha & ", '" & hora & "', '" & susername & "' FROM inputs i WHERE i.iinputid = " & bi.iinputid & " ORDER BY i.iupdatedate DESC, i.supdatetime DESC) cid GROUP BY icompoundinputid "
                            End If
                            firstTimeCompoundAddedAlert = True

                        End If

                    End If

                    executeTransactedSQLCommand(0, queriesInsert)

                Else

                    If IsModel = True Then

                        Dim queriesInsert(3) As String

                        queriesInsert(0) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "CardInputs SELECT " & iprojectid & ", " & icardid & ", " & bi.iinputid & ", sinputunit, 1, " & fecha & ", '" & hora & "', '" & susername & "' FROM inputs WHERE iinputid = " & bi.iinputid

                        If isManoDeObra = True Then

                            busquedaMO = "" & _
                            "SELECT COUNT(*) " & _
                            "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "CardInputs mtfi " & _
                            "JOIN inputtypes it ON it.iinputid = mtfi.iinputid " & _
                            "WHERE mtfi.imodelid = " & iprojectid & " AND mtfi.icardid = " & icardid & " " & _
                            "AND it.sinputtypedescription = 'MANO DE OBRA' "

                            conteoMO = getSQLQueryAsDouble(0, busquedaMO)

                            If conteoMO = 0 Then
                                queriesInsert(1) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "CardInputs SELECT " & iprojectid & ", " & icardid & ", 0, '%', 1, " & fecha & ", '" & hora & "', '" & susername & "'"
                            End If

                        End If

                        If isCompound = True Then

                            If getSQLQueryAsBoolean(0, "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "CardInputs WHERE imodelid = " & iprojectid & " AND iinputid = " & bi.iinputid) = True Then
                                queriesInsert(2) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "CardCompoundInputs SELECT * FROM (SELECT " & iprojectid & " AS projectid, " & icardid & " AS cardid, " & bi.iinputid & " AS inputid, bcci.icompoundinputid, i.sinputunit, bcci.dcompoundinputqty AS qty, " & fecha & ", '" & hora & "', '" & susername & "' FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "CardInputs bci JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "CardCompoundInputs bcci ON bci.imodelid = bcci.imodelid AND bci.icardid = bcci.icardid AND bci.iinputid = bcci.iinputid JOIN inputs i ON i.iinputid = bcci.icompoundinputid WHERE bci.imodelid = " & iprojectid & " AND bci.iinputid = " & bi.iinputid & " ORDER BY bci.iupdatedate DESC, bci.supdatetime DESC) cid GROUP BY icompoundinputid "
                            Else

                                If getSQLQueryAsBoolean(0, "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardInputs WHERE ibaseid = " & baseid & " AND iinputid = " & bi.iinputid) = True Then
                                    queriesInsert(2) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "CardCompoundInputs SELECT * FROM (SELECT " & iprojectid & " AS projectid, " & icardid & " AS cardid, " & bi.iinputid & " AS inputid, bcci.icompoundinputid, i.sinputunit, bcci.dcompoundinputqty AS qty, " & fecha & ", '" & hora & "', '" & susername & "' FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardInputs bci JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardCompoundInputs bcci ON bci.ibaseid = bcci.ibaseid AND bci.icardid = bcci.icardid AND bci.iinputid = bcci.iinputid JOIN inputs i ON i.iinputid = bcci.icompoundinputid WHERE bci.ibaseid = " & baseid & " AND bci.iinputid = " & bi.iinputid & " ORDER BY bci.iupdatedate DESC, bci.supdatetime DESC) cid GROUP BY icompoundinputid "
                                Else
                                    queriesInsert(2) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "CardCompoundInputs SELECT * FROM (SELECT " & iprojectid & " AS projectid, " & icardid & " AS cardid, " & bi.iinputid & " AS inputid, bcci.icompoundinputid, i.sinputunit, 1 AS qty, " & fecha & ", '" & hora & "', '" & susername & "' FROM inputs i WHERE i.iinputid = " & bi.iinputid & " ORDER BY i.iupdatedate DESC, i.supdatetime DESC) cid GROUP BY icompoundinputid "
                                End If
                                firstTimeCompoundAddedAlert = True

                            End If

                        End If

                        executeTransactedSQLCommand(0, queriesInsert)

                    Else

                        Dim queriesInsert(3) As String

                        queriesInsert(0) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "CardInputs SELECT " & iprojectid & ", " & icardid & ", " & bi.iinputid & ", sinputunit, 1, " & fecha & ", '" & hora & "', '" & susername & "' FROM inputs WHERE iinputid = " & bi.iinputid

                        If isManoDeObra = True Then

                            busquedaMO = "" & _
                            "SELECT COUNT(*) " & _
                            "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "CardInputs ptfi " & _
                            "JOIN inputtypes it ON it.iinputid = ptfi.iinputid " & _
                            "WHERE ptfi.iprojectid = " & iprojectid & " AND ptfi.icardid = " & icardid & " " & _
                            "AND it.sinputtypedescription = 'MANO DE OBRA' "

                            conteoMO = getSQLQueryAsDouble(0, busquedaMO)

                            If conteoMO = 0 Then
                                queriesInsert(1) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "CardInputs SELECT " & iprojectid & ", " & icardid & ", 0, '%', 1, " & fecha & ", '" & hora & "', '" & susername & "'"
                            End If

                        End If

                        If isCompound = True Then

                            If getSQLQueryAsBoolean(0, "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "CardInputs WHERE iprojectid = " & iprojectid & " AND iinputid = " & bi.iinputid) = True Then
                                queriesInsert(2) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "CardCompoundInputs SELECT * FROM (SELECT " & iprojectid & " AS projectid, " & icardid & " AS cardid, " & bi.iinputid & " AS inputid, bcci.icompoundinputid, i.sinputunit, bcci.dcompoundinputqty AS qty, " & fecha & ", '" & hora & "', '" & susername & "' FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "CardInputs bci JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "CardCompoundInputs bcci ON bci.iprojectid = bcci.iprojectid AND bci.icardid = bcci.icardid AND bci.iinputid = bcci.iinputid JOIN inputs i ON i.iinputid = bcci.icompoundinputid WHERE bci.iinputid = " & bi.iinputid & " ORDER BY bci.iupdatedate DESC, bci.supdatetime DESC) cid GROUP BY icompoundinputid "
                            Else

                                If getSQLQueryAsBoolean(0, "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardInputs WHERE ibaseid = " & baseid & " AND iinputid = " & bi.iinputid) = True Then
                                    queriesInsert(2) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "CardCompoundInputs SELECT * FROM (SELECT " & iprojectid & " AS projectid, " & icardid & " AS cardid, " & bi.iinputid & " AS inputid, bcci.icompoundinputid, i.sinputunit, bcci.dcompoundinputqty AS qty, " & fecha & ", '" & hora & "', '" & susername & "' FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardInputs bci JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardCompoundInputs bcci ON bci.ibaseid = bcci.ibaseid AND bci.icardid = bcci.icardid AND bci.iinputid = bcci.iinputid JOIN inputs i ON i.iinputid = bcci.icompoundinputid WHERE bci.ibaseid = " & baseid & " AND bci.iinputid = " & bi.iinputid & " ORDER BY bci.iupdatedate DESC, bci.supdatetime DESC) cid GROUP BY icompoundinputid "
                                Else
                                    queriesInsert(2) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "CardCompoundInputs SELECT * FROM (SELECT " & iprojectid & " AS projectid, " & icardid & " AS cardid, " & bi.iinputid & " AS inputid, bcci.icompoundinputid, i.sinputunit, 1 AS qty, " & fecha & ", '" & hora & "', '" & susername & "' FROM inputs i WHERE i.iinputid = " & bi.iinputid & " ORDER BY i.iupdatedate DESC, i.supdatetime DESC) cid GROUP BY icompoundinputid "
                                End If
                                firstTimeCompoundAddedAlert = True

                            End If

                        End If

                        executeTransactedSQLCommand(0, queriesInsert)

                    End If

                End If

            Else

                Dim queriesInsert(6) As String

                If IsBase = True Then

                    queriesInsert(0) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardInputs SELECT " & baseid & ", " & icardid & ", " & bi.iinputid & ", sinputunit, 1, " & fecha & ", '" & hora & "', '" & susername & "' FROM inputs WHERE iinputid = " & bi.iinputid

                    If isManoDeObra = True Then

                        busquedaMO = "" & _
                        "SELECT COUNT(*) " & _
                        "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardInputs btfi " & _
                        "JOIN inputtypes it ON it.iinputid = btfi.iinputid " & _
                        "WHERE btfi.ibaseid = " & baseid & " AND btfi.icardid = " & icardid & " " & _
                        "AND it.sinputtypedescription = 'MANO DE OBRA' "

                        conteoMO = getSQLQueryAsDouble(0, busquedaMO)

                        If conteoMO = 0 Then
                            queriesInsert(1) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardInputs SELECT " & baseid & ", " & icardid & ", 0, '%', 1, " & fecha & ", '" & hora & "', '" & susername & "'"
                        End If

                    End If

                    If isCompound = True Then

                        If getSQLQueryAsBoolean(0, "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardInputs WHERE ibaseid = " & iprojectid & " AND iinputid = " & bi.iinputid) = True Then
                            queriesInsert(2) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardCompoundInputs SELECT * FROM (SELECT " & iprojectid & " AS projectid, " & icardid & " AS cardid, " & bi.iinputid & " AS inputid, bcci.icompoundinputid, i.sinputunit, bcci.dcompoundinputqty AS qty, " & fecha & ", '" & hora & "', '" & susername & "' FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardInputs bci JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardCompoundInputs bcci ON bci.ibaseid = bcci.ibaseid AND bci.icardid = bcci.icardid AND bci.iinputid = bcci.iinputid JOIN inputs i ON i.iinputid = bcci.icompoundinputid WHERE bci.iinputid = " & bi.iinputid & " ORDER BY bci.iupdatedate DESC, bci.supdatetime DESC) cid GROUP BY icompoundinputid "
                        Else
                            If getSQLQueryAsBoolean(0, "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardInputs WHERE ibaseid = " & baseid & " AND iinputid = " & bi.iinputid) = True Then
                                queriesInsert(2) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardCompoundInputs SELECT * FROM (SELECT " & iprojectid & " AS projectid, " & icardid & " AS cardid, " & bi.iinputid & " AS inputid, bcci.icompoundinputid, i.sinputunit, bcci.dcompoundinputqty AS qty, " & fecha & ", '" & hora & "', '" & susername & "' FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardInputs bci JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardCompoundInputs bcci ON bci.ibaseid = bcci.ibaseid AND bci.icardid = bcci.icardid AND bci.iinputid = bcci.iinputid JOIN inputs i ON i.iinputid = bcci.icompoundinputid WHERE bci.ibaseid = " & baseid & " AND bci.iinputid = " & bi.iinputid & " ORDER BY bci.iupdatedate DESC, bci.supdatetime DESC) cid GROUP BY icompoundinputid "
                            Else
                                queriesInsert(2) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardCompoundInputs SELECT * FROM (SELECT " & iprojectid & " AS projectid, " & icardid & " AS cardid, " & bi.iinputid & " AS inputid, bcci.icompoundinputid, i.sinputunit, 1 AS qty, " & fecha & ", '" & hora & "', '" & susername & "' FROM inputs i WHERE i.iinputid = " & bi.iinputid & " ORDER BY i.iupdatedate DESC, i.supdatetime DESC) cid GROUP BY icompoundinputid "
                            End If
                            firstTimeCompoundAddedAlert = True
                        End If

                    End If

                    executeTransactedSQLCommand(0, queriesInsert)

                ElseIf IsModel = True Then

                    queriesInsert(0) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardInputs SELECT " & baseid & ", " & icardid & ", " & bi.iinputid & ", sinputunit, 1, " & fecha & ", '" & hora & "', '" & susername & "' FROM inputs WHERE iinputid = " & bi.iinputid

                    If isManoDeObra = True Then

                        busquedaMO = "" & _
                        "SELECT COUNT(*) " & _
                        "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardInputs btfi " & _
                        "JOIN inputtypes it ON it.iinputid = btfi.iinputid " & _
                        "WHERE btfi.ibaseid = " & baseid & " AND btfi.icardid = " & icardid & " " & _
                        "AND it.sinputtypedescription = 'MANO DE OBRA' "

                        conteoMO = getSQLQueryAsDouble(0, busquedaMO)

                        If conteoMO = 0 Then
                            queriesInsert(1) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardInputs SELECT " & baseid & ", " & icardid & ", 0, '%', 1, " & fecha & ", '" & hora & "', '" & susername & "'"
                        End If

                    End If

                    If isCompound = True Then

                        If getSQLQueryAsBoolean(0, "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardInputs WHERE ibaseid = " & iprojectid & " AND iinputid = " & bi.iinputid) = True Then
                            queriesInsert(2) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardCompoundInputs SELECT * FROM (SELECT " & iprojectid & " AS projectid, " & icardid & " AS cardid, " & bi.iinputid & " AS inputid, bcci.icompoundinputid, i.sinputunit, bcci.dcompoundinputqty AS qty, " & fecha & ", '" & hora & "', '" & susername & "' FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardInputs bci JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardCompoundInputs bcci ON bci.ibaseid = bcci.ibaseid AND bci.icardid = bcci.icardid AND bci.iinputid = bcci.iinputid JOIN inputs i ON i.iinputid = bcci.icompoundinputid WHERE bci.iinputid = " & bi.iinputid & " ORDER BY bci.iupdatedate DESC, bci.supdatetime DESC) cid GROUP BY icompoundinputid "
                        Else
                            If getSQLQueryAsBoolean(0, "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardInputs WHERE ibaseid = " & baseid & " AND iinputid = " & bi.iinputid) = True Then
                                queriesInsert(2) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardCompoundInputs SELECT * FROM (SELECT " & iprojectid & " AS projectid, " & icardid & " AS cardid, " & bi.iinputid & " AS inputid, bcci.icompoundinputid, i.sinputunit, bcci.dcompoundinputqty AS qty, " & fecha & ", '" & hora & "', '" & susername & "' FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardInputs bci JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardCompoundInputs bcci ON bci.ibaseid = bcci.ibaseid AND bci.icardid = bcci.icardid AND bci.iinputid = bcci.iinputid JOIN inputs i ON i.iinputid = bcci.icompoundinputid WHERE bci.ibaseid = " & baseid & " AND bci.iinputid = " & bi.iinputid & " ORDER BY bci.iupdatedate DESC, bci.supdatetime DESC) cid GROUP BY icompoundinputid "
                            Else
                                queriesInsert(2) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardCompoundInputs SELECT * FROM (SELECT " & iprojectid & " AS projectid, " & icardid & " AS cardid, " & bi.iinputid & " AS inputid, bcci.icompoundinputid, i.sinputunit, 1 AS qty, " & fecha & ", '" & hora & "', '" & susername & "' FROM inputs i WHERE i.iinputid = " & bi.iinputid & " ORDER BY i.iupdatedate DESC, i.supdatetime DESC) cid GROUP BY icompoundinputid "
                            End If
                            firstTimeCompoundAddedAlert = True
                        End If

                    End If

                    queriesInsert(3) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "CardInputs SELECT " & iprojectid & ", " & icardid & ", " & bi.iinputid & ", sinputunit, 1, " & fecha & ", '" & hora & "', '" & susername & "' FROM inputs WHERE iinputid = " & bi.iinputid

                    If isManoDeObra = True Then

                        busquedaMO = "" & _
                        "SELECT COUNT(*) " & _
                        "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "CardInputs mtfi " & _
                        "JOIN inputtypes it ON it.iinputid = mtfi.iinputid " & _
                        "WHERE mtfi.imodelid = " & iprojectid & " AND mtfi.icardid = " & icardid & " " & _
                        "AND it.sinputtypedescription = 'MANO DE OBRA' "

                        conteoMO = getSQLQueryAsDouble(0, busquedaMO)

                        If conteoMO = 0 Then
                            queriesInsert(4) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "CardInputs SELECT " & iprojectid & ", " & icardid & ", 0, '%', 1, " & fecha & ", '" & hora & "', '" & susername & "'"
                        End If

                    End If

                    If isCompound = True Then

                        If getSQLQueryAsBoolean(0, "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "CardInputs WHERE imodelid = " & iprojectid & " AND iinputid = " & bi.iinputid) = True Then
                            queriesInsert(5) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "CardCompoundInputs SELECT * FROM (SELECT " & iprojectid & " AS projectid, " & icardid & " AS cardid, " & bi.iinputid & " AS inputid, bcci.icompoundinputid, i.sinputunit, bcci.dcompoundinputqty AS qty, " & fecha & ", '" & hora & "', '" & susername & "' FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "CardInputs bci JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "CardCompoundInputs bcci ON bci.imodelid = bcci.imodelid AND bci.icardid = bcci.icardid AND bci.iinputid = bcci.iinputid JOIN inputs i ON i.iinputid = bcci.icompoundinputid WHERE bci.iinputid = " & bi.iinputid & " ORDER BY bci.iupdatedate DESC, bci.supdatetime DESC) cid GROUP BY icompoundinputid "
                        Else

                            If getSQLQueryAsBoolean(0, "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardInputs WHERE ibaseid = " & baseid & " AND iinputid = " & bi.iinputid) = True Then
                                queriesInsert(5) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "CardCompoundInputs SELECT * FROM (SELECT " & iprojectid & " AS projectid, " & icardid & " AS cardid, " & bi.iinputid & " AS inputid, bcci.icompoundinputid, i.sinputunit, bcci.dcompoundinputqty AS qty, " & fecha & ", '" & hora & "', '" & susername & "' FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardInputs bci JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardCompoundInputs bcci ON bci.ibaseid = bcci.ibaseid AND bci.icardid = bcci.icardid AND bci.iinputid = bcci.iinputid JOIN inputs i ON i.iinputid = bcci.icompoundinputid WHERE bci.ibaseid = " & baseid & " AND bci.iinputid = " & bi.iinputid & " ORDER BY bci.iupdatedate DESC, bci.supdatetime DESC) cid GROUP BY icompoundinputid "
                            Else
                                queriesInsert(5) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "CardCompoundInputs SELECT * FROM (SELECT " & iprojectid & " AS projectid, " & icardid & " AS cardid, " & bi.iinputid & " AS inputid, bcci.icompoundinputid, i.sinputunit, 1 AS qty, " & fecha & ", '" & hora & "', '" & susername & "' FROM inputs i WHERE i.iinputid = " & bi.iinputid & " ORDER BY i.iupdatedate DESC, i.supdatetime DESC) cid GROUP BY icompoundinputid "
                            End If
                            firstTimeCompoundAddedAlert = True

                        End If

                    End If

                    executeTransactedSQLCommand(0, queriesInsert)

                Else

                    queriesInsert(0) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardInputs SELECT " & baseid & ", " & icardid & ", " & bi.iinputid & ", sinputunit, 1, " & fecha & ", '" & hora & "', '" & susername & "' FROM inputs WHERE iinputid = " & bi.iinputid

                    If isManoDeObra = True Then

                        busquedaMO = "" & _
                        "SELECT COUNT(*) " & _
                        "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardInputs btfi " & _
                        "JOIN inputtypes it ON it.iinputid = btfi.iinputid " & _
                        "WHERE btfi.ibaseid = " & baseid & " AND btfi.icardid = " & icardid & " " & _
                        "AND it.sinputtypedescription = 'MANO DE OBRA' "

                        conteoMO = getSQLQueryAsDouble(0, busquedaMO)

                        If conteoMO = 0 Then
                            queriesInsert(1) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardInputs SELECT " & baseid & ", " & icardid & ", 0, '%', 1, " & fecha & ", '" & hora & "', '" & susername & "'"
                        End If

                    End If

                    If isCompound = True Then

                        If getSQLQueryAsBoolean(0, "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardInputs WHERE ibaseid = " & iprojectid & " AND iinputid = " & bi.iinputid) = True Then
                            queriesInsert(2) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardCompoundInputs SELECT * FROM (SELECT " & iprojectid & " AS projectid, " & icardid & " AS cardid, " & bi.iinputid & " AS inputid, bcci.icompoundinputid, i.sinputunit, bcci.dcompoundinputqty AS qty, " & fecha & ", '" & hora & "', '" & susername & "' FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardInputs bci JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardCompoundInputs bcci ON bci.ibaseid = bcci.ibaseid AND bci.icardid = bcci.icardid AND bci.iinputid = bcci.iinputid JOIN inputs i ON i.iinputid = bcci.icompoundinputid WHERE bci.iinputid = " & bi.iinputid & " ORDER BY bci.iupdatedate DESC, bci.supdatetime DESC) cid GROUP BY icompoundinputid "
                        Else
                            If getSQLQueryAsBoolean(0, "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardInputs WHERE ibaseid = " & baseid & " AND iinputid = " & bi.iinputid) = True Then
                                queriesInsert(2) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardCompoundInputs SELECT * FROM (SELECT " & iprojectid & " AS projectid, " & icardid & " AS cardid, " & bi.iinputid & " AS inputid, bcci.icompoundinputid, i.sinputunit, bcci.dcompoundinputqty AS qty, " & fecha & ", '" & hora & "', '" & susername & "' FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardInputs bci JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardCompoundInputs bcci ON bci.ibaseid = bcci.ibaseid AND bci.icardid = bcci.icardid AND bci.iinputid = bcci.iinputid JOIN inputs i ON i.iinputid = bcci.icompoundinputid WHERE bci.ibaseid = " & baseid & " AND bci.iinputid = " & bi.iinputid & " ORDER BY bci.iupdatedate DESC, bci.supdatetime DESC) cid GROUP BY icompoundinputid "
                            Else
                                queriesInsert(2) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardCompoundInputs SELECT * FROM (SELECT " & iprojectid & " AS projectid, " & icardid & " AS cardid, " & bi.iinputid & " AS inputid, bcci.icompoundinputid, i.sinputunit, 1 AS qty, " & fecha & ", '" & hora & "', '" & susername & "' FROM inputs i WHERE i.iinputid = " & bi.iinputid & " ORDER BY i.iupdatedate DESC, i.supdatetime DESC) cid GROUP BY icompoundinputid "
                            End If
                            firstTimeCompoundAddedAlert = True
                        End If

                    End If

                    queriesInsert(3) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "CardInputs SELECT " & iprojectid & ", " & icardid & ", " & bi.iinputid & ", sinputunit, 1, " & fecha & ", '" & hora & "', '" & susername & "' FROM inputs WHERE iinputid = " & bi.iinputid

                    If isManoDeObra = True Then

                        busquedaMO = "" & _
                        "SELECT COUNT(*) " & _
                        "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "CardInputs ptfi " & _
                        "JOIN inputtypes it ON it.iinputid = ptfi.iinputid " & _
                        "WHERE ptfi.iprojectid = " & iprojectid & " AND ptfi.icardid = " & icardid & " " & _
                        "AND it.sinputtypedescription = 'MANO DE OBRA' "

                        conteoMO = getSQLQueryAsDouble(0, busquedaMO)

                        If conteoMO = 0 Then
                            queriesInsert(4) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "CardInputs SELECT " & iprojectid & ", " & icardid & ", 0, '%', 1, " & fecha & ", '" & hora & "', '" & susername & "'"
                        End If

                    End If

                    If isCompound = True Then

                        If getSQLQueryAsBoolean(0, "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "CardInputs WHERE iprojectid = " & iprojectid & " AND iinputid = " & bi.iinputid) = True Then
                            queriesInsert(5) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "CardCompoundInputs SELECT * FROM (SELECT " & iprojectid & " AS projectid, " & icardid & " AS cardid, " & bi.iinputid & " AS inputid, bcci.icompoundinputid, i.sinputunit, bcci.dcompoundinputqty AS qty, " & fecha & ", '" & hora & "', '" & susername & "' FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "CardInputs bci JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "CardCompoundInputs bcci ON bci.iprojectid = bcci.iprojectid AND bci.icardid = bcci.icardid AND bci.iinputid = bcci.iinputid JOIN inputs i ON i.iinputid = bcci.icompoundinputid WHERE bci.iinputid = " & bi.iinputid & " ORDER BY bci.iupdatedate DESC, bci.supdatetime DESC) cid GROUP BY icompoundinputid "
                        Else
                            If getSQLQueryAsBoolean(0, "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardInputs WHERE ibaseid = " & baseid & " AND iinputid = " & bi.iinputid) = True Then
                                queriesInsert(5) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "CardCompoundInputs SELECT * FROM (SELECT " & iprojectid & " AS projectid, " & icardid & " AS cardid, " & bi.iinputid & " AS inputid, bcci.icompoundinputid, i.sinputunit, bcci.dcompoundinputqty AS qty, " & fecha & ", '" & hora & "', '" & susername & "' FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardInputs bci JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardCompoundInputs bcci ON bci.ibaseid = bcci.ibaseid AND bci.icardid = bcci.icardid AND bci.iinputid = bcci.iinputid JOIN inputs i ON i.iinputid = bcci.icompoundinputid WHERE bci.ibaseid = " & baseid & " AND bci.iinputid = " & bi.iinputid & " ORDER BY bci.iupdatedate DESC, bci.supdatetime DESC) cid GROUP BY icompoundinputid "
                            Else
                                queriesInsert(5) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "CardCompoundInputs SELECT * FROM (SELECT " & iprojectid & " AS projectid, " & icardid & " AS cardid, " & bi.iinputid & " AS inputid, bcci.icompoundinputid, i.sinputunit, 1 AS qty, " & fecha & ", '" & hora & "', '" & susername & "' FROM inputs i WHERE i.iinputid = " & bi.iinputid & " ORDER BY i.iupdatedate DESC, i.supdatetime DESC) cid GROUP BY icompoundinputid "
                            End If
                            firstTimeCompoundAddedAlert = True
                        End If

                    End If

                    executeTransactedSQLCommand(0, queriesInsert)

                End If


            End If


            dgvConceptosTarjeta.EndEdit()

            isConceptosTarjetaDGVReady = True

        Else

            dgvConceptosTarjeta.EndEdit()

            isConceptosTarjetaDGVReady = True

        End If

        If firstTimeCompoundAddedAlert = True Then
            MsgBox("Como es la primera vez que insertas este Insumo Compuesto a este proyecto, te sugiero que revises sus rendimientos y cantidades", MsgBoxStyle.OkOnly Or MsgBoxStyle.Information, "Establecer rendimientos correctos")
        End If

        Cursor.Current = System.Windows.Forms.Cursors.Default

    End Sub


    Private Sub dgvConceptosTarjeta_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgvConceptosTarjeta.KeyUp

        If e.KeyCode = Keys.Delete Then

            If deleteInputPermission = False Then
                Exit Sub
            End If

            Try

                If dgvConceptosTarjeta.CurrentRow.Index = -1 Then
                    Exit Sub
                End If

            Catch ex As Exception

                iselectedinputid = 0
                sselectedinputdescription = ""
                sselectedunit = ""
                dselectedinputqty = 1.0
                Exit Sub

            End Try

            If MsgBox("¿Estás seguro de que quieres eliminar el Insumo " & sselectedinputdescription & " de la Tarjeta?", MsgBoxStyle.YesNo, "Confirmación de Eliminación de Insumo de la Tarjeta") = MsgBoxResult.Yes Then

                Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

                Dim tmpselectedinputid As Integer = 0
                Try
                    tmpselectedinputid = CInt(dgvConceptosTarjeta.CurrentRow.Cells(0).Value)
                Catch ex As Exception

                End Try

                If tmpselectedinputid = 0 Then
                    MsgBox("No es posible dejar una Tarjeta sin Equipo y Herramientas", MsgBoxStyle.OkOnly, "Equipo y Herramientas Requeridos")
                    Exit Sub
                End If

                Dim conteoMO As Integer = 0
                Dim busquedaMO As String = ""
                Dim isManoDeObra As String = ""
                Dim queriesDelete(2) As String

                If IsBase = True Then

                    isManoDeObra = "SELECT COUNT(*) FROM inputtypes WHERE iinputid = " & tmpselectedinputid & " AND it.sinputtypedescription = 'MANO DE OBRA' "

                    If getSQLQueryAsInteger(0, isManoDeObra) > 0 Then

                        busquedaMO = "" & _
                        "SELECT COUNT(*) " & _
                        "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardInputs btfi " & _
                        "JOIN inputtypes it ON it.iinputid = btfi.iinputid " & _
                        "WHERE btfi.ibaseid = " & iprojectid & " AND btfi.icardid = " & icardid & " " & _
                        "AND it.sinputtypedescription = 'MANO DE OBRA' "

                        conteoMO = getSQLQueryAsDouble(0, busquedaMO)

                        If conteoMO = 0 Then
                            'Continue
                        ElseIf conteoMO > 1 Then
                            queriesDelete(0) = "DELETE FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardInputs WHERE ibaseid = " & iprojectid & " AND icardid = " & icardid & " AND iinputid = " & tmpselectedinputid
                        Else
                            MsgBox("No es posible dejar una Tarjeta sin Mano de Obra", MsgBoxStyle.OkOnly, "Mano de Obra Requerida")
                            Exit Sub
                        End If

                    End If

                    queriesDelete(0) = "DELETE FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardInputs WHERE ibaseid = " & iprojectid & " AND icardid = " & icardid & " AND iinputid = " & tmpselectedinputid

                    Dim conteoCompound As Integer = 0

                    conteoCompound = getSQLQueryAsInteger(0, "SELECT COUNT(*) FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardCompoundInputs WHERE iinputid = " & iselectedinputid)

                    If conteoCompound > 0 Then
                        queriesDelete(1) = "DELETE FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardCompoundInputs WHERE ibaseid = " & iprojectid & " AND icardid = " & icardid & " AND iinputid = " & tmpselectedinputid
                    End If

                    executeTransactedSQLCommand(0, queriesDelete)

                Else

                    If IsModel = True Then

                        isManoDeObra = "SELECT COUNT(*) FROM inputtypes WHERE iinputid = " & tmpselectedinputid & " AND it.sinputtypedescription = 'MANO DE OBRA' "

                        If getSQLQueryAsInteger(0, isManoDeObra) > 0 Then

                            busquedaMO = "" & _
                            "SELECT COUNT(*) " & _
                            "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "CardInputs mtfi " & _
                            "JOIN inputtypes it ON it.iinputid = mtfi.iinputid " & _
                            "WHERE mtfi.imodelid = " & iprojectid & " AND mtfi.icardid = " & icardid & " " & _
                            "AND it.sinputtypedescription = 'MANO DE OBRA' "

                            conteoMO = getSQLQueryAsDouble(0, busquedaMO)

                            If conteoMO = 0 Then
                                'Continue
                            ElseIf conteoMO > 1 Then
                                queriesDelete(0) = "DELETE FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "CardInputs WHERE imodelid = " & iprojectid & " AND icardid = " & icardid & " AND iinputid = " & tmpselectedinputid
                            Else
                                MsgBox("No es posible dejar una Tarjeta sin Mano de Obra", MsgBoxStyle.OkOnly, "Mano de Obra Requerida")
                                Exit Sub
                            End If

                        End If

                        queriesDelete(0) = "DELETE FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "CardInputs WHERE imodelid = " & iprojectid & " AND icardid = " & icardid & " AND iinputid = " & tmpselectedinputid

                        Dim conteoCompound As Integer = 0

                        conteoCompound = getSQLQueryAsInteger(0, "SELECT COUNT(*) FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardCompoundInputs WHERE iinputid = " & iselectedinputid)

                        If conteoCompound > 0 Then
                            queriesDelete(1) = "DELETE FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "CardCompoundInputs WHERE imodelid = " & iprojectid & " AND icardid = " & icardid & " AND iinputid = " & tmpselectedinputid
                        End If

                        executeTransactedSQLCommand(0, queriesDelete)

                    Else

                        isManoDeObra = "SELECT COUNT(*) FROM inputtypes WHERE iinputid = " & tmpselectedinputid & " AND it.sinputtypedescription = 'MANO DE OBRA' "

                        If getSQLQueryAsInteger(0, isManoDeObra) > 0 Then

                            busquedaMO = "" & _
                            "SELECT COUNT(*) " & _
                            "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "CardInputs ptfi " & _
                            "JOIN inputtypes it ON it.iinputid = ptfi.iinputid " & _
                            "WHERE ptfi.iprojectid = " & iprojectid & " AND ptfi.icardid = " & icardid & " " & _
                            "AND it.sinputtypedescription = 'MANO DE OBRA' "

                            conteoMO = getSQLQueryAsDouble(0, busquedaMO)

                            If conteoMO = 0 Then
                                'Continue
                            ElseIf conteoMO > 1 Then
                                queriesDelete(0) = "DELETE FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "CardInputs WHERE iprojectid = " & iprojectid & " AND icardid = " & icardid & " AND iinputid = " & tmpselectedinputid
                            Else
                                MsgBox("No es posible dejar una Tarjeta sin Mano de Obra", MsgBoxStyle.OkOnly, "Mano de Obra Requerida")
                                Exit Sub
                            End If

                        End If

                        queriesDelete(0) = "DELETE FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "CardInputs WHERE iprojectid = " & iprojectid & " AND icardid = " & icardid & " AND iinputid = " & tmpselectedinputid

                        Dim conteoCompound As Integer = 0

                        conteoCompound = getSQLQueryAsInteger(0, "SELECT COUNT(*) FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardCompoundInputs WHERE iinputid = " & iselectedinputid)

                        If conteoCompound > 0 Then
                            queriesDelete(1) = "DELETE FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "CardCompoundInputs WHERE iprojectid = " & iprojectid & " AND icardid = " & icardid & " AND iinputid = " & tmpselectedinputid
                        End If

                        executeTransactedSQLCommand(0, queriesDelete)

                    End If

                End If

                updateEverything()

                isConceptosTarjetaDGVReady = True

                Cursor.Current = System.Windows.Forms.Cursors.Default

            End If

        End If

    End Sub


    Private Sub dgvConceptosTarjeta_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgvConceptosTarjeta.Click

        If IsEdit = False And icardid <> 0 And validaDatosInicialesTarjeta(True, False) = True Then

            dgvConceptosTarjeta.Enabled = True
            btnNuevoInsumo.Enabled = True
            btnInsertarInsumo.Enabled = True
            btnEliminarInsumo.Enabled = True

        ElseIf IsEdit = False And icardid = 0 And validaDatosInicialesTarjeta(True, False) = True Then

            Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

            If isFormReadyForAction = False Then
                Exit Sub
            End If

            If validaDatosInicialesTarjeta(True, False) = False Then
                Exit Sub
            Else
                dgvConceptosTarjeta.Enabled = True
                btnNuevoInsumo.Enabled = True
                btnInsertarInsumo.Enabled = True
                btnEliminarInsumo.Enabled = True
            End If


            If saveCardPermission = False Then
                Exit Sub
            End If


            Dim valorIndirectos As Double = 0.0
            Try
                valorIndirectos = CDbl(txtPorcentajeIndirectos.Text)
            Catch ex As Exception

            End Try

            Dim valorUtilidades As Double = 0.0
            Try
                valorUtilidades = CDbl(txtPorcentajeUtilidades.Text)
            Catch ex As Exception

            End Try


            Dim baseid As Integer = 0
            baseid = getSQLQueryAsInteger(0, "SELECT ibaseid FROM base ORDER BY iupdatedate DESC, supdatetime DESC LIMIT 1")

            If baseid = 0 Then
                baseid = 99999
            End If

            Dim fecha As Integer = 0
            Dim hora As String = ""

            fecha = getMySQLDate()
            hora = getAppTime()

            If IsEdit = True Then

                If IsBase = True Then

                    executeSQLCommand(0, "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Cards SET iupdatedate = " & fecha & ", supdatetime = '" & hora & "', supdateusername = '" & susername & "', scardlegacycategoryid = '" & cmbCategoriaDeLaTarjeta.SelectedValue.ToString & "', scardlegacyid = '" & txtCodigoDeLaTarjeta.Text & "', scarddescription = '" & txtNombreDeLaTarjeta.Text & "', scardunit = '" & txtUnidadDeMedida.Text & "', dcardindirectcostspercentage = " & (valorIndirectos / 100) & ", dcardgainpercentage = " & (valorUtilidades / 100) & " WHERE ibaseid = " & iprojectid & " AND icardid = " & icardid)

                Else

                    If IsModel = True Then

                        executeSQLCommand(0, "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "Cards SET iupdatedate = " & fecha & ", supdatetime = '" & hora & "', supdateusername = '" & susername & "', scardlegacycategoryid = '" & cmbCategoriaDeLaTarjeta.SelectedValue.ToString & "', scardlegacyid = '" & txtCodigoDeLaTarjeta.Text & "', scarddescription = '" & txtNombreDeLaTarjeta.Text & "', scardunit = '" & txtUnidadDeMedida.Text & "', dcardindirectcostspercentage = " & (valorIndirectos / 100) & ", dcardgainpercentage = " & (valorUtilidades / 100) & " WHERE imodelid = " & iprojectid & " AND icardid = " & icardid)

                    Else

                        executeSQLCommand(0, "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "Cards SET iupdatedate = " & fecha & ", supdatetime = '" & hora & "', supdateusername = '" & susername & "', scardlegacycategoryid = '" & cmbCategoriaDeLaTarjeta.SelectedValue.ToString & "', scardlegacyid = '" & txtCodigoDeLaTarjeta.Text & "', scarddescription = '" & txtNombreDeLaTarjeta.Text & "', scardunit = '" & txtUnidadDeMedida.Text & "', dcardindirectcostspercentage = " & (valorIndirectos / 100) & ", dcardgainpercentage = " & (valorUtilidades / 100) & " WHERE iprojectid = " & iprojectid & " AND icardid = " & icardid)

                    End If

                End If

            Else

                Dim checkIfItsOnlyTextUpdate As Boolean = False

                checkIfItsOnlyTextUpdate = getSQLQueryAsBoolean(0, "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Cards WHERE ibaseid = " & baseid & " AND icardid = " & icardid)

                If checkIfItsOnlyTextUpdate = True Then

                    Dim queriesUpdate(2) As String

                    If IsBase = True Then

                        queriesUpdate(0) = "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Cards SET iupdatedate = " & fecha & ", supdatetime = '" & hora & "', supdateusername = '" & susername & "', scardlegacycategoryid = '" & cmbCategoriaDeLaTarjeta.SelectedValue.ToString & "', scardlegacyid = '" & txtCodigoDeLaTarjeta.Text & "', scarddescription = '" & txtNombreDeLaTarjeta.Text & "', scardunit = '" & txtUnidadDeMedida.Text & "', dcardindirectcostspercentage = " & (valorIndirectos / 100) & ", dcardgainpercentage = " & (valorUtilidades / 100) & " WHERE ibaseid = " & baseid & " AND icardid = " & icardid

                    Else

                        If IsModel = True Then

                            queriesUpdate(0) = "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Cards SET iupdatedate = " & fecha & ", supdatetime = '" & hora & "', supdateusername = '" & susername & "', scardlegacycategoryid = '" & cmbCategoriaDeLaTarjeta.SelectedValue.ToString & "', scardlegacyid = '" & txtCodigoDeLaTarjeta.Text & "', scarddescription = '" & txtNombreDeLaTarjeta.Text & "', scardunit = '" & txtUnidadDeMedida.Text & "', dcardindirectcostspercentage = " & (valorIndirectos / 100) & ", dcardgainpercentage = " & (valorUtilidades / 100) & " WHERE ibaseid = " & baseid & " AND icardid = " & icardid
                            queriesUpdate(1) = "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "Cards SET iupdatedate = " & fecha & ", supdatetime = '" & hora & "', supdateusername = '" & susername & "', scardlegacycategoryid = '" & cmbCategoriaDeLaTarjeta.SelectedValue.ToString & "', scardlegacyid = '" & txtCodigoDeLaTarjeta.Text & "', scarddescription = '" & txtNombreDeLaTarjeta.Text & "', scardunit = '" & txtUnidadDeMedida.Text & "', dcardindirectcostspercentage = " & (valorIndirectos / 100) & ", dcardgainpercentage = " & (valorUtilidades / 100) & " WHERE imodelid = " & iprojectid & " AND icardid = " & icardid

                        Else

                            queriesUpdate(0) = "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Cards SET iupdatedate = " & fecha & ", supdatetime = '" & hora & "', supdateusername = '" & susername & "', scardlegacycategoryid = '" & cmbCategoriaDeLaTarjeta.SelectedValue.ToString & "', scardlegacyid = '" & txtCodigoDeLaTarjeta.Text & "', scarddescription = '" & txtNombreDeLaTarjeta.Text & "', scardunit = '" & txtUnidadDeMedida.Text & "', dcardindirectcostspercentage = " & (valorIndirectos / 100) & ", dcardgainpercentage = " & (valorUtilidades / 100) & " WHERE ibaseid = " & baseid & " AND icardid = " & icardid
                            queriesUpdate(1) = "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "Cards SET iupdatedate = " & fecha & ", supdatetime = '" & hora & "', supdateusername = '" & susername & "', scardlegacycategoryid = '" & cmbCategoriaDeLaTarjeta.SelectedValue.ToString & "', scardlegacyid = '" & txtCodigoDeLaTarjeta.Text & "', scarddescription = '" & txtNombreDeLaTarjeta.Text & "', scardunit = '" & txtUnidadDeMedida.Text & "', dcardindirectcostspercentage = " & (valorIndirectos / 100) & ", dcardgainpercentage = " & (valorUtilidades / 100) & " WHERE iprojectid = " & iprojectid & " AND icardid = " & icardid

                        End If

                    End If

                    executeTransactedSQLCommand(0, queriesUpdate)

                Else

                    icardid = getSQLQueryAsInteger(0, "SELECT IF(MAX(icardid) + 1 IS NULL, 1, MAX(icardid) + 1) AS icardid FROM basecards WHERE ibaseid = " & baseid)
                    scardlegacyid = txtCodigoDeLaTarjeta.Text
                    scarddescription = txtNombreDeLaTarjeta.Text

                    Dim queriesInsert(2) As String

                    If IsBase = True Then

                        queriesInsert(0) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Cards VALUES (" & baseid & ", " & icardid & ", '" & cmbCategoriaDeLaTarjeta.SelectedValue & "', '" & txtCodigoDeLaTarjeta.Text & "', '" & txtNombreDeLaTarjeta.Text & "', '" & txtUnidadDeMedida.Text & "', 1, " & (valorIndirectos / 100) & ", " & (valorUtilidades / 100) & ", " & fecha & ", '" & hora & "', '" & susername & "')"

                    Else

                        If IsModel = True Then

                            queriesInsert(0) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Cards VALUES (" & baseid & ", " & icardid & ", '" & cmbCategoriaDeLaTarjeta.SelectedValue & "', '" & txtCodigoDeLaTarjeta.Text & "', '" & txtNombreDeLaTarjeta.Text & "', '" & txtUnidadDeMedida.Text & "', 1, " & (valorIndirectos / 100) & ", " & (valorUtilidades / 100) & ", " & fecha & ", '" & hora & "', '" & susername & "')"
                            queriesInsert(1) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "Cards VALUES (" & iprojectid & ", " & icardid & ", '" & cmbCategoriaDeLaTarjeta.SelectedValue & "', '" & txtCodigoDeLaTarjeta.Text & "', '" & txtNombreDeLaTarjeta.Text & "', '" & txtUnidadDeMedida.Text & "', 1, " & (valorIndirectos / 100) & ", " & (valorUtilidades / 100) & ", " & fecha & ", '" & hora & "', '" & susername & "')"

                        Else

                            queriesInsert(0) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Cards VALUES (" & baseid & ", " & icardid & ", '" & cmbCategoriaDeLaTarjeta.SelectedValue & "', '" & txtCodigoDeLaTarjeta.Text & "', '" & txtNombreDeLaTarjeta.Text & "', '" & txtUnidadDeMedida.Text & "', 1, " & (valorIndirectos / 100) & ", " & (valorUtilidades / 100) & ", " & fecha & ", '" & hora & "', '" & susername & "')"
                            queriesInsert(1) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "Cards VALUES (" & iprojectid & ", " & icardid & ", '" & cmbCategoriaDeLaTarjeta.SelectedValue & "', '" & txtCodigoDeLaTarjeta.Text & "', '" & txtNombreDeLaTarjeta.Text & "', '" & txtUnidadDeMedida.Text & "', 1, " & (valorIndirectos / 100) & ", " & (valorUtilidades / 100) & ", " & fecha & ", '" & hora & "', '" & susername & "')"

                        End If

                    End If

                    executeTransactedSQLCommand(0, queriesInsert)

                End If

            End If

            Cursor.Current = System.Windows.Forms.Cursors.Default

            dgvConceptosTarjeta.Enabled = True
            btnNuevoInsumo.Enabled = True
            btnInsertarInsumo.Enabled = True
            btnEliminarInsumo.Enabled = True

        ElseIf validaDatosInicialesTarjeta(True, False) = True And IsEdit = True Then

            dgvConceptosTarjeta.Enabled = True
            btnNuevoInsumo.Enabled = True
            btnInsertarInsumo.Enabled = True
            btnEliminarInsumo.Enabled = True

        End If

    End Sub


    Private Sub txtPorcentajeIndirectos_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtPorcentajeIndirectos.KeyUp

        Dim strcaracteresprohibidos As String = "abcdefghijklmnñopqrstuvwxyz|°!#$%&/()=?¡*¨[]_:;,-{}+´¿'¬^`~@\<> "
        Dim arrayCaractProhib As Char() = strcaracteresprohibidos.ToCharArray
        Dim resultado As Boolean = False


        For carp = 0 To arrayCaractProhib.Length - 1

            If txtPorcentajeIndirectos.Text.Contains(arrayCaractProhib(carp)) Then
                txtPorcentajeIndirectos.Text = txtPorcentajeIndirectos.Text.Replace(arrayCaractProhib(carp), "")
                resultado = True
            End If

        Next carp

        If txtPorcentajeIndirectos.Text.Contains(".") Then

            Dim comparaPuntos As Char() = txtPorcentajeIndirectos.Text.ToCharArray
            Dim cuantosPuntos As Integer = 0


            For letra = 0 To comparaPuntos.Length - 1

                If comparaPuntos(letra) = "." Then
                    cuantosPuntos = cuantosPuntos + 1
                End If

            Next

            If cuantosPuntos > 1 Then

                For cantidad = 1 To cuantosPuntos
                    Dim lugar As Integer = txtPorcentajeIndirectos.Text.LastIndexOf(".")
                    Dim longitud As Integer = txtPorcentajeIndirectos.Text.Length

                    If longitud > (lugar + 1) Then
                        txtPorcentajeIndirectos.Text = txtPorcentajeIndirectos.Text.Substring(0, lugar) & txtPorcentajeIndirectos.Text.Substring(lugar + 1)
                        resultado = True
                        Exit For
                    Else
                        txtPorcentajeIndirectos.Text = txtPorcentajeIndirectos.Text.Substring(0, lugar)
                        resultado = True
                        Exit For
                    End If
                Next

            End If

        End If

        If resultado = True Then
            txtPorcentajeIndirectos.Select(txtPorcentajeIndirectos.Text.Length, 0)
        End If

    End Sub


    Private Sub txtPorcentajeUtilidades_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtPorcentajeUtilidades.KeyUp

        Dim strcaracteresprohibidos As String = "abcdefghijklmnñopqrstuvwxyz|°!#$%&/()=?¡*¨[]_:;,-{}+´¿'¬^`~@\<> "
        Dim arrayCaractProhib As Char() = strcaracteresprohibidos.ToCharArray
        Dim resultado As Boolean = False


        For carp = 0 To arrayCaractProhib.Length - 1

            If txtPorcentajeUtilidades.Text.Contains(arrayCaractProhib(carp)) Then
                txtPorcentajeUtilidades.Text = txtPorcentajeUtilidades.Text.Replace(arrayCaractProhib(carp), "")
                resultado = True
            End If

        Next carp

        If txtPorcentajeUtilidades.Text.Contains(".") Then

            Dim comparaPuntos As Char() = txtPorcentajeUtilidades.Text.ToCharArray
            Dim cuantosPuntos As Integer = 0


            For letra = 0 To comparaPuntos.Length - 1

                If comparaPuntos(letra) = "." Then
                    cuantosPuntos = cuantosPuntos + 1
                End If

            Next

            If cuantosPuntos > 1 Then

                For cantidad = 1 To cuantosPuntos
                    Dim lugar As Integer = txtPorcentajeUtilidades.Text.LastIndexOf(".")
                    Dim longitud As Integer = txtPorcentajeUtilidades.Text.Length

                    If longitud > (lugar + 1) Then
                        txtPorcentajeUtilidades.Text = txtPorcentajeUtilidades.Text.Substring(0, lugar) & txtPorcentajeUtilidades.Text.Substring(lugar + 1)
                        resultado = True
                        Exit For
                    Else
                        txtPorcentajeUtilidades.Text = txtPorcentajeUtilidades.Text.Substring(0, lugar)
                        resultado = True
                        Exit For
                    End If
                Next

            End If

        End If

        If resultado = True Then
            txtPorcentajeUtilidades.Select(txtPorcentajeUtilidades.Text.Length, 0)
        End If

    End Sub


    Private Sub txtPorcentajeIndirectos_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtPorcentajeIndirectos.TextChanged

        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        If isFormReadyForAction = False Then
            Exit Sub
        End If

        Dim strcaracteresprohibidos As String = "abcdefghijklmnopqrstuvwxyzñABCDEFGHIJKLMNOPQRSTUVWXYZÑ|°!#$%&/()=?¡*¨[]_:;,-{}+´¿'¬^`~@\<> "
        Dim arrayCaractProhib As Char() = strcaracteresprohibidos.ToCharArray
        txtPorcentajeIndirectos.Text = txtPorcentajeIndirectos.Text.Trim(arrayCaractProhib)

        Dim valor As Double = 0.0
        Try
            valor = CDbl(txtPorcentajeIndirectos.Text.Trim.Trim("--").Trim("'").Trim("@", ""))

            If IsBase = True Then
                executeSQLCommand(0, "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Cards SET dcardindirectcostspercentage = " & valor / 100 & ", iupdatedate = " & getMySQLDate() & ", supdatetime = '" & getAppTime() & "', supdateusername = '" & susername & "' WHERE ibaseid = " & iprojectid & " AND icardid = " & icardid)
            ElseIf IsModel = True Then
                executeSQLCommand(0, "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "Cards SET dcardindirectcostspercentage = " & valor / 100 & ", iupdatedate = " & getMySQLDate() & ", supdatetime = '" & getAppTime() & "', supdateusername = '" & susername & "' WHERE imodelid = " & iprojectid & " AND icardid = " & icardid)
            Else
                executeSQLCommand(0, "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "Cards SET dcardindirectcostspercentage = " & valor / 100 & ", iupdatedate = " & getMySQLDate() & ", supdatetime = '" & getAppTime() & "', supdateusername = '" & susername & "' WHERE iprojectid = " & iprojectid & " AND icardid = " & icardid)
            End If

        Catch ex As Exception

        End Try

        isUpdatingPercentages = True
        updateTotals()
        isUpdatingPercentages = False

        Cursor.Current = System.Windows.Forms.Cursors.Default

    End Sub


    Private Sub txtPorcentajeUtilidades_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtPorcentajeUtilidades.TextChanged

        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        If isFormReadyForAction = False Then
            Exit Sub
        End If

        Dim strcaracteresprohibidos As String = "abcdefghijklmnopqrstuvwxyzñABCDEFGHIJKLMNOPQRSTUVWXYZÑ|°!#$%&/()=?¡*¨[]_:;,-{}+´¿'¬^`~@\<> "
        Dim arrayCaractProhib As Char() = strcaracteresprohibidos.ToCharArray
        txtPorcentajeUtilidades.Text = txtPorcentajeUtilidades.Text.Trim(arrayCaractProhib)

        Dim valor As Double = 0.0
        Try
            valor = CDbl(txtPorcentajeUtilidades.Text.Trim.Trim("--").Trim("'").Trim("@", ""))

            If IsBase = True Then
                executeSQLCommand(0, "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Cards SET dcardgainpercentage = " & valor / 100 & ", iupdatedate = " & getMySQLDate() & ", supdatetime = '" & getAppTime() & "', supdateusername = '" & susername & "' WHERE ibaseid = " & iprojectid & " AND icardid = " & icardid)
            ElseIf IsModel = True Then
                executeSQLCommand(0, "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "Cards SET dcardgainpercentage = " & valor / 100 & ", iupdatedate = " & getMySQLDate() & ", supdatetime = '" & getAppTime() & "', supdateusername = '" & susername & "' WHERE imodelid = " & iprojectid & " AND icardid = " & icardid)
            Else
                executeSQLCommand(0, "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "Cards SET dcardgainpercentage = " & valor / 100 & ", iupdatedate = " & getMySQLDate() & ", supdatetime = '" & getAppTime() & "', supdateusername = '" & susername & "' WHERE iprojectid = " & iprojectid & " AND icardid = " & icardid)
            End If

        Catch ex As Exception

        End Try

        isUpdatingPercentages = True
        updateTotals()
        isUpdatingPercentages = False

        Cursor.Current = System.Windows.Forms.Cursors.Default

    End Sub


    Private Sub btnNuevoInsumo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNuevoInsumo.Click

        If newInputPermission = False Then
            Exit Sub
        End If

        If IsEdit = False And icardid = 0 And validaDatosInicialesTarjeta(True, False) = True Then

            Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

            If isFormReadyForAction = False Then
                Exit Sub
            End If

            If validaDatosInicialesTarjeta(True, False) = False Then
                Exit Sub
            Else
                dgvConceptosTarjeta.Enabled = True
                btnNuevoInsumo.Enabled = True
                btnInsertarInsumo.Enabled = True
                btnEliminarInsumo.Enabled = True
            End If


            Dim valorIndirectos As Double = 0.0
            Try
                valorIndirectos = CDbl(txtPorcentajeIndirectos.Text)
            Catch ex As Exception

            End Try

            Dim valorUtilidades As Double = 0.0
            Try
                valorUtilidades = CDbl(txtPorcentajeUtilidades.Text)
            Catch ex As Exception

            End Try


            Dim baseid As Integer = 0
            baseid = getSQLQueryAsInteger(0, "SELECT ibaseid FROM base ORDER BY iupdatedate DESC, supdatetime DESC LIMIT 1")

            If baseid = 0 Then
                baseid = 99999
            End If

            Dim fecha As Integer = 0
            Dim hora As String = ""

            fecha = getMySQLDate()
            hora = getAppTime()

            If IsEdit = True Then

                If IsBase = True Then

                    executeSQLCommand(0, "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Cards SET iupdatedate = " & fecha & ", supdatetime = '" & hora & "', supdateusername = '" & susername & "', scardlegacycategoryid = '" & cmbCategoriaDeLaTarjeta.SelectedValue.ToString & "', scardlegacyid = '" & txtCodigoDeLaTarjeta.Text & "', scarddescription = '" & txtNombreDeLaTarjeta.Text & "', scardunit = '" & txtUnidadDeMedida.Text & "', dcardindirectcostspercentage = " & (valorIndirectos / 100) & ", dcardgainpercentage = " & (valorUtilidades / 100) & " WHERE ibaseid = " & iprojectid & " AND icardid = " & icardid)

                Else

                    If IsModel = True Then

                        executeSQLCommand(0, "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "Cards SET iupdatedate = " & fecha & ", supdatetime = '" & hora & "', supdateusername = '" & susername & "', scardlegacycategoryid = '" & cmbCategoriaDeLaTarjeta.SelectedValue.ToString & "', scardlegacyid = '" & txtCodigoDeLaTarjeta.Text & "', scarddescription = '" & txtNombreDeLaTarjeta.Text & "', scardunit = '" & txtUnidadDeMedida.Text & "', dcardindirectcostspercentage = " & (valorIndirectos / 100) & ", dcardgainpercentage = " & (valorUtilidades / 100) & " WHERE imodelid = " & iprojectid & " AND icardid = " & icardid)

                    Else

                        executeSQLCommand(0, "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "Cards SET iupdatedate = " & fecha & ", supdatetime = '" & hora & "', supdateusername = '" & susername & "', scardlegacycategoryid = '" & cmbCategoriaDeLaTarjeta.SelectedValue.ToString & "', scardlegacyid = '" & txtCodigoDeLaTarjeta.Text & "', scarddescription = '" & txtNombreDeLaTarjeta.Text & "', scardunit = '" & txtUnidadDeMedida.Text & "', dcardindirectcostspercentage = " & (valorIndirectos / 100) & ", dcardgainpercentage = " & (valorUtilidades / 100) & " WHERE iprojectid = " & iprojectid & " AND icardid = " & icardid)

                    End If

                End If

            Else

                Dim checkIfItsOnlyTextUpdate As Boolean = False

                checkIfItsOnlyTextUpdate = getSQLQueryAsBoolean(0, "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Cards WHERE ibaseid = " & baseid & " AND icardid = " & icardid)

                If checkIfItsOnlyTextUpdate = True Then

                    Dim queriesUpdate(2) As String

                    If IsBase = True Then

                        queriesUpdate(0) = "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Cards SET iupdatedate = " & fecha & ", supdatetime = '" & hora & "', supdateusername = '" & susername & "', scardlegacycategoryid = '" & cmbCategoriaDeLaTarjeta.SelectedValue.ToString & "', scardlegacyid = '" & txtCodigoDeLaTarjeta.Text & "', scarddescription = '" & txtNombreDeLaTarjeta.Text & "', scardunit = '" & txtUnidadDeMedida.Text & "', dcardindirectcostspercentage = " & (valorIndirectos / 100) & ", dcardgainpercentage = " & (valorUtilidades / 100) & " WHERE ibaseid = " & baseid & " AND icardid = " & icardid

                    Else

                        If IsModel = True Then

                            queriesUpdate(0) = "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Cards SET iupdatedate = " & fecha & ", supdatetime = '" & hora & "', supdateusername = '" & susername & "', scardlegacycategoryid = '" & cmbCategoriaDeLaTarjeta.SelectedValue.ToString & "', scardlegacyid = '" & txtCodigoDeLaTarjeta.Text & "', scarddescription = '" & txtNombreDeLaTarjeta.Text & "', scardunit = '" & txtUnidadDeMedida.Text & "', dcardindirectcostspercentage = " & (valorIndirectos / 100) & ", dcardgainpercentage = " & (valorUtilidades / 100) & " WHERE ibaseid = " & baseid & " AND icardid = " & icardid
                            queriesUpdate(1) = "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "Cards SET iupdatedate = " & fecha & ", supdatetime = '" & hora & "', supdateusername = '" & susername & "', scardlegacycategoryid = '" & cmbCategoriaDeLaTarjeta.SelectedValue.ToString & "', scardlegacyid = '" & txtCodigoDeLaTarjeta.Text & "', scarddescription = '" & txtNombreDeLaTarjeta.Text & "', scardunit = '" & txtUnidadDeMedida.Text & "', dcardindirectcostspercentage = " & (valorIndirectos / 100) & ", dcardgainpercentage = " & (valorUtilidades / 100) & " WHERE imodelid = " & iprojectid & " AND icardid = " & icardid

                        Else

                            queriesUpdate(0) = "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Cards SET iupdatedate = " & fecha & ", supdatetime = '" & hora & "', supdateusername = '" & susername & "', scardlegacycategoryid = '" & cmbCategoriaDeLaTarjeta.SelectedValue.ToString & "', scardlegacyid = '" & txtCodigoDeLaTarjeta.Text & "', scarddescription = '" & txtNombreDeLaTarjeta.Text & "', scardunit = '" & txtUnidadDeMedida.Text & "', dcardindirectcostspercentage = " & (valorIndirectos / 100) & ", dcardgainpercentage = " & (valorUtilidades / 100) & " WHERE ibaseid = " & baseid & " AND icardid = " & icardid
                            queriesUpdate(1) = "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "Cards SET iupdatedate = " & fecha & ", supdatetime = '" & hora & "', supdateusername = '" & susername & "', scardlegacycategoryid = '" & cmbCategoriaDeLaTarjeta.SelectedValue.ToString & "', scardlegacyid = '" & txtCodigoDeLaTarjeta.Text & "', scarddescription = '" & txtNombreDeLaTarjeta.Text & "', scardunit = '" & txtUnidadDeMedida.Text & "', dcardindirectcostspercentage = " & (valorIndirectos / 100) & ", dcardgainpercentage = " & (valorUtilidades / 100) & " WHERE iprojectid = " & iprojectid & " AND icardid = " & icardid

                        End If

                    End If

                    executeTransactedSQLCommand(0, queriesUpdate)

                Else

                    icardid = getSQLQueryAsInteger(0, "SELECT IF(MAX(icardid) + 1 IS NULL, 1, MAX(icardid) + 1) AS icardid FROM basecards WHERE ibaseid = " & baseid)
                    scardlegacyid = txtCodigoDeLaTarjeta.Text
                    scarddescription = txtNombreDeLaTarjeta.Text

                    Dim queriesInsert(2) As String

                    If IsBase = True Then

                        queriesInsert(0) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Cards VALUES (" & baseid & ", " & icardid & ", '" & cmbCategoriaDeLaTarjeta.SelectedValue & "', '" & txtCodigoDeLaTarjeta.Text & "', '" & txtNombreDeLaTarjeta.Text & "', '" & txtUnidadDeMedida.Text & "', 1, " & (valorIndirectos / 100) & ", " & (valorUtilidades / 100) & ", " & fecha & ", '" & hora & "', '" & susername & "')"

                    Else

                        If IsModel = True Then

                            queriesInsert(0) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Cards VALUES (" & baseid & ", " & icardid & ", '" & cmbCategoriaDeLaTarjeta.SelectedValue & "', '" & txtCodigoDeLaTarjeta.Text & "', '" & txtNombreDeLaTarjeta.Text & "', '" & txtUnidadDeMedida.Text & "', 1, " & (valorIndirectos / 100) & ", " & (valorUtilidades / 100) & ", " & fecha & ", '" & hora & "', '" & susername & "')"
                            queriesInsert(1) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "Cards VALUES (" & iprojectid & ", " & icardid & ", '" & cmbCategoriaDeLaTarjeta.SelectedValue & "', '" & txtCodigoDeLaTarjeta.Text & "', '" & txtNombreDeLaTarjeta.Text & "', '" & txtUnidadDeMedida.Text & "', 1, " & (valorIndirectos / 100) & ", " & (valorUtilidades / 100) & ", " & fecha & ", '" & hora & "', '" & susername & "')"

                        Else

                            queriesInsert(0) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Cards VALUES (" & baseid & ", " & icardid & ", '" & cmbCategoriaDeLaTarjeta.SelectedValue & "', '" & txtCodigoDeLaTarjeta.Text & "', '" & txtNombreDeLaTarjeta.Text & "', '" & txtUnidadDeMedida.Text & "', 1, " & (valorIndirectos / 100) & ", " & (valorUtilidades / 100) & ", " & fecha & ", '" & hora & "', '" & susername & "')"
                            queriesInsert(1) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "Cards VALUES (" & iprojectid & ", " & icardid & ", '" & cmbCategoriaDeLaTarjeta.SelectedValue & "', '" & txtCodigoDeLaTarjeta.Text & "', '" & txtNombreDeLaTarjeta.Text & "', '" & txtUnidadDeMedida.Text & "', 1, " & (valorIndirectos / 100) & ", " & (valorUtilidades / 100) & ", " & fecha & ", '" & hora & "', '" & susername & "')"

                        End If

                    End If

                    executeTransactedSQLCommand(0, queriesInsert)

                End If

            End If

            Cursor.Current = System.Windows.Forms.Cursors.Default

        End If

        'Inicia Código de botón Nuevo Insumo

        Dim bipni As New BuscaInsumosPreguntaTipoNuevoInsumo
        bipni.susername = susername
        bipni.bactive = bactive
        bipni.bonline = bonline
        bipni.suserfullname = suserfullname

        bipni.suseremail = suseremail
        bipni.susersession = susersession
        bipni.susermachinename = susermachinename
        bipni.suserip = suserip

        bipni.ShowDialog(Me)

        If bipni.DialogResult = Windows.Forms.DialogResult.OK Then

            Dim fecha As Integer = 0
            Dim hora As String = ""

            fecha = getMySQLDate()
            hora = getAppTime()

            If bipni.iselectedoption = 1 Then

                'Nuevo Insumo Normal

                Dim ai As New AgregarInsumo
                ai.susername = susername
                ai.bactive = bactive
                ai.bonline = bonline
                ai.suserfullname = suserfullname

                ai.suseremail = suseremail
                ai.susersession = susersession
                ai.susermachinename = susermachinename
                ai.suserip = suserip

                ai.iprojectid = iprojectid
                ai.icardid = icardid

                ai.IsEdit = False
                ai.IsHistoric = False
                ai.IsModel = IsModel
                ai.IsBase = IsBase

                If Me.WindowState = FormWindowState.Maximized Then
                    ai.WindowState = FormWindowState.Maximized
                End If

                ai.ShowDialog(Me)

                If ai.DialogResult = Windows.Forms.DialogResult.OK Then

                    Dim dsBusquedaInsumosRepetidos As DataSet

                    If IsBase = True Then

                        dsBusquedaInsumosRepetidos = getSQLQueryAsDataset(0, "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardInputs WHERE ibaseid = " & iprojectid & " AND icardid = " & icardid & " AND iinputid = " & ai.iinputid)

                    Else

                        If IsModel = True Then

                            dsBusquedaInsumosRepetidos = getSQLQueryAsDataset(0, "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "CardInputs WHERE imodelid = " & iprojectid & " AND icardid = " & icardid & " AND iinputid = " & ai.iinputid)

                        Else

                            dsBusquedaInsumosRepetidos = getSQLQueryAsDataset(0, "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "CardInputs WHERE iprojectid = " & iprojectid & " AND icardid = " & icardid & " AND iinputid = " & ai.iinputid)

                        End If

                    End If


                    If dsBusquedaInsumosRepetidos.Tables(0).Rows.Count > 0 Then

                        MsgBox("Ya tienes ese Insumo insertado en esta Tarjeta. ¿Podrías buscarla en la lista y cambiar la cantidad si así lo deseas?", MsgBoxStyle.OkOnly, "Insumo Repetido")
                        updateEverything()
                        isConceptosTarjetaDGVReady = True
                        Exit Sub

                    End If

                    Dim cantidaddeinsumo As Double = 1.0

                    Dim isManoDeObra As Boolean = False
                    Dim conteoMO As Integer = 0
                    Dim busquedaMO As String = ""

                    isManoDeObra = getSQLQueryAsBoolean(0, "SELECT * FROM inputs i JOIN inputtypes it ON i.iinputid = it.iinputid WHERE it.sinputtypedescription = 'MANO DE OBRA' AND i.iinputid = " & ai.iinputid)


                    If IsEdit = True Then


                        If IsBase = True Then

                            Dim queriesInsert(3) As String

                            queriesInsert(0) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardInputs SELECT " & iprojectid & ", " & icardid & ", iinputid, sinputunit, " & cantidaddeinsumo & ", " & fecha & ", '" & hora & "', '" & susername & "' FROM inputs WHERE iinputid = " & ai.iinputid

                            If isManoDeObra = True Then

                                busquedaMO = "" & _
                                "SELECT COUNT(*) " & _
                                "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardInputs btfi " & _
                                "JOIN inputtypes it ON it.iinputid = btfi.iinputid " & _
                                "WHERE btfi.ibaseid = " & iprojectid & " AND btfi.icardid = " & icardid & " " & _
                                "AND it.sinputtypedescription = 'MANO DE OBRA' "

                                conteoMO = getSQLQueryAsDouble(0, busquedaMO)

                                If conteoMO = 0 Then
                                    queriesInsert(1) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardInputs SELECT " & iprojectid & ", " & icardid & ", 0, '%', 1, " & fecha & ", '" & hora & "', '" & susername & "'"
                                End If

                            End If

                            executeTransactedSQLCommand(0, queriesInsert)

                        Else

                            If IsModel = True Then

                                Dim queriesInsert(3) As String

                                queriesInsert(0) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "CardInputs SELECT " & iprojectid & ", " & icardid & ", iinputid, sinputunit, " & cantidaddeinsumo & ", " & fecha & ", '" & hora & "', '" & susername & "' FROM inputs WHERE iinputid = " & ai.iinputid

                                If isManoDeObra = True Then

                                    busquedaMO = "" & _
                                    "SELECT COUNT(*) " & _
                                    "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "CardInputs mtfi " & _
                                    "JOIN inputtypes it ON it.iinputid = mtfi.iinputid " & _
                                    "WHERE mtfi.imodelid = " & iprojectid & " AND mtfi.icardid = " & icardid & " " & _
                                    "AND it.sinputtypedescription = 'MANO DE OBRA' "

                                    conteoMO = getSQLQueryAsDouble(0, busquedaMO)

                                    If conteoMO = 0 Then
                                        queriesInsert(1) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "CardInputs SELECT " & iprojectid & ", " & icardid & ", 0, '%', 1, " & fecha & ", '" & hora & "', '" & susername & "'"
                                    End If

                                End If

                                executeTransactedSQLCommand(0, queriesInsert)

                            Else

                                Dim queriesInsert(3) As String

                                queriesInsert(0) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "CardInputs SELECT " & iprojectid & ", " & icardid & ", iinputid, sinputunit, " & cantidaddeinsumo & ", " & fecha & ", '" & hora & "', '" & susername & "' FROM inputs WHERE iinputid = " & ai.iinputid

                                If isManoDeObra = True Then

                                    busquedaMO = "" & _
                                    "SELECT COUNT(*) " & _
                                    "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "CardInputs ptfi " & _
                                    "JOIN inputtypes it ON it.iinputid = ptfi.iinputid " & _
                                    "WHERE ptfi.iprojectid = " & iprojectid & " AND ptfi.icardid = " & icardid & " " & _
                                    "AND it.sinputtypedescription = 'MANO DE OBRA' "

                                    conteoMO = getSQLQueryAsDouble(0, busquedaMO)

                                    If conteoMO = 0 Then
                                        queriesInsert(1) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "CardInputs SELECT " & iprojectid & ", " & icardid & ", 0, '%', 1, " & fecha & ", '" & hora & "', '" & susername & "'"
                                    End If

                                End If

                                executeTransactedSQLCommand(0, queriesInsert)

                            End If

                        End If


                    Else

                        Dim baseid As Integer = 0
                        baseid = getSQLQueryAsInteger(0, "SELECT ibaseid FROM base ORDER BY iupdatedate DESC, supdatetime DESC LIMIT 1")

                        If baseid = 0 Then
                            baseid = 99999
                        End If

                        Dim queriesInsert(6) As String

                        If IsBase = True Then

                            queriesInsert(0) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardInputs SELECT " & baseid & ", " & icardid & ", iinputid, sinputunit, " & cantidaddeinsumo & ", " & fecha & ", '" & hora & "', '" & susername & "' FROM inputs WHERE iinputid = " & ai.iinputid

                            If isManoDeObra = True Then

                                busquedaMO = "" & _
                                "SELECT COUNT(*) " & _
                                "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardInputs btfi " & _
                                "JOIN inputtypes it ON it.iinputid = btfi.iinputid " & _
                                "WHERE btfi.ibaseid = " & baseid & " AND btfi.icardid = " & icardid & " " & _
                                "AND it.sinputtypedescription = 'MANO DE OBRA' "

                                conteoMO = getSQLQueryAsDouble(0, busquedaMO)

                                If conteoMO = 0 Then
                                    queriesInsert(1) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardInputs SELECT " & baseid & ", " & icardid & ", 0, '%', 1, " & fecha & ", '" & hora & "', '" & susername & "'"
                                End If

                            End If

                            executeTransactedSQLCommand(0, queriesInsert)

                        ElseIf IsModel = True Then

                            queriesInsert(0) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardInputs SELECT " & baseid & ", " & icardid & ", iinputid, sinputunit, " & cantidaddeinsumo & ", " & fecha & ", '" & hora & "', '" & susername & "' FROM inputs WHERE iinputid = " & ai.iinputid

                            If isManoDeObra = True Then

                                busquedaMO = "" & _
                                "SELECT COUNT(*) " & _
                                "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardInputs btfi " & _
                                "JOIN inputtypes it ON it.iinputid = btfi.iinputid " & _
                                "WHERE btfi.ibaseid = " & baseid & " AND btfi.icardid = " & icardid & " " & _
                                "AND it.sinputtypedescription = 'MANO DE OBRA' "

                                conteoMO = getSQLQueryAsDouble(0, busquedaMO)

                                If conteoMO = 0 Then
                                    queriesInsert(1) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardInputs SELECT " & baseid & ", " & icardid & ", 0, '%', 1, " & fecha & ", '" & hora & "', '" & susername & "'"
                                End If

                            End If

                            queriesInsert(3) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "CardInputs SELECT " & iprojectid & ", " & icardid & ", iinputid, sinputunit, " & cantidaddeinsumo & ", " & fecha & ", '" & hora & "', '" & susername & "' FROM inputs WHERE iinputid = " & ai.iinputid

                            If isManoDeObra = True Then

                                busquedaMO = "" & _
                                "SELECT COUNT(*) " & _
                                "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "CardInputs mtfi " & _
                                "JOIN inputtypes it ON it.iinputid = mtfi.iinputid " & _
                                "WHERE mtfi.imodelid = " & iprojectid & " AND mtfi.icardid = " & icardid & " " & _
                                "AND it.sinputtypedescription = 'MANO DE OBRA' "

                                conteoMO = getSQLQueryAsDouble(0, busquedaMO)

                                If conteoMO = 0 Then
                                    queriesInsert(4) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "CardInputs SELECT " & iprojectid & ", " & icardid & ", 0, '%', 1, " & fecha & ", '" & hora & "', '" & susername & "'"
                                End If

                            End If

                            executeTransactedSQLCommand(0, queriesInsert)

                        Else

                            queriesInsert(0) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardInputs SELECT " & baseid & ", " & icardid & ", iinputid, sinputunit, " & cantidaddeinsumo & ", " & fecha & ", '" & hora & "', '" & susername & "' FROM inputs WHERE iinputid = " & ai.iinputid

                            If isManoDeObra = True Then

                                busquedaMO = "" & _
                                "SELECT COUNT(*) " & _
                                "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardInputs btfi " & _
                                "JOIN inputtypes it ON it.iinputid = btfi.iinputid " & _
                                "WHERE btfi.ibaseid = " & baseid & " AND btfi.icardid = " & icardid & " " & _
                                "AND it.sinputtypedescription = 'MANO DE OBRA' "

                                conteoMO = getSQLQueryAsDouble(0, busquedaMO)

                                If conteoMO = 0 Then
                                    queriesInsert(1) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardInputs SELECT " & baseid & ", " & icardid & ", 0, '%', 1, " & fecha & ", '" & hora & "', '" & susername & "'"
                                End If

                            End If

                            queriesInsert(3) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "CardInputs SELECT " & iprojectid & ", " & icardid & ", iinputid, sinputunit, " & cantidaddeinsumo & ", " & fecha & ", '" & hora & "', '" & susername & "' FROM inputs WHERE iinputid = " & ai.iinputid

                            If isManoDeObra = True Then

                                busquedaMO = "" & _
                                "SELECT COUNT(*) " & _
                                "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "CardInputs ptfi " & _
                                "JOIN inputtypes it ON it.iinputid = ptfi.iinputid " & _
                                "WHERE ptfi.iprojectid = " & iprojectid & " AND ptfi.icardid = " & icardid & " " & _
                                "AND it.sinputtypedescription = 'MANO DE OBRA' "

                                conteoMO = getSQLQueryAsDouble(0, busquedaMO)

                                If conteoMO = 0 Then
                                    queriesInsert(4) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "CardInputs SELECT " & iprojectid & ", " & icardid & ", 0, '%', 1, " & fecha & ", '" & hora & "', '" & susername & "'"
                                End If

                            End If

                            executeTransactedSQLCommand(0, queriesInsert)

                        End If

                    End If

                    updateEverything()

                    isConceptosTarjetaDGVReady = True

                End If

            ElseIf bipni.iselectedoption = 2 Then


                ' Nuevo Insumo Compuesto

                Dim aic As New AgregarInsumoCompuesto
                aic.susername = susername
                aic.bactive = bactive
                aic.bonline = bonline
                aic.suserfullname = suserfullname

                aic.suseremail = suseremail
                aic.susersession = susersession
                aic.susermachinename = susermachinename
                aic.suserip = suserip


                aic.iprojectid = iprojectid
                aic.icardid = icardid

                aic.IsEdit = False
                aic.IsHistoric = False
                aic.IsModel = IsModel
                aic.IsBase = IsBase

                If Me.WindowState = FormWindowState.Maximized Then
                    aic.WindowState = FormWindowState.Maximized
                End If

                aic.ShowDialog(Me)

                If aic.DialogResult = Windows.Forms.DialogResult.OK Then

                    updateEverything()

                    isConceptosTarjetaDGVReady = True

                End If

            End If

        End If

    End Sub


    Private Sub btnInsertarInsumo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnInsertarInsumo.Click

        Dim baseid As Integer = 0
        baseid = getSQLQueryAsInteger(0, "SELECT ibaseid FROM base ORDER BY iupdatedate DESC, supdatetime DESC LIMIT 1")

        If baseid = 0 Then
            baseid = 99999
        End If

        Dim fecha As Integer = 0
        Dim hora As String = ""

        fecha = getMySQLDate()
        hora = getAppTime()

        Dim firstTimeCompoundAddedAlert As Boolean = False


        If IsEdit = False And icardid = 0 And validaDatosInicialesTarjeta(True, False) = True Then

            Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

            If isFormReadyForAction = False Then
                Exit Sub
            End If

            If validaDatosInicialesTarjeta(True, False) = False Then
                Exit Sub
            Else
                dgvConceptosTarjeta.Enabled = True
                btnNuevoInsumo.Enabled = True
                btnInsertarInsumo.Enabled = True
                btnEliminarInsumo.Enabled = True
            End If


            Dim valorIndirectos As Double = 0.0
            Try
                valorIndirectos = CDbl(txtPorcentajeIndirectos.Text)
            Catch ex As Exception

            End Try

            Dim valorUtilidades As Double = 0.0
            Try
                valorUtilidades = CDbl(txtPorcentajeUtilidades.Text)
            Catch ex As Exception

            End Try

            If IsEdit = True Then

                If IsBase = True Then

                    executeSQLCommand(0, "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Cards SET iupdatedate = " & fecha & ", supdatetime = '" & hora & "', supdateusername = '" & susername & "', scardlegacycategoryid = '" & cmbCategoriaDeLaTarjeta.SelectedValue.ToString & "', scardlegacyid = '" & txtCodigoDeLaTarjeta.Text & "', scarddescription = '" & txtNombreDeLaTarjeta.Text & "', scardunit = '" & txtUnidadDeMedida.Text & "', dcardindirectcostspercentage = " & (valorIndirectos / 100) & ", dcardgainpercentage = " & (valorUtilidades / 100) & " WHERE ibaseid = " & iprojectid & " AND icardid = " & icardid)

                Else

                    If IsModel = True Then

                        executeSQLCommand(0, "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "Cards SET iupdatedate = " & fecha & ", supdatetime = '" & hora & "', supdateusername = '" & susername & "', scardlegacycategoryid = '" & cmbCategoriaDeLaTarjeta.SelectedValue.ToString & "', scardlegacyid = '" & txtCodigoDeLaTarjeta.Text & "', scarddescription = '" & txtNombreDeLaTarjeta.Text & "', scardunit = '" & txtUnidadDeMedida.Text & "', dcardindirectcostspercentage = " & (valorIndirectos / 100) & ", dcardgainpercentage = " & (valorUtilidades / 100) & " WHERE imodelid = " & iprojectid & " AND icardid = " & icardid)

                    Else

                        executeSQLCommand(0, "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "Cards SET iupdatedate = " & fecha & ", supdatetime = '" & hora & "', supdateusername = '" & susername & "', scardlegacycategoryid = '" & cmbCategoriaDeLaTarjeta.SelectedValue.ToString & "', scardlegacyid = '" & txtCodigoDeLaTarjeta.Text & "', scarddescription = '" & txtNombreDeLaTarjeta.Text & "', scardunit = '" & txtUnidadDeMedida.Text & "', dcardindirectcostspercentage = " & (valorIndirectos / 100) & ", dcardgainpercentage = " & (valorUtilidades / 100) & " WHERE iprojectid = " & iprojectid & " AND icardid = " & icardid)

                    End If

                End If

            Else

                Dim checkIfItsOnlyTextUpdate As Boolean = False

                checkIfItsOnlyTextUpdate = getSQLQueryAsBoolean(0, "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Cards WHERE ibaseid = " & baseid & " AND icardid = " & icardid)

                If checkIfItsOnlyTextUpdate = True Then

                    Dim queriesUpdate(2) As String

                    If IsBase = True Then

                        queriesUpdate(0) = "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Cards SET iupdatedate = " & fecha & ", supdatetime = '" & hora & "', supdateusername = '" & susername & "', scardlegacycategoryid = '" & cmbCategoriaDeLaTarjeta.SelectedValue.ToString & "', scardlegacyid = '" & txtCodigoDeLaTarjeta.Text & "', scarddescription = '" & txtNombreDeLaTarjeta.Text & "', scardunit = '" & txtUnidadDeMedida.Text & "', dcardindirectcostspercentage = " & (valorIndirectos / 100) & ", dcardgainpercentage = " & (valorUtilidades / 100) & " WHERE ibaseid = " & baseid & " AND icardid = " & icardid

                    Else

                        If IsModel = True Then

                            queriesUpdate(0) = "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Cards SET iupdatedate = " & fecha & ", supdatetime = '" & hora & "', supdateusername = '" & susername & "', scardlegacycategoryid = '" & cmbCategoriaDeLaTarjeta.SelectedValue.ToString & "', scardlegacyid = '" & txtCodigoDeLaTarjeta.Text & "', scarddescription = '" & txtNombreDeLaTarjeta.Text & "', scardunit = '" & txtUnidadDeMedida.Text & "', dcardindirectcostspercentage = " & (valorIndirectos / 100) & ", dcardgainpercentage = " & (valorUtilidades / 100) & " WHERE ibaseid = " & baseid & " AND icardid = " & icardid
                            queriesUpdate(1) = "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "Cards SET iupdatedate = " & fecha & ", supdatetime = '" & hora & "', supdateusername = '" & susername & "', scardlegacycategoryid = '" & cmbCategoriaDeLaTarjeta.SelectedValue.ToString & "', scardlegacyid = '" & txtCodigoDeLaTarjeta.Text & "', scarddescription = '" & txtNombreDeLaTarjeta.Text & "', scardunit = '" & txtUnidadDeMedida.Text & "', dcardindirectcostspercentage = " & (valorIndirectos / 100) & ", dcardgainpercentage = " & (valorUtilidades / 100) & " WHERE imodelid = " & iprojectid & " AND icardid = " & icardid

                        Else

                            queriesUpdate(0) = "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Cards SET iupdatedate = " & fecha & ", supdatetime = '" & hora & "', supdateusername = '" & susername & "', scardlegacycategoryid = '" & cmbCategoriaDeLaTarjeta.SelectedValue.ToString & "', scardlegacyid = '" & txtCodigoDeLaTarjeta.Text & "', scarddescription = '" & txtNombreDeLaTarjeta.Text & "', scardunit = '" & txtUnidadDeMedida.Text & "', dcardindirectcostspercentage = " & (valorIndirectos / 100) & ", dcardgainpercentage = " & (valorUtilidades / 100) & " WHERE ibaseid = " & baseid & " AND icardid = " & icardid
                            queriesUpdate(1) = "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "Cards SET iupdatedate = " & fecha & ", supdatetime = '" & hora & "', supdateusername = '" & susername & "', scardlegacycategoryid = '" & cmbCategoriaDeLaTarjeta.SelectedValue.ToString & "', scardlegacyid = '" & txtCodigoDeLaTarjeta.Text & "', scarddescription = '" & txtNombreDeLaTarjeta.Text & "', scardunit = '" & txtUnidadDeMedida.Text & "', dcardindirectcostspercentage = " & (valorIndirectos / 100) & ", dcardgainpercentage = " & (valorUtilidades / 100) & " WHERE iprojectid = " & iprojectid & " AND icardid = " & icardid

                        End If

                    End If

                    executeTransactedSQLCommand(0, queriesUpdate)

                Else

                    icardid = getSQLQueryAsInteger(0, "SELECT IF(MAX(icardid) + 1 IS NULL, 1, MAX(icardid) + 1) AS icardid FROM basecards WHERE ibaseid = " & baseid)
                    scardlegacyid = txtCodigoDeLaTarjeta.Text
                    scarddescription = txtNombreDeLaTarjeta.Text

                    Dim queriesInsert(2) As String

                    If IsBase = True Then

                        queriesInsert(0) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Cards VALUES (" & baseid & ", " & icardid & ", '" & cmbCategoriaDeLaTarjeta.SelectedValue & "', '" & txtCodigoDeLaTarjeta.Text & "', '" & txtNombreDeLaTarjeta.Text & "', '" & txtUnidadDeMedida.Text & "', 1, " & (valorIndirectos / 100) & ", " & (valorUtilidades / 100) & ", " & fecha & ", '" & hora & "', '" & susername & "')"

                    Else

                        If IsModel = True Then

                            queriesInsert(0) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Cards VALUES (" & baseid & ", " & icardid & ", '" & cmbCategoriaDeLaTarjeta.SelectedValue & "', '" & txtCodigoDeLaTarjeta.Text & "', '" & txtNombreDeLaTarjeta.Text & "', '" & txtUnidadDeMedida.Text & "', 1, " & (valorIndirectos / 100) & ", " & (valorUtilidades / 100) & ", " & fecha & ", '" & hora & "', '" & susername & "')"
                            queriesInsert(1) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "Cards VALUES (" & iprojectid & ", " & icardid & ", '" & cmbCategoriaDeLaTarjeta.SelectedValue & "', '" & txtCodigoDeLaTarjeta.Text & "', '" & txtNombreDeLaTarjeta.Text & "', '" & txtUnidadDeMedida.Text & "', 1, " & (valorIndirectos / 100) & ", " & (valorUtilidades / 100) & ", " & fecha & ", '" & hora & "', '" & susername & "')"

                        Else

                            queriesInsert(0) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Cards VALUES (" & baseid & ", " & icardid & ", '" & cmbCategoriaDeLaTarjeta.SelectedValue & "', '" & txtCodigoDeLaTarjeta.Text & "', '" & txtNombreDeLaTarjeta.Text & "', '" & txtUnidadDeMedida.Text & "', 1, " & (valorIndirectos / 100) & ", " & (valorUtilidades / 100) & ", " & fecha & ", '" & hora & "', '" & susername & "')"
                            queriesInsert(1) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "Cards VALUES (" & iprojectid & ", " & icardid & ", '" & cmbCategoriaDeLaTarjeta.SelectedValue & "', '" & txtCodigoDeLaTarjeta.Text & "', '" & txtNombreDeLaTarjeta.Text & "', '" & txtUnidadDeMedida.Text & "', 1, " & (valorIndirectos / 100) & ", " & (valorUtilidades / 100) & ", " & fecha & ", '" & hora & "', '" & susername & "')"

                        End If

                    End If

                    executeTransactedSQLCommand(0, queriesInsert)

                End If

            End If

            Cursor.Current = System.Windows.Forms.Cursors.Default

        End If


        'Inicia Código de Insertar Insumo


        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        isConceptosTarjetaDGVReady = False

        Dim chequeoPrimeraVezQueSeInsertaAlgo As Integer = 0

        If IsBase = True Then
            chequeoPrimeraVezQueSeInsertaAlgo = getSQLQueryAsInteger(0, "SELECT COUNT(*) FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Prices WHERE ibaseid = " & iprojectid)
        Else

            If IsModel = True Then
                chequeoPrimeraVezQueSeInsertaAlgo = getSQLQueryAsInteger(0, "SELECT COUNT(*) FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "Prices WHERE imodelid = " & iprojectid)
            Else
                chequeoPrimeraVezQueSeInsertaAlgo = getSQLQueryAsInteger(0, "SELECT COUNT(*) FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "Prices WHERE iprojectid = " & iprojectid)
            End If

        End If

        If chequeoPrimeraVezQueSeInsertaAlgo = 0 Then

            If IsBase = True Then

                Dim queriesPrimeraVez(2) As String
                queriesPrimeraVez(0) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Prices SELECT " & iprojectid & ", iinputid, dinputpricewithoutIVA, dinputprotectionpercentage, dinputfinalprice, " & fecha & ", '" & hora & "', '" & susername & "' FROM (SELECT * FROM baseprices WHERE ibaseid = " & baseid & " ORDER BY iupdatedate DESC, supdatetime DESC) pp GROUP BY iinputid"
                queriesPrimeraVez(1) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Timber SELECT " & iprojectid & ", bt.iinputid, bt.dinputtimberespesor, bt.dinputtimberancho, bt.dinputtimberlargo, bt.dinputtimberpreciopiecubico, " & fecha & ", '" & hora & "', '" & susername & "' FROM basetimber bt where ibaseid = " & baseid
                executeTransactedSQLCommand(0, queriesPrimeraVez)

            Else

                If IsModel = True Then

                    Dim queriesPrimeraVez(2) As String
                    queriesPrimeraVez(0) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "Prices SELECT " & iprojectid & ", iinputid, dinputpricewithoutIVA, dinputprotectionpercentage, dinputfinalprice, " & fecha & ", '" & hora & "', '" & susername & "' FROM (SELECT * FROM baseprices WHERE ibaseid = " & baseid & " ORDER BY iupdatedate DESC, supdatetime DESC) pp GROUP BY iinputid"
                    queriesPrimeraVez(1) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "Timber SELECT " & iprojectid & ", bt.iinputid, bt.dinputtimberespesor, bt.dinputtimberancho, bt.dinputtimberlargo, bt.dinputtimberpreciopiecubico, " & fecha & ", '" & hora & "', '" & susername & "' FROM basetimber bt where ibaseid = " & baseid
                    executeTransactedSQLCommand(0, queriesPrimeraVez)

                Else

                    Dim queriesPrimeraVez(2) As String
                    queriesPrimeraVez(0) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "Prices SELECT " & iprojectid & ", iinputid, dinputpricewithoutIVA, dinputprotectionpercentage, dinputfinalprice, " & fecha & ", '" & hora & "', '" & susername & "' FROM (SELECT * FROM baseprices WHERE ibaseid = " & baseid & " ORDER BY iupdatedate DESC, supdatetime DESC) pp GROUP BY iinputid"
                    queriesPrimeraVez(1) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "Timber SELECT " & iprojectid & ", bt.iinputid, bt.dinputtimberespesor, bt.dinputtimberancho, bt.dinputtimberlargo, bt.dinputtimberpreciopiecubico, " & fecha & ", '" & hora & "', '" & susername & "' FROM basetimber bt WHERE ibaseid = " & baseid
                    executeTransactedSQLCommand(0, queriesPrimeraVez)

                End If

            End If

        End If

        Dim bi As New BuscaInsumos
        bi.susername = susername
        bi.bactive = bactive
        bi.bonline = bonline
        bi.suserfullname = suserfullname

        bi.suseremail = suseremail
        bi.susersession = susersession
        bi.susermachinename = susermachinename
        bi.suserip = suserip

        bi.IsEdit = IsEdit
        bi.IsBase = IsBase
        bi.IsModel = IsModel
        bi.IsHistoric = IsHistoric

        bi.iprojectid = iprojectid
        bi.icardid = icardid

        bi.IsEdit = False

        If Me.WindowState = FormWindowState.Maximized Then
            bi.WindowState = FormWindowState.Maximized
        End If

        bi.ShowDialog(Me)

        If bi.DialogResult = Windows.Forms.DialogResult.OK Then

            Dim isCompound As Boolean

            isCompound = getSQLQueryAsBoolean(0, "SELECT * FROM basecardcompoundinputs WHERE iinputid = " & bi.iinputid)

            If bi.wasCreated = True And isCompound = True Then

                updateEverything()
                isConceptosTarjetaDGVReady = True
                Exit Sub

            End If

            Dim dsBusquedaInsumosRepetidos As DataSet

            If IsBase = True Then

                dsBusquedaInsumosRepetidos = getSQLQueryAsDataset(0, "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardInputs WHERE ibaseid = " & iprojectid & " AND icardid = " & icardid & " AND iinputid = " & bi.iinputid)

            Else

                If IsModel = True Then

                    dsBusquedaInsumosRepetidos = getSQLQueryAsDataset(0, "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "CardInputs WHERE imodelid = " & iprojectid & " AND icardid = " & icardid & " AND iinputid = " & bi.iinputid)

                Else

                    dsBusquedaInsumosRepetidos = getSQLQueryAsDataset(0, "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "CardInputs WHERE iprojectid = " & iprojectid & " AND icardid = " & icardid & " AND iinputid = " & bi.iinputid)

                End If

            End If

            If dsBusquedaInsumosRepetidos.Tables(0).Rows.Count > 0 Then

                MsgBox("Ya tienes ese Insumo insertado en esta Tarjeta. ¿Podrías buscarlo en la lista y cambiar la cantidad si así lo deseas?", MsgBoxStyle.OkOnly, "Insumo Repetido")
                updateEverything()
                isConceptosTarjetaDGVReady = True
                Exit Sub

            End If

            Dim isManoDeObra As Boolean = False
            Dim conteoMO As Integer = 0
            Dim busquedaMO As String = ""

            isManoDeObra = getSQLQueryAsBoolean(0, "SELECT * FROM inputs i JOIN inputtypes it ON i.iinputid = it.iinputid WHERE it.sinputtypedescription = 'MANO DE OBRA' AND i.iinputid = " & bi.iinputid)


            If IsEdit = True Then

                If IsBase = True Then

                    Dim queriesInsert(3) As String

                    queriesInsert(0) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardInputs SELECT " & iprojectid & ", " & icardid & ", " & bi.iinputid & ", sinputunit, 1, " & fecha & ", '" & hora & "', '" & susername & "' FROM inputs WHERE iinputid = " & bi.iinputid

                    If isManoDeObra = True Then

                        busquedaMO = "" & _
                        "SELECT COUNT(*) " & _
                        "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardInputs btfi " & _
                        "JOIN inputtypes it ON it.iinputid = btfi.iinputid " & _
                        "WHERE btfi.ibaseid = " & iprojectid & " AND btfi.icardid = " & icardid & " " & _
                        "AND it.sinputtypedescription = 'MANO DE OBRA' "

                        conteoMO = getSQLQueryAsDouble(0, busquedaMO)

                        If conteoMO = 0 Then
                            queriesInsert(1) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardInputs SELECT " & iprojectid & ", " & icardid & ", 0, '%', 1, " & fecha & ", '" & hora & "', '" & susername & "'"
                        End If

                    End If

                    If isCompound = True Then

                        If getSQLQueryAsBoolean(0, "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardInputs WHERE ibaseid = " & iprojectid & " AND iinputid = " & bi.iinputid) = True Then
                            queriesInsert(2) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardCompoundInputs SELECT * FROM (SELECT " & iprojectid & " AS projectid, " & icardid & " AS cardid, " & bi.iinputid & " AS inputid, bcci.icompoundinputid, i.sinputunit, bcci.dcompoundinputqty AS qty, " & fecha & ", '" & hora & "', '" & susername & "' FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardInputs bci JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardCompoundInputs bcci ON bci.ibaseid = bcci.ibaseid AND bci.icardid = bcci.icardid AND bci.iinputid = bcci.iinputid JOIN inputs i ON i.iinputid = bcci.icompoundinputid WHERE bci.iinputid = " & bi.iinputid & " ORDER BY bci.iupdatedate DESC, bci.supdatetime DESC) cid GROUP BY icompoundinputid "
                        Else
                            If getSQLQueryAsBoolean(0, "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardInputs WHERE ibaseid = " & baseid & " AND iinputid = " & bi.iinputid) = True Then
                                queriesInsert(2) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardCompoundInputs SELECT * FROM (SELECT " & iprojectid & " AS projectid, " & icardid & " AS cardid, " & bi.iinputid & " AS inputid, bcci.icompoundinputid, i.sinputunit, bcci.dcompoundinputqty AS qty, " & fecha & ", '" & hora & "', '" & susername & "' FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardInputs bci JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardCompoundInputs bcci ON bci.ibaseid = bcci.ibaseid AND bci.icardid = bcci.icardid AND bci.iinputid = bcci.iinputid JOIN inputs i ON i.iinputid = bcci.icompoundinputid WHERE bci.ibaseid = " & baseid & " AND bci.iinputid = " & bi.iinputid & " ORDER BY bci.iupdatedate DESC, bci.supdatetime DESC) cid GROUP BY icompoundinputid "
                            Else
                                queriesInsert(2) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardCompoundInputs SELECT * FROM (SELECT " & iprojectid & " AS projectid, " & icardid & " AS cardid, " & bi.iinputid & " AS inputid, bcci.icompoundinputid, i.sinputunit, 1 AS qty, " & fecha & ", '" & hora & "', '" & susername & "' FROM inputs i WHERE i.iinputid = " & bi.iinputid & " ORDER BY i.iupdatedate DESC, i.supdatetime DESC) cid GROUP BY icompoundinputid "
                            End If
                            firstTimeCompoundAddedAlert = True
                        End If

                    End If

                    executeTransactedSQLCommand(0, queriesInsert)

                Else

                    If IsModel = True Then

                        Dim queriesInsert(3) As String

                        queriesInsert(0) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "CardInputs SELECT " & iprojectid & ", " & icardid & ", " & bi.iinputid & ", sinputunit, 1, " & fecha & ", '" & hora & "', '" & susername & "' FROM inputs WHERE iinputid = " & bi.iinputid

                        If isManoDeObra = True Then

                            busquedaMO = "" & _
                            "SELECT COUNT(*) " & _
                            "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "CardInputs mtfi " & _
                            "JOIN inputtypes it ON it.iinputid = mtfi.iinputid " & _
                            "WHERE mtfi.imodelid = " & iprojectid & " AND mtfi.icardid = " & icardid & " " & _
                            "AND it.sinputtypedescription = 'MANO DE OBRA' "

                            conteoMO = getSQLQueryAsDouble(0, busquedaMO)

                            If conteoMO = 0 Then
                                queriesInsert(1) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "CardInputs SELECT " & iprojectid & ", " & icardid & ", 0, '%', 1, " & fecha & ", '" & hora & "', '" & susername & "'"
                            End If

                        End If

                        If isCompound = True Then

                            If getSQLQueryAsBoolean(0, "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "CardInputs WHERE imodelid = " & iprojectid & " AND iinputid = " & bi.iinputid) = True Then
                                queriesInsert(2) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "CardCompoundInputs SELECT * FROM (SELECT " & iprojectid & " AS projectid, " & icardid & " AS cardid, " & bi.iinputid & " AS inputid, bcci.icompoundinputid, i.sinputunit, bcci.dcompoundinputqty AS qty, " & fecha & ", '" & hora & "', '" & susername & "' FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "CardInputs bci JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "CardCompoundInputs bcci ON bci.imodelid = bcci.imodelid AND bci.icardid = bcci.icardid AND bci.iinputid = bcci.iinputid JOIN inputs i ON i.iinputid = bcci.icompoundinputid WHERE bci.iinputid = " & bi.iinputid & " ORDER BY bci.iupdatedate DESC, bci.supdatetime DESC) cid GROUP BY icompoundinputid "
                            Else
                                If getSQLQueryAsBoolean(0, "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardInputs WHERE ibaseid = " & baseid & " AND iinputid = " & bi.iinputid) = True Then
                                    queriesInsert(2) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "CardCompoundInputs SELECT * FROM (SELECT " & iprojectid & " AS projectid, " & icardid & " AS cardid, " & bi.iinputid & " AS inputid, bcci.icompoundinputid, i.sinputunit, bcci.dcompoundinputqty AS qty, " & fecha & ", '" & hora & "', '" & susername & "' FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardInputs bci JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardCompoundInputs bcci ON bci.ibaseid = bcci.ibaseid AND bci.icardid = bcci.icardid AND bci.iinputid = bcci.iinputid JOIN inputs i ON i.iinputid = bcci.icompoundinputid WHERE bci.ibaseid = " & baseid & " AND bci.iinputid = " & bi.iinputid & " ORDER BY bci.iupdatedate DESC, bci.supdatetime DESC) cid GROUP BY icompoundinputid "
                                Else
                                    queriesInsert(2) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "CardCompoundInputs SELECT * FROM (SELECT " & iprojectid & " AS projectid, " & icardid & " AS cardid, " & bi.iinputid & " AS inputid, bcci.icompoundinputid, i.sinputunit, 1 AS qty, " & fecha & ", '" & hora & "', '" & susername & "' FROM inputs i WHERE i.iinputid = " & bi.iinputid & " ORDER BY i.iupdatedate DESC, i.supdatetime DESC) cid GROUP BY icompoundinputid "
                                End If
                                firstTimeCompoundAddedAlert = True
                            End If

                        End If

                        executeTransactedSQLCommand(0, queriesInsert)

                    Else

                        Dim queriesInsert(3) As String

                        queriesInsert(0) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "CardInputs SELECT " & iprojectid & ", " & icardid & ", " & bi.iinputid & ", sinputunit, 1, " & fecha & ", '" & hora & "', '" & susername & "' FROM inputs WHERE iinputid = " & bi.iinputid

                        If isManoDeObra = True Then

                            busquedaMO = "" & _
                            "SELECT COUNT(*) " & _
                            "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "CardInputs ptfi " & _
                            "JOIN inputtypes it ON it.iinputid = ptfi.iinputid " & _
                            "WHERE ptfi.iprojectid = " & iprojectid & " AND ptfi.icardid = " & icardid & " " & _
                            "AND it.sinputtypedescription = 'MANO DE OBRA' "

                            conteoMO = getSQLQueryAsDouble(0, busquedaMO)

                            If conteoMO = 0 Then
                                queriesInsert(1) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "CardInputs SELECT " & iprojectid & ", " & icardid & ", 0, '%', 1, " & fecha & ", '" & hora & "', '" & susername & "'"
                            End If

                        End If

                        If isCompound = True Then

                            If getSQLQueryAsBoolean(0, "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "CardInputs WHERE iprojectid = " & iprojectid & " AND iinputid = " & bi.iinputid) = True Then
                                queriesInsert(2) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "CardCompoundInputs SELECT * FROM (SELECT " & iprojectid & " AS projectid, " & icardid & " AS cardid, " & bi.iinputid & " AS inputid, bcci.icompoundinputid, i.sinputunit, bcci.dcompoundinputqty AS qty, " & fecha & ", '" & hora & "', '" & susername & "' FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "CardInputs bci JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "CardCompoundInputs bcci ON bci.iprojectid = bcci.iprojectid AND bci.icardid = bcci.icardid AND bci.iinputid = bcci.iinputid JOIN inputs i ON i.iinputid = bcci.icompoundinputid WHERE bci.iinputid = " & bi.iinputid & " ORDER BY bci.iupdatedate DESC, bci.supdatetime DESC) cid GROUP BY icompoundinputid "
                            Else
                                If getSQLQueryAsBoolean(0, "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardInputs WHERE ibaseid = " & baseid & " AND iinputid = " & bi.iinputid) = True Then
                                    queriesInsert(2) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "CardCompoundInputs SELECT * FROM (SELECT " & iprojectid & " AS projectid, " & icardid & " AS cardid, " & bi.iinputid & " AS inputid, bcci.icompoundinputid, i.sinputunit, bcci.dcompoundinputqty AS qty, " & fecha & ", '" & hora & "', '" & susername & "' FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardInputs bci JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardCompoundInputs bcci ON bci.ibaseid = bcci.ibaseid AND bci.icardid = bcci.icardid AND bci.iinputid = bcci.iinputid JOIN inputs i ON i.iinputid = bcci.icompoundinputid WHERE bci.ibaseid = " & baseid & " AND bci.iinputid = " & bi.iinputid & " ORDER BY bci.iupdatedate DESC, bci.supdatetime DESC) cid GROUP BY icompoundinputid "
                                Else
                                    queriesInsert(2) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "CardCompoundInputs SELECT * FROM (SELECT " & iprojectid & " AS projectid, " & icardid & " AS cardid, " & bi.iinputid & " AS inputid, bcci.icompoundinputid, i.sinputunit, 1 AS qty, " & fecha & ", '" & hora & "', '" & susername & "' FROM inputs i WHERE i.iinputid = " & bi.iinputid & " ORDER BY i.iupdatedate DESC, i.supdatetime DESC) cid GROUP BY icompoundinputid "
                                End If
                                firstTimeCompoundAddedAlert = True
                            End If

                        End If

                        executeTransactedSQLCommand(0, queriesInsert)

                    End If

                End If

            Else

                Dim queriesInsert(6) As String

                If IsBase = True Then

                    queriesInsert(0) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardInputs SELECT " & baseid & ", " & icardid & ", " & bi.iinputid & ", sinputunit, 1, " & fecha & ", '" & hora & "', '" & susername & "' FROM inputs WHERE iinputid = " & bi.iinputid

                    If isManoDeObra = True Then

                        busquedaMO = "" & _
                        "SELECT COUNT(*) " & _
                        "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardInputs btfi " & _
                        "JOIN inputtypes it ON it.iinputid = btfi.iinputid " & _
                        "WHERE btfi.ibaseid = " & baseid & " AND btfi.icardid = " & icardid & " " & _
                        "AND it.sinputtypedescription = 'MANO DE OBRA' "

                        conteoMO = getSQLQueryAsDouble(0, busquedaMO)

                        If conteoMO = 0 Then
                            queriesInsert(1) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardInputs SELECT " & baseid & ", " & icardid & ", 0, '%', 1, " & fecha & ", '" & hora & "', '" & susername & "'"
                        End If

                    End If

                    If isCompound = True Then

                        If getSQLQueryAsBoolean(0, "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardInputs WHERE ibaseid = " & iprojectid & " AND iinputid = " & bi.iinputid) = True Then
                            queriesInsert(2) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardCompoundInputs SELECT * FROM (SELECT " & iprojectid & " AS projectid, " & icardid & " AS cardid, " & bi.iinputid & " AS inputid, bcci.icompoundinputid, i.sinputunit, bcci.dcompoundinputqty AS qty, " & fecha & ", '" & hora & "', '" & susername & "' FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardInputs bci JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardCompoundInputs bcci ON bci.ibaseid = bcci.ibaseid AND bci.icardid = bcci.icardid AND bci.iinputid = bcci.iinputid JOIN inputs i ON i.iinputid = bcci.icompoundinputid WHERE bci.iinputid = " & bi.iinputid & " ORDER BY bci.iupdatedate DESC, bci.supdatetime DESC) cid GROUP BY icompoundinputid "
                        Else
                            If getSQLQueryAsBoolean(0, "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardInputs WHERE ibaseid = " & baseid & " AND iinputid = " & bi.iinputid) = True Then
                                queriesInsert(2) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardCompoundInputs SELECT * FROM (SELECT " & iprojectid & " AS projectid, " & icardid & " AS cardid, " & bi.iinputid & " AS inputid, bcci.icompoundinputid, i.sinputunit, bcci.dcompoundinputqty AS qty, " & fecha & ", '" & hora & "', '" & susername & "' FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardInputs bci JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardCompoundInputs bcci ON bci.ibaseid = bcci.ibaseid AND bci.icardid = bcci.icardid AND bci.iinputid = bcci.iinputid JOIN inputs i ON i.iinputid = bcci.icompoundinputid WHERE bci.ibaseid = " & baseid & " AND bci.iinputid = " & bi.iinputid & " ORDER BY bci.iupdatedate DESC, bci.supdatetime DESC) cid GROUP BY icompoundinputid "
                            Else
                                queriesInsert(2) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardCompoundInputs SELECT * FROM (SELECT " & iprojectid & " AS projectid, " & icardid & " AS cardid, " & bi.iinputid & " AS inputid, bcci.icompoundinputid, i.sinputunit, 1 AS qty, " & fecha & ", '" & hora & "', '" & susername & "' FROM inputs i WHERE i.iinputid = " & bi.iinputid & " ORDER BY i.iupdatedate DESC, i.supdatetime DESC) cid GROUP BY icompoundinputid "
                            End If
                            firstTimeCompoundAddedAlert = True
                        End If

                    End If

                    executeTransactedSQLCommand(0, queriesInsert)

                ElseIf IsModel = True Then

                    queriesInsert(0) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardInputs SELECT " & baseid & ", " & icardid & ", " & bi.iinputid & ", sinputunit, 1, " & fecha & ", '" & hora & "', '" & susername & "' FROM inputs WHERE iinputid = " & bi.iinputid

                    If isManoDeObra = True Then

                        busquedaMO = "" & _
                        "SELECT COUNT(*) " & _
                        "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardInputs btfi " & _
                        "JOIN inputtypes it ON it.iinputid = btfi.iinputid " & _
                        "WHERE btfi.ibaseid = " & baseid & " AND btfi.icardid = " & icardid & " " & _
                        "AND it.sinputtypedescription = 'MANO DE OBRA' "

                        conteoMO = getSQLQueryAsDouble(0, busquedaMO)

                        If conteoMO = 0 Then
                            queriesInsert(1) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardInputs SELECT " & baseid & ", " & icardid & ", 0, '%', 1, " & fecha & ", '" & hora & "', '" & susername & "'"
                        End If

                    End If

                    If isCompound = True Then

                        If getSQLQueryAsBoolean(0, "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardInputs WHERE ibaseid = " & iprojectid & " AND iinputid = " & bi.iinputid) = True Then
                            queriesInsert(2) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardCompoundInputs SELECT * FROM (SELECT " & iprojectid & " AS projectid, " & icardid & " AS cardid, " & bi.iinputid & " AS inputid, bcci.icompoundinputid, i.sinputunit, bcci.dcompoundinputqty AS qty, " & fecha & ", '" & hora & "', '" & susername & "' FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardInputs bci JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardCompoundInputs bcci ON bci.ibaseid = bcci.ibaseid AND bci.icardid = bcci.icardid AND bci.iinputid = bcci.iinputid JOIN inputs i ON i.iinputid = bcci.icompoundinputid WHERE bci.iinputid = " & bi.iinputid & " ORDER BY bci.iupdatedate DESC, bci.supdatetime DESC) cid GROUP BY icompoundinputid "
                        Else
                            If getSQLQueryAsBoolean(0, "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardInputs WHERE ibaseid = " & baseid & " AND iinputid = " & bi.iinputid) = True Then
                                queriesInsert(2) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardCompoundInputs SELECT * FROM (SELECT " & iprojectid & " AS projectid, " & icardid & " AS cardid, " & bi.iinputid & " AS inputid, bcci.icompoundinputid, i.sinputunit, bcci.dcompoundinputqty AS qty, " & fecha & ", '" & hora & "', '" & susername & "' FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardInputs bci JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardCompoundInputs bcci ON bci.ibaseid = bcci.ibaseid AND bci.icardid = bcci.icardid AND bci.iinputid = bcci.iinputid JOIN inputs i ON i.iinputid = bcci.icompoundinputid WHERE bci.ibaseid = " & baseid & " AND bci.iinputid = " & bi.iinputid & " ORDER BY bci.iupdatedate DESC, bci.supdatetime DESC) cid GROUP BY icompoundinputid "
                            Else
                                queriesInsert(2) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardCompoundInputs SELECT * FROM (SELECT " & iprojectid & " AS projectid, " & icardid & " AS cardid, " & bi.iinputid & " AS inputid, bcci.icompoundinputid, i.sinputunit, 1 AS qty, " & fecha & ", '" & hora & "', '" & susername & "' FROM inputs i WHERE i.iinputid = " & bi.iinputid & " ORDER BY i.iupdatedate DESC, i.supdatetime DESC) cid GROUP BY icompoundinputid "
                            End If
                            firstTimeCompoundAddedAlert = True
                        End If

                    End If

                    queriesInsert(3) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "CardInputs SELECT " & iprojectid & ", " & icardid & ", " & bi.iinputid & ", sinputunit, 1, " & fecha & ", '" & hora & "', '" & susername & "' FROM inputs WHERE iinputid = " & bi.iinputid

                    If isManoDeObra = True Then

                        busquedaMO = "" & _
                        "SELECT COUNT(*) " & _
                        "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "CardInputs mtfi " & _
                        "JOIN inputtypes it ON it.iinputid = mtfi.iinputid " & _
                        "WHERE mtfi.imodelid = " & iprojectid & " AND mtfi.icardid = " & icardid & " " & _
                        "AND it.sinputtypedescription = 'MANO DE OBRA' "

                        conteoMO = getSQLQueryAsDouble(0, busquedaMO)

                        If conteoMO = 0 Then
                            queriesInsert(4) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "CardInputs SELECT " & iprojectid & ", " & icardid & ", 0, '%', 1, " & fecha & ", '" & hora & "', '" & susername & "'"
                        End If

                    End If

                    If isCompound = True Then

                        If getSQLQueryAsBoolean(0, "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "CardInputs WHERE imodelid = " & iprojectid & " AND iinputid = " & bi.iinputid) = True Then
                            queriesInsert(5) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "CardCompoundInputs SELECT * FROM (SELECT " & iprojectid & " AS projectid, " & icardid & " AS cardid, " & bi.iinputid & " AS inputid, bcci.icompoundinputid, i.sinputunit, bcci.dcompoundinputqty AS qty, " & fecha & ", '" & hora & "', '" & susername & "' FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "CardInputs bci JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "CardCompoundInputs bcci ON bci.imodelid = bcci.imodelid AND bci.icardid = bcci.icardid AND bci.iinputid = bcci.iinputid JOIN inputs i ON i.iinputid = bcci.icompoundinputid WHERE bci.iinputid = " & bi.iinputid & " ORDER BY bci.iupdatedate DESC, bci.supdatetime DESC) cid GROUP BY icompoundinputid "
                        Else
                            If getSQLQueryAsBoolean(0, "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardInputs WHERE ibaseid = " & baseid & " AND iinputid = " & bi.iinputid) = True Then
                                queriesInsert(5) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "CardCompoundInputs SELECT * FROM (SELECT " & iprojectid & " AS projectid, " & icardid & " AS cardid, " & bi.iinputid & " AS inputid, bcci.icompoundinputid, i.sinputunit, bcci.dcompoundinputqty AS qty, " & fecha & ", '" & hora & "', '" & susername & "' FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardInputs bci JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardCompoundInputs bcci ON bci.ibaseid = bcci.ibaseid AND bci.icardid = bcci.icardid AND bci.iinputid = bcci.iinputid JOIN inputs i ON i.iinputid = bcci.icompoundinputid WHERE bci.ibaseid = " & baseid & " AND bci.iinputid = " & bi.iinputid & " ORDER BY bci.iupdatedate DESC, bci.supdatetime DESC) cid GROUP BY icompoundinputid "
                            Else
                                queriesInsert(5) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "CardCompoundInputs SELECT * FROM (SELECT " & iprojectid & " AS projectid, " & icardid & " AS cardid, " & bi.iinputid & " AS inputid, bcci.icompoundinputid, i.sinputunit, 1 AS qty, " & fecha & ", '" & hora & "', '" & susername & "' FROM inputs i WHERE i.iinputid = " & bi.iinputid & " ORDER BY i.iupdatedate DESC, i.supdatetime DESC) cid GROUP BY icompoundinputid "
                            End If
                            firstTimeCompoundAddedAlert = True
                        End If

                    End If

                    executeTransactedSQLCommand(0, queriesInsert)

                Else

                    queriesInsert(0) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardInputs SELECT " & baseid & ", " & icardid & ", " & bi.iinputid & ", sinputunit, 1, " & fecha & ", '" & hora & "', '" & susername & "' FROM inputs WHERE iinputid = " & bi.iinputid

                    If isManoDeObra = True Then

                        busquedaMO = "" & _
                        "SELECT COUNT(*) " & _
                        "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardInputs btfi " & _
                        "JOIN inputtypes it ON it.iinputid = btfi.iinputid " & _
                        "WHERE btfi.ibaseid = " & baseid & " AND btfi.icardid = " & icardid & " " & _
                        "AND it.sinputtypedescription = 'MANO DE OBRA' "

                        conteoMO = getSQLQueryAsDouble(0, busquedaMO)

                        If conteoMO = 0 Then
                            queriesInsert(1) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardInputs SELECT " & baseid & ", " & icardid & ", 0, '%', 1, " & fecha & ", '" & hora & "', '" & susername & "'"
                        End If

                    End If

                    If isCompound = True Then

                        If getSQLQueryAsBoolean(0, "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardInputs WHERE ibaseid = " & iprojectid & " AND iinputid = " & bi.iinputid) = True Then
                            queriesInsert(2) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardCompoundInputs SELECT * FROM (SELECT " & iprojectid & " AS projectid, " & icardid & " AS cardid, " & bi.iinputid & " AS inputid, bcci.icompoundinputid, i.sinputunit, bcci.dcompoundinputqty AS qty, " & fecha & ", '" & hora & "', '" & susername & "' FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardInputs bci JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardCompoundInputs bcci ON bci.ibaseid = bcci.ibaseid AND bci.icardid = bcci.icardid AND bci.iinputid = bcci.iinputid JOIN inputs i ON i.iinputid = bcci.icompoundinputid WHERE bci.iinputid = " & bi.iinputid & " ORDER BY bci.iupdatedate DESC, bci.supdatetime DESC) cid GROUP BY icompoundinputid "
                        Else
                            If getSQLQueryAsBoolean(0, "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardInputs WHERE ibaseid = " & baseid & " AND iinputid = " & bi.iinputid) = True Then
                                queriesInsert(2) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardCompoundInputs SELECT * FROM (SELECT " & iprojectid & " AS projectid, " & icardid & " AS cardid, " & bi.iinputid & " AS inputid, bcci.icompoundinputid, i.sinputunit, bcci.dcompoundinputqty AS qty, " & fecha & ", '" & hora & "', '" & susername & "' FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardInputs bci JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardCompoundInputs bcci ON bci.ibaseid = bcci.ibaseid AND bci.icardid = bcci.icardid AND bci.iinputid = bcci.iinputid JOIN inputs i ON i.iinputid = bcci.icompoundinputid WHERE bci.ibaseid = " & baseid & " AND bci.iinputid = " & bi.iinputid & " ORDER BY bci.iupdatedate DESC, bci.supdatetime DESC) cid GROUP BY icompoundinputid "
                            Else
                                queriesInsert(2) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardCompoundInputs SELECT * FROM (SELECT " & iprojectid & " AS projectid, " & icardid & " AS cardid, " & bi.iinputid & " AS inputid, bcci.icompoundinputid, i.sinputunit, 1 AS qty, " & fecha & ", '" & hora & "', '" & susername & "' FROM inputs i WHERE i.iinputid = " & bi.iinputid & " ORDER BY i.iupdatedate DESC, i.supdatetime DESC) cid GROUP BY icompoundinputid "
                            End If
                            firstTimeCompoundAddedAlert = True
                        End If

                    End If

                    queriesInsert(3) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "CardInputs SELECT " & iprojectid & ", " & icardid & ", " & bi.iinputid & ", sinputunit, 1, " & fecha & ", '" & hora & "', '" & susername & "' FROM inputs WHERE iinputid = " & bi.iinputid

                    If isManoDeObra = True Then

                        busquedaMO = "" & _
                        "SELECT COUNT(*) " & _
                        "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "CardInputs ptfi " & _
                        "JOIN inputtypes it ON it.iinputid = ptfi.iinputid " & _
                        "WHERE ptfi.iprojectid = " & iprojectid & " AND ptfi.icardid = " & icardid & " " & _
                        "AND it.sinputtypedescription = 'MANO DE OBRA' "

                        conteoMO = getSQLQueryAsDouble(0, busquedaMO)

                        If conteoMO = 0 Then
                            queriesInsert(4) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "CardInputs SELECT " & iprojectid & ", " & icardid & ", 0, '%', 1, " & fecha & ", '" & hora & "', '" & susername & "'"
                        End If

                    End If

                    If isCompound = True Then

                        If getSQLQueryAsBoolean(0, "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "CardInputs WHERE iprojectid = " & iprojectid & " AND iinputid = " & bi.iinputid) = True Then
                            queriesInsert(5) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "CardCompoundInputs SELECT * FROM (SELECT " & iprojectid & " AS projectid, " & icardid & " AS cardid, " & bi.iinputid & " AS inputid, bcci.icompoundinputid, i.sinputunit, bcci.dcompoundinputqty AS qty, " & fecha & ", '" & hora & "', '" & susername & "' FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "CardInputs bci JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "CardCompoundInputs bcci ON bci.iprojectid = bcci.iprojectid AND bci.icardid = bcci.icardid AND bci.iinputid = bcci.iinputid JOIN inputs i ON i.iinputid = bcci.icompoundinputid WHERE bci.iinputid = " & bi.iinputid & " ORDER BY bci.iupdatedate DESC, bci.supdatetime DESC) cid GROUP BY icompoundinputid "
                        Else
                            If getSQLQueryAsBoolean(0, "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardInputs WHERE ibaseid = " & baseid & " AND iinputid = " & bi.iinputid) = True Then
                                queriesInsert(5) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "CardCompoundInputs SELECT * FROM (SELECT " & iprojectid & " AS projectid, " & icardid & " AS cardid, " & bi.iinputid & " AS inputid, bcci.icompoundinputid, i.sinputunit, bcci.dcompoundinputqty AS qty, " & fecha & ", '" & hora & "', '" & susername & "' FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardInputs bci JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardCompoundInputs bcci ON bci.ibaseid = bcci.ibaseid AND bci.icardid = bcci.icardid AND bci.iinputid = bcci.iinputid JOIN inputs i ON i.iinputid = bcci.icompoundinputid WHERE bci.ibaseid = " & baseid & " AND bci.iinputid = " & bi.iinputid & " ORDER BY bci.iupdatedate DESC, bci.supdatetime DESC) cid GROUP BY icompoundinputid "
                            Else
                                queriesInsert(5) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "CardCompoundInputs SELECT * FROM (SELECT " & iprojectid & " AS projectid, " & icardid & " AS cardid, " & bi.iinputid & " AS inputid, bcci.icompoundinputid, i.sinputunit, 1 AS qty, " & fecha & ", '" & hora & "', '" & susername & "' FROM inputs i WHERE i.iinputid = " & bi.iinputid & " ORDER BY i.iupdatedate DESC, i.supdatetime DESC) cid GROUP BY icompoundinputid "
                            End If
                            firstTimeCompoundAddedAlert = True
                        End If

                    End If

                    executeTransactedSQLCommand(0, queriesInsert)

                End If


            End If


            updateEverything()

            isConceptosTarjetaDGVReady = True

        Else

            updateEverything()

            isConceptosTarjetaDGVReady = True

        End If

        If firstTimeCompoundAddedAlert = True Then
            MsgBox("Como es la primera vez que insertas este Insumo Compuesto a este proyecto, te sugiero que revises sus rendimientos y cantidades", MsgBoxStyle.OkOnly Or MsgBoxStyle.Information, "Establecer rendimientos correctos")
        End If

        Cursor.Current = System.Windows.Forms.Cursors.Default

    End Sub


    Private Sub btnEliminarInsumo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEliminarInsumo.Click

        If deleteInputPermission = False Then
            Exit Sub
        End If

        Try

            If dgvConceptosTarjeta.CurrentRow.Index = -1 Then
                Exit Sub
            End If

        Catch ex As Exception

            iselectedinputid = 0
            sselectedinputdescription = ""
            sselectedunit = ""
            dselectedinputqty = 1.0
            Exit Sub

        End Try

        If MsgBox("¿Estás seguro de que quieres eliminar el Insumo " & sselectedinputdescription & " de la Tarjeta?", MsgBoxStyle.YesNo, "Confirmación de Eliminación de Insumo de la Tarjeta") = MsgBoxResult.Yes Then

            Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

            Dim tmpselectedinputid As Integer = 0
            Try
                tmpselectedinputid = CInt(dgvConceptosTarjeta.CurrentRow.Cells(0).Value)
            Catch ex As Exception

            End Try

            If tmpselectedinputid = 0 Then
                MsgBox("No es posible dejar una Tarjeta sin Equipo y Herramientas", MsgBoxStyle.OkOnly, "Equipo y Herramientas Requeridos")
                Exit Sub
            End If

            Dim conteoMO As Integer = 0
            Dim busquedaMO As String = ""
            Dim isManoDeObra As String = ""

            Dim queriesDelete(2) As String

            If IsBase = True Then

                isManoDeObra = "SELECT COUNT(*) FROM inputtypes WHERE iinputid = " & tmpselectedinputid & " AND it.sinputtypedescription = 'MANO DE OBRA' "

                If getSQLQueryAsInteger(0, isManoDeObra) > 0 Then

                    busquedaMO = "" & _
                    "SELECT COUNT(*) " & _
                    "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardInputs btfi " & _
                    "JOIN inputtypes it ON it.iinputid = btfi.iinputid " & _
                    "WHERE btfi.ibaseid = " & iprojectid & " AND btfi.icardid = " & icardid & " " & _
                    "AND it.sinputtypedescription = 'MANO DE OBRA' "

                    conteoMO = getSQLQueryAsDouble(0, busquedaMO)

                    If conteoMO = 0 Then
                        'Continue
                    ElseIf conteoMO > 1 Then
                        queriesDelete(0) = "DELETE FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardInputs WHERE ibaseid = " & iprojectid & " AND icardid = " & icardid & " AND iinputid = " & tmpselectedinputid
                    Else
                        MsgBox("No es posible dejar una Tarjeta sin Mano de Obra", MsgBoxStyle.OkOnly, "Mano de Obra Requerida")
                        Exit Sub
                    End If

                End If

                queriesDelete(0) = "DELETE FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardInputs WHERE ibaseid = " & iprojectid & " AND icardid = " & icardid & " AND iinputid = " & tmpselectedinputid

                Dim conteoCompound As Integer = 0

                conteoCompound = getSQLQueryAsInteger(0, "SELECT COUNT(*) FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardCompoundInputs WHERE iinputid = " & iselectedinputid)

                If conteoCompound > 0 Then
                    queriesDelete(1) = "DELETE FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardCompoundInputs WHERE ibaseid = " & iprojectid & " AND icardid = " & icardid & " AND iinputid = " & tmpselectedinputid
                End If

                executeTransactedSQLCommand(0, queriesDelete)

            Else

                If IsModel = True Then

                    isManoDeObra = "SELECT COUNT(*) FROM inputtypes WHERE iinputid = " & tmpselectedinputid & " AND it.sinputtypedescription = 'MANO DE OBRA' "

                    If getSQLQueryAsInteger(0, isManoDeObra) > 0 Then

                        busquedaMO = "" & _
                        "SELECT COUNT(*) " & _
                        "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "CardInputs mtfi " & _
                        "JOIN inputtypes it ON it.iinputid = mtfi.iinputid " & _
                        "WHERE mtfi.imodelid = " & iprojectid & " AND mtfi.icardid = " & icardid & " " & _
                        "AND it.sinputtypedescription = 'MANO DE OBRA' "

                        conteoMO = getSQLQueryAsDouble(0, busquedaMO)

                        If conteoMO = 0 Then
                            'Continue
                        ElseIf conteoMO > 1 Then
                            queriesDelete(0) = "DELETE FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "CardInputs WHERE imodelid = " & iprojectid & " AND icardid = " & icardid & " AND iinputid = " & tmpselectedinputid
                        Else
                            MsgBox("No es posible dejar una Tarjeta sin Mano de Obra", MsgBoxStyle.OkOnly, "Mano de Obra Requerida")
                            Exit Sub
                        End If

                    End If

                    queriesDelete(0) = "DELETE FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "CardInputs WHERE imodelid = " & iprojectid & " AND icardid = " & icardid & " AND iinputid = " & tmpselectedinputid

                    Dim conteoCompound As Integer = 0

                    conteoCompound = getSQLQueryAsInteger(0, "SELECT COUNT(*) FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardCompoundInputs WHERE iinputid = " & iselectedinputid)

                    If conteoCompound > 0 Then
                        queriesDelete(1) = "DELETE FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "CardCompoundInputs WHERE imodelid = " & iprojectid & " AND icardid = " & icardid & " AND iinputid = " & tmpselectedinputid
                    End If

                    executeTransactedSQLCommand(0, queriesDelete)

                Else

                    isManoDeObra = "SELECT COUNT(*) FROM inputtypes WHERE iinputid = " & tmpselectedinputid & " AND it.sinputtypedescription = 'MANO DE OBRA' "

                    If getSQLQueryAsInteger(0, isManoDeObra) > 0 Then

                        busquedaMO = "" & _
                        "SELECT COUNT(*) " & _
                        "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "CardInputs ptfi " & _
                        "JOIN inputtypes it ON it.iinputid = ptfi.iinputid " & _
                        "WHERE ptfi.iprojectid = " & iprojectid & " AND ptfi.icardid = " & icardid & " " & _
                        "AND it.sinputtypedescription = 'MANO DE OBRA' "

                        conteoMO = getSQLQueryAsDouble(0, busquedaMO)

                        If conteoMO = 0 Then
                            'Continue
                        ElseIf conteoMO > 1 Then
                            queriesDelete(0) = "DELETE FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "CardInputs WHERE iprojectid = " & iprojectid & " AND icardid = " & icardid & " AND iinputid = " & tmpselectedinputid
                        Else
                            MsgBox("No es posible dejar una Tarjeta sin Mano de Obra", MsgBoxStyle.OkOnly, "Mano de Obra Requerida")
                            Exit Sub
                        End If

                    End If

                    queriesDelete(0) = "DELETE FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "CardInputs WHERE iprojectid = " & iprojectid & " AND icardid = " & icardid & " AND iinputid = " & tmpselectedinputid

                    Dim conteoCompound As Integer = 0

                    conteoCompound = getSQLQueryAsInteger(0, "SELECT COUNT(*) FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardCompoundInputs WHERE iinputid = " & iselectedinputid)

                    If conteoCompound > 0 Then
                        queriesDelete(1) = "DELETE FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "CardCompoundInputs WHERE iprojectid = " & iprojectid & " AND icardid = " & icardid & " AND iinputid = " & tmpselectedinputid
                    End If

                    executeTransactedSQLCommand(0, queriesDelete)

                End If

            End If

            updateEverything()

            isConceptosTarjetaDGVReady = True

            Cursor.Current = System.Windows.Forms.Cursors.Default

        End If

    End Sub


    'Private Sub dgvPreciosHistoricosTarjeta_CellClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs)

    '    Try

    '        If dgvPreciosHistoricosTarjeta.CurrentRow Is Nothing Then
    '            Exit Sub
    '        End If

    '        ihistoricprojectid = CInt(dgvPreciosHistoricosTarjeta.Rows(e.RowIndex).Cells(0).Value())
    '        ihistoriccardid = CInt(dgvPreciosHistoricosTarjeta.Rows(e.RowIndex).Cells(1).Value())

    '    Catch ex As Exception

    '        ihistoricprojectid = 0
    '        ihistoriccardid = 0

    '    End Try

    'End Sub


    'Private Sub dgvPreciosHistoricosTarjeta_CellContentClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs)

    '    Try

    '        If dgvPreciosHistoricosTarjeta.CurrentRow Is Nothing Then
    '            Exit Sub
    '        End If

    '        ihistoricprojectid = CInt(dgvPreciosHistoricosTarjeta.Rows(e.RowIndex).Cells(0).Value())
    '        ihistoriccardid = CInt(dgvPreciosHistoricosTarjeta.Rows(e.RowIndex).Cells(1).Value())

    '    Catch ex As Exception

    '        ihistoricprojectid = 0
    '        ihistoriccardid = 0

    '    End Try

    'End Sub


    'Private Sub dgvPreciosHistoricosTarjeta_CellDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs)

    '    If IsHistoric = False Then

    '        Dim ag As New AgregarTarjeta
    '        ag.susername = susername
    '        ag.bactive = bactive
    '        ag.bonline = bonline
    '        ag.suserfullname = suserfullname

    '        ag.suseremail = suseremail
    '        ag.susersession = susersession
    '        ag.susermachinename = susermachinename
    '        ag.suserip = suserip

    '        ag.iprojectid = ihistoricprojectid
    '        ag.icardid = ihistoriccardid

    '        ag.IsEdit = True
    '        ag.IsHistoric = True
    '        ag.IsModel = IsModel
    '        ag.IsBase = IsBase

    'If Me.WindowState = FormWindowState.Maximized Then
    '        ag.WindowState = FormWindowState.Maximized
    '    End If

    '        Me.Visible = False
    '        ag.ShowDialog(Me)
    '        Me.Visible = True

    '    End If

    'End Sub


    'Private Sub dgvPreciosHistoricosTarjeta_CellContentDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs)

    '    If IsHistoric = False Then

    '        Dim ag As New AgregarTarjeta
    '        ag.susername = susername
    '        ag.bactive = bactive
    '        ag.bonline = bonline
    '        ag.suserfullname = suserfullname

    '        ag.suseremail = suseremail
    '        ag.susersession = susersession
    '        ag.susermachinename = susermachinename
    '        ag.suserip = suserip

    '        ag.iprojectid = ihistoricprojectid
    '        ag.icardid = ihistoriccardid

    '        ag.IsEdit = True
    '        ag.IsHistoric = True
    '        ag.IsModel = IsModel
    '        ag.IsBase = IsBase

    'If Me.WindowState = FormWindowState.Maximized Then
    '        ag.WindowState = FormWindowState.Maximized
    '    End If

    '        Me.Visible = False
    '        ag.ShowDialog(Me)
    '        Me.Visible = True

    '    End If

    'End Sub


    'Private Sub dgvConceptosTarjeta_DataError(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewDataErrorEventArgs) Handles dgvConceptosTarjeta.DataError

    '    Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

    '    Dim handle As Boolean
    '    Dim tmp As String

    '    Try
    '        tmp = dgvConceptosTarjeta.Rows(e.RowIndex).Cells(e.ColumnIndex).Value
    '    Catch ex As Exception

    '    End Try


    '    If dgvConceptosTarjeta.Rows(e.RowIndex).Cells(e.ColumnIndex).Value Is DBNull.Value Then

    '        dgvConceptosTarjeta.Rows(e.RowIndex).Cells(e.ColumnIndex).Value = ""
    '        handle = True

    '    Else

    '        handle = False

    '    End If

    '    e.Cancel = handle

    '    Cursor.Current = System.Windows.Forms.Cursors.Default

    'End Sub


    Private Function validaTarjeta(ByVal silent As Boolean, ByVal save As Boolean) As Boolean

        If IsEdit = False And icardid = 0 Then

            Dim baseid As Integer = 0
            baseid = getSQLQueryAsInteger(0, "SELECT ibaseid FROM base ORDER BY iupdatedate DESC, supdatetime DESC LIMIT 1")

            If baseid = 0 Then
                baseid = 99999
            End If

            icardid = getSQLQueryAsInteger(0, "SELECT IF(MAX(icardid) + 1 IS NULL, 1, MAX(icardid) + 1) AS icardid FROM basecards WHERE ibaseid = " & baseid)
            scardlegacyid = txtCodigoDeLaTarjeta.Text
            scarddescription = txtNombreDeLaTarjeta.Text

        End If

        If validaDatosInicialesTarjeta(silent, save) = True Then

            Dim queryCalculoTarjeta As String = ""
            Dim precioTarjeta As Double = 0.0

            If IsBase = True Then

                queryCalculoTarjeta = "" & _
                "SELECT (((((SUM(amountMO+amountMAT+amountEQ)+(SUM(amountMO+amountMAT+amountEQ)*btf.dcardindirectcostspercentage))*dcardgainpercentage)+(SUM(amountMO+amountMAT+amountEQ)+(SUM(amountMO+amountMAT+amountEQ)*btf.dcardindirectcostspercentage)))*b.dbaseIVApercentage)+(((SUM(amountMO+amountMAT+amountEQ)+(SUM(amountMO+amountMAT+amountEQ)*btf.dcardindirectcostspercentage))*dcardgainpercentage)+(SUM(amountMO+amountMAT+amountEQ)+(SUM(amountMO+amountMAT+amountEQ)*btf.dcardindirectcostspercentage)))) AS total " & _
                "FROM " & _
                " ( " & _
                "  SELECT ibaseid, icardid, IF(SUM(amountMAT) IS NULL, 0, SUM(amountMAT)) AS amountMAT, IF(SUM(amountMO) IS NULL, 0, SUM(amountMO)) AS amountMO, IF(SUM(amountEQ) IS NULL, 0, SUM(amountEQ)) AS amountEQ FROM " & _
                "  (SELECT " & iprojectid & " AS ibaseid, " & icardid & " AS icardid, 'SUMA DE MATERIALES', IF(SUM(btfi.dcardinputqty*bp.dinputfinalprice) IS NULL, 0, SUM(btfi.dcardinputqty*bp.dinputfinalprice))+IF(SUM(btfi.dcardinputqty*cibp.dinputfinalprice) IS NULL, 0, SUM(btfi.dcardinputqty*cibp.dinputfinalprice)) AS amountMAT, 0 AS amountMO, 0 AS amountEQ " & _
                "  FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardInputs btfi " & _
                "  JOIN inputs i ON btfi.iinputid = i.iinputid " & _
                "  JOIN inputtypes it ON it.iinputid = i.iinputid " & _
                "  LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) bp GROUP BY iinputid, ibaseid) bp ON btfi.ibaseid = bp.ibaseid AND i.iinputid = bp.iinputid " & _
                "  LEFT JOIN (SELECT btfci.ibaseid, btfci.iinputid, cibp.iupdatedate, cibp.supdatetime, SUM(btfci.dcompoundinputqty*cibp.dinputfinalprice) AS dinputpricewithoutIVA, 0.00000 AS dinputprotectionpercentage, SUM(btfci.dcompoundinputqty*cibp.dinputfinalprice) AS dinputfinalprice FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardCompoundInputs btfci JOIN inputs i" & icardid & " ON i" & icardid & ".iinputid = btfci.icompoundinputid LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) cibp GROUP BY iinputid, ibaseid) cibp ON cibp.ibaseid = btfci.ibaseid AND cibp.iinputid = btfci.icompoundinputid WHERE btfci.ibaseid = " & iprojectid & " AND btfci.icardid = " & icardid & " GROUP BY btfci.ibaseid, btfci.icardid, btfci.iinputid) cibp ON btfi.ibaseid = cibp.ibaseid AND i.iinputid = cibp.iinputid " & _
                "  WHERE btfi.ibaseid = " & iprojectid & " AND btfi.icardid =" & icardid & " AND it.sinputtypedescription = 'MATERIALES' " & _
                "  UNION " & _
                "  SELECT " & iprojectid & " AS ibaseid, " & icardid & " AS icardid, 'SUMA DE MANO DE OBRA', 0 AS amountMAT, IF(SUM(btfi.dcardinputqty*bp.dinputfinalprice) IS NULL, 0, SUM(btfi.dcardinputqty*bp.dinputfinalprice)) AS amountMO, 0 AS amountEQ " & _
                "  FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardInputs btfi " & _
                "  JOIN inputs i ON btfi.iinputid = i.iinputid " & _
                "  JOIN inputtypes it ON it.iinputid = i.iinputid " & _
                "  LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) bp GROUP BY iinputid, ibaseid) bp ON btfi.ibaseid = bp.ibaseid AND i.iinputid = bp.iinputid " & _
                "  WHERE btfi.ibaseid = " & iprojectid & " AND btfi.icardid =" & icardid & " AND it.sinputtypedescription = 'MANO DE OBRA' " & _
                "  UNION " & _
                "  SELECT " & iprojectid & " AS ibaseid, " & icardid & " AS icardid, 'SUMA DE EQUIPO Y HERRAMIENTA', 0 AS amountMAT, 0 AS amountMO, IF(SUM(btfi.dcardinputqty*eq.amount) IS NULL, 0, SUM(btfi.dcardinputqty*eq.amount)) AS amountEQ " & _
                "  FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardInputs btfi " & _
                "  JOIN (SELECT btfi.ibaseid,  btfi.icardid, 0 AS iinputid, SUM(btfi.dcardinputqty*bp.dinputfinalprice) AS amount FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardInputs btfi JOIN inputs i ON btfi.iinputid = i.iinputid JOIN inputtypes it ON i.iinputid = it.iinputid LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) bp GROUP BY iinputid, ibaseid) bp ON btfi.ibaseid = bp.ibaseid AND i.iinputid = bp.iinputid WHERE btfi.ibaseid = " & iprojectid & " AND btfi.icardid =" & icardid & " AND it.sinputtypedescription = 'MANO DE OBRA' GROUP BY btfi.ibaseid, btfi.iinputid) eq ON btfi.ibaseid = eq.ibaseid AND btfi.iinputid = eq.iinputid " & _
                "  WHERE btfi.ibaseid = " & iprojectid & " AND btfi.icardid =" & icardid & " AND btfi.iinputid = 0 " & _
                "  ) AS costodirectoinner " & _
                "  GROUP BY 1,2 " & _
                " ) AS costodirectoouter " & _
                "JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Cards btf ON costodirectoouter.ibaseid = btf.ibaseid AND costodirectoouter.icardid = btf.icardid " & _
                "JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & " b ON btf.ibaseid = b.ibaseid " & _
                "WHERE b.ibaseid = " & iprojectid & " "

            Else

                If IsModel = True Then

                    queryCalculoTarjeta = "" & _
                    "SELECT (((((SUM(amountMO+amountMAT+amountEQ)+(SUM(amountMO+amountMAT+amountEQ)*mtf.dcardindirectcostspercentage))*dcardgainpercentage)+(SUM(amountMO+amountMAT+amountEQ)+(SUM(amountMO+amountMAT+amountEQ)*mtf.dcardindirectcostspercentage)))*m.dmodelIVApercentage)+(((SUM(amountMO+amountMAT+amountEQ)+(SUM(amountMO+amountMAT+amountEQ)*mtf.dcardindirectcostspercentage))*dcardgainpercentage)+(SUM(amountMO+amountMAT+amountEQ)+(SUM(amountMO+amountMAT+amountEQ)*mtf.dcardindirectcostspercentage)))) AS total " & _
                    "FROM " & _
                    " ( " & _
                    "  SELECT imodelid, icardid, IF(SUM(amountMAT) IS NULL, 0, SUM(amountMAT)) AS amountMAT, IF(SUM(amountMO) IS NULL, 0, SUM(amountMO)) AS amountMO, IF(SUM(amountEQ) IS NULL, 0, SUM(amountEQ)) AS amountEQ FROM " & _
                    "  (SELECT " & iprojectid & " AS imodelid, " & icardid & " AS icardid, 'SUMA DE MATERIALES', IF(SUM(mtfi.dcardinputqty*mp.dinputfinalprice) IS NULL, 0, SUM(mtfi.dcardinputqty*mp.dinputfinalprice))+IF(SUM(mtfi.dcardinputqty*cimp.dinputfinalprice) IS NULL, 0, SUM(mtfi.dcardinputqty*cimp.dinputfinalprice)) AS amountMAT, 0 AS amountMO, 0 AS amountEQ " & _
                    "  FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "CardInputs mtfi " & _
                    "  JOIN inputs i ON mtfi.iinputid = i.iinputid " & _
                    "  JOIN inputtypes it ON it.iinputid = i.iinputid " & _
                    "  LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) mp GROUP BY iinputid, imodelid) mp ON mtfi.imodelid = mp.imodelid AND i.iinputid = mp.iinputid " & _
                    "  LEFT JOIN (SELECT mtfci.imodelid, mtfci.iinputid, cimp.iupdatedate, cimp.supdatetime, SUM(mtfci.dcompoundinputqty*cimp.dinputfinalprice) AS dinputpricewithoutIVA, 0.00000 AS dinputprotectionpercentage, SUM(mtfci.dcompoundinputqty*cimp.dinputfinalprice) AS dinputfinalprice FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "CardCompoundInputs mtfci JOIN inputs i" & icardid & " ON i" & icardid & ".iinputid = mtfci.icompoundinputid LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) cimp GROUP BY iinputid, imodelid) cimp ON cimp.imodelid = mtfci.imodelid AND cimp.iinputid = mtfci.icompoundinputid WHERE mtfci.imodelid = " & iprojectid & " AND mtfci.icardid = " & icardid & " GROUP BY mtfci.imodelid, mtfci.icardid, mtfci.iinputid) cimp ON mtfi.imodelid = cimp.imodelid AND i.iinputid = cimp.iinputid " & _
                    "  WHERE mtfi.imodelid = " & iprojectid & " AND mtfi.icardid =" & icardid & " AND it.sinputtypedescription = 'MATERIALES' " & _
                    "  UNION " & _
                    "  SELECT " & iprojectid & " AS imodelid, " & icardid & " AS icardid, 'SUMA DE MANO DE OBRA', 0 AS amountMAT, IF(SUM(mtfi.dcardinputqty*mp.dinputfinalprice) IS NULL, 0, SUM(mtfi.dcardinputqty*mp.dinputfinalprice)) AS amountMO, 0 AS amountEQ " & _
                    "  FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "CardInputs mtfi " & _
                    "  JOIN inputs i ON mtfi.iinputid = i.iinputid " & _
                    "  JOIN inputtypes it ON it.iinputid = i.iinputid " & _
                    "  LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) mp GROUP BY iinputid, imodelid) mp ON mtfi.imodelid = mp.imodelid AND i.iinputid = mp.iinputid " & _
                    "  WHERE mtfi.imodelid = " & iprojectid & " AND mtfi.icardid =" & icardid & " AND it.sinputtypedescription = 'MANO DE OBRA' " & _
                    "  UNION " & _
                    "  SELECT " & iprojectid & " AS imodelid, " & icardid & " AS icardid, 'SUMA DE EQUIPO Y HERRAMIENTA', 0 AS amountMAT, 0 AS amountMO, IF(SUM(mtfi.dcardinputqty*eq.amount) IS NULL, 0, SUM(mtfi.dcardinputqty*eq.amount)) AS amountEQ " & _
                    "  FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "CardInputs mtfi " & _
                    "  JOIN (SELECT mtfi.imodelid,  mtfi.icardid, 0 AS iinputid, SUM(mtfi.dcardinputqty*mp.dinputfinalprice) AS amount FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "CardInputs mtfi JOIN inputs i ON mtfi.iinputid = i.iinputid JOIN inputtypes it ON i.iinputid = it.iinputid LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) mp GROUP BY iinputid, imodelid) mp ON mtfi.imodelid = mp.imodelid AND i.iinputid = mp.iinputid WHERE mtfi.imodelid = " & iprojectid & " AND mtfi.icardid =" & icardid & " AND it.sinputtypedescription = 'MANO DE OBRA' GROUP BY mtfi.imodelid, mtfi.iinputid) eq ON mtfi.imodelid = eq.imodelid AND mtfi.iinputid = eq.iinputid " & _
                    "  WHERE mtfi.imodelid = " & iprojectid & " AND mtfi.icardid =" & icardid & " AND mtfi.iinputid = 0 " & _
                    "  ) AS costodirectoinner " & _
                    "  GROUP BY 1,2 " & _
                    " ) AS costodirectoouter " & _
                    "JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "Cards mtf ON costodirectoouter.imodelid = mtf.imodelid AND costodirectoouter.icardid = mtf.icardid " & _
                    "JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & " m ON mtf.imodelid = m.imodelid " & _
                    "WHERE m.imodelid = " & iprojectid & " "

                Else

                    queryCalculoTarjeta = "" & _
                    "SELECT (((((SUM(amountMO+amountMAT+amountEQ)+(SUM(amountMO+amountMAT+amountEQ)*ptf.dcardindirectcostspercentage))*dcardgainpercentage)+(SUM(amountMO+amountMAT+amountEQ)+(SUM(amountMO+amountMAT+amountEQ)*ptf.dcardindirectcostspercentage)))*p.dprojectIVApercentage)+(((SUM(amountMO+amountMAT+amountEQ)+(SUM(amountMO+amountMAT+amountEQ)*ptf.dcardindirectcostspercentage))*dcardgainpercentage)+(SUM(amountMO+amountMAT+amountEQ)+(SUM(amountMO+amountMAT+amountEQ)*ptf.dcardindirectcostspercentage)))) AS total " & _
                    "FROM " & _
                    " ( " & _
                    "  SELECT iprojectid, icardid, IF(SUM(amountMAT) IS NULL, 0, SUM(amountMAT)) AS amountMAT, IF(SUM(amountMO) IS NULL, 0, SUM(amountMO)) AS amountMO, IF(SUM(amountEQ) IS NULL, 0, SUM(amountEQ)) AS amountEQ FROM " & _
                    "  (SELECT " & iprojectid & " AS iprojectid, " & icardid & " AS icardid, 'SUMA DE MATERIALES', IF(SUM(ptfi.dcardinputqty*pp.dinputfinalprice) IS NULL, 0, SUM(ptfi.dcardinputqty*pp.dinputfinalprice))+IF(SUM(ptfi.dcardinputqty*cipp.dinputfinalprice) IS NULL, 0, SUM(ptfi.dcardinputqty*cipp.dinputfinalprice)) AS amountMAT, 0 AS amountMO, 0 AS amountEQ " & _
                    "  FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "CardInputs ptfi " & _
                    "  JOIN inputs i ON ptfi.iinputid = i.iinputid " & _
                    "  JOIN inputtypes it ON it.iinputid = i.iinputid " & _
                    "  LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) pp GROUP BY iinputid, iprojectid) pp ON ptfi.iprojectid = pp.iprojectid AND i.iinputid = pp.iinputid " & _
                    "  LEFT JOIN (SELECT ptfci.iprojectid, ptfci.iinputid, cipp.iupdatedate, cipp.supdatetime, SUM(ptfci.dcompoundinputqty*cipp.dinputfinalprice) AS dinputpricewithoutIVA, 0.00000 AS dinputprotectionpercentage, SUM(ptfci.dcompoundinputqty*cipp.dinputfinalprice) AS dinputfinalprice FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "CardCompoundInputs ptfci JOIN inputs i" & icardid & " ON i" & icardid & ".iinputid = ptfci.icompoundinputid LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) cipp GROUP BY iinputid, iprojectid) cipp ON cipp.iprojectid = ptfci.iprojectid AND cipp.iinputid = ptfci.icompoundinputid WHERE ptfci.iprojectid = " & iprojectid & " AND ptfci.icardid = " & icardid & " GROUP BY ptfci.iprojectid, ptfci.icardid, ptfci.iinputid) cipp ON ptfi.iprojectid = cipp.iprojectid AND i.iinputid = cipp.iinputid " & _
                    "  WHERE ptfi.iprojectid = " & iprojectid & " AND ptfi.icardid = " & icardid & " AND it.sinputtypedescription = 'MATERIALES' " & _
                    "  UNION " & _
                    "  SELECT " & iprojectid & " AS iprojectid, " & icardid & " AS icardid, 'SUMA DE MANO DE OBRA', 0 AS amountMAT, IF(SUM(ptfi.dcardinputqty*pp.dinputfinalprice) IS NULL, 0, SUM(ptfi.dcardinputqty*pp.dinputfinalprice)) AS amountMO, 0 AS amountEQ " & _
                    "  FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "CardInputs ptfi " & _
                    "  JOIN inputs i ON ptfi.iinputid = i.iinputid " & _
                    "  JOIN inputtypes it ON it.iinputid = i.iinputid " & _
                    "  LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) pp GROUP BY iinputid, iprojectid) pp ON ptfi.iprojectid = pp.iprojectid AND i.iinputid = pp.iinputid " & _
                    "  WHERE ptfi.iprojectid = " & iprojectid & " AND ptfi.icardid = " & icardid & " AND it.sinputtypedescription = 'MANO DE OBRA' " & _
                    "  UNION " & _
                    "  SELECT " & iprojectid & " AS iprojectid, " & icardid & " AS icardid, 'SUMA DE EQUIPO Y HERRAMIENTA', 0 AS amountMAT, 0 AS amountMO, IF(SUM(ptfi.dcardinputqty*eq.amount) IS NULL, 0, SUM(ptfi.dcardinputqty*eq.amount)) AS amountEQ " & _
                    "  FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "CardInputs ptfi " & _
                    "  JOIN (SELECT ptfi.iprojectid,  ptfi.icardid, 0 AS iinputid, SUM(ptfi.dcardinputqty*pp.dinputfinalprice) AS amount FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "CardInputs ptfi JOIN inputs i ON ptfi.iinputid = i.iinputid JOIN inputtypes it ON i.iinputid = it.iinputid LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) pp GROUP BY iinputid, iprojectid) pp ON ptfi.iprojectid = pp.iprojectid AND i.iinputid = pp.iinputid WHERE ptfi.iprojectid = " & iprojectid & " AND ptfi.icardid = " & icardid & " AND it.sinputtypedescription = 'MANO DE OBRA' GROUP BY ptfi.iprojectid, ptfi.iinputid) eq ON ptfi.iprojectid = eq.iprojectid AND ptfi.iinputid = eq.iinputid " & _
                    "  WHERE ptfi.iprojectid = " & iprojectid & " AND ptfi.icardid = " & icardid & " AND ptfi.iinputid = 0 " & _
                    "  ) AS costodirectoinner " & _
                    "  GROUP BY 1,2 " & _
                    " ) AS costodirectoouter " & _
                    "JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "Cards ptf ON costodirectoouter.iprojectid = ptf.iprojectid AND costodirectoouter.icardid = ptf.icardid " & _
                    "JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & " p ON ptf.iprojectid = p.iprojectid " & _
                    "WHERE p.iprojectid = " & iprojectid & " "

                End If

            End If


            precioTarjeta = getSQLQueryAsDouble(0, queryCalculoTarjeta)

            If precioTarjeta = 0 Then
                If silent = False Then
                    MsgBox("¿Podrías poner algún concepto para la Tarjeta?", MsgBoxStyle.OkOnly, "Dato Faltante")
                End If
                Return False
            End If


            Dim conteoMO As Integer = 0
            Dim conteoEQ As Integer = 0

            Dim busquedaMO As String = ""
            Dim busquedaEQ As String = ""

            If IsBase = True Then

                busquedaMO = "" & _
                "SELECT COUNT(*) " & _
                "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardInputs btfi " & _
                "JOIN inputtypes it ON it.iinputid = btfi.iinputid " & _
                "WHERE btfi.ibaseid = " & iprojectid & " AND btfi.icardid = " & icardid & " " & _
                "AND it.sinputtypedescription = 'MANO DE OBRA' "

                busquedaEQ = "" & _
                "SELECT COUNT(*) " & _
                "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardInputs btfi " & _
                "JOIN inputtypes it ON it.iinputid = btfi.iinputid " & _
                "WHERE btfi.ibaseid = " & iprojectid & " AND btfi.icardid = " & icardid & " " & _
                "AND it.sinputtypedescription = 'EQUIPO Y HERRAMIENTA' "

            Else

                If IsModel = True Then

                    busquedaMO = "" & _
                    "SELECT COUNT(*) " & _
                    "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "CardInputs mtfi " & _
                    "JOIN inputtypes it ON it.iinputid = mtfi.iinputid " & _
                    "WHERE mtfi.imodelid = " & iprojectid & " AND mtfi.icardid = " & icardid & " " & _
                    "AND it.sinputtypedescription = 'MANO DE OBRA' "

                    busquedaEQ = "" & _
                    "SELECT COUNT(*) " & _
                    "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "CardInputs mtfi " & _
                    "JOIN inputtypes it ON it.iinputid = mtfi.iinputid " & _
                    "WHERE mtfi.imodelid = " & iprojectid & " AND mtfi.icardid = " & icardid & " " & _
                    "AND it.sinputtypedescription = 'EQUIPO Y HERRAMIENTA' "

                Else

                    busquedaMO = "" & _
                    "SELECT COUNT(*) " & _
                    "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "CardInputs ptfi " & _
                    "JOIN inputtypes it ON it.iinputid = ptfi.iinputid " & _
                    "WHERE ptfi.iprojectid = " & iprojectid & " AND ptfi.icardid = " & icardid & " " & _
                    "AND it.sinputtypedescription = 'MANO DE OBRA' "

                    busquedaEQ = "" & _
                    "SELECT COUNT(*) " & _
                    "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "CardInputs ptfi " & _
                    "JOIN inputtypes it ON it.iinputid = ptfi.iinputid " & _
                    "WHERE ptfi.iprojectid = " & iprojectid & " AND ptfi.icardid = " & icardid & " " & _
                    "AND it.sinputtypedescription = 'EQUIPO Y HERRAMIENTA' "

                End If

            End If

            conteoMO = getSQLQueryAsDouble(0, busquedaMO)
            conteoEQ = getSQLQueryAsDouble(0, busquedaEQ)

            If conteoMO < 1 Then
                If silent = False Then
                    MsgBox("No es posible dejar una Tarjeta sin Mano de Obra", MsgBoxStyle.OkOnly, "Mano de Obra Requerida")
                End If
                Return False
            End If

            If conteoEQ < 1 Then
                If silent = False Then
                    MsgBox("No es posible dejar una Tarjeta sin Equipo y Herramientas", MsgBoxStyle.OkOnly, "Equipo y Herramientas Requeridos")
                End If
                Return False
            End If

            Return True

        Else

            Return False

        End If

    End Function


    Private Sub btnCancelar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancelar.Click

        Me.DialogResult = Windows.Forms.DialogResult.Cancel
        Me.Close()

    End Sub


    'Private Sub btnGuardar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnGuardar.Click

    '    Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

    '    If validaTarjeta(False, True) = False Then
    '        Exit Sub
    '    End If

    '    Dim baseid As Integer = 0
    '    baseid = getSQLQueryAsInteger(0, "SELECT ibaseid FROM base ORDER BY iupdatedate DESC, supdatetime DESC LIMIT 1")

    '    If baseid = 0 Then
    '        baseid = 99999
    '    End If

    '    Dim valorIndirectos As Double = 0.0
    '    Try
    '        valorIndirectos = CDbl(txtPorcentajeIndirectos.Text)
    '    Catch ex As Exception

    '    End Try

    '    Dim valorUtilidades As Double = 0.0
    '    Try
    '        valorUtilidades = CDbl(txtPorcentajeUtilidades.Text)
    '    Catch ex As Exception

    '    End Try

    '    Dim fecha As Integer = 0
    '    Dim hora As String = ""

    '    fecha = getMySQLDate()
    '    hora = getAppTime()

    '    If IsEdit = True Then

    '        If IsBase = True Then

    '            executeSQLCommand(0, "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Cards SET iupdatedate = " & fecha & ", supdatetime = '" & hora & "', supdateusername = '" & susername & "', scardlegacycategoryid = '" & cmbCategoriaDeLaTarjeta.SelectedValue.ToString & "', scardlegacyid = '" & txtCodigoDeLaTarjeta.Text & "', scarddescription = '" & txtNombreDeLaTarjeta.Text & "', scardunit = '" & txtUnidadDeMedida.Text & "', dcardindirectcostspercentage = " & (valorIndirectos / 100) & ", dcardgainpercentage = " & (valorUtilidades / 100) & " WHERE ibaseid = " & iprojectid & " AND icardid = " & icardid)

    '        Else

    '            If IsModel = True Then

    '                executeSQLCommand(0, "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "Cards SET iupdatedate = " & fecha & ", supdatetime = '" & hora & "', supdateusername = '" & susername & "', scardlegacycategoryid = '" & cmbCategoriaDeLaTarjeta.SelectedValue.ToString & "', scardlegacyid = '" & txtCodigoDeLaTarjeta.Text & "', scarddescription = '" & txtNombreDeLaTarjeta.Text & "', scardunit = '" & txtUnidadDeMedida.Text & "', dcardindirectcostspercentage = " & (valorIndirectos / 100) & ", dcardgainpercentage = " & (valorUtilidades / 100) & " WHERE imodelid = " & iprojectid & " AND icardid = " & icardid)

    '            Else

    '                Dim queries(9) As String

    '                queries(0) = "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "Cards SET iupdatedate = " & fecha & ", supdatetime = '" & hora & "', supdateusername = '" & susername & "', scardlegacycategoryid = '" & cmbCategoriaDeLaTarjeta.SelectedValue.ToString & "', scardlegacyid = '" & txtCodigoDeLaTarjeta.Text & "', scarddescription = '" & txtNombreDeLaTarjeta.Text & "', scardunit = '" & txtUnidadDeMedida.Text & "', dcardindirectcostspercentage = " & (valorIndirectos / 100) & ", dcardgainpercentage = " & (valorUtilidades / 100) & " WHERE iprojectid = " & iprojectid & " AND icardid = " & icardid

    '                queries(1) = "" & _
    '                "DELETE " & _
    '                "FROM projectcardcompoundinputs " & _
    '                "WHERE iprojectid = " & iprojectid & " AND " & _
    '                "NOT EXISTS (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "CardCompoundInputs) "

    '                queries(2) = "" & _
    '                "DELETE " & _
    '                "FROM projectcardinputs " & _
    '                "WHERE iprojectid = " & iprojectid & " AND " & _
    '                "NOT EXISTS (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "CardInputs) "

    '                queries(3) = "" & _
    '                "UPDATE pp SET pp.iupdatedate = tpp.iupdatedate, pp.supdatetime = tpp.supdatetime, pp.supdateusername = tpp.supdateusername, pp.dinputpricewithoutIVA = tpp.dinputpricewithoutIVA, pp.dinputprotectionpercentage = tpp.dinputprotectionpercentage, pp.dinputfinalprice = tpp.dinputfinalprice FROM projectprices pp JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "Prices tpp ON tpp.iprojectid = pp.iprojectid AND tpp.iinputid = pp.iinputid AND tpp.iupdatedate = pp.iupdatedate AND tpp.supdatetime = pp.supdatetime "

    '                queries(4) = "" & _
    '                "UPDATE ptci SET ptci.iupdatedate = tptci.iupdatedate, ptci.supdatetime = tptci.supdatetime, ptci.supdateusername = tptci.supdateusername, ptci.scompoundinputunit = tptci.scompoundinputunit, ptci.dcompoundinputqty = tptci.dcompoundinputqty FROM projectcardcompoundinputs ptci JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "CardCompoundInputs tptci ON tptci.iprojectid = ptci.iprojectid AND tptci.icardid = ptci.icardid AND tptci.icompoundinputid = ptci.icompoundinputid WHERE STR_TO_DATE(CONCAT(tptci.iupdatedate, ' ', tptci.supdatetime), '%Y%c%d %T') > STR_TO_DATE(CONCAT(ptci.iupdatedate, ' ', ptci.supdatetime), '%Y%c%d %T') "

    '                queries(5) = "" & _
    '                "UPDATE pti SET pti.iupdatedate = tpti.iupdatedate, pti.supdatetime = tpti.supdatetime, pti.supdateusername = tpti.supdateusername, pti.scardinputunit = tpti.scardinputunit, pti.dcardinputqty = tpti.dcardinputqty FROM projectcardinputs pti JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "CardInputs tpti ON tpti.iprojectid = pti.iprojectid AND tpti.icardid = pti.icardid AND tpti.iinputid = pti.iinputid WHERE STR_TO_DATE(CONCAT(tpti.iupdatedate, ' ', tpti.supdatetime), '%Y%c%d %T') > STR_TO_DATE(CONCAT(pti.iupdatedate, ' ', pti.supdatetime), '%Y%c%d %T') "

    '                queries(6) = "" & _
    '                "INSERT INTO projectprices " & _
    '                "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "Prices " & _
    '                "WHERE NOT EXISTS (SELECT * FROM projectprices) "

    '                queries(7) = "" & _
    '                "INSERT INTO projectcardcompoundinputs " & _
    '                "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "CardCompoundInputs " & _
    '                "WHERE NOT EXISTS (SELECT * FROM projectcardcompoundinputs) "

    '                queries(8) = "" & _
    '                "INSERT INTO projectcardinputs " & _
    '                "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "CardInputs " & _
    '                "WHERE NOT EXISTS (SELECT * FROM projectcardinputs) "

    '                executeTransactedSQLCommand(0, queries)


    '            End If

    '        End If

    '    Else

    '        Dim checkIfItsOnlyTextUpdate As Boolean = False

    '        checkIfItsOnlyTextUpdate = getSQLQueryAsBoolean(0, "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Cards WHERE ibaseid = " & baseid & " AND icardid = " & icardid)

    '        If checkIfItsOnlyTextUpdate = True Then

    '            Dim queriesUpdate(2) As String

    '            queriesUpdate(0) = "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Cards SET iupdatedate = " & getMySQLDate() & ", supdatetime = '" & getAppTime() & "', supdateusername = '" & susername & "', scardlegacycategoryid = '" & cmbCategoriaDeLaTarjeta.SelectedValue.ToString & "', scardlegacyid = '" & txtCodigoDeLaTarjeta.Text & "', scarddescription = '" & txtNombreDeLaTarjeta.Text & "', scardunit = '" & txtUnidadDeMedida.Text & "', dcardindirectcostspercentage = " & (valorIndirectos / 100) & ", dcardgainpercentage = " & (valorUtilidades / 100) & " WHERE ibaseid = " & baseid & " AND icardid = " & icardid

    '            If IsModel = True Then

    '                queriesUpdate(1) = "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "Cards SET iupdatedate = " & getMySQLDate() & ", supdatetime = '" & getAppTime() & "', supdateusername = '" & susername & "', scardlegacycategoryid = '" & cmbCategoriaDeLaTarjeta.SelectedValue.ToString & "', scardlegacyid = '" & txtCodigoDeLaTarjeta.Text & "', scarddescription = '" & txtNombreDeLaTarjeta.Text & "', scardunit = '" & txtUnidadDeMedida.Text & "', dcardindirectcostspercentage = " & (valorIndirectos / 100) & ", dcardgainpercentage = " & (valorUtilidades / 100) & " WHERE imodelid = " & iprojectid & " AND icardid = " & icardid

    '            Else

    '                queriesUpdate(1) = "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "Cards SET iupdatedate = " & getMySQLDate() & ", supdatetime = '" & getAppTime() & "', supdateusername = '" & susername & "', scardlegacycategoryid = '" & cmbCategoriaDeLaTarjeta.SelectedValue.ToString & "', scardlegacyid = '" & txtCodigoDeLaTarjeta.Text & "', scarddescription = '" & txtNombreDeLaTarjeta.Text & "', scardunit = '" & txtUnidadDeMedida.Text & "', dcardindirectcostspercentage = " & (valorIndirectos / 100) & ", dcardgainpercentage = " & (valorUtilidades / 100) & " WHERE iprojectid = " & iprojectid & " AND icardid = " & icardid

    '            End If

    '            executeTransactedSQLCommand(0, queriesUpdate)

    '        Else

    '            icardid = getSQLQueryAsInteger(0, "SELECT IF(MAX(icardid) + 1 IS NULL, 1, MAX(icardid) + 1) AS icardid FROM basecards WHERE ibaseid = " & baseid)

    '            scardlegacyid = txtCodigoDeLaTarjeta.Text
    '            scarddescription = txtNombreDeLaTarjeta.Text

    '            Dim queries(11) As String

    '            queries(0) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Cards VALUES (" & baseid & ", " & icardid & ", '" & cmbCategoriaDeLaTarjeta.SelectedValue & "', '" & txtCodigoDeLaTarjeta.Text & "', '" & txtNombreDeLaTarjeta.Text & "', '" & txtUnidadDeMedida.Text & "', 1, " & (valorIndirectos / 100) & ", " & (valorUtilidades / 100) & ", " & fecha & ", '" & hora & "', '" & susername & "')"

    '            queries(1) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "Cards VALUES (" & iprojectid & ", " & icardid & ", " & getMySQLDate() & ", '" & getAppTime() & "', '" & cmbCategoriaDeLaTarjeta.SelectedValue & "', '" & txtCodigoDeLaTarjeta.Text & "', '" & txtNombreDeLaTarjeta.Text & "', '" & txtUnidadDeMedida.Text & "', 1, " & (valorIndirectos / 100) & ", " & (valorUtilidades / 100) & ", " & fecha & ", '" & hora & "', '" & susername & "')"

    '            queries(2) = "" & _
    '            "DELETE " & _
    '            "FROM projectcardcompoundinputs " & _
    '            "WHERE iprojectid = " & iprojectid & " AND " & _
    '            "NOT EXISTS (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "CardCompoundInputs) "

    '            queries(3) = "" & _
    '            "DELETE " & _
    '            "FROM projectcardinputs " & _
    '            "WHERE iprojectid = " & iprojectid & " AND " & _
    '            "NOT EXISTS (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "CardInputs) "

    '            queries(4) = "" & _
    '            'ERROR: PRIMERO TABLAS LUEGO SET
    '            "UPDATE pp SET pp.iupdatedate = tpp.iupdatedate, pp.supdatetime = tpp.supdatetime, pp.supdateusername = tpp.supdateusername, pp.dinputpricewithoutIVA = tpp.dinputpricewithoutIVA, pp.dinputprotectionpercentage = tpp.dinputprotectionpercentage, pp.dinputfinalprice = tpp.dinputfinalprice FROM projectprices pp JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "Prices tpp ON tpp.iprojectid = pp.iprojectid AND tpp.iinputid = pp.iinputid AND tpp.iupdatedate = pp.iupdatedate AND tpp.supdatetime = pp.supdatetime "

    '            queries(5) = "" & _
    '            'ERROR: PRIMERO TABLAS LUEGO SET
    '            "UPDATE ptci SET ptci.iupdatedate = tptci.iupdatedate, ptci.supdatetime = tptci.supdatetime, ptci.supdateusername = tptci.supdateusername, ptci.scompoundinputunit = tptci.scompoundinputunit, ptci.dcompoundinputqty = tptci.dcompoundinputqty FROM projectcardcompoundinputs ptci JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "CardCompoundInputs tptci ON tptci.iprojectid = ptci.iprojectid AND tptci.icardid = ptci.icardid AND tptci.icompoundinputid = ptci.icompoundinputid WHERE STR_TO_DATE(CONCAT(tptci.iupdatedate, ' ', tptci.supdatetime), '%Y%c%d %T') > STR_TO_DATE(CONCAT(ptci.iupdatedate, ' ', ptci.supdatetime), '%Y%c%d %T') "

    '            queries(6) = "" & _
    '            'ERROR: PRIMERO TABLAS LUEGO SET
    '            "UPDATE pti SET pti.iupdatedate = tpti.iupdatedate, pti.supdatetime = tpti.supdatetime, pti.supdateusername = tpti.supdateusername, pti.scardinputunit = tpti.scardinputunit, pti.dcardinputqty = tpti.dcardinputqty FROM projectcardinputs pti JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "CardInputs tpti ON tpti.iprojectid = pti.iprojectid AND tpti.icardid = pti.icardid AND tpti.iinputid = pti.iinputid WHERE STR_TO_DATE(CONCAT(tpti.iupdatedate, ' ', tpti.supdatetime), '%Y%c%d %T') > STR_TO_DATE(CONCAT(pti.iupdatedate, ' ', pti.supdatetime), '%Y%c%d %T') "

    '            queries(7) = "" & _
    '            "INSERT INTO projectprices " & _
    '            "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "Prices " & _
    '            "WHERE NOT EXISTS (SELECT * FROM projectprices) "

    '            queries(8) = "" & _
    '            "INSERT INTO projectcardcompoundinputs " & _
    '            "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "CardCompoundInputs " & _
    '            "WHERE NOT EXISTS (SELECT * FROM projectcardcompoundinputs) "

    '            queries(9) = "" & _
    '            "INSERT INTO projectcardinputs " & _
    '            "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "CardInputs " & _
    '            "WHERE NOT EXISTS (SELECT * FROM projectcardinputs) "

    '            executeTransactedSQLCommand(0, queries)

    '        End If

    '    End If

    '    wasCreated = True

    '    Me.DialogResult = Windows.Forms.DialogResult.OK
    '    Me.Close()

    '    Cursor.Current = System.Windows.Forms.Cursors.Default

    'End Sub


    Private Sub btnGuardar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnGuardar.Click

        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        If validaTarjeta(False, True) = False Then
            Exit Sub
        End If

        Dim timesCardIsOpen As Integer = 1

        timesCardIsOpen = getSQLQueryAsInteger(0, "SELECT COUNT(*) FROM information_schema.TABLES WHERE TABLE_NAME LIKE '%CardAux" & icardid & "'")

        If timesCardIsOpen > 1 And IsEdit = True Then

            Cursor.Current = System.Windows.Forms.Cursors.Default

            If MsgBox("Otro usuario tiene abierta la misma Tarjeta. Esto podría causar que alguno de ustedes perdiera los cambios que hiciera. ¿Deseas seguir guardando la Tarjeta?", MsgBoxStyle.YesNo, "Confirmación Guardado") = MsgBoxResult.No Then

                Exit Sub

            Else

                Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

            End If

        ElseIf timesCardIsOpen > 1 And IsEdit = False Then

            Dim newIdAddition As Integer = 1

            Do While getSQLQueryAsInteger(0, "SELECT COUNT(*) FROM information_schema.TABLES WHERE TABLE_NAME LIKE '%CardAux" & icardid + newIdAddition & "'") > 1 And IsEdit = False
                newIdAddition = newIdAddition + 1
            Loop

            'I got the new id (previousId + newIdAddition)

            Dim queriesNewId(4) As String

            If IsBase = True Then

                queriesNewId(0) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardAux" & icardid
                queriesNewId(1) = "UPDATE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardInputs SET icardid = " & icardid + newIdAddition & " WHERE icardid = " & icardid

            Else

                If IsModel = True Then

                    queriesNewId(0) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardAux" & icardid
                    queriesNewId(1) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "CardAux" & icardid
                    queriesNewId(2) = "UPDATE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardInputs SET icardid = " & icardid + newIdAddition & " WHERE icardid = " & icardid
                    queriesNewId(3) = "UPDATE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "CardInputs SET icardid = " & icardid + newIdAddition & " WHERE icardid = " & icardid

                Else

                    queriesNewId(0) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardAux" & icardid
                    queriesNewId(1) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "CardAux" & icardid
                    queriesNewId(2) = "UPDATE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardInputs SET icardid = " & icardid + newIdAddition & " WHERE icardid = " & icardid
                    queriesNewId(3) = "UPDATE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "CardInputs SET icardid = " & icardid + newIdAddition & " WHERE icardid = " & icardid

                End If

            End If

            If executeTransactedSQLCommand(0, queriesNewId) = True Then
                icardid = icardid + newIdAddition
            End If

        End If

        Dim baseid As Integer = 0
        baseid = getSQLQueryAsInteger(0, "SELECT ibaseid FROM base ORDER BY iupdatedate DESC, supdatetime DESC LIMIT 1")

        If baseid = 0 Then
            baseid = 99999
        End If

        Dim valorIndirectos As Double = 0.0
        Try
            valorIndirectos = CDbl(txtPorcentajeIndirectos.Text)
        Catch ex As Exception

        End Try

        Dim valorUtilidades As Double = 0.0
        Try
            valorUtilidades = CDbl(txtPorcentajeUtilidades.Text)
        Catch ex As Exception

        End Try

        Dim fecha As Integer = 0
        Dim hora As String = ""

        fecha = getMySQLDate()
        hora = getAppTime()

        If IsEdit = True Then

            If IsBase = True Then
                executeSQLCommand(0, "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Cards SET iupdatedate = " & fecha & ", supdatetime = '" & hora & "', supdateusername = '" & susername & "', scardlegacycategoryid = '" & cmbCategoriaDeLaTarjeta.SelectedValue.ToString & "', scardlegacyid = '" & txtCodigoDeLaTarjeta.Text & "', scarddescription = '" & txtNombreDeLaTarjeta.Text & "', scardunit = '" & txtUnidadDeMedida.Text & "', dcardindirectcostspercentage = " & (valorIndirectos / 100) & ", dcardgainpercentage = " & (valorUtilidades / 100) & " WHERE ibaseid = " & iprojectid & " AND icardid = " & icardid)
            Else

                If IsModel = True Then
                    executeSQLCommand(0, "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "Cards SET iupdatedate = " & fecha & ", supdatetime = '" & hora & "', supdateusername = '" & susername & "', scardlegacycategoryid = '" & cmbCategoriaDeLaTarjeta.SelectedValue.ToString & "', scardlegacyid = '" & txtCodigoDeLaTarjeta.Text & "', scarddescription = '" & txtNombreDeLaTarjeta.Text & "', scardunit = '" & txtUnidadDeMedida.Text & "', dcardindirectcostspercentage = " & (valorIndirectos / 100) & ", dcardgainpercentage = " & (valorUtilidades / 100) & " WHERE imodelid = " & iprojectid & " AND icardid = " & icardid)
                Else
                    executeSQLCommand(0, "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "Cards SET iupdatedate = " & fecha & ", supdatetime = '" & hora & "', supdateusername = '" & susername & "', scardlegacycategoryid = '" & cmbCategoriaDeLaTarjeta.SelectedValue.ToString & "', scardlegacyid = '" & txtCodigoDeLaTarjeta.Text & "', scarddescription = '" & txtNombreDeLaTarjeta.Text & "', scardunit = '" & txtUnidadDeMedida.Text & "', dcardindirectcostspercentage = " & (valorIndirectos / 100) & ", dcardgainpercentage = " & (valorUtilidades / 100) & " WHERE iprojectid = " & iprojectid & " AND icardid = " & icardid)
                End If

            End If

        Else

            Dim checkIfItsOnlyTextUpdate As Boolean = False

            checkIfItsOnlyTextUpdate = getSQLQueryAsBoolean(0, "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Cards WHERE ibaseid = " & baseid & " AND icardid = " & icardid)

            If checkIfItsOnlyTextUpdate = True Then

                Dim queriesUpdate(2) As String

                queriesUpdate(0) = "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Cards SET iupdatedate = " & getMySQLDate() & ", supdatetime = '" & getAppTime() & "', supdateusername = '" & susername & "', scardlegacycategoryid = '" & cmbCategoriaDeLaTarjeta.SelectedValue.ToString & "', scardlegacyid = '" & txtCodigoDeLaTarjeta.Text & "', scarddescription = '" & txtNombreDeLaTarjeta.Text & "', scardunit = '" & txtUnidadDeMedida.Text & "', dcardindirectcostspercentage = " & (valorIndirectos / 100) & ", dcardgainpercentage = " & (valorUtilidades / 100) & " WHERE ibaseid = " & baseid & " AND icardid = " & icardid

                If IsModel = True Then
                    queriesUpdate(1) = "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "Cards SET iupdatedate = " & getMySQLDate() & ", supdatetime = '" & getAppTime() & "', supdateusername = '" & susername & "', scardlegacycategoryid = '" & cmbCategoriaDeLaTarjeta.SelectedValue.ToString & "', scardlegacyid = '" & txtCodigoDeLaTarjeta.Text & "', scarddescription = '" & txtNombreDeLaTarjeta.Text & "', scardunit = '" & txtUnidadDeMedida.Text & "', dcardindirectcostspercentage = " & (valorIndirectos / 100) & ", dcardgainpercentage = " & (valorUtilidades / 100) & " WHERE imodelid = " & iprojectid & " AND icardid = " & icardid
                Else
                    queriesUpdate(1) = "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "Cards SET iupdatedate = " & getMySQLDate() & ", supdatetime = '" & getAppTime() & "', supdateusername = '" & susername & "', scardlegacycategoryid = '" & cmbCategoriaDeLaTarjeta.SelectedValue.ToString & "', scardlegacyid = '" & txtCodigoDeLaTarjeta.Text & "', scarddescription = '" & txtNombreDeLaTarjeta.Text & "', scardunit = '" & txtUnidadDeMedida.Text & "', dcardindirectcostspercentage = " & (valorIndirectos / 100) & ", dcardgainpercentage = " & (valorUtilidades / 100) & " WHERE iprojectid = " & iprojectid & " AND icardid = " & icardid
                End If

                executeTransactedSQLCommand(0, queriesUpdate)

            Else

                icardid = getSQLQueryAsInteger(0, "SELECT IF(MAX(icardid) + 1 IS NULL, 1, MAX(icardid) + 1) AS icardid FROM basecards WHERE ibaseid = " & baseid)
                scardlegacyid = txtCodigoDeLaTarjeta.Text
                scarddescription = txtNombreDeLaTarjeta.Text

                Dim queries(2) As String

                queries(0) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Cards VALUES (" & baseid & ", " & icardid & ", '" & cmbCategoriaDeLaTarjeta.SelectedValue & "', '" & txtCodigoDeLaTarjeta.Text & "', '" & txtNombreDeLaTarjeta.Text & "', '" & txtUnidadDeMedida.Text & "', 1, " & (valorIndirectos / 100) & ", " & (valorUtilidades / 100) & ", " & fecha & ", '" & hora & "', '" & susername & "')"

                If IsModel = True Then
                    queries(1) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "Cards VALUES (" & iprojectid & ", " & icardid & ", '" & cmbCategoriaDeLaTarjeta.SelectedValue & "', '" & txtCodigoDeLaTarjeta.Text & "', '" & txtNombreDeLaTarjeta.Text & "', '" & txtUnidadDeMedida.Text & "', 1, " & (valorIndirectos / 100) & ", " & (valorUtilidades / 100) & ", " & fecha & ", '" & hora & "', '" & susername & "')"
                Else
                    queries(1) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "Cards VALUES (" & iprojectid & ", " & icardid & ", '" & cmbCategoriaDeLaTarjeta.SelectedValue & "', '" & txtCodigoDeLaTarjeta.Text & "', '" & txtNombreDeLaTarjeta.Text & "', '" & txtUnidadDeMedida.Text & "', 1, " & (valorIndirectos / 100) & ", " & (valorUtilidades / 100) & ", " & fecha & ", '" & hora & "', '" & susername & "')"
                End If

                executeTransactedSQLCommand(0, queries)

            End If

        End If

        executeSQLCommand(0, "INSERT IGNORE INTO logs VALUES (" & getMySQLDate() & ", '" & getAppTime() & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Guardó la Tarjeta " & icardid & " : " & txtNombreDeLaTarjeta.Text.Replace("'", "").Replace("--", "") & "', 'OK')")

        wasCreated = True

        Me.DialogResult = Windows.Forms.DialogResult.OK
        Me.Close()

        Cursor.Current = System.Windows.Forms.Cursors.Default

    End Sub


End Class