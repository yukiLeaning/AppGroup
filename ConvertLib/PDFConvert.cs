using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Text;

namespace ConvertLib
{
    public class PDFConvert
    {
        #region 画像 ⇒ PDF
        /// <summary>
        /// 複数画像を1枚のPDFに変換する.
        /// </summary>
        /// <param name="inputFilePaths">PDFに変換する画像パス</param>
        /// <param name="outputFilePath">変換後のPDFの保存先</param>
        /// <returns>成否</returns>
        public static bool ConvertImage2PDF(string[] inputFilePaths, string outputFilePath)
        {
            bool result = true;

            //出力pdfのバイナリ格納用。  
            List<byte> ary_pdf_file = new List<byte>();

            //出力pdfのバイナリ各オブジェクトの、バイト開始位置を格納。  
            List<Int64> ary_pdf_byte_head = new List<Int64>();

            //改行用文字列  
            const string vbCrLf = "\r\n"; //Environment.NewLine  

            //子オブジェクトの位置の指定用  
            const string pdf_indicate_obj = "NO 0 R ";
            //子オブジェクト名＆番号を格納  
            const string pdf_obj_Name = "CC 0 obj";

            //△冒頭の構文( header )------------------------------------------------------------------------------------------------------------  
            byte[] pdf_header_start = System.Text.Encoding.ASCII.GetBytes(
                "%PDF-1.5" + vbCrLf + "%TageSP" + vbCrLf);

            //△1番目のオブジェクト( 1 0 obj )------------------------------------------------------  
            byte[] pdf_1st_obj = System.Text.Encoding.ASCII.GetBytes(
                "1 0 obj" + vbCrLf +
                "<<" + vbCrLf + "/Type /Catalog" + vbCrLf +
                "/Pages 2 0 R" + vbCrLf +
                ">>" + vbCrLf + "endobj" + vbCrLf);

            //△2番目のオブジェクト( 2 0 obj )------------------------------------------------------  
            byte[] pdf_2nd_obj_Kids = System.Text.Encoding.ASCII.GetBytes(
                "2 0 obj" + vbCrLf + "<<" +
                "/Type /Pages" + vbCrLf + "/Kids [ ");

            //ページ数指定用  
            byte[] pdf_2nd_obj_Count = System.Text.Encoding.ASCII.GetBytes(
                "]" + vbCrLf + "/Count ");

            //ヘッダーの終端  
            byte[] pdf_2nd_obj_End = System.Text.Encoding.ASCII.GetBytes(
                vbCrLf + ">>" + vbCrLf + "endobj" + vbCrLf);

            //■子オブジェクト関係( Kids )**************************************************  

            //△3番目のオブジェクト( 3 0 obj )------------------------------------------------------  
            //キャンバスのサイズ設定  
            byte[] pdf_3rd_obj_Start = System.Text.Encoding.ASCII.GetBytes(
                vbCrLf + "<<" + vbCrLf + "/Type /Page" + vbCrLf +
                "/Parent 2 0 R" + vbCrLf + "/MediaBox  [ 0 0 ");

            const string pdf_3rd_obj_MediaBoxSize = "WW HH ]";

            //画像の配置指定の設定  
            byte[] pdf_3rd_obj_Resources = System.Text.Encoding.ASCII.GetBytes(
                vbCrLf + "/Resources << /ProcSet [ /PDF /ImageB ]" + vbCrLf);

            const string pdf_3rd_obj_XObject = "/XObject << /Im0 XX 0 R >>" + vbCrLf + ">>";

            byte[] pdf_3rd_obj_Contents =
                System.Text.Encoding.ASCII.GetBytes(
                vbCrLf + "/Contents ");

            byte[] pdf_3rd_obj_End =
                System.Text.Encoding.ASCII.GetBytes(
                vbCrLf + ">>" + vbCrLf +
                "endobj" + vbCrLf);

            //△4番目のオブジェクト( 4 0 obj )------------------------------------------------------  
            //画像配置の前のバイナリ&画像本体のサイズ指定  
            byte[] pdf_4th_obj_Start =
                System.Text.Encoding.ASCII.GetBytes(
                vbCrLf + "<<" + vbCrLf + "/Type /XObject" + vbCrLf +
                "/Subtype /Image" + vbCrLf + "/");

            const string pdf_4th_obj_ImageSize =
                "Width AA /Height BB";

            byte[] pdf_4th_obj_Filter =
                System.Text.Encoding.ASCII.GetBytes(
                vbCrLf + "/BitsPerComponent 8" + vbCrLf +
                "/ColorSpace /DeviceRGB" + vbCrLf +
                "/Filter /DCTDecode" + vbCrLf);

            const string pdf_4th_obj_Length =
                "/Length LLLLL" + vbCrLf + ">>" + vbCrLf +
                "stream" + vbCrLf;

            //画像配置の後のバイナリ&画像の表示サイズの指定  
            byte[] pdf_4th_obj_End =
                System.Text.Encoding.ASCII.GetBytes(
                vbCrLf + "endstream" + vbCrLf + "endobj" + vbCrLf);


            //△5番目のオブジェクト( 5 0 obj )------------------------------------------------------  
            const string pdf_5th_obj_Length =
                vbCrLf + "<< /Length LLLLL >>" +
                vbCrLf + "stream" + vbCrLf;

            const string pdf_5th_obj_ShowSize =
                "q WW 0 0 HH 0 0 cm /Im0 Do Q";

            byte[] pdf_5th_obj_End =
                System.Text.Encoding.ASCII.GetBytes(
                vbCrLf + "endstream" +
                vbCrLf + "endobj" + vbCrLf);


            //■フッター( footer )------------------------------------------------------  

            //△xref関係( xref )------------------------------------------------------  
            byte[] pdf_xref_Start =
                System.Text.Encoding.ASCII.GetBytes("xref" + vbCrLf);

            const string pdf_xref_objCount = "0 MM" + vbCrLf;

            byte[] pdf_xref_ZERO =
                System.Text.Encoding.ASCII.GetBytes(
                "0000000000 65535 f" + vbCrLf);

            const string pdf_xref_objStartPos =
                "QQQQQQQQQQ 00000 n" + vbCrLf;


            //△trailer関係( trailer )------------------------------------------------------  
            const string pdf_trailer =
                "trailer" + vbCrLf + "<<" + vbCrLf +
                "/Size MM" + vbCrLf +
                "/Root 1 0 R" + vbCrLf +
                ">>" + vbCrLf;

            //△EOF関係( EOF )------------------------------------------------------  
            const string pdf_startxref_EOF =
                "startxref" + vbCrLf + "TTT" + vbCrLf +
                "%%EOF" + vbCrLf;

            try
            {
                //出力先のパスにファイルがある場合は、先に消す 
                if (File.Exists(outputFilePath))
                {
                    File.Delete(outputFilePath);
                }

                //出力pdfバイナリの初期化
                ary_pdf_file = new List<byte>();
                ary_pdf_byte_head = new List<long>();

                //■ファイルを作成して書き込む
                FileStream fs = new FileStream(outputFilePath, FileMode.Create, FileAccess.Write);

                /// ヘッダーの作成
                //冒頭(Add header binary.)
                ary_pdf_file.AddRange(pdf_header_start);
                //Add 1st object start bytes position.
                ary_pdf_byte_head.Add(Convert.ToInt64(ary_pdf_file.Count)); //1オブジェクトの開始番号を格納  

                //オブジェクト1(Add 1st object binary.)
                ary_pdf_file.AddRange(pdf_1st_obj);
                //Add 2nd object start bytes position.
                ary_pdf_byte_head.Add(Convert.ToInt64(ary_pdf_file.Count)); //2オブジェクトの開始番号を格納  

                //オブジェクト2(Add 2nd object binary.)
                ary_pdf_file.AddRange(pdf_2nd_obj_Kids);

                byte[] pdf_write_binary;

                //ページ数だけ、子オブジェクトを指定・追加する
                for (int i = 3; i <= (inputFilePaths.Length * 3); i += 3)
                {
                    //各・子オブジェクトの開始番号を、格納
                    //Set start position of kids object No.  
                    string pdf_2nd_obj_Kids_Set = pdf_indicate_obj.Replace("NO", i.ToString());

                    //文字列を、バイト配列に変換して、格納する。  
                    pdf_write_binary = Encoding.ASCII.GetBytes(pdf_2nd_obj_Kids_Set);
                    ary_pdf_file.AddRange(pdf_write_binary);

                    //配列を初期化  
                    pdf_write_binary = new byte[] { };
                }

                //ページ数を格納する。  
                ary_pdf_file.AddRange(pdf_2nd_obj_Count);
                pdf_write_binary = Encoding.ASCII.GetBytes(inputFilePaths.Length.ToString());
                ary_pdf_file.AddRange(pdf_write_binary);

                //ヘッダーを閉める。  
                ary_pdf_file.AddRange(pdf_2nd_obj_End);

                //配列を初期化  
                pdf_write_binary = new byte[] { };


                try
                {   //一旦、バイト型配列の内容をすべて上書き
                    fs.Write(ary_pdf_file.ToArray(), 0, ary_pdf_file.Count);
                }
                catch
                {
                };
                
                //配列を、一旦、クリアする
                ary_pdf_file.Clear();

                //◆各ページ用の、子オブジェクトの作成
                //ページ数だけ、子オブジェクトを指定・追加する
                for (int i = 0; i < inputFilePaths.Length; i += 1)
                {
                    //▼まずは、今回の、子オブジェクトの番号を指定
                    Int32 obj_No = (i + 1) * 3;
                    string pdf_obj_String = pdf_obj_Name.Replace("CC", obj_No.ToString());

                    //3オブジェクトの開始バイト位置を格納
                    ary_pdf_byte_head.Add(Convert.ToInt64(fs.Length));

                    //文字列を、バイト配列に変換して、バイナリに追加
                    ary_pdf_file.AddRange(Encoding.ASCII.GetBytes(pdf_obj_String));
                    
                    //▼キャンバス・サイズを指定
                    ary_pdf_file.AddRange(pdf_3rd_obj_Start);
                    
                    //★画像を読み込む
                    //※※※今回は、読み取りエラーは想定していないです。※※※
                    using (Bitmap bitmap = new Bitmap(inputFilePaths[i])) 
                    {
                        /* Bitmap Size変更 */


                        //各設定に、改める
                        pdf_obj_String = pdf_3rd_obj_MediaBoxSize;
                        pdf_obj_String = pdf_obj_String.Replace("WW", Convert.ToSingle(bitmap.Width * 0.75).ToString());
                        pdf_obj_String = pdf_obj_String.Replace("HH", Convert.ToSingle(bitmap.Height * 0.75).ToString());

                        //文字列を、バイト配列に変換して、バイナリに追加
                        ary_pdf_file.AddRange(Encoding.ASCII.GetBytes(pdf_obj_String));
                        
                        //▼画像の配置指定の設定
                        ary_pdf_file.AddRange(pdf_3rd_obj_Resources);
                        pdf_obj_String = pdf_3rd_obj_XObject;
                        pdf_obj_String = pdf_obj_String.Replace("XX", (obj_No + 1).ToString());

                        //文字列を、バイト配列に変換して、バイナリに追加
                        ary_pdf_file.AddRange(Encoding.ASCII.GetBytes(pdf_obj_String));
                        
                        //▼画像の配置指定の設定2
                        ary_pdf_file.AddRange(pdf_3rd_obj_Contents);
                        pdf_obj_String = pdf_indicate_obj.TrimEnd(); //右端のスペースは、消す
                        pdf_obj_String = pdf_obj_String.Replace("NO", (obj_No + 2).ToString());

                        //文字列を、バイト配列に変換して、バイナリに追加
                        ary_pdf_file.AddRange(Encoding.ASCII.GetBytes(pdf_obj_String));
                        ary_pdf_file.AddRange(pdf_3rd_obj_End);
                        
                        //▼画像配置の前のオブジェクト・バイナリ

                        //4オブジェクトの開始バイト位置を格納
                        ary_pdf_byte_head.Add(Convert.ToInt64(fs.Length + ary_pdf_file.Count));

                        pdf_obj_String = pdf_obj_Name.Replace("CC", (obj_No + 1).ToString());
                        ary_pdf_file.AddRange(Encoding.ASCII.GetBytes(pdf_obj_String));

                        ary_pdf_file.AddRange(pdf_4th_obj_Start);
                        
                        //▼画像本体のサイズ指定
                        pdf_obj_String = pdf_4th_obj_ImageSize;
                        pdf_obj_String = pdf_obj_String.Replace("AA", bitmap.Width.ToString());
                        pdf_obj_String = pdf_obj_String.Replace("BB", bitmap.Height.ToString());
                        ary_pdf_file.AddRange(Encoding.ASCII.GetBytes(pdf_obj_String));

                        ary_pdf_file.AddRange(pdf_4th_obj_Filter);

                        //▼今回処理する画像をメモリに格納
                        //メモリに保存
                        var baos = new MemoryStream();
                        bitmap.Save(baos, ImageFormat.Jpeg);
                        
                        //▼画像本体の、バイトサイズを指定
                        pdf_obj_String = pdf_4th_obj_Length;
                        pdf_obj_String = pdf_obj_String.Replace("LLLLL", baos.Length.ToString());
                        ary_pdf_file.AddRange(Encoding.ASCII.GetBytes(pdf_obj_String));


                        //▼以下、jpg画像のバイナリを、挿入-----------------------------------------
                        //画像を、バイト配列に変換したものを、渡す
                        //画像のバイナリ取得
                        ary_pdf_file.AddRange(baos.ToArray());


                        //streamを閉じる
                        ary_pdf_file.AddRange(pdf_4th_obj_End);


                        //▼画像配置の後のバイナリ

                        //5オブジェクトの開始バイト位置を格納
                        ary_pdf_byte_head.Add(Convert.ToInt64(fs.Length + ary_pdf_file.Count));

                        pdf_obj_String = pdf_obj_Name.Replace("CC", (obj_No + 2).ToString());
                        ary_pdf_file.AddRange(Encoding.ASCII.GetBytes(pdf_obj_String));


                        //▼画像の表示サイズの指定
                        pdf_obj_String = pdf_5th_obj_ShowSize;
                        pdf_obj_String = pdf_obj_String.Replace("WW", Convert.ToSingle(bitmap.Width * 0.75).ToString());
                        pdf_obj_String = pdf_obj_String.Replace("HH", Convert.ToSingle(bitmap.Height * 0.75).ToString());

                        //とりあえず、stream内部のバイト数を、数えるために、pdf_write_binaryに、移しておく
                        pdf_write_binary = Encoding.ASCII.GetBytes(pdf_obj_String);

                        //stream内部のバイト数を格納
                        ary_pdf_file.AddRange(Encoding.ASCII.GetBytes(
                                pdf_5th_obj_Length.Replace("LLLLL", pdf_write_binary.Length.ToString())));

                        //stream本体を格納
                        ary_pdf_file.AddRange(pdf_write_binary);
                        pdf_write_binary = new byte[] { }; //消去

                        //◆子オブジェクトを、閉める
                        ary_pdf_file.AddRange(pdf_5th_obj_End);


                        try
                        {   //一旦、バイト型配列の内容をすべて上書き
                            fs.Write(ary_pdf_file.ToArray(), 0, ary_pdf_file.Count);
                        }
                        catch
                        {

                        };
                        
                        //配列を、一旦、クリアする
                        ary_pdf_file.Clear();
                        baos.Close();
                    }
                }


                //▼フッター  
                //xrefの開始バイト位置を格納  
                long xref_start_pos = fs.Length;

                //フッターの開始。まずは、xrefから。
                ary_pdf_file.AddRange(pdf_xref_Start);

                //全オブジェクト数を格納  
                ary_pdf_file.AddRange(Encoding.ASCII.GetBytes(
                            pdf_xref_objCount.Replace("MM", (ary_pdf_byte_head.Count + 1).ToString())));

                //0位置指定。  
                ary_pdf_file.AddRange(pdf_xref_ZERO);
                
                //各・子オブジェクトのバイト位置を、 各10桁で指定していく  
                for (int i = 0; i <= ary_pdf_byte_head.Count - 1; i += 1)
                {
                    string fff = pdf_xref_objStartPos;
                    //開始位置のバイト数は、10桁表示→つまり、PDFの最大サイズは、10GB程度？  
                    fff = fff.Replace("QQQQQQQQQQ", Convert.ToUInt64(ary_pdf_byte_head[i]).ToString("0000000000"));
                    ary_pdf_file.AddRange(Encoding.ASCII.GetBytes(fff));
                }

                //trailerを格納 / Add trailer.  
                ary_pdf_file.AddRange(Encoding.ASCII.GetBytes(
                            pdf_trailer.Replace("MM", (ary_pdf_byte_head.Count + 1).ToString())));

                //startxref～%%EOFを格納  
                ary_pdf_file.AddRange(Encoding.ASCII.GetBytes(
                            pdf_startxref_EOF.Replace("TTT", xref_start_pos.ToString())));

                try
                {   //一旦、バイト型配列の内容をすべて上書き   
                    fs.Write(ary_pdf_file.ToArray(), 0, ary_pdf_file.Count);
                }
                catch
                {

                };
                
                //■後処理  
                fs.Close(); //ファイルを閉じる   
                fs = null;

                //バイナリ・メモリの占有を開放する。  
                ary_pdf_file.Clear();
                ary_pdf_file = null;

                ary_pdf_byte_head.Clear();
                ary_pdf_byte_head = null;
            }
            catch
            {
                result = false;
            }

            return result;
        }
        #endregion

