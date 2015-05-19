Public Class AgregarBase

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

    Public alertaMostrada As Boolean = False

    Public isEdit As Boolean = False
    Public isHistoric As Boolean = False
    Public isRecover As Boolean = False
    Public wasCreated As Boolean = False

    Private forceClose As Boolean = False

    Private paso2 As Boolean = False
    Private paso3 As Boolean = False
    Private paso4 As Boolean = False

    Private iselectedcardid As Integer = 0
    Private sselectedcardlegacyid As String = ""
    Private dselectedcardqty As Double = 1.0
    Private sselectedcarddescription As String = ""

    Private iselectedcostid As Integer = 0
    Private sselectedcostdescription As String = ""
    Private dselectedcostamount As Double = 0.0

    Private iprojectmodifieddate As Integer = 0
    Private sprojectmodifiedtime As String = "00:00:00"
    Private slastmodelapplied As String = ""

    Private isFormReadyForAction As Boolean = False
    Private isResumenDGVReady As Boolean = False

    Private WithEvents txtCantidadDgvCostosIndirectos As TextBox
    Private WithEvents txtNombreDgvCostosIndirectos As TextBox

    Private WithEvents txtCantidadDgvResumenDeTarjetas As TextBox
    Private WithEvents txtNombreDgvResumenDeTarjetas As TextBox
    Private WithEvents txtLegacyIdDgvResumenDeTarjetas As TextBox

    Private txtCantidadDgvCostosIndirectos_OldText As String = ""
    Private txtNombreDgvCostosIndirectos_OldText As String = ""

    Private txtCantidadDgvResumenDeTarjetas_OldText As String = ""
    Private txtNombreDgvResumenDeTarjetas_OldText As String = ""
    Private txtLegacyIdDgvResumenDeTarjetas_OldText As String = ""

    Private addIndirectCosts As Boolean = False
    Private modifyIndirectCosts As Boolean = False
    Private deleteIndirectCosts As Boolean = False

    Private addCards As Boolean = False
    Private openCards As Boolean = False
    Private deleteCards As Boolean = False

    Private modifyCardQty As Boolean = False
    Private viewDgvIndirectCosts As Boolean = False
    Private viewDgvProfits As Boolean = False
    Private viewDgvUnitPrices As Boolean = False
    Private viewDgvAmount As Boolean = False

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
                    btnGuardar.Enabled = True
                    btnGuardarYCerrar.Enabled = True
                End If

                If permission = "Ver Costos Indirectos" Then
                    dgvCostosIndirectos.Visible = True
                    lblTotalIndirectosLbl.Visible = True
                    lblTotalIndirectos.Visible = True
                    lblIngresosIndirectos.Visible = True
                    txtIngresosIndirectos.Visible = True
                    lblPorcentajeIndirectosLbl.Visible = True
                    lblPorcentajeIndirectos.Visible = True
                End If

                If permission = "Agregar Costo Indirecto" Then
                    addIndirectCosts = True
                    btnNuevoCostoIndirecto.Enabled = True
                    dgvCostosIndirectos.Visible = True
                    dgvCostosIndirectos.ReadOnly = False
                    lblTotalIndirectosLbl.Visible = True
                    lblTotalIndirectos.Visible = True
                    lblIngresosIndirectos.Visible = True
                    txtIngresosIndirectos.Visible = True
                    lblPorcentajeIndirectosLbl.Visible = True
                    lblPorcentajeIndirectos.Visible = True
                End If

                If permission = "Modificar Costos Indirectos" Then
                    modifyIndirectCosts = True
                    dgvCostosIndirectos.Visible = True
                    dgvCostosIndirectos.ReadOnly = False
                    lblTotalIndirectosLbl.Visible = True
                    lblTotalIndirectos.Visible = True
                    lblIngresosIndirectos.Visible = True
                    txtIngresosIndirectos.Visible = True
                    lblPorcentajeIndirectosLbl.Visible = True
                    lblPorcentajeIndirectos.Visible = True
                End If

                If permission = "Eliminar Costos Indirectos" Then
                    deleteIndirectCosts = True
                    btnEliminarCostoIndirecto.Enabled = True
                    dgvCostosIndirectos.Visible = True
                    lblTotalIndirectosLbl.Visible = True
                    lblTotalIndirectos.Visible = True
                    lblIngresosIndirectos.Visible = True
                    txtIngresosIndirectos.Visible = True
                    lblPorcentajeIndirectosLbl.Visible = True
                    lblPorcentajeIndirectos.Visible = True
                End If

                If permission = "Ver Resumen de Tarjetas" Then
                    dgvResumenDeTarjetas.Visible = True
                End If

                If permission = "Agregar Tarjeta" Then
                    addCards = True
                    dgvResumenDeTarjetas.Visible = True
                    btnNuevaTarjeta.Enabled = True
                End If

                If permission = "Modificar Resumen de Tarjetas" Then
                    openCards = True
                    modifyCardQty = True
                    dgvResumenDeTarjetas.Visible = True
                    btnInsertarTarjeta.Enabled = True
                End If

                If permission = "Eliminar Tarjeta" Then
                    deleteCards = True
                    dgvResumenDeTarjetas.Visible = True
                    btnEliminarTarjeta.Enabled = True
                End If

                If permission = "Cambiar Utilidades e Ind" Then
                    btnActualizarUtilidad.Enabled = True
                End If

                If permission = "Cambiar Defaults e IVA" Then
                    txtPorcentajeIVA.Enabled = True
                    txtPorcentajeUtilidadDefault.Enabled = True
                    txtPorcentajeIndirectosDefault.Enabled = True
                End If

                If permission = "Generar Archivo Excel" Then
                    btnGenerarArchivoExcel.Enabled = True
                End If

                If permission = "Ver Indirectos" Then
                    viewDgvIndirectCosts = True
                End If

                If permission = "Ver Utilidades" Then
                    viewDgvProfits = True
                End If

                If permission = "Ver Precios Unitarios" Then
                    viewDgvUnitPrices = True
                End If

                If permission = "Ver Importes" Then
                    viewDgvAmount = True
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

            executeSQLCommand(0, "INSERT IGNORE INTO logs VALUES (" & fecha & ", '" & hora & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Acceso denegado a la ventana de Ver/Editar Presupuestos Base', 'OK')")

            Dim dsUsuariosSysAdmin As DataSet

            dsUsuariosSysAdmin = getSQLQueryAsDataset(0, "SELECT susername FROM userspecialattributes WHERE bsysadmin = 1")

            If dsUsuariosSysAdmin.Tables(0).Rows.Count > 0 Then

                For i = 0 To dsUsuariosSysAdmin.Tables(0).Rows.Count - 1
                    executeSQLCommand(0, "INSERT INTO messages (susername, susersession, smessage, bread, imessagedatetime, smessagecreatorusername, iupdatedatetime, supdateusername) VALUES ('" & dsUsuariosSysAdmin.Tables(0).Rows(i).Item(0) & "', 0, 'Un usuario ha intentado propasar sus permisos. ¿Podrías revisar?', 0, '" & convertYYYYMMDDtoYYYYhyphenMMhyphenDD(fecha) & " " & hora & "', 'SYSTEM', '" & convertYYYYMMDDtoYYYYhyphenMMhyphenDD(fecha) & " " & hora & "', 'SYSTEM')")
                Next i

            End If

            MsgBox("No tienes los permisos necesarios para abrir esta Ventana. Este intento ha sido notificado al administrador.", MsgBoxStyle.Exclamation, "Access Denied")

            forceClose = True

            Me.DialogResult = Windows.Forms.DialogResult.Cancel
            Me.Close()

        End If

    End Sub


    Private Sub AgregarBase_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing

        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        If forceClose = True Then
            Exit Sub
        End If

        Dim conteo1 As Integer = 0
        Dim conteo2 As Integer = 0
        Dim conteo3 As Integer = 0
        Dim conteo4 As Integer = 0
        Dim conteo5 As Integer = 0
        Dim conteo6 As Integer = 0
        Dim conteo7 As Integer = 0
        Dim conteo8 As Integer = 0
        Dim conteo9 As Integer = 0
        Dim conteo10 As Integer = 0
        Dim conteo11 As Integer = 0
        Dim conteo12 As Integer = 0
        Dim conteo13 As Integer = 0
        Dim conteo14 As Integer = 0
        Dim conteo15 As Integer = 0
        Dim conteo16 As Integer = 0
        Dim conteo17 As Integer = 0
        Dim conteo18 As Integer = 0
        Dim conteo19 As Integer = 0
        Dim conteo20 As Integer = 0
        Dim conteo21 As Integer = 0

        Dim unsaved As Boolean = False


        conteo1 = getSQLQueryAsInteger(0, "" & _
        "SELECT COUNT(*) " & _
        "FROM base " & _
        "WHERE ibaseid = " & iprojectid & " AND " & _
        "NOT EXISTS (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & " tb WHERE base.ibaseid = tb.ibaseid) ")

        conteo2 = getSQLQueryAsInteger(0, "" & _
        "SELECT COUNT(*) " & _
        "FROM baseindirectcosts " & _
        "WHERE ibaseid = " & iprojectid & " AND " & _
        "NOT EXISTS (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "IndirectCosts tbic WHERE baseindirectcosts.ibaseid = tbic.ibaseid AND baseindirectcosts.icostid = tbic.icostid) ")

        conteo3 = getSQLQueryAsInteger(0, "" & _
        "SELECT COUNT(*) " & _
        "FROM basecards " & _
        "WHERE ibaseid = " & iprojectid & " AND " & _
        "NOT EXISTS (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Cards tbc WHERE basecards.ibaseid = tbc.ibaseid AND basecards.icardid = tbc.icardid) ")

        conteo4 = getSQLQueryAsInteger(0, "" & _
        "SELECT COUNT(*) " & _
        "FROM basecardinputs " & _
        "WHERE ibaseid = " & iprojectid & " AND " & _
        "NOT EXISTS (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardInputs tbci WHERE basecardinputs.ibaseid = tbci.ibaseid AND basecardinputs.icardid = tbci.icardid AND basecardinputs.iinputid = tbci.iinputid) ")

        conteo5 = getSQLQueryAsInteger(0, "" & _
        "SELECT COUNT(*) " & _
        "FROM basecardcompoundinputs " & _
        "WHERE ibaseid = " & iprojectid & " AND " & _
        "NOT EXISTS (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardCompoundInputs tbcci WHERE basecardcompoundinputs.ibaseid = tbcci.ibaseid AND basecardcompoundinputs.icardid = tbcci.icardid AND basecardcompoundinputs.iinputid = tbcci.iinputid AND basecardcompoundinputs.icompoundinputid = tbcci.icompoundinputid) ")

        conteo6 = getSQLQueryAsInteger(0, "" & _
        "SELECT COUNT(*) " & _
        "FROM baseprices " & _
        "WHERE ibaseid = " & iprojectid & " AND " & _
        "NOT EXISTS (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Prices tbp WHERE baseprices.ibaseid = tbp.ibaseid AND baseprices.iinputid = tbp.iinputid AND baseprices.iupdatedate = tbp.iupdatedate AND baseprices.supdatetime = tbp.supdatetime) ")

        conteo19 = getSQLQueryAsInteger(0, "" & _
        "SELECT COUNT(*) " & _
        "FROM basetimber " & _
        "WHERE ibaseid = " & iprojectid & " AND " & _
        "NOT EXISTS (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Timber tbt WHERE basetimber.ibaseid = tbt.ibaseid AND basetimber.iinputid = tbt.iinputid) ")

        conteo7 = getSQLQueryAsInteger(0, "" & _
        "SELECT COUNT(tp.*) FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & " tp JOIN base p ON tp.ibaseid = p.ibaseid WHERE STR_TO_DATE(CONCAT(tp.iupdatedate, ' ', tp.supdatetime), '%Y%c%d %T') > STR_TO_DATE(CONCAT(p.iupdatedate, ' ', p.supdatetime), '%Y%c%d %T') ")

        conteo8 = getSQLQueryAsInteger(0, "" & _
        "SELECT COUNT(tpic.*) FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "IndirectCosts tpic JOIN baseindirectcosts pic ON tpic.ibaseid = pic.ibaseid AND tpic.icostid = pic.icostid WHERE STR_TO_DATE(CONCAT(tpic.iupdatedate, ' ', tpic.supdatetime), '%Y%c%d %T') > STR_TO_DATE(CONCAT(pic.iupdatedate, ' ', pic.supdatetime), '%Y%c%d %T') ")

        conteo9 = getSQLQueryAsInteger(0, "" & _
        "SELECT COUNT(tpc.*) FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Cards tpc JOIN basecards pc ON tpc.ibaseid = pc.ibaseid AND tpc.icardid = pc.icardid WHERE STR_TO_DATE(CONCAT(tpc.iupdatedate, ' ', tpc.supdatetime), '%Y%c%d %T') > STR_TO_DATE(CONCAT(pc.iupdatedate, ' ', pc.supdatetime), '%Y%c%d %T') ")

        conteo10 = getSQLQueryAsInteger(0, "" & _
        "SELECT COUNT(tpci.*) FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardInputs tpci JOIN basecardinputs pci ON tpci.ibaseid = pci.ibaseid AND tpci.icardid = pci.icardid AND tpci.iinputid = pci.iinputid WHERE STR_TO_DATE(CONCAT(tpci.iupdatedate, ' ', tpci.supdatetime), '%Y%c%d %T') > STR_TO_DATE(CONCAT(pci.iupdatedate, ' ', pci.supdatetime), '%Y%c%d %T') ")

        conteo11 = getSQLQueryAsInteger(0, "" & _
        "SELECT COUNT(tpcci.*) FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardCompoundInputs tpcci JOIN basecardcompoundinputs pcci ON tpcci.ibaseid = pcci.ibaseid AND tpcci.icardid = pcci.icardid AND tpcci.iinputid = pcci.iinputid AND tpcci.icompoundinputid = pcci.icompoundinputid WHERE STR_TO_DATE(CONCAT(tpcci.iupdatedate, ' ', tpcci.supdatetime), '%Y%c%d %T') > STR_TO_DATE(CONCAT(pcci.iupdatedate, ' ', pcci.supdatetime), '%Y%c%d %T') ")

        conteo12 = getSQLQueryAsInteger(0, "" & _
        "SELECT COUNT(tpp.*) FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Prices tpp JOIN baseprices pp ON tpp.ibaseid = pp.ibaseid AND tpp.iinputid = pp.iinputid WHERE STR_TO_DATE(CONCAT(tpp.iupdatedate, ' ', tpp.supdatetime), '%Y%c%d %T') > STR_TO_DATE(CONCAT(pp.iupdatedate, ' ', pp.supdatetime), '%Y%c%d %T') ")

        conteo20 = getSQLQueryAsInteger(0, "" & _
        "SELECT COUNT(tpt.*) FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Timber tpt JOIN basetimber pt ON tpt.ibaseid = pt.ibaseid AND tpt.iinputid = pt.iinputid WHERE STR_TO_DATE(CONCAT(tpt.iupdatedate, ' ', tpt.supdatetime), '%Y%c%d %T') > STR_TO_DATE(CONCAT(pt.iupdatedate, ' ', pt.supdatetime), '%Y%c%d %T') ")

        conteo13 = getSQLQueryAsInteger(0, "" & _
        "SELECT COUNT(*) " & _
        "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & " tb " & _
        "WHERE NOT EXISTS (SELECT * FROM base b WHERE b.ibaseid = tb.ibaseid AND b.ibaseid = " & iprojectid & ") ")

        conteo14 = getSQLQueryAsInteger(0, "" & _
        "SELECT COUNT(*) " & _
        "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "IndirectCosts tbic " & _
        "WHERE NOT EXISTS (SELECT * FROM baseindirectcosts bic WHERE tbic.ibaseid = bic.ibaseid AND tbic.icostid = bic.icostid AND bic.ibaseid = " & iprojectid & ") ")

        conteo15 = getSQLQueryAsInteger(0, "" & _
        "SELECT COUNT(*) " & _
        "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Cards tbc " & _
        "WHERE NOT EXISTS (SELECT * FROM basecards bc WHERE tbc.ibaseid = bc.ibaseid AND tbc.icardid = bc.icardid AND bc.ibaseid = " & iprojectid & ") ")

        conteo16 = getSQLQueryAsInteger(0, "" & _
        "SELECT COUNT(*) " & _
        "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardInputs tbci " & _
        "WHERE NOT EXISTS (SELECT * FROM basecardinputs bci WHERE tbci.ibaseid = bci.ibaseid AND tbci.icardid = bci.icardid AND tbci.iinputid = bci.iinputid AND bci.ibaseid = " & iprojectid & ") ")

        conteo17 = getSQLQueryAsInteger(0, "" & _
        "SELECT COUNT(*) " & _
        "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardCompoundInputs tbcci " & _
        "WHERE NOT EXISTS (SELECT * FROM basecardcompoundinputs bcci WHERE tbcci.ibaseid = bcci.ibaseid AND tbcci.icardid = bcci.icardid AND tbcci.iinputid = bcci.iinputid AND tbcci.icompoundinputid = bcci.icompoundinputid AND bcci.ibaseid = " & iprojectid & ") ")

        conteo18 = getSQLQueryAsInteger(0, "" & _
        "SELECT COUNT(*) " & _
        "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Prices tbp " & _
        "WHERE NOT EXISTS (SELECT * FROM baseprices bp WHERE tbp.ibaseid = bp.ibaseid AND tbp.iinputid = bp.iinputid AND tbp.iupdatedate = bp.iupdatedate AND tbp.supdatetime = bp.supdatetime AND bp.ibaseid = " & iprojectid & ") ")

        conteo21 = getSQLQueryAsInteger(0, "" & _
        "SELECT COUNT(*) " & _
        "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Timber tbt " & _
        "WHERE NOT EXISTS (SELECT * FROM basetimber bt WHERE tbt.ibaseid = bt.ibaseid AND tbt.iinputid = bt.iinputid AND bt.ibaseid = " & iprojectid & ") ")

        If conteo1 + conteo2 + conteo3 + conteo4 + conteo5 + conteo6 + conteo7 + conteo8 + conteo10 + conteo11 + conteo12 + conteo13 + conteo14 + conteo15 + conteo16 + conteo17 + conteo18 + conteo19 + conteo20 + conteo21 > 0 Then

            unsaved = True

        End If

        Dim incomplete As Boolean = False
        Dim msg As String = ""
        Dim result As Integer = 0

        If (validaCostosIndirectos(True) = False Or validaResumenDeTarjetas(True) = False) And Me.DialogResult <> Windows.Forms.DialogResult.OK And isHistoric = False Then
            incomplete = True
        End If

        Cursor.Current = System.Windows.Forms.Cursors.Default

        If incomplete = True Then
            result = MsgBox("Este Presupuesto Base está incompleto. Si sales ahora, se perderán los cambios que hayas hecho." & Chr(13) & "¿Realmente deseas Salir de esta ventana ahora?", MsgBoxStyle.YesNo, "Confirmación Salida")
        ElseIf unsaved = True Then
            result = MsgBox("Tienes datos sin guardar! Tienes 3 opciones: " & Chr(13) & "Guardar los cambios (Sí), Regresar a revisar los cambios y guardarlos manualmente (Cancelar) o No guardarlos (No)", MsgBoxStyle.YesNoCancel, "Confirmación Salida")
        End If

        If result = MsgBoxResult.No And incomplete = True Then

            Cursor.Current = System.Windows.Forms.Cursors.Default
            e.Cancel = True
            Exit Sub

        ElseIf result = MsgBoxResult.Yes And incomplete = False And isHistoric = False Then

            Dim timesBaseIsOpen As Integer = 1

            timesBaseIsOpen = getSQLQueryAsInteger(0, "SELECT COUNT(*) FROM information_schema.TABLES WHERE TABLE_NAME LIKE '%Base" & iprojectid & "'")

            If timesBaseIsOpen > 1 And isEdit = True Then

                Cursor.Current = System.Windows.Forms.Cursors.Default

                If MsgBox("Otro usuario tiene abierto el mismo Presupuesto Base. Esto podría causar que alguno de ustedes perdiera los cambios que hiciera. ¿Deseas seguir guardando el Presupuesto Base?", MsgBoxStyle.YesNo, "Confirmación Guardado") = MsgBoxResult.No Then

                    e.Cancel = True
                    Exit Sub

                Else

                    Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

                End If

            ElseIf timesBaseIsOpen > 1 And isEdit = False Then

                Dim newIdAddition As Integer = 1

                Do While getSQLQueryAsInteger(0, "SELECT COUNT(*) FROM information_schema.TABLES WHERE TABLE_NAME LIKE '%Base" & iprojectid + newIdAddition & "'") > 1 And isEdit = False
                    newIdAddition = newIdAddition + 1
                Loop

                'I got the new id (previousId + newIdAddition)

                Dim queriesNewId(31) As String

                queriesNewId(8) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & " RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid + newIdAddition
                queriesNewId(9) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "IndirectCosts RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid + newIdAddition & "IndirectCosts"
                queriesNewId(10) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Cards RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid + newIdAddition & "Cards"
                queriesNewId(11) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardInputs RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid + newIdAddition & "CardInputs"
                queriesNewId(12) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardCompoundInputs RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid + newIdAddition & "CardCompoundInputs"
                queriesNewId(13) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Prices RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid + newIdAddition & "Prices"
                queriesNewId(14) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Timber RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid + newIdAddition & "Timber"
                queriesNewId(15) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "CardsAux RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid + newIdAddition & "CardsAux"
                queriesNewId(24) = "UPDATE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid + newIdAddition & " SET ibaseid = " & iprojectid + newIdAddition & " WHERE ibaseid = " & iprojectid
                queriesNewId(25) = "UPDATE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid + newIdAddition & "IndirectCosts SET ibaseid = " & iprojectid + newIdAddition & " WHERE ibaseid = " & iprojectid
                queriesNewId(26) = "UPDATE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid + newIdAddition & "Cards SET ibaseid = " & iprojectid + newIdAddition & " WHERE ibaseid = " & iprojectid
                queriesNewId(27) = "UPDATE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid + newIdAddition & "CardInputs SET ibaseid = " & iprojectid + newIdAddition & " WHERE ibaseid = " & iprojectid
                queriesNewId(28) = "UPDATE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid + newIdAddition & "CardCompoundInputs SET ibaseid = " & iprojectid + newIdAddition & " WHERE ibaseid = " & iprojectid
                queriesNewId(29) = "UPDATE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid + newIdAddition & "Prices SET ibaseid = " & iprojectid + newIdAddition & " WHERE ibaseid = " & iprojectid
                queriesNewId(30) = "UPDATE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid + newIdAddition & "Timber SET ibaseid = " & iprojectid + newIdAddition & " WHERE ibaseid = " & iprojectid

                If executeTransactedSQLCommand(0, queriesNewId) = True Then
                    iprojectid = iprojectid + newIdAddition
                End If

            End If

            Dim queriesSave(29) As String

            queriesSave(0) = "" & _
            "DELETE " & _
            "FROM base " & _
            "WHERE ibaseid = " & iprojectid & " AND " & _
            "NOT EXISTS (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & " tb WHERE base.ibaseid = tb.ibaseid) "

            queriesSave(1) = "" & _
            "DELETE " & _
            "FROM baseindirectcosts " & _
            "WHERE ibaseid = " & iprojectid & " AND " & _
            "NOT EXISTS (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "IndirectCosts tbic WHERE baseindirectcosts.ibaseid = tbic.ibaseid AND baseindirectcosts.icostid = tbic.icostid) "

            queriesSave(2) = "" & _
            "DELETE " & _
            "FROM basecards " & _
            "WHERE ibaseid = " & iprojectid & " AND " & _
            "NOT EXISTS (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Cards tbc WHERE basecards.ibaseid = tbc.ibaseid AND basecards.icardid = tbc.icardid) "

            queriesSave(3) = "" & _
            "DELETE " & _
            "FROM basecards " & _
            "WHERE ibaseid = " & iprojectid & " AND " & _
            "NOT EXISTS (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Cards tbc WHERE basecards.ibaseid = tbc.ibaseid AND basecards.icardid = tbc.icardid) "

            'queriesSave(4) = "" & _
            '"DELETE " & _
            '"FROM projectcards " & _
            '"WHERE NOT EXISTS (SELECT * FROM basecards bc WHERE bc.ibaseid = " & iprojectid & " AND projectcards.icardid = bc.icardid) "

            'queriesSave(5) = "" & _
            '"DELETE " & _
            '"FROM modelcards " & _
            '"WHERE NOT EXISTS (SELECT * FROM basecards bc WHERE bc.ibaseid = " & iprojectid & " AND modelcards.icardid = bc.icardid) "

            queriesSave(6) = "" & _
            "DELETE " & _
            "FROM basecardinputs " & _
            "WHERE ibaseid = " & iprojectid & " AND " & _
            "NOT EXISTS (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardInputs tbci WHERE basecardinputs.ibaseid = tbci.ibaseid AND basecardinputs.icardid = tbci.icardid AND basecardinputs.iinputid = tbci.iinputid) "

            'queriesSave(7) = "" & _
            '"DELETE " & _
            '"FROM projectcardinputs " & _
            '"WHERE NOT EXISTS (SELECT * FROM basecardinputs bci WHERE bci.ibaseid = " & iprojectid & " AND projectcardinputs.icardid = bci.icardid AND projectcardinputs.iinputid = bci.iinputid) "

            'queriesSave(8) = "" & _
            '"DELETE " & _
            '"FROM modelcardinputs " & _
            '"WHERE NOT EXISTS (SELECT * FROM basecardinputs bci WHERE bci.ibaseid = " & iprojectid & " AND modelcardinputs.icardid = bci.icardid AND modelcardinputs.iinputid = bci.iinputid) "

            queriesSave(9) = "" & _
            "DELETE " & _
            "FROM basecardcompoundinputs " & _
            "WHERE ibaseid = " & iprojectid & " AND " & _
            "NOT EXISTS (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardCompoundInputs tbcci WHERE basecardcompoundinputs.ibaseid = tbcci.ibaseid AND basecardcompoundinputs.icardid = tbcci.icardid AND basecardcompoundinputs.iinputid = tbcci.iinputid AND basecardcompoundinputs.icompoundinputid = tbcci.icompoundinputid) "

            'queriesSave(10) = "" & _
            '"DELETE " & _
            '"FROM projectcardcompoundinputs " & _
            '"WHERE NOT EXISTS (SELECT * FROM basecardcompoundinputs bcci WHERE bcci.ibaseid = " & iprojectid & " AND projectcardcompoundinputs.icardid = bcci.icardid AND projectcardcompoundinputs.iinputid = bcci.iinputid AND projectcardcompoundinputs.icompoundinputid = bcci.icompoundinputid) "

            'queriesSave(11) = "" & _
            '"DELETE " & _
            '"FROM modelcardcompoundinputs " & _
            '"WHERE NOT EXISTS (SELECT * FROM basecardcompoundinputs bcci WHERE bcci.ibaseid = " & iprojectid & " AND modelcardcompoundinputs.icardid = bcci.icardid AND modelcardcompoundinputs.iinputid = bcci.iinputid AND modelcardcompoundinputs.icompoundinputid = bcci.icompoundinputid) "

            queriesSave(12) = "" & _
            "DELETE " & _
            "FROM baseprices " & _
            "WHERE ibaseid = " & iprojectid & " AND " & _
            "NOT EXISTS (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Prices tbp WHERE baseprices.ibaseid = tbp.ibaseid AND baseprices.iinputid = tbp.iinputid AND baseprices.iupdatedate = tbp.iupdatedate AND baseprices.supdatetime = tbp.supdatetime) "

            queriesSave(13) = "" & _
            "DELETE " & _
            "FROM basetimber " & _
            "WHERE ibaseid = " & iprojectid & " AND " & _
            "NOT EXISTS (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Timber tbt WHERE basetimber.ibaseid = tbt.ibaseid AND basetimber.iinputid = tbt.iinputid) "

            queriesSave(14) = "" & _
            "UPDATE base p JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & " tp ON tp.ibaseid = p.ibaseid SET p.iupdatedate = tp.iupdatedate, p.supdatetime = tp.supdatetime, p.supdateusername = tp.supdateusername, p.sbasefileslocation = tp.sbasefileslocation, p.dbaseindirectpercentagedefault = tp.dbaseindirectpercentagedefault, p.dbasegainpercentagedefault = tp.dbasegainpercentagedefault, p.dbaseIVApercentage = tp.dbaseIVApercentage WHERE STR_TO_DATE(CONCAT(tp.iupdatedate, ' ', tp.supdatetime), '%Y%c%d %T') > STR_TO_DATE(CONCAT(p.iupdatedate, ' ', p.supdatetime), '%Y%c%d %T') "

            queriesSave(15) = "" & _
            "UPDATE baseindirectcosts pic JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "IndirectCosts tpic ON tpic.ibaseid = pic.ibaseid AND tpic.icostid = pic.icostid SET pic.iupdatedate = tpic.iupdatedate, pic.supdatetime = tpic.supdatetime, pic.supdateusername = tpic.supdateusername, pic.sbasecostname = tpic.sbasecostname, pic.dbasecost = tpic.dbasecost, pic.dcompanyprojectedearnings = tpic.dcompanyprojectedearnings WHERE STR_TO_DATE(CONCAT(tpic.iupdatedate, ' ', tpic.supdatetime), '%Y%c%d %T') > STR_TO_DATE(CONCAT(pic.iupdatedate, ' ', pic.supdatetime), '%Y%c%d %T') "

            queriesSave(16) = "" & _
            "UPDATE basecards pc JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Cards tpc ON tpc.ibaseid = pc.ibaseid AND tpc.icardid = pc.icardid SET pc.iupdatedate = tpc.iupdatedate, pc.supdatetime = tpc.supdatetime, pc.supdateusername = tpc.supdateusername, pc.scardlegacycategoryid = tpc.scardlegacycategoryid, pc.scardlegacyid = tpc.scardlegacyid, pc.scarddescription = tpc.scarddescription, pc.scardunit = tpc.scardunit, pc.dcardqty = tpc.dcardqty, pc.dcardindirectcostspercentage = tpc.dcardindirectcostspercentage, pc.dcardgainpercentage = tpc.dcardgainpercentage WHERE STR_TO_DATE(CONCAT(tpc.iupdatedate, ' ', tpc.supdatetime), '%Y%c%d %T') > STR_TO_DATE(CONCAT(pc.iupdatedate, ' ', pc.supdatetime), '%Y%c%d %T') "

            queriesSave(17) = "" & _
            "UPDATE basecardinputs pci JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardInputs tpci ON tpci.ibaseid = pci.ibaseid AND tpci.icardid = pci.icardid AND tpci.iinputid = pci.iinputid SET pci.iupdatedate = tpci.iupdatedate, pci.supdatetime = tpci.supdatetime, pci.supdateusername = tpci.supdateusername, pci.scardinputunit = tpci.scardinputunit, pci.dcardinputqty = tpci.dcardinputqty WHERE STR_TO_DATE(CONCAT(tpci.iupdatedate, ' ', tpci.supdatetime), '%Y%c%d %T') > STR_TO_DATE(CONCAT(pci.iupdatedate, ' ', pci.supdatetime), '%Y%c%d %T') "

            queriesSave(18) = "" & _
            "UPDATE basecardcompoundinputs pcci JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardCompoundInputs tpcci ON tpcci.ibaseid = pcci.ibaseid AND tpcci.icardid = pcci.icardid AND tpcci.iinputid = pcci.iinputid AND tpcci.icompoundinputid = pcci.icompoundinputid SET pcci.iupdatedate = tpcci.iupdatedate, pcci.supdatetime = tpcci.supdatetime, pcci.supdateusername = tpcci.supdateusername, pcci.scompoundinputunit = tpcci.scompoundinputunit, pcci.dcompoundinputqty = tpcci.dcompoundinputqty WHERE STR_TO_DATE(CONCAT(tpcci.iupdatedate, ' ', tpcci.supdatetime), '%Y%c%d %T') > STR_TO_DATE(CONCAT(pcci.iupdatedate, ' ', pcci.supdatetime), '%Y%c%d %T') "

            queriesSave(19) = "" & _
            "UPDATE baseprices pp JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Prices tpp ON tpp.ibaseid = pp.ibaseid AND tpp.iinputid = pp.iinputid AND tpp.iupdatedate = pp.iupdatedate AND tpp.supdatetime = pp.supdatetime SET pp.iupdatedate = tpp.iupdatedate, pp.supdatetime = tpp.supdatetime, pp.supdateusername = tpp.supdateusername, pp.dinputpricewithoutIVA = tpp.dinputpricewithoutIVA, pp.dinputprotectionpercentage = tpp.dinputprotectionpercentage, pp.dinputfinalprice = tpp.dinputfinalprice WHERE STR_TO_DATE(CONCAT(pp.iupdatedate, ' ', pp.supdatetime), '%Y%c%d %T') > STR_TO_DATE(CONCAT(pp.iupdatedate, ' ', pp.supdatetime), '%Y%c%d %T') "

            queriesSave(20) = "" & _
            "UPDATE basetimber pt JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Timber tpt ON tpt.ibaseid = pt.ibaseid AND tpt.iinputid = pt.iinputid SET pt.iupdatedate = tpt.iupdatedate, pt.supdatetime = tpt.supdatetime, pt.supdateusername = tpt.supdateusername, pt.dinputtimberespesor = tpt.dinputtimberespesor, pt.dinputtimberancho = tpt.dinputtimberancho, pt.dinputtimberlargo = tpt.dinputtimberlargo, pt.dinputtimberpreciopiecubico = tpt.dinputtimberpreciopiecubico WHERE STR_TO_DATE(CONCAT(tpt.iupdatedate, ' ', tpt.supdatetime), '%Y%c%d %T') > STR_TO_DATE(CONCAT(pt.iupdatedate, ' ', pt.supdatetime), '%Y%c%d %T') "

            queriesSave(21) = "" & _
            "INSERT INTO base " & _
            "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & " tb " & _
            "WHERE NOT EXISTS (SELECT * FROM base b WHERE b.ibaseid = tb.ibaseid AND b.ibaseid = " & iprojectid & ") "

            queriesSave(22) = "" & _
            "INSERT INTO baseindirectcosts " & _
            "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "IndirectCosts tbic " & _
            "WHERE NOT EXISTS (SELECT * FROM baseindirectcosts bic WHERE tbic.ibaseid = bic.ibaseid AND tbic.icostid = bic.icostid AND bic.ibaseid = " & iprojectid & ") "

            queriesSave(23) = "" & _
            "INSERT INTO basecards " & _
            "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Cards tbc " & _
            "WHERE NOT EXISTS (SELECT * FROM basecards bc WHERE tbc.ibaseid = bc.ibaseid AND tbc.icardid = bc.icardid AND bc.ibaseid = " & iprojectid & ") "

            queriesSave(24) = "" & _
            "INSERT INTO basecardinputs " & _
            "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardInputs tbci " & _
            "WHERE NOT EXISTS (SELECT * FROM basecardinputs bci WHERE tbci.ibaseid = bci.ibaseid AND tbci.icardid = bci.icardid AND tbci.iinputid = bci.iinputid AND bci.ibaseid = " & iprojectid & ") "

            queriesSave(25) = "" & _
            "INSERT INTO basecardcompoundinputs " & _
            "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardCompoundInputs tbcci " & _
            "WHERE NOT EXISTS (SELECT * FROM basecardcompoundinputs bcci WHERE tbcci.ibaseid = bcci.ibaseid AND tbcci.icardid = bcci.icardid AND tbcci.iinputid = bcci.iinputid AND tbcci.icompoundinputid = bcci.icompoundinputid AND bcci.ibaseid = " & iprojectid & ") "

            queriesSave(26) = "" & _
            "INSERT INTO baseprices " & _
            "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Prices tbp " & _
            "WHERE NOT EXISTS (SELECT * FROM baseprices bp WHERE tbp.ibaseid = bp.ibaseid AND tbp.iinputid = bp.iinputid AND tbp.iupdatedate = bp.iupdatedate AND tbp.supdatetime = bp.supdatetime AND bp.ibaseid = " & iprojectid & ") "

            queriesSave(27) = "" & _
            "INSERT INTO basetimber " & _
            "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Timber tbt " & _
            "WHERE NOT EXISTS (SELECT * FROM basetimber bt WHERE tbt.ibaseid = bt.ibaseid AND tbt.iinputid = bt.iinputid AND bt.ibaseid = " & iprojectid & ") "

            queriesSave(28) = "INSERT IGNORE INTO logs VALUES (" & getMySQLDate() & ", '" & getAppTime() & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Guardó cambios al Presupuesto Base " & iprojectid & ": " & dtFechaPresupuesto.Value & "', 'OK')"

            If executeTransactedSQLCommand(0, queriesSave) = True Then
                MsgBox("Guardado exitosamente", MsgBoxStyle.OkOnly, "")
            Else
                MsgBox("Hubo un error al Guardar. Probablemente un error de Red. Intenta nuevamente", MsgBoxStyle.OkOnly, "")
                Cursor.Current = System.Windows.Forms.Cursors.Default
                e.Cancel = True
                Exit Sub
            End If

            wasCreated = True

        ElseIf result = MsgBoxResult.Cancel Then

            Cursor.Current = System.Windows.Forms.Cursors.Default
            e.Cancel = True
            Exit Sub

        End If



        Dim fecha As Integer = getMySQLDate()
        Dim hora As String = getAppTime()

        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        Dim queriesDelete(10) As String

        queriesDelete(0) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid
        queriesDelete(1) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "IndirectCosts"
        queriesDelete(2) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Cards"
        queriesDelete(3) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardsAux"
        queriesDelete(4) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardInputs"
        queriesDelete(5) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardCompoundInputs"
        queriesDelete(6) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Prices"
        queriesDelete(7) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Timber"
        queriesDelete(8) = "INSERT IGNORE INTO logs VALUES (" & fecha & ", '" & hora & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Cerró el Presupuesto Base " & iprojectid & " : " & dtFechaPresupuesto.Value & "', 'OK')"
        queriesDelete(9) = "INSERT INTO recentlyopenedfiles VALUES ('" & susername & "', '" & susersession & "', 'Base', 'Presupuesto Base', '" & iprojectid & "', '" & dtFechaPresupuesto.Value & "', 0, " & fecha & ", '" & hora & "', '" & susername & "')"

        executeTransactedSQLCommand(0, queriesDelete)

        verifySuspiciousData()

        Cursor.Current = System.Windows.Forms.Cursors.Default

    End Sub


    Private Sub AgregarBase_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown

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


    Private Sub AgregarBase_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        Me.KeyPreview = True

        closeTimedOutConnections()
        checkForKickoutsAndTimedOuts()
        checkMessages(susername, Me.Location.X, Me.Location.Y)
        setControlsByPermissions(Me.Name, susername)

        Dim timesBaseIsOpen As Integer = 0

        timesBaseIsOpen = getSQLQueryAsInteger(0, "SELECT COUNT(*) FROM information_schema.TABLES WHERE TABLE_NAME LIKE '%Base" & iprojectid & "CardAux%'")

        If timesBaseIsOpen > 0 And isEdit = True Then

            Cursor.Current = System.Windows.Forms.Cursors.Default

            If MsgBox("Otro usuario tiene abierto el mismo Presupuesto Base. Esto podría causar que alguno de ustedes perdiera los cambios que hiciera. ¿Deseas seguir abriendo el Presupuesto Base?", MsgBoxStyle.YesNo, "Confirmación Apertura") = MsgBoxResult.No Then

                Me.DialogResult = Windows.Forms.DialogResult.Cancel
                Me.Close()

                Cursor.Current = System.Windows.Forms.Cursors.Default
                Exit Sub

            Else

                Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

            End If

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

        If isEdit = False Then

            'Presupuesto Base nuevo

            tcBase.SelectedTab = tpCostosIndirectos

            dtFechaPresupuesto.Value = convertYYYYMMDDtoDDhyphenMMhyphenYYYY(fecha) & " " & hora

            iprojectid = getSQLQueryAsInteger(0, "SELECT 99999 - IF(MAX(ibaseid) IS NULL, 0, MAX(ibaseid)) AS ibaseid FROM base")

            Dim queriesCreation(14) As String

            queriesCreation(0) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid
            queriesCreation(1) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & " ( `ibaseid` int(11) NOT NULL AUTO_INCREMENT, `sbasefileslocation` varchar(400) CHARACTER SET latin1 NOT NULL, `dbaseindirectpercentagedefault` decimal(20,5) NOT NULL, `dbasegainpercentagedefault` decimal(20,5) NOT NULL, `dbaseIVApercentage` decimal(20,5) NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL, PRIMARY KEY (`ibaseid`), KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"

            queriesCreation(2) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "IndirectCosts"
            queriesCreation(3) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "IndirectCosts" & " ( `ibaseid` int(11) NOT NULL, `icostid` int(11) NOT NULL, `sbasecostname` varchar(500) CHARACTER SET latin1 NOT NULL, `dbasecost` decimal(20,5) NOT NULL, `dcompanyprojectedearnings` decimal(20,5) NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL, PRIMARY KEY (`ibaseid`,`icostid`), KEY `baseid` (`ibaseid`), KEY `costid` (`icostid`), KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"

            queriesCreation(4) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Cards"
            queriesCreation(5) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Cards" & " ( `ibaseid` int(11) NOT NULL, `icardid` int(11) NOT NULL, `scardlegacycategoryid` varchar(10) CHARACTER SET latin1 NOT NULL, `scardlegacyid` varchar(10) CHARACTER SET latin1 NOT NULL, `scarddescription` varchar(1000) CHARACTER SET latin1 NOT NULL, `scardunit` varchar(50) CHARACTER SET latin1 NOT NULL, `dcardqty` decimal(20,5) NOT NULL, `dcardindirectcostspercentage` decimal(20,5) NOT NULL, `dcardgainpercentage` decimal(20,5) NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL, PRIMARY KEY (`ibaseid`,`icardid`), KEY `baseid` (`ibaseid`), KEY `cardid` (`icardid`), KEY `cardlegacycategoryid` (`scardlegacycategoryid`), KEY `cardlegacyid` (`scardlegacyid`), KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"

            queriesCreation(6) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardInputs"
            queriesCreation(7) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardInputs" & " ( `ibaseid` int(11) NOT NULL, `icardid` int(11) NOT NULL, `iinputid` int(11) NOT NULL, `scardinputunit` varchar(50) CHARACTER SET latin1 NOT NULL, `dcardinputqty` decimal(20,5) NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL, PRIMARY KEY (`ibaseid`,`icardid`,`iinputid`), KEY `baseid` (`ibaseid`), KEY `cardid` (`icardid`), KEY `inputid` (`iinputid`), KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"

            queriesCreation(8) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardCompoundInputs"
            queriesCreation(9) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardCompoundInputs" & " ( `ibaseid` int(11) NOT NULL, `icardid` int(11) NOT NULL, `iinputid` int(11) NOT NULL, `icompoundinputid` int(11) NOT NULL, `scompoundinputunit` varchar(50) CHARACTER SET latin1 NOT NULL, `dcompoundinputqty` decimal(20,5) NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL, PRIMARY KEY (`ibaseid`,`icardid`,`iinputid`,`icompoundinputid`), KEY `baseid` (`ibaseid`), KEY `cardid` (`icardid`), KEY `inputid` (`iinputid`), KEY `compoundinputid` (`icompoundinputid`), KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"

            queriesCreation(10) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Prices"
            queriesCreation(11) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Prices" & " ( `ibaseid` int(11) NOT NULL, `iinputid` int(11) NOT NULL, `dinputpricewithoutIVA` decimal(20,5) NOT NULL, `dinputprotectionpercentage` decimal(20,5) NOT NULL, `dinputfinalprice` decimal(20,5) NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL, PRIMARY KEY (`ibaseid`,`iinputid`,`iupdatedate`,`supdatetime`), KEY `baseid` (`ibaseid`), KEY `inputid` (`iinputid`), KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"

            queriesCreation(12) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Timber"
            queriesCreation(13) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Timber ( `ibaseid` int(11) NOT NULL, `iinputid` int(11) NOT NULL, `dinputtimberespesor` decimal(20,5) NOT NULL, `dinputtimberancho` decimal(20,5) NOT NULL, `dinputtimberlargo` decimal(20,5) NOT NULL, `dinputtimberpreciopiecubico` decimal(20,5) NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL, PRIMARY KEY (`ibaseid`,`iinputid`), KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"

            executeTransactedSQLCommand(0, queriesCreation)


            Dim queriesInsert(7) As String

            queriesInsert(0) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & " (sbasefileslocation, dbaseindirectpercentagedefault, dbasegainpercentagedefault, dbaseIVApercentage, iupdatedate, supdatetime, supdateusername) VALUES ('', " & txtPorcentajeIndirectosDefault.Text & ", " & txtPorcentajeUtilidadDefault.Text & ", " & txtPorcentajeIVA.Text & ", " & getMySQLDate() & ", '" & getAppTime() & "', '" & susername & "')"
            queriesInsert(1) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "IndirectCosts SELECT * FROM baseindirectcosts WHERE ibaseid = " & baseid
            queriesInsert(2) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Cards SELECT * FROM basecards WHERE ibaseid = " & baseid
            queriesInsert(3) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardInputs SELECT * FROM basecardinputs WHERE ibaseid = " & baseid
            queriesInsert(4) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardCompoundInputs SELECT * FROM basecardcompoundinputs WHERE ibaseid = " & baseid
            queriesInsert(5) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Prices SELECT * FROM baseprices WHERE ibaseid = " & baseid
            queriesInsert(6) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Timber SELECT * FROM basetimber WHERE ibaseid = " & baseid

            executeTransactedSQLCommand(0, queriesInsert)


            isEdit = True

        Else

            If isRecover = False Then

                Dim queriesCreation(14) As String

                queriesCreation(0) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid
                queriesCreation(1) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & " ( `ibaseid` int(11) NOT NULL AUTO_INCREMENT, `sbasefileslocation` varchar(400) CHARACTER SET latin1 NOT NULL, `dbaseindirectpercentagedefault` decimal(20,5) NOT NULL, `dbasegainpercentagedefault` decimal(20,5) NOT NULL, `dbaseIVApercentage` decimal(20,5) NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL, PRIMARY KEY (`ibaseid`), KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"

                queriesCreation(2) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "IndirectCosts"
                queriesCreation(3) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "IndirectCosts" & " ( `ibaseid` int(11) NOT NULL, `icostid` int(11) NOT NULL, `sbasecostname` varchar(500) CHARACTER SET latin1 NOT NULL, `dbasecost` decimal(20,5) NOT NULL, `dcompanyprojectedearnings` decimal(20,5) NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL, PRIMARY KEY (`ibaseid`,`icostid`), KEY `baseid` (`ibaseid`), KEY `costid` (`icostid`), KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"

                queriesCreation(4) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Cards"
                queriesCreation(5) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Cards" & " ( `ibaseid` int(11) NOT NULL, `icardid` int(11) NOT NULL, `scardlegacycategoryid` varchar(10) CHARACTER SET latin1 NOT NULL, `scardlegacyid` varchar(10) CHARACTER SET latin1 NOT NULL, `scarddescription` varchar(1000) CHARACTER SET latin1 NOT NULL, `scardunit` varchar(50) CHARACTER SET latin1 NOT NULL, `dcardqty` decimal(20,5) NOT NULL, `dcardindirectcostspercentage` decimal(20,5) NOT NULL, `dcardgainpercentage` decimal(20,5) NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL, PRIMARY KEY (`ibaseid`,`icardid`), KEY `baseid` (`ibaseid`), KEY `cardid` (`icardid`), KEY `cardlegacycategoryid` (`scardlegacycategoryid`), KEY `cardlegacyid` (`scardlegacyid`), KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"

                queriesCreation(6) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardInputs"
                queriesCreation(7) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardInputs" & " ( `ibaseid` int(11) NOT NULL, `icardid` int(11) NOT NULL, `iinputid` int(11) NOT NULL, `scardinputunit` varchar(50) CHARACTER SET latin1 NOT NULL, `dcardinputqty` decimal(20,5) NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL, PRIMARY KEY (`ibaseid`,`icardid`,`iinputid`), KEY `baseid` (`ibaseid`), KEY `cardid` (`icardid`), KEY `inputid` (`iinputid`), KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"

                queriesCreation(8) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardCompoundInputs"
                queriesCreation(9) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardCompoundInputs" & " ( `ibaseid` int(11) NOT NULL, `icardid` int(11) NOT NULL, `iinputid` int(11) NOT NULL, `icompoundinputid` int(11) NOT NULL, `scompoundinputunit` varchar(50) CHARACTER SET latin1 NOT NULL, `dcompoundinputqty` decimal(20,5) NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL, PRIMARY KEY (`ibaseid`,`icardid`,`iinputid`,`icompoundinputid`), KEY `baseid` (`ibaseid`), KEY `cardid` (`icardid`), KEY `inputid` (`iinputid`), KEY `compoundinputid` (`icompoundinputid`), KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"

                queriesCreation(10) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Prices"
                queriesCreation(11) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Prices" & " ( `ibaseid` int(11) NOT NULL, `iinputid` int(11) NOT NULL, `dinputpricewithoutIVA` decimal(20,5) NOT NULL, `dinputprotectionpercentage` decimal(20,5) NOT NULL, `dinputfinalprice` decimal(20,5) NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL, PRIMARY KEY (`ibaseid`,`iinputid`,`iupdatedate`,`supdatetime`), KEY `baseid` (`ibaseid`), KEY `inputid` (`iinputid`), KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"

                queriesCreation(12) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Timber"
                queriesCreation(13) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Timber ( `ibaseid` int(11) NOT NULL, `iinputid` int(11) NOT NULL, `dinputtimberespesor` decimal(20,5) NOT NULL, `dinputtimberancho` decimal(20,5) NOT NULL, `dinputtimberlargo` decimal(20,5) NOT NULL, `dinputtimberpreciopiecubico` decimal(20,5) NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL, PRIMARY KEY (`ibaseid`,`iinputid`), KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"

                executeTransactedSQLCommand(0, queriesCreation)

                Dim queriesInsert(7) As String

                queriesInsert(0) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & " SELECT * FROM base WHERE ibaseid = " & baseid
                queriesInsert(1) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "IndirectCosts SELECT * FROM baseindirectcosts WHERE ibaseid = " & baseid
                queriesInsert(2) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Cards SELECT * FROM basecards WHERE ibaseid = " & baseid
                queriesInsert(3) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardInputs SELECT * FROM basecardinputs WHERE ibaseid = " & baseid
                queriesInsert(4) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardCompoundInputs SELECT * FROM basecardcompoundinputs WHERE ibaseid = " & baseid
                queriesInsert(5) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Prices SELECT * FROM baseprices WHERE ibaseid = " & baseid
                queriesInsert(6) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Timber SELECT * FROM basetimber WHERE ibaseid = " & baseid

                executeTransactedSQLCommand(0, queriesInsert)

            End If

        End If


        'Tab de Datos Iniciales


        Dim alertaPresupuestoBaseIncompleto As Boolean = False
        Dim mensajeAlerta As String = ""

        Dim dsBase As DataSet
        dsBase = getSQLQueryAsDataset(0, "SELECT b.ibaseid, b.iupdatedate, b.supdatetime, b.sbasefileslocation, " & _
        "b.dbaseindirectpercentagedefault, b.dbasegainpercentagedefault, b.dbaseIVApercentage " & _
        "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & " b " & _
        "WHERE ibaseid = " & iprojectid)

        Try

            If dsBase.Tables(0).Rows.Count > 0 Then

                dtFechaPresupuesto.Value = convertYYYYMMDDtoDDhyphenMMhyphenYYYY(dsBase.Tables(0).Rows(0).Item("iupdatedate")) & " " & dsBase.Tables(0).Rows(0).Item("supdatetime")

                Me.Text = "Presupuesto Base " & dtFechaPresupuesto.Value

                Dim porcentajeIVA As Double
                Try
                    porcentajeIVA = CDbl(dsBase.Tables(0).Rows(0).Item("dbaseIVApercentage"))
                Catch ex As Exception
                    porcentajeIVA = 0.0
                End Try

                txtPorcentajeIVA.Text = porcentajeIVA * 100

                Dim porcentajeIndirectos As Double = 0.0
                Dim porcentajeUtilidades As Double = 0.0

                Try
                    porcentajeIndirectos = CDbl(dsBase.Tables(0).Rows(0).Item("dbaseindirectpercentagedefault"))
                Catch ex As Exception
                    porcentajeIndirectos = 0.0
                End Try

                Try
                    porcentajeUtilidades = CDbl(dsBase.Tables(0).Rows(0).Item("dbasegainpercentagedefault"))
                Catch ex As Exception
                    porcentajeUtilidades = 0.0
                End Try

                txtPorcentajeIndirectosDefault.Text = FormatCurrency(porcentajeIndirectos * 100, 2, TriState.True, TriState.False, TriState.True).Replace("$", "")

                txtPorcentajeUtilidadDefault.Text = FormatCurrency(porcentajeUtilidades * 100, 2, TriState.True, TriState.False, TriState.True).Replace("$", "")

                tcBase.SelectedTab = tpCostosIndirectos

            End If

        Catch ex As Exception

        End Try



        Dim queryCostosIndirectosBase As String = ""
        queryCostosIndirectosBase = "SELECT bc.ibaseid, bc.icostid, bc.sbasecostname AS 'Nombre del Costo', " & _
        "FORMAT(bc.dbasecost, 2) AS 'Importe del Costo', bc.dcompanyprojectedearnings " & _
        "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "IndirectCosts bc " & _
        "WHERE bc.ibaseid = " & iprojectid

        setDataGridView(dgvCostosIndirectos, queryCostosIndirectosBase, False)

        dgvCostosIndirectos.Columns(0).Visible = False
        dgvCostosIndirectos.Columns(1).Visible = False
        dgvCostosIndirectos.Columns(4).Visible = False

        dgvCostosIndirectos.Columns(0).ReadOnly = True
        dgvCostosIndirectos.Columns(1).ReadOnly = True
        dgvCostosIndirectos.Columns(4).ReadOnly = True

        dgvCostosIndirectos.Columns(0).Width = 30
        dgvCostosIndirectos.Columns(1).Width = 30
        dgvCostosIndirectos.Columns(2).Width = 200
        dgvCostosIndirectos.Columns(3).Width = 100
        dgvCostosIndirectos.Columns(4).Width = 30


        Dim sumaCostosBase As Double = 0.0
        Dim querySumaCostosBase As String = ""
        querySumaCostosBase = "SELECT SUM(bc.dbasecost) AS dbasecost FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "IndirectCosts bc WHERE bc.ibaseid = " & iprojectid & " GROUP BY ibaseid"

        sumaCostosBase = getSQLQueryAsDouble(0, querySumaCostosBase)
        lblTotalIndirectos.Text = FormatCurrency(sumaCostosBase, 2, TriState.True, TriState.False, TriState.True)

        Dim ingresosIndirectos As Double = 0.0

        Try

            If dgvCostosIndirectos.RowCount > 0 Then
                ingresosIndirectos = CDbl(dgvCostosIndirectos.Rows(0).Cells(4).Value)
            End If

        Catch ex As Exception

        End Try

        txtIngresosIndirectos.Text = ingresosIndirectos

        Dim porcentajeIndirectoSugerido As Double = 0.0

        Try
            porcentajeIndirectoSugerido = sumaCostosBase / ingresosIndirectos
        Catch ex As Exception

        End Try

        lblPorcentajeIndirectos.Text = FormatCurrency(porcentajeIndirectoSugerido * 100, 2, TriState.True, TriState.False, TriState.True).Replace("$", "") & " % "

        If isHistoric = True Then

            dgvCostosIndirectos.ReadOnly = True
            txtIngresosIndirectos.Enabled = False

            dgvResumenDeTarjetas.ReadOnly = True
            txtPorcentajeIVA.Enabled = False

            btnGuardar.Enabled = False
            btnGuardarYCerrar.Enabled = False

        End If

        executeSQLCommand(0, "INSERT IGNORE INTO logs VALUES (" & fecha & ", '" & hora & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Abrió el Presupuesto Base " & iprojectid & ": " & dtFechaPresupuesto.Value & "', 'OK')")
        executeSQLCommand(0, "INSERT INTO recentlyopenedfiles VALUES ('" & susername & "', 'Base', 'Presupuesto Base', '" & iprojectid & "', '" & dtFechaPresupuesto.Value & "', 1, " & fecha & ", '" & hora & "', '" & susername & "')")

        isFormReadyForAction = True
        isResumenDGVReady = True

        txtIngresosIndirectos.Select()
        txtIngresosIndirectos.Focus()
        txtIngresosIndirectos.SelectionStart() = txtIngresosIndirectos.Text.Length

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


    Private Sub tcBase_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tcBase.SelectedIndexChanged

        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        If isFormReadyForAction = False Then
            Exit Sub
        End If

        If validaCostosIndirectos(True) = True And (tcBase.SelectedTab Is tpCostosIndirectos) = False Then

            'Continue
            If validaResumenDeTarjetas(True) = True Then
                'Continue
            Else
                tcBase.SelectedTab = tpResumenTarjetas
            End If

        Else

            tcBase.SelectedTab = tpCostosIndirectos

        End If

        executeSQLCommand(0, "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & " SET sprojectfileslocation = '', dbaseindirectpercentagedefault = " & txtPorcentajeIndirectosDefault.Text & ", dbasegainpercentagedefault = " & txtPorcentajeUtilidadDefault.Text & ", dbaseIVApercentage = " & txtPorcentajeIVA.Text & ", iupdatedate = " & getMySQLDate() & ", supdatetime = '" & getAppTime() & "', supdateusername = '" & susername & "' WHERE iprojectid = " & iprojectid)

        If tcBase.SelectedTab Is tpCostosIndirectos Then



            Dim queryCostosIndirectosBase As String = ""
            queryCostosIndirectosBase = "SELECT bc.ibaseid, bc.icostid, bc.sbasecostname AS 'Nombre del Costo', " & _
            "FORMAT(bc.dbasecost, 2) AS 'Importe del Costo', bc.dcompanyprojectedearnings " & _
            "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "IndirectCosts bc " & _
            "WHERE bc.ibaseid = " & iprojectid

            setDataGridView(dgvCostosIndirectos, queryCostosIndirectosBase, False)

            dgvCostosIndirectos.Columns(0).Visible = False
            dgvCostosIndirectos.Columns(1).Visible = False
            dgvCostosIndirectos.Columns(4).Visible = False

            dgvCostosIndirectos.Columns(0).ReadOnly = True
            dgvCostosIndirectos.Columns(1).ReadOnly = True
            dgvCostosIndirectos.Columns(4).ReadOnly = True

            dgvCostosIndirectos.Columns(0).Width = 30
            dgvCostosIndirectos.Columns(1).Width = 30
            dgvCostosIndirectos.Columns(2).Width = 200
            dgvCostosIndirectos.Columns(3).Width = 100
            dgvCostosIndirectos.Columns(4).Width = 30


            Dim sumaCostosBase As Double = 0.0
            Dim querySumaCostosBase As String = ""
            querySumaCostosBase = "SELECT SUM(bc.dbasecost) AS dbasecost FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "IndirectCosts bc WHERE bc.ibaseid = " & iprojectid & " GROUP BY ibaseid"

            sumaCostosBase = getSQLQueryAsDouble(0, querySumaCostosBase)
            lblTotalIndirectos.Text = FormatCurrency(sumaCostosBase, 2, TriState.True, TriState.False, TriState.True)

            Dim ingresosIndirectos As Double = 0.0

            Try

                If dgvCostosIndirectos.RowCount > 0 Then
                    ingresosIndirectos = CDbl(dgvCostosIndirectos.Rows(0).Cells(4).Value)
                End If

            Catch ex As Exception

            End Try

            txtIngresosIndirectos.Text = ingresosIndirectos

            Dim porcentajeIndirectoSugerido As Double = 0.0

            Try
                porcentajeIndirectoSugerido = sumaCostosBase / ingresosIndirectos
            Catch ex As Exception

            End Try



        ElseIf tcBase.SelectedTab Is tpResumenTarjetas Then



            Dim dsCategorias As DataSet
            Dim contadorCategorias As Integer = 0
            Dim resumenDeTarjetas As String = ""
            dsCategorias = getSQLQueryAsDataset(0, "SELECT scardlegacycategoryid, scardlegacycategorydescription FROM cardlegacycategories")
            contadorCategorias = dsCategorias.Tables(0).Rows.Count

            Dim queriesFill(contadorCategorias) As String

            Dim queriesCreation(2) As String

            queriesCreation(0) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardsAux"
            queriesCreation(1) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardsAux (   scardid varchar(11) COLLATE latin1_spanish_ci NOT NULL,   scardlegacyid varchar(510) CHARACTER SET latin1 NOT NULL,   scarddescription VARCHAR(1000) CHARACTER SET latin1 NOT NULL,   scardunit varchar(49) CHARACTER SET latin1 NOT NULL,   sbasecardqty varchar(20) COLLATE latin1_spanish_ci NOT NULL,   scardindirectcosts varchar(20) COLLATE latin1_spanish_ci NOT NULL,   scardgain varchar(20) COLLATE latin1_spanish_ci NOT NULL,   scardunitaryprice varchar(20) COLLATE latin1_spanish_ci NOT NULL,   scardamount varchar(20) COLLATE latin1_spanish_ci NOT NULL ) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"

            executeTransactedSQLCommand(0, queriesCreation)



            Try

                For i As Integer = 0 To contadorCategorias - 1

                    Dim queryContadorDeTarjetas As String = "" & _
                    "SELECT COUNT(*) " & _
                    "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Cards btf " & _
                    "JOIN cardlegacycategories btflc ON btf.scardlegacycategoryid = btflc.scardlegacycategoryid " & _
                    "JOIN (SELECT btfi.ibaseid, btfi.icardid, btfi.iinputid, (costoMO.costo*btfi.dcardinputqty) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardInputs btfi JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Cards btf ON btf.ibaseid = btfi.ibaseid AND btf.icardid = btfi.icardid JOIN (SELECT btfi.ibaseid, btfi.icardid AS icardid, 0 AS iinputid, SUM(btfi.dcardinputqty*bp.dinputfinalprice) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Cards btf LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardInputs btfi ON btf.ibaseid = btfi.ibaseid AND btf.icardid = btfi.icardid LEFT JOIN inputs i ON btfi.iinputid = i.iinputid JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) bp GROUP BY iinputid, ibaseid) bp ON btfi.ibaseid = bp.ibaseid AND i.iinputid = bp.iinputid LEFT JOIN inputtypes it ON i.iinputid = it.iinputid WHERE it.sinputtypedescription = 'MANO DE OBRA' GROUP BY btf.icardid, btfi.ibaseid ) AS costoMO ON btfi.iinputid = costoMO.iinputid AND btfi.icardid = costoMO.icardid GROUP BY btfi.icardid, btfi.ibaseid) AS costoEQ ON btf.ibaseid = costoEQ.ibaseid AND btf.icardid = costoEQ.icardid " & _
                    "JOIN (SELECT btfi.ibaseid, btfi.icardid, btfi.iinputid, SUM(btfi.dcardinputqty*bp.dinputfinalprice) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Cards btf LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardInputs btfi ON btf.ibaseid = btfi.ibaseid AND btf.icardid = btfi.icardid LEFT JOIN inputs i ON btfi.iinputid = i.iinputid JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) bp GROUP BY iinputid, ibaseid) bp ON btfi.ibaseid = bp.ibaseid AND i.iinputid = bp.iinputid JOIN inputtypes it ON i.iinputid = it.iinputid WHERE it.sinputtypedescription = 'MANO DE OBRA' GROUP BY btf.icardid, btfi.ibaseid) AS costoMO ON btf.ibaseid = costoMO.ibaseid AND btf.icardid = costoMO.icardid " & _
                    "LEFT JOIN (SELECT btfi.ibaseid, btfi.icardid, IF(SUM(btfi.dcardinputqty*bp.dinputfinalprice) IS NULL, 0, SUM(btfi.dcardinputqty*bp.dinputfinalprice))+IF(SUM(btfi.dcardinputqty*cibp.dinputfinalprice) IS NULL, 0, SUM(btfi.dcardinputqty*cibp.dinputfinalprice)) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Cards btf LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardInputs btfi ON btf.ibaseid = btfi.ibaseid AND btf.icardid = btfi.icardid LEFT JOIN inputs i ON btfi.iinputid = i.iinputid LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) bp GROUP BY iinputid, ibaseid) bp ON btfi.ibaseid = bp.ibaseid AND i.iinputid = bp.iinputid LEFT JOIN (SELECT btfi.ibaseid, btfi.icardid, btfi.iinputid, cibp.iupdatedate, cibp.supdatetime, SUM(btfci.dcompoundinputqty*cibp.dinputfinalprice) AS dinputpricewithoutIVA, 0.00000 AS dinputprotectionpercentage, SUM(btfci.dcompoundinputqty*cibp.dinputfinalprice) AS dinputfinalprice FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardInputs btfi JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardCompoundInputs btfci ON btfci.ibaseid = btfi.ibaseid AND btfci.icardid = btfi.icardid AND btfci.iinputid = btfi.iinputid LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) cibp GROUP BY iinputid, ibaseid) cibp ON cibp.ibaseid = btfci.ibaseid AND cibp.iinputid = btfci.icompoundinputid GROUP BY btfci.ibaseid, btfci.icardid, btfi.iinputid) cibp ON btfi.ibaseid = cibp.ibaseid AND btfi.icardid = cibp.icardid AND i.iinputid = cibp.iinputid JOIN inputtypes it ON i.iinputid = it.iinputid WHERE it.sinputtypedescription = 'MATERIALES' GROUP BY btfi.ibaseid, btfi.icardid ORDER BY btfi.ibaseid, btfi.icardid, btfi.iinputid) AS costoMAT ON btf.ibaseid = costoMAT.ibaseid AND btf.icardid = costoMAT.icardid " & _
                    "WHERE btf.ibaseid = " & iprojectid & " AND btflc.scardlegacycategoryid = '" & dsCategorias.Tables(0).Rows(i).Item("scardlegacycategoryid") & "' "

                    Dim contadorDeTarjetas As Integer = 0

                    contadorDeTarjetas = getSQLQueryAsInteger(0, queryContadorDeTarjetas)

                    If contadorDeTarjetas > 0 Then

                        queriesFill(i) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardsAux " & _
                        "SELECT '' AS 'icardid', CONCAT(btflc.scardlegacycategoryid, ' ', btflc.scardlegacycategorydescription) AS 'scardlegacyid', " & _
                        "'' AS 'scarddescription', '' AS 'scardunit', '' AS 'dcardqty', '' AS 'dcardindirectcosts', '' AS 'dcardgain', '' AS 'dcardunitaryprice', '' AS 'dcardsprice' FROM cardlegacycategories btflc WHERE btflc.scardlegacycategoryid = '" & dsCategorias.Tables(0).Rows(i).Item("scardlegacycategoryid") & "' " & _
                        "UNION SELECT btf.icardid, btf.scardlegacyid, btf.scarddescription, btf.scardunit, 1 AS dcardqty, " & _
                        "FORMAT(((IF(costoMAT.costo is null, 0, costoMAT.costo) + IF(costoMO.costo is null, 0, costoMO.costo) + IF(costoEQ.costo is null, 0, costoEQ.costo))*(btf.dcardindirectcostspercentage)*(1)), 2) AS dcardindirectcosts, " & _
                        "FORMAT(((IF(costoMAT.costo is null, 0, costoMAT.costo) + IF(costoMO.costo is null, 0, costoMO.costo) + IF(costoEQ.costo is null, 0, costoEQ.costo))*(1+btf.dcardindirectcostspercentage)*(btf.dcardgainpercentage)*(1)), 2) AS dcardgain, " & _
                        "FORMAT(((IF(costoMAT.costo is null, 0, costoMAT.costo) + IF(costoMO.costo is null, 0, costoMO.costo) + IF(costoEQ.costo is null, 0, costoEQ.costo))*(1+btf.dcardindirectcostspercentage)*(1+btf.dcardgainpercentage)), 2) AS dcardunitaryprice, " & _
                        "FORMAT(((IF(costoMAT.costo is null, 0, costoMAT.costo) + IF(costoMO.costo is null, 0, costoMO.costo) + IF(costoEQ.costo is null, 0, costoEQ.costo))*(1+btf.dcardindirectcostspercentage)*(1+btf.dcardgainpercentage)*(1)), 2) AS dcardsprice " & _
                        "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Cards btf " & _
                        "JOIN cardlegacycategories btflc ON btf.scardlegacycategoryid = btflc.scardlegacycategoryid " & _
                        "JOIN (SELECT btfi.ibaseid, btfi.icardid, btfi.iinputid, (costoMO.costo*btfi.dcardinputqty) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardInputs btfi JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Cards btf ON btf.ibaseid = btfi.ibaseid AND btf.icardid = btfi.icardid JOIN (SELECT btfi.ibaseid, btfi.icardid AS icardid, 0 AS iinputid, SUM(btfi.dcardinputqty*bp.dinputfinalprice) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Cards btf LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardInputs btfi ON btf.ibaseid = btfi.ibaseid AND btf.icardid = btfi.icardid LEFT JOIN inputs i ON btfi.iinputid = i.iinputid JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) bp GROUP BY iinputid, ibaseid) bp ON btfi.ibaseid = bp.ibaseid AND i.iinputid = bp.iinputid LEFT JOIN inputtypes it ON i.iinputid = it.iinputid WHERE it.sinputtypedescription = 'MANO DE OBRA' GROUP BY btf.icardid, btfi.ibaseid ) AS costoMO ON btfi.iinputid = costoMO.iinputid AND btfi.icardid = costoMO.icardid GROUP BY btfi.icardid, btfi.ibaseid) AS costoEQ ON btf.ibaseid = costoEQ.ibaseid AND btf.icardid = costoEQ.icardid " & _
                        "JOIN (SELECT btfi.ibaseid, btfi.icardid, btfi.iinputid, SUM(btfi.dcardinputqty*bp.dinputfinalprice) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Cards btf LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardInputs btfi ON btf.ibaseid = btfi.ibaseid AND btf.icardid = btfi.icardid LEFT JOIN inputs i ON btfi.iinputid = i.iinputid JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) bp GROUP BY iinputid, ibaseid) bp ON btfi.ibaseid = bp.ibaseid AND i.iinputid = bp.iinputid JOIN inputtypes it ON i.iinputid = it.iinputid WHERE it.sinputtypedescription = 'MANO DE OBRA' GROUP BY btf.icardid, btfi.ibaseid) AS costoMO ON btf.ibaseid = costoMO.ibaseid AND btf.icardid = costoMO.icardid " & _
                        "LEFT JOIN (SELECT btfi.ibaseid, btfi.icardid, IF(SUM(btfi.dcardinputqty*bp.dinputfinalprice) IS NULL, 0, SUM(btfi.dcardinputqty*bp.dinputfinalprice))+IF(SUM(btfi.dcardinputqty*cibp.dinputfinalprice) IS NULL, 0, SUM(btfi.dcardinputqty*cibp.dinputfinalprice)) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Cards btf LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardInputs btfi ON btf.ibaseid = btfi.ibaseid AND btf.icardid = btfi.icardid LEFT JOIN inputs i ON btfi.iinputid = i.iinputid LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) bp GROUP BY iinputid, ibaseid) bp ON btfi.ibaseid = bp.ibaseid AND i.iinputid = bp.iinputid LEFT JOIN (SELECT btfi.ibaseid, btfi.icardid, btfi.iinputid, cibp.iupdatedate, cibp.supdatetime, SUM(btfci.dcompoundinputqty*cibp.dinputfinalprice) AS dinputpricewithoutIVA, 0.00000 AS dinputprotectionpercentage, SUM(btfci.dcompoundinputqty*cibp.dinputfinalprice) AS dinputfinalprice FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardInputs btfi JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardCompoundInputs btfci ON btfci.ibaseid = btfi.ibaseid AND btfci.icardid = btfi.icardid AND btfci.iinputid = btfi.iinputid LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) cibp GROUP BY iinputid, ibaseid) cibp ON cibp.ibaseid = btfci.ibaseid AND cibp.iinputid = btfci.icompoundinputid GROUP BY btfci.ibaseid, btfci.icardid, btfi.iinputid) cibp ON btfi.ibaseid = cibp.ibaseid AND btfi.icardid = cibp.icardid AND i.iinputid = cibp.iinputid JOIN inputtypes it ON i.iinputid = it.iinputid WHERE it.sinputtypedescription = 'MATERIALES' GROUP BY btfi.ibaseid, btfi.icardid ORDER BY btfi.ibaseid, btfi.icardid, btfi.iinputid) AS costoMAT ON btf.ibaseid = costoMAT.ibaseid AND btf.icardid = costoMAT.icardid " & _
                        "WHERE btf.ibaseid = " & iprojectid & " AND btflc.scardlegacycategoryid = '" & dsCategorias.Tables(0).Rows(i).Item("scardlegacycategoryid") & "' ORDER BY scardlegacyid "

                    End If

                Next i

                executeTransactedSQLCommand(0, queriesFill)

            Catch ex As Exception

            End Try


            setDataGridView(dgvResumenDeTarjetas, "SELECT scardid, scardlegacyid AS 'ID', scarddescription AS 'Descripcion Tarjeta', scardunit AS 'Unidad de Medida', sbasecardqty AS 'Cantidad', scardindirectcosts AS 'Indirectos', scardgain AS 'Utilidad', scardunitaryprice AS 'P.U.', scardamount AS 'Importe' FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardsAux", False)

            dgvResumenDeTarjetas.Columns(0).Visible = False

            If viewDgvIndirectCosts = False Then
                dgvResumenDeTarjetas.Columns(5).Visible = False
            End If

            If viewDgvProfits = False Then
                dgvResumenDeTarjetas.Columns(6).Visible = False
            End If

            If viewDgvUnitPrices = False Then
                dgvResumenDeTarjetas.Columns(7).Visible = False
            End If

            If viewDgvAmount = False Then
                dgvResumenDeTarjetas.Columns(8).Visible = False
            End If

            dgvResumenDeTarjetas.Columns(0).ReadOnly = True
            dgvResumenDeTarjetas.Columns(1).ReadOnly = True
            dgvResumenDeTarjetas.Columns(2).ReadOnly = True
            dgvResumenDeTarjetas.Columns(3).ReadOnly = True
            dgvResumenDeTarjetas.Columns(4).ReadOnly = True
            dgvResumenDeTarjetas.Columns(5).ReadOnly = True
            dgvResumenDeTarjetas.Columns(6).ReadOnly = True
            dgvResumenDeTarjetas.Columns(7).ReadOnly = True
            dgvResumenDeTarjetas.Columns(8).ReadOnly = True

            dgvResumenDeTarjetas.Columns(0).Width = 30
            dgvResumenDeTarjetas.Columns(1).Width = 60
            dgvResumenDeTarjetas.Columns(2).Width = 200
            dgvResumenDeTarjetas.Columns(3).Width = 60
            dgvResumenDeTarjetas.Columns(4).Width = 70
            dgvResumenDeTarjetas.Columns(5).Width = 70
            dgvResumenDeTarjetas.Columns(6).Width = 70
            dgvResumenDeTarjetas.Columns(7).Width = 70
            dgvResumenDeTarjetas.Columns(8).Width = 70


            Dim dsBase As DataSet
            dsBase = getSQLQueryAsDataset(0, "SELECT b.ibaseid, b.iupdatedate, b.supdatetime, b.sbasefileslocation, " & _
            "b.dbaseindirectpercentagedefault, b.dbasegainpercentagedefault, b.dbaseIVApercentage " & _
            "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & " b " & _
            "WHERE ibaseid = " & iprojectid)

            Dim porcentajeIVA As Double = 0.0

            Try
                porcentajeIVA = CDbl(dsBase.Tables(0).Rows(0).Item("dbaseIVApercentage"))
            Catch ex As Exception
                porcentajeIVA = 0.0
            End Try

            txtPorcentajeIVA.Text = porcentajeIVA * 100

        End If


        If isHistoric = True Then

            dgvCostosIndirectos.ReadOnly = True
            txtIngresosIndirectos.Enabled = False

            dgvResumenDeTarjetas.ReadOnly = True
            txtPorcentajeIVA.Enabled = False

            btnGuardar.Enabled = False
            btnGuardarYCerrar.Enabled = False

        End If


        Cursor.Current = System.Windows.Forms.Cursors.Default


    End Sub


    Private Sub dgvCostosIndirectos_CellClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgvCostosIndirectos.CellClick

        Try

            If dgvCostosIndirectos.CurrentRow.IsNewRow Then
                Exit Sub
            End If

            iselectedcostid = CInt(dgvCostosIndirectos.Rows(e.RowIndex).Cells(1).Value())
            sselectedcostdescription = dgvCostosIndirectos.Rows(e.RowIndex).Cells(2).Value()
            dselectedcostamount = CDbl(dgvCostosIndirectos.Rows(e.RowIndex).Cells(3).Value())

        Catch ex As Exception

        End Try

    End Sub


    Private Sub dgvCostosIndirectos_CellContentClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgvCostosIndirectos.CellContentClick

        Try

            If dgvCostosIndirectos.CurrentRow.IsNewRow Then
                Exit Sub
            End If

            iselectedcostid = CInt(dgvCostosIndirectos.Rows(e.RowIndex).Cells(1).Value())
            sselectedcostdescription = dgvCostosIndirectos.Rows(e.RowIndex).Cells(2).Value()
            dselectedcostamount = CDbl(dgvCostosIndirectos.Rows(e.RowIndex).Cells(3).Value())

        Catch ex As Exception

        End Try

    End Sub


    Private Sub dgvCostosIndirectos_SelectionChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgvCostosIndirectos.SelectionChanged

        txtCantidadDgvCostosIndirectos = Nothing
        txtCantidadDgvCostosIndirectos_OldText = Nothing
        txtNombreDgvCostosIndirectos = Nothing
        txtNombreDgvCostosIndirectos_OldText = Nothing

        If isFormReadyForAction = False Then
            Exit Sub
        End If

        If dgvCostosIndirectos.Rows.Count > 0 Then

            If dgvCostosIndirectos.Rows.Count = 1 Then
                Exit Sub
            End If

            Try
                If dgvCostosIndirectos.CurrentRow.IsNewRow Then
                    Exit Sub
                End If
            Catch ex As Exception
                Exit Sub
            End Try

        End If

        Try
            iselectedcostid = CInt(dgvCostosIndirectos.CurrentRow.Cells(1).Value)
            sselectedcostdescription = dgvCostosIndirectos.CurrentRow.Cells(2).Value()
            dselectedcostamount = CDbl(dgvCostosIndirectos.CurrentRow.Cells(3).Value)
        Catch ex As Exception

        End Try

    End Sub


    Private Sub dgvCostosIndirectos_CellEndEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgvCostosIndirectos.CellEndEdit

        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        Dim queryCostosIndirectosBase As String = ""
        queryCostosIndirectosBase = "SELECT bc.ibaseid, bc.icostid, bc.sbasecostname AS 'Nombre del Costo', " & _
        "FORMAT(bc.dbasecost, 2) AS 'Importe del Costo', bc.dcompanyprojectedearnings " & _
        "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "IndirectCosts bc " & _
        "WHERE bc.ibaseid = " & iprojectid

        setDataGridView(dgvCostosIndirectos, queryCostosIndirectosBase, False)

        dgvCostosIndirectos.Columns(0).Visible = False
        dgvCostosIndirectos.Columns(1).Visible = False
        dgvCostosIndirectos.Columns(4).Visible = False

        dgvCostosIndirectos.Columns(0).ReadOnly = True
        dgvCostosIndirectos.Columns(1).ReadOnly = True
        dgvCostosIndirectos.Columns(4).ReadOnly = True

        dgvCostosIndirectos.Columns(0).Width = 30
        dgvCostosIndirectos.Columns(1).Width = 30
        dgvCostosIndirectos.Columns(2).Width = 200
        dgvCostosIndirectos.Columns(3).Width = 100
        dgvCostosIndirectos.Columns(4).Width = 30

        Cursor.Current = System.Windows.Forms.Cursors.Default

    End Sub


    Private Sub dgvCostosIndirectos_CellValueChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgvCostosIndirectos.CellValueChanged

        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        If modifyIndirectCosts = False Then
            Exit Sub
        End If

        'LAS UNICAS COLUMNAS EDITABLES SON LAS COLUMNAS 2 Y 3: sbasecostname y dbasecost

        If e.ColumnIndex = 2 Then

            If dgvCostosIndirectos.Rows(e.RowIndex).Cells(e.ColumnIndex).Value Is DBNull.Value Then

                If MsgBox("¿Estás seguro de que quieres eliminar este Costo Indirecto del Presupuesto Base?", MsgBoxStyle.YesNo, "Confirmación de Eliminación de Costo Indirecto del Presupuesto Base") = MsgBoxResult.Yes Then

                    executeSQLCommand(0, "DELETE FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "IndirectCosts WHERE ibaseid = " & iprojectid & " AND icostid = " & dgvCostosIndirectos.CurrentRow.Cells(1).Value)

                    Dim total As Double = 0.0

                    total = getSQLQueryAsDouble(0, "SELECT SUM(dbasecost) AS dbasecost FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "IndirectCosts WHERE ibaseid = " & iprojectid)

                    lblTotalIndirectos.Text = FormatCurrency(total, 2, TriState.True, TriState.False, TriState.True)

                    Dim ingresosIndirectos As Double = 0.0

                    Try

                        If dgvCostosIndirectos.RowCount > 0 Then
                            ingresosIndirectos = CDbl(dgvCostosIndirectos.Rows(0).Cells(4).Value)
                        End If

                    Catch ex As Exception

                    End Try

                    txtIngresosIndirectos.Text = ingresosIndirectos

                    Dim porcentajeIndirectoSugerido As Double = 0.0
                    Try
                        porcentajeIndirectoSugerido = total / ingresosIndirectos
                    Catch ex As Exception

                    End Try

                    lblPorcentajeIndirectos.Text = FormatCurrency(porcentajeIndirectoSugerido * 100, 2, TriState.True, TriState.False, TriState.True).Replace("$", "") & " % "

                Else

                    dgvCostosIndirectos.Rows(e.RowIndex).Cells(e.ColumnIndex).Value = sselectedcostdescription

                End If

            Else

                executeSQLCommand(0, "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "IndirectCosts SET sbasecostname = '" & dgvCostosIndirectos.Rows(e.RowIndex).Cells(e.ColumnIndex).Value & "', iupdatedate = " & getMySQLDate() & ", supdatetime = '" & getAppTime() & "', supdateusername = '" & susername & "' WHERE ibaseid = " & iprojectid & " AND icostid = " & dgvCostosIndirectos.CurrentRow.Cells(1).Value)

            End If

        ElseIf e.ColumnIndex = 3 Then

            If dgvCostosIndirectos.Rows(e.RowIndex).Cells(e.ColumnIndex).Value Is DBNull.Value Then

                If MsgBox("¿Estás seguro de que quieres eliminar este Costo Indirecto del Presupuesto Base?", MsgBoxStyle.YesNo, "Confirmación de Eliminación de Costo Indirecto del Presupuesto Base") = MsgBoxResult.Yes Then

                    executeSQLCommand(0, "DELETE FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "IndirectCosts WHERE ibaseid = " & iprojectid & " AND icostid = " & dgvCostosIndirectos.CurrentRow.Cells(1).Value)

                    Dim total As Double = 0.0

                    total = getSQLQueryAsDouble(0, "SELECT SUM(dbasecost) AS dbasecost FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "IndirectCosts WHERE ibaseid = " & iprojectid)

                    lblTotalIndirectos.Text = FormatCurrency(total, 2, TriState.True, TriState.False, TriState.True)

                    Dim ingresosIndirectos As Double = 0.0

                    Try

                        If dgvCostosIndirectos.RowCount > 0 Then
                            ingresosIndirectos = CDbl(dgvCostosIndirectos.Rows(0).Cells(4).Value)
                        End If

                    Catch ex As Exception

                    End Try

                    txtIngresosIndirectos.Text = ingresosIndirectos

                    Dim porcentajeIndirectoSugerido As Double = 0.0
                    Try
                        porcentajeIndirectoSugerido = total / ingresosIndirectos
                    Catch ex As Exception

                    End Try

                    lblPorcentajeIndirectos.Text = FormatCurrency(porcentajeIndirectoSugerido * 100, 2, TriState.True, TriState.False, TriState.True).Replace("$", "") & " % "

                Else

                    dgvCostosIndirectos.Rows(e.RowIndex).Cells(e.ColumnIndex).Value = dselectedcostamount

                End If

            ElseIf dgvCostosIndirectos.Rows(e.RowIndex).Cells(e.ColumnIndex).Value = 0 Then

                If MsgBox("¿Estás seguro de que quieres eliminar este Costo Indirecto del Presupuesto Base?", MsgBoxStyle.YesNo, "Confirmación de Eliminación de Costo Indirecto del Presupuesto Base") = MsgBoxResult.Yes Then

                    executeSQLCommand(0, "DELETE FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "IndirectCosts WHERE ibaseid = " & iprojectid & " AND icostid = " & dgvCostosIndirectos.CurrentRow.Cells(1).Value)

                    Dim total As Double = 0.0

                    total = getSQLQueryAsDouble(0, "SELECT SUM(dbasecost) AS dbasecost FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "IndirectCosts WHERE ibaseid = " & iprojectid)

                    lblTotalIndirectos.Text = FormatCurrency(total, 2, TriState.True, TriState.False, TriState.True)

                    Dim ingresosIndirectos As Double = 0.0

                    Try

                        If dgvCostosIndirectos.RowCount > 0 Then
                            ingresosIndirectos = CDbl(dgvCostosIndirectos.Rows(0).Cells(4).Value)
                        End If

                    Catch ex As Exception

                    End Try

                    txtIngresosIndirectos.Text = ingresosIndirectos

                    Dim porcentajeIndirectoSugerido As Double = 0.0
                    Try
                        porcentajeIndirectoSugerido = total / ingresosIndirectos
                    Catch ex As Exception

                    End Try

                    lblPorcentajeIndirectos.Text = FormatCurrency(porcentajeIndirectoSugerido * 100, 2, TriState.True, TriState.False, TriState.True).Replace("$", "") & " % "

                Else

                    dgvCostosIndirectos.Rows(e.RowIndex).Cells(e.ColumnIndex).Value = dselectedcostamount

                End If

            Else

                Dim costoindirecto As Double = 0.0

                Try
                    costoindirecto = CDbl(dgvCostosIndirectos.Rows(e.RowIndex).Cells(3).Value)
                Catch ex As Exception
                    costoindirecto = dselectedcostamount
                End Try

                executeSQLCommand(0, "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "IndirectCosts SET dbasecost = " & dgvCostosIndirectos.Rows(e.RowIndex).Cells(e.ColumnIndex).Value & ", iupdatedate = " & getMySQLDate() & ", supdatetime = '" & getAppTime() & "', supdateusername = '" & susername & "' WHERE ibaseid = " & iprojectid & " AND icostid = " & dgvCostosIndirectos.CurrentRow.Cells(1).Value)

                Dim total As Double = 0.0

                total = getSQLQueryAsDouble(0, "SELECT SUM(dbasecost) AS dbasecost FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "IndirectCosts WHERE ibaseid = " & iprojectid)

                lblTotalIndirectos.Text = FormatCurrency(total, 2, TriState.True, TriState.False, TriState.True)

                Dim ingresosIndirectos As Double = 0.0

                Try

                    If dgvCostosIndirectos.RowCount > 0 Then
                        ingresosIndirectos = CDbl(dgvCostosIndirectos.Rows(0).Cells(4).Value)
                    End If

                Catch ex As Exception

                End Try

                txtIngresosIndirectos.Text = ingresosIndirectos

                Dim porcentajeIndirectoSugerido As Double = 0.0

                Try
                    porcentajeIndirectoSugerido = total / ingresosIndirectos
                Catch ex As Exception

                End Try

                lblPorcentajeIndirectos.Text = FormatCurrency(porcentajeIndirectoSugerido * 100, 2, TriState.True, TriState.False, TriState.True).Replace("$", "") & " % "

            End If

        End If

        Cursor.Current = System.Windows.Forms.Cursors.Default

    End Sub


    Private Sub dgvCostosIndirectos_EditingControlShowing(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewEditingControlShowingEventArgs) Handles dgvCostosIndirectos.EditingControlShowing

        If dgvCostosIndirectos.CurrentCell.ColumnIndex = 2 Then

            txtNombreDgvCostosIndirectos = CType(e.Control, TextBox)
            txtNombreDgvCostosIndirectos_OldText = txtNombreDgvCostosIndirectos.Text

        ElseIf dgvCostosIndirectos.CurrentCell.ColumnIndex = 3 Then

            txtCantidadDgvCostosIndirectos = CType(e.Control, TextBox)
            txtCantidadDgvCostosIndirectos_OldText = txtCantidadDgvCostosIndirectos.Text

        Else
            txtCantidadDgvCostosIndirectos = Nothing
            txtCantidadDgvCostosIndirectos_OldText = Nothing
            txtNombreDgvCostosIndirectos = Nothing
            txtNombreDgvCostosIndirectos_OldText = Nothing
        End If

    End Sub


    Private Sub txtNombreDgvCostosIndirectos_KeyUp(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtNombreDgvCostosIndirectos.KeyUp

        txtNombreDgvCostosIndirectos.Text = txtNombreDgvCostosIndirectos.Text.Replace("'", "").Replace("--", "").Replace("@", "").Replace("|", "")

        Dim strForbidden1 As String = "|°!#$%&/()=?¡*¨[]_:;,-{}+´¿'¬^`~@\<>"
        Dim arrayForbidden1 As Char() = strForbidden1.ToCharArray

        For fc = 0 To arrayForbidden1.Length - 1

            If txtNombreDgvCostosIndirectos.Text.Contains(arrayForbidden1(fc)) Then
                txtNombreDgvCostosIndirectos.Text = txtNombreDgvCostosIndirectos.Text.Replace(arrayForbidden1(fc), "")
            End If

        Next fc

    End Sub


    Private Sub txtCantidadDgvCostosIndirectos_KeyUp(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtCantidadDgvCostosIndirectos.KeyUp

        If Not IsNumeric(txtCantidadDgvCostosIndirectos.Text) Then

            Dim strForbidden2 As String = "abcdefghijklmnopqrstuvwxyzñABCDEFGHIJKLMNOPQRSTUVWXYZÑ|°!#$%&/()=?¡*¨[]_:;,-{}+´¿'¬^`~@\<>"
            Dim arrayForbidden2 As Char() = strForbidden2.ToCharArray

            For cp = 0 To arrayForbidden2.Length - 1

                If txtCantidadDgvCostosIndirectos.Text.Contains(arrayForbidden2(cp)) Then
                    txtCantidadDgvCostosIndirectos.Text = txtCantidadDgvCostosIndirectos.Text.Replace(arrayForbidden2(cp), "")
                End If

            Next cp

            If txtCantidadDgvCostosIndirectos.Text.Contains(".") Then

                Dim comparaPuntos As Char() = txtCantidadDgvCostosIndirectos.Text.ToCharArray
                Dim cuantosPuntos As Integer = 0


                For letra = 0 To comparaPuntos.Length - 1

                    If comparaPuntos(letra) = "." Then
                        cuantosPuntos = cuantosPuntos + 1
                    End If

                Next

                If cuantosPuntos > 1 Then

                    For cantidad = 1 To cuantosPuntos
                        Dim lugar As Integer = txtCantidadDgvCostosIndirectos.Text.LastIndexOf(".")
                        Dim longitud As Integer = txtCantidadDgvCostosIndirectos.Text.Length

                        If longitud > (lugar + 1) Then
                            txtCantidadDgvCostosIndirectos.Text = txtCantidadDgvCostosIndirectos.Text.Substring(0, lugar) & txtCantidadDgvCostosIndirectos.Text.Substring(lugar + 1)
                            Exit For
                        Else
                            txtCantidadDgvCostosIndirectos.Text = txtCantidadDgvCostosIndirectos.Text.Substring(0, lugar)
                            Exit For
                        End If
                    Next

                End If

            End If

            txtCantidadDgvCostosIndirectos.Text = txtCantidadDgvCostosIndirectos.Text.Replace("--", "").Replace("'", "")
            txtCantidadDgvCostosIndirectos.Text = txtCantidadDgvCostosIndirectos.Text.Trim

        Else
            txtCantidadDgvCostosIndirectos_OldText = txtCantidadDgvCostosIndirectos.Text
        End If

    End Sub


    Private Sub dgvCostosIndirectos_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgvCostosIndirectos.KeyUp

        If e.KeyCode = Keys.Delete Then

            If deleteIndirectCosts = False Then
                Exit Sub
            End If

            If MsgBox("¿Estás seguro de que quieres eliminar este Costo Indirecto del Presupuesto Base?", MsgBoxStyle.YesNo, "Confirmación de Eliminación de Costo Indirecto del Presupuesto Base") = MsgBoxResult.Yes Then

                Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

                Dim tmpselectedcostid As Integer = 0
                Try
                    tmpselectedcostid = CInt(dgvCostosIndirectos.CurrentRow.Cells(1).Value)
                Catch ex As Exception

                End Try

                executeSQLCommand(0, "DELETE FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "IndirectCosts WHERE ibaseid = " & iprojectid & " AND icostid = " & tmpselectedcostid)

                Dim queryCostosIndirectosBase As String = ""
                queryCostosIndirectosBase = "SELECT bc.ibaseid, bc.icostid, bc.sbasecostname AS 'Nombre del Costo', " & _
                "FORMAT(bc.dbasecost, 2) AS 'Importe del Costo', bc.dcompanyprojectedearnings " & _
                "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "IndirectCosts bc " & _
                "WHERE bc.ibaseid = " & iprojectid

                setDataGridView(dgvCostosIndirectos, queryCostosIndirectosBase, False)

                dgvCostosIndirectos.Columns(0).Visible = False
                dgvCostosIndirectos.Columns(1).Visible = False
                dgvCostosIndirectos.Columns(4).Visible = False

                dgvCostosIndirectos.Columns(0).ReadOnly = True
                dgvCostosIndirectos.Columns(1).ReadOnly = True
                dgvCostosIndirectos.Columns(4).ReadOnly = True

                dgvCostosIndirectos.Columns(0).Width = 30
                dgvCostosIndirectos.Columns(1).Width = 30
                dgvCostosIndirectos.Columns(2).Width = 200
                dgvCostosIndirectos.Columns(3).Width = 100
                dgvCostosIndirectos.Columns(4).Width = 30

                Dim total As Double = 0.0
                total = getSQLQueryAsDouble(0, "SELECT SUM(dbasecost) AS dbasecost FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "IndirectCosts WHERE ibaseid = " & iprojectid)
                lblTotalIndirectos.Text = FormatCurrency(total, 2, TriState.True, TriState.False, TriState.True)

                Dim ingresosIndirectos As Double = 0.0

                Try

                    If dgvCostosIndirectos.RowCount > 0 Then
                        ingresosIndirectos = CDbl(dgvCostosIndirectos.Rows(0).Cells(4).Value)
                    End If

                Catch ex As Exception

                End Try

                txtIngresosIndirectos.Text = ingresosIndirectos

                Dim porcentajeIndirectoSugerido As Double = 0.0

                Try
                    porcentajeIndirectoSugerido = total / ingresosIndirectos
                Catch ex As Exception

                End Try

                lblPorcentajeIndirectos.Text = FormatCurrency(porcentajeIndirectoSugerido * 100, 2, TriState.True, TriState.False, TriState.True).Replace("$", "") & " % "

                Cursor.Current = System.Windows.Forms.Cursors.Default

            End If

        End If

    End Sub


    Private Sub dgvCostosIndirectos_UserAddedRow(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewRowEventArgs) Handles dgvCostosIndirectos.UserAddedRow

        If addIndirectCosts = False Then
            Exit Sub
        End If

        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        iselectedcostid = getSQLQueryAsInteger(0, "SELECT IF(MAX(icostid) + 1 IS NULL, 1, MAX(icostid) + 1) AS icostid FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "IndirectCosts WHERE ibaseid = " & iprojectid)

        dgvCostosIndirectos.CurrentRow.Cells(0).Value = iprojectid
        dgvCostosIndirectos.CurrentRow.Cells(1).Value = iselectedcostid
        dgvCostosIndirectos.CurrentRow.Cells(3).Value = 1

        Dim valor As Double = 0.0

        Try
            valor = CDbl(txtIngresosIndirectos.Text.Trim.Replace("--", "").Replace("'", "").Replace("@", ""))
        Catch ex As Exception

        End Try

        executeSQLCommand(0, "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "IndirectCosts VALUES (" & iprojectid & ", " & iselectedcostid & ", '" & dgvCostosIndirectos.CurrentRow.Cells(2).Value & "', " & dgvCostosIndirectos.CurrentRow.Cells(3).Value & ", " & valor & ", " & getMySQLDate() & ", '" & getAppTime() & "', '" & susername & "')")

        Dim total As Double = 0.0
        total = getSQLQueryAsDouble(0, "SELECT SUM(dbasecost) AS dbasecost FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "IndirectCosts WHERE ibaseid = " & iprojectid)
        lblTotalIndirectos.Text = FormatCurrency(total, 2, TriState.True, TriState.False, TriState.True)

        Dim ingresosIndirectos As Double = 0.0

        Try
            If dgvCostosIndirectos.RowCount > 0 Then
                ingresosIndirectos = CDbl(dgvCostosIndirectos.Rows(0).Cells(4).Value)
            End If
        Catch ex As Exception

        End Try

        txtIngresosIndirectos.Text = ingresosIndirectos

        Dim porcentajeIndirectoSugerido As Double = 0.0
        Try
            porcentajeIndirectoSugerido = total / ingresosIndirectos
        Catch ex As Exception

        End Try

        lblPorcentajeIndirectos.Text = FormatCurrency(porcentajeIndirectoSugerido * 100, 2, TriState.True, TriState.False, TriState.True).Replace("$", "") & " % "

        Cursor.Current = System.Windows.Forms.Cursors.Default

    End Sub


    Private Sub txtIngresosIndirectos_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtIngresosIndirectos.KeyUp

        Dim strcaracteresprohibidos As String = "abcdefghijklmnopqrstuvwxyzñABCDEFGHIJKLMNOPQRSTUVWXYZÑ|°!#$%&/()=?¡*¨[]_:;,-{}+´¿'¬^`~@\<> "
        Dim arrayCaractProhib As Char() = strcaracteresprohibidos.ToCharArray
        Dim resultado As Boolean = False

        For carp = 0 To arrayCaractProhib.Length - 1

            If txtIngresosIndirectos.Text.Contains(arrayCaractProhib(carp)) Then
                txtIngresosIndirectos.Text = txtIngresosIndirectos.Text.Replace(arrayCaractProhib(carp), "")
                resultado = True
            End If

        Next carp

        If txtIngresosIndirectos.Text.Contains(".") Then

            Dim comparaPuntos As Char() = txtIngresosIndirectos.Text.ToCharArray
            Dim cuantosPuntos As Integer = 0


            For letra = 0 To comparaPuntos.Length - 1

                If comparaPuntos(letra) = "." Then
                    cuantosPuntos = cuantosPuntos + 1
                End If

            Next

            If cuantosPuntos > 1 Then

                For cantidad = 1 To cuantosPuntos
                    Dim lugar As Integer = txtIngresosIndirectos.Text.LastIndexOf(".")
                    Dim longitud As Integer = txtIngresosIndirectos.Text.Length

                    If longitud > (lugar + 1) Then
                        txtIngresosIndirectos.Text = txtIngresosIndirectos.Text.Substring(0, lugar) & txtIngresosIndirectos.Text.Substring(lugar + 1)
                        resultado = True
                        Exit For
                    Else
                        txtIngresosIndirectos.Text = txtIngresosIndirectos.Text.Substring(0, lugar)
                        resultado = True
                        Exit For
                    End If
                Next

            End If

        End If

        If resultado = True Then
            txtIngresosIndirectos.Select(txtIngresosIndirectos.Text.Length, 0)
        End If

        txtIngresosIndirectos.Text = txtIngresosIndirectos.Text.Replace("--", "").Replace("'", "")
        txtIngresosIndirectos.Text = txtIngresosIndirectos.Text.Trim

    End Sub


    Private Sub txtIngresosIndirectos_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtIngresosIndirectos.TextChanged

        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        If isFormReadyForAction = False Then
            Exit Sub
        End If

        If modifyIndirectCosts = False Then
            Exit Sub
        End If

        Dim strcaracteresprohibidos As String = "abcdefghijklmnopqrstuvwxyzñABCDEFGHIJKLMNOPQRSTUVWXYZÑ|°!#$%&/()=?¡*¨[]_:;,-{}+´¿'¬^`~@\<> "
        Dim arrayCaractProhib As Char() = strcaracteresprohibidos.ToCharArray
        txtIngresosIndirectos.Text = txtIngresosIndirectos.Text.Trim(arrayCaractProhib)

        Dim valor As Double = 0.0
        Try
            valor = CDbl(txtIngresosIndirectos.Text.Trim.Replace("--", "").Replace("'", "").Replace("@", ""))
            executeSQLCommand(0, "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "IndirectCosts SET dcompanyprojectedearnings = " & valor & ", iupdatedate = " & getMySQLDate() & ", supdatetime = '" & getAppTime() & "', supdateusername = '" & susername & "' WHERE ibaseid = " & iprojectid)

            Dim queryCostosIndirectosBase As String = ""
            queryCostosIndirectosBase = "SELECT bc.ibaseid, bc.icostid, bc.sbasecostname AS 'Nombre del Costo', " & _
            "FORMAT(bc.dbasecost, 2) AS 'Importe del Costo', bc.dcompanyprojectedearnings " & _
            "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "IndirectCosts bc " & _
            "WHERE bc.ibaseid = " & iprojectid

            setDataGridView(dgvCostosIndirectos, queryCostosIndirectosBase, False)

            dgvCostosIndirectos.Columns(0).Visible = False
            dgvCostosIndirectos.Columns(1).Visible = False
            dgvCostosIndirectos.Columns(4).Visible = False

            dgvCostosIndirectos.Columns(0).ReadOnly = True
            dgvCostosIndirectos.Columns(1).ReadOnly = True
            dgvCostosIndirectos.Columns(4).ReadOnly = True

            dgvCostosIndirectos.Columns(0).Width = 30
            dgvCostosIndirectos.Columns(1).Width = 30
            dgvCostosIndirectos.Columns(2).Width = 200
            dgvCostosIndirectos.Columns(3).Width = 100
            dgvCostosIndirectos.Columns(4).Width = 30

            Dim total As Double = 0.0
            total = getSQLQueryAsDouble(0, "SELECT SUM(dbasecost) AS dbasecost FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "IndirectCosts WHERE ibaseid = " & iprojectid)
            lblTotalIndirectos.Text = FormatCurrency(total, 2, TriState.True, TriState.False, TriState.True)

            Dim ingresosIndirectos As Double = 0.0

            Try

                If dgvCostosIndirectos.RowCount > 0 Then
                    ingresosIndirectos = CDbl(dgvCostosIndirectos.Rows(0).Cells(4).Value)
                End If

            Catch ex As Exception

            End Try

            Dim porcentajeIndirectoSugerido As Double = 0.0

            Try
                porcentajeIndirectoSugerido = total / ingresosIndirectos
            Catch ex As Exception

            End Try

            lblPorcentajeIndirectos.Text = FormatCurrency(porcentajeIndirectoSugerido * 100, 2, TriState.True, TriState.False, TriState.True).Replace("$", "") & " % "

        Catch ex As Exception

        End Try

        Cursor.Current = System.Windows.Forms.Cursors.Default

    End Sub


    Private Sub btnNuevoCostoIndirecto_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNuevoCostoIndirecto.Click

        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        iselectedcostid = getSQLQueryAsInteger(0, "SELECT IF(MAX(icostid) + 1 IS NULL, 1, MAX(icostid) + 1) AS icostid FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "IndirectCosts WHERE ibaseid = " & iprojectid)

        Dim valor As Double = 0.0

        Try

            dgvCostosIndirectos.CurrentRow.Cells(0).Value = iprojectid
            dgvCostosIndirectos.CurrentRow.Cells(1).Value = iselectedcostid
            dgvCostosIndirectos.CurrentRow.Cells(3).Value = 1

            valor = CDbl(txtIngresosIndirectos.Text.Trim.Replace("--", "").Replace("'", "").Replace("@", ""))

            executeSQLCommand(0, "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "IndirectCosts VALUES (" & iprojectid & ", " & iselectedcostid & ", '" & dgvCostosIndirectos.CurrentRow.Cells(2).Value & "', " & dgvCostosIndirectos.CurrentRow.Cells(3).Value & ", " & valor & ", " & getMySQLDate() & ", '" & getAppTime() & "', '" & susername & "')")

        Catch ex As Exception

        End Try

        Dim queryCostosIndirectosBase As String = ""
        queryCostosIndirectosBase = "SELECT bc.ibaseid, bc.icostid, bc.sbasecostname AS 'Nombre del Costo', " & _
        "FORMAT(bc.dbasecost, 2) AS 'Importe del Costo', bc.dcompanyprojectedearnings " & _
        "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "IndirectCosts bc " & _
        "WHERE bc.ibaseid = " & iprojectid

        setDataGridView(dgvCostosIndirectos, queryCostosIndirectosBase, False)

        dgvCostosIndirectos.Columns(0).Visible = False
        dgvCostosIndirectos.Columns(1).Visible = False
        dgvCostosIndirectos.Columns(4).Visible = False

        dgvCostosIndirectos.Columns(0).ReadOnly = True
        dgvCostosIndirectos.Columns(1).ReadOnly = True
        dgvCostosIndirectos.Columns(4).ReadOnly = True

        dgvCostosIndirectos.Columns(0).Width = 30
        dgvCostosIndirectos.Columns(1).Width = 30
        dgvCostosIndirectos.Columns(2).Width = 200
        dgvCostosIndirectos.Columns(3).Width = 100
        dgvCostosIndirectos.Columns(4).Width = 30

        Dim total As Double = 0.0
        total = getSQLQueryAsDouble(0, "SELECT SUM(dbasecost) AS dbasecost FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "IndirectCosts WHERE ibaseid = " & iprojectid)
        lblTotalIndirectos.Text = FormatCurrency(total, 2, TriState.True, TriState.False, TriState.True)

        Dim ingresosIndirectos As Double = 0.0

        Try
            If dgvCostosIndirectos.RowCount > 0 Then
                ingresosIndirectos = CDbl(dgvCostosIndirectos.Rows(0).Cells(4).Value)
            End If
        Catch ex As Exception

        End Try

        txtIngresosIndirectos.Text = ingresosIndirectos

        Dim porcentajeIndirectoSugerido As Double = 0.0
        Try
            porcentajeIndirectoSugerido = total / ingresosIndirectos
        Catch ex As Exception

        End Try

        lblPorcentajeIndirectos.Text = FormatCurrency(porcentajeIndirectoSugerido * 100, 2, TriState.True, TriState.False, TriState.True).Replace("$", "") & " % "

        Cursor.Current = System.Windows.Forms.Cursors.Default


    End Sub


    Private Sub btnEliminarCostoIndirecto_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEliminarCostoIndirecto.Click

        If MsgBox("¿Estás seguro de que quieres eliminar este Costo Indirecto del Presupuesto Base?", MsgBoxStyle.YesNo, "Confirmación de Eliminación de Costo Indirecto del Presupuesto Base") = MsgBoxResult.Yes Then

            Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

            Dim tmpselectedcostid As Integer = 0
            Try
                tmpselectedcostid = CInt(dgvCostosIndirectos.CurrentRow.Cells(1).Value)
            Catch ex As Exception

            End Try

            executeSQLCommand(0, "DELETE FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "IndirectCosts WHERE ibaseid = " & iprojectid & " AND icostid = " & tmpselectedcostid)

            Dim queryCostosIndirectosBase As String = ""
            queryCostosIndirectosBase = "SELECT bc.ibaseid, bc.icostid, bc.sbasecostname AS 'Nombre del Costo', " & _
            "FORMAT(bc.dbasecost, 2) AS 'Importe del Costo', bc.dcompanyprojectedearnings " & _
            "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "IndirectCosts bc " & _
            "WHERE bc.ibaseid = " & iprojectid

            setDataGridView(dgvCostosIndirectos, queryCostosIndirectosBase, False)

            dgvCostosIndirectos.Columns(0).Visible = False
            dgvCostosIndirectos.Columns(1).Visible = False
            dgvCostosIndirectos.Columns(4).Visible = False

            dgvCostosIndirectos.Columns(0).ReadOnly = True
            dgvCostosIndirectos.Columns(1).ReadOnly = True
            dgvCostosIndirectos.Columns(4).ReadOnly = True

            dgvCostosIndirectos.Columns(0).Width = 30
            dgvCostosIndirectos.Columns(1).Width = 30
            dgvCostosIndirectos.Columns(2).Width = 200
            dgvCostosIndirectos.Columns(3).Width = 100
            dgvCostosIndirectos.Columns(4).Width = 30

            Dim total As Double = 0.0
            total = getSQLQueryAsDouble(0, "SELECT SUM(dbasecost) AS dbasecost FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "IndirectCosts WHERE ibaseid = " & iprojectid)
            lblTotalIndirectos.Text = FormatCurrency(total, 2, TriState.True, TriState.False, TriState.True)

            Dim ingresosIndirectos As Double = 0.0

            Try

                If dgvCostosIndirectos.RowCount > 0 Then
                    ingresosIndirectos = CDbl(dgvCostosIndirectos.Rows(0).Cells(4).Value)
                End If

            Catch ex As Exception

            End Try

            txtIngresosIndirectos.Text = ingresosIndirectos

            Dim porcentajeIndirectoSugerido As Double = 0.0

            Try
                porcentajeIndirectoSugerido = total / ingresosIndirectos
            Catch ex As Exception

            End Try

            lblPorcentajeIndirectos.Text = FormatCurrency(porcentajeIndirectoSugerido * 100, 2, TriState.True, TriState.False, TriState.True).Replace("$", "") & " % "

            Cursor.Current = System.Windows.Forms.Cursors.Default

        End If

    End Sub


    Private Function validaCostosIndirectos(ByVal silent As Boolean) As Boolean

        If isFormReadyForAction = False Then
            Exit Function
        End If

        Dim querySumaCostosBase As String = ""
        Dim sumaCostosBase As Double = 0.0

        querySumaCostosBase = "SELECT SUM(bc.dbasecost) AS dbasecost FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "IndirectCosts bc WHERE bc.ibaseid = " & iprojectid & " GROUP BY ibaseid"

        sumaCostosBase = getSQLQueryAsDouble(0, querySumaCostosBase)

        If sumaCostosBase = 0.0 Then

            If silent = False Then
                MsgBox("¿Podrías hacer el cálculo de costos indirectos del Presupuesto Base?", MsgBoxStyle.OkOnly, "Dato Faltante")
            End If

            alertaMostrada = True
            Return False

        End If

        If isEdit = False Then

            If paso3 = False Then

                Return False

            End If

        End If

        Return True


    End Function


    Private Sub btnPaso3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPaso3.Click

        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        paso3 = True

        If validaCostosIndirectos(False) = False Then

            MsgBox("¿Podrías ponerle costos indirectos al Presupuesto Base?", MsgBoxStyle.OkOnly, "Dato Faltante")

            paso3 = False

            Exit Sub

        Else

            tcBase.SelectedTab = tpResumenTarjetas

        End If

        Cursor.Current = System.Windows.Forms.Cursors.Default

    End Sub


    Private Sub dgvResumenDeTarjetas_CellClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgvResumenDeTarjetas.CellClick

        Try

            If dgvResumenDeTarjetas.CurrentRow.IsNewRow Then
                Exit Sub
            End If

            iselectedcardid = CInt(dgvResumenDeTarjetas.Rows(e.RowIndex).Cells(0).Value())
            sselectedcardlegacyid = dgvResumenDeTarjetas.Rows(e.RowIndex).Cells(1).Value()
            sselectedcarddescription = dgvResumenDeTarjetas.Rows(e.RowIndex).Cells(2).Value()
            dselectedcardqty = CDbl(dgvResumenDeTarjetas.Rows(e.RowIndex).Cells(4).Value())

        Catch ex As Exception

            iselectedcardid = 0
            sselectedcardlegacyid = ""
            sselectedcostdescription = ""
            dselectedcardqty = 1.0

        End Try

    End Sub


    Private Sub dgvResumenDeTarjetas_CellContentClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgvResumenDeTarjetas.CellContentClick

        Try

            If dgvResumenDeTarjetas.CurrentRow.IsNewRow Then
                Exit Sub
            End If

            iselectedcardid = CInt(dgvResumenDeTarjetas.Rows(e.RowIndex).Cells(0).Value())
            sselectedcardlegacyid = dgvResumenDeTarjetas.Rows(e.RowIndex).Cells(1).Value()
            sselectedcarddescription = dgvResumenDeTarjetas.Rows(e.RowIndex).Cells(2).Value()
            dselectedcardqty = CDbl(dgvResumenDeTarjetas.Rows(e.RowIndex).Cells(4).Value())

        Catch ex As Exception

            iselectedcardid = 0
            sselectedcardlegacyid = ""
            sselectedcostdescription = ""
            dselectedcardqty = 1.0

        End Try

    End Sub


    Private Sub dgvResumenDeTarjetas_SelectionChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgvResumenDeTarjetas.SelectionChanged

        txtLegacyIdDgvResumenDeTarjetas = Nothing
        txtLegacyIdDgvResumenDeTarjetas_OldText = Nothing
        txtNombreDgvResumenDeTarjetas = Nothing
        txtNombreDgvResumenDeTarjetas_OldText = Nothing
        txtCantidadDgvResumenDeTarjetas = Nothing
        txtCantidadDgvResumenDeTarjetas_OldText = Nothing

        If isFormReadyForAction = False Then
            Exit Sub
        End If

        Try

            If dgvResumenDeTarjetas.CurrentRow.IsNewRow Then
                Exit Sub
            End If

            iselectedcardid = CInt(dgvResumenDeTarjetas.CurrentRow.Cells(0).Value)
            sselectedcardlegacyid = dgvResumenDeTarjetas.CurrentRow.Cells(1).Value()
            sselectedcarddescription = dgvResumenDeTarjetas.CurrentRow.Cells(2).Value()
            dselectedcardqty = CDbl(dgvResumenDeTarjetas.CurrentRow.Cells(4).Value)

        Catch ex As Exception

            iselectedcardid = 0
            sselectedcardlegacyid = ""
            sselectedcostdescription = ""
            dselectedcardqty = 1.0

        End Try

    End Sub


    Private Sub dgvResumenDeTarjetas_CellValueChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgvResumenDeTarjetas.CellValueChanged

        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        'LAS UNICAS COLUMNAS EDITABLES SON LAS COLUMNAS 1, 2 Y 4: LEGACYID, SCARDDESCRIPTION Y DCARDQTY

        If isResumenDGVReady = False Then
            Exit Sub
        End If

        If modifyCardQty = False Then
            Exit Sub
        End If

        Dim dsBusquedaTarjetasRepetidas As DataSet

        Dim baseid As Integer = 0
        baseid = getSQLQueryAsInteger(0, "SELECT ibaseid FROM base ORDER BY iupdatedate DESC, supdatetime DESC LIMIT 1")

        If baseid = 0 Then
            baseid = 99999
        End If

        If e.ColumnIndex = 1 Then

            'SCardLegacyID

            If dgvResumenDeTarjetas.Rows(e.RowIndex).Cells(e.ColumnIndex).Value Is DBNull.Value Then

                dgvResumenDeTarjetas.Rows(e.RowIndex).Cells(e.ColumnIndex).Value = sselectedcardlegacyid

            Else

                'Si pone un texto, e.g. un LegacyID...

                dsBusquedaTarjetasRepetidas = getSQLQueryAsDataset(0, "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Cards WHERE ibaseid = " & iprojectid & " AND scardlegacyid = '" & dgvResumenDeTarjetas.Rows(e.RowIndex).Cells(e.ColumnIndex).Value & "'")

                If dsBusquedaTarjetasRepetidas.Tables(0).Rows.Count > 0 Then

                    MsgBox("Ya tienes esa Tarjeta insertada en este Presupuesto Base. ¿Podrías buscarla en la lista de Resumen de Tarjetas y cambiar la cantidad si así lo deseas?", MsgBoxStyle.OkOnly, "Tarjeta Repetida")
                    Exit Sub

                Else

                    Dim bf As New BuscaTarjetas
                    bf.susername = susername
                    bf.bactive = bactive
                    bf.bonline = bonline
                    bf.suserfullname = suserfullname

                    bf.suseremail = suseremail
                    bf.susersession = susersession
                    bf.susermachinename = susermachinename
                    bf.suserip = suserip

                    bf.iprojectid = iprojectid

                    bf.querystring = dgvResumenDeTarjetas.CurrentCell.EditedFormattedValue

                    bf.isEdit = False

                    bf.isBase = True
                    bf.isModel = False

                    bf.isHistoric = isHistoric

                    If Me.WindowState = FormWindowState.Maximized Then
                        bf.WindowState = FormWindowState.Maximized
                    End If

                    Me.Visible = False
                    bf.ShowDialog(Me)
                    Me.Visible = True

                    If bf.DialogResult = Windows.Forms.DialogResult.OK Then

                        dsBusquedaTarjetasRepetidas = getSQLQueryAsDataset(0, "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Cards WHERE ibaseid = " & iprojectid & " AND icardid = " & bf.icardid)

                        If dsBusquedaTarjetasRepetidas.Tables(0).Rows.Count > 0 Then

                            MsgBox("Ya tienes esa Tarjeta insertada en este Presupuesto Base. ¿Podrías buscarla en la lista de Resumen de Tarjetas y cambiar la cantidad si así lo deseas?", MsgBoxStyle.OkOnly, "Tarjeta Repetida")
                            Exit Sub

                        End If

                        Dim fecha As Integer = 0
                        Dim hora As String = "00:00:00"

                        fecha = getMySQLDate()
                        hora = getAppTime()

                        Dim queriesInsert(3) As String

                        queriesInsert(0) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardInputs SELECT " & iprojectid & ", " & iselectedcardid & ", iinputid, scardinputunit, dcardinputqty, " & fecha & ", '" & hora & "', '" & susername & "' FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardInputs WHERE ibaseid = " & baseid & " AND icardid = " & bf.icardid
                        queriesInsert(1) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardCompoundInputs SELECT " & iprojectid & ", " & iselectedcardid & ", iinputid, icompoundinputid, scompoundinputunit, dcompoundinputqty, " & fecha & ", '" & hora & "', '" & susername & "' FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardCompoundInputs WHERE ibaseid = " & baseid & " AND icardid = " & bf.icardid
                        queriesInsert(2) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Cards SELECT " & iprojectid & ", " & iselectedcardid & ", scardlegacycategoryid, scardlegacyid, scarddescription, scardunit, 1 AS dcardqty, dcardindirectcostspercentage, dcardgainpercentage, " & fecha & ", '" & hora & "', '" & susername & "' FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Cards WHERE ibaseid = " & baseid & " AND icardid = " & bf.icardid

                        executeTransactedSQLCommand(0, queriesInsert)

                        Dim porcentajeIVA As Double = 0.0
                        porcentajeIVA = getSQLQueryAsDouble(0, "SELECT dbaseIVApercentage FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & " WHERE ibaseid = " & iprojectid)
                        txtPorcentajeIVA.Text = porcentajeIVA * 100

                    Else

                        'Si cancela el ingreso de Tarjeta
                        dgvResumenDeTarjetas.Rows(e.RowIndex).Cells(e.ColumnIndex).Value = sselectedcardlegacyid

                    End If

                End If


            End If

            'ElseIf e.ColumnIndex = 2 Then

            '    'SCardDescription

            '    If dgvResumenDeTarjetas.Rows(e.RowIndex).Cells(e.ColumnIndex).Value Is DBNull.Value Then

            '        dgvResumenDeTarjetas.Rows(e.RowIndex).Cells(e.ColumnIndex).Value = sselectedcarddescription

            '    Else

            '        'Si pone un texto, e.g. una descripcion de otro producto

            '        dsBusquedaTarjetasRepetidas = getSQLQueryAsDataset(0, "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Cards WHERE ibaseid = " & iprojectid & " AND scardlegacyid = '" & dgvResumenDeTarjetas.Rows(e.RowIndex).Cells(e.ColumnIndex).Value & "'")

            '        If dsBusquedaTarjetasRepetidas.Tables(0).Rows.Count > 0 Then

            '            MsgBox("Ya tienes esa Tarjeta insertada en este Presupuesto Base. ¿Podrías buscarla en la lista de Resumen de Tarjetas y cambiar la cantidad si así lo deseas?", MsgBoxStyle.OkOnly, "Tarjeta Repetida")
            '            Exit Sub

            '        Else

            '            Dim bf As New BuscaTarjetas
            '            bf.susername = susername
            '            bf.bactive = bactive
            '            bf.bonline = bonline
            '            bf.suserfullname = suserfullname

            '            bf.suseremail = suseremail
            '            bf.susersession = susersession
            '            bf.susermachinename = susermachinename
            '            bf.suserip = suserip

            '            bf.iprojectid = iprojectid

            '            bf.querystring = dgvResumenDeTarjetas.CurrentCell.EditedFormattedValue

            '            bf.isEdit = False

            '            bf.isBase = True
            '            bf.isModel = False

            '            bf.isHistoric = isHistoric

            'If Me.WindowState = FormWindowState.Maximized Then
            '    bf.WindowState = FormWindowState.Maximized
            'End If
            '            Me.Visible = False
            '            bf.ShowDialog(Me)
            '            Me.Visible = True

            '            If bf.DialogResult = Windows.Forms.DialogResult.OK Then

            '                dsBusquedaTarjetasRepetidas = getSQLQueryAsDataset(0, "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Cards WHERE ibaseid = " & iprojectid & " AND icardid = " & bf.icardid)

            '                If dsBusquedaTarjetasRepetidas.Tables(0).Rows.Count > 0 Then

            '                    MsgBox("Ya tienes esa Tarjeta insertada en este Presupuesto Base. ¿Podrías buscarla en la lista de Resumen de Tarjetas y cambiar la cantidad si así lo deseas?", MsgBoxStyle.OkOnly, "Tarjeta Repetida")
            '                    Exit Sub

            '                End If

            '                Dim fecha As Integer = 0
            '                Dim hora As String = "00:00:00"

            '                fecha = getMySQLDate()
            '                hora = getAppTime()

            '                Dim queriesInsert(3) As String

            '                queriesInsert(0) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardInputs SELECT " & iprojectid & ", " & iselectedcardid & ", iinputid, scardinputunit, dcardinputqty, " & fecha & ", '" & hora & "', '" & susername & "' FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardInputs WHERE ibaseid = " & baseid & " AND icardid = " & bf.icardid
            '                queriesInsert(1) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardCompoundInputs SELECT " & iprojectid & ", " & iselectedcardid & ", iinputid, icompoundinputid, scompoundinputunit, dcompoundinputqty, " & fecha & ", '" & hora & "', '" & susername & "' FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardCompoundInputs WHERE ibaseid = " & baseid & " AND icardid = " & bf.icardid
            '                queriesInsert(2) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Cards SELECT " & iprojectid & ", " & iselectedcardid & ", scardlegacycategoryid, scardlegacyid, scarddescription, scardunit, 1 AS dcardqty, dcardindirectcostspercentage, dcardgainpercentage, " & fecha & ", '" & hora & "', '" & susername & "' FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Cards WHERE ibaseid = " & baseid & " AND icardid = " & bf.icardid

            '                executeTransactedSQLCommand(0, queriesInsert)

            '                Dim porcentajeIVA As Double = 0.0
            '                porcentajeIVA = getSQLQueryAsDouble(0, "SELECT dbaseIVApercentage FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & " WHERE ibaseid = " & iprojectid)
            '                txtPorcentajeIVA.Text = porcentajeIVA * 100


            '            Else

            '                'Si cancela el ingreso de Tarjeta
            '                dgvResumenDeTarjetas.Rows(e.RowIndex).Cells(e.ColumnIndex).Value = sselectedcarddescription

            '            End If

            '        End If

            '    End If

        ElseIf e.ColumnIndex = 4 Then

            'DCardQty

            If dgvResumenDeTarjetas.Rows(e.RowIndex).Cells(e.ColumnIndex).Value Is DBNull.Value Then

                dgvResumenDeTarjetas.Rows(e.RowIndex).Cells(e.ColumnIndex).Value = dselectedcardqty

            Else

                'Si pone un número

                Dim cantidaddeTarjeta As Double = 1.0

                Try
                    cantidaddeTarjeta = CDbl(dgvResumenDeTarjetas.Rows(e.RowIndex).Cells(4).Value)
                Catch ex As Exception
                    cantidaddeTarjeta = dselectedcardqty
                End Try

                executeSQLCommand(0, "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Cards SET dcardqty = " & dgvResumenDeTarjetas.Rows(e.RowIndex).Cells(e.ColumnIndex).Value & ", iupdatedate = " & getMySQLDate() & ", supdatetime = '" & getAppTime() & "', supdateusername = '" & susername & "' WHERE ibaseid = " & iprojectid & " AND icardid = " & dgvResumenDeTarjetas.CurrentRow.Cells(0).Value)

                Dim porcentajeIVA As Double = 0.0
                porcentajeIVA = getSQLQueryAsDouble(0, "SELECT dbaseIVApercentage FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & " WHERE ibaseid = " & iprojectid)
                txtPorcentajeIVA.Text = porcentajeIVA * 100

            End If

        End If

        Cursor.Current = System.Windows.Forms.Cursors.Default

    End Sub


    Private Sub dgvResumenDeTarjetas_EditingControlShowing(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewEditingControlShowingEventArgs) Handles dgvResumenDeTarjetas.EditingControlShowing

        If dgvResumenDeTarjetas.CurrentCell.ColumnIndex = 1 Then

            txtLegacyIdDgvResumenDeTarjetas = CType(e.Control, TextBox)
            txtLegacyIdDgvResumenDeTarjetas_OldText = txtLegacyIdDgvResumenDeTarjetas.Text

            'ElseIf dgvResumenDeTarjetas.CurrentCell.ColumnIndex = 2 Then

            '    txtNombreDgvResumenDeTarjetas = CType(e.Control, TextBox)
            '    txtNombreDgvResumenDeTarjetas_OldText = txtNombreDgvResumenDeTarjetas.Text

        ElseIf dgvResumenDeTarjetas.CurrentCell.ColumnIndex = 4 Then

            txtCantidadDgvResumenDeTarjetas = CType(e.Control, TextBox)
            txtCantidadDgvResumenDeTarjetas_OldText = txtCantidadDgvResumenDeTarjetas.Text

        Else

            txtLegacyIdDgvResumenDeTarjetas = Nothing
            txtLegacyIdDgvResumenDeTarjetas_OldText = Nothing
            txtNombreDgvResumenDeTarjetas = Nothing
            txtNombreDgvResumenDeTarjetas_OldText = Nothing
            txtCantidadDgvResumenDeTarjetas = Nothing
            txtCantidadDgvResumenDeTarjetas_OldText = Nothing

        End If

    End Sub


    Private Sub dgvResumenDeTarjetas_CellEndEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgvResumenDeTarjetas.CellEndEdit

        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        Dim dsCategorias As DataSet
        Dim contadorCategorias As Integer = 0
        Dim resumenDeTarjetas As String = ""
        dsCategorias = getSQLQueryAsDataset(0, "SELECT scardlegacycategoryid, scardlegacycategorydescription FROM cardlegacycategories")
        contadorCategorias = dsCategorias.Tables(0).Rows.Count

        Dim queriesFill(contadorCategorias) As String

        Dim queriesCreation(2) As String

        queriesCreation(0) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardsAux"
        queriesCreation(1) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardsAux (   scardid varchar(11) COLLATE latin1_spanish_ci NOT NULL,   scardlegacyid varchar(510) CHARACTER SET latin1 NOT NULL,   scarddescription VARCHAR(1000) CHARACTER SET latin1 NOT NULL,   scardunit varchar(49) CHARACTER SET latin1 NOT NULL,   sbasecardqty varchar(20) COLLATE latin1_spanish_ci NOT NULL,   scardindirectcosts varchar(20) COLLATE latin1_spanish_ci NOT NULL,   scardgain varchar(20) COLLATE latin1_spanish_ci NOT NULL,   scardunitaryprice varchar(20) COLLATE latin1_spanish_ci NOT NULL,   scardamount varchar(20) COLLATE latin1_spanish_ci NOT NULL ) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"

        executeTransactedSQLCommand(0, queriesCreation)



        Try

            For i As Integer = 0 To contadorCategorias - 1

                Dim queryContadorDeTarjetas As String = "" & _
                "SELECT COUNT(*) " & _
                "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Cards btf " & _
                "JOIN cardlegacycategories btflc ON btf.scardlegacycategoryid = btflc.scardlegacycategoryid " & _
                "JOIN (SELECT btfi.ibaseid, btfi.icardid, btfi.iinputid, (costoMO.costo*btfi.dcardinputqty) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardInputs btfi JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Cards btf ON btf.ibaseid = btfi.ibaseid AND btf.icardid = btfi.icardid JOIN (SELECT btfi.ibaseid, btfi.icardid AS icardid, 0 AS iinputid, SUM(btfi.dcardinputqty*bp.dinputfinalprice) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Cards btf LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardInputs btfi ON btf.ibaseid = btfi.ibaseid AND btf.icardid = btfi.icardid LEFT JOIN inputs i ON btfi.iinputid = i.iinputid JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) bp GROUP BY iinputid, ibaseid) bp ON btfi.ibaseid = bp.ibaseid AND i.iinputid = bp.iinputid LEFT JOIN inputtypes it ON i.iinputid = it.iinputid WHERE it.sinputtypedescription = 'MANO DE OBRA' GROUP BY btf.icardid, btfi.ibaseid ) AS costoMO ON btfi.iinputid = costoMO.iinputid AND btfi.icardid = costoMO.icardid GROUP BY btfi.icardid, btfi.ibaseid) AS costoEQ ON btf.ibaseid = costoEQ.ibaseid AND btf.icardid = costoEQ.icardid " & _
                "JOIN (SELECT btfi.ibaseid, btfi.icardid, btfi.iinputid, SUM(btfi.dcardinputqty*bp.dinputfinalprice) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Cards btf LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardInputs btfi ON btf.ibaseid = btfi.ibaseid AND btf.icardid = btfi.icardid LEFT JOIN inputs i ON btfi.iinputid = i.iinputid JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) bp GROUP BY iinputid, ibaseid) bp ON btfi.ibaseid = bp.ibaseid AND i.iinputid = bp.iinputid JOIN inputtypes it ON i.iinputid = it.iinputid WHERE it.sinputtypedescription = 'MANO DE OBRA' GROUP BY btf.icardid, btfi.ibaseid) AS costoMO ON btf.ibaseid = costoMO.ibaseid AND btf.icardid = costoMO.icardid " & _
                "LEFT JOIN (SELECT btfi.ibaseid, btfi.icardid, IF(SUM(btfi.dcardinputqty*bp.dinputfinalprice) IS NULL, 0, SUM(btfi.dcardinputqty*bp.dinputfinalprice))+IF(SUM(btfi.dcardinputqty*cibp.dinputfinalprice) IS NULL, 0, SUM(btfi.dcardinputqty*cibp.dinputfinalprice)) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Cards btf LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardInputs btfi ON btf.ibaseid = btfi.ibaseid AND btf.icardid = btfi.icardid LEFT JOIN inputs i ON btfi.iinputid = i.iinputid LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) bp GROUP BY iinputid, ibaseid) bp ON btfi.ibaseid = bp.ibaseid AND i.iinputid = bp.iinputid LEFT JOIN (SELECT btfi.ibaseid, btfi.icardid, btfi.iinputid, cibp.iupdatedate, cibp.supdatetime, SUM(btfci.dcompoundinputqty*cibp.dinputfinalprice) AS dinputpricewithoutIVA, 0.00000 AS dinputprotectionpercentage, SUM(btfci.dcompoundinputqty*cibp.dinputfinalprice) AS dinputfinalprice FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardInputs btfi JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardCompoundInputs btfci ON btfci.ibaseid = btfi.ibaseid AND btfci.icardid = btfi.icardid AND btfci.iinputid = btfi.iinputid LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) cibp GROUP BY iinputid, ibaseid) cibp ON cibp.ibaseid = btfci.ibaseid AND cibp.iinputid = btfci.icompoundinputid GROUP BY btfci.ibaseid, btfci.icardid, btfi.iinputid) cibp ON btfi.ibaseid = cibp.ibaseid AND btfi.icardid = cibp.icardid AND i.iinputid = cibp.iinputid JOIN inputtypes it ON i.iinputid = it.iinputid WHERE it.sinputtypedescription = 'MATERIALES' GROUP BY btfi.ibaseid, btfi.icardid ORDER BY btfi.ibaseid, btfi.icardid, btfi.iinputid) AS costoMAT ON btf.ibaseid = costoMAT.ibaseid AND btf.icardid = costoMAT.icardid " & _
                "WHERE btf.ibaseid = " & iprojectid & " AND btflc.scardlegacycategoryid = '" & dsCategorias.Tables(0).Rows(i).Item("scardlegacycategoryid") & "' "

                Dim contadorDeTarjetas As Integer = 0

                contadorDeTarjetas = getSQLQueryAsInteger(0, queryContadorDeTarjetas)

                If contadorDeTarjetas > 0 Then

                    queriesFill(i) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardsAux " & _
                    "SELECT '' AS 'icardid', CONCAT(btflc.scardlegacycategoryid, ' ', btflc.scardlegacycategorydescription) AS 'scardlegacyid', " & _
                    "'' AS 'scarddescription', '' AS 'scardunit', '' AS 'dcardqty', '' AS 'dcardindirectcosts', '' AS 'dcardgain', '' AS 'dcardunitaryprice', '' AS 'dcardsprice' FROM cardlegacycategories btflc WHERE btflc.scardlegacycategoryid = '" & dsCategorias.Tables(0).Rows(i).Item("scardlegacycategoryid") & "' " & _
                    "UNION SELECT btf.icardid, btf.scardlegacyid, btf.scarddescription, btf.scardunit, 1 AS dcardqty, " & _
                    "FORMAT(((IF(costoMAT.costo is null, 0, costoMAT.costo) + IF(costoMO.costo is null, 0, costoMO.costo) + IF(costoEQ.costo is null, 0, costoEQ.costo))*(btf.dcardindirectcostspercentage)*(1)), 2) AS dcardindirectcosts, " & _
                    "FORMAT(((IF(costoMAT.costo is null, 0, costoMAT.costo) + IF(costoMO.costo is null, 0, costoMO.costo) + IF(costoEQ.costo is null, 0, costoEQ.costo))*(1+btf.dcardindirectcostspercentage)*(btf.dcardgainpercentage)*(1)), 2) AS dcardgain, " & _
                    "FORMAT(((IF(costoMAT.costo is null, 0, costoMAT.costo) + IF(costoMO.costo is null, 0, costoMO.costo) + IF(costoEQ.costo is null, 0, costoEQ.costo))*(1+btf.dcardindirectcostspercentage)*(1+btf.dcardgainpercentage)), 2) AS dcardunitaryprice, " & _
                    "FORMAT(((IF(costoMAT.costo is null, 0, costoMAT.costo) + IF(costoMO.costo is null, 0, costoMO.costo) + IF(costoEQ.costo is null, 0, costoEQ.costo))*(1+btf.dcardindirectcostspercentage)*(1+btf.dcardgainpercentage)*(1)), 2) AS dcardsprice " & _
                    "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Cards btf " & _
                    "JOIN cardlegacycategories btflc ON btf.scardlegacycategoryid = btflc.scardlegacycategoryid " & _
                    "JOIN (SELECT btfi.ibaseid, btfi.icardid, btfi.iinputid, (costoMO.costo*btfi.dcardinputqty) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardInputs btfi JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Cards btf ON btf.ibaseid = btfi.ibaseid AND btf.icardid = btfi.icardid JOIN (SELECT btfi.ibaseid, btfi.icardid AS icardid, 0 AS iinputid, SUM(btfi.dcardinputqty*bp.dinputfinalprice) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Cards btf LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardInputs btfi ON btf.ibaseid = btfi.ibaseid AND btf.icardid = btfi.icardid LEFT JOIN inputs i ON btfi.iinputid = i.iinputid JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) bp GROUP BY iinputid, ibaseid) bp ON btfi.ibaseid = bp.ibaseid AND i.iinputid = bp.iinputid LEFT JOIN inputtypes it ON i.iinputid = it.iinputid WHERE it.sinputtypedescription = 'MANO DE OBRA' GROUP BY btf.icardid, btfi.ibaseid ) AS costoMO ON btfi.iinputid = costoMO.iinputid AND btfi.icardid = costoMO.icardid GROUP BY btfi.icardid, btfi.ibaseid) AS costoEQ ON btf.ibaseid = costoEQ.ibaseid AND btf.icardid = costoEQ.icardid " & _
                    "JOIN (SELECT btfi.ibaseid, btfi.icardid, btfi.iinputid, SUM(btfi.dcardinputqty*bp.dinputfinalprice) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Cards btf LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardInputs btfi ON btf.ibaseid = btfi.ibaseid AND btf.icardid = btfi.icardid LEFT JOIN inputs i ON btfi.iinputid = i.iinputid JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) bp GROUP BY iinputid, ibaseid) bp ON btfi.ibaseid = bp.ibaseid AND i.iinputid = bp.iinputid JOIN inputtypes it ON i.iinputid = it.iinputid WHERE it.sinputtypedescription = 'MANO DE OBRA' GROUP BY btf.icardid, btfi.ibaseid) AS costoMO ON btf.ibaseid = costoMO.ibaseid AND btf.icardid = costoMO.icardid " & _
                    "LEFT JOIN (SELECT btfi.ibaseid, btfi.icardid, IF(SUM(btfi.dcardinputqty*bp.dinputfinalprice) IS NULL, 0, SUM(btfi.dcardinputqty*bp.dinputfinalprice))+IF(SUM(btfi.dcardinputqty*cibp.dinputfinalprice) IS NULL, 0, SUM(btfi.dcardinputqty*cibp.dinputfinalprice)) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Cards btf LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardInputs btfi ON btf.ibaseid = btfi.ibaseid AND btf.icardid = btfi.icardid LEFT JOIN inputs i ON btfi.iinputid = i.iinputid LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) bp GROUP BY iinputid, ibaseid) bp ON btfi.ibaseid = bp.ibaseid AND i.iinputid = bp.iinputid LEFT JOIN (SELECT btfi.ibaseid, btfi.icardid, btfi.iinputid, cibp.iupdatedate, cibp.supdatetime, SUM(btfci.dcompoundinputqty*cibp.dinputfinalprice) AS dinputpricewithoutIVA, 0.00000 AS dinputprotectionpercentage, SUM(btfci.dcompoundinputqty*cibp.dinputfinalprice) AS dinputfinalprice FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardInputs btfi JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardCompoundInputs btfci ON btfci.ibaseid = btfi.ibaseid AND btfci.icardid = btfi.icardid AND btfci.iinputid = btfi.iinputid LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) cibp GROUP BY iinputid, ibaseid) cibp ON cibp.ibaseid = btfci.ibaseid AND cibp.iinputid = btfci.icompoundinputid GROUP BY btfci.ibaseid, btfci.icardid, btfi.iinputid) cibp ON btfi.ibaseid = cibp.ibaseid AND btfi.icardid = cibp.icardid AND i.iinputid = cibp.iinputid JOIN inputtypes it ON i.iinputid = it.iinputid WHERE it.sinputtypedescription = 'MATERIALES' GROUP BY btfi.ibaseid, btfi.icardid ORDER BY btfi.ibaseid, btfi.icardid, btfi.iinputid) AS costoMAT ON btf.ibaseid = costoMAT.ibaseid AND btf.icardid = costoMAT.icardid " & _
                    "WHERE btf.ibaseid = " & iprojectid & " AND btflc.scardlegacycategoryid = '" & dsCategorias.Tables(0).Rows(i).Item("scardlegacycategoryid") & "' ORDER BY scardlegacyid "

                End If

            Next i

            executeTransactedSQLCommand(0, queriesFill)

        Catch ex As Exception

        End Try


        setDataGridView(dgvResumenDeTarjetas, "SELECT scardid, scardlegacyid AS 'ID', scarddescription AS 'Descripcion Tarjeta', scardunit AS 'Unidad de Medida', sbasecardqty AS 'Cantidad', scardindirectcosts AS 'Indirectos', scardgain AS 'Utilidad', scardunitaryprice AS 'P.U.', scardamount AS 'Importe' FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardsAux", False)

        dgvResumenDeTarjetas.Columns(0).Visible = False

        If viewDgvIndirectCosts = False Then
            dgvResumenDeTarjetas.Columns(5).Visible = False
        End If

        If viewDgvProfits = False Then
            dgvResumenDeTarjetas.Columns(6).Visible = False
        End If

        If viewDgvUnitPrices = False Then
            dgvResumenDeTarjetas.Columns(7).Visible = False
        End If

        If viewDgvAmount = False Then
            dgvResumenDeTarjetas.Columns(8).Visible = False
        End If

        dgvResumenDeTarjetas.Columns(0).ReadOnly = True
        dgvResumenDeTarjetas.Columns(1).ReadOnly = True
        dgvResumenDeTarjetas.Columns(2).ReadOnly = True
        dgvResumenDeTarjetas.Columns(3).ReadOnly = True
        dgvResumenDeTarjetas.Columns(4).ReadOnly = True
        dgvResumenDeTarjetas.Columns(5).ReadOnly = True
        dgvResumenDeTarjetas.Columns(6).ReadOnly = True
        dgvResumenDeTarjetas.Columns(7).ReadOnly = True
        dgvResumenDeTarjetas.Columns(8).ReadOnly = True

        dgvResumenDeTarjetas.Columns(0).Width = 30
        dgvResumenDeTarjetas.Columns(1).Width = 60
        dgvResumenDeTarjetas.Columns(2).Width = 200
        dgvResumenDeTarjetas.Columns(3).Width = 60
        dgvResumenDeTarjetas.Columns(4).Width = 70
        dgvResumenDeTarjetas.Columns(5).Width = 70
        dgvResumenDeTarjetas.Columns(6).Width = 70
        dgvResumenDeTarjetas.Columns(7).Width = 70
        dgvResumenDeTarjetas.Columns(8).Width = 70


        Cursor.Current = System.Windows.Forms.Cursors.Default

    End Sub


    Private Sub dgvResumenDeTarjetas_CellDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgvResumenDeTarjetas.CellDoubleClick

        If openCards = False Then
            Exit Sub
        End If

        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        Try

            If dgvResumenDeTarjetas.CurrentRow.IsNewRow Then
                Exit Sub
            End If

            iselectedcardid = CInt(dgvResumenDeTarjetas.Rows(e.RowIndex).Cells(0).Value())
            sselectedcardlegacyid = dgvResumenDeTarjetas.Rows(e.RowIndex).Cells(1).Value()
            sselectedcarddescription = dgvResumenDeTarjetas.Rows(e.RowIndex).Cells(2).Value()
            dselectedcardqty = CDbl(dgvResumenDeTarjetas.Rows(e.RowIndex).Cells(4).Value())

        Catch ex As Exception

            iselectedcardid = 0
            sselectedcardlegacyid = ""
            sselectedcostdescription = ""
            dselectedcardqty = 1.0

        End Try

        If iselectedcardid = 0 Then
            Exit Sub
        End If

        Dim ag As New AgregarTarjeta
        ag.susername = susername
        ag.bactive = bactive
        ag.bonline = bonline
        ag.suserfullname = suserfullname

        ag.suseremail = suseremail
        ag.susersession = susersession
        ag.susermachinename = susermachinename
        ag.suserip = suserip

        ag.IsBase = True
        ag.IsModel = False

        ag.IsEdit = True
        ag.IsHistoric = isHistoric

        ag.iprojectid = iprojectid
        ag.icardid = iselectedcardid

        If Me.WindowState = FormWindowState.Maximized Then
            ag.WindowState = FormWindowState.Maximized
        End If

        Me.Visible = False
        ag.ShowDialog(Me)
        Me.Visible = True

        If ag.DialogResult = Windows.Forms.DialogResult.OK Then

            Dim dsCategorias As DataSet
            Dim contadorCategorias As Integer = 0
            Dim resumenDeTarjetas As String = ""
            dsCategorias = getSQLQueryAsDataset(0, "SELECT scardlegacycategoryid, scardlegacycategorydescription FROM cardlegacycategories")
            contadorCategorias = dsCategorias.Tables(0).Rows.Count

            Dim queriesFill(contadorCategorias) As String

            Dim queriesCreation(2) As String

            queriesCreation(0) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardsAux"
            queriesCreation(1) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardsAux (   scardid varchar(11) COLLATE latin1_spanish_ci NOT NULL,   scardlegacyid varchar(510) CHARACTER SET latin1 NOT NULL,   scarddescription VARCHAR(1000) CHARACTER SET latin1 NOT NULL,   scardunit varchar(49) CHARACTER SET latin1 NOT NULL,   sbasecardqty varchar(20) COLLATE latin1_spanish_ci NOT NULL,   scardindirectcosts varchar(20) COLLATE latin1_spanish_ci NOT NULL,   scardgain varchar(20) COLLATE latin1_spanish_ci NOT NULL,   scardunitaryprice varchar(20) COLLATE latin1_spanish_ci NOT NULL,   scardamount varchar(20) COLLATE latin1_spanish_ci NOT NULL ) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"

            executeTransactedSQLCommand(0, queriesCreation)



            Try

                For i As Integer = 0 To contadorCategorias - 1

                    Dim queryContadorDeTarjetas As String = "" & _
                    "SELECT COUNT(*) " & _
                    "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Cards btf " & _
                    "JOIN cardlegacycategories btflc ON btf.scardlegacycategoryid = btflc.scardlegacycategoryid " & _
                    "JOIN (SELECT btfi.ibaseid, btfi.icardid, btfi.iinputid, (costoMO.costo*btfi.dcardinputqty) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardInputs btfi JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Cards btf ON btf.ibaseid = btfi.ibaseid AND btf.icardid = btfi.icardid JOIN (SELECT btfi.ibaseid, btfi.icardid AS icardid, 0 AS iinputid, SUM(btfi.dcardinputqty*bp.dinputfinalprice) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Cards btf LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardInputs btfi ON btf.ibaseid = btfi.ibaseid AND btf.icardid = btfi.icardid LEFT JOIN inputs i ON btfi.iinputid = i.iinputid JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) bp GROUP BY iinputid, ibaseid) bp ON btfi.ibaseid = bp.ibaseid AND i.iinputid = bp.iinputid LEFT JOIN inputtypes it ON i.iinputid = it.iinputid WHERE it.sinputtypedescription = 'MANO DE OBRA' GROUP BY btf.icardid, btfi.ibaseid ) AS costoMO ON btfi.iinputid = costoMO.iinputid AND btfi.icardid = costoMO.icardid GROUP BY btfi.icardid, btfi.ibaseid) AS costoEQ ON btf.ibaseid = costoEQ.ibaseid AND btf.icardid = costoEQ.icardid " & _
                    "JOIN (SELECT btfi.ibaseid, btfi.icardid, btfi.iinputid, SUM(btfi.dcardinputqty*bp.dinputfinalprice) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Cards btf LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardInputs btfi ON btf.ibaseid = btfi.ibaseid AND btf.icardid = btfi.icardid LEFT JOIN inputs i ON btfi.iinputid = i.iinputid JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) bp GROUP BY iinputid, ibaseid) bp ON btfi.ibaseid = bp.ibaseid AND i.iinputid = bp.iinputid JOIN inputtypes it ON i.iinputid = it.iinputid WHERE it.sinputtypedescription = 'MANO DE OBRA' GROUP BY btf.icardid, btfi.ibaseid) AS costoMO ON btf.ibaseid = costoMO.ibaseid AND btf.icardid = costoMO.icardid " & _
                    "LEFT JOIN (SELECT btfi.ibaseid, btfi.icardid, IF(SUM(btfi.dcardinputqty*bp.dinputfinalprice) IS NULL, 0, SUM(btfi.dcardinputqty*bp.dinputfinalprice))+IF(SUM(btfi.dcardinputqty*cibp.dinputfinalprice) IS NULL, 0, SUM(btfi.dcardinputqty*cibp.dinputfinalprice)) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Cards btf LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardInputs btfi ON btf.ibaseid = btfi.ibaseid AND btf.icardid = btfi.icardid LEFT JOIN inputs i ON btfi.iinputid = i.iinputid LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) bp GROUP BY iinputid, ibaseid) bp ON btfi.ibaseid = bp.ibaseid AND i.iinputid = bp.iinputid LEFT JOIN (SELECT btfi.ibaseid, btfi.icardid, btfi.iinputid, cibp.iupdatedate, cibp.supdatetime, SUM(btfci.dcompoundinputqty*cibp.dinputfinalprice) AS dinputpricewithoutIVA, 0.00000 AS dinputprotectionpercentage, SUM(btfci.dcompoundinputqty*cibp.dinputfinalprice) AS dinputfinalprice FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardInputs btfi JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardCompoundInputs btfci ON btfci.ibaseid = btfi.ibaseid AND btfci.icardid = btfi.icardid AND btfci.iinputid = btfi.iinputid LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) cibp GROUP BY iinputid, ibaseid) cibp ON cibp.ibaseid = btfci.ibaseid AND cibp.iinputid = btfci.icompoundinputid GROUP BY btfci.ibaseid, btfci.icardid, btfi.iinputid) cibp ON btfi.ibaseid = cibp.ibaseid AND btfi.icardid = cibp.icardid AND i.iinputid = cibp.iinputid JOIN inputtypes it ON i.iinputid = it.iinputid WHERE it.sinputtypedescription = 'MATERIALES' GROUP BY btfi.ibaseid, btfi.icardid ORDER BY btfi.ibaseid, btfi.icardid, btfi.iinputid) AS costoMAT ON btf.ibaseid = costoMAT.ibaseid AND btf.icardid = costoMAT.icardid " & _
                    "WHERE btf.ibaseid = " & iprojectid & " AND btflc.scardlegacycategoryid = '" & dsCategorias.Tables(0).Rows(i).Item("scardlegacycategoryid") & "' "

                    Dim contadorDeTarjetas As Integer = 0

                    contadorDeTarjetas = getSQLQueryAsInteger(0, queryContadorDeTarjetas)

                    If contadorDeTarjetas > 0 Then

                        queriesFill(i) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardsAux " & _
                        "SELECT '' AS 'icardid', CONCAT(btflc.scardlegacycategoryid, ' ', btflc.scardlegacycategorydescription) AS 'scardlegacyid', " & _
                        "'' AS 'scarddescription', '' AS 'scardunit', '' AS 'dcardqty', '' AS 'dcardindirectcosts', '' AS 'dcardgain', '' AS 'dcardunitaryprice', '' AS 'dcardsprice' FROM cardlegacycategories btflc WHERE btflc.scardlegacycategoryid = '" & dsCategorias.Tables(0).Rows(i).Item("scardlegacycategoryid") & "' " & _
                        "UNION SELECT btf.icardid, btf.scardlegacyid, btf.scarddescription, btf.scardunit, 1 AS dcardqty, " & _
                        "FORMAT(((IF(costoMAT.costo is null, 0, costoMAT.costo) + IF(costoMO.costo is null, 0, costoMO.costo) + IF(costoEQ.costo is null, 0, costoEQ.costo))*(btf.dcardindirectcostspercentage)*(1)), 2) AS dcardindirectcosts, " & _
                        "FORMAT(((IF(costoMAT.costo is null, 0, costoMAT.costo) + IF(costoMO.costo is null, 0, costoMO.costo) + IF(costoEQ.costo is null, 0, costoEQ.costo))*(1+btf.dcardindirectcostspercentage)*(btf.dcardgainpercentage)*(1)), 2) AS dcardgain, " & _
                        "FORMAT(((IF(costoMAT.costo is null, 0, costoMAT.costo) + IF(costoMO.costo is null, 0, costoMO.costo) + IF(costoEQ.costo is null, 0, costoEQ.costo))*(1+btf.dcardindirectcostspercentage)*(1+btf.dcardgainpercentage)), 2) AS dcardunitaryprice, " & _
                        "FORMAT(((IF(costoMAT.costo is null, 0, costoMAT.costo) + IF(costoMO.costo is null, 0, costoMO.costo) + IF(costoEQ.costo is null, 0, costoEQ.costo))*(1+btf.dcardindirectcostspercentage)*(1+btf.dcardgainpercentage)*(1)), 2) AS dcardsprice " & _
                        "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Cards btf " & _
                        "JOIN cardlegacycategories btflc ON btf.scardlegacycategoryid = btflc.scardlegacycategoryid " & _
                        "JOIN (SELECT btfi.ibaseid, btfi.icardid, btfi.iinputid, (costoMO.costo*btfi.dcardinputqty) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardInputs btfi JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Cards btf ON btf.ibaseid = btfi.ibaseid AND btf.icardid = btfi.icardid JOIN (SELECT btfi.ibaseid, btfi.icardid AS icardid, 0 AS iinputid, SUM(btfi.dcardinputqty*bp.dinputfinalprice) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Cards btf LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardInputs btfi ON btf.ibaseid = btfi.ibaseid AND btf.icardid = btfi.icardid LEFT JOIN inputs i ON btfi.iinputid = i.iinputid JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) bp GROUP BY iinputid, ibaseid) bp ON btfi.ibaseid = bp.ibaseid AND i.iinputid = bp.iinputid LEFT JOIN inputtypes it ON i.iinputid = it.iinputid WHERE it.sinputtypedescription = 'MANO DE OBRA' GROUP BY btf.icardid, btfi.ibaseid ) AS costoMO ON btfi.iinputid = costoMO.iinputid AND btfi.icardid = costoMO.icardid GROUP BY btfi.icardid, btfi.ibaseid) AS costoEQ ON btf.ibaseid = costoEQ.ibaseid AND btf.icardid = costoEQ.icardid " & _
                        "JOIN (SELECT btfi.ibaseid, btfi.icardid, btfi.iinputid, SUM(btfi.dcardinputqty*bp.dinputfinalprice) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Cards btf LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardInputs btfi ON btf.ibaseid = btfi.ibaseid AND btf.icardid = btfi.icardid LEFT JOIN inputs i ON btfi.iinputid = i.iinputid JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) bp GROUP BY iinputid, ibaseid) bp ON btfi.ibaseid = bp.ibaseid AND i.iinputid = bp.iinputid JOIN inputtypes it ON i.iinputid = it.iinputid WHERE it.sinputtypedescription = 'MANO DE OBRA' GROUP BY btf.icardid, btfi.ibaseid) AS costoMO ON btf.ibaseid = costoMO.ibaseid AND btf.icardid = costoMO.icardid " & _
                        "LEFT JOIN (SELECT btfi.ibaseid, btfi.icardid, IF(SUM(btfi.dcardinputqty*bp.dinputfinalprice) IS NULL, 0, SUM(btfi.dcardinputqty*bp.dinputfinalprice))+IF(SUM(btfi.dcardinputqty*cibp.dinputfinalprice) IS NULL, 0, SUM(btfi.dcardinputqty*cibp.dinputfinalprice)) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Cards btf LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardInputs btfi ON btf.ibaseid = btfi.ibaseid AND btf.icardid = btfi.icardid LEFT JOIN inputs i ON btfi.iinputid = i.iinputid LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) bp GROUP BY iinputid, ibaseid) bp ON btfi.ibaseid = bp.ibaseid AND i.iinputid = bp.iinputid LEFT JOIN (SELECT btfi.ibaseid, btfi.icardid, btfi.iinputid, cibp.iupdatedate, cibp.supdatetime, SUM(btfci.dcompoundinputqty*cibp.dinputfinalprice) AS dinputpricewithoutIVA, 0.00000 AS dinputprotectionpercentage, SUM(btfci.dcompoundinputqty*cibp.dinputfinalprice) AS dinputfinalprice FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardInputs btfi JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardCompoundInputs btfci ON btfci.ibaseid = btfi.ibaseid AND btfci.icardid = btfi.icardid AND btfci.iinputid = btfi.iinputid LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) cibp GROUP BY iinputid, ibaseid) cibp ON cibp.ibaseid = btfci.ibaseid AND cibp.iinputid = btfci.icompoundinputid GROUP BY btfci.ibaseid, btfci.icardid, btfi.iinputid) cibp ON btfi.ibaseid = cibp.ibaseid AND btfi.icardid = cibp.icardid AND i.iinputid = cibp.iinputid JOIN inputtypes it ON i.iinputid = it.iinputid WHERE it.sinputtypedescription = 'MATERIALES' GROUP BY btfi.ibaseid, btfi.icardid ORDER BY btfi.ibaseid, btfi.icardid, btfi.iinputid) AS costoMAT ON btf.ibaseid = costoMAT.ibaseid AND btf.icardid = costoMAT.icardid " & _
                        "WHERE btf.ibaseid = " & iprojectid & " AND btflc.scardlegacycategoryid = '" & dsCategorias.Tables(0).Rows(i).Item("scardlegacycategoryid") & "' ORDER BY scardlegacyid "

                    End If

                Next i

                executeTransactedSQLCommand(0, queriesFill)

            Catch ex As Exception

            End Try


            setDataGridView(dgvResumenDeTarjetas, "SELECT scardid, scardlegacyid AS 'ID', scarddescription AS 'Descripcion Tarjeta', scardunit AS 'Unidad de Medida', sbasecardqty AS 'Cantidad', scardindirectcosts AS 'Indirectos', scardgain AS 'Utilidad', scardunitaryprice AS 'P.U.', scardamount AS 'Importe' FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardsAux", False)

            dgvResumenDeTarjetas.Columns(0).Visible = False

            If viewDgvIndirectCosts = False Then
                dgvResumenDeTarjetas.Columns(5).Visible = False
            End If

            If viewDgvProfits = False Then
                dgvResumenDeTarjetas.Columns(6).Visible = False
            End If

            If viewDgvUnitPrices = False Then
                dgvResumenDeTarjetas.Columns(7).Visible = False
            End If

            If viewDgvAmount = False Then
                dgvResumenDeTarjetas.Columns(8).Visible = False
            End If

            dgvResumenDeTarjetas.Columns(0).ReadOnly = True
            dgvResumenDeTarjetas.Columns(1).ReadOnly = True
            dgvResumenDeTarjetas.Columns(2).ReadOnly = True
            dgvResumenDeTarjetas.Columns(3).ReadOnly = True
            dgvResumenDeTarjetas.Columns(4).ReadOnly = True
            dgvResumenDeTarjetas.Columns(5).ReadOnly = True
            dgvResumenDeTarjetas.Columns(6).ReadOnly = True
            dgvResumenDeTarjetas.Columns(7).ReadOnly = True
            dgvResumenDeTarjetas.Columns(8).ReadOnly = True

            dgvResumenDeTarjetas.Columns(0).Width = 30
            dgvResumenDeTarjetas.Columns(1).Width = 60
            dgvResumenDeTarjetas.Columns(2).Width = 200
            dgvResumenDeTarjetas.Columns(3).Width = 60
            dgvResumenDeTarjetas.Columns(4).Width = 70
            dgvResumenDeTarjetas.Columns(5).Width = 70
            dgvResumenDeTarjetas.Columns(6).Width = 70
            dgvResumenDeTarjetas.Columns(7).Width = 70
            dgvResumenDeTarjetas.Columns(8).Width = 70


        End If

        Cursor.Current = System.Windows.Forms.Cursors.Default

    End Sub


    Private Sub dgvResumenDeTarjetas_CellContentDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgvResumenDeTarjetas.CellContentDoubleClick

        If openCards = False Then
            Exit Sub
        End If

        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        Try

            If dgvResumenDeTarjetas.CurrentRow.IsNewRow Then
                Exit Sub
            End If

            iselectedcardid = CInt(dgvResumenDeTarjetas.Rows(e.RowIndex).Cells(0).Value())
            sselectedcardlegacyid = dgvResumenDeTarjetas.Rows(e.RowIndex).Cells(1).Value()
            sselectedcarddescription = dgvResumenDeTarjetas.Rows(e.RowIndex).Cells(2).Value()
            dselectedcardqty = CDbl(dgvResumenDeTarjetas.Rows(e.RowIndex).Cells(4).Value())

        Catch ex As Exception

            iselectedcardid = 0
            sselectedcardlegacyid = ""
            sselectedcostdescription = ""
            dselectedcardqty = 1.0

        End Try

        If iselectedcardid = 0 Then
            Exit Sub
        End If

        Dim ag As New AgregarTarjeta
        ag.susername = susername
        ag.bactive = bactive
        ag.bonline = bonline
        ag.suserfullname = suserfullname

        ag.suseremail = suseremail
        ag.susersession = susersession
        ag.susermachinename = susermachinename
        ag.suserip = suserip

        ag.IsBase = True
        ag.IsModel = False

        ag.IsEdit = True
        ag.IsHistoric = isHistoric

        ag.iprojectid = iprojectid
        ag.icardid = iselectedcardid

        If Me.WindowState = FormWindowState.Maximized Then
            ag.WindowState = FormWindowState.Maximized
        End If

        Me.Visible = False
        ag.ShowDialog(Me)
        Me.Visible = True

        If ag.DialogResult = Windows.Forms.DialogResult.OK Then

            Dim dsCategorias As DataSet
            Dim contadorCategorias As Integer = 0
            Dim resumenDeTarjetas As String = ""
            dsCategorias = getSQLQueryAsDataset(0, "SELECT scardlegacycategoryid, scardlegacycategorydescription FROM cardlegacycategories")
            contadorCategorias = dsCategorias.Tables(0).Rows.Count

            Dim queriesFill(contadorCategorias) As String

            Dim queriesCreation(2) As String

            queriesCreation(0) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardsAux"
            queriesCreation(1) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardsAux (   scardid varchar(11) COLLATE latin1_spanish_ci NOT NULL,   scardlegacyid varchar(510) CHARACTER SET latin1 NOT NULL,   scarddescription VARCHAR(1000) CHARACTER SET latin1 NOT NULL,   scardunit varchar(49) CHARACTER SET latin1 NOT NULL,   sbasecardqty varchar(20) COLLATE latin1_spanish_ci NOT NULL,   scardindirectcosts varchar(20) COLLATE latin1_spanish_ci NOT NULL,   scardgain varchar(20) COLLATE latin1_spanish_ci NOT NULL,   scardunitaryprice varchar(20) COLLATE latin1_spanish_ci NOT NULL,   scardamount varchar(20) COLLATE latin1_spanish_ci NOT NULL ) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"

            executeTransactedSQLCommand(0, queriesCreation)



            Try

                For i As Integer = 0 To contadorCategorias - 1

                    Dim queryContadorDeTarjetas As String = "" & _
                    "SELECT COUNT(*) " & _
                    "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Cards btf " & _
                    "JOIN cardlegacycategories btflc ON btf.scardlegacycategoryid = btflc.scardlegacycategoryid " & _
                    "JOIN (SELECT btfi.ibaseid, btfi.icardid, btfi.iinputid, (costoMO.costo*btfi.dcardinputqty) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardInputs btfi JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Cards btf ON btf.ibaseid = btfi.ibaseid AND btf.icardid = btfi.icardid JOIN (SELECT btfi.ibaseid, btfi.icardid AS icardid, 0 AS iinputid, SUM(btfi.dcardinputqty*bp.dinputfinalprice) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Cards btf LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardInputs btfi ON btf.ibaseid = btfi.ibaseid AND btf.icardid = btfi.icardid LEFT JOIN inputs i ON btfi.iinputid = i.iinputid JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) bp GROUP BY iinputid, ibaseid) bp ON btfi.ibaseid = bp.ibaseid AND i.iinputid = bp.iinputid LEFT JOIN inputtypes it ON i.iinputid = it.iinputid WHERE it.sinputtypedescription = 'MANO DE OBRA' GROUP BY btf.icardid, btfi.ibaseid ) AS costoMO ON btfi.iinputid = costoMO.iinputid AND btfi.icardid = costoMO.icardid GROUP BY btfi.icardid, btfi.ibaseid) AS costoEQ ON btf.ibaseid = costoEQ.ibaseid AND btf.icardid = costoEQ.icardid " & _
                    "JOIN (SELECT btfi.ibaseid, btfi.icardid, btfi.iinputid, SUM(btfi.dcardinputqty*bp.dinputfinalprice) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Cards btf LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardInputs btfi ON btf.ibaseid = btfi.ibaseid AND btf.icardid = btfi.icardid LEFT JOIN inputs i ON btfi.iinputid = i.iinputid JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) bp GROUP BY iinputid, ibaseid) bp ON btfi.ibaseid = bp.ibaseid AND i.iinputid = bp.iinputid JOIN inputtypes it ON i.iinputid = it.iinputid WHERE it.sinputtypedescription = 'MANO DE OBRA' GROUP BY btf.icardid, btfi.ibaseid) AS costoMO ON btf.ibaseid = costoMO.ibaseid AND btf.icardid = costoMO.icardid " & _
                    "LEFT JOIN (SELECT btfi.ibaseid, btfi.icardid, IF(SUM(btfi.dcardinputqty*bp.dinputfinalprice) IS NULL, 0, SUM(btfi.dcardinputqty*bp.dinputfinalprice))+IF(SUM(btfi.dcardinputqty*cibp.dinputfinalprice) IS NULL, 0, SUM(btfi.dcardinputqty*cibp.dinputfinalprice)) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Cards btf LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardInputs btfi ON btf.ibaseid = btfi.ibaseid AND btf.icardid = btfi.icardid LEFT JOIN inputs i ON btfi.iinputid = i.iinputid LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) bp GROUP BY iinputid, ibaseid) bp ON btfi.ibaseid = bp.ibaseid AND i.iinputid = bp.iinputid LEFT JOIN (SELECT btfi.ibaseid, btfi.icardid, btfi.iinputid, cibp.iupdatedate, cibp.supdatetime, SUM(btfci.dcompoundinputqty*cibp.dinputfinalprice) AS dinputpricewithoutIVA, 0.00000 AS dinputprotectionpercentage, SUM(btfci.dcompoundinputqty*cibp.dinputfinalprice) AS dinputfinalprice FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardInputs btfi JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardCompoundInputs btfci ON btfci.ibaseid = btfi.ibaseid AND btfci.icardid = btfi.icardid AND btfci.iinputid = btfi.iinputid LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) cibp GROUP BY iinputid, ibaseid) cibp ON cibp.ibaseid = btfci.ibaseid AND cibp.iinputid = btfci.icompoundinputid GROUP BY btfci.ibaseid, btfci.icardid, btfi.iinputid) cibp ON btfi.ibaseid = cibp.ibaseid AND btfi.icardid = cibp.icardid AND i.iinputid = cibp.iinputid JOIN inputtypes it ON i.iinputid = it.iinputid WHERE it.sinputtypedescription = 'MATERIALES' GROUP BY btfi.ibaseid, btfi.icardid ORDER BY btfi.ibaseid, btfi.icardid, btfi.iinputid) AS costoMAT ON btf.ibaseid = costoMAT.ibaseid AND btf.icardid = costoMAT.icardid " & _
                    "WHERE btf.ibaseid = " & iprojectid & " AND btflc.scardlegacycategoryid = '" & dsCategorias.Tables(0).Rows(i).Item("scardlegacycategoryid") & "' "

                    Dim contadorDeTarjetas As Integer = 0

                    contadorDeTarjetas = getSQLQueryAsInteger(0, queryContadorDeTarjetas)

                    If contadorDeTarjetas > 0 Then

                        queriesFill(i) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardsAux " & _
                        "SELECT '' AS 'icardid', CONCAT(btflc.scardlegacycategoryid, ' ', btflc.scardlegacycategorydescription) AS 'scardlegacyid', " & _
                        "'' AS 'scarddescription', '' AS 'scardunit', '' AS 'dcardqty', '' AS 'dcardindirectcosts', '' AS 'dcardgain', '' AS 'dcardunitaryprice', '' AS 'dcardsprice' FROM cardlegacycategories btflc WHERE btflc.scardlegacycategoryid = '" & dsCategorias.Tables(0).Rows(i).Item("scardlegacycategoryid") & "' " & _
                        "UNION SELECT btf.icardid, btf.scardlegacyid, btf.scarddescription, btf.scardunit, 1 AS dcardqty, " & _
                        "FORMAT(((IF(costoMAT.costo is null, 0, costoMAT.costo) + IF(costoMO.costo is null, 0, costoMO.costo) + IF(costoEQ.costo is null, 0, costoEQ.costo))*(btf.dcardindirectcostspercentage)*(1)), 2) AS dcardindirectcosts, " & _
                        "FORMAT(((IF(costoMAT.costo is null, 0, costoMAT.costo) + IF(costoMO.costo is null, 0, costoMO.costo) + IF(costoEQ.costo is null, 0, costoEQ.costo))*(1+btf.dcardindirectcostspercentage)*(btf.dcardgainpercentage)*(1)), 2) AS dcardgain, " & _
                        "FORMAT(((IF(costoMAT.costo is null, 0, costoMAT.costo) + IF(costoMO.costo is null, 0, costoMO.costo) + IF(costoEQ.costo is null, 0, costoEQ.costo))*(1+btf.dcardindirectcostspercentage)*(1+btf.dcardgainpercentage)), 2) AS dcardunitaryprice, " & _
                        "FORMAT(((IF(costoMAT.costo is null, 0, costoMAT.costo) + IF(costoMO.costo is null, 0, costoMO.costo) + IF(costoEQ.costo is null, 0, costoEQ.costo))*(1+btf.dcardindirectcostspercentage)*(1+btf.dcardgainpercentage)*(1)), 2) AS dcardsprice " & _
                        "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Cards btf " & _
                        "JOIN cardlegacycategories btflc ON btf.scardlegacycategoryid = btflc.scardlegacycategoryid " & _
                        "JOIN (SELECT btfi.ibaseid, btfi.icardid, btfi.iinputid, (costoMO.costo*btfi.dcardinputqty) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardInputs btfi JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Cards btf ON btf.ibaseid = btfi.ibaseid AND btf.icardid = btfi.icardid JOIN (SELECT btfi.ibaseid, btfi.icardid AS icardid, 0 AS iinputid, SUM(btfi.dcardinputqty*bp.dinputfinalprice) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Cards btf LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardInputs btfi ON btf.ibaseid = btfi.ibaseid AND btf.icardid = btfi.icardid LEFT JOIN inputs i ON btfi.iinputid = i.iinputid JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) bp GROUP BY iinputid, ibaseid) bp ON btfi.ibaseid = bp.ibaseid AND i.iinputid = bp.iinputid LEFT JOIN inputtypes it ON i.iinputid = it.iinputid WHERE it.sinputtypedescription = 'MANO DE OBRA' GROUP BY btf.icardid, btfi.ibaseid ) AS costoMO ON btfi.iinputid = costoMO.iinputid AND btfi.icardid = costoMO.icardid GROUP BY btfi.icardid, btfi.ibaseid) AS costoEQ ON btf.ibaseid = costoEQ.ibaseid AND btf.icardid = costoEQ.icardid " & _
                        "JOIN (SELECT btfi.ibaseid, btfi.icardid, btfi.iinputid, SUM(btfi.dcardinputqty*bp.dinputfinalprice) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Cards btf LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardInputs btfi ON btf.ibaseid = btfi.ibaseid AND btf.icardid = btfi.icardid LEFT JOIN inputs i ON btfi.iinputid = i.iinputid JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) bp GROUP BY iinputid, ibaseid) bp ON btfi.ibaseid = bp.ibaseid AND i.iinputid = bp.iinputid JOIN inputtypes it ON i.iinputid = it.iinputid WHERE it.sinputtypedescription = 'MANO DE OBRA' GROUP BY btf.icardid, btfi.ibaseid) AS costoMO ON btf.ibaseid = costoMO.ibaseid AND btf.icardid = costoMO.icardid " & _
                        "LEFT JOIN (SELECT btfi.ibaseid, btfi.icardid, IF(SUM(btfi.dcardinputqty*bp.dinputfinalprice) IS NULL, 0, SUM(btfi.dcardinputqty*bp.dinputfinalprice))+IF(SUM(btfi.dcardinputqty*cibp.dinputfinalprice) IS NULL, 0, SUM(btfi.dcardinputqty*cibp.dinputfinalprice)) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Cards btf LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardInputs btfi ON btf.ibaseid = btfi.ibaseid AND btf.icardid = btfi.icardid LEFT JOIN inputs i ON btfi.iinputid = i.iinputid LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) bp GROUP BY iinputid, ibaseid) bp ON btfi.ibaseid = bp.ibaseid AND i.iinputid = bp.iinputid LEFT JOIN (SELECT btfi.ibaseid, btfi.icardid, btfi.iinputid, cibp.iupdatedate, cibp.supdatetime, SUM(btfci.dcompoundinputqty*cibp.dinputfinalprice) AS dinputpricewithoutIVA, 0.00000 AS dinputprotectionpercentage, SUM(btfci.dcompoundinputqty*cibp.dinputfinalprice) AS dinputfinalprice FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardInputs btfi JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardCompoundInputs btfci ON btfci.ibaseid = btfi.ibaseid AND btfci.icardid = btfi.icardid AND btfci.iinputid = btfi.iinputid LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) cibp GROUP BY iinputid, ibaseid) cibp ON cibp.ibaseid = btfci.ibaseid AND cibp.iinputid = btfci.icompoundinputid GROUP BY btfci.ibaseid, btfci.icardid, btfi.iinputid) cibp ON btfi.ibaseid = cibp.ibaseid AND btfi.icardid = cibp.icardid AND i.iinputid = cibp.iinputid JOIN inputtypes it ON i.iinputid = it.iinputid WHERE it.sinputtypedescription = 'MATERIALES' GROUP BY btfi.ibaseid, btfi.icardid ORDER BY btfi.ibaseid, btfi.icardid, btfi.iinputid) AS costoMAT ON btf.ibaseid = costoMAT.ibaseid AND btf.icardid = costoMAT.icardid " & _
                        "WHERE btf.ibaseid = " & iprojectid & " AND btflc.scardlegacycategoryid = '" & dsCategorias.Tables(0).Rows(i).Item("scardlegacycategoryid") & "' ORDER BY scardlegacyid "

                    End If

                Next i

                executeTransactedSQLCommand(0, queriesFill)

            Catch ex As Exception

            End Try


            setDataGridView(dgvResumenDeTarjetas, "SELECT scardid, scardlegacyid AS 'ID', scarddescription AS 'Descripcion Tarjeta', scardunit AS 'Unidad de Medida', sbasecardqty AS 'Cantidad', scardindirectcosts AS 'Indirectos', scardgain AS 'Utilidad', scardunitaryprice AS 'P.U.', scardamount AS 'Importe' FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardsAux", False)

            dgvResumenDeTarjetas.Columns(0).Visible = False

            If viewDgvIndirectCosts = False Then
                dgvResumenDeTarjetas.Columns(5).Visible = False
            End If

            If viewDgvProfits = False Then
                dgvResumenDeTarjetas.Columns(6).Visible = False
            End If

            If viewDgvUnitPrices = False Then
                dgvResumenDeTarjetas.Columns(7).Visible = False
            End If

            If viewDgvAmount = False Then
                dgvResumenDeTarjetas.Columns(8).Visible = False
            End If

            dgvResumenDeTarjetas.Columns(0).ReadOnly = True
            dgvResumenDeTarjetas.Columns(1).ReadOnly = True
            dgvResumenDeTarjetas.Columns(2).ReadOnly = True
            dgvResumenDeTarjetas.Columns(3).ReadOnly = True
            dgvResumenDeTarjetas.Columns(4).ReadOnly = True
            dgvResumenDeTarjetas.Columns(5).ReadOnly = True
            dgvResumenDeTarjetas.Columns(6).ReadOnly = True
            dgvResumenDeTarjetas.Columns(7).ReadOnly = True
            dgvResumenDeTarjetas.Columns(8).ReadOnly = True

            dgvResumenDeTarjetas.Columns(0).Width = 30
            dgvResumenDeTarjetas.Columns(1).Width = 60
            dgvResumenDeTarjetas.Columns(2).Width = 200
            dgvResumenDeTarjetas.Columns(3).Width = 60
            dgvResumenDeTarjetas.Columns(4).Width = 70
            dgvResumenDeTarjetas.Columns(5).Width = 70
            dgvResumenDeTarjetas.Columns(6).Width = 70
            dgvResumenDeTarjetas.Columns(7).Width = 70
            dgvResumenDeTarjetas.Columns(8).Width = 70



        End If

        Cursor.Current = System.Windows.Forms.Cursors.Default

    End Sub


    Private Sub txtLegacyIdDgvResumenDeTarjetas_KeyUp(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtLegacyIdDgvResumenDeTarjetas.KeyUp

        txtLegacyIdDgvResumenDeTarjetas.Text = txtLegacyIdDgvResumenDeTarjetas.Text.Replace("'", "").Replace("--", "").Replace("@", "").Replace("|", "")

        Dim strForbidden1 As String = "|°!#$%&/()=?¡*¨[]_:;,-{}+´¿'¬^`~@\<>"
        Dim arrayForbidden1 As Char() = strForbidden1.ToCharArray

        For fc = 0 To arrayForbidden1.Length - 1

            If txtLegacyIdDgvResumenDeTarjetas.Text.Contains(arrayForbidden1(fc)) Then
                txtLegacyIdDgvResumenDeTarjetas.Text = txtLegacyIdDgvResumenDeTarjetas.Text.Replace(arrayForbidden1(fc), "")
            End If

        Next fc

    End Sub


    Private Sub txtNombreDgvResumenDeTarjetas_KeyUp(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtNombreDgvResumenDeTarjetas.KeyUp

        txtNombreDgvResumenDeTarjetas.Text = txtNombreDgvResumenDeTarjetas.Text.Replace("'", "").Replace("--", "").Replace("@", "").Replace("|", "")

        Dim strForbidden1 As String = "|°!#$%&/()=?¡*¨[]_:;,-{}+´¿'¬^`~@\<>"
        Dim arrayForbidden1 As Char() = strForbidden1.ToCharArray

        For fc = 0 To arrayForbidden1.Length - 1

            If txtNombreDgvResumenDeTarjetas.Text.Contains(arrayForbidden1(fc)) Then
                txtNombreDgvResumenDeTarjetas.Text = txtNombreDgvResumenDeTarjetas.Text.Replace(arrayForbidden1(fc), "")
            End If

        Next fc

    End Sub


    Private Sub txtCantidadDgvResumenDeTarjetas_KeyUp(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtCantidadDgvResumenDeTarjetas.KeyUp

        If Not IsNumeric(txtCantidadDgvResumenDeTarjetas.Text) Then

            Dim strForbidden2 As String = "abcdefghijklmnopqrstuvwxyzñABCDEFGHIJKLMNOPQRSTUVWXYZÑ|°!#$%&/()=?¡*¨[]_:;,-{}+´¿'¬^`~@\<>"
            Dim arrayForbidden2 As Char() = strForbidden2.ToCharArray

            For cp = 0 To arrayForbidden2.Length - 1

                If txtCantidadDgvResumenDeTarjetas.Text.Contains(arrayForbidden2(cp)) Then
                    txtCantidadDgvResumenDeTarjetas.Text = txtCantidadDgvResumenDeTarjetas.Text.Replace(arrayForbidden2(cp), "")
                End If

            Next cp

            If txtCantidadDgvResumenDeTarjetas.Text.Contains(".") Then

                Dim comparaPuntos As Char() = txtCantidadDgvResumenDeTarjetas.Text.ToCharArray
                Dim cuantosPuntos As Integer = 0


                For letra = 0 To comparaPuntos.Length - 1

                    If comparaPuntos(letra) = "." Then
                        cuantosPuntos = cuantosPuntos + 1
                    End If

                Next

                If cuantosPuntos > 1 Then

                    For cantidad = 1 To cuantosPuntos
                        Dim lugar As Integer = txtCantidadDgvResumenDeTarjetas.Text.LastIndexOf(".")
                        Dim longitud As Integer = txtCantidadDgvResumenDeTarjetas.Text.Length

                        If longitud > (lugar + 1) Then
                            txtCantidadDgvResumenDeTarjetas.Text = txtCantidadDgvResumenDeTarjetas.Text.Substring(0, lugar) & txtCantidadDgvResumenDeTarjetas.Text.Substring(lugar + 1)
                            Exit For
                        Else
                            txtCantidadDgvResumenDeTarjetas.Text = txtCantidadDgvResumenDeTarjetas.Text.Substring(0, lugar)
                            Exit For
                        End If
                    Next

                End If

            End If

            txtCantidadDgvResumenDeTarjetas.Text = txtCantidadDgvResumenDeTarjetas.Text.Replace("--", "").Replace("'", "")
            txtCantidadDgvResumenDeTarjetas.Text = txtCantidadDgvResumenDeTarjetas.Text.Trim

        Else
            txtCantidadDgvResumenDeTarjetas_OldText = txtCantidadDgvResumenDeTarjetas.Text
        End If

    End Sub


    Private Sub dgvResumenDeTarjetas_UserAddedRow(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewRowEventArgs) Handles dgvResumenDeTarjetas.UserAddedRow

        If addCards = False Then
            Exit Sub
        End If

        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        isResumenDGVReady = False

        Dim bf As New BuscaTarjetas
        bf.susername = susername
        bf.bactive = bactive
        bf.bonline = bonline
        bf.suserfullname = suserfullname

        bf.suseremail = suseremail
        bf.susersession = susersession
        bf.susermachinename = susermachinename
        bf.suserip = suserip

        bf.iprojectid = iprojectid

        bf.querystring = dgvResumenDeTarjetas.CurrentCell.EditedFormattedValue

        bf.isEdit = False

        bf.isBase = True
        bf.isModel = False

        bf.isHistoric = isHistoric

        If Me.WindowState = FormWindowState.Maximized Then
            bf.WindowState = FormWindowState.Maximized
        End If

        Me.Visible = False
        bf.ShowDialog(Me)
        Me.Visible = True

        If bf.DialogResult = Windows.Forms.DialogResult.OK Then

            Dim baseid As Integer = 0
            baseid = getSQLQueryAsInteger(0, "SELECT ibaseid FROM base ORDER BY iupdatedate DESC, supdatetime DESC LIMIT 1")

            If baseid = 0 Then
                baseid = 99999
            End If

            Dim fecha As Integer = 0
            Dim hora As String = "00:00:00"

            fecha = getMySQLDate()
            hora = getAppTime()

            Dim chequeoPrimeraVezQueSeInsertaTarjetaDelPresupuestoBase As Integer = 0
            chequeoPrimeraVezQueSeInsertaTarjetaDelPresupuestoBase = getSQLQueryAsInteger(0, "SELECT COUNT(*) FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Prices WHERE ibaseid = " & iprojectid)

            If chequeoPrimeraVezQueSeInsertaTarjetaDelPresupuestoBase = 0 Then

                Dim queriesPrimeraVez(2) As String
                queriesPrimeraVez(0) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Prices SELECT " & iprojectid & ", iinputid, dinputpricewithoutIVA, dinputprotectionpercentage, dinputfinalprice, " & fecha & ", '" & hora & "', '" & susername & "' FROM (SELECT * FROM baseprices WHERE ibaseid = " & baseid & " ORDER BY iupdatedate DESC, supdatetime DESC) bp GROUP BY iinputid"
                queriesPrimeraVez(1) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Timber SELECT " & iprojectid & ", bt.iinputid, bt.dinputtimberespesor, bt.dinputtimberancho, bt.dinputtimberlargo, bt.dinputtimberpreciopiecubico, " & fecha & ", '" & hora & "', '" & susername & "' FROM basetimber bt where ibaseid = " & baseid
                executeTransactedSQLCommand(0, queriesPrimeraVez)

            End If

            If bf.wasCreated = False Then

                Dim dsBusquedaTarjetasRepetidas As DataSet

                dsBusquedaTarjetasRepetidas = getSQLQueryAsDataset(0, "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Cards WHERE ibaseid = " & iprojectid & " AND scardlegacyid = '" & bf.scardlegacyid & "'")

                If dsBusquedaTarjetasRepetidas.Tables(0).Rows.Count > 0 Then

                    MsgBox("Ya tienes esa Tarjeta insertada en este Presupuesto Base. ¿Podrías buscarla en la lista de Resumen de Tarjetas y cambiar la cantidad si así lo deseas?", MsgBoxStyle.OkOnly, "Tarjeta Repetida")
                    dgvResumenDeTarjetas.EndEdit()
                    Exit Sub

                End If


                'Dim cardid As Integer
                'cardid = getSQLQueryAsInteger(0, "SELECT IF(MAX(icardid) + 1 IS NULL, 1, MAX(icardid) + 1) AS icardid FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Cards WHERE ibaseid = " & iprojectid & " ORDER BY iupdatedate DESC, supdatetime DESC LIMIT 1")
                'iselectedcardid = cardid

                Dim queriesInsert(2) As String

                queriesInsert(0) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardInputs SELECT " & iprojectid & ", " & bf.icardid & ", iinputid, scardinputunit, dcardinputqty, " & fecha & ", '" & hora & "', '" & susername & "' FROM basecardinputs WHERE ibaseid = " & baseid & " AND icardid = " & bf.icardid
                queriesInsert(0) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardInputs SELECT " & iprojectid & ", " & bf.icardid & ", iinputid, scardinputunit, dcardinputqty, " & fecha & ", '" & hora & "', '" & susername & "' FROM basecardcompoundinputs WHERE ibaseid = " & baseid & " AND icardid = " & bf.icardid
                queriesInsert(1) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Cards SELECT " & iprojectid & ", " & bf.icardid & ", scardlegacycategoryid, scardlegacyid, scarddescription, scardunit, dcardqty, dcardindirectcostspercentage, dcardgainpercentage, " & fecha & ", '" & hora & "', '" & susername & "' FROM basecards WHERE ibaseid = " & baseid & " AND icardid = " & bf.icardid

                executeTransactedSQLCommand(0, queriesInsert)

            End If


            Dim porcentajeIVA As Double = 0.0
            porcentajeIVA = getSQLQueryAsDouble(0, "SELECT dbaseIVApercentage FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & " WHERE ibaseid = " & iprojectid)
            txtPorcentajeIVA.Text = porcentajeIVA * 100

            dgvResumenDeTarjetas.EndEdit()

            isResumenDGVReady = True

        Else

            dgvResumenDeTarjetas.EndEdit()

            isResumenDGVReady = True

        End If

        Cursor.Current = System.Windows.Forms.Cursors.Default

    End Sub


    Private Sub txtPorcentajeIVA_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtPorcentajeIVA.KeyUp

        Dim strcaracteresprohibidos As String = "abcdefghijklmnopqrstuvwxyzñABCDEFGHIJKLMNOPQRSTUVWXYZÑ|°!#$%&/()=?¡*¨[]_:;,-{}+´¿'¬^`~@\<>"
        Dim arrayCaractProhib As Char() = strcaracteresprohibidos.ToCharArray
        Dim resultado As Boolean = False

        For carp = 0 To arrayCaractProhib.Length - 1

            If txtPorcentajeIVA.Text.Contains(arrayCaractProhib(carp)) Then
                txtPorcentajeIVA.Text = txtPorcentajeIVA.Text.Replace(arrayCaractProhib(carp), "")
                resultado = True
            End If

        Next carp

        If txtPorcentajeIVA.Text.Contains(".") Then

            Dim comparaPuntos As Char() = txtPorcentajeIVA.Text.ToCharArray
            Dim cuantosPuntos As Integer = 0


            For letra = 0 To comparaPuntos.Length - 1

                If comparaPuntos(letra) = "." Then
                    cuantosPuntos = cuantosPuntos + 1
                End If

            Next

            If cuantosPuntos > 1 Then

                For cantidad = 1 To cuantosPuntos
                    Dim lugar As Integer = txtPorcentajeIVA.Text.LastIndexOf(".")
                    Dim longitud As Integer = txtPorcentajeIVA.Text.Length

                    If longitud > (lugar + 1) Then
                        txtPorcentajeIVA.Text = txtPorcentajeIVA.Text.Substring(0, lugar) & txtPorcentajeIVA.Text.Substring(lugar + 1)
                        resultado = True
                        Exit For
                    Else
                        txtPorcentajeIVA.Text = txtPorcentajeIVA.Text.Substring(0, lugar)
                        resultado = True
                        Exit For
                    End If
                Next

            End If

        End If

        If resultado = True Then
            txtPorcentajeIVA.Select(txtPorcentajeIVA.Text.Length, 0)
        End If

        txtPorcentajeIVA.Text = txtPorcentajeIVA.Text.Replace("--", "").Replace("'", "")
        txtPorcentajeIVA.Text = txtPorcentajeIVA.Text.Trim

    End Sub


    Private Sub txtPorcentajeIVA_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtPorcentajeIVA.TextChanged

        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        If isFormReadyForAction = False Then
            Exit Sub
        End If

        Dim strcaracteresprohibidos As String = "abcdefghijklmnopqrstuvwxyzñABCDEFGHIJKLMNOPQRSTUVWXYZÑ|°!#$%&/()=?¡*¨[]_:;,-{}+´¿'¬^`~@\<> "
        Dim arrayCaractProhib As Char() = strcaracteresprohibidos.ToCharArray
        txtPorcentajeIVA.Text = txtPorcentajeIVA.Text.Trim(arrayCaractProhib)

        Dim valor As Double = 0.0
        Try
            valor = CDbl(txtPorcentajeIVA.Text.Trim.Trim("--").Trim("'").Trim("@", ""))
            executeSQLCommand(0, "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & " SET dbaseIVApercentage = " & valor / 100 & ", iupdatedate = " & getMySQLDate() & ", supdatetime = '" & getAppTime() & "', supdateusername = '" & susername & "' WHERE ibaseid = " & iprojectid)
        Catch ex As Exception

        End Try

        Dim dsBase As DataSet
        dsBase = getSQLQueryAsDataset(0, "SELECT b.ibaseid, b.iupdatedate, b.supdatetime, b.sbasefileslocation, " & _
        "b.dbaseindirectpercentagedefault, b.dbasegainpercentagedefault, b.dbaseIVApercentage " & _
        "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & " b " & _
        "WHERE ibaseid = " & iprojectid)

        Dim porcentajeIVA As Double = 0.0

        Try
            porcentajeIVA = CDbl(dsBase.Tables(0).Rows(0).Item("dbaseIVApercentage"))
        Catch ex As Exception
            porcentajeIVA = 0.0
        End Try

        'txtPorcentajeIVA.Text = porcentajeIVA * 100

        Cursor.Current = System.Windows.Forms.Cursors.Default

    End Sub


    Private Sub btnNuevaTarjeta_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNuevaTarjeta.Click

        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        isResumenDGVReady = False

        Dim af As New AgregarTarjeta

        af.susername = susername
        af.bactive = bactive
        af.bonline = bonline
        af.suserfullname = suserfullname

        af.suseremail = suseremail
        af.susersession = susersession
        af.susermachinename = susermachinename
        af.suserip = suserip


        af.iprojectid = iprojectid

        af.IsEdit = False

        af.IsBase = True
        af.IsModel = False

        af.IsHistoric = isHistoric

        If Me.WindowState = FormWindowState.Maximized Then
            af.WindowState = FormWindowState.Maximized
        End If

        Me.Visible = False
        af.ShowDialog(Me)
        Me.Visible = True

        If af.DialogResult = Windows.Forms.DialogResult.OK Then

            Me.WindowState = FormWindowState.Normal

            Dim baseid As Integer = 0
            baseid = getSQLQueryAsInteger(0, "SELECT ibaseid FROM base ORDER BY iupdatedate DESC, supdatetime DESC LIMIT 1")

            If baseid = 0 Then
                baseid = 99999
            End If

            Dim fecha As Integer = 0
            Dim hora As String = "00:00:00"

            fecha = getMySQLDate()
            hora = getAppTime()

            Dim chequeoPrimeraVezQueSeInsertaTarjetaDelPresupuestoBase As Integer = 0
            chequeoPrimeraVezQueSeInsertaTarjetaDelPresupuestoBase = getSQLQueryAsInteger(0, "SELECT COUNT(*) FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Prices WHERE ibaseid = " & iprojectid)

            If chequeoPrimeraVezQueSeInsertaTarjetaDelPresupuestoBase = 0 Then

                Dim queriesPrimeraVez(2) As String
                queriesPrimeraVez(0) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Prices SELECT " & iprojectid & ", iinputid, dinputpricewithoutIVA, dinputprotectionpercentage, dinputfinalprice, " & fecha & ", '" & hora & "', '" & susername & "' FROM (SELECT * FROM baseprices WHERE ibaseid = " & baseid & " ORDER BY iupdatedate DESC, supdatetime DESC) bp GROUP BY iinputid"
                queriesPrimeraVez(1) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Timber SELECT " & iprojectid & ", bt.iinputid, bt.dinputtimberespesor, bt.dinputtimberancho, bt.dinputtimberlargo, bt.dinputtimberpreciopiecubico, " & fecha & ", '" & hora & "', '" & susername & "' FROM basetimber bt where ibaseid = " & baseid
                executeTransactedSQLCommand(0, queriesPrimeraVez)

            End If

            If af.wasCreated = False Then

                Dim dsBusquedaTarjetasRepetidas As DataSet

                dsBusquedaTarjetasRepetidas = getSQLQueryAsDataset(0, "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Cards WHERE ibaseid = " & iprojectid & " AND scardlegacyid = '" & af.scardlegacyid & "'")

                If dsBusquedaTarjetasRepetidas.Tables(0).Rows.Count > 0 Then

                    MsgBox("Ya tienes esa Tarjeta insertada en este Presupuesto Base. ¿Podrías buscarla en la lista de Resumen de Tarjetas y cambiar la cantidad si así lo deseas?", MsgBoxStyle.OkOnly, "Tarjeta Repetida")
                    dgvResumenDeTarjetas.EndEdit()
                    Exit Sub

                End If


                'Dim cardid As Integer
                'cardid = getSQLQueryAsInteger(0, "SELECT IF(MAX(icardid) + 1 IS NULL, 1, MAX(icardid) + 1) AS icardid FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Cards WHERE ibaseid = " & iprojectid & " ORDER BY iupdatedate DESC, supdatetime DESC LIMIT 1")
                'iselectedcardid = cardid

                Dim queriesInsert(2) As String

                queriesInsert(0) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardInputs SELECT " & iprojectid & ", " & af.icardid & ", iinputid, scardinputunit, dcardinputqty, " & fecha & ", '" & hora & "', '" & susername & "' FROM basecardinputs WHERE ibaseid = " & baseid & " AND icardid = " & af.icardid
                queriesInsert(0) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardInputs SELECT " & iprojectid & ", " & af.icardid & ", iinputid, scardinputunit, dcardinputqty, " & fecha & ", '" & hora & "', '" & susername & "' FROM basecardcompoundinputs WHERE ibaseid = " & baseid & " AND icardid = " & af.icardid
                queriesInsert(1) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Cards SELECT " & iprojectid & ", " & af.icardid & ", scardlegacycategoryid, scardlegacyid, scarddescription, scardunit, dcardqty, dcardindirectcostspercentage, dcardgainpercentage, " & fecha & ", '" & hora & "', '" & susername & "' FROM basecards WHERE ibaseid = " & baseid & " AND icardid = " & af.icardid

                executeTransactedSQLCommand(0, queriesInsert)

            End If


            Dim porcentajeIVA As Double = 0.0
            porcentajeIVA = getSQLQueryAsDouble(0, "SELECT dbaseIVApercentage FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & " WHERE ibaseid = " & iprojectid)
            txtPorcentajeIVA.Text = porcentajeIVA * 100

        End If

        Dim dsCategorias As DataSet
        Dim contadorCategorias As Integer = 0
        Dim resumenDeTarjetas As String = ""
        dsCategorias = getSQLQueryAsDataset(0, "SELECT scardlegacycategoryid, scardlegacycategorydescription FROM cardlegacycategories")
        contadorCategorias = dsCategorias.Tables(0).Rows.Count

        Dim queriesFill(contadorCategorias) As String

        Dim queriesCreation(2) As String

        queriesCreation(0) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardsAux"
        queriesCreation(1) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardsAux (   scardid varchar(11) COLLATE latin1_spanish_ci NOT NULL,   scardlegacyid varchar(510) CHARACTER SET latin1 NOT NULL,   scarddescription VARCHAR(1000) CHARACTER SET latin1 NOT NULL,   scardunit varchar(49) CHARACTER SET latin1 NOT NULL,   sbasecardqty varchar(20) COLLATE latin1_spanish_ci NOT NULL,   scardindirectcosts varchar(20) COLLATE latin1_spanish_ci NOT NULL,   scardgain varchar(20) COLLATE latin1_spanish_ci NOT NULL,   scardunitaryprice varchar(20) COLLATE latin1_spanish_ci NOT NULL,   scardamount varchar(20) COLLATE latin1_spanish_ci NOT NULL ) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"

        executeTransactedSQLCommand(0, queriesCreation)



        Try

            For i As Integer = 0 To contadorCategorias - 1

                Dim queryContadorDeTarjetas As String = "" & _
                "SELECT COUNT(*) " & _
                "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Cards btf " & _
                "JOIN cardlegacycategories btflc ON btf.scardlegacycategoryid = btflc.scardlegacycategoryid " & _
                "JOIN (SELECT btfi.ibaseid, btfi.icardid, btfi.iinputid, (costoMO.costo*btfi.dcardinputqty) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardInputs btfi JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Cards btf ON btf.ibaseid = btfi.ibaseid AND btf.icardid = btfi.icardid JOIN (SELECT btfi.ibaseid, btfi.icardid AS icardid, 0 AS iinputid, SUM(btfi.dcardinputqty*bp.dinputfinalprice) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Cards btf LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardInputs btfi ON btf.ibaseid = btfi.ibaseid AND btf.icardid = btfi.icardid LEFT JOIN inputs i ON btfi.iinputid = i.iinputid JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) bp GROUP BY iinputid, ibaseid) bp ON btfi.ibaseid = bp.ibaseid AND i.iinputid = bp.iinputid LEFT JOIN inputtypes it ON i.iinputid = it.iinputid WHERE it.sinputtypedescription = 'MANO DE OBRA' GROUP BY btf.icardid, btfi.ibaseid ) AS costoMO ON btfi.iinputid = costoMO.iinputid AND btfi.icardid = costoMO.icardid GROUP BY btfi.icardid, btfi.ibaseid) AS costoEQ ON btf.ibaseid = costoEQ.ibaseid AND btf.icardid = costoEQ.icardid " & _
                "JOIN (SELECT btfi.ibaseid, btfi.icardid, btfi.iinputid, SUM(btfi.dcardinputqty*bp.dinputfinalprice) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Cards btf LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardInputs btfi ON btf.ibaseid = btfi.ibaseid AND btf.icardid = btfi.icardid LEFT JOIN inputs i ON btfi.iinputid = i.iinputid JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) bp GROUP BY iinputid, ibaseid) bp ON btfi.ibaseid = bp.ibaseid AND i.iinputid = bp.iinputid JOIN inputtypes it ON i.iinputid = it.iinputid WHERE it.sinputtypedescription = 'MANO DE OBRA' GROUP BY btf.icardid, btfi.ibaseid) AS costoMO ON btf.ibaseid = costoMO.ibaseid AND btf.icardid = costoMO.icardid " & _
                "LEFT JOIN (SELECT btfi.ibaseid, btfi.icardid, IF(SUM(btfi.dcardinputqty*bp.dinputfinalprice) IS NULL, 0, SUM(btfi.dcardinputqty*bp.dinputfinalprice))+IF(SUM(btfi.dcardinputqty*cibp.dinputfinalprice) IS NULL, 0, SUM(btfi.dcardinputqty*cibp.dinputfinalprice)) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Cards btf LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardInputs btfi ON btf.ibaseid = btfi.ibaseid AND btf.icardid = btfi.icardid LEFT JOIN inputs i ON btfi.iinputid = i.iinputid LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) bp GROUP BY iinputid, ibaseid) bp ON btfi.ibaseid = bp.ibaseid AND i.iinputid = bp.iinputid LEFT JOIN (SELECT btfi.ibaseid, btfi.icardid, btfi.iinputid, cibp.iupdatedate, cibp.supdatetime, SUM(btfci.dcompoundinputqty*cibp.dinputfinalprice) AS dinputpricewithoutIVA, 0.00000 AS dinputprotectionpercentage, SUM(btfci.dcompoundinputqty*cibp.dinputfinalprice) AS dinputfinalprice FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardInputs btfi JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardCompoundInputs btfci ON btfci.ibaseid = btfi.ibaseid AND btfci.icardid = btfi.icardid AND btfci.iinputid = btfi.iinputid LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) cibp GROUP BY iinputid, ibaseid) cibp ON cibp.ibaseid = btfci.ibaseid AND cibp.iinputid = btfci.icompoundinputid GROUP BY btfci.ibaseid, btfci.icardid, btfi.iinputid) cibp ON btfi.ibaseid = cibp.ibaseid AND btfi.icardid = cibp.icardid AND i.iinputid = cibp.iinputid JOIN inputtypes it ON i.iinputid = it.iinputid WHERE it.sinputtypedescription = 'MATERIALES' GROUP BY btfi.ibaseid, btfi.icardid ORDER BY btfi.ibaseid, btfi.icardid, btfi.iinputid) AS costoMAT ON btf.ibaseid = costoMAT.ibaseid AND btf.icardid = costoMAT.icardid " & _
                "WHERE btf.ibaseid = " & iprojectid & " AND btflc.scardlegacycategoryid = '" & dsCategorias.Tables(0).Rows(i).Item("scardlegacycategoryid") & "' "

                Dim contadorDeTarjetas As Integer = 0

                contadorDeTarjetas = getSQLQueryAsInteger(0, queryContadorDeTarjetas)

                If contadorDeTarjetas > 0 Then

                    queriesFill(i) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardsAux " & _
                    "SELECT '' AS 'icardid', CONCAT(btflc.scardlegacycategoryid, ' ', btflc.scardlegacycategorydescription) AS 'scardlegacyid', " & _
                    "'' AS 'scarddescription', '' AS 'scardunit', '' AS 'dcardqty', '' AS 'dcardindirectcosts', '' AS 'dcardgain', '' AS 'dcardunitaryprice', '' AS 'dcardsprice' FROM cardlegacycategories btflc WHERE btflc.scardlegacycategoryid = '" & dsCategorias.Tables(0).Rows(i).Item("scardlegacycategoryid") & "' " & _
                    "UNION SELECT btf.icardid, btf.scardlegacyid, btf.scarddescription, btf.scardunit, 1 AS dcardqty, " & _
                    "FORMAT(((IF(costoMAT.costo is null, 0, costoMAT.costo) + IF(costoMO.costo is null, 0, costoMO.costo) + IF(costoEQ.costo is null, 0, costoEQ.costo))*(btf.dcardindirectcostspercentage)*(1)), 2) AS dcardindirectcosts, " & _
                    "FORMAT(((IF(costoMAT.costo is null, 0, costoMAT.costo) + IF(costoMO.costo is null, 0, costoMO.costo) + IF(costoEQ.costo is null, 0, costoEQ.costo))*(1+btf.dcardindirectcostspercentage)*(btf.dcardgainpercentage)*(1)), 2) AS dcardgain, " & _
                    "FORMAT(((IF(costoMAT.costo is null, 0, costoMAT.costo) + IF(costoMO.costo is null, 0, costoMO.costo) + IF(costoEQ.costo is null, 0, costoEQ.costo))*(1+btf.dcardindirectcostspercentage)*(1+btf.dcardgainpercentage)), 2) AS dcardunitaryprice, " & _
                    "FORMAT(((IF(costoMAT.costo is null, 0, costoMAT.costo) + IF(costoMO.costo is null, 0, costoMO.costo) + IF(costoEQ.costo is null, 0, costoEQ.costo))*(1+btf.dcardindirectcostspercentage)*(1+btf.dcardgainpercentage)*(1)), 2) AS dcardsprice " & _
                    "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Cards btf " & _
                    "JOIN cardlegacycategories btflc ON btf.scardlegacycategoryid = btflc.scardlegacycategoryid " & _
                    "JOIN (SELECT btfi.ibaseid, btfi.icardid, btfi.iinputid, (costoMO.costo*btfi.dcardinputqty) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardInputs btfi JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Cards btf ON btf.ibaseid = btfi.ibaseid AND btf.icardid = btfi.icardid JOIN (SELECT btfi.ibaseid, btfi.icardid AS icardid, 0 AS iinputid, SUM(btfi.dcardinputqty*bp.dinputfinalprice) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Cards btf LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardInputs btfi ON btf.ibaseid = btfi.ibaseid AND btf.icardid = btfi.icardid LEFT JOIN inputs i ON btfi.iinputid = i.iinputid JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) bp GROUP BY iinputid, ibaseid) bp ON btfi.ibaseid = bp.ibaseid AND i.iinputid = bp.iinputid LEFT JOIN inputtypes it ON i.iinputid = it.iinputid WHERE it.sinputtypedescription = 'MANO DE OBRA' GROUP BY btf.icardid, btfi.ibaseid ) AS costoMO ON btfi.iinputid = costoMO.iinputid AND btfi.icardid = costoMO.icardid GROUP BY btfi.icardid, btfi.ibaseid) AS costoEQ ON btf.ibaseid = costoEQ.ibaseid AND btf.icardid = costoEQ.icardid " & _
                    "JOIN (SELECT btfi.ibaseid, btfi.icardid, btfi.iinputid, SUM(btfi.dcardinputqty*bp.dinputfinalprice) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Cards btf LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardInputs btfi ON btf.ibaseid = btfi.ibaseid AND btf.icardid = btfi.icardid LEFT JOIN inputs i ON btfi.iinputid = i.iinputid JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) bp GROUP BY iinputid, ibaseid) bp ON btfi.ibaseid = bp.ibaseid AND i.iinputid = bp.iinputid JOIN inputtypes it ON i.iinputid = it.iinputid WHERE it.sinputtypedescription = 'MANO DE OBRA' GROUP BY btf.icardid, btfi.ibaseid) AS costoMO ON btf.ibaseid = costoMO.ibaseid AND btf.icardid = costoMO.icardid " & _
                    "LEFT JOIN (SELECT btfi.ibaseid, btfi.icardid, IF(SUM(btfi.dcardinputqty*bp.dinputfinalprice) IS NULL, 0, SUM(btfi.dcardinputqty*bp.dinputfinalprice))+IF(SUM(btfi.dcardinputqty*cibp.dinputfinalprice) IS NULL, 0, SUM(btfi.dcardinputqty*cibp.dinputfinalprice)) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Cards btf LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardInputs btfi ON btf.ibaseid = btfi.ibaseid AND btf.icardid = btfi.icardid LEFT JOIN inputs i ON btfi.iinputid = i.iinputid LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) bp GROUP BY iinputid, ibaseid) bp ON btfi.ibaseid = bp.ibaseid AND i.iinputid = bp.iinputid LEFT JOIN (SELECT btfi.ibaseid, btfi.icardid, btfi.iinputid, cibp.iupdatedate, cibp.supdatetime, SUM(btfci.dcompoundinputqty*cibp.dinputfinalprice) AS dinputpricewithoutIVA, 0.00000 AS dinputprotectionpercentage, SUM(btfci.dcompoundinputqty*cibp.dinputfinalprice) AS dinputfinalprice FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardInputs btfi JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardCompoundInputs btfci ON btfci.ibaseid = btfi.ibaseid AND btfci.icardid = btfi.icardid AND btfci.iinputid = btfi.iinputid LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) cibp GROUP BY iinputid, ibaseid) cibp ON cibp.ibaseid = btfci.ibaseid AND cibp.iinputid = btfci.icompoundinputid GROUP BY btfci.ibaseid, btfci.icardid, btfi.iinputid) cibp ON btfi.ibaseid = cibp.ibaseid AND btfi.icardid = cibp.icardid AND i.iinputid = cibp.iinputid JOIN inputtypes it ON i.iinputid = it.iinputid WHERE it.sinputtypedescription = 'MATERIALES' GROUP BY btfi.ibaseid, btfi.icardid ORDER BY btfi.ibaseid, btfi.icardid, btfi.iinputid) AS costoMAT ON btf.ibaseid = costoMAT.ibaseid AND btf.icardid = costoMAT.icardid " & _
                    "WHERE btf.ibaseid = " & iprojectid & " AND btflc.scardlegacycategoryid = '" & dsCategorias.Tables(0).Rows(i).Item("scardlegacycategoryid") & "' ORDER BY scardlegacyid "

                End If

            Next i

            executeTransactedSQLCommand(0, queriesFill)

        Catch ex As Exception

        End Try


        setDataGridView(dgvResumenDeTarjetas, "SELECT scardid, scardlegacyid AS 'ID', scarddescription AS 'Descripcion Tarjeta', scardunit AS 'Unidad de Medida', sbasecardqty AS 'Cantidad', scardindirectcosts AS 'Indirectos', scardgain AS 'Utilidad', scardunitaryprice AS 'P.U.', scardamount AS 'Importe' FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardsAux", False)

        dgvResumenDeTarjetas.Columns(0).Visible = False

        If viewDgvIndirectCosts = False Then
            dgvResumenDeTarjetas.Columns(5).Visible = False
        End If

        If viewDgvProfits = False Then
            dgvResumenDeTarjetas.Columns(6).Visible = False
        End If

        If viewDgvUnitPrices = False Then
            dgvResumenDeTarjetas.Columns(7).Visible = False
        End If

        If viewDgvAmount = False Then
            dgvResumenDeTarjetas.Columns(8).Visible = False
        End If

        dgvResumenDeTarjetas.Columns(0).ReadOnly = True
        dgvResumenDeTarjetas.Columns(1).ReadOnly = True
        dgvResumenDeTarjetas.Columns(2).ReadOnly = True
        dgvResumenDeTarjetas.Columns(3).ReadOnly = True
        dgvResumenDeTarjetas.Columns(4).ReadOnly = True
        dgvResumenDeTarjetas.Columns(5).ReadOnly = True
        dgvResumenDeTarjetas.Columns(6).ReadOnly = True
        dgvResumenDeTarjetas.Columns(7).ReadOnly = True
        dgvResumenDeTarjetas.Columns(8).ReadOnly = True

        dgvResumenDeTarjetas.Columns(0).Width = 30
        dgvResumenDeTarjetas.Columns(1).Width = 60
        dgvResumenDeTarjetas.Columns(2).Width = 200
        dgvResumenDeTarjetas.Columns(3).Width = 60
        dgvResumenDeTarjetas.Columns(4).Width = 70
        dgvResumenDeTarjetas.Columns(5).Width = 70
        dgvResumenDeTarjetas.Columns(6).Width = 70
        dgvResumenDeTarjetas.Columns(7).Width = 70
        dgvResumenDeTarjetas.Columns(8).Width = 70

        isResumenDGVReady = True

        Cursor.Current = System.Windows.Forms.Cursors.Default

    End Sub


    Private Sub btnInsertarTarjeta_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnInsertarTarjeta.Click

        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        isResumenDGVReady = False

        Dim bf As New BuscaTarjetas
        bf.susername = susername
        bf.bactive = bactive
        bf.bonline = bonline
        bf.suserfullname = suserfullname

        bf.suseremail = suseremail
        bf.susersession = susersession
        bf.susermachinename = susermachinename
        bf.suserip = suserip

        bf.iprojectid = iprojectid

        bf.isEdit = False

        bf.isBase = True
        bf.isModel = False

        bf.isHistoric = isHistoric

        If Me.WindowState = FormWindowState.Maximized Then
            bf.WindowState = FormWindowState.Maximized
        End If

        Me.Visible = False
        bf.ShowDialog(Me)
        Me.Visible = True

        If bf.DialogResult = Windows.Forms.DialogResult.OK Then

            Dim baseid As Integer = 0
            baseid = getSQLQueryAsInteger(0, "SELECT ibaseid FROM base ORDER BY iupdatedate DESC, supdatetime DESC LIMIT 1")

            If baseid = 0 Then
                baseid = 99999
            End If

            Dim fecha As Integer = 0
            Dim hora As String = "00:00:00"

            fecha = getMySQLDate()
            hora = getAppTime()

            Dim chequeoPrimeraVezQueSeInsertaTarjetaDelPresupuestoBase As Integer = 0
            chequeoPrimeraVezQueSeInsertaTarjetaDelPresupuestoBase = getSQLQueryAsInteger(0, "SELECT COUNT(*) FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Prices WHERE ibaseid = " & iprojectid)

            If chequeoPrimeraVezQueSeInsertaTarjetaDelPresupuestoBase = 0 Then

                Dim queriesPrimeraVez(2) As String
                queriesPrimeraVez(0) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Prices SELECT " & iprojectid & ", iinputid, dinputpricewithoutIVA, dinputprotectionpercentage, dinputfinalprice, " & fecha & ", '" & hora & "', '" & susername & "' FROM (SELECT * FROM baseprices WHERE ibaseid = " & baseid & " ORDER BY iupdatedate DESC, supdatetime DESC) bp GROUP BY iinputid"
                queriesPrimeraVez(1) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Timber SELECT " & iprojectid & ", bt.iinputid, bt.dinputtimberespesor, bt.dinputtimberancho, bt.dinputtimberlargo, bt.dinputtimberpreciopiecubico, " & fecha & ", '" & hora & "', '" & susername & "' FROM basetimber bt where ibaseid = " & baseid
                executeTransactedSQLCommand(0, queriesPrimeraVez)

            End If

            If bf.wasCreated = False Then

                Dim dsBusquedaTarjetasRepetidas As DataSet

                dsBusquedaTarjetasRepetidas = getSQLQueryAsDataset(0, "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Cards WHERE ibaseid = " & iprojectid & " AND scardlegacyid = '" & bf.scardlegacyid & "'")

                If dsBusquedaTarjetasRepetidas.Tables(0).Rows.Count > 0 Then

                    MsgBox("Ya tienes esa Tarjeta insertada en este Presupuesto Base. ¿Podrías buscarla en la lista de Resumen de Tarjetas y cambiar la cantidad si así lo deseas?", MsgBoxStyle.OkOnly, "Tarjeta Repetida")
                    dgvResumenDeTarjetas.EndEdit()
                    Exit Sub

                End If


                'Dim cardid As Integer
                'cardid = getSQLQueryAsInteger(0, "SELECT IF(MAX(icardid) + 1 IS NULL, 1, MAX(icardid) + 1) AS icardid FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Cards WHERE ibaseid = " & iprojectid & " ORDER BY iupdatedate DESC, supdatetime DESC LIMIT 1")
                'iselectedcardid = cardid

                Dim queriesInsert(2) As String

                queriesInsert(0) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardInputs SELECT " & iprojectid & ", " & bf.icardid & ", iinputid, scardinputunit, dcardinputqty, " & fecha & ", '" & hora & "', '" & susername & "' FROM basecardinputs WHERE ibaseid = " & baseid & " AND icardid = " & bf.icardid
                queriesInsert(0) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardInputs SELECT " & iprojectid & ", " & bf.icardid & ", iinputid, scardinputunit, dcardinputqty, " & fecha & ", '" & hora & "', '" & susername & "' FROM basecardcompoundinputs WHERE ibaseid = " & baseid & " AND icardid = " & bf.icardid
                queriesInsert(1) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Cards SELECT " & iprojectid & ", " & bf.icardid & ", scardlegacycategoryid, scardlegacyid, scarddescription, scardunit, dcardqty, dcardindirectcostspercentage, dcardgainpercentage, " & fecha & ", '" & hora & "', '" & susername & "' FROM basecards WHERE ibaseid = " & baseid & " AND icardid = " & bf.icardid

                executeTransactedSQLCommand(0, queriesInsert)

            End If


            Dim porcentajeIVA As Double = 0.0
            porcentajeIVA = getSQLQueryAsDouble(0, "SELECT dbaseIVApercentage FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & " WHERE ibaseid = " & iprojectid)
            txtPorcentajeIVA.Text = porcentajeIVA * 100

        End If

        Dim dsCategorias As DataSet
        Dim contadorCategorias As Integer = 0
        Dim resumenDeTarjetas As String = ""
        dsCategorias = getSQLQueryAsDataset(0, "SELECT scardlegacycategoryid, scardlegacycategorydescription FROM cardlegacycategories")
        contadorCategorias = dsCategorias.Tables(0).Rows.Count

        Dim queriesFill(contadorCategorias) As String

        Dim queriesCreation(2) As String

        queriesCreation(0) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardsAux"
        queriesCreation(1) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardsAux (   scardid varchar(11) COLLATE latin1_spanish_ci NOT NULL,   scardlegacyid varchar(510) CHARACTER SET latin1 NOT NULL,   scarddescription VARCHAR(1000) CHARACTER SET latin1 NOT NULL,   scardunit varchar(49) CHARACTER SET latin1 NOT NULL,   sbasecardqty varchar(20) COLLATE latin1_spanish_ci NOT NULL,   scardindirectcosts varchar(20) COLLATE latin1_spanish_ci NOT NULL,   scardgain varchar(20) COLLATE latin1_spanish_ci NOT NULL,   scardunitaryprice varchar(20) COLLATE latin1_spanish_ci NOT NULL,   scardamount varchar(20) COLLATE latin1_spanish_ci NOT NULL ) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"

        executeTransactedSQLCommand(0, queriesCreation)


        Try

            For i As Integer = 0 To contadorCategorias - 1

                Dim queryContadorDeTarjetas As String = "" & _
                "SELECT COUNT(*) " & _
                "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Cards btf " & _
                "JOIN cardlegacycategories btflc ON btf.scardlegacycategoryid = btflc.scardlegacycategoryid " & _
                "JOIN (SELECT btfi.ibaseid, btfi.icardid, btfi.iinputid, (costoMO.costo*btfi.dcardinputqty) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardInputs btfi JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Cards btf ON btf.ibaseid = btfi.ibaseid AND btf.icardid = btfi.icardid JOIN (SELECT btfi.ibaseid, btfi.icardid AS icardid, 0 AS iinputid, SUM(btfi.dcardinputqty*bp.dinputfinalprice) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Cards btf LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardInputs btfi ON btf.ibaseid = btfi.ibaseid AND btf.icardid = btfi.icardid LEFT JOIN inputs i ON btfi.iinputid = i.iinputid JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) bp GROUP BY iinputid, ibaseid) bp ON btfi.ibaseid = bp.ibaseid AND i.iinputid = bp.iinputid LEFT JOIN inputtypes it ON i.iinputid = it.iinputid WHERE it.sinputtypedescription = 'MANO DE OBRA' GROUP BY btf.icardid, btfi.ibaseid ) AS costoMO ON btfi.iinputid = costoMO.iinputid AND btfi.icardid = costoMO.icardid GROUP BY btfi.icardid, btfi.ibaseid) AS costoEQ ON btf.ibaseid = costoEQ.ibaseid AND btf.icardid = costoEQ.icardid " & _
                "JOIN (SELECT btfi.ibaseid, btfi.icardid, btfi.iinputid, SUM(btfi.dcardinputqty*bp.dinputfinalprice) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Cards btf LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardInputs btfi ON btf.ibaseid = btfi.ibaseid AND btf.icardid = btfi.icardid LEFT JOIN inputs i ON btfi.iinputid = i.iinputid JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) bp GROUP BY iinputid, ibaseid) bp ON btfi.ibaseid = bp.ibaseid AND i.iinputid = bp.iinputid JOIN inputtypes it ON i.iinputid = it.iinputid WHERE it.sinputtypedescription = 'MANO DE OBRA' GROUP BY btf.icardid, btfi.ibaseid) AS costoMO ON btf.ibaseid = costoMO.ibaseid AND btf.icardid = costoMO.icardid " & _
                "LEFT JOIN (SELECT btfi.ibaseid, btfi.icardid, IF(SUM(btfi.dcardinputqty*bp.dinputfinalprice) IS NULL, 0, SUM(btfi.dcardinputqty*bp.dinputfinalprice))+IF(SUM(btfi.dcardinputqty*cibp.dinputfinalprice) IS NULL, 0, SUM(btfi.dcardinputqty*cibp.dinputfinalprice)) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Cards btf LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardInputs btfi ON btf.ibaseid = btfi.ibaseid AND btf.icardid = btfi.icardid LEFT JOIN inputs i ON btfi.iinputid = i.iinputid LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) bp GROUP BY iinputid, ibaseid) bp ON btfi.ibaseid = bp.ibaseid AND i.iinputid = bp.iinputid LEFT JOIN (SELECT btfi.ibaseid, btfi.icardid, btfi.iinputid, cibp.iupdatedate, cibp.supdatetime, SUM(btfci.dcompoundinputqty*cibp.dinputfinalprice) AS dinputpricewithoutIVA, 0.00000 AS dinputprotectionpercentage, SUM(btfci.dcompoundinputqty*cibp.dinputfinalprice) AS dinputfinalprice FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardInputs btfi JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardCompoundInputs btfci ON btfci.ibaseid = btfi.ibaseid AND btfci.icardid = btfi.icardid AND btfci.iinputid = btfi.iinputid LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) cibp GROUP BY iinputid, ibaseid) cibp ON cibp.ibaseid = btfci.ibaseid AND cibp.iinputid = btfci.icompoundinputid GROUP BY btfci.ibaseid, btfci.icardid, btfi.iinputid) cibp ON btfi.ibaseid = cibp.ibaseid AND btfi.icardid = cibp.icardid AND i.iinputid = cibp.iinputid JOIN inputtypes it ON i.iinputid = it.iinputid WHERE it.sinputtypedescription = 'MATERIALES' GROUP BY btfi.ibaseid, btfi.icardid ORDER BY btfi.ibaseid, btfi.icardid, btfi.iinputid) AS costoMAT ON btf.ibaseid = costoMAT.ibaseid AND btf.icardid = costoMAT.icardid " & _
                "WHERE btf.ibaseid = " & iprojectid & " AND btflc.scardlegacycategoryid = '" & dsCategorias.Tables(0).Rows(i).Item("scardlegacycategoryid") & "' "

                Dim contadorDeTarjetas As Integer = 0

                contadorDeTarjetas = getSQLQueryAsInteger(0, queryContadorDeTarjetas)

                If contadorDeTarjetas > 0 Then

                    queriesFill(i) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardsAux " & _
                    "SELECT '' AS 'icardid', CONCAT(btflc.scardlegacycategoryid, ' ', btflc.scardlegacycategorydescription) AS 'scardlegacyid', " & _
                    "'' AS 'scarddescription', '' AS 'scardunit', '' AS 'dcardqty', '' AS 'dcardindirectcosts', '' AS 'dcardgain', '' AS 'dcardunitaryprice', '' AS 'dcardsprice' FROM cardlegacycategories btflc WHERE btflc.scardlegacycategoryid = '" & dsCategorias.Tables(0).Rows(i).Item("scardlegacycategoryid") & "' " & _
                    "UNION SELECT btf.icardid, btf.scardlegacyid, btf.scarddescription, btf.scardunit, 1 AS dcardqty, " & _
                    "FORMAT(((IF(costoMAT.costo is null, 0, costoMAT.costo) + IF(costoMO.costo is null, 0, costoMO.costo) + IF(costoEQ.costo is null, 0, costoEQ.costo))*(btf.dcardindirectcostspercentage)*(1)), 2) AS dcardindirectcosts, " & _
                    "FORMAT(((IF(costoMAT.costo is null, 0, costoMAT.costo) + IF(costoMO.costo is null, 0, costoMO.costo) + IF(costoEQ.costo is null, 0, costoEQ.costo))*(1+btf.dcardindirectcostspercentage)*(btf.dcardgainpercentage)*(1)), 2) AS dcardgain, " & _
                    "FORMAT(((IF(costoMAT.costo is null, 0, costoMAT.costo) + IF(costoMO.costo is null, 0, costoMO.costo) + IF(costoEQ.costo is null, 0, costoEQ.costo))*(1+btf.dcardindirectcostspercentage)*(1+btf.dcardgainpercentage)), 2) AS dcardunitaryprice, " & _
                    "FORMAT(((IF(costoMAT.costo is null, 0, costoMAT.costo) + IF(costoMO.costo is null, 0, costoMO.costo) + IF(costoEQ.costo is null, 0, costoEQ.costo))*(1+btf.dcardindirectcostspercentage)*(1+btf.dcardgainpercentage)*(1)), 2) AS dcardsprice " & _
                    "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Cards btf " & _
                    "JOIN cardlegacycategories btflc ON btf.scardlegacycategoryid = btflc.scardlegacycategoryid " & _
                    "JOIN (SELECT btfi.ibaseid, btfi.icardid, btfi.iinputid, (costoMO.costo*btfi.dcardinputqty) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardInputs btfi JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Cards btf ON btf.ibaseid = btfi.ibaseid AND btf.icardid = btfi.icardid JOIN (SELECT btfi.ibaseid, btfi.icardid AS icardid, 0 AS iinputid, SUM(btfi.dcardinputqty*bp.dinputfinalprice) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Cards btf LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardInputs btfi ON btf.ibaseid = btfi.ibaseid AND btf.icardid = btfi.icardid LEFT JOIN inputs i ON btfi.iinputid = i.iinputid JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) bp GROUP BY iinputid, ibaseid) bp ON btfi.ibaseid = bp.ibaseid AND i.iinputid = bp.iinputid LEFT JOIN inputtypes it ON i.iinputid = it.iinputid WHERE it.sinputtypedescription = 'MANO DE OBRA' GROUP BY btf.icardid, btfi.ibaseid ) AS costoMO ON btfi.iinputid = costoMO.iinputid AND btfi.icardid = costoMO.icardid GROUP BY btfi.icardid, btfi.ibaseid) AS costoEQ ON btf.ibaseid = costoEQ.ibaseid AND btf.icardid = costoEQ.icardid " & _
                    "JOIN (SELECT btfi.ibaseid, btfi.icardid, btfi.iinputid, SUM(btfi.dcardinputqty*bp.dinputfinalprice) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Cards btf LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardInputs btfi ON btf.ibaseid = btfi.ibaseid AND btf.icardid = btfi.icardid LEFT JOIN inputs i ON btfi.iinputid = i.iinputid JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) bp GROUP BY iinputid, ibaseid) bp ON btfi.ibaseid = bp.ibaseid AND i.iinputid = bp.iinputid JOIN inputtypes it ON i.iinputid = it.iinputid WHERE it.sinputtypedescription = 'MANO DE OBRA' GROUP BY btf.icardid, btfi.ibaseid) AS costoMO ON btf.ibaseid = costoMO.ibaseid AND btf.icardid = costoMO.icardid " & _
                    "LEFT JOIN (SELECT btfi.ibaseid, btfi.icardid, IF(SUM(btfi.dcardinputqty*bp.dinputfinalprice) IS NULL, 0, SUM(btfi.dcardinputqty*bp.dinputfinalprice))+IF(SUM(btfi.dcardinputqty*cibp.dinputfinalprice) IS NULL, 0, SUM(btfi.dcardinputqty*cibp.dinputfinalprice)) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Cards btf LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardInputs btfi ON btf.ibaseid = btfi.ibaseid AND btf.icardid = btfi.icardid LEFT JOIN inputs i ON btfi.iinputid = i.iinputid LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) bp GROUP BY iinputid, ibaseid) bp ON btfi.ibaseid = bp.ibaseid AND i.iinputid = bp.iinputid LEFT JOIN (SELECT btfi.ibaseid, btfi.icardid, btfi.iinputid, cibp.iupdatedate, cibp.supdatetime, SUM(btfci.dcompoundinputqty*cibp.dinputfinalprice) AS dinputpricewithoutIVA, 0.00000 AS dinputprotectionpercentage, SUM(btfci.dcompoundinputqty*cibp.dinputfinalprice) AS dinputfinalprice FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardInputs btfi JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardCompoundInputs btfci ON btfci.ibaseid = btfi.ibaseid AND btfci.icardid = btfi.icardid AND btfci.iinputid = btfi.iinputid LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) cibp GROUP BY iinputid, ibaseid) cibp ON cibp.ibaseid = btfci.ibaseid AND cibp.iinputid = btfci.icompoundinputid GROUP BY btfci.ibaseid, btfci.icardid, btfi.iinputid) cibp ON btfi.ibaseid = cibp.ibaseid AND btfi.icardid = cibp.icardid AND i.iinputid = cibp.iinputid JOIN inputtypes it ON i.iinputid = it.iinputid WHERE it.sinputtypedescription = 'MATERIALES' GROUP BY btfi.ibaseid, btfi.icardid ORDER BY btfi.ibaseid, btfi.icardid, btfi.iinputid) AS costoMAT ON btf.ibaseid = costoMAT.ibaseid AND btf.icardid = costoMAT.icardid " & _
                    "WHERE btf.ibaseid = " & iprojectid & " AND btflc.scardlegacycategoryid = '" & dsCategorias.Tables(0).Rows(i).Item("scardlegacycategoryid") & "' ORDER BY scardlegacyid "

                End If

            Next i

            executeTransactedSQLCommand(0, queriesFill)

        Catch ex As Exception

        End Try


        setDataGridView(dgvResumenDeTarjetas, "SELECT scardid, scardlegacyid AS 'ID', scarddescription AS 'Descripcion Tarjeta', scardunit AS 'Unidad de Medida', sbasecardqty AS 'Cantidad', scardindirectcosts AS 'Indirectos', scardgain AS 'Utilidad', scardunitaryprice AS 'P.U.', scardamount AS 'Importe' FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardsAux", False)

        dgvResumenDeTarjetas.Columns(0).Visible = False

        If viewDgvIndirectCosts = False Then
            dgvResumenDeTarjetas.Columns(5).Visible = False
        End If

        If viewDgvProfits = False Then
            dgvResumenDeTarjetas.Columns(6).Visible = False
        End If

        If viewDgvUnitPrices = False Then
            dgvResumenDeTarjetas.Columns(7).Visible = False
        End If

        If viewDgvAmount = False Then
            dgvResumenDeTarjetas.Columns(8).Visible = False
        End If

        dgvResumenDeTarjetas.Columns(0).ReadOnly = True
        dgvResumenDeTarjetas.Columns(1).ReadOnly = True
        dgvResumenDeTarjetas.Columns(2).ReadOnly = True
        dgvResumenDeTarjetas.Columns(3).ReadOnly = True
        dgvResumenDeTarjetas.Columns(4).ReadOnly = True
        dgvResumenDeTarjetas.Columns(5).ReadOnly = True
        dgvResumenDeTarjetas.Columns(6).ReadOnly = True
        dgvResumenDeTarjetas.Columns(7).ReadOnly = True
        dgvResumenDeTarjetas.Columns(8).ReadOnly = True

        dgvResumenDeTarjetas.Columns(0).Width = 30
        dgvResumenDeTarjetas.Columns(1).Width = 60
        dgvResumenDeTarjetas.Columns(2).Width = 200
        dgvResumenDeTarjetas.Columns(3).Width = 60
        dgvResumenDeTarjetas.Columns(4).Width = 70
        dgvResumenDeTarjetas.Columns(5).Width = 70
        dgvResumenDeTarjetas.Columns(6).Width = 70
        dgvResumenDeTarjetas.Columns(7).Width = 70
        dgvResumenDeTarjetas.Columns(8).Width = 70

        isResumenDGVReady = True

        Cursor.Current = System.Windows.Forms.Cursors.Default

    End Sub


    Private Sub btnEliminarTarjeta_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEliminarTarjeta.Click

        If MsgBox("¿Está seguro que deseas eliminar la Tarjeta " & sselectedcardlegacyid & " del Presupuesto Base?", MsgBoxStyle.YesNo, "Confirmar Eliminación de Tarjeta") = MsgBoxResult.Yes Then

            Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

            Dim tmpselectedcardid As Integer = 0
            Try
                tmpselectedcardid = CInt(dgvResumenDeTarjetas.CurrentRow.Cells(0).Value)
            Catch ex As Exception

            End Try

            Dim queriesDelete(3) As String

            queriesDelete(0) = "DELETE FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Cards WHERE ibaseid = " & iprojectid & " AND icardid = " & tmpselectedcardid
            queriesDelete(1) = "DELETE FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardInputs WHERE ibaseid = " & iprojectid & " AND icardid = " & tmpselectedcardid
            queriesDelete(2) = "DELETE FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardCompoundInputs WHERE ibaseid = " & iprojectid & " AND icardid = " & tmpselectedcardid

            executeTransactedSQLCommand(0, queriesDelete)


            Dim dsCategorias As DataSet
            Dim contadorCategorias As Integer = 0
            Dim resumenDeTarjetas As String = ""
            dsCategorias = getSQLQueryAsDataset(0, "SELECT scardlegacycategoryid, scardlegacycategorydescription FROM cardlegacycategories")
            contadorCategorias = dsCategorias.Tables(0).Rows.Count

            Dim queriesFill(contadorCategorias) As String

            Dim queriesCreation(2) As String

            queriesCreation(0) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardsAux"
            queriesCreation(1) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardsAux (   scardid varchar(11) COLLATE latin1_spanish_ci NOT NULL,   scardlegacyid varchar(510) CHARACTER SET latin1 NOT NULL,   scarddescription VARCHAR(1000) CHARACTER SET latin1 NOT NULL,   scardunit varchar(49) CHARACTER SET latin1 NOT NULL,   sbasecardqty varchar(20) COLLATE latin1_spanish_ci NOT NULL,   scardindirectcosts varchar(20) COLLATE latin1_spanish_ci NOT NULL,   scardgain varchar(20) COLLATE latin1_spanish_ci NOT NULL,   scardunitaryprice varchar(20) COLLATE latin1_spanish_ci NOT NULL,   scardamount varchar(20) COLLATE latin1_spanish_ci NOT NULL ) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"

            executeTransactedSQLCommand(0, queriesCreation)


            Try

                For i As Integer = 0 To contadorCategorias - 1

                    Dim queryContadorDeTarjetas As String = "" & _
                    "SELECT COUNT(*) " & _
                    "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Cards btf " & _
                    "JOIN cardlegacycategories btflc ON btf.scardlegacycategoryid = btflc.scardlegacycategoryid " & _
                    "JOIN (SELECT btfi.ibaseid, btfi.icardid, btfi.iinputid, (costoMO.costo*btfi.dcardinputqty) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardInputs btfi JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Cards btf ON btf.ibaseid = btfi.ibaseid AND btf.icardid = btfi.icardid JOIN (SELECT btfi.ibaseid, btfi.icardid AS icardid, 0 AS iinputid, SUM(btfi.dcardinputqty*bp.dinputfinalprice) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Cards btf LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardInputs btfi ON btf.ibaseid = btfi.ibaseid AND btf.icardid = btfi.icardid LEFT JOIN inputs i ON btfi.iinputid = i.iinputid JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) bp GROUP BY iinputid, ibaseid) bp ON btfi.ibaseid = bp.ibaseid AND i.iinputid = bp.iinputid LEFT JOIN inputtypes it ON i.iinputid = it.iinputid WHERE it.sinputtypedescription = 'MANO DE OBRA' GROUP BY btf.icardid, btfi.ibaseid ) AS costoMO ON btfi.iinputid = costoMO.iinputid AND btfi.icardid = costoMO.icardid GROUP BY btfi.icardid, btfi.ibaseid) AS costoEQ ON btf.ibaseid = costoEQ.ibaseid AND btf.icardid = costoEQ.icardid " & _
                    "JOIN (SELECT btfi.ibaseid, btfi.icardid, btfi.iinputid, SUM(btfi.dcardinputqty*bp.dinputfinalprice) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Cards btf LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardInputs btfi ON btf.ibaseid = btfi.ibaseid AND btf.icardid = btfi.icardid LEFT JOIN inputs i ON btfi.iinputid = i.iinputid JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) bp GROUP BY iinputid, ibaseid) bp ON btfi.ibaseid = bp.ibaseid AND i.iinputid = bp.iinputid JOIN inputtypes it ON i.iinputid = it.iinputid WHERE it.sinputtypedescription = 'MANO DE OBRA' GROUP BY btf.icardid, btfi.ibaseid) AS costoMO ON btf.ibaseid = costoMO.ibaseid AND btf.icardid = costoMO.icardid " & _
                    "LEFT JOIN (SELECT btfi.ibaseid, btfi.icardid, IF(SUM(btfi.dcardinputqty*bp.dinputfinalprice) IS NULL, 0, SUM(btfi.dcardinputqty*bp.dinputfinalprice))+IF(SUM(btfi.dcardinputqty*cibp.dinputfinalprice) IS NULL, 0, SUM(btfi.dcardinputqty*cibp.dinputfinalprice)) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Cards btf LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardInputs btfi ON btf.ibaseid = btfi.ibaseid AND btf.icardid = btfi.icardid LEFT JOIN inputs i ON btfi.iinputid = i.iinputid LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) bp GROUP BY iinputid, ibaseid) bp ON btfi.ibaseid = bp.ibaseid AND i.iinputid = bp.iinputid LEFT JOIN (SELECT btfi.ibaseid, btfi.icardid, btfi.iinputid, cibp.iupdatedate, cibp.supdatetime, SUM(btfci.dcompoundinputqty*cibp.dinputfinalprice) AS dinputpricewithoutIVA, 0.00000 AS dinputprotectionpercentage, SUM(btfci.dcompoundinputqty*cibp.dinputfinalprice) AS dinputfinalprice FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardInputs btfi JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardCompoundInputs btfci ON btfci.ibaseid = btfi.ibaseid AND btfci.icardid = btfi.icardid AND btfci.iinputid = btfi.iinputid LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) cibp GROUP BY iinputid, ibaseid) cibp ON cibp.ibaseid = btfci.ibaseid AND cibp.iinputid = btfci.icompoundinputid GROUP BY btfci.ibaseid, btfci.icardid, btfi.iinputid) cibp ON btfi.ibaseid = cibp.ibaseid AND btfi.icardid = cibp.icardid AND i.iinputid = cibp.iinputid JOIN inputtypes it ON i.iinputid = it.iinputid WHERE it.sinputtypedescription = 'MATERIALES' GROUP BY btfi.ibaseid, btfi.icardid ORDER BY btfi.ibaseid, btfi.icardid, btfi.iinputid) AS costoMAT ON btf.ibaseid = costoMAT.ibaseid AND btf.icardid = costoMAT.icardid " & _
                    "WHERE btf.ibaseid = " & iprojectid & " AND btflc.scardlegacycategoryid = '" & dsCategorias.Tables(0).Rows(i).Item("scardlegacycategoryid") & "' "

                    Dim contadorDeTarjetas As Integer = 0

                    contadorDeTarjetas = getSQLQueryAsInteger(0, queryContadorDeTarjetas)

                    If contadorDeTarjetas > 0 Then

                        queriesFill(i) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardsAux " & _
                        "SELECT '' AS 'icardid', CONCAT(btflc.scardlegacycategoryid, ' ', btflc.scardlegacycategorydescription) AS 'scardlegacyid', " & _
                        "'' AS 'scarddescription', '' AS 'scardunit', '' AS 'dcardqty', '' AS 'dcardindirectcosts', '' AS 'dcardgain', '' AS 'dcardunitaryprice', '' AS 'dcardsprice' FROM cardlegacycategories btflc WHERE btflc.scardlegacycategoryid = '" & dsCategorias.Tables(0).Rows(i).Item("scardlegacycategoryid") & "' " & _
                        "UNION SELECT btf.icardid, btf.scardlegacyid, btf.scarddescription, btf.scardunit, 1 AS dcardqty, " & _
                        "FORMAT(((IF(costoMAT.costo is null, 0, costoMAT.costo) + IF(costoMO.costo is null, 0, costoMO.costo) + IF(costoEQ.costo is null, 0, costoEQ.costo))*(btf.dcardindirectcostspercentage)*(1)), 2) AS dcardindirectcosts, " & _
                        "FORMAT(((IF(costoMAT.costo is null, 0, costoMAT.costo) + IF(costoMO.costo is null, 0, costoMO.costo) + IF(costoEQ.costo is null, 0, costoEQ.costo))*(1+btf.dcardindirectcostspercentage)*(btf.dcardgainpercentage)*(1)), 2) AS dcardgain, " & _
                        "FORMAT(((IF(costoMAT.costo is null, 0, costoMAT.costo) + IF(costoMO.costo is null, 0, costoMO.costo) + IF(costoEQ.costo is null, 0, costoEQ.costo))*(1+btf.dcardindirectcostspercentage)*(1+btf.dcardgainpercentage)), 2) AS dcardunitaryprice, " & _
                        "FORMAT(((IF(costoMAT.costo is null, 0, costoMAT.costo) + IF(costoMO.costo is null, 0, costoMO.costo) + IF(costoEQ.costo is null, 0, costoEQ.costo))*(1+btf.dcardindirectcostspercentage)*(1+btf.dcardgainpercentage)*(1)), 2) AS dcardsprice " & _
                        "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Cards btf " & _
                        "JOIN cardlegacycategories btflc ON btf.scardlegacycategoryid = btflc.scardlegacycategoryid " & _
                        "JOIN (SELECT btfi.ibaseid, btfi.icardid, btfi.iinputid, (costoMO.costo*btfi.dcardinputqty) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardInputs btfi JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Cards btf ON btf.ibaseid = btfi.ibaseid AND btf.icardid = btfi.icardid JOIN (SELECT btfi.ibaseid, btfi.icardid AS icardid, 0 AS iinputid, SUM(btfi.dcardinputqty*bp.dinputfinalprice) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Cards btf LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardInputs btfi ON btf.ibaseid = btfi.ibaseid AND btf.icardid = btfi.icardid LEFT JOIN inputs i ON btfi.iinputid = i.iinputid JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) bp GROUP BY iinputid, ibaseid) bp ON btfi.ibaseid = bp.ibaseid AND i.iinputid = bp.iinputid LEFT JOIN inputtypes it ON i.iinputid = it.iinputid WHERE it.sinputtypedescription = 'MANO DE OBRA' GROUP BY btf.icardid, btfi.ibaseid ) AS costoMO ON btfi.iinputid = costoMO.iinputid AND btfi.icardid = costoMO.icardid GROUP BY btfi.icardid, btfi.ibaseid) AS costoEQ ON btf.ibaseid = costoEQ.ibaseid AND btf.icardid = costoEQ.icardid " & _
                        "JOIN (SELECT btfi.ibaseid, btfi.icardid, btfi.iinputid, SUM(btfi.dcardinputqty*bp.dinputfinalprice) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Cards btf LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardInputs btfi ON btf.ibaseid = btfi.ibaseid AND btf.icardid = btfi.icardid LEFT JOIN inputs i ON btfi.iinputid = i.iinputid JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) bp GROUP BY iinputid, ibaseid) bp ON btfi.ibaseid = bp.ibaseid AND i.iinputid = bp.iinputid JOIN inputtypes it ON i.iinputid = it.iinputid WHERE it.sinputtypedescription = 'MANO DE OBRA' GROUP BY btf.icardid, btfi.ibaseid) AS costoMO ON btf.ibaseid = costoMO.ibaseid AND btf.icardid = costoMO.icardid " & _
                        "LEFT JOIN (SELECT btfi.ibaseid, btfi.icardid, IF(SUM(btfi.dcardinputqty*bp.dinputfinalprice) IS NULL, 0, SUM(btfi.dcardinputqty*bp.dinputfinalprice))+IF(SUM(btfi.dcardinputqty*cibp.dinputfinalprice) IS NULL, 0, SUM(btfi.dcardinputqty*cibp.dinputfinalprice)) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Cards btf LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardInputs btfi ON btf.ibaseid = btfi.ibaseid AND btf.icardid = btfi.icardid LEFT JOIN inputs i ON btfi.iinputid = i.iinputid LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) bp GROUP BY iinputid, ibaseid) bp ON btfi.ibaseid = bp.ibaseid AND i.iinputid = bp.iinputid LEFT JOIN (SELECT btfi.ibaseid, btfi.icardid, btfi.iinputid, cibp.iupdatedate, cibp.supdatetime, SUM(btfci.dcompoundinputqty*cibp.dinputfinalprice) AS dinputpricewithoutIVA, 0.00000 AS dinputprotectionpercentage, SUM(btfci.dcompoundinputqty*cibp.dinputfinalprice) AS dinputfinalprice FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardInputs btfi JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardCompoundInputs btfci ON btfci.ibaseid = btfi.ibaseid AND btfci.icardid = btfi.icardid AND btfci.iinputid = btfi.iinputid LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) cibp GROUP BY iinputid, ibaseid) cibp ON cibp.ibaseid = btfci.ibaseid AND cibp.iinputid = btfci.icompoundinputid GROUP BY btfci.ibaseid, btfci.icardid, btfi.iinputid) cibp ON btfi.ibaseid = cibp.ibaseid AND btfi.icardid = cibp.icardid AND i.iinputid = cibp.iinputid JOIN inputtypes it ON i.iinputid = it.iinputid WHERE it.sinputtypedescription = 'MATERIALES' GROUP BY btfi.ibaseid, btfi.icardid ORDER BY btfi.ibaseid, btfi.icardid, btfi.iinputid) AS costoMAT ON btf.ibaseid = costoMAT.ibaseid AND btf.icardid = costoMAT.icardid " & _
                        "WHERE btf.ibaseid = " & iprojectid & " AND btflc.scardlegacycategoryid = '" & dsCategorias.Tables(0).Rows(i).Item("scardlegacycategoryid") & "' ORDER BY scardlegacyid "

                    End If

                Next i

                executeTransactedSQLCommand(0, queriesFill)

            Catch ex As Exception

            End Try


            setDataGridView(dgvResumenDeTarjetas, "SELECT scardid, scardlegacyid AS 'ID', scarddescription AS 'Descripcion Tarjeta', scardunit AS 'Unidad de Medida', sbasecardqty AS 'Cantidad', scardindirectcosts AS 'Indirectos', scardgain AS 'Utilidad', scardunitaryprice AS 'P.U.', scardamount AS 'Importe' FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardsAux", False)

            dgvResumenDeTarjetas.Columns(0).Visible = False

            If viewDgvIndirectCosts = False Then
                dgvResumenDeTarjetas.Columns(5).Visible = False
            End If

            If viewDgvProfits = False Then
                dgvResumenDeTarjetas.Columns(6).Visible = False
            End If

            If viewDgvUnitPrices = False Then
                dgvResumenDeTarjetas.Columns(7).Visible = False
            End If

            If viewDgvAmount = False Then
                dgvResumenDeTarjetas.Columns(8).Visible = False
            End If

            dgvResumenDeTarjetas.Columns(0).ReadOnly = True
            dgvResumenDeTarjetas.Columns(1).ReadOnly = True
            dgvResumenDeTarjetas.Columns(2).ReadOnly = True
            dgvResumenDeTarjetas.Columns(3).ReadOnly = True
            dgvResumenDeTarjetas.Columns(4).ReadOnly = True
            dgvResumenDeTarjetas.Columns(5).ReadOnly = True
            dgvResumenDeTarjetas.Columns(6).ReadOnly = True
            dgvResumenDeTarjetas.Columns(7).ReadOnly = True
            dgvResumenDeTarjetas.Columns(8).ReadOnly = True

            dgvResumenDeTarjetas.Columns(0).Width = 30
            dgvResumenDeTarjetas.Columns(1).Width = 60
            dgvResumenDeTarjetas.Columns(2).Width = 200
            dgvResumenDeTarjetas.Columns(3).Width = 60
            dgvResumenDeTarjetas.Columns(4).Width = 70
            dgvResumenDeTarjetas.Columns(5).Width = 70
            dgvResumenDeTarjetas.Columns(6).Width = 70
            dgvResumenDeTarjetas.Columns(7).Width = 70
            dgvResumenDeTarjetas.Columns(8).Width = 70

            isResumenDGVReady = True

            Cursor.Current = System.Windows.Forms.Cursors.Default


        End If

    End Sub


    Private Function validaResumenDeTarjetas(ByVal silent As Boolean) As Boolean

        If isFormReadyForAction = False Then
            Exit Function
        End If


        Dim porcentajeIVA As Double = 0.0

        Try
            porcentajeIVA = CDbl(txtPorcentajeIVA.Text)

        Catch ex As Exception

        End Try

        If porcentajeIVA = 0 Then

            tcBase.SelectedTab = tpResumenTarjetas
            txtPorcentajeIVA.Select()
            txtPorcentajeIVA.Focus()

            If silent = False Then
                MsgBox("¿Podrías poner el porcentaje de IVA aplicable a las Tarjetas del Presupuesto Base?", MsgBoxStyle.OkOnly, "Dato Faltante")
            End If

            alertaMostrada = True
            Return False

        End If

        If isEdit = False Then

            If paso4 = False Then

                Return False

            End If

        End If

        Return True


    End Function


    Private Sub btnActualizarUtilidad_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnActualizarUtilidad.Click

        If MsgBox("¿Está seguro de que deseas actualizar los Porcentajes de Indirectos y Utilidades para TODAS las Tarjetas de este Presupuesto Base?", MsgBoxStyle.YesNo, "Confirmación de Actualización de Utilidades e Indirectos") = MsgBoxResult.Yes Then

            Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

            paso4 = True

            If validaResumenDeTarjetas(False) = False Then
                paso4 = False
                Exit Sub
            End If

            Dim ciyu As New AgregarProyectoCambiaIndirectosYUtilidad
            ciyu.susername = susername
            ciyu.bactive = bactive
            ciyu.bonline = bonline
            ciyu.suserfullname = suserfullname

            ciyu.suseremail = suseremail
            ciyu.susersession = susersession
            ciyu.susermachinename = susermachinename
            ciyu.suserip = suserip

            ciyu.isBase = True
            ciyu.IsModel = False

            ciyu.iprojectid = iprojectid

            Me.Visible = False
            ciyu.ShowDialog(Me)
            Me.Visible = True

            If ciyu.DialogResult = Windows.Forms.DialogResult.OK Then

                'Acá van los querys del tab de Resumen de Tarjetas (del Load)
                Dim dsCategorias As DataSet
                Dim contadorCategorias As Integer = 0
                Dim resumenDeTarjetas As String = ""
                dsCategorias = getSQLQueryAsDataset(0, "SELECT scardlegacycategoryid, scardlegacycategorydescription FROM cardlegacycategories")
                contadorCategorias = dsCategorias.Tables(0).Rows.Count

                Dim queriesFill(contadorCategorias) As String

                Dim queriesCreation(2) As String

                queriesCreation(0) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardsAux"
                queriesCreation(1) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardsAux (   scardid varchar(11) COLLATE latin1_spanish_ci NOT NULL,   scardlegacyid varchar(510) CHARACTER SET latin1 NOT NULL,   scarddescription VARCHAR(1000) CHARACTER SET latin1 NOT NULL,   scardunit varchar(49) CHARACTER SET latin1 NOT NULL,   sbasecardqty varchar(20) COLLATE latin1_spanish_ci NOT NULL,   scardindirectcosts varchar(20) COLLATE latin1_spanish_ci NOT NULL,   scardgain varchar(20) COLLATE latin1_spanish_ci NOT NULL,   scardunitaryprice varchar(20) COLLATE latin1_spanish_ci NOT NULL,   scardamount varchar(20) COLLATE latin1_spanish_ci NOT NULL ) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"

                executeTransactedSQLCommand(0, queriesCreation)

                Try

                    For i As Integer = 0 To contadorCategorias - 1

                        Dim queryContadorDeTarjetas As String = "" & _
                        "SELECT COUNT(*) " & _
                        "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Cards btf " & _
                        "JOIN cardlegacycategories btflc ON btf.scardlegacycategoryid = btflc.scardlegacycategoryid " & _
                        "JOIN (SELECT btfi.ibaseid, btfi.icardid, btfi.iinputid, (costoMO.costo*btfi.dcardinputqty) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardInputs btfi JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Cards btf ON btf.ibaseid = btfi.ibaseid AND btf.icardid = btfi.icardid JOIN (SELECT btfi.ibaseid, btfi.icardid AS icardid, 0 AS iinputid, SUM(btfi.dcardinputqty*bp.dinputfinalprice) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Cards btf LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardInputs btfi ON btf.ibaseid = btfi.ibaseid AND btf.icardid = btfi.icardid LEFT JOIN inputs i ON btfi.iinputid = i.iinputid JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) bp GROUP BY iinputid, ibaseid) bp ON btfi.ibaseid = bp.ibaseid AND i.iinputid = bp.iinputid LEFT JOIN inputtypes it ON i.iinputid = it.iinputid WHERE it.sinputtypedescription = 'MANO DE OBRA' GROUP BY btf.icardid, btfi.ibaseid ) AS costoMO ON btfi.iinputid = costoMO.iinputid AND btfi.icardid = costoMO.icardid GROUP BY btfi.icardid, btfi.ibaseid) AS costoEQ ON btf.ibaseid = costoEQ.ibaseid AND btf.icardid = costoEQ.icardid " & _
                        "JOIN (SELECT btfi.ibaseid, btfi.icardid, btfi.iinputid, SUM(btfi.dcardinputqty*bp.dinputfinalprice) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Cards btf LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardInputs btfi ON btf.ibaseid = btfi.ibaseid AND btf.icardid = btfi.icardid LEFT JOIN inputs i ON btfi.iinputid = i.iinputid JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) bp GROUP BY iinputid, ibaseid) bp ON btfi.ibaseid = bp.ibaseid AND i.iinputid = bp.iinputid JOIN inputtypes it ON i.iinputid = it.iinputid WHERE it.sinputtypedescription = 'MANO DE OBRA' GROUP BY btf.icardid, btfi.ibaseid) AS costoMO ON btf.ibaseid = costoMO.ibaseid AND btf.icardid = costoMO.icardid " & _
                        "LEFT JOIN (SELECT btfi.ibaseid, btfi.icardid, IF(SUM(btfi.dcardinputqty*bp.dinputfinalprice) IS NULL, 0, SUM(btfi.dcardinputqty*bp.dinputfinalprice))+IF(SUM(btfi.dcardinputqty*cibp.dinputfinalprice) IS NULL, 0, SUM(btfi.dcardinputqty*cibp.dinputfinalprice)) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Cards btf LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardInputs btfi ON btf.ibaseid = btfi.ibaseid AND btf.icardid = btfi.icardid LEFT JOIN inputs i ON btfi.iinputid = i.iinputid LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) bp GROUP BY iinputid, ibaseid) bp ON btfi.ibaseid = bp.ibaseid AND i.iinputid = bp.iinputid LEFT JOIN (SELECT btfi.ibaseid, btfi.icardid, btfi.iinputid, cibp.iupdatedate, cibp.supdatetime, SUM(btfci.dcompoundinputqty*cibp.dinputfinalprice) AS dinputpricewithoutIVA, 0.00000 AS dinputprotectionpercentage, SUM(btfci.dcompoundinputqty*cibp.dinputfinalprice) AS dinputfinalprice FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardInputs btfi JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardCompoundInputs btfci ON btfci.ibaseid = btfi.ibaseid AND btfci.icardid = btfi.icardid AND btfci.iinputid = btfi.iinputid LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) cibp GROUP BY iinputid, ibaseid) cibp ON cibp.ibaseid = btfci.ibaseid AND cibp.iinputid = btfci.icompoundinputid GROUP BY btfci.ibaseid, btfci.icardid, btfi.iinputid) cibp ON btfi.ibaseid = cibp.ibaseid AND btfi.icardid = cibp.icardid AND i.iinputid = cibp.iinputid JOIN inputtypes it ON i.iinputid = it.iinputid WHERE it.sinputtypedescription = 'MATERIALES' GROUP BY btfi.ibaseid, btfi.icardid ORDER BY btfi.ibaseid, btfi.icardid, btfi.iinputid) AS costoMAT ON btf.ibaseid = costoMAT.ibaseid AND btf.icardid = costoMAT.icardid " & _
                        "WHERE btf.ibaseid = " & iprojectid & " AND btflc.scardlegacycategoryid = '" & dsCategorias.Tables(0).Rows(i).Item("scardlegacycategoryid") & "' "

                        Dim contadorDeTarjetas As Integer = 0

                        contadorDeTarjetas = getSQLQueryAsInteger(0, queryContadorDeTarjetas)

                        If contadorDeTarjetas > 0 Then

                            queriesFill(i) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardsAux " & _
                            "SELECT '' AS 'icardid', CONCAT(btflc.scardlegacycategoryid, ' ', btflc.scardlegacycategorydescription) AS 'scardlegacyid', " & _
                            "'' AS 'scarddescription', '' AS 'scardunit', '' AS 'dcardqty', '' AS 'dcardindirectcosts', '' AS 'dcardgain', '' AS 'dcardunitaryprice', '' AS 'dcardsprice' FROM cardlegacycategories btflc WHERE btflc.scardlegacycategoryid = '" & dsCategorias.Tables(0).Rows(i).Item("scardlegacycategoryid") & "' " & _
                            "UNION SELECT btf.icardid, btf.scardlegacyid, btf.scarddescription, btf.scardunit, 1 AS dcardqty, " & _
                            "FORMAT(((IF(costoMAT.costo is null, 0, costoMAT.costo) + IF(costoMO.costo is null, 0, costoMO.costo) + IF(costoEQ.costo is null, 0, costoEQ.costo))*(btf.dcardindirectcostspercentage)*(1)), 2) AS dcardindirectcosts, " & _
                            "FORMAT(((IF(costoMAT.costo is null, 0, costoMAT.costo) + IF(costoMO.costo is null, 0, costoMO.costo) + IF(costoEQ.costo is null, 0, costoEQ.costo))*(1+btf.dcardindirectcostspercentage)*(btf.dcardgainpercentage)*(1)), 2) AS dcardgain, " & _
                            "FORMAT(((IF(costoMAT.costo is null, 0, costoMAT.costo) + IF(costoMO.costo is null, 0, costoMO.costo) + IF(costoEQ.costo is null, 0, costoEQ.costo))*(1+btf.dcardindirectcostspercentage)*(1+btf.dcardgainpercentage)), 2) AS dcardunitaryprice, " & _
                            "FORMAT(((IF(costoMAT.costo is null, 0, costoMAT.costo) + IF(costoMO.costo is null, 0, costoMO.costo) + IF(costoEQ.costo is null, 0, costoEQ.costo))*(1+btf.dcardindirectcostspercentage)*(1+btf.dcardgainpercentage)*(1)), 2) AS dcardsprice " & _
                            "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Cards btf " & _
                            "JOIN cardlegacycategories btflc ON btf.scardlegacycategoryid = btflc.scardlegacycategoryid " & _
                            "JOIN (SELECT btfi.ibaseid, btfi.icardid, btfi.iinputid, (costoMO.costo*btfi.dcardinputqty) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardInputs btfi JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Cards btf ON btf.ibaseid = btfi.ibaseid AND btf.icardid = btfi.icardid JOIN (SELECT btfi.ibaseid, btfi.icardid AS icardid, 0 AS iinputid, SUM(btfi.dcardinputqty*bp.dinputfinalprice) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Cards btf LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardInputs btfi ON btf.ibaseid = btfi.ibaseid AND btf.icardid = btfi.icardid LEFT JOIN inputs i ON btfi.iinputid = i.iinputid JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) bp GROUP BY iinputid, ibaseid) bp ON btfi.ibaseid = bp.ibaseid AND i.iinputid = bp.iinputid LEFT JOIN inputtypes it ON i.iinputid = it.iinputid WHERE it.sinputtypedescription = 'MANO DE OBRA' GROUP BY btf.icardid, btfi.ibaseid ) AS costoMO ON btfi.iinputid = costoMO.iinputid AND btfi.icardid = costoMO.icardid GROUP BY btfi.icardid, btfi.ibaseid) AS costoEQ ON btf.ibaseid = costoEQ.ibaseid AND btf.icardid = costoEQ.icardid " & _
                            "JOIN (SELECT btfi.ibaseid, btfi.icardid, btfi.iinputid, SUM(btfi.dcardinputqty*bp.dinputfinalprice) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Cards btf LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardInputs btfi ON btf.ibaseid = btfi.ibaseid AND btf.icardid = btfi.icardid LEFT JOIN inputs i ON btfi.iinputid = i.iinputid JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) bp GROUP BY iinputid, ibaseid) bp ON btfi.ibaseid = bp.ibaseid AND i.iinputid = bp.iinputid JOIN inputtypes it ON i.iinputid = it.iinputid WHERE it.sinputtypedescription = 'MANO DE OBRA' GROUP BY btf.icardid, btfi.ibaseid) AS costoMO ON btf.ibaseid = costoMO.ibaseid AND btf.icardid = costoMO.icardid " & _
                            "LEFT JOIN (SELECT btfi.ibaseid, btfi.icardid, IF(SUM(btfi.dcardinputqty*bp.dinputfinalprice) IS NULL, 0, SUM(btfi.dcardinputqty*bp.dinputfinalprice))+IF(SUM(btfi.dcardinputqty*cibp.dinputfinalprice) IS NULL, 0, SUM(btfi.dcardinputqty*cibp.dinputfinalprice)) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Cards btf LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardInputs btfi ON btf.ibaseid = btfi.ibaseid AND btf.icardid = btfi.icardid LEFT JOIN inputs i ON btfi.iinputid = i.iinputid LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) bp GROUP BY iinputid, ibaseid) bp ON btfi.ibaseid = bp.ibaseid AND i.iinputid = bp.iinputid LEFT JOIN (SELECT btfi.ibaseid, btfi.icardid, btfi.iinputid, cibp.iupdatedate, cibp.supdatetime, SUM(btfci.dcompoundinputqty*cibp.dinputfinalprice) AS dinputpricewithoutIVA, 0.00000 AS dinputprotectionpercentage, SUM(btfci.dcompoundinputqty*cibp.dinputfinalprice) AS dinputfinalprice FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardInputs btfi JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardCompoundInputs btfci ON btfci.ibaseid = btfi.ibaseid AND btfci.icardid = btfi.icardid AND btfci.iinputid = btfi.iinputid LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) cibp GROUP BY iinputid, ibaseid) cibp ON cibp.ibaseid = btfci.ibaseid AND cibp.iinputid = btfci.icompoundinputid GROUP BY btfci.ibaseid, btfci.icardid, btfi.iinputid) cibp ON btfi.ibaseid = cibp.ibaseid AND btfi.icardid = cibp.icardid AND i.iinputid = cibp.iinputid JOIN inputtypes it ON i.iinputid = it.iinputid WHERE it.sinputtypedescription = 'MATERIALES' GROUP BY btfi.ibaseid, btfi.icardid ORDER BY btfi.ibaseid, btfi.icardid, btfi.iinputid) AS costoMAT ON btf.ibaseid = costoMAT.ibaseid AND btf.icardid = costoMAT.icardid " & _
                            "WHERE btf.ibaseid = " & iprojectid & " AND btflc.scardlegacycategoryid = '" & dsCategorias.Tables(0).Rows(i).Item("scardlegacycategoryid") & "' ORDER BY scardlegacyid "

                        End If

                    Next i

                    executeTransactedSQLCommand(0, queriesFill)

                Catch ex As Exception

                End Try


                setDataGridView(dgvResumenDeTarjetas, "SELECT scardid, scardlegacyid AS 'ID', scarddescription AS 'Descripcion Tarjeta', scardunit AS 'Unidad de Medida', sbasecardqty AS 'Cantidad', scardindirectcosts AS 'Indirectos', scardgain AS 'Utilidad', scardunitaryprice AS 'P.U.', scardamount AS 'Importe' FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardsAux", False)

                dgvResumenDeTarjetas.Columns(0).Visible = False

                If viewDgvIndirectCosts = False Then
                    dgvResumenDeTarjetas.Columns(5).Visible = False
                End If

                If viewDgvProfits = False Then
                    dgvResumenDeTarjetas.Columns(6).Visible = False
                End If

                If viewDgvUnitPrices = False Then
                    dgvResumenDeTarjetas.Columns(7).Visible = False
                End If

                If viewDgvAmount = False Then
                    dgvResumenDeTarjetas.Columns(8).Visible = False
                End If

                dgvResumenDeTarjetas.Columns(0).ReadOnly = True
                dgvResumenDeTarjetas.Columns(1).ReadOnly = True
                dgvResumenDeTarjetas.Columns(2).ReadOnly = True
                dgvResumenDeTarjetas.Columns(3).ReadOnly = True
                dgvResumenDeTarjetas.Columns(4).ReadOnly = True
                dgvResumenDeTarjetas.Columns(5).ReadOnly = True
                dgvResumenDeTarjetas.Columns(6).ReadOnly = True
                dgvResumenDeTarjetas.Columns(7).ReadOnly = True
                dgvResumenDeTarjetas.Columns(8).ReadOnly = True

                dgvResumenDeTarjetas.Columns(0).Width = 30
                dgvResumenDeTarjetas.Columns(1).Width = 60
                dgvResumenDeTarjetas.Columns(2).Width = 200
                dgvResumenDeTarjetas.Columns(3).Width = 60
                dgvResumenDeTarjetas.Columns(4).Width = 70
                dgvResumenDeTarjetas.Columns(5).Width = 70
                dgvResumenDeTarjetas.Columns(6).Width = 70
                dgvResumenDeTarjetas.Columns(7).Width = 70
                dgvResumenDeTarjetas.Columns(8).Width = 70


                Dim porcentajeIVA As Double = 0.0
                porcentajeIVA = getSQLQueryAsDouble(0, "SELECT dbaseIVApercentage FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & " WHERE ibaseid = " & iprojectid)
                txtPorcentajeIVA.Text = porcentajeIVA * 100

            End If

            Cursor.Current = System.Windows.Forms.Cursors.Default

        End If

    End Sub


    Private Sub txtPorcentajeDeComisionDeObraPresupuestada_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtPorcentajeIndirectosDefault.KeyUp

        Dim strcaracteresprohibidos As String = "abcdefghijklmnopqrstuvwxyzñABCDEFGHIJKLMNOPQRSTUVWXYZÑ|°!#$%&/()=?¡*¨[]_:;,-{}+´¿'¬^`~@\<>"
        Dim arrayCaractProhib As Char() = strcaracteresprohibidos.ToCharArray
        Dim resultado As Boolean = False

        For carp = 0 To arrayCaractProhib.Length - 1

            If txtPorcentajeIndirectosDefault.Text.Contains(arrayCaractProhib(carp)) Then
                txtPorcentajeIndirectosDefault.Text = txtPorcentajeIndirectosDefault.Text.Replace(arrayCaractProhib(carp), "")
                resultado = True
            End If

        Next carp

        If txtPorcentajeIndirectosDefault.Text.Contains(".") Then

            Dim comparaPuntos As Char() = txtPorcentajeIndirectosDefault.Text.ToCharArray
            Dim cuantosPuntos As Integer = 0


            For letra = 0 To comparaPuntos.Length - 1

                If comparaPuntos(letra) = "." Then
                    cuantosPuntos = cuantosPuntos + 1
                End If

            Next

            If cuantosPuntos > 1 Then

                For cantidad = 1 To cuantosPuntos
                    Dim lugar As Integer = txtPorcentajeIndirectosDefault.Text.LastIndexOf(".")
                    Dim longitud As Integer = txtPorcentajeIndirectosDefault.Text.Length

                    If longitud > (lugar + 1) Then
                        txtPorcentajeIndirectosDefault.Text = txtPorcentajeIndirectosDefault.Text.Substring(0, lugar) & txtPorcentajeIndirectosDefault.Text.Substring(lugar + 1)
                        resultado = True
                        Exit For
                    Else
                        txtPorcentajeIndirectosDefault.Text = txtPorcentajeIndirectosDefault.Text.Substring(0, lugar)
                        resultado = True
                        Exit For
                    End If
                Next

            End If

        End If

        If resultado = True Then
            txtPorcentajeIndirectosDefault.Select(txtPorcentajeIndirectosDefault.Text.Length, 0)
        End If

        txtPorcentajeIndirectosDefault.Text = txtPorcentajeIndirectosDefault.Text.Replace("--", "").Replace("'", "")
        txtPorcentajeIndirectosDefault.Text = txtPorcentajeIndirectosDefault.Text.Trim

    End Sub


    Private Sub txtPorcentajePorCierreDeOperacionPresupuestada_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtPorcentajeUtilidadDefault.KeyUp

        Dim strcaracteresprohibidos As String = "abcdefghijklmnopqrstuvwxyzñABCDEFGHIJKLMNOPQRSTUVWXYZÑ|°!#$%&/()=?¡*¨[]_:;,-{}+´¿'¬^`~@\<>"
        Dim arrayCaractProhib As Char() = strcaracteresprohibidos.ToCharArray
        Dim resultado As Boolean = False

        For carp = 0 To arrayCaractProhib.Length - 1

            If txtPorcentajeUtilidadDefault.Text.Contains(arrayCaractProhib(carp)) Then
                txtPorcentajeUtilidadDefault.Text = txtPorcentajeUtilidadDefault.Text.Replace(arrayCaractProhib(carp), "")
                resultado = True
            End If

        Next carp

        If txtPorcentajeUtilidadDefault.Text.Contains(".") Then

            Dim comparaPuntos As Char() = txtPorcentajeUtilidadDefault.Text.ToCharArray
            Dim cuantosPuntos As Integer = 0


            For letra = 0 To comparaPuntos.Length - 1

                If comparaPuntos(letra) = "." Then
                    cuantosPuntos = cuantosPuntos + 1
                End If

            Next

            If cuantosPuntos > 1 Then

                For cantidad = 1 To cuantosPuntos
                    Dim lugar As Integer = txtPorcentajeUtilidadDefault.Text.LastIndexOf(".")
                    Dim longitud As Integer = txtPorcentajeUtilidadDefault.Text.Length

                    If longitud > (lugar + 1) Then
                        txtPorcentajeUtilidadDefault.Text = txtPorcentajeUtilidadDefault.Text.Substring(0, lugar) & txtPorcentajeUtilidadDefault.Text.Substring(lugar + 1)
                        resultado = True
                        Exit For
                    Else
                        txtPorcentajeUtilidadDefault.Text = txtPorcentajeUtilidadDefault.Text.Substring(0, lugar)
                        resultado = True
                        Exit For
                    End If
                Next

            End If

        End If

        If resultado = True Then
            txtPorcentajeUtilidadDefault.Select(txtPorcentajeUtilidadDefault.Text.Length, 0)
        End If

        txtPorcentajeUtilidadDefault.Text = txtPorcentajeUtilidadDefault.Text.Replace("--", "").Replace("'", "")
        txtPorcentajeUtilidadDefault.Text = txtPorcentajeUtilidadDefault.Text.Trim

    End Sub


    Private Sub txtPorcentajeDeComisionDeObraPresupuestada_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtPorcentajeIndirectosDefault.TextChanged

        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        If isFormReadyForAction = False Then
            Exit Sub
        End If

        Dim strcaracteresprohibidos As String = "abcdefghijklmnopqrstuvwxyzñABCDEFGHIJKLMNOPQRSTUVWXYZÑ|°!#$%&/()=?¡*¨[]_:;,-{}+´¿'¬^`~@\<> "
        Dim arrayCaractProhib As Char() = strcaracteresprohibidos.ToCharArray
        txtPorcentajeIndirectosDefault.Text = txtPorcentajeIndirectosDefault.Text.Trim(arrayCaractProhib)

        Dim valor As Double = 0.0
        Try
            valor = CDbl(txtPorcentajeIndirectosDefault.Text.Trim.Trim("--").Trim("'").Trim("@", ""))
            executeSQLCommand(0, "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & " SET dbaseindirectpercentagedefault = " & valor / 100 & ", iupdatedate = " & getMySQLDate() & ", supdatetime = '" & getAppTime() & "', supdateusername = '" & susername & "' WHERE ibaseid = " & iprojectid)
        Catch ex As Exception

        End Try

        Cursor.Current = System.Windows.Forms.Cursors.Default

    End Sub


    Private Sub txtPorcentajePorCierreDeOperacionPresupuestada_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtPorcentajeUtilidadDefault.TextChanged

        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        If isFormReadyForAction = False Then
            Exit Sub
        End If

        Dim strcaracteresprohibidos As String = "abcdefghijklmnopqrstuvwxyzñABCDEFGHIJKLMNOPQRSTUVWXYZÑ|°!#$%&/()=?¡*¨[]_:;,-{}+´¿'¬^`~@\<> "
        Dim arrayCaractProhib As Char() = strcaracteresprohibidos.ToCharArray
        txtPorcentajeUtilidadDefault.Text = txtPorcentajeUtilidadDefault.Text.Trim(arrayCaractProhib)

        Dim valor As Double = 0.0
        Try
            valor = CDbl(txtPorcentajeUtilidadDefault.Text.Trim.Trim("--").Trim("'").Trim("@", ""))
            executeSQLCommand(0, "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & " SET dbasegainpercentagedefault = " & valor / 100 & ", iupdatedate = " & getMySQLDate() & ", supdatetime = '" & getAppTime() & "', supdateusername = '" & susername & "' WHERE ibaseid = " & iprojectid)
        Catch ex As Exception

        End Try

        Cursor.Current = System.Windows.Forms.Cursors.Default

    End Sub


    Private Sub btnGenerarArchivoExcel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnGenerarArchivoExcel.Click

        Dim queryCostoTotal As String
        Dim costoTotal As Double = 0.0
        queryCostoTotal = "SELECT SUM(1*((IF(costoMAT.costo is null, 0, costoMAT.costo) + IF(costoMO.costo is null, 0, costoMO.costo) + IF(costoEQ.costo is null, 0, costoEQ.costo))*(1+btf.dcardindirectcostspercentage)*(1+btf.dcardgainpercentage))) AS dcardamount " & _
        "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Cards btf " & _
        "JOIN cardlegacycategories btflc ON btf.scardlegacycategoryid = btflc.scardlegacycategoryid " & _
        "JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & " p ON p.ibaseid = btf.ibaseid " & _
        "JOIN (SELECT btfi.ibaseid, btfi.icardid, btfi.iinputid, (costoMO.costo*btfi.dcardinputqty) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardInputs btfi JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Cards btf ON btf.ibaseid = btfi.ibaseid AND btf.icardid = btfi.icardid JOIN (SELECT btfi.ibaseid, btfi.icardid AS icardid, 0 AS iinputid, SUM(btfi.dcardinputqty*bp.dinputfinalprice) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Cards btf LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardInputs btfi ON btf.ibaseid = btfi.ibaseid AND btf.icardid = btfi.icardid LEFT JOIN inputs i ON btfi.iinputid = i.iinputid JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) bp GROUP BY iinputid, ibaseid) bp ON btfi.ibaseid = bp.ibaseid AND i.iinputid = bp.iinputid LEFT JOIN inputtypes it ON i.iinputid = it.iinputid WHERE it.sinputtypedescription = 'MANO DE OBRA' GROUP BY btf.icardid, btfi.ibaseid ) AS costoMO ON btfi.iinputid = costoMO.iinputid AND btfi.icardid = costoMO.icardid GROUP BY btfi.icardid, btfi.ibaseid) AS costoEQ ON btf.ibaseid = costoEQ.ibaseid AND btf.icardid = costoEQ.icardid " & _
        "JOIN (SELECT btfi.ibaseid, btfi.icardid, btfi.iinputid, SUM(btfi.dcardinputqty*bp.dinputfinalprice) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Cards btf LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardInputs btfi ON btf.ibaseid = btfi.ibaseid AND btf.icardid = btfi.icardid LEFT JOIN inputs i ON btfi.iinputid = i.iinputid JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) bp GROUP BY iinputid, ibaseid) bp ON btfi.ibaseid = bp.ibaseid AND i.iinputid = bp.iinputid JOIN inputtypes it ON i.iinputid = it.iinputid WHERE it.sinputtypedescription = 'MANO DE OBRA' GROUP BY btf.icardid, btfi.ibaseid) AS costoMO ON btf.ibaseid = costoMO.ibaseid AND btf.icardid = costoMO.icardid " & _
        "LEFT JOIN (SELECT btfi.ibaseid, btfi.icardid, IF(SUM(btfi.dcardinputqty*bp.dinputfinalprice) IS NULL, 0, SUM(btfi.dcardinputqty*bp.dinputfinalprice))+IF(SUM(btfi.dcardinputqty*cibp.dinputfinalprice) IS NULL, 0, SUM(btfi.dcardinputqty*cibp.dinputfinalprice)) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Cards btf LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardInputs btfi ON btf.ibaseid = btfi.ibaseid AND btf.icardid = btfi.icardid LEFT JOIN inputs i ON btfi.iinputid = i.iinputid LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) bp GROUP BY iinputid, ibaseid) bp ON btfi.ibaseid = bp.ibaseid AND i.iinputid = bp.iinputid LEFT JOIN (SELECT btfi.ibaseid, btfi.icardid, btfi.iinputid, cibp.iupdatedate, cibp.supdatetime, SUM(btfci.dcompoundinputqty*cibp.dinputfinalprice) AS dinputpricewithoutIVA, 0.00000 AS dinputprotectionpercentage, SUM(btfci.dcompoundinputqty*cibp.dinputfinalprice) AS dinputfinalprice FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardInputs btfi JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardCompoundInputs btfci ON btfci.ibaseid = btfi.ibaseid AND btfci.icardid = btfi.icardid AND btfci.iinputid = btfi.iinputid LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) cibp GROUP BY iinputid, ibaseid) cibp ON cibp.ibaseid = btfci.ibaseid AND cibp.iinputid = btfci.icompoundinputid GROUP BY btfci.ibaseid, btfci.icardid, btfi.iinputid) cibp ON btfi.ibaseid = cibp.ibaseid AND btfi.icardid = cibp.icardid AND i.iinputid = cibp.iinputid JOIN inputtypes it ON i.iinputid = it.iinputid WHERE it.sinputtypedescription = 'MATERIALES' GROUP BY btfi.ibaseid, btfi.icardid ORDER BY btfi.ibaseid, btfi.icardid, btfi.iinputid) AS costoMAT ON btf.ibaseid = costoMAT.ibaseid AND btf.icardid = costoMAT.icardid " & _
        "WHERE p.ibaseid = " & iprojectid

        costoTotal = getSQLQueryAsDouble(0, queryCostoTotal)

        If costoTotal = 0.0 Then
            MsgBox("Aún NO has terminado de definir este Presupuesto Base. ¿Podrías completarlo?", MsgBoxStyle.OkOnly, "Presupuesto Base incompleto")
            Exit Sub
        End If

        Try

            Dim resultado As Boolean = False

            Dim fecha As String = ""
            Dim dayAux As String = ""
            Dim monthAux As String = ""
            Dim hourAux As String = ""
            Dim minuteAux As String = ""
            Dim secondAux As String = ""

            If DateTime.Today.Month.ToString.Length < 2 Then
                monthAux = "0" & DateTime.Today.Month
            Else
                monthAux = DateTime.Today.Month
            End If

            If DateTime.Today.Day.ToString.Length < 2 Then
                dayAux = "0" & DateTime.Today.Day
            Else
                dayAux = DateTime.Today.Day
            End If

            If Date.Now.Hour.ToString.Length < 2 Then
                hourAux = "0" & DateTime.Now.Hour
            Else
                hourAux = DateTime.Now.Hour
            End If

            If Date.Now.Minute.ToString.Length < 2 Then
                minuteAux = "0" & DateTime.Now.Minute
            Else
                minuteAux = DateTime.Now.Minute
            End If

            If Date.Now.Second.ToString.Length < 2 Then
                secondAux = "0" & DateTime.Now.Second
            Else
                secondAux = DateTime.Now.Second
            End If

            fecha = DateTime.Today.Year & monthAux & dayAux & hourAux & minuteAux & secondAux



            msSaveFileDialog.FileName = "Presupuesto Base " & fecha

            Try

                'If Not My.Computer.FileSystem.DirectoryExists(txtRuta.Text) Then
                '    My.Computer.FileSystem.CreateDirectory(txtRuta.Text)
                'End If

                'msSaveFileDialog.InitialDirectory = txtRuta.Text

            Catch ex As Exception

            End Try

            msSaveFileDialog.Filter = "Excel Files (*.xls) |*.xls"
            msSaveFileDialog.DefaultExt = "*.xls"

            If msSaveFileDialog.ShowDialog() = DialogResult.OK Then

                Cursor.Current = System.Windows.Forms.Cursors.WaitCursor
                resultado = ExportPresupuestoToExcel(msSaveFileDialog.FileName)
                Cursor.Current = System.Windows.Forms.Cursors.Default

                If resultado = True Then
                    MsgBox("Presupuesto Exportado Correctamente!" & Chr(13) & "El archivo se abrirá al dar click en OK", MsgBoxStyle.OkOnly, "Exportación Completada")
                    System.Diagnostics.Process.Start(msSaveFileDialog.FileName)
                Else
                    MsgBox("No se ha podido exportar el presupuesto. Intente nuevamente.", MsgBoxStyle.OkOnly, "Error al exportar el presupuesto")
                End If

            End If

        Catch ex As Exception

        End Try

    End Sub


    Private Function ExportPresupuestoToExcel(ByVal pth As String) As Boolean

        Try

            Dim fs As New IO.StreamWriter(pth, False)
            fs.WriteLine("<?xml version=""1.0""?>")
            fs.WriteLine("<?mso-application progid=""Excel.Sheet""?>")
            fs.WriteLine("<Workbook xmlns=""urn:schemas-microsoft-com:office:spreadsheet"" xmlns:o=""urn:schemas-microsoft-com:office:office"" xmlns:x=""urn:schemas-microsoft-com:office:excel"" xmlns:ss=""urn:schemas-microsoft-com:office:spreadsheet"" xmlns:html=""http://www.w3.org/TR/REC-html40"">")

            ' Create the styles for the worksheet
            fs.WriteLine("  <Styles>")

            ' Style for the document name
            fs.WriteLine("  <Style ss:ID=""1"">")
            fs.WriteLine("   <Alignment ss:Horizontal=""Left"" ss:Vertical=""Center""></Alignment>")
            fs.WriteLine("   <Borders>")
            fs.WriteLine("  <Border ss:Position=""Bottom"" ss:LineStyle=""Continuous"" ss:Weight=""1""></Border>")
            fs.WriteLine("  <Border ss:Position=""Left"" ss:LineStyle=""Continuous"" ss:Weight=""1""></Border>")
            fs.WriteLine("  <Border ss:Position=""Right"" ss:LineStyle=""Continuous"" ss:Weight=""1""></Border>")
            fs.WriteLine("  <Border ss:Position=""Top"" ss:LineStyle=""Continuous"" ss:Weight=""1""></Border>")
            fs.WriteLine("   </Borders>")
            fs.WriteLine("   <Font ss:FontName=""Arial"" ss:Size=""12"" ss:Bold=""1""></Font>")
            fs.WriteLine("   <Interior ss:Color=""#FF9900"" ss:Pattern=""Solid""></Interior>")
            fs.WriteLine("   <NumberFormat></NumberFormat>")
            fs.WriteLine("   <Protection></Protection>")
            fs.WriteLine("  </Style>")

            ' Style for the column headers
            fs.WriteLine("   <Style ss:ID=""2"">")
            fs.WriteLine("   <Alignment ss:Horizontal=""Center"" ss:Vertical=""Center"" ss:WrapText=""1""></Alignment>")
            fs.WriteLine("   <Borders>")
            fs.WriteLine("  <Border ss:Position=""Bottom"" ss:LineStyle=""Continuous"" ss:Weight=""1""></Border>")
            fs.WriteLine("  <Border ss:Position=""Left"" ss:LineStyle=""Continuous"" ss:Weight=""1""></Border>")
            fs.WriteLine("  <Border ss:Position=""Right"" ss:LineStyle=""Continuous"" ss:Weight=""1""></Border>")
            fs.WriteLine("  <Border ss:Position=""Top"" ss:LineStyle=""Continuous"" ss:Weight=""1""></Border>")
            fs.WriteLine("   </Borders>")
            fs.WriteLine("   <Font ss:FontName=""Arial"" ss:Size=""9"" ss:Bold=""1""></Font>")
            fs.WriteLine("  </Style>")


            ' Style for the left sided info
            fs.WriteLine("    <Style ss:ID=""9"">")
            fs.WriteLine("      <Font ss:FontName=""Arial"" ss:Size=""10""></Font>")
            fs.WriteLine("      <Alignment ss:Horizontal=""Left"" ss:Vertical=""Center""></Alignment>")
            fs.WriteLine("    </Style>")

            ' Style for the right sided info
            fs.WriteLine("    <Style ss:ID=""10"">")
            fs.WriteLine("      <Font ss:FontName=""Arial"" ss:Size=""10""></Font>")
            fs.WriteLine("      <Alignment ss:Horizontal=""Right"" ss:Vertical=""Center""></Alignment>")
            fs.WriteLine("    </Style>")

            ' Style for the middle sided info
            fs.WriteLine("    <Style ss:ID=""11"">")
            fs.WriteLine("      <Font ss:FontName=""Arial"" ss:Size=""10""></Font>")
            fs.WriteLine("      <Alignment ss:Horizontal=""Center"" ss:Vertical=""Center""></Alignment>")
            fs.WriteLine("    </Style>")

            ' Style for the SUBtotals labels
            fs.WriteLine("    <Style ss:ID=""12"">")
            fs.WriteLine("      <Font ss:FontName=""Arial"" ss:Size=""9""></Font>")
            fs.WriteLine("   <Alignment ss:Horizontal=""Left"" ss:Vertical=""Center""></Alignment>")
            fs.WriteLine("   <Interior ss:Color=""#FFCC00"" ss:Pattern=""Solid""></Interior>")
            fs.WriteLine("   <NumberFormat></NumberFormat>")
            fs.WriteLine("    </Style>")

            ' Style for the totals labels
            fs.WriteLine("    <Style ss:ID=""13"">")
            fs.WriteLine("      <Font ss:FontName=""Arial"" ss:Size=""9""></Font>")
            fs.WriteLine("      <Alignment ss:Horizontal=""Right"" ss:Vertical=""Center""></Alignment>")
            fs.WriteLine("   <Interior ss:Color=""#FF9900"" ss:Pattern=""Solid""></Interior>")
            fs.WriteLine("   <NumberFormat></NumberFormat>")
            fs.WriteLine("    </Style>")

            ' Style for the totals
            fs.WriteLine("    <Style ss:ID=""14"">")
            fs.WriteLine("      <Font ss:FontName=""Arial"" ss:Size=""9""></Font>")
            fs.WriteLine("   <Alignment ss:Horizontal=""Left"" ss:Vertical=""Center""></Alignment>")
            fs.WriteLine("   <Interior ss:Color=""#FF9900"" ss:Pattern=""Solid""></Interior>")
            fs.WriteLine("   <NumberFormat ss:Format=""Standard""></NumberFormat>")
            fs.WriteLine("    </Style>")

            fs.WriteLine("  </Styles>")

            ' Write the worksheet contents
            fs.WriteLine("<Worksheet ss:Name=""Hoja1"">")
            fs.WriteLine("  <Table ss:DefaultColumnWidth=""60"" ss:DefaultRowHeight=""15"">")

            'Write the project header info
            fs.WriteLine("   <Column ss:AutoFitWidth=""0"" ss:Width=""63""/>")
            fs.WriteLine("   <Column ss:AutoFitWidth=""0"" ss:Width=""494.25""/>")
            fs.WriteLine("   <Column ss:AutoFitWidth=""0"" ss:Width=""65.25"" ss:Span=""5""/>")

            fs.WriteLine("   <Row ss:AutoFitHeight=""0"">")
            fs.WriteLine("  <Cell></Cell>")
            fs.WriteLine("  <Cell ss:MergeAcross=""6"" ss:StyleID=""1""><Data ss:Type=""String"">PRESUPUESTO BASE DE CONSTRUCCION</Data></Cell>")
            fs.WriteLine("   </Row>")

            fs.WriteLine(String.Format("    <Row ss:AutoFitHeight=""0"">"))
            fs.WriteLine("  <Cell ss:StyleID=""9""></Cell>")
            fs.WriteLine(String.Format("      <Cell ss:StyleID=""9""><Data ss:Type=""String"">{0}</Data></Cell>", "Fecha:"))
            fs.WriteLine(String.Format("      <Cell ss:StyleID=""9""><Data ss:Type=""String"">{0}</Data></Cell>", convertYYYYMMDDtoDDhyphenMMhyphenYYYY(getMySQLDate()) & " " & getAppTime()))
            fs.WriteLine("  <Cell ss:StyleID=""9""></Cell>")
            fs.WriteLine("  <Cell ss:StyleID=""9""></Cell>")
            fs.WriteLine("  <Cell ss:StyleID=""9""></Cell>")
            fs.WriteLine("  <Cell ss:StyleID=""9""></Cell>")
            fs.WriteLine("  <Cell ss:StyleID=""9""></Cell>")
            fs.WriteLine("    </Row>")


            'Write the grid headers columns (taken out since columns are already defined)
            'For Each col As DataGridViewColumn In dgv.Columns
            '    If col.Visible Then
            '        fs.WriteLine(String.Format("    <Column ss:Width=""{0}""></Column>", col.Width))
            '    End If
            'Next

            'Write the grid headers
            fs.WriteLine("    <Row ss:AutoFitHeight=""0"" ss:Height=""22.5"">")

            For Each col As DataGridViewColumn In dgvResumenDeTarjetas.Columns
                If col.Visible Then
                    fs.WriteLine(String.Format("      <Cell ss:StyleID=""2""><Data ss:Type=""String"">{0}</Data></Cell>", col.HeaderText))
                End If
            Next

            fs.WriteLine("    </Row>")

            ' Write contents for each cell
            For Each row As DataGridViewRow In dgvResumenDeTarjetas.Rows

                If dgvResumenDeTarjetas.AllowUserToAddRows = True And row.Index = dgvResumenDeTarjetas.Rows.Count - 1 Then
                    Exit For
                End If

                fs.WriteLine(String.Format("    <Row ss:AutoFitHeight=""0"">"))

                For Each col As DataGridViewColumn In dgvResumenDeTarjetas.Columns

                    If col.Visible Then

                        If row.Cells(col.Name).Value.ToString = "" Then

                            If row.Cells(0).Value.ToString.StartsWith("SUBTOTAL") = True Then
                                fs.WriteLine(String.Format("      <Cell ss:StyleID=""12""></Cell>"))
                            Else
                                fs.WriteLine(String.Format("      <Cell ss:StyleID=""9""></Cell>"))
                            End If

                        Else

                            If row.Cells(0).Value.ToString.StartsWith("SUBTOTAL") = True Then
                                fs.WriteLine(String.Format("      <Cell ss:StyleID=""12""><Data ss:Type=""String"">{0}</Data></Cell>", row.Cells(col.Name).Value.ToString))
                            Else
                                fs.WriteLine(String.Format("      <Cell ss:StyleID=""9""><Data ss:Type=""String"">{0}</Data></Cell>", row.Cells(col.Name).Value.ToString))
                            End If

                        End If

                    End If

                Next col

                fs.WriteLine("    </Row>")

            Next row


            ' Close up the document
            fs.WriteLine("  </Table>")

            fs.WriteLine("  <WorksheetOptions xmlns=""urn:schemas-microsoft-com:office:excel"">")
            fs.WriteLine("   <PageSetup>")
            fs.WriteLine("  <Layout x:Orientation=""Landscape""/>")
            fs.WriteLine("  <Header x:Margin=""0.51181102362204722""/>")
            fs.WriteLine("  <Footer x:Margin=""0.51181102362204722""/>")
            fs.WriteLine("  <PageMargins x:Bottom=""0.98425196850393704"" x:Left=""0.74803149606299213"" x:Right=""0.74803149606299213"" x:Top=""0.98425196850393704""/>")
            fs.WriteLine("   </PageSetup>")
            fs.WriteLine("   <Unsynced/>")
            fs.WriteLine("   <Print>")
            fs.WriteLine("  <ValidPrinterInfo/>")
            fs.WriteLine("  <PaperSizeIndex>9</PaperSizeIndex>")
            fs.WriteLine("  <Scale>65</Scale>")
            fs.WriteLine("  <HorizontalResolution>200</HorizontalResolution>")
            fs.WriteLine("  <VerticalResolution>200</VerticalResolution>")
            fs.WriteLine("   </Print>")
            fs.WriteLine("   <Zoom>75</Zoom>")
            fs.WriteLine("   <Selected/>")
            fs.WriteLine("   <Panes>")
            fs.WriteLine("  <Pane>")
            fs.WriteLine("   <Number>3</Number>")
            fs.WriteLine("   <ActiveRow>16</ActiveRow>")
            fs.WriteLine("   <ActiveCol>7</ActiveCol>")
            fs.WriteLine("  </Pane>")
            fs.WriteLine("   </Panes>")
            fs.WriteLine("   <ProtectObjects>False</ProtectObjects>")
            fs.WriteLine("   <ProtectScenarios>False</ProtectScenarios>")
            fs.WriteLine("  </WorksheetOptions>")


            fs.WriteLine("</Worksheet>")
            fs.WriteLine("</Workbook>")
            fs.Close()

            Return True

        Catch ex As Exception
            Return False
        End Try

    End Function


    Private Sub btnGuardar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnGuardar.Click

        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        If validaCostosIndirectos(False) = False Or validaResumenDeTarjetas(False) = False Or isHistoric = True Then
            Cursor.Current = System.Windows.Forms.Cursors.Default
            Exit Sub
        End If

        Dim timesBaseIsOpen As Integer = 1

        timesBaseIsOpen = getSQLQueryAsInteger(0, "SELECT COUNT(*) FROM information_schema.TABLES WHERE TABLE_NAME LIKE '%Base" & iprojectid & "'")

        If timesBaseIsOpen > 1 And isEdit = True Then

            Cursor.Current = System.Windows.Forms.Cursors.Default

            If MsgBox("Otro usuario tiene abierto el mismo Presupuesto Base. Esto podría causar que alguno de ustedes perdiera los cambios que hiciera. ¿Deseas seguir guardando el Presupuesto Base?", MsgBoxStyle.YesNo, "Confirmación Guardado") = MsgBoxResult.No Then

                Exit Sub

            Else

                Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

            End If

        ElseIf timesBaseIsOpen > 1 And isEdit = False Then

            Dim newIdAddition As Integer = 1

            Do While getSQLQueryAsInteger(0, "SELECT COUNT(*) FROM information_schema.TABLES WHERE TABLE_NAME LIKE '%Base" & iprojectid + newIdAddition & "'") > 1 And isEdit = False
                newIdAddition = newIdAddition + 1
            Loop

            'I got the new id (previousId + newIdAddition)

            Dim queriesNewId(31) As String

            queriesNewId(8) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & " RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid + newIdAddition
            queriesNewId(9) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "IndirectCosts RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid + newIdAddition & "IndirectCosts"
            queriesNewId(10) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Cards RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid + newIdAddition & "Cards"
            queriesNewId(11) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardInputs RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid + newIdAddition & "CardInputs"
            queriesNewId(12) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardCompoundInputs RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid + newIdAddition & "CardCompoundInputs"
            queriesNewId(13) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Prices RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid + newIdAddition & "Prices"
            queriesNewId(14) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Timber RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid + newIdAddition & "Timber"
            queriesNewId(15) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "CardsAux RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid + newIdAddition & "CardsAux"
            queriesNewId(24) = "UPDATE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid + newIdAddition & " SET ibaseid = " & iprojectid + newIdAddition & " WHERE ibaseid = " & iprojectid
            queriesNewId(25) = "UPDATE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid + newIdAddition & "IndirectCosts SET ibaseid = " & iprojectid + newIdAddition & " WHERE ibaseid = " & iprojectid
            queriesNewId(26) = "UPDATE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid + newIdAddition & "Cards SET ibaseid = " & iprojectid + newIdAddition & " WHERE ibaseid = " & iprojectid
            queriesNewId(27) = "UPDATE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid + newIdAddition & "CardInputs SET ibaseid = " & iprojectid + newIdAddition & " WHERE ibaseid = " & iprojectid
            queriesNewId(28) = "UPDATE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid + newIdAddition & "CardCompoundInputs SET ibaseid = " & iprojectid + newIdAddition & " WHERE ibaseid = " & iprojectid
            queriesNewId(29) = "UPDATE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid + newIdAddition & "Prices SET ibaseid = " & iprojectid + newIdAddition & " WHERE ibaseid = " & iprojectid
            queriesNewId(30) = "UPDATE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid + newIdAddition & "Timber SET ibaseid = " & iprojectid + newIdAddition & " WHERE ibaseid = " & iprojectid

            If executeTransactedSQLCommand(0, queriesNewId) = True Then
                iprojectid = iprojectid + newIdAddition
            End If

        End If

        Dim queriesSave(29) As String

        queriesSave(0) = "" & _
        "DELETE " & _
        "FROM base " & _
        "WHERE ibaseid = " & iprojectid & " AND " & _
        "NOT EXISTS (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & " tb WHERE base.ibaseid = tb.ibaseid) "

        queriesSave(1) = "" & _
        "DELETE " & _
        "FROM baseindirectcosts " & _
        "WHERE ibaseid = " & iprojectid & " AND " & _
        "NOT EXISTS (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "IndirectCosts tbic WHERE baseindirectcosts.ibaseid = tbic.ibaseid AND baseindirectcosts.icostid = tbic.icostid) "

        queriesSave(2) = "" & _
        "DELETE " & _
        "FROM basecards " & _
        "WHERE ibaseid = " & iprojectid & " AND " & _
        "NOT EXISTS (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Cards tbc WHERE basecards.ibaseid = tbc.ibaseid AND basecards.icardid = tbc.icardid) "

        queriesSave(3) = "" & _
        "DELETE " & _
        "FROM basecards " & _
        "WHERE ibaseid = " & iprojectid & " AND " & _
        "NOT EXISTS (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Cards tbc WHERE basecards.ibaseid = tbc.ibaseid AND basecards.icardid = tbc.icardid) "

        'queriesSave(4) = "" & _
        '"DELETE " & _
        '"FROM projectcards " & _
        '"WHERE NOT EXISTS (SELECT * FROM basecards bc WHERE bc.ibaseid = " & iprojectid & " AND projectcards.icardid = bc.icardid) "

        'queriesSave(5) = "" & _
        '"DELETE " & _
        '"FROM modelcards " & _
        '"WHERE NOT EXISTS (SELECT * FROM basecards bc WHERE bc.ibaseid = " & iprojectid & " AND modelcards.icardid = bc.icardid) "

        queriesSave(6) = "" & _
        "DELETE " & _
        "FROM basecardinputs " & _
        "WHERE ibaseid = " & iprojectid & " AND " & _
        "NOT EXISTS (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardInputs tbci WHERE basecardinputs.ibaseid = tbci.ibaseid AND basecardinputs.icardid = tbci.icardid AND basecardinputs.iinputid = tbci.iinputid) "

        'queriesSave(7) = "" & _
        '"DELETE " & _
        '"FROM projectcardinputs " & _
        '"WHERE NOT EXISTS (SELECT * FROM basecardinputs bci WHERE bci.ibaseid = " & iprojectid & " AND projectcardinputs.icardid = bci.icardid AND projectcardinputs.iinputid = bci.iinputid) "

        'queriesSave(8) = "" & _
        '"DELETE " & _
        '"FROM modelcardinputs " & _
        '"WHERE NOT EXISTS (SELECT * FROM basecardinputs bci WHERE bci.ibaseid = " & iprojectid & " AND modelcardinputs.icardid = bci.icardid AND modelcardinputs.iinputid = bci.iinputid) "

        queriesSave(9) = "" & _
        "DELETE " & _
        "FROM basecardcompoundinputs " & _
        "WHERE ibaseid = " & iprojectid & " AND " & _
        "NOT EXISTS (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardCompoundInputs tbcci WHERE basecardcompoundinputs.ibaseid = tbcci.ibaseid AND basecardcompoundinputs.icardid = tbcci.icardid AND basecardcompoundinputs.iinputid = tbcci.iinputid AND basecardcompoundinputs.icompoundinputid = tbcci.icompoundinputid) "

        'queriesSave(10) = "" & _
        '"DELETE " & _
        '"FROM projectcardcompoundinputs " & _
        '"WHERE NOT EXISTS (SELECT * FROM basecardcompoundinputs bcci WHERE bcci.ibaseid = " & iprojectid & " AND projectcardcompoundinputs.icardid = bcci.icardid AND projectcardcompoundinputs.iinputid = bcci.iinputid AND projectcardcompoundinputs.icompoundinputid = bcci.icompoundinputid) "

        'queriesSave(11) = "" & _
        '"DELETE " & _
        '"FROM modelcardcompoundinputs " & _
        '"WHERE NOT EXISTS (SELECT * FROM basecardcompoundinputs bcci WHERE bcci.ibaseid = " & iprojectid & " AND modelcardcompoundinputs.icardid = bcci.icardid AND modelcardcompoundinputs.iinputid = bcci.iinputid AND modelcardcompoundinputs.icompoundinputid = bcci.icompoundinputid) "

        queriesSave(12) = "" & _
        "DELETE " & _
        "FROM baseprices " & _
        "WHERE ibaseid = " & iprojectid & " AND " & _
        "NOT EXISTS (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Prices tbp WHERE baseprices.ibaseid = tbp.ibaseid AND baseprices.iinputid = tbp.iinputid AND baseprices.iupdatedate = tbp.iupdatedate AND baseprices.supdatetime = tbp.supdatetime) "

        queriesSave(13) = "" & _
        "DELETE " & _
        "FROM basetimber " & _
        "WHERE ibaseid = " & iprojectid & " AND " & _
        "NOT EXISTS (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Timber tbt WHERE basetimber.ibaseid = tbt.ibaseid AND basetimber.iinputid = tbt.iinputid) "

        queriesSave(14) = "" & _
        "UPDATE base p JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & " tp ON tp.ibaseid = p.ibaseid SET p.iupdatedate = tp.iupdatedate, p.supdatetime = tp.supdatetime, p.supdateusername = tp.supdateusername, p.sbasefileslocation = tp.sbasefileslocation, p.dbaseindirectpercentagedefault = tp.dbaseindirectpercentagedefault, p.dbasegainpercentagedefault = tp.dbasegainpercentagedefault, p.dbaseIVApercentage = tp.dbaseIVApercentage WHERE STR_TO_DATE(CONCAT(tp.iupdatedate, ' ', tp.supdatetime), '%Y%c%d %T') > STR_TO_DATE(CONCAT(p.iupdatedate, ' ', p.supdatetime), '%Y%c%d %T') "

        queriesSave(15) = "" & _
        "UPDATE baseindirectcosts pic JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "IndirectCosts tpic ON tpic.ibaseid = pic.ibaseid AND tpic.icostid = pic.icostid SET pic.iupdatedate = tpic.iupdatedate, pic.supdatetime = tpic.supdatetime, pic.supdateusername = tpic.supdateusername, pic.sbasecostname = tpic.sbasecostname, pic.dbasecost = tpic.dbasecost, pic.dcompanyprojectedearnings = tpic.dcompanyprojectedearnings WHERE STR_TO_DATE(CONCAT(tpic.iupdatedate, ' ', tpic.supdatetime), '%Y%c%d %T') > STR_TO_DATE(CONCAT(pic.iupdatedate, ' ', pic.supdatetime), '%Y%c%d %T') "

        queriesSave(16) = "" & _
        "UPDATE basecards pc JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Cards tpc ON tpc.ibaseid = pc.ibaseid AND tpc.icardid = pc.icardid SET pc.iupdatedate = tpc.iupdatedate, pc.supdatetime = tpc.supdatetime, pc.supdateusername = tpc.supdateusername, pc.scardlegacycategoryid = tpc.scardlegacycategoryid, pc.scardlegacyid = tpc.scardlegacyid, pc.scarddescription = tpc.scarddescription, pc.scardunit = tpc.scardunit, pc.dcardqty = tpc.dcardqty, pc.dcardindirectcostspercentage = tpc.dcardindirectcostspercentage, pc.dcardgainpercentage = tpc.dcardgainpercentage WHERE STR_TO_DATE(CONCAT(tpc.iupdatedate, ' ', tpc.supdatetime), '%Y%c%d %T') > STR_TO_DATE(CONCAT(pc.iupdatedate, ' ', pc.supdatetime), '%Y%c%d %T') "

        queriesSave(17) = "" & _
        "UPDATE basecardinputs pci JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardInputs tpci ON tpci.ibaseid = pci.ibaseid AND tpci.icardid = pci.icardid AND tpci.iinputid = pci.iinputid SET pci.iupdatedate = tpci.iupdatedate, pci.supdatetime = tpci.supdatetime, pci.supdateusername = tpci.supdateusername, pci.scardinputunit = tpci.scardinputunit, pci.dcardinputqty = tpci.dcardinputqty WHERE STR_TO_DATE(CONCAT(tpci.iupdatedate, ' ', tpci.supdatetime), '%Y%c%d %T') > STR_TO_DATE(CONCAT(pci.iupdatedate, ' ', pci.supdatetime), '%Y%c%d %T') "

        queriesSave(18) = "" & _
        "UPDATE basecardcompoundinputs pcci JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardCompoundInputs tpcci ON tpcci.ibaseid = pcci.ibaseid AND tpcci.icardid = pcci.icardid AND tpcci.iinputid = pcci.iinputid AND tpcci.icompoundinputid = pcci.icompoundinputid SET pcci.iupdatedate = tpcci.iupdatedate, pcci.supdatetime = tpcci.supdatetime, pcci.supdateusername = tpcci.supdateusername, pcci.scompoundinputunit = tpcci.scompoundinputunit, pcci.dcompoundinputqty = tpcci.dcompoundinputqty WHERE STR_TO_DATE(CONCAT(tpcci.iupdatedate, ' ', tpcci.supdatetime), '%Y%c%d %T') > STR_TO_DATE(CONCAT(pcci.iupdatedate, ' ', pcci.supdatetime), '%Y%c%d %T') "

        queriesSave(19) = "" & _
        "UPDATE baseprices pp JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Prices tpp ON tpp.ibaseid = pp.ibaseid AND tpp.iinputid = pp.iinputid AND tpp.iupdatedate = pp.iupdatedate AND tpp.supdatetime = pp.supdatetime SET pp.iupdatedate = tpp.iupdatedate, pp.supdatetime = tpp.supdatetime, pp.supdateusername = tpp.supdateusername, pp.dinputpricewithoutIVA = tpp.dinputpricewithoutIVA, pp.dinputprotectionpercentage = tpp.dinputprotectionpercentage, pp.dinputfinalprice = tpp.dinputfinalprice WHERE STR_TO_DATE(CONCAT(pp.iupdatedate, ' ', pp.supdatetime), '%Y%c%d %T') > STR_TO_DATE(CONCAT(pp.iupdatedate, ' ', pp.supdatetime), '%Y%c%d %T') "

        queriesSave(20) = "" & _
        "UPDATE basetimber pt JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Timber tpt ON tpt.ibaseid = pt.ibaseid AND tpt.iinputid = pt.iinputid SET pt.iupdatedate = tpt.iupdatedate, pt.supdatetime = tpt.supdatetime, pt.supdateusername = tpt.supdateusername, pt.dinputtimberespesor = tpt.dinputtimberespesor, pt.dinputtimberancho = tpt.dinputtimberancho, pt.dinputtimberlargo = tpt.dinputtimberlargo, pt.dinputtimberpreciopiecubico = tpt.dinputtimberpreciopiecubico WHERE STR_TO_DATE(CONCAT(tpt.iupdatedate, ' ', tpt.supdatetime), '%Y%c%d %T') > STR_TO_DATE(CONCAT(pt.iupdatedate, ' ', pt.supdatetime), '%Y%c%d %T') "

        queriesSave(21) = "" & _
        "INSERT INTO base " & _
        "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & " tb " & _
        "WHERE NOT EXISTS (SELECT * FROM base b WHERE b.ibaseid = tb.ibaseid AND b.ibaseid = " & iprojectid & ") "

        queriesSave(22) = "" & _
        "INSERT INTO baseindirectcosts " & _
        "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "IndirectCosts tbic " & _
        "WHERE NOT EXISTS (SELECT * FROM baseindirectcosts bic WHERE tbic.ibaseid = bic.ibaseid AND tbic.icostid = bic.icostid AND bic.ibaseid = " & iprojectid & ") "

        queriesSave(23) = "" & _
        "INSERT INTO basecards " & _
        "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Cards tbc " & _
        "WHERE NOT EXISTS (SELECT * FROM basecards bc WHERE tbc.ibaseid = bc.ibaseid AND tbc.icardid = bc.icardid AND bc.ibaseid = " & iprojectid & ") "

        queriesSave(24) = "" & _
        "INSERT INTO basecardinputs " & _
        "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardInputs tbci " & _
        "WHERE NOT EXISTS (SELECT * FROM basecardinputs bci WHERE tbci.ibaseid = bci.ibaseid AND tbci.icardid = bci.icardid AND tbci.iinputid = bci.iinputid AND bci.ibaseid = " & iprojectid & ") "

        queriesSave(25) = "" & _
        "INSERT INTO basecardcompoundinputs " & _
        "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardCompoundInputs tbcci " & _
        "WHERE NOT EXISTS (SELECT * FROM basecardcompoundinputs bcci WHERE tbcci.ibaseid = bcci.ibaseid AND tbcci.icardid = bcci.icardid AND tbcci.iinputid = bcci.iinputid AND tbcci.icompoundinputid = bcci.icompoundinputid AND bcci.ibaseid = " & iprojectid & ") "

        queriesSave(26) = "" & _
        "INSERT INTO baseprices " & _
        "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Prices tbp " & _
        "WHERE NOT EXISTS (SELECT * FROM baseprices bp WHERE tbp.ibaseid = bp.ibaseid AND tbp.iinputid = bp.iinputid AND tbp.iupdatedate = bp.iupdatedate AND tbp.supdatetime = bp.supdatetime AND bp.ibaseid = " & iprojectid & ") "

        queriesSave(27) = "" & _
        "INSERT INTO basetimber " & _
        "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Timber tbt " & _
        "WHERE NOT EXISTS (SELECT * FROM basetimber bt WHERE tbt.ibaseid = bt.ibaseid AND tbt.iinputid = bt.iinputid AND bt.ibaseid = " & iprojectid & ") "

        queriesSave(28) = "INSERT IGNORE INTO logs VALUES (" & getMySQLDate() & ", '" & getAppTime() & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Guardó cambios al Presupuesto Base " & iprojectid & ": " & dtFechaPresupuesto.Value & "', 'OK')"

        If executeTransactedSQLCommand(0, queriesSave) = True Then
            MsgBox("Guardado exitosamente", MsgBoxStyle.OkOnly, "")
        Else
            MsgBox("Hubo un error al Guardar. Probablemente un error de Red. Intenta nuevamente", MsgBoxStyle.OkOnly, "")
            Exit Sub
        End If

        wasCreated = True

        Cursor.Current = System.Windows.Forms.Cursors.Default

    End Sub


    Private Sub btnGuardarYCerrar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnGuardarYCerrar.Click

        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        If validaCostosIndirectos(False) = False Or validaResumenDeTarjetas(False) = False Or isHistoric = True Then
            Cursor.Current = System.Windows.Forms.Cursors.Default
            Exit Sub
        End If

        Dim timesBaseIsOpen As Integer = 1

        timesBaseIsOpen = getSQLQueryAsInteger(0, "SELECT COUNT(*) FROM information_schema.TABLES WHERE TABLE_NAME LIKE '%Base" & iprojectid & "'")

        If timesBaseIsOpen > 1 And isEdit = True Then

            Cursor.Current = System.Windows.Forms.Cursors.Default

            If MsgBox("Otro usuario tiene abierto el mismo Presupuesto Base. Esto podría causar que alguno de ustedes perdiera los cambios que hiciera. ¿Deseas seguir guardando el Presupuesto Base?", MsgBoxStyle.YesNo, "Confirmación Guardado") = MsgBoxResult.No Then

                Exit Sub

            Else

                Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

            End If

        ElseIf timesBaseIsOpen > 1 And isEdit = False Then

            Dim newIdAddition As Integer = 1

            Do While getSQLQueryAsInteger(0, "SELECT COUNT(*) FROM information_schema.TABLES WHERE TABLE_NAME LIKE '%Base" & iprojectid + newIdAddition & "'") > 1 And isEdit = False
                newIdAddition = newIdAddition + 1
            Loop

            'I got the new id (previousId + newIdAddition)

            Dim queriesNewId(31) As String

            queriesNewId(8) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & " RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid + newIdAddition
            queriesNewId(9) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "IndirectCosts RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid + newIdAddition & "IndirectCosts"
            queriesNewId(10) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Cards RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid + newIdAddition & "Cards"
            queriesNewId(11) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardInputs RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid + newIdAddition & "CardInputs"
            queriesNewId(12) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardCompoundInputs RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid + newIdAddition & "CardCompoundInputs"
            queriesNewId(13) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Prices RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid + newIdAddition & "Prices"
            queriesNewId(14) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Timber RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid + newIdAddition & "Timber"
            queriesNewId(15) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "CardsAux RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid + newIdAddition & "CardsAux"
            queriesNewId(24) = "UPDATE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid + newIdAddition & " SET ibaseid = " & iprojectid + newIdAddition & " WHERE ibaseid = " & iprojectid
            queriesNewId(25) = "UPDATE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid + newIdAddition & "IndirectCosts SET ibaseid = " & iprojectid + newIdAddition & " WHERE ibaseid = " & iprojectid
            queriesNewId(26) = "UPDATE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid + newIdAddition & "Cards SET ibaseid = " & iprojectid + newIdAddition & " WHERE ibaseid = " & iprojectid
            queriesNewId(27) = "UPDATE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid + newIdAddition & "CardInputs SET ibaseid = " & iprojectid + newIdAddition & " WHERE ibaseid = " & iprojectid
            queriesNewId(28) = "UPDATE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid + newIdAddition & "CardCompoundInputs SET ibaseid = " & iprojectid + newIdAddition & " WHERE ibaseid = " & iprojectid
            queriesNewId(29) = "UPDATE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid + newIdAddition & "Prices SET ibaseid = " & iprojectid + newIdAddition & " WHERE ibaseid = " & iprojectid
            queriesNewId(30) = "UPDATE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid + newIdAddition & "Timber SET ibaseid = " & iprojectid + newIdAddition & " WHERE ibaseid = " & iprojectid

            If executeTransactedSQLCommand(0, queriesNewId) = True Then
                iprojectid = iprojectid + newIdAddition
            End If

        End If

        Dim queriesSave(29) As String

        queriesSave(0) = "" & _
        "DELETE " & _
        "FROM base " & _
        "WHERE ibaseid = " & iprojectid & " AND " & _
        "NOT EXISTS (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & " tb WHERE base.ibaseid = tb.ibaseid) "

        queriesSave(1) = "" & _
        "DELETE " & _
        "FROM baseindirectcosts " & _
        "WHERE ibaseid = " & iprojectid & " AND " & _
        "NOT EXISTS (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "IndirectCosts tbic WHERE baseindirectcosts.ibaseid = tbic.ibaseid AND baseindirectcosts.icostid = tbic.icostid) "

        queriesSave(2) = "" & _
        "DELETE " & _
        "FROM basecards " & _
        "WHERE ibaseid = " & iprojectid & " AND " & _
        "NOT EXISTS (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Cards tbc WHERE basecards.ibaseid = tbc.ibaseid AND basecards.icardid = tbc.icardid) "

        queriesSave(3) = "" & _
        "DELETE " & _
        "FROM basecards " & _
        "WHERE ibaseid = " & iprojectid & " AND " & _
        "NOT EXISTS (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Cards tbc WHERE basecards.ibaseid = tbc.ibaseid AND basecards.icardid = tbc.icardid) "

        queriesSave(4) = "" & _
        "DELETE " & _
        "FROM projectcards " & _
        "WHERE NOT EXISTS (SELECT * FROM basecards bc WHERE bc.ibaseid = " & iprojectid & " AND projectcards.icardid = bc.icardid) "

        queriesSave(5) = "" & _
        "DELETE " & _
        "FROM modelcards " & _
        "WHERE NOT EXISTS (SELECT * FROM basecards bc WHERE bc.ibaseid = " & iprojectid & " AND modelcards.icardid = bc.icardid) "

        queriesSave(6) = "" & _
        "DELETE " & _
        "FROM basecardinputs " & _
        "WHERE ibaseid = " & iprojectid & " AND " & _
        "NOT EXISTS (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardInputs tbci WHERE basecardinputs.ibaseid = tbci.ibaseid AND basecardinputs.icardid = tbci.icardid AND basecardinputs.iinputid = tbci.iinputid) "

        queriesSave(7) = "" & _
        "DELETE " & _
        "FROM projectcardinputs " & _
        "WHERE NOT EXISTS (SELECT * FROM basecardinputs bci WHERE bci.ibaseid = " & iprojectid & " AND projectcardinputs.icardid = bci.icardid AND projectcardinputs.iinputid = bci.iinputid) "

        queriesSave(8) = "" & _
        "DELETE " & _
        "FROM modelcardinputs " & _
        "WHERE NOT EXISTS (SELECT * FROM basecardinputs bci WHERE bci.ibaseid = " & iprojectid & " AND modelcardinputs.icardid = bci.icardid AND modelcardinputs.iinputid = bci.iinputid) "

        queriesSave(9) = "" & _
        "DELETE " & _
        "FROM basecardcompoundinputs " & _
        "WHERE ibaseid = " & iprojectid & " AND " & _
        "NOT EXISTS (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardCompoundInputs tbcci WHERE basecardcompoundinputs.ibaseid = tbcci.ibaseid AND basecardcompoundinputs.icardid = tbcci.icardid AND basecardcompoundinputs.iinputid = tbcci.iinputid AND basecardcompoundinputs.icompoundinputid = tbcci.icompoundinputid) "

        queriesSave(10) = "" & _
        "DELETE " & _
        "FROM projectcardcompoundinputs " & _
        "WHERE NOT EXISTS (SELECT * FROM basecardcompoundinputs bcci WHERE bcci.ibaseid = " & iprojectid & " AND projectcardcompoundinputs.icardid = bcci.icardid AND projectcardcompoundinputs.iinputid = bcci.iinputid AND projectcardcompoundinputs.icompoundinputid = bcci.icompoundinputid) "

        queriesSave(11) = "" & _
        "DELETE " & _
        "FROM modelcardcompoundinputs " & _
        "WHERE NOT EXISTS (SELECT * FROM basecardcompoundinputs bcci WHERE bcci.ibaseid = " & iprojectid & " AND modelcardcompoundinputs.icardid = bcci.icardid AND modelcardcompoundinputs.iinputid = bcci.iinputid AND modelcardcompoundinputs.icompoundinputid = bcci.icompoundinputid) "

        queriesSave(12) = "" & _
        "DELETE " & _
        "FROM baseprices " & _
        "WHERE ibaseid = " & iprojectid & " AND " & _
        "NOT EXISTS (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Prices tbp WHERE baseprices.ibaseid = tbp.ibaseid AND baseprices.iinputid = tbp.iinputid AND baseprices.iupdatedate = tbp.iupdatedate AND baseprices.supdatetime = tbp.supdatetime) "

        queriesSave(13) = "" & _
        "DELETE " & _
        "FROM basetimber " & _
        "WHERE ibaseid = " & iprojectid & " AND " & _
        "NOT EXISTS (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Timber tbt WHERE basetimber.ibaseid = tbt.ibaseid AND basetimber.iinputid = tbt.iinputid) "

        queriesSave(14) = "" & _
        "UPDATE base p JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & " tp ON tp.ibaseid = p.ibaseid SET p.iupdatedate = tp.iupdatedate, p.supdatetime = tp.supdatetime, p.supdateusername = tp.supdateusername, p.sbasefileslocation = tp.sbasefileslocation, p.dbaseindirectpercentagedefault = tp.dbaseindirectpercentagedefault, p.dbasegainpercentagedefault = tp.dbasegainpercentagedefault, p.dbaseIVApercentage = tp.dbaseIVApercentage WHERE STR_TO_DATE(CONCAT(tp.iupdatedate, ' ', tp.supdatetime), '%Y%c%d %T') > STR_TO_DATE(CONCAT(p.iupdatedate, ' ', p.supdatetime), '%Y%c%d %T') "

        queriesSave(15) = "" & _
        "UPDATE baseindirectcosts pic JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "IndirectCosts tpic ON tpic.ibaseid = pic.ibaseid AND tpic.icostid = pic.icostid SET pic.iupdatedate = tpic.iupdatedate, pic.supdatetime = tpic.supdatetime, pic.supdateusername = tpic.supdateusername, pic.sbasecostname = tpic.sbasecostname, pic.dbasecost = tpic.dbasecost, pic.dcompanyprojectedearnings = tpic.dcompanyprojectedearnings WHERE STR_TO_DATE(CONCAT(tpic.iupdatedate, ' ', tpic.supdatetime), '%Y%c%d %T') > STR_TO_DATE(CONCAT(pic.iupdatedate, ' ', pic.supdatetime), '%Y%c%d %T') "

        queriesSave(16) = "" & _
        "UPDATE basecards pc JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Cards tpc ON tpc.ibaseid = pc.ibaseid AND tpc.icardid = pc.icardid SET pc.iupdatedate = tpc.iupdatedate, pc.supdatetime = tpc.supdatetime, pc.supdateusername = tpc.supdateusername, pc.scardlegacycategoryid = tpc.scardlegacycategoryid, pc.scardlegacyid = tpc.scardlegacyid, pc.scarddescription = tpc.scarddescription, pc.scardunit = tpc.scardunit, pc.dcardqty = tpc.dcardqty, pc.dcardindirectcostspercentage = tpc.dcardindirectcostspercentage, pc.dcardgainpercentage = tpc.dcardgainpercentage WHERE STR_TO_DATE(CONCAT(tpc.iupdatedate, ' ', tpc.supdatetime), '%Y%c%d %T') > STR_TO_DATE(CONCAT(pc.iupdatedate, ' ', pc.supdatetime), '%Y%c%d %T') "

        queriesSave(17) = "" & _
        "UPDATE basecardinputs pci JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardInputs tpci ON tpci.ibaseid = pci.ibaseid AND tpci.icardid = pci.icardid AND tpci.iinputid = pci.iinputid SET pci.iupdatedate = tpci.iupdatedate, pci.supdatetime = tpci.supdatetime, pci.supdateusername = tpci.supdateusername, pci.scardinputunit = tpci.scardinputunit, pci.dcardinputqty = tpci.dcardinputqty WHERE STR_TO_DATE(CONCAT(tpci.iupdatedate, ' ', tpci.supdatetime), '%Y%c%d %T') > STR_TO_DATE(CONCAT(pci.iupdatedate, ' ', pci.supdatetime), '%Y%c%d %T') "

        queriesSave(18) = "" & _
        "UPDATE basecardcompoundinputs pcci JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardCompoundInputs tpcci ON tpcci.ibaseid = pcci.ibaseid AND tpcci.icardid = pcci.icardid AND tpcci.iinputid = pcci.iinputid AND tpcci.icompoundinputid = pcci.icompoundinputid SET pcci.iupdatedate = tpcci.iupdatedate, pcci.supdatetime = tpcci.supdatetime, pcci.supdateusername = tpcci.supdateusername, pcci.scompoundinputunit = tpcci.scompoundinputunit, pcci.dcompoundinputqty = tpcci.dcompoundinputqty WHERE STR_TO_DATE(CONCAT(tpcci.iupdatedate, ' ', tpcci.supdatetime), '%Y%c%d %T') > STR_TO_DATE(CONCAT(pcci.iupdatedate, ' ', pcci.supdatetime), '%Y%c%d %T') "

        queriesSave(19) = "" & _
        "UPDATE baseprices pp JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Prices tpp ON tpp.ibaseid = pp.ibaseid AND tpp.iinputid = pp.iinputid AND tpp.iupdatedate = pp.iupdatedate AND tpp.supdatetime = pp.supdatetime SET pp.iupdatedate = tpp.iupdatedate, pp.supdatetime = tpp.supdatetime, pp.supdateusername = tpp.supdateusername, pp.dinputpricewithoutIVA = tpp.dinputpricewithoutIVA, pp.dinputprotectionpercentage = tpp.dinputprotectionpercentage, pp.dinputfinalprice = tpp.dinputfinalprice WHERE STR_TO_DATE(CONCAT(pp.iupdatedate, ' ', pp.supdatetime), '%Y%c%d %T') > STR_TO_DATE(CONCAT(pp.iupdatedate, ' ', pp.supdatetime), '%Y%c%d %T') "

        queriesSave(20) = "" & _
        "UPDATE basetimber pt JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Timber tpt ON tpt.ibaseid = pt.ibaseid AND tpt.iinputid = pt.iinputid SET pt.iupdatedate = tpt.iupdatedate, pt.supdatetime = tpt.supdatetime, pt.supdateusername = tpt.supdateusername, pt.dinputtimberespesor = tpt.dinputtimberespesor, pt.dinputtimberancho = tpt.dinputtimberancho, pt.dinputtimberlargo = tpt.dinputtimberlargo, pt.dinputtimberpreciopiecubico = tpt.dinputtimberpreciopiecubico WHERE STR_TO_DATE(CONCAT(tpt.iupdatedate, ' ', tpt.supdatetime), '%Y%c%d %T') > STR_TO_DATE(CONCAT(pt.iupdatedate, ' ', pt.supdatetime), '%Y%c%d %T') "

        queriesSave(21) = "" & _
        "INSERT INTO base " & _
        "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & " tb " & _
        "WHERE NOT EXISTS (SELECT * FROM base b WHERE b.ibaseid = tb.ibaseid AND b.ibaseid = " & iprojectid & ") "

        queriesSave(22) = "" & _
        "INSERT INTO baseindirectcosts " & _
        "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "IndirectCosts tbic " & _
        "WHERE NOT EXISTS (SELECT * FROM baseindirectcosts bic WHERE tbic.ibaseid = bic.ibaseid AND tbic.icostid = bic.icostid AND bic.ibaseid = " & iprojectid & ") "

        queriesSave(23) = "" & _
        "INSERT INTO basecards " & _
        "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Cards tbc " & _
        "WHERE NOT EXISTS (SELECT * FROM basecards bc WHERE tbc.ibaseid = bc.ibaseid AND tbc.icardid = bc.icardid AND bc.ibaseid = " & iprojectid & ") "

        queriesSave(24) = "" & _
        "INSERT INTO basecardinputs " & _
        "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardInputs tbci " & _
        "WHERE NOT EXISTS (SELECT * FROM basecardinputs bci WHERE tbci.ibaseid = bci.ibaseid AND tbci.icardid = bci.icardid AND tbci.iinputid = bci.iinputid AND bci.ibaseid = " & iprojectid & ") "

        queriesSave(25) = "" & _
        "INSERT INTO basecardcompoundinputs " & _
        "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardCompoundInputs tbcci " & _
        "WHERE NOT EXISTS (SELECT * FROM basecardcompoundinputs bcci WHERE tbcci.ibaseid = bcci.ibaseid AND tbcci.icardid = bcci.icardid AND tbcci.iinputid = bcci.iinputid AND tbcci.icompoundinputid = bcci.icompoundinputid AND bcci.ibaseid = " & iprojectid & ") "

        queriesSave(26) = "" & _
        "INSERT INTO baseprices " & _
        "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Prices tbp " & _
        "WHERE NOT EXISTS (SELECT * FROM baseprices bp WHERE tbp.ibaseid = bp.ibaseid AND tbp.iinputid = bp.iinputid AND tbp.iupdatedate = bp.iupdatedate AND tbp.supdatetime = bp.supdatetime AND bp.ibaseid = " & iprojectid & ") "

        queriesSave(27) = "" & _
        "INSERT INTO basetimber " & _
        "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Timber tbt " & _
        "WHERE NOT EXISTS (SELECT * FROM basetimber bt WHERE tbt.ibaseid = bt.ibaseid AND tbt.iinputid = bt.iinputid AND bt.ibaseid = " & iprojectid & ") "

        queriesSave(28) = "INSERT IGNORE INTO logs VALUES (" & getMySQLDate() & ", '" & getAppTime() & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Guardó cambios al Presupuesto Base " & iprojectid & ": " & dtFechaPresupuesto.Value & "', 'OK')"

        If executeTransactedSQLCommand(0, queriesSave) = True Then
            MsgBox("Guardado exitosamente", MsgBoxStyle.OkOnly, "")
        Else
            MsgBox("Hubo un error al Guardar. Probablemente un error de Red. Intenta nuevamente", MsgBoxStyle.OkOnly, "")
            Exit Sub
        End If

        wasCreated = True

        Cursor.Current = System.Windows.Forms.Cursors.Default

        Me.DialogResult = Windows.Forms.DialogResult.OK
        Me.Close()

    End Sub


    Private Sub btnCancelar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancelar.Click

        Me.DialogResult = Windows.Forms.DialogResult.Cancel
        Me.Close()

        wasCreated = False

    End Sub


    Private Sub btnRevisiones_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRevisiones.Click

        If MsgBox("Revisar un Presupuesto Base automáticamente guarda sus cambios. ¿Deseas guardar este Presupuesto Base ahora?", MsgBoxStyle.YesNo, "Pregunta Guardado") = MsgBoxResult.No Then
            Exit Sub
        End If

        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        Dim timesBaseIsOpen As Integer = 1

        timesBaseIsOpen = getSQLQueryAsInteger(0, "SELECT COUNT(*) FROM information_schema.TABLES WHERE TABLE_NAME LIKE '%Base" & iprojectid & "'")

        If timesBaseIsOpen > 1 And isEdit = True Then

            Cursor.Current = System.Windows.Forms.Cursors.Default

            If MsgBox("Otro usuario tiene abierto el mismo Presupuesto Base. Esto podría causar que alguno de ustedes perdiera los cambios que hiciera. ¿Deseas seguir guardando el Presupuesto Base?", MsgBoxStyle.YesNo, "Confirmación Guardado") = MsgBoxResult.No Then

                Exit Sub

            Else

                Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

            End If

        ElseIf timesBaseIsOpen > 1 And isEdit = False Then

            Dim newIdAddition As Integer = 1

            Do While getSQLQueryAsInteger(0, "SELECT COUNT(*) FROM information_schema.TABLES WHERE TABLE_NAME LIKE '%Base" & iprojectid + newIdAddition & "'") > 1 And isEdit = False
                newIdAddition = newIdAddition + 1
            Loop

            'I got the new id (previousId + newIdAddition)

            Dim queriesNewId(31) As String

            queriesNewId(8) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & " RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid + newIdAddition
            queriesNewId(9) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "IndirectCosts RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid + newIdAddition & "IndirectCosts"
            queriesNewId(10) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Cards RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid + newIdAddition & "Cards"
            queriesNewId(11) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardInputs RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid + newIdAddition & "CardInputs"
            queriesNewId(12) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardCompoundInputs RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid + newIdAddition & "CardCompoundInputs"
            queriesNewId(13) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Prices RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid + newIdAddition & "Prices"
            queriesNewId(14) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Timber RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid + newIdAddition & "Timber"
            queriesNewId(15) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "CardsAux RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid + newIdAddition & "CardsAux"
            queriesNewId(24) = "UPDATE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid + newIdAddition & " SET ibaseid = " & iprojectid + newIdAddition & " WHERE ibaseid = " & iprojectid
            queriesNewId(25) = "UPDATE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid + newIdAddition & "IndirectCosts SET ibaseid = " & iprojectid + newIdAddition & " WHERE ibaseid = " & iprojectid
            queriesNewId(26) = "UPDATE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid + newIdAddition & "Cards SET ibaseid = " & iprojectid + newIdAddition & " WHERE ibaseid = " & iprojectid
            queriesNewId(27) = "UPDATE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid + newIdAddition & "CardInputs SET ibaseid = " & iprojectid + newIdAddition & " WHERE ibaseid = " & iprojectid
            queriesNewId(28) = "UPDATE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid + newIdAddition & "CardCompoundInputs SET ibaseid = " & iprojectid + newIdAddition & " WHERE ibaseid = " & iprojectid
            queriesNewId(29) = "UPDATE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid + newIdAddition & "Prices SET ibaseid = " & iprojectid + newIdAddition & " WHERE ibaseid = " & iprojectid
            queriesNewId(30) = "UPDATE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid + newIdAddition & "Timber SET ibaseid = " & iprojectid + newIdAddition & " WHERE ibaseid = " & iprojectid

            If executeTransactedSQLCommand(0, queriesNewId) = True Then
                iprojectid = iprojectid + newIdAddition
            End If

        End If

        Dim queriesSave(29) As String

        queriesSave(0) = "" & _
        "DELETE " & _
        "FROM base " & _
        "WHERE ibaseid = " & iprojectid & " AND " & _
        "NOT EXISTS (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & " tb WHERE base.ibaseid = tb.ibaseid) "

        queriesSave(1) = "" & _
        "DELETE " & _
        "FROM baseindirectcosts " & _
        "WHERE ibaseid = " & iprojectid & " AND " & _
        "NOT EXISTS (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "IndirectCosts tbic WHERE baseindirectcosts.ibaseid = tbic.ibaseid AND baseindirectcosts.icostid = tbic.icostid) "

        queriesSave(2) = "" & _
        "DELETE " & _
        "FROM basecards " & _
        "WHERE ibaseid = " & iprojectid & " AND " & _
        "NOT EXISTS (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Cards tbc WHERE basecards.ibaseid = tbc.ibaseid AND basecards.icardid = tbc.icardid) "

        queriesSave(3) = "" & _
        "DELETE " & _
        "FROM basecards " & _
        "WHERE ibaseid = " & iprojectid & " AND " & _
        "NOT EXISTS (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Cards tbc WHERE basecards.ibaseid = tbc.ibaseid AND basecards.icardid = tbc.icardid) "

        queriesSave(4) = "" & _
        "DELETE " & _
        "FROM projectcards " & _
        "WHERE NOT EXISTS (SELECT * FROM basecards bc WHERE bc.ibaseid = " & iprojectid & " AND projectcards.icardid = bc.icardid) "

        queriesSave(5) = "" & _
        "DELETE " & _
        "FROM modelcards " & _
        "WHERE NOT EXISTS (SELECT * FROM basecards bc WHERE bc.ibaseid = " & iprojectid & " AND modelcards.icardid = bc.icardid) "

        queriesSave(6) = "" & _
        "DELETE " & _
        "FROM basecardinputs " & _
        "WHERE ibaseid = " & iprojectid & " AND " & _
        "NOT EXISTS (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardInputs tbci WHERE basecardinputs.ibaseid = tbci.ibaseid AND basecardinputs.icardid = tbci.icardid AND basecardinputs.iinputid = tbci.iinputid) "

        queriesSave(7) = "" & _
        "DELETE " & _
        "FROM projectcardinputs " & _
        "WHERE NOT EXISTS (SELECT * FROM basecardinputs bci WHERE bci.ibaseid = " & iprojectid & " AND projectcardinputs.icardid = bci.icardid AND projectcardinputs.iinputid = bci.iinputid) "

        queriesSave(8) = "" & _
        "DELETE " & _
        "FROM modelcardinputs " & _
        "WHERE NOT EXISTS (SELECT * FROM basecardinputs bci WHERE bci.ibaseid = " & iprojectid & " AND modelcardinputs.icardid = bci.icardid AND modelcardinputs.iinputid = bci.iinputid) "

        queriesSave(9) = "" & _
        "DELETE " & _
        "FROM basecardcompoundinputs " & _
        "WHERE ibaseid = " & iprojectid & " AND " & _
        "NOT EXISTS (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardCompoundInputs tbcci WHERE basecardcompoundinputs.ibaseid = tbcci.ibaseid AND basecardcompoundinputs.icardid = tbcci.icardid AND basecardcompoundinputs.iinputid = tbcci.iinputid AND basecardcompoundinputs.icompoundinputid = tbcci.icompoundinputid) "

        queriesSave(10) = "" & _
        "DELETE " & _
        "FROM projectcardcompoundinputs " & _
        "WHERE NOT EXISTS (SELECT * FROM basecardcompoundinputs bcci WHERE bcci.ibaseid = " & iprojectid & " AND projectcardcompoundinputs.icardid = bcci.icardid AND projectcardcompoundinputs.iinputid = bcci.iinputid AND projectcardcompoundinputs.icompoundinputid = bcci.icompoundinputid) "

        queriesSave(11) = "" & _
        "DELETE " & _
        "FROM modelcardcompoundinputs " & _
        "WHERE NOT EXISTS (SELECT * FROM basecardcompoundinputs bcci WHERE bcci.ibaseid = " & iprojectid & " AND modelcardcompoundinputs.icardid = bcci.icardid AND modelcardcompoundinputs.iinputid = bcci.iinputid AND modelcardcompoundinputs.icompoundinputid = bcci.icompoundinputid) "

        queriesSave(12) = "" & _
        "DELETE " & _
        "FROM baseprices " & _
        "WHERE ibaseid = " & iprojectid & " AND " & _
        "NOT EXISTS (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Prices tbp WHERE baseprices.ibaseid = tbp.ibaseid AND baseprices.iinputid = tbp.iinputid AND baseprices.iupdatedate = tbp.iupdatedate AND baseprices.supdatetime = tbp.supdatetime) "

        queriesSave(13) = "" & _
        "DELETE " & _
        "FROM basetimber " & _
        "WHERE ibaseid = " & iprojectid & " AND " & _
        "NOT EXISTS (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Timber tbt WHERE basetimber.ibaseid = tbt.ibaseid AND basetimber.iinputid = tbt.iinputid) "

        queriesSave(14) = "" & _
        "UPDATE base p JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & " tp ON tp.ibaseid = p.ibaseid SET p.iupdatedate = tp.iupdatedate, p.supdatetime = tp.supdatetime, p.supdateusername = tp.supdateusername, p.sbasefileslocation = tp.sbasefileslocation, p.dbaseindirectpercentagedefault = tp.dbaseindirectpercentagedefault, p.dbasegainpercentagedefault = tp.dbasegainpercentagedefault, p.dbaseIVApercentage = tp.dbaseIVApercentage WHERE STR_TO_DATE(CONCAT(tp.iupdatedate, ' ', tp.supdatetime), '%Y%c%d %T') > STR_TO_DATE(CONCAT(p.iupdatedate, ' ', p.supdatetime), '%Y%c%d %T') "

        queriesSave(15) = "" & _
        "UPDATE baseindirectcosts pic JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "IndirectCosts tpic ON tpic.ibaseid = pic.ibaseid AND tpic.icostid = pic.icostid SET pic.iupdatedate = tpic.iupdatedate, pic.supdatetime = tpic.supdatetime, pic.supdateusername = tpic.supdateusername, pic.sbasecostname = tpic.sbasecostname, pic.dbasecost = tpic.dbasecost, pic.dcompanyprojectedearnings = tpic.dcompanyprojectedearnings WHERE STR_TO_DATE(CONCAT(tpic.iupdatedate, ' ', tpic.supdatetime), '%Y%c%d %T') > STR_TO_DATE(CONCAT(pic.iupdatedate, ' ', pic.supdatetime), '%Y%c%d %T') "

        queriesSave(16) = "" & _
        "UPDATE basecards pc JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Cards tpc ON tpc.ibaseid = pc.ibaseid AND tpc.icardid = pc.icardid SET pc.iupdatedate = tpc.iupdatedate, pc.supdatetime = tpc.supdatetime, pc.supdateusername = tpc.supdateusername, pc.scardlegacycategoryid = tpc.scardlegacycategoryid, pc.scardlegacyid = tpc.scardlegacyid, pc.scarddescription = tpc.scarddescription, pc.scardunit = tpc.scardunit, pc.dcardqty = tpc.dcardqty, pc.dcardindirectcostspercentage = tpc.dcardindirectcostspercentage, pc.dcardgainpercentage = tpc.dcardgainpercentage WHERE STR_TO_DATE(CONCAT(tpc.iupdatedate, ' ', tpc.supdatetime), '%Y%c%d %T') > STR_TO_DATE(CONCAT(pc.iupdatedate, ' ', pc.supdatetime), '%Y%c%d %T') "

        queriesSave(17) = "" & _
        "UPDATE basecardinputs pci JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardInputs tpci ON tpci.ibaseid = pci.ibaseid AND tpci.icardid = pci.icardid AND tpci.iinputid = pci.iinputid SET pci.iupdatedate = tpci.iupdatedate, pci.supdatetime = tpci.supdatetime, pci.supdateusername = tpci.supdateusername, pci.scardinputunit = tpci.scardinputunit, pci.dcardinputqty = tpci.dcardinputqty WHERE STR_TO_DATE(CONCAT(tpci.iupdatedate, ' ', tpci.supdatetime), '%Y%c%d %T') > STR_TO_DATE(CONCAT(pci.iupdatedate, ' ', pci.supdatetime), '%Y%c%d %T') "

        queriesSave(18) = "" & _
        "UPDATE basecardcompoundinputs pcci JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardCompoundInputs tpcci ON tpcci.ibaseid = pcci.ibaseid AND tpcci.icardid = pcci.icardid AND tpcci.iinputid = pcci.iinputid AND tpcci.icompoundinputid = pcci.icompoundinputid SET pcci.iupdatedate = tpcci.iupdatedate, pcci.supdatetime = tpcci.supdatetime, pcci.supdateusername = tpcci.supdateusername, pcci.scompoundinputunit = tpcci.scompoundinputunit, pcci.dcompoundinputqty = tpcci.dcompoundinputqty WHERE STR_TO_DATE(CONCAT(tpcci.iupdatedate, ' ', tpcci.supdatetime), '%Y%c%d %T') > STR_TO_DATE(CONCAT(pcci.iupdatedate, ' ', pcci.supdatetime), '%Y%c%d %T') "

        queriesSave(19) = "" & _
        "UPDATE baseprices pp JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Prices tpp ON tpp.ibaseid = pp.ibaseid AND tpp.iinputid = pp.iinputid AND tpp.iupdatedate = pp.iupdatedate AND tpp.supdatetime = pp.supdatetime SET pp.iupdatedate = tpp.iupdatedate, pp.supdatetime = tpp.supdatetime, pp.supdateusername = tpp.supdateusername, pp.dinputpricewithoutIVA = tpp.dinputpricewithoutIVA, pp.dinputprotectionpercentage = tpp.dinputprotectionpercentage, pp.dinputfinalprice = tpp.dinputfinalprice WHERE STR_TO_DATE(CONCAT(pp.iupdatedate, ' ', pp.supdatetime), '%Y%c%d %T') > STR_TO_DATE(CONCAT(pp.iupdatedate, ' ', pp.supdatetime), '%Y%c%d %T') "

        queriesSave(20) = "" & _
        "UPDATE basetimber pt JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Timber tpt ON tpt.ibaseid = pt.ibaseid AND tpt.iinputid = pt.iinputid SET pt.iupdatedate = tpt.iupdatedate, pt.supdatetime = tpt.supdatetime, pt.supdateusername = tpt.supdateusername, pt.dinputtimberespesor = tpt.dinputtimberespesor, pt.dinputtimberancho = tpt.dinputtimberancho, pt.dinputtimberlargo = tpt.dinputtimberlargo, pt.dinputtimberpreciopiecubico = tpt.dinputtimberpreciopiecubico WHERE STR_TO_DATE(CONCAT(tpt.iupdatedate, ' ', tpt.supdatetime), '%Y%c%d %T') > STR_TO_DATE(CONCAT(pt.iupdatedate, ' ', pt.supdatetime), '%Y%c%d %T') "

        queriesSave(21) = "" & _
        "INSERT INTO base " & _
        "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & " tb " & _
        "WHERE NOT EXISTS (SELECT * FROM base b WHERE b.ibaseid = tb.ibaseid AND b.ibaseid = " & iprojectid & ") "

        queriesSave(22) = "" & _
        "INSERT INTO baseindirectcosts " & _
        "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "IndirectCosts tbic " & _
        "WHERE NOT EXISTS (SELECT * FROM baseindirectcosts bic WHERE tbic.ibaseid = bic.ibaseid AND tbic.icostid = bic.icostid AND bic.ibaseid = " & iprojectid & ") "

        queriesSave(23) = "" & _
        "INSERT INTO basecards " & _
        "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Cards tbc " & _
        "WHERE NOT EXISTS (SELECT * FROM basecards bc WHERE tbc.ibaseid = bc.ibaseid AND tbc.icardid = bc.icardid AND bc.ibaseid = " & iprojectid & ") "

        queriesSave(24) = "" & _
        "INSERT INTO basecardinputs " & _
        "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardInputs tbci " & _
        "WHERE NOT EXISTS (SELECT * FROM basecardinputs bci WHERE tbci.ibaseid = bci.ibaseid AND tbci.icardid = bci.icardid AND tbci.iinputid = bci.iinputid AND bci.ibaseid = " & iprojectid & ") "

        queriesSave(25) = "" & _
        "INSERT INTO basecardcompoundinputs " & _
        "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardCompoundInputs tbcci " & _
        "WHERE NOT EXISTS (SELECT * FROM basecardcompoundinputs bcci WHERE tbcci.ibaseid = bcci.ibaseid AND tbcci.icardid = bcci.icardid AND tbcci.iinputid = bcci.iinputid AND tbcci.icompoundinputid = bcci.icompoundinputid AND bcci.ibaseid = " & iprojectid & ") "

        queriesSave(26) = "" & _
        "INSERT INTO baseprices " & _
        "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Prices tbp " & _
        "WHERE NOT EXISTS (SELECT * FROM baseprices bp WHERE tbp.ibaseid = bp.ibaseid AND tbp.iinputid = bp.iinputid AND tbp.iupdatedate = bp.iupdatedate AND tbp.supdatetime = bp.supdatetime AND bp.ibaseid = " & iprojectid & ") "

        queriesSave(27) = "" & _
        "INSERT INTO basetimber " & _
        "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Timber tbt " & _
        "WHERE NOT EXISTS (SELECT * FROM basetimber bt WHERE tbt.ibaseid = bt.ibaseid AND tbt.iinputid = bt.iinputid AND bt.ibaseid = " & iprojectid & ") "

        queriesSave(28) = "INSERT IGNORE INTO logs VALUES (" & getMySQLDate() & ", '" & getAppTime() & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Guardó cambios al Presupuesto Base " & iprojectid & ": " & dtFechaPresupuesto.Value & "', 'OK')"

        If executeTransactedSQLCommand(0, queriesSave) = True Then

            MsgBox("Guardado exitosamente", MsgBoxStyle.OkOnly, "")

            wasCreated = True

            Dim br As New BuscaRevisiones

            br.susername = susername
            br.bactive = bactive
            br.bonline = bonline
            br.suserfullname = suserfullname
            br.suseremail = suseremail
            br.susersession = susersession
            br.susermachinename = susermachinename
            br.suserip = suserip

            br.isEdit = True

            br.srevisiondocument = "Presupuesto Base"
            br.sid = iprojectid

            If Me.WindowState = FormWindowState.Maximized Then
                br.WindowState = FormWindowState.Maximized
            End If

            Me.Visible = False
            br.ShowDialog(Me)
            Me.Visible = True

        Else

            MsgBox("Hubo un error al Guardar. Probablemente un error de Red. Intenta nuevamente", MsgBoxStyle.OkOnly, "")
            Exit Sub

        End If

        Cursor.Current = System.Windows.Forms.Cursors.Default

    End Sub


End Class