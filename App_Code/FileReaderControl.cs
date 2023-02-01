using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.ComponentModel;

/// <summary>
/// Summary description for FileReaderControl
/// </summary>
namespace CustomControls
{
    [ToolboxData("<{0}:FileReaderControl runat=server></{0}:FileReaderControl>")]
    public class FileReaderControl : LinkButton
    {
        private string _filePath;       
       
        [Category("Appearance")]
        public string FilePath
        {
            get { return _filePath; }
            set
            {
                _filePath = value;                
                _filePath = _filePath.Contains("\\\\") ? _filePath : _filePath.Replace("\\", "\\\\");
                this.Attributes["onclick"] = String.Format("javascript:ShowMyFile('{0}');return false;", _filePath);
            }
        }
       

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            #region File Reader Control Register Script 
            String JavaScript=@"
                function ShowMyFile(FilePath)
                {
                    var FileReaderIFrame;
                    var FileReaderForm;
                    var FilePathInput;
                    var IFrameContent;
                    var FilePathBox;
                    
                    //Check whether alraedy IFrame is created?
                    if(!document.getElementById('FileReaderIFrame'))
                    {      
                        //Create a IFrame
                        FileReaderIFrame=document.createElement('iframe');
                        //Define IFrame name and id
                        FileReaderIFrame.name=FileReaderIFrame.id='FileReaderIFrame';
                        //Define IFrame styles        
                        FileReaderIFrame.style.height=FileReaderIFrame.style.width=FileReaderIFrame.style.border='0px';       
                        //Add the frame to the page
                        document.body.appendChild( FileReaderIFrame);               
                    }
                    //Define what needs to be in the IFrame
                    IFrameContent = '<form id=FileReaderHandlerForm method=post enctype=application/data action=FileReaderHandler.ashx>';
                    IFrameContent += '<input type=hidden id=FilePath name=FilePath></input>';
                    IFrameContent += '</form>';
                 
                    //open the IFrame
                    FileReaderIFrame= window.frames['FileReaderIFrame'];
                    FileReaderIFrame.document.open();
                    //Add form and Hidden input tag
                    FileReaderIFrame.document.write(IFrameContent);
                    //Close the Iframe
                    FileReaderIFrame.document.close();
                    
                    FilePathBox=FileReaderIFrame.document.getElementById('FilePath');
                    if(FilePathBox)
                    {
                        FilePathBox.value=FilePath;              
                    }
                    FileReaderForm=FileReaderIFrame.document.getElementById('FileReaderHandlerForm');
                    if(FileReaderForm)
                    {
                        FileReaderForm.submit();
                    }
                    return false; 
                }
            ";
        #endregion

            Page.ClientScript.RegisterClientScriptBlock(GetType(), "FileReaderControlScript", JavaScript, true);
            this.CausesValidation = false;
        }

    }
}