        #region PDF ⇒ 画像
        /// <summary>
        /// PDFを複数画像に変換する.
        /// </summary>
        /// <param name="inputFilePath">画像に変換するPDFパス</param>
        /// <param name="outputDirectory">変換した画像の保存先</param>
        /// <returns>成否</returns>
        public static bool ConvertPDF2Image(string inputFilePath, string outputDirectory)
        {
            bool result = true;

            try
            {
                Image[] images = { };
                ConvertPDF2Image(inputFilePath, ref images);
                SaveImages(images, outputDirectory, inputFilePath);
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message); //Debug用
                result = false;
            }
            
            return result;
        }

        private static void ConvertPDF2Image(string inputFilePath, ref Image[] outputImages)
        {
            List<Image> tmpImages = new List<Image>();

            //変換処理
            using (FileStream fs = new FileStream(inputFilePath, FileMode.Open))
            {
                var image = Image.FromStream(fs);
                var guid = image.FrameDimensionsList[0];
                var frameDimension = new FrameDimension(guid);
                int pageCount = image.GetFrameCount(frameDimension);
                for (int i = 0; i < pageCount; i++)
                {
                    using (MemoryStream ms = new MemoryStream())
                    {
                        image.SelectActiveFrame(frameDimension, i);
                        tmpImages.Add(image);
                    }
                }
            }
            //結果を格納
            outputImages = tmpImages.ToArray();
        }

        private static void SaveImages(Image[] images, string saveDirectoryPath, string originalFilePath)
        {
            //元PDFファイル名を取得する
            string originalFileName = Path.GetFileNameWithoutExtension(originalFilePath);

            for (int i = 0; i < images.Length; i++)
            {
                //ファイル名を生成
                string fileName = originalFileName + $"_{i:000}" + ".png";
                string saveFilePath = saveDirectoryPath + fileName;
                images[i].Save(saveFilePath, ImageFormat.Png);
            }
        }
        #endregion
    }
}
