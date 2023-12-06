// See https://aka.ms/new-console-template for more information
using GrapeCity.Documents.Drawing;
using GrapeCity.Documents.Pdf;
using GrapeCity.Documents.Pdf.Graphics;
using GrapeCity.Documents.Text;

Console.WriteLine("フォームフィールドをフラット化");

#region フォームフィールドに値を設定
var doc = new GcPdfDocument();
var fc = new FontCollection();

// フォントを設定
fc.RegisterFont("Fonts\\ipaexg.ttf");
fc.RegisterFont("Fonts\\ipaexm.ttf");
doc.FontCollection = fc;

// PDFを読み込み
doc.Load(new FileStream("mescius_order_template.pdf", FileMode.Open, FileAccess.Read));

// PDFフォームへ入力するデータ
var kvp = new KeyValuePair<string, IList<string>>[]
{
                new KeyValuePair<string, IList<string>>("氏名", new string[] { "葡萄城　太郎" }),
                new KeyValuePair<string, IList<string>>("会社名", new string[] { "ディオドック株式会社" }),
                new KeyValuePair<string, IList<string>>("フリガナ", new string[] { "ブドウジョウ　タロウ" }),
                new KeyValuePair<string, IList<string>>("TEL", new string[] { "022-777-8888" }),
                new KeyValuePair<string, IList<string>>("部署名", new string[] { "ピノタージュ" }),
                new KeyValuePair<string, IList<string>>("住所", new string[] { "M県S市広瀬区花京院3-1-4" }),
                new KeyValuePair<string, IList<string>>("郵便番号", new string[] { "981-9999" }),
                new KeyValuePair<string, IList<string>>("Email", new string[] { "tarou.budojo@mescius.com" })
};

// KeyValuePairからデータを入力
doc.ImportFormDataFromCollection(kvp);

// PDFを保存
doc.Save("mescius_order_embed.pdf");
#endregion


#region PDFをフラット化
// フォームフィールドに値が設定済みのPDFを読み込み
doc.Load(new FileStream("mescius_order_embed.pdf", FileMode.Open, FileAccess.Read));

// 元となるPDFのすべてのページと注釈を新しいPDFに描画
var newdoc = new GcPdfDocument();

foreach (var srcPage in doc.Pages)
{
    var page = newdoc.Pages.Add(srcPage.Size);

    // 元のPDFのForm XObjectをソースにする
    var fxo = new FormXObject(newdoc, srcPage);

    // Form XObjectを描画
    page.Graphics.DrawForm(fxo, page.Bounds, null, ImageAlign.Default);

    // フォームフィールドを含むページ上のすべての注釈を描画
    srcPage.DrawAnnotations(page.Graphics, page.Bounds);
}

// PDFを保存
newdoc.Save("mescius_order_embed_flatten.pdf");
#endregion
