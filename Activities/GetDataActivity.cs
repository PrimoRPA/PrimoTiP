using LTools.Common.Model;
using LTools.Common.UIElements;
using LTools.Common.WFItems;
using LTools.SDK;

using System;
using System.Collections.Generic;
using System.Windows;

using System.Resources;
using System.Net.Http;
using Newtonsoft.Json.Linq;

using Primo.TiP.Activities.Properties;
using Primo.TiP.Model;
using Newtonsoft.Json;
using LTools.Common.Helpers;

namespace Primo.TiP.Activities
{
    public class GetDataActivity : PrimoComponentSimple<Design.GetDataActivity_Form>
    {
        #region [GROUP NAME]

        public override string GroupName
        {
            get => "Train In Progress";
            protected set { }
        }

        #endregion


        #region [GLOBAL VARIABLE]

        private Design.GetDataActivity_Form cbase;
        const string _defaultUrl = "https://api.traininprogress.ai/api-fin/predict";

        #endregion


        #region [PROPERTIES]


        ///URL сервера TiP
        /// 
        private string _serverUrl = _defaultUrl;
        [LTools.Common.Model.Serialization.StoringProperty]
        [LTools.Common.Model.Studio.ValidateReturnScript(DataType = typeof(String))]
        [LocalizedCategory("GROUP_SERVER", typeof(Resources))]
        [LocalizedDisplayName("PROP_URL", typeof(Resources))]
        public string ServerUrl
        {
            get { return this._serverUrl; }
            set { this._serverUrl = value; this.InvokePropertyChanged(this, nameof(ServerUrl)); }
        }

        ///Токен к серверу TiP
        /// 
        private string _token;
        [LTools.Common.Model.Serialization.StoringProperty]
        [LTools.Common.Model.Studio.ValidateReturnScript(DataType = typeof(String))]
        [LocalizedCategory("GROUP_SERVER", typeof(Resources))]
        [LocalizedDisplayName("PROP_TOKEN", typeof(Resources))]
        public string Token
        {
            get { return this._token; }
            set { this._token = value; this.InvokePropertyChanged(this, nameof(Token)); }
        }

        ///Файл для распознавания
        /// 
        private string _file;
        [LTools.Common.Model.Serialization.StoringProperty]
        [LTools.Common.Model.Studio.ValidateReturnScript(DataType = typeof(String))]
        [LocalizedCategory("GROUP_INPUT", typeof(Resources))]
        [LocalizedDisplayName("PROP_FILE", typeof(Resources))]
        public string FileName
        {
            get { return this._file; }
            set { this._file = value; this.InvokePropertyChanged(this, nameof(FileName)); }
        }

        /// Out_Argument
        /// 
        private string _result;
        [LTools.Common.Model.Serialization.StoringProperty]
        [LTools.Common.Model.Studio.ValidateReturnScript(DataType = typeof(String))]
        [LocalizedCategory("GROUP_OUTPUT", typeof(LTools.Desktop.Properties.Strings))]
        [LocalizedDisplayName("PROP_RESULT", typeof(Resources))]

        public string ExtractionResult
        {
            get { return this._result; }
            set { this._result = value; this.InvokePropertyChanged(this, nameof(ExtractionResult)); }
        }

        /// Out_Argument: Type of document
        /// 
        private string _type;
        [LTools.Common.Model.Serialization.StoringProperty]
        [LTools.Common.Model.Studio.ValidateReturnScript(DataType = typeof(String))]
        [LocalizedCategory("GROUP_OUTPUT", typeof(LTools.Desktop.Properties.Strings))]
        [LocalizedDisplayName("PROP_TYPE", typeof(Resources))]

        public string DocType
        {
            get { return this._type; }
            set { this._type = value; this.InvokePropertyChanged(this, nameof(DocType)); }
        }

        /// Out_Argument: Document model
        /// 
        private string _model;
        [LTools.Common.Model.Serialization.StoringProperty]
        [LTools.Common.Model.Studio.ValidateReturnScript(DataType = typeof(Document))]
        [LocalizedCategory("GROUP_OUTPUT", typeof(LTools.Desktop.Properties.Strings))]
        [LocalizedDisplayName("PROP_MODEL", typeof(Resources))]
        public string DocumentModel
        {
            get { return this._model; }
            set { this._model = value; this.InvokePropertyChanged(this, nameof(DocumentModel)); }
        }

