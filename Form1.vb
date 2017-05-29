Imports Emgu.CV
Imports Emgu.Util
Imports Emgu.CV.OCR
Imports Emgu.CV.Structure
Imports System.Data.OleDb
Public Class Form1
    'Dim OCRz As Tesseract = New Tesseract("tessdata", "eng", Tesseract.OcrEngineMode.OEM_DEFAULT)
    Dim OCRzz As Tesseract = New Tesseract("tessdata", "eng", Tesseract.OcrEngineMode.OEM_DEFAULT)
    Dim pic As Bitmap = New Bitmap(350, 150)
    Dim picUpload As Bitmap = New Bitmap(270, 100)
    'Dim gfx As Graphics = Graphics.FromImage(pic)
    Dim OCRz As Tesseract = New Tesseract("tessdata", "eng", Tesseract.OcrEngineMode.OEM_DEFAULT)
    'Dim pic As Bitmap = New Bitmap(270, 100)
    Dim gfx As Graphics = Graphics.FromImage(pic)
    Dim capturez As Capture = New Capture

    Dim oledbpenghubung As OleDbDataAdapter
    Dim ds As New DataSet()
    Dim conn As OleDbConnection
    Dim query As String
    Dim constring As String = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=F:\ \pemvis\Extract Text From Image OCR\databaseCustomer.accdb"

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Dim OpenFileDialog1 As New OpenFileDialog
        OpenFileDialog1.Filter = "Picture Files (*)|*.bmp;*.gif;*.jpg"
        If OpenFileDialog1.ShowDialog = Windows.Forms.DialogResult.OK Then
            PictureBox2.Image = Image.FromFile(OpenFileDialog1.FileName)
            picUpload = PictureBox2.Image
        End If
    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        gfx.CopyFromScreen(New Point(Me.Location.X + PictureBox1.Location.X + 4, Me.Location.Y + PictureBox1.Location.Y + 30), New Point(0, 0), pic.Size)
        PictureBox1.Image = pic
        PictureBox1.Image = Nothing
        Dim imagez As Image(Of Bgr, Byte) = capturez.QueryFrame() 'Instead of QueryFrame, you may need to do RetrieveBgrFrame depending on the version of EmguCV you download.
        PictureBox1.Image = imagez.ToBitmap()
    End Sub

    'SCAN PADA CAM
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        pic = PictureBox1.Image
        OCRz.Recognize(New Image(Of Bgr, Byte)(pic))
        RichTextBox1.Text = OCRz.GetText
        TextBox1.Text = RichTextBox1.Text
    End Sub

    'SCAN PADA UPLOAD
    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        OCRzz.Recognize(New Image(Of Bgr, Byte)(picUpload))
        RichTextBox2.Text = OCRzz.GetText
        TextBox1.Text = RichTextBox2.Text
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        RichTextBox1.Clear()
    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        RichTextBox2.Clear()
    End Sub
    Sub tampil()
        ds.Clear()
        query = "select * from tbl_customer"
        oledbpenghubung = New OleDbDataAdapter(query, conn)
        oledbpenghubung.Fill(ds, "databaseCustomer")
        DataGridView1.DataSource = ds.Tables("databaseCustomer")
    End Sub
    Function cari(ByVal plat As String)
        query = "select * from tbl_customer where noPlat='" & plat & "'"
        Dim dscek As New DataSet()
        conn = New OleDbConnection(constring)
        conn.Open()
        oledbpenghubung = New OleDbDataAdapter(query, conn)
        oledbpenghubung.Fill(dscek, "ngecek")
        Return dscek.Tables("ngecek").Rows.Count
    End Function

    Private Sub Button7_Click(sender As Object, e As EventArgs) Handles Button7.Click
        If TextBox1.Text <> "" And TextBox2.Text <> "" And TextBox3.Text <> "" And TextBox4.Text <> "" Then
            Dim command As OleDbCommand
            Dim isi As Integer = cari(TextBox1.Text)
            If isi = 0 Then
                query = "insert into tbl_customer values('" & TextBox1.Text & "','" & TextBox2.Text & "','" & TextBox3.Text & "','" & TextBox4.Text & "')"
                command = New OleDbCommand(query, conn)
                command.ExecuteNonQuery()
                tampil()
                MsgBox("insert anda berhasil")
            Else
                MsgBox("No.plat sudah ada", MsgBoxStyle.Information)
            End If
            conn.Close()
        End If
    End Sub

    Private Sub Button8_Click(sender As Object, e As EventArgs) Handles Button8.Click
        If TextBox1.Text <> "" And TextBox2.Text <> "" And TextBox3.Text <> "" And TextBox4.Text <> "" Then
            Dim command As OleDbCommand
            Dim isi As Integer = cari(TextBox1.Text)
            If isi <> 0 Then
                query = "update tbl_customer set nama='" & TextBox2.Text & ", alamat='" & TextBox3.Text & "', nomerHP='" & TextBox4.Text & "'where noPlat='" & TextBox1.Text & "'"
                oledbpenghubung = New OleDbDataAdapter(query, conn)
                oledbpenghubung.Fill(ds, "databaseCustomer")
                tampil()
                MsgBox("update anda berhasil")
            Else
                MsgBox("data-data belum diisi", MsgBoxStyle.Information)
            End If
            conn.Close()
        End If
    End Sub

    Private Sub Button9_Click(sender As Object, e As EventArgs) Handles Button9.Click
        If TextBox1.Text <> "" And TextBox2.Text <> "" And TextBox3.Text <> "" And TextBox4.Text <> "" Then
            Dim command As OleDbCommand
            Dim isi As Integer = cari(TextBox1.Text)
            If isi <> 0 Then
                query = "delete from tbl_customer where noPlat='" & TextBox1.Text & "'"
                command = New OleDbCommand(query, conn)
                command.ExecuteNonQuery()

                tampil()
                MsgBox("delete anda berhasil")
            Else
                MsgBox("No.plat tidak ada", MsgBoxStyle.Information)
            End If
            conn.Close()
        End If
    End Sub

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        'TODO: This line of code loads data into the 'DatabaseCustomerDataSet1.tbl_customer' table. You can move, or remove it, as needed.
        Me.Tbl_customerTableAdapter1.Fill(Me.DatabaseCustomerDataSet1.tbl_customer)
        'TODO: This line of code loads data into the 'DatabaseCustomerDataSet.tbl_customer' table. You can move, or remove it, as needed.
        Me.Tbl_customerTableAdapter.Fill(Me.DatabaseCustomerDataSet.tbl_customer)
    End Sub

    Private Sub TextBox1_LostFocus(sender As Object, e As EventArgs) Handles TextBox1.LostFocus
        Dim a As Integer = 0
        While a < DataGridView1.Rows.Count - 1
            If TextBox1.Text = DataGridView1.Rows(a).Cells(0).Value Then
                TextBox2.Text = DataGridView1.Rows(a).Cells(1).Value
                TextBox3.Text = DataGridView1.Rows(a).Cells(2).Value
                TextBox4.Text = DataGridView1.Rows(a).Cells(3).Value
            End If
            a += 1
        End While
    End Sub
End Class