        #endregion

        private string trans(string text, System.Type type = null)
        {
            type = type ?? typeof(Resources);
            return new ResourceManager(type).GetString(text);
        }

        #region [PROPERTIES][TYPES]

        public GetDataActivity(IWFContainer container) : base(container)
        {

            //Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
            //System.Diagnostics.FileVersionInfo fvi = System.Diagnostics.FileVersionInfo.GetVersionInfo(assembly.Location);
            string version = "23.4";//fvi.FileVersion;

            sdkComponentIcon = "pack://application:,,/Primo.TiP.Activities;component/Images/tip_icon_small.png";

            sdkComponentName = trans("ACTIVITY_NAME");

            sdkComponentHelp = String.Format("{0} v{1}\r\n", trans("ACTIVITY_NAME"), version);
            sdkComponentHelp += trans("ACTIVITY_DESC");

            ///[PROPERTIES]
            ///
            sdkComponentHelp += "\r\n\r\n" + trans("PROPERTIES") + ":";


            ///[Server]
            sdkComponentHelp += "\r\n[" + trans("GROUP_SERVER") + "]";

            sdkComponentHelp += "\r\n"
                             + trans("PROP_URL") + "*: "
                             + trans("PROP_URL_DESC");
            sdkComponentHelp += "\r\n"
                             + trans("PROP_TOKEN") + "*: "
                             + trans("PROP_TOKEN_DESC");


            ///[Input]
            sdkComponentHelp += "\r\n[" + trans("GROUP_INPUT") + "]";

            sdkComponentHelp += "\r\n"
                             + trans("PROP_FILE") + "*: "
                             + trans("PROP_FILE_DESC");

            ///[Output]
            sdkComponentHelp += "\r\n[" + trans("GROUP_OUTPUT") + "]";

            sdkComponentHelp += "\r\n"
                             + trans("PROP_RESULT") + "*: "
                             + trans("PROP_RESULT_DESC");

            sdkComponentHelp += "\r\n"
                             + trans("PROP_MODEL") + "*: "
                             + trans("PROP_MODEL_DESC");

            sdkComponentHelp += "\r\n"
                             + trans("PROP_TYPE") + "*: "
                             + trans("PROP_TYPE_DESC");

            sdkProperties = new List<LTools.Common.Helpers.WFHelper.PropertiesItem>()
            {
                new LTools.Common.Helpers.WFHelper.PropertiesItem()
                {
                    PropName = nameof(ServerUrl),
                    PropertyType = LTools.Common.Helpers.WFHelper.PropertiesItem.PropertyTypes.SCRIPT,
                    EditorType = ScriptEditorTypes.NONE,
                    DataType = typeof(string),
                    ToolTip = trans("PROP_URL_DESC"),
                    IsReadOnly = false
                },

                new LTools.Common.Helpers.WFHelper.PropertiesItem()
                {
                    PropName = nameof(Token),
                    PropertyType = LTools.Common.Helpers.WFHelper.PropertiesItem.PropertyTypes.SCRIPT,
                    EditorType = ScriptEditorTypes.NONE,
                    DataType = typeof(string),
                    ToolTip = trans("PROP_TOKEN_DESC"),
                    IsReadOnly = false
                },

                new LTools.Common.Helpers.WFHelper.PropertiesItem()
                {
                    PropName = nameof(FileName),
                    PropertyType = LTools.Common.Helpers.WFHelper.PropertiesItem.PropertyTypes.SCRIPT,
                    EditorType = ScriptEditorTypes.FILE_SELECTOR,
                    DataType = typeof(string),
                    ToolTip = trans("PROP_FILE_DESC"),
                    IsReadOnly = false
                },

                new LTools.Common.Helpers.WFHelper.PropertiesItem()
                {
                    PropName = nameof(ExtractionResult),
                    PropertyType = LTools.Common.Helpers.WFHelper.PropertiesItem.PropertyTypes.VARIABLE,
                    EditorType = ScriptEditorTypes.NONE,
                    DataType = typeof(string),
                    ToolTip = trans("PROP_RESULT_DESC"),
                    IsReadOnly = false
                }
                ,           
                new LTools.Common.Helpers.WFHelper.PropertiesItem()
                {
                    PropName = nameof(DocType),
                    PropertyType = LTools.Common.Helpers.WFHelper.PropertiesItem.PropertyTypes.VARIABLE,
                    EditorType = ScriptEditorTypes.NONE,
                    DataType = typeof(string),
                    ToolTip = trans("PROP_TYPE_DESC"),
                    IsReadOnly = false
                }
                ,
                new LTools.Common.Helpers.WFHelper.PropertiesItem()
                {
                    PropName = nameof(DocumentModel),
                    PropertyType = LTools.Common.Helpers.WFHelper.PropertiesItem.PropertyTypes.SCRIPT,
                    EditorType = ScriptEditorTypes.NONE,
                    DataType = typeof(Document),
                    ToolTip = trans("PROP_MODEL_DESC"),
                    IsReadOnly = false
                }
            };

            Design.GetDataActivity_Form inputform = new Design.GetDataActivity_Form();
            this.cbase = inputform;
            this.cbase.DataContext = (object)this;
            WFElement wfElement = new WFElement((IWFElement)this, container);
            this.element = wfElement;
            this.element.Container = (FrameworkElement)this.cbase;

            InitClass(container);
            ServerUrl = this.IsNoCode("ServerUrl") ? _defaultUrl : "\"" + _defaultUrl + "\"";
        }

        #endregion


        #region [EXECUTION]

        private string GetResult(string url, string token, string fileName, ExecutionResult executionResult)
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Post, url);
            request.Headers.Add("Authorization", String.Format("Bearer {0}", token));
            var content = new MultipartFormDataContent();
            content.Add(new StreamContent(System.IO.File.OpenRead(fileName)), "file", fileName);
            request.Content = content;
            var response = client.SendAsync(request).Result;

            if (response.IsSuccessStatusCode)
            {
                executionResult.IsSuccess = true;
                executionResult.SuccessMessage = trans("ERR_OK");
                return (response.Content.ReadAsStringAsync()).Result;
            } else
            {
                executionResult.IsSuccess = false;
                executionResult.ErrorMessage = String.Format("API ERROR {0}: {1}", response.StatusCode, response.Content);
                return null;
            }
        }

        public override ExecutionResult SimpleAction(ScriptingData sd)
        {
            var executionResult = new ExecutionResult();
            executionResult.IsSuccess = true;
            executionResult.SuccessMessage = trans("ERR_OK");

            #region [VARIABLES][INIT]
            
            string prop_url = GetPropertyValue<String>(ServerUrl, nameof(ServerUrl), sd);
            string prop_token = GetPropertyValue<String>(Token, nameof(Token), sd);
            string prop_file = GetPropertyValue<String>(FileName, nameof(FileName), sd);

            #endregion

            #region [OUTPUT]

            var data = "";

            try
            {
                data = GetResult(prop_url, prop_token, prop_file, executionResult);
                var document = JsonConvert.DeserializeObject<List<Document>>(data);

                SetVariableValue<string>(this.ExtractionResult, data, sd);
                SetVariableValue<string>(this.DocType, document[0].Type.Value, sd);
                SetVariableValue<Document>(this.DocumentModel, document[0], sd);
                
            }
            catch (Exception ex)
            {
                executionResult.IsSuccess = false;
                executionResult.ErrorMessage = ex?.Message;
            }


            #endregion

            return executionResult;
        }
        #endregion


        #region [VALIDATE]
        public override ValidationResult Validate()
        {
            ValidationResult ret = new ValidationResult();
            if (this.ServerUrl is null) ret.Items.Add(new ValidationResult.ValidationItem() { PropertyName = nameof(ServerUrl), Error = trans("ERR_NO_SERVER") });
            if (this.Token is null) ret.Items.Add(new ValidationResult.ValidationItem() { PropertyName = nameof(Token), Error = trans("ERR_NO_TOKEN") });
            return ret;
        }
        #endregion


        #region [ADVANCED]
        #endregion
    }
}
