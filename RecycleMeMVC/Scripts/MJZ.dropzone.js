;(function(){
//MJZedit
String.prototype.toFaDigit = function(){
	return this.replace(/\d+/g, function(digit){
		var ret = '';
		for (var i = 0, len = digit.length; i < len; i++) 
			ret += String.fromCharCode(digit.charCodeAt(i) + 1728);
		return ret;
	});
};
String.prototype.toEnDigit = function() {
    return this.replace(/[\u06F0-\u06F9]+/g, function(digit) {
        var ret = '';
        for (var i = 0, len = digit.length; i < len; i++) {
            ret += String.fromCharCode(digit.charCodeAt(i) - 1728);
        }
 
        return ret;
    });
};
//ابتدا تابعی را تعریف کردیم در زیر سپس خواصی را بر آن افزوده ایم بعد از تعریف تابع
function require(path) 
{
	var module = require.modules[path];
	 
	module.call(null, module.exports, require , module);//و اهمیتی ندارد undefined به پنجره اشاره می کند که می توان جای آن قرار داد  this اشاره گر 
	
	return module.exports;
}
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
require.modules = {};
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
require.register = function(path, definition) 
{
  	require.modules[path] = definition;
};
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
require.register("func1", function(exports, require, module)
{
  (function() 
  {
			  var Dropzone, Em, camelize, contentLoaded, noop, without,
			  	  __hasProp = {}.hasOwnProperty, //یک نام مستعار
				  
				  __extends = function(child, parent) 
									{ 
											for (var key in parent) 
											{ 
												if (__hasProp.call(parent, key)) 
														child[key] = parent[key]; 
											} 
											function ctor() 
											{ 
												this.constructor = child; 
											} 
											ctor.prototype = parent.prototype; 
											child.prototype = new ctor(); 
											child.__super__ = parent.prototype; 
											return child; 
									},
				__slice = [].slice, //یک نام مستعار
				
				//If the browser doesn't support indexOf for Array type
				__indexOf = [].indexOf || function(item) 
											{ 
													for (var i = 0, l = this.length; i < l; i++) 
													{ 
														if (i in this && this[i] === item) 
															return i; 
													} 
												return -1; 
											};
			  //end of Var definition section
			  Em = require("func2"); //تابع مدیریت کننده رویدادها که تعریف های کاربر را به سیستم می افزاید
			  
			  noop = function() {}; //یک نام مستعار برای توابع تهی
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
			  Dropzone = (function(_super) 
			  {
				  
					__extends(Dropzone, _super);
				
					Dropzone.prototype.events = ["drop", "dragstart", "dragend", "dragenter", "dragover", "dragleave", "selectedfiles", "addedfile", "imageResized", "removedfile", "thumbnail", "error", "processingfile", "uploadprogress", "sending", "success", "complete", "dblclick", "reset"];
				
					Dropzone.prototype.defaultOptions = 
					{
							  urlUploader: null,//MJZedit
							  urlDownloader: null,//MJZedit //this file must cause to download images and prevent browser to show image, if you don't set it it will show the image in the window
							  urlDeleter: null,//MJZedit delete a single file
							  urlFileInfo: null,//MJZEdit //URL for a file to get info about an uploaded file and show the Icon for that in initialization time
							  method: "post",
							  parallelUploads: 2,
							  maxFilesize: 256,
							  paramName: "file",
							  createImageThumbnails: true,
							  maxThumbnailFilesize: 2,
							  thumbnailWidth: 100,
							  thumbnailHeight: 100,
							  params: {}, //پارامترهای اضافه که می توان هنگام آپلود همراه فایل ارسال کرد
							  clickable: true,
							  enqueueForUpload: true,
							  previewsContainer: null,
							  dictDefaultMessage: "Drop files here to upload",
							  dictFallbackMessage: "Your browser does not support drag'n'drop file uploads.",
							  dictFallbackText: "Please use the fallback form below to upload your files like in the olden days.",
							  //MJZedit start
							  dictUnderProcessing: "Deletion is not possible at this time, because this file is under processing. Try again later.",
							  dictDeleteMessage: "Drag the thumbnail icon and drop out of form to delete.",
							  dictDeleteFailedFilesMessage: "Click on the cross icon to delete this file from the form.",
							  dictDownloadSuccessFilesMessage: "Click on the icon to download this file from the server.",
							  dictDeleteSuccessFilesMessage: "To delete this file from the server drag it and drop out of the form.",
							  informUnsuccessDeletation: "Deletion of this file from the server have been failed.",
							  UnableToDelete: "Deletion of file from the server is not allowed.",
							  UnsuccessDeleteAllFiles: "Deletion of one or more files have filed.",
							  dictDeleteAllFilesButton: "Click me to delete all files.",
							  dictresizeChBoxText: "Auto resize image files:",
							  dictNewWidthText: " Maximum new width (px):",
							  dictNewHeightText: " Maximum new height (px):",
							  terabyte: "TB",
							  gigabyte: "GB",
							  megabyte: "MB",
							  kilobyte: "KB",
							  byte: "b",
							  hugefileMsgPart1: "File is too big (", 
							  hugefileMsgPart2: "MB). Max filesize: ", 
							  hugefileMsgPart3: "MB",
							  useArabicNumbers: false, //it just usable for Arabic and Persian languages that are right to left
							  fileExtensions: "*.*",
							  icons: "ac3,ace,ade,adp,ai,aiff,au,avi,bat,bin,bmp,bup,cab,cat,chm,com,css,cue,dat,dcr,der,dic,divx,diz,dll,doc,docx,dvd,dwg,dwt,emf,exc,exe,fon,gif,hlp,htm,html,ifo,inf,ini,ins,ip,iso,isp,java,jfif,jpeg,jpg,log,m4a,mid,mmf,mmm,mov,movie,mp2,mp2v,mp3,mp4,mpe,mpeg,mpg,mpv2,nfo,pdd,pdf,php,png,ppt,pptx,psd,rar,reg,rtf,scp,theme,tif,tiff,tlb,ttf,txt,uis,url,vbs,vcr,vob,wav,wba,wma,wmv,wpl,wri,wtx,xls,xlsx,xml,xsl,zap,zip",
							  showTopPanel: true,
							  createResizedImage: true,
      						  resizedWidth: 400,
      						  resizedHeight: 300,
							  
							  //MJZedit end
							  //تابعی برای رد کردن فایل های غیر مجاز که باید بازنویسی شود در هنگام فراخوانی
							  accept: function(file, done) {
								return done();
							  },
							  
							  
							  init: function() {
								return noop;
							  },
							  //////////////////////این تابع همان فرم قدیمی را جایگزین می کند dag'n'drop تابعی که در صورت پشتیبانی نکردن مرورگر از   
							  fallback: function() 
							  {
									var child, messageElement, span, _i, _len, _ref;
									
									this.element.className = "" + this.element.className + " browser-not-supported";
									
									_ref = this.element.getElementsByTagName("div");
									
									for (_i = 0, _len = _ref.length; _i < _len; _i++) 
									{
										  child = _ref[_i];
										  if (/(^| )message($| )/.test(child.className)) 
										  {
											messageElement = child;
											child.className = "message";
											continue;
										  }
									}
									
									if (!messageElement) 
									{
									  messageElement = Dropzone.createElement("<div class=\"message\"><span></span></div>");
									  this.element.appendChild(messageElement);
									}
									
									span = messageElement.getElementsByTagName("span")[0];
									
									if (span) 
									{
									  span.textContent = this.options.dictFallbackMessage;
									}
									
									return this.element.appendChild(this.getFallbackForm());
							  },
							 
							  /*
							  Those functions register themselves to the events on init and handle all
							  the user interface specific stuff. Overwriting them won't break the upload
							  but can break the way it's displayed.
							  You can overwrite them if you don't like the default behavior. If you just
							  want to add an additional event handler, register it on the dropzone object
							  and don't overwrite those options.
							  */
							  
							  //توابع زیر در حقیقت نوع شیوه نامه را در حالات مختلف دراگ اند دراپ تغییر می دهد
							  drop: function(e) {
								return this.element.classList.remove("drag-hover");
							  },
							  //MJZEdit start
							  dragstart: function(e) {
								this.DeleteState = false;//MJZEDit
							  },
							  //MJZEdit End
							  dragend: function(e) {
								  //MJZEDit Start
								if(this.DeleteState)
								{
									this.DeleteFile(e.target.id);//pass the third parameter true to delete the file from the server
								}
								//MJZEDit End
								return this.element.classList.remove("drag-hover");
							  },
							  
							  dragenter: function(e) {
								this.DeleteState = false;//MJZEDit
								return this.element.classList.add("drag-hover");
							  },
							  
							  dragover: function(e) {
								this.DeleteState = false;//MJZEDit
								return this.element.classList.add("drag-hover");
							  },
							  
							  dragleave: function(e) {
								this.DeleteState = true;//MJZEDit
								return this.element.classList.remove("drag-hover");
							  },
							  
							  selectedfiles: function(files) {
								if (this.element === this.previewsContainer) {
								  return this.element.classList.add("started");
								}
							  },
							  
							  reset: function() {
								return this.element.classList.remove("started");
							  },
							  
							  addedfile: function(file) {
								file.previewTemplate = Dropzone.createElement(this.options.previewTemplate);
								this.previewsContainer.appendChild(file.previewTemplate);
								file.previewTemplate.querySelector(".filename span").textContent = file.name;
								return file.previewTemplate.querySelector(".details").appendChild(Dropzone.createElement("<div class=\"footer\"><span class=\"size\">" + (this.filesize(file.size)) + "<span><input type=\"checkbox\" class=\"chBox\" checked=\"checked\" name=\"product_images\" value=\""+file.name+"\"></div>"));
							  },
							  
							  removedfile: function(file) {
								return file.previewTemplate.parentNode.removeChild(file.previewTemplate);
							  },
							  
							  thumbnail: function(file, dataUrl) 
							  {
								  	file.previewTemplate.classList.remove("file-preview");
									file.previewTemplate.classList.add("image-preview");
									//MJZedit for adding icons for non image files
									if(dataUrl!=null)
									{
																																																	//this id is needed for delete and this class is needed for colorbox																											
										return file.previewTemplate.querySelector(".imagecontainer").appendChild(Dropzone.createElement("<img alt=\"" + file.name + "\" src=\"" + dataUrl + "\" id=\""+file.id+"\" class=\"ImagesOf"+this.element.id+"\" href=\""+file.serverPath+"\"/>"));//MJZedit
										$(".ImagesOf"+"myDivDropzone").colorbox({rel:".ImagesOf"+"myDivDropzone", transition:"fade", width:"800px", height:"600px"});
									}
									//extracting file extension
									var re = /(?:\.([^.]+))?$/;
									var ext = re.exec(file.name)[1];
									var url_icon;
									
									if(ext!=undefined)
									{
										ext = ext.toLowerCase();
										if(this.defaultOptions.icons.indexOf(ext) !== -1)
											url_icon= './images/icons/'+ext+'.png';
										else
											url_icon= './images/icons/unknown.png';	
									}
									else
										url_icon= './images/icons/unknown.png';
										
									return file.previewTemplate.querySelector(".imagecontainer").appendChild(Dropzone.createElement("<img alt=\"" + file.name + "\" src=\"" + url_icon + "\" class=\"icon\" id=\""+file.id+"\" />"));//MJZedit
									//end of mjzedit section
							  },
							  //MJZedit
      						  imageResized: function(file, dataUrl) {
									//console.log('resized');
						      },
							  dblclick: function(evt) {
								//if(typeof(evt.target)=='object' && evt.target.nodeName=='IMG')
								//	console.log('dbclicked: '+evt.target.href);
						      },
   							  //end MJZedit modified section
 							  error: function(file, message) 
							  {
									file.previewTemplate.classList.add("error");
									return file.previewTemplate.querySelector(".error-message span").textContent = message;
							  },
							  
							  processingfile: function(file) {
								return file.previewTemplate.classList.add("processing");
							  },
							  
							  uploadprogress: function(file, progress) {
								  //در حقیقت هر بار عرض ناحیه سبز رنگ را افزایش می دهد
								return file.previewTemplate.querySelector(".progress .upload").style.width = "" + progress + "%";
							  },
							  
							  sending: noop,
							  
							  success: function(file) {
								  
								return file.previewTemplate.classList.add("success");
								
							  },
							  
							  complete: noop,
							  //MJZedit																																																																																											
							  topPanelTemplate: "<span class=\"triangle-topleft-shadow\"></span><span class=\"triangle-topleft\"></span><span class=\"topPanel\"><span id=\"DeleteAllFilesButton\" title=\"\"></span><div class=\"linebreak\">&nbsp;</div>&nbsp;<span class=\"WLabel\" id=\"resizeChBoxText\">&nbsp;</span><input type=\"checkbox\" id=\"ResizeActionState\" checked=\"true\" />&nbsp;<span class=\"WLabel\" id=\"NewWidthText\">&nbsp;</span><input type=\"text\" id=\"NewWidth\" class=\"numberInput\" maxlength=\"4\" value=\"800\" /><span class=\"WLabel\" id=\"NewHeightText\">&nbsp;</span><input type=\"text\" id=\"NewHeight\" class=\"numberInput\" maxlength=\"4\" value=\"600\" /></span><span class=\"triangle-topright\"></span><span class=\"triangle-topright-shadow\"></span><br />",
							  
							  previewTemplate: "<div class=\"preview file-preview\">\n  <div class=\"details\">\n   <div class=\"imagecontainer\"></div>\n  <div class=\"filename\"><span></span></div>\n  </div>\n  <div class=\"progress\"><span class=\"upload\"></span></div>\n  <a href=\"#\" class=\"img_download_link\" target=\"_blank\"><div class=\"success-mark\"><span>✔</span></div></a>\n  <div class=\"error-mark\" ><span>✘</span></div>\n  <div class=\"error-message\"><span></span></div>\n</div>" //MJZedit
							  //I changed the template
							  
				};//End of defult options section
			
				function Dropzone(element, options) 
				{
					
					  var elementId, elementOptions, extend, fallback, _ref;
				 	  
					  
					  this.DeleteState = false; //MJZedit
					  
					  this.element = element;
					  
					  this.version = Dropzone.version;
					  
					  this.defaultOptions.previewTemplate = this.defaultOptions.previewTemplate.replace(/\n*/g, "");
					  
					  if (typeof this.element === "string") 
					  {
						this.element = document.querySelector(this.element);
					  }
					  
					  if (!(this.element && (this.element.nodeType != null))) 
					  {
						throw new Error("Invalid dropzone element.");
					  }
					  
					  if (Dropzone.forElement(this.element)) 
					  {
						throw new Error("Dropzone already attached.");
						//console.log("Dropzone already attached.");
					  }
					  
					  Dropzone.instances.push(this);//دراپ زون ساخته شده را در نمونه ها قرار می دهد
					  
					  elementId = this.element.id;
					  
					  elementOptions = (_ref = (elementId ? Dropzone.options[camelize(elementId)] : void 0)) != null ? _ref : {};
					  
					  extend = function() 
					  {
							var key, object, objects, target, val, _i, _len;
					
							target = arguments[0], objects = 2 <= arguments.length ? __slice.call(arguments, 1) : [];
							for (_i = 0, _len = objects.length; _i < _len; _i++) 
							{
							  object = objects[_i];
							  for (key in object) 
							  {
								val = object[key];
								target[key] = val;
							  }
							}
							return target;
				  	  };
				  
					  this.options = extend({}, this.defaultOptions, elementOptions, options != null ? options : {});

					  if (this.options.urlUploader == null) 
					  {
						this.options.urlUploader = this.element.action;
					  }
					  
					  if (!this.options.urlUploader) 
					  {
						throw new Error("No URL provided.");
						//console.log(this.options.urlUploader);
					  }
					  
					  this.options.method = this.options.method.toUpperCase();
					  
					  if (!Dropzone.isBrowserSupported()) //اگر مرورگر پشتیبانی نکند تابع فالبک حالت قدیمی را آپلود می کند
					  {
						return this.options.fallback.call(this);
					  }
					  
					  if ((fallback = this.getExistingFallback()) && fallback.parentNode) 
					  {
						fallback.parentNode.removeChild(fallback);
					  }
					  
					  if (this.options.previewsContainer) 
					  {
							if (typeof this.options.previewsContainer === "string") 
							{
							  this.previewsContainer = document.querySelector(this.options.previewsContainer);
							} 
							else if (this.options.previewsContainer.nodeType != null) 
							{
							  this.previewsContainer = this.options.previewsContainer;
							}
							if (this.previewsContainer == null) 
							{
							  throw new Error("Invalid `previewsContainer` option provided. Please provide a CSS selector or a plain HTML element.");
							}
					  } 
					  else 
					  {
						this.previewsContainer = this.element;
					  }
					  
					  this.init();
				}
				////////////////////////////////////////////////////////////////////////////////////////////////////////////////
				Dropzone.prototype.init = function() 
				{
					
					  var eventName, noPropagation, setupHiddenFileInput, _i, _len, _ref, _ref1,_this = this;
					  if (this.element.tagName === "form") 
					  {
						this.element.setAttribute("enctype", "multipart/form-data");
					  }
					  //MJZEdit start /*code for show top panel*/
					  if (this.options.showTopPanel)
					  {
						  var _this=this;
						  this.element.innerHTML=this.options.topPanelTemplate;
						  this.element.querySelector('#DeleteAllFilesButton').setAttribute('title', this.options.dictDeleteAllFilesButton);
						  this.element.querySelector('#resizeChBoxText').innerHTML = this.options.dictresizeChBoxText;
						  this.element.querySelector('#NewWidthText').innerHTML = this.options.dictNewWidthText;
						  this.element.querySelector('#NewHeightText').innerHTML = this.options.dictNewHeightText;
						  this.element.querySelector('#NewWidth').value= this.options.useArabicNumbers ? (this.options.resizedWidth+"").toFaDigit() : this.options.resizedWidth;
						  this.element.querySelector('#NewHeight').value= this.options.useArabicNumbers ? (this.options.resizedHeight+"").toFaDigit() : this.options.resizedHeight;
						  
						  if(!this.options.createResizedImage)
						  {
							  this.element.querySelector('#ResizeActionState').checked=this.options.createResizedImage;
							  this.element.querySelector('#NewWidth').setAttribute('disabled', true);
							  this.element.querySelector('#NewHeight').setAttribute('disabled', true);
						  }
						  
						  function fixTopPanelSize(element)
						  {
							  element.querySelector('span.triangle-topleft').style.right=(element.querySelector('span.triangle-topleft-shadow').parentNode.offsetWidth*0.02+4)+'px';
							  element.querySelector('span.triangle-topright').style.left=(element.querySelector('span.triangle-topleft-shadow').parentNode.offsetWidth*0.02+4)+'px';
							  element.querySelector('span.topPanel').style.left= (element.querySelector('span.triangle-topleft-shadow').parentNode.offsetWidth*0.02+39)+'px';
							  element.querySelector('span.topPanel').style.right= (element.querySelector('span.triangle-topleft-shadow').parentNode.offsetWidth*0.02+39)+'px';
						  };
						  
						  this.element.querySelector('#DeleteAllFilesButton').onmouseup=function()
						  {
								_this.DeleteAllFiles();
						  }
						
						  document.addEventListener('DOMContentLoaded', fixTopPanelSize(_this.element), false);
						  
						  window.onresize=function(){fixTopPanelSize(_this.element)};
						  
						  this.element.querySelector("#ResizeActionState").onchange=function()
						  {
							  if(!this.checked)
								  {
									  _this.element.querySelector('#NewWidth').setAttribute('disabled', true);
									  _this.element.querySelector('#NewHeight').setAttribute('disabled', true);
									  _this.setResizeOptions(false);
								  }
							  else
								  {
									  _this.element.querySelector('#NewWidth').removeAttribute('disabled');
									  _this.element.querySelector('#NewHeight').removeAttribute('disabled');
									  _this.setResizeOptions(true,_this.element.querySelector("#NewWidth").value,_this.element.querySelector("#NewHeight").value);
								  }
						  };
						  
						  _this.element.querySelector("#NewWidth").onkeyup=function()
							{
								if(_this.element.querySelector("#NewWidth").value.length>0)
									_this.setResizeOptions(true, _this.element.querySelector("#NewWidth").value, _this.element.querySelector("#NewHeight").value);
							};
							
						  _this.element.querySelector("#NewWidth").onblur=function()
							{
								if(_this.element.querySelector("#NewWidth").value.length==0 || parseInt(_this.element.querySelector("#NewWidth").value.toEnDigit(),10)==0)
									if(_this.options.useArabicNumbers)
										_this.element.querySelector("#NewWidth").value=(_this.options.resizedWidth+"").toFaDigit();
									else
										_this.element.querySelector("#NewWidth").value=_this.options.resizedWidth;
							};
							//
						  _this.element.querySelector("#NewHeight").onkeyup=function()
							{
								if(_this.element.querySelector("#NewHeight").value.length>0)
									_this.setResizeOptions(true, _this.element.querySelector("#NewWidth").value, _this.element.querySelector("#NewHeight").value);
							};
							
						  _this.element.querySelector("#NewHeight").onblur=function()
							{
								if(_this.element.querySelector("#NewHeight").value.length==0 || parseInt(_this.element.querySelector("#NewHeight").value.toEnDigit(),10)==0)
									if(_this.options.useArabicNumbers)
										_this.element.querySelector("#NewHeight").value=(_this.options.resizedHeight+"").toFaDigit();
									else
										_this.element.querySelector("#NewHeight").value=_this.options.resizedHeight;
							};
							
							//must be unbind at first to prevent from running mutiple of time visit the link for more information: http://stackoverflow.com/questions/7987454/input-fires-keypress-event-twice
							$('.numberInput').unbind('keypress').bind('keypress',function(e) //put cursor at right position
								{
									   if(this.maxLength>this.value.length)
										{
										   var char = String.fromCharCode(e.which || e.keyCode);
										   var keycode = (e.which || e.keyCode);
										   if(_this.options.useArabicNumbers)
											 {	   
												if (char >= '0' && char <='9' ) 
												{
													char=char.charCodeAt(0)+1728;
													var start=$(this).caret().start;
													var end=$(this).caret().end;
													var str=this.value;
													var result =str.slice(0,start) + String.fromCharCode(char) + str.slice(end);
													this.value = result;
													start++;
													this.setSelectionRange(start, start);//بازگرداندن کرسر به مکان قبلی خود
													e.preventDefault();
													e.stopPropagation();
													e.cancelBubble = true;
													return false;
												}
												else if(keycode!=8 && keycode != 46 && keycode!=37 && keycode != 39)
												{
													e.preventDefault();
													e.stopPropagation();
													e.cancelBubble = true;
													return false;
												}
										   }
										   else if ((char < '0' || char >'9') && (keycode!=8 && keycode != 46 && keycode!=37 && keycode != 39))
										   { 
												e.cancelBubble = true;
												e.stopPropagation();
												e.preventDefault();
												return false;
														
										   }
										}
								});	
						  
					  }
					  //End MJZEdit
					  //افزودن عکسی که در اول کار راهنمایی می کند که درگ اندراپ کنید
					  //if (this.element.classList.contains("dropzone") && !this.element.querySelector(".message")) 
					  if (!this.element.querySelector(".message"))//MJZEdit deleted first part to show message in ajax pages 
					  {
						this.element.appendChild(Dropzone.createElement("<div class=\"default message\"><span>" + this.options.dictDefaultMessage + "</span></div>"));
					  }
					  //افزودن یک فیلد مخفی که هنگام کلیک ظاهر می شود
					  if (this.options.clickable) 
					  {
							setupHiddenFileInput = function() 
							{
								  if (_this.hiddenFileInput) 
								  {
									document.body.removeChild(_this.hiddenFileInput);
								  }
								  _this.hiddenFileInput = document.createElement("input");
								  _this.hiddenFileInput.setAttribute("type", "file");
								  _this.hiddenFileInput.setAttribute("multiple", "multiple");
								  _this.hiddenFileInput.setAttribute("accept", _this.options.fileExtensions);//MJZ edit
								  _this.hiddenFileInput.style.display = "none";
								  document.body.appendChild(_this.hiddenFileInput);
									  return _this.hiddenFileInput.addEventListener("change", function(){ 
											var files;
								
											files = _this.hiddenFileInput.files;
											
											if (files.length) 
											{
											  _this.emit("selectedfiles", files);//تعداد فایل های انتخاب شده در فایل بروزر را در خود دارد
											  _this.handleFiles(files);
											}
											return setupHiddenFileInput();
									  });
							};
							setupHiddenFileInput();
					  }
					  
					  
					  
					  this.file_id= 1; //MjzEdit //it used for some actions the same as deletion because we need a unique id for each file
					  
					  this.files = [];
					  this.filesQueue = [];
					  this.filesProcessing = [];
					  this.URL = (_ref = window.URL) != null ? _ref : window.webkitURL;
					  _ref1 = this.events;
					  for (_i = 0, _len = _ref1.length; _i < _len; _i++) 
					  {
						eventName = _ref1[_i];
						this.on(eventName, this.options[eventName]);
					  }
					  noPropagation = function(e) 
					  {
							e.stopPropagation();
							if (e.preventDefault) 
							{
							  return e.preventDefault();
							} 
							else 
							{
							  return e.returnValue = false;
							}
					  };
					  
					  this.listeners = {
						"dragstart": function(e) {
						  return _this.emit("dragstart", e);
						},
						"dragenter": function(e) {
						  noPropagation(e);
						  return _this.emit("dragenter", e);
						},
						"dragover": function(e) {
						  noPropagation(e);
						  return _this.emit("dragover", e);
						},
						"dragleave": function(e) {
						  return _this.emit("dragleave", e);
						},
						"drop": function(e) {
						  noPropagation(e);
						  _this.drop(e);
						  return _this.emit("drop", e);
						},
						"dragend": function(e) {
						  return _this.emit("dragend", e);
						},
						"click": function(evt) 
						{
						  if (!_this.options.clickable) {
							return;
						  }
						  if (evt.target === _this.element || Dropzone.elementInside(evt.target, _this.element.querySelector(".message"))) {
							return _this.hiddenFileInput.click();
						  }
						},
						
						"dblclick": function(evt) {
						  return _this.emit("dblclick", evt);
						}
						
					  };
					  this.enable();
					  
					  return this.options.init.call(this);
				};
				///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
				//تولید فرم ارسال برای مرورگرهای قدیمی
				Dropzone.prototype.getFallbackForm = function() 
				{
					  var existingFallback, fields, fieldsString, form;
				
					  if (existingFallback = this.getExistingFallback()) {
						return existingFallback;
					  }
					  fieldsString = "<div class=\"fallback\">";
					  if (this.options.dictFallbackText) {
						fieldsString += "<p>" + this.options.dictFallbackText + "</p>";
					  }
					  fieldsString += "<input type=\"file\" name=\"" + this.options.paramName + "\" multiple=\"multiple\" accept=\""+this.options.fileExtensions+"\" /><button type=\"submit\">Upload!</button></div>";
					  fields = Dropzone.createElement(fieldsString);
					  if (this.element.tagName !== "FORM") {
						form = Dropzone.createElement("<form action=\"" + this.options.urlUploader + "\" enctype=\"multipart/form-data\" method=\"" + this.options.method + "\"></form>");
						form.appendChild(fields);
					  } else {
						this.element.setAttribute("enctype", "multipart/form-data");
						this.element.setAttribute("method", this.options.method);
					  }
					  return form != null ? form : fields;
				};
			
				Dropzone.prototype.getExistingFallback = function() 
				{
					  var fallback, getFallback, tagName, _i, _len, _ref;
				
					  getFallback = function(elements) {
							var el, _i, _len;
					
							for (_i = 0, _len = elements.length; _i < _len; _i++) {
							  el = elements[_i];
							  if (/(^| )fallback($| )/.test(el.className)) {
								return el;
							  }
							}
					  };
					  _ref = ["div", "form"];
					  for (_i = 0, _len = _ref.length; _i < _len; _i++) {
							tagName = _ref[_i];
							if (fallback = getFallback(this.element.getElementsByTagName("div"))) {
							  return fallback;
							}
					  }
				};
			
				Dropzone.prototype.setupEventListeners = function() 
				{
					  var event, listener, _ref, _results;
				
					  _ref = this.listeners;
					  _results = [];
					  for (event in _ref) {
						listener = _ref[event];
						_results.push(this.element.addEventListener(event, listener, false));
					  }
					  return _results;
				};
			
				Dropzone.prototype.removeEventListeners = function() 
				{
					  var event, listener, _ref, _results;
				
					  _ref = this.listeners;
					  _results = [];
					  for (event in _ref) {
						listener = _ref[event];
						_results.push(this.element.removeEventListener(event, listener, false));
					  }
					  return _results;
				};
			
				Dropzone.prototype.disable = function() 
				{
					  if (this.options.clickable) {
						this.element.classList.remove("clickable");
					  }
					  this.removeEventListeners();
					  this.filesProcessing = [];
					  return this.filesQueue = [];
				};
			
				Dropzone.prototype.enable = function() 
				{
					  if (this.options.clickable) {
						this.element.classList.add("clickable");
					  }
					  return this.setupEventListeners();
				};
				
				
				Dropzone.prototype.filesize = function(size) 
				{
					  var string;
					  if (size >= 100000000000) {
						size = size / 100000000000;
						string = this.options.terabyte;
					  } else if (size >= 100000000) {
						size = size / 100000000;
						string = this.options.gigabyte;
					  } else if (size >= 100000) {
						size = size / 100000;
						string = this.options.megabyte;
					  } else if (size >= 100) {
						size = size / 100;
						string = this.options.kilobyte;
					  } else {
						size = size * 10;
						string = this.options.byte;
					  }
					  //MJZ edit
					  var temp=(Math.round(size) / 10);
					  
					  temp = temp + " ";//change temp to string
					  if(this.options.useArabicNumbers)
					  	temp = temp.toFaDigit();
					  //end modified section
					  return "<strong>" + temp + "</strong>" + string;
				};
			
				Dropzone.prototype.drop = function(e) 
				{
					  var files;
				
					  if (!e.dataTransfer) {
						return;
					  }
					  
					  files = e.dataTransfer.files;//ذخیره تمام فایل ها در یک آرایه
					  
					  this.emit("selectedfiles", files);
					 
					  if (files.length) {
						return this.handleFiles(files);
					  }
				};
			
				Dropzone.prototype.handleFiles = function(files) 
				{
					  var file, _i, _len, _results;
				
					  _results = [];
					  for (_i = 0, _len = files.length; _i < _len; _i++) 
					  {
							file = files[_i];
							_results.push(this.addFile(file));
					  }
					  return _results;
				};
			///////////////////////////////////////////////////////////////////////////////////////////////////////////////تابعی که حجم فایل ارسالی را می سنجد
				Dropzone.prototype.accept = function(file, done) 
				{
					  if (file.size > this.options.maxFilesize * 1024 * 1024) 
					  {
						
							//MJZedit
							var str1= " ", str2=" ";
							str1+=(Math.round(file.size / 1024 / 10.24) / 100);
							str2+=this.options.maxFilesize;
							if(this.options.useArabicNumbers)
							{
								str1=str1.toFaDigit();
								str2=str2.toFaDigit();
							}
							return done( this.options.hugefileMsgPart1 + str1 + this.options.hugefileMsgPart2 + str2 + this.options.hugefileMsgPart3);
					  		//End mjzedit
					  } 
					  else {
						return this.options.accept.call(this, file, done);
					  }
				};
			///////////////////////////////////////////////////////////////////////////////////////////////////////////////تابع افزودن فایل به صفحه
				Dropzone.prototype.addFile = function(file) 
				{
					
					  var _this = this;
					   
					  this.files.push(file);
					  
					  //file's ID is a combination of index of file in the Files array and a counter
					  file.id="file_"+__indexOf.call(this.files, file)+"_"+this.file_id++; //MJZedit it will make a compelete unique id for each file even in paralel execution
					  
					  this.emit("addedfile", file);//قالب فایل روی فرم با صدا زدن این تابع ساخته می شود همچنین نام فایل و غیره نیز افزوده می گردد
					  
					  if (this.options.createImageThumbnails && file.type.match(/image.*/) && !file.type.match(/image\/tiff/) && file.size <= this.options.maxThumbnailFilesize * 1024 * 1024) 
					  {
							this.createThumbnail(file);
					  }
					  //MJZedit //create icon for non image files
					  else
					  {
						  	this.emit("thumbnail", file, null); //pass null as third parameter tell to function to add a defualt icon for the non image files.
					  }
					  //end mjzedit
					  return this.accept(file, function(error) 
					  {
							if (error) 
							{
							  return _this.errorProcessing(file, error);
							} 
							else 
							{
								  if (_this.options.enqueueForUpload) 
								  {
									_this.filesQueue.push(file);
									return _this.processQueue();
								  }
							}
					  });
				};
			///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
				Dropzone.prototype.addServerFile = function(url) 
				{
					var _this=this;
					
					function getFileInfoFromServer(_url, doneCallback) 
					{
						
						  var xhr = new XMLHttpRequest();
						  xhr.open("POST", _this.options.urlFileInfo, true);
						  xhr.setRequestHeader("Accept", "application/json");
					  	  xhr.setRequestHeader("Cache-Control", "no-cache");
					      xhr.setRequestHeader("X-Requested-With", "XMLHttpRequest");
					      formData = new FormData();
					  	  formData.append('url', _url);
						 xhr.onreadystatechange = function() 
						  {
							  if (xhr.readyState === 4) 
							  {
								  doneCallback(xhr.status == 200 ? xhr.responseText : null);
							  }
							  
						  }
						  
						  xhr.send(formData);
					  }
					  
					  
					  getFileInfoFromServer(url, function(response) {
						  if (response != null)
						  	{   
								//var str=response.match(/^\{(.*)\}/g);
								
								var result = JSON.parse(response);
								
								if(result.existence=="true")
								{
										var file = {};
										_this.files.push(file);
										file.id="file_"+__indexOf.call(_this.files, file)+"_"+_this.file_id++; //MJZedit it will make a compelete unique id for each file even in paralel execution
										file.name = result.name;
										file.size = result.size;
										file.type = result.mime
										file.serverPath = result.url;
										file.processing = true;
										_this.element.classList.add("started"); //remove the message
										_this.emit("addedfile", file);//قالب فایل روی فرم با صدا زدن این تابع ساخته می شود همچنین نام فایل و غیره نیز افزوده می گردد
										
										if (_this.options.createImageThumbnails && file.type.match(/image.*/) && !file.type.match(/image\/tiff/) && file.size <= _this.options.maxThumbnailFilesize * 1024 * 1024) 
										{
											  //file.previewTemplate.classList.add("processing");
											  _this.createThumbnailOfServerFile(file);
										}
										//MJZedit //create icon for non image files
										else
										{
											  _this.emit("thumbnail", file, null); //pass null as third parameter tell to function to add a defualt icon for the non image files.
										}
										
										file.FailedOrSuccessUpload = true; //for selecting delete method
										
										file.previewTemplate.querySelector("a.img_download_link").href = file.serverPath;
										file.previewTemplate.querySelector(".footer .chBox").value = file.serverPath; //add the (new name + the address) of file in the server for storing in data base
										file.previewTemplate.querySelector(".success-mark").title=_this.options.dictDownloadSuccessFilesMessage;
										file.previewTemplate.querySelector(".success-mark").style.backgroundPosition="-308px -203px";
										file.previewTemplate.querySelector(".success-mark").onmouseover = function(){
										file.previewTemplate.querySelector(".success-mark").style.backgroundPosition="-268px -203px";};
										file.previewTemplate.querySelector(".success-mark").onmouseout = function(){
										file.previewTemplate.querySelector(".success-mark").style.backgroundPosition="-308px -203px";};
										file.previewTemplate.querySelector(".imagecontainer").title=_this.options.dictDeleteSuccessFilesMessage;
										
										file.previewTemplate.querySelector("a.img_download_link").onclick = function(event)
										{
											  event.preventDefault();//prevent the normal click action from occuring
											  
											  window.location = _this.options.urlDownloader+'?file=' + encodeURIComponent(this.href);
										}
										file.processing = false;
										file.previewTemplate.classList.add("success");
										
										
								}
							}
					  });
					  
					  
				}
			/////////////////////////////////////////////////////////////////////////////////////////////////////////////delete the file based on failed or successed 
				//MJZEdit //this function delete just one file
				Dropzone.prototype.DeleteFile = function(file_id) 
				{
					for(var _i=0; _i<this.files.length; _i++)
					   if(this.files[_i].id==file_id)
						{
							if(!this.files[_i].filesProcessing)
							{
								var flag=this.files[_i].FailedOrSuccessUpload; //Select the delete path true= delete from server either, false=just delete icon from form
								if(flag)
								{
									if(this.options.urlDeleter!=null)
									{
										var xmlhttp;
										xmlhttp=new XMLHttpRequest();
										var _file=this.files[_i];
										var _this=this;
										xmlhttp.onreadystatechange=function()
										  {
											  if (xmlhttp.readyState==4 && xmlhttp.status==200)
												{
													if(xmlhttp.responseText!='false')
														_this.removeFile(_file);
													else
														alert(_this.options.informUnsuccessDeletation);
												}
										  }
										
										var formData = new FormData();
										formData.append('type', 'single');
										formData.append('file', this.files[_i].serverPath);  
										xmlhttp.open("POST",this.options.urlDeleter,true);
										xmlhttp.setRequestHeader("Accept", "application/json");
										xmlhttp.setRequestHeader("Cache-Control", "no-cache");
										xmlhttp.setRequestHeader("X-Requested-With", "XMLHttpRequest");
										xmlhttp.setRequestHeader("X-File-Name", _file.name);
										xmlhttp.send(formData);
									}
									else
										alert(this.options.UnableToDelete);
								} 
								else
									this.removeFile(this.files[_i]);
							}
							else
								alert(this.options.dictUnderProcessing);
							return;
						}
					alert(_this.options.informUnsuccessDeletation);
				};
				//End of modified section
			/////////////////////////////////////////////////////////////////////////////////////////////////////////////delete all file				//MJZEdit
				//MJZEdit //function for delete all files button in the top panel
				Dropzone.prototype.DeleteAllFiles = function() 
				{
					var filesUrls = new Array();
					var failedFiles = new Array();
					var flagUnsuccessDelete = false;
					for(var _i=0; _i<this.files.length; _i++)
					{
						var file=this.files[_i];
					   if(!file.filesProcessing)
						{
								if(file.FailedOrSuccessUpload)
								{
									if(this.options.urlDeleter!=null)
										filesUrls.push(Array(file.id,file.serverPath));
									else
										flagUnsuccessDelete = true;
								} 
								else
									failedFiles.push(file);
						}
						else
							flagUnsuccessDelete = true;
					}
					
					for(var _i=0; _i<failedFiles.length; _i++)
					{
						this.removeFile(failedFiles[_i]);//delete the icons that are not file and failed to upload at first.
					}
					
					if(filesUrls.length>0)
					{
						var _this=this;
						var xmlhttp;
						xmlhttp=new XMLHttpRequest();
						xmlhttp.onreadystatechange=function()
						  {
							  if (xmlhttp.readyState==4 && xmlhttp.status==200)
								{
									
									var result=JSON.parse(xmlhttp.responseText);
									var deletedFiles = new Array();
									for(var _j=0; _j<result.length; _j++)
									{
										if(result[_j][1])
										{
											for(var _i=0; _i<_this.files.length; _i++)
					   							if(_this.files[_i].id==result[_j][0])
												{													
													deletedFiles.push(_this.files[_i]);
													break;
												}
										}
										else
											flagUnsuccessDelete = true;	
									}
									for(var _i=0; _i<deletedFiles.length; _i++)
									{
										_this.removeFile(deletedFiles[_i]);
									}
								}
						  }
						var formData = new FormData();
						formData.append('type', 'multi');
						formData.append('FilesAddresses', JSON.stringify(filesUrls));  
						xmlhttp.open("POST",this.options.urlDeleter,true);
						xmlhttp.setRequestHeader("Accept", "application/json");
					    xmlhttp.setRequestHeader("Cache-Control", "no-cache");
					    xmlhttp.setRequestHeader("X-Requested-With", "XMLHttpRequest");
					    //xmlhttp.setRequestHeader("X-File-Name", file.name);
						xmlhttp.send(formData);
					}
					if(flagUnsuccessDelete)
						alert(this.options.UnsuccessDeleteAllFiles);
					else
						this.emit("reset", file);
				};
				//End of modified section
			////////////////////////////////////////////////////////////////////////////////////////////////////////////تابع پاکسازی یک فایل خاص از صفحه
				Dropzone.prototype.removeFile = function(file) 
				{
					  if (file.processing) {
						throw new Error("Can't remove file currently processing");
					  }
					  this.files = without(this.files, file);
					  this.filesQueue = without(this.filesQueue, file);
					  this.emit("removedfile", file);
					  if (this.files.length === 0) {
						return this.emit("reset");
					  }
				};
			//////////////////////////////////////////////////////////////////////////////////////////////////////////////////تابع پاکسازی تمامی فایل ها از صفحه
				Dropzone.prototype.removeAllFiles = function() 
				{
					  var file, _i, _len, _ref;
				
					  _ref = this.files.slice();
					  for (_i = 0, _len = _ref.length; _i < _len; _i++) 
					  {
						file = _ref[_i];
						if (__indexOf.call(this.filesProcessing, file) < 0) 
						{
						  	this.removeFile(file);
						}
					  }
					  return null;
				};
			//////////////////////////////////////////////////////////////////////////////////////////////////////////////////تابع ساخت تصویر کوچک
				Dropzone.prototype.createThumbnail = function(file) 
				{
					  var fileReader,
						_this = this;
				
					  fileReader = new FileReader();
					  
						  fileReader.onload = function() 
						  {
								var img;
						
								img = new Image();
								
									img.onload = function() 
									{
										  var canvas, ctx, srcHeight, srcRatio, srcWidth, srcX, srcY, thumbnail, trgHeight, trgRatio, trgWidth, trgX, trgY;
											
										  canvas = document.createElement("canvas");
										  ctx = canvas.getContext("2d");
										  srcX = 0;
										  srcY = 0;
										  srcWidth = img.width;
										  srcHeight = img.height;
										  canvas.width = _this.options.thumbnailWidth;
										  canvas.height = _this.options.thumbnailHeight;
										  trgX = 0;
										  trgY = 0;
										  trgWidth = canvas.width;
										  trgHeight = canvas.height;
										  srcRatio = img.width / img.height;
										  trgRatio = canvas.width / canvas.height;
										  if (img.height < canvas.height || img.width < canvas.width) {
											trgHeight = srcHeight;
											trgWidth = srcWidth;
										  } else {
											if (srcRatio > trgRatio) {
											  srcHeight = img.height;
											  srcWidth = srcHeight * trgRatio;
											} else {
											  srcWidth = img.width;
											  srcHeight = srcWidth / trgRatio;
											}
										  }
										  srcX = (img.width - srcWidth) / 2;
										  srcY = (img.height - srcHeight) / 2;
										  trgY = (canvas.height - trgHeight) / 2;
										  trgX = (canvas.width - trgWidth) / 2;
										  ctx.drawImage(img, srcX, srcY, srcWidth, srcHeight, trgX, trgY, trgWidth, trgHeight);//resize and crop
										  
										  thumbnail = canvas.toDataURL("image/png"); // convert the canvas to a data URL 
										  return _this.emit("thumbnail", file, thumbnail);
										  	  
									};
								
								return img.src = fileReader.result;
						  };
					  return fileReader.readAsDataURL(file);
				};
			//////////////////////////////////////////////////////////////////////////////////////////////////////////////////تابع ساخت تصویر کوچک
				Dropzone.prototype.createThumbnailOfServerFile = function(file) 
				{
					  			var img;
								var _this=this;
								img = new Image();
								
									img.onload = function() 
									{
										  var canvas, ctx, srcHeight, srcRatio, srcWidth, srcX, srcY, thumbnail, trgHeight, trgRatio, trgWidth, trgX, trgY;
											
										  canvas = document.createElement("canvas");
										  ctx = canvas.getContext("2d");
										  srcX = 0;
										  srcY = 0;
										  srcWidth = img.width;
										  srcHeight = img.height;
										  canvas.width = _this.options.thumbnailWidth;
										  canvas.height = _this.options.thumbnailHeight;
										  trgX = 0;
										  trgY = 0;
										  trgWidth = canvas.width;
										  trgHeight = canvas.height;
										  srcRatio = img.width / img.height;
										  trgRatio = canvas.width / canvas.height;
										  if (img.height < canvas.height || img.width < canvas.width) {
											trgHeight = srcHeight;
											trgWidth = srcWidth;
										  } else {
											if (srcRatio > trgRatio) {
											  srcHeight = img.height;
											  srcWidth = srcHeight * trgRatio;
											} else {
											  srcWidth = img.width;
											  srcHeight = srcWidth / trgRatio;
											}
										  }
										  srcX = (img.width - srcWidth) / 2;
										  srcY = (img.height - srcHeight) / 2;
										  trgY = (canvas.height - trgHeight) / 2;
										  trgX = (canvas.width - trgWidth) / 2;
										  ctx.drawImage(img, srcX, srcY, srcWidth, srcHeight, trgX, trgY, trgWidth, trgHeight);//resize and crop
										  
										  thumbnail = canvas.toDataURL("image/png"); // convert the canvas to a data URL 
										  return _this.emit("thumbnail", file, thumbnail);
										  	  
									};
								
								
								return img.src = file.serverPath;
						  
				};
			//////////////////////////////////////////////////////// کمتر باشد فایل را به مرحله پردازش می فرستد و الا باز می گردد parallelUploads تا زمانی که تعداد فایل در حال ارسال از  
				Dropzone.prototype.processQueue = function() 
				{
					  var i, parallelUploads, processingLength;
				
					  parallelUploads = this.options.parallelUploads;
					  processingLength = this.filesProcessing.length;
					  i = processingLength;
					  while (i < parallelUploads) 
					  {
							if (!this.filesQueue.length) 
							{
							  return;
							}
							this.processFile(this.filesQueue.shift());
							i++;
					  }
				};
			////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////ارزیابی فایل
				Dropzone.prototype.processFile = function(file) 
				{
					  this.filesProcessing.push(file);
					  file.processing = true;
					  this.emit("processingfile", file);
					  //MJZedit start
					  if (this.options.createResizedImage && file.type.match(/image.*/) && !file.type.match(/image\/tiff/)) 
					  		return this.resizeAndUpload(file);
					  else
							return this.uploadFile(file);
					  //end MJZedit
				};
			///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////start of upload
				Dropzone.prototype.uploadFile = function(file) 
				{
					  var _this = this;//MJZedit
					  var formData, handleError, input, inputName, inputType, key, progressObj, value, xhr, _i, _len, _ref, _ref1, _ref2;
					  
					  //ajax code for uploading 
					  xhr = new XMLHttpRequest(); 
					  
					  xhr.open(this.options.method, this.options.urlUploader, true);
					  
					  handleError = function() 
					  {
						return _this.errorProcessing(file, xhr.responseText || ("Server responded with " + xhr.status + " code."));
					  };
					  
					  xhr.onload = function(e) 
					  {
							var response, _ref;
					
							if (!((200 <= (_ref = xhr.status) && _ref < 300))) 
							{
							  return handleError();
							} 
							else 
							{
								  _this.emit("uploadprogress", file, 100);//نشان دادن علامت آپلود 100٪
								  
								  response = xhr.responseText;
								  if (xhr.getResponseHeader("content-type") && ~xhr.getResponseHeader("content-type").indexOf("application/json")) 
								  {
									  
									response = JSON.parse(response);
								  }
								  
								  return _this.finished(file, response, e);
							}
					  };
					  
					  xhr.onerror = function() 
					  {
						return handleError();
					  };
					  //اعلام شیء جهت نشان دادن درصد پیشرفت کار
					  progressObj = (_ref = xhr.upload) != null ? _ref : xhr;
					  //نشان دادن درصد پیشرفت کار
					  progressObj.onprogress = function(e) 
					  {
						return _this.emit("uploadprogress", file, Math.max(0, Math.min(100, (e.loaded / e.total) * 100)));
					  };
					  
					  xhr.setRequestHeader("Accept", "application/json");
					  xhr.setRequestHeader("Cache-Control", "no-cache");
					  xhr.setRequestHeader("X-Requested-With", "XMLHttpRequest");
					  xhr.setRequestHeader("X-File-Name", file.name);
					  
					  formData = new FormData();
					  //post به عنوان اطلاعات متد params افزودن داده های داخل متغییر
					  if (this.options.params) 
					  {
							_ref1 = this.options.params;
							for (key in _ref1) 
							{
								  value = _ref1[key];
								  formData.append(key, value);
							}
					  }
					  
					  //افزودن اطلاعات فرم به دنبال عکس
					  
					  if (this.element.tagName === "FORM") 
					  {
							_ref2 = this.element.querySelectorAll("input, textarea, select, button");
							for (_i = 0, _len = _ref2.length; _i < _len; _i++) 
							{
							  input = _ref2[_i];
							  inputName = input.getAttribute("name");
							  inputType = input.getAttribute("type");
							  if (!inputType || inputType.toLowerCase() !== "checkbox" || input.checked) 
							  {
								formData.append(inputName, input.value);
							  }
							}
					  }
					  
					  
					  this.emit("sending", file, xhr, formData);
					  formData.append('resized', 'false'); //MJZedit
					  formData.append(this.options.paramName, file);
					  return xhr.send(formData);
				};
			///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
			//MJZedit start
				Dropzone.prototype.resizeAndUpload = function(file) 
				{
					if(file.type.match(/image.*/) && !file.type.match(/image\/tiff/))
					{
							var _this = this;
							var formData, handleError, input, inputName, inputType, key, progressObj, value, xhr, _i, _len, _ref, _ref1, _ref2;
							var reader = new FileReader();
									reader.onloadend = function() 
									{
								 
											var tempImg = new Image();
											tempImg.src = reader.result;
											tempImg.onload = function() 
											{
													var MAX_WIDTH = _this.options.resizedWidth;
													var MAX_HEIGHT = _this.options.resizedHeight;
													var tempW = tempImg.width;
													var tempH = tempImg.height;
													if (tempW > tempH) 
													{
														if (tempW > MAX_WIDTH) 
														{
														   tempH *= MAX_WIDTH / tempW;
														   tempW = MAX_WIDTH;
														}
													} 
													else 
													{
														if (tempH > MAX_HEIGHT) 
														{
														   tempW *= MAX_HEIGHT / tempH;
														   tempH = MAX_HEIGHT;
														}
													}
											 
													var canvas = document.createElement('canvas');
													canvas.width = tempW;
													canvas.height = tempH;
													var ctx = canvas.getContext("2d");
													ctx.drawImage(this, 0, 0, tempW, tempH);
													var dataURL = canvas.toDataURL(file.type);
													_this.emit("imageResized", file, dataURL);
													
														xhr = new XMLHttpRequest(); 
									  
														xhr.open(_this.options.method, _this.options.urlUploader, true);
														
														handleError = function() 
														{
														  return _this.errorProcessing(file, xhr.responseText || ("Server responded with " + xhr.status + " code."));
														};
														
														xhr.onload = function(e) 
														{
															  var response, _ref;
													  
															  if (!((200 <= (_ref = xhr.status) && _ref < 300))) 
															  {
																return handleError();
															  } 
															  else 
															  {
																	_this.emit("uploadprogress", file, 100);//نشان دادن علامت آپلود 100٪
																	
																	response = xhr.responseText;
																	
																	if (xhr.getResponseHeader("content-type") && ~xhr.getResponseHeader("content-type").indexOf("application/json")) 
																	{
																		
																	  response = JSON.parse(response);
																	}
																	
																	return _this.finished(file, response, e);
															  }
														};
														
														xhr.onerror = function() 
														{
														  return handleError();
														};
														//اعلام شیء جهت نشان دادن درصد پیشرفت کار
														progressObj = (_ref = xhr.upload) != null ? _ref : xhr;
														//نشان دادن درصد پیشرفت کار
														progressObj.onprogress = function(e) 
														{
														  return _this.emit("uploadprogress", file, Math.max(0, Math.min(100, (e.loaded / e.total) * 100)));
														};
														
														xhr.setRequestHeader("Accept", "application/json");
														xhr.setRequestHeader("Cache-Control", "no-cache");
														xhr.setRequestHeader("X-Requested-With", "XMLHttpRequest");
														xhr.setRequestHeader("X-File-Name", file.name);
														
														formData = new FormData();
														//post به عنوان اطلاعات متد params افزودن داده های داخل متغییر
														if (_this.options.params) 
														{
															  _ref1 = _this.options.params;
															  for (key in _ref1) 
															  {
																	value = _ref1[key];
																	formData.append(key, value);
															  }
														}
														
														//افزودن اطلاعات فرم به دنبال عکس
					  
														if (_this.element.tagName === "FORM") 
														{
															  _ref2 = _this.element.querySelectorAll("input, textarea, select, button");
															  for (_i = 0, _len = _ref2.length; _i < _len; _i++) 
															  {
																input = _ref2[_i];
																inputName = input.getAttribute("name");
																inputType = input.getAttribute("type");
																if (!inputType || inputType.toLowerCase() !== "checkbox" || input.checked) 
																{
																  formData.append(inputName, input.value);
																}
															  }
														}
														
														_this.emit("sending", file, xhr, formData);
														
														//extracting the extension of file
														var re = /(?:\.([^.]+))?$/;
														var ext = re.exec(file.name)[1];
														//formData.append('extension', ext);//send the extension as a data
														//formData.append('resized', true);
														formData.append(_this.options.paramName, dataURL);
														return xhr.send(formData);
											}
									}
									reader.onprogress = function(e) 
									{
										return _this.emit("uploadprogress", file, Math.max(0, Math.min(100, (e.loaded / e.total) * 100)));
										
									};
								reader.readAsDataURL(file);
					}
				};
				//End of MJZedit
			//////////////////////////////////////////////////////////////////////////////////////////////////////end of uploading function
				Dropzone.prototype.finished = function(file, responseText, e) 
				{
					  //MJZedit					
					  file.FailedOrSuccessUpload = true; //for selecting delete method
					  file.serverPath = responseText;
					  if(file.previewTemplate.querySelector("img#"+file.id)!=null)
					  	{
							file.previewTemplate.querySelector("img#"+file.id).setAttribute('href',file.serverPath);
						}
					  file.previewTemplate.querySelector("a.img_download_link").href = file.serverPath;
					  file.previewTemplate.querySelector(".footer .chBox").value = file.serverPath; //add the (new name + the address) of file in the server for storing in data base
					  file.previewTemplate.querySelector(".success-mark").title=this.options.dictDownloadSuccessFilesMessage;
					  file.previewTemplate.querySelector(".imagecontainer").title=this.options.dictDeleteSuccessFilesMessage;
					  
					  
					  if(this.options.urlDownloader!=null)
					  {
						  var _this=this;
						  file.previewTemplate.querySelector("a.img_download_link").onclick = function(event)
						  {
								event.preventDefault();//prevent the normal click action from occuring
								window.location = _this.options.urlDownloader+'?file=' + encodeURIComponent(this.href);
						  }
					  }
					  
					  //End mjzedit
					  this.filesProcessing = without(this.filesProcessing, file);
					  file.processing = false;
					  
					  
					  this.processQueue();
					  this.emit("success", file, responseText, e);
					  this.emit("finished", file, responseText, e);
					  return this.emit("complete", file);
					  
				};
			///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
				Dropzone.prototype.errorProcessing = function(file, message) 
				{
					  //MJZedit
					  file.FailedOrSuccessUpload = false;//for selecting delete method
					  var _this=this;
					  file.previewTemplate.querySelector(".error-mark").id = file.id;
					  file.previewTemplate.querySelector(".error-mark").title=this.options.dictDeleteFailedFilesMessage;
					  file.previewTemplate.querySelector(".error-mark").onclick = function ()
					  {
						   _this.DeleteFile(this.id);//to add operation for remove the file from the form
					  }
					  //End of modified section
					  
					  this.filesProcessing = without(this.filesProcessing, file);
					  file.processing = false;
					  this.processQueue();
					  this.emit("error", file, message);
					  file.previewTemplate.classList.remove("processing");
					  return this.emit("complete", file);
				};
			///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
				//MJZedit start
				Dropzone.prototype.setResizeOptions = function(createResizedImage, resizedWidth, resizedHeight) 
				{
					if(createResizedImage)
					{
						resizedWidth  = parseInt(resizedWidth.toEnDigit(),10);
						resizedHeight = parseInt(resizedHeight.toEnDigit(),10);
					   	if(!isNaN(resizedWidth)&&resizedWidth>0)
							this.options.resizedWidth=resizedWidth;
						if(!isNaN(resizedHeight)&&resizedHeight>0)
							this.options.resizedHeight=resizedHeight;
						this.options.createResizedImage=true;	
					}
					else
						this.options.createResizedImage=false;
				};
				//MJZedit end
				///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
			
				return Dropzone;
			
			})(Em);
			
			
			
			////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
			  Dropzone.version = "3.0.0";
			
			  Dropzone.options = {};
			
			  Dropzone.instances = [];
			  //تابعی که آی دی فرم را گرفته و اشاره گری به دراپ زون بر می گرداند
			  Dropzone.forElement = function(element) 
			  {
					var instance, _i, _len, _ref;
				
					if (typeof element === "string") 
					{
					  element = document.querySelector(element);
					}
					_ref = Dropzone.instances;
					for (_i = 0, _len = _ref.length; _i < _len; _i++) 
					{
					  instance = _ref[_i];
					  if (instance.element === element) 
					  {
						return instance;
					  }
					}
					return null;
			  };
			  //جلوگیری از لود شدن در اوپرا و مکینتاش//////////////////////////////////////////////////////////////////////////////////////////////
			  //MJZedit
			  Dropzone.blacklistedBrowsers = new Array();
			  Dropzone.blacklistedBrowsers[0]=[/opera.*version\/12/i];
			  Dropzone.blacklistedBrowsers[1]=[/Macintosh.*version\/12/i];
			  Dropzone.isBrowserSupported = function() 
			  {
					var capableBrowser, regex, _i, _len, _ref;
				
					capableBrowser = true;
					
					if (window.File && window.FileReader && window.FileList && window.Blob && window.FormData && document.querySelector) 
					{
						
						  if (!("classList" in document.createElement("a"))) 
						  {
							capableBrowser = false;
						  } 
						  else 
						  {
							  for(_j=0;_j<Dropzone.blacklistedBrowsers.length && capableBrowser;_j++)
							  {
								_ref = Dropzone.blacklistedBrowsers[_j];
								for (_i = 0, _len = _ref.length; _i < _len && capableBrowser; _i++) 
								{
									  regex = _ref[_i];
									  if (regex.test(navigator.userAgent)) 
									  		capableBrowser = false;
								}
							  }
						  }
					} 
					else 
					{
					  capableBrowser = false;
					}
					
					return capableBrowser;
			  };
			  //End MJZedit
		      /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
			  without = function(list, rejectedItem) //تابع حذف آیکون فایل از صفحه فرم
			  {
					var item, _i, _len, _results;
				
					_results = [];
					for (_i = 0, _len = list.length; _i < _len; _i++) {
					  item = list[_i];
					  if (item !== rejectedItem) {
						_results.push(item);
					  }
					}
					return _results;
			  };
			  /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
			  camelize = function(str) {
				return str.replace(/[\-_](\w)/g, function(match) {
				  return match[1].toUpperCase();
				});
			  };
			  /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
			  Dropzone.createElement = function(string) 
			  {
				var div;
			
				div = document.createElement("div");
				div.innerHTML = string;
				return div.childNodes[0];
			  };
			  ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
			  Dropzone.elementInside = function(element, container) 
			  {
				if (element === container) 
				{
				  return true;
				}
				while (element = element.parentNode) 
				{
				  if (element === container) 
				  {
					return true;
				  }
				}
				return false;
			  };
			  ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
			  if (typeof jQuery !== "undefined" && jQuery !== null) 
			  {
				jQuery.fn.dropzone = function(options) 
				{
				  return this.each(function() 
				  {
					return new Dropzone(this, options);
				  });
				};
			  }
			  ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
			  if (typeof module !== "undefined" && module !== null) 
			  {
					module.exports = Dropzone;
			  } 
			  else 
			  {
					window.Dropzone = Dropzone;
			  }
			  ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
			  contentLoaded = function(win, fn) 
			  {
					var add, doc, done, init, poll, pre, rem, root, top;
				
					done = false;
					top = true;
					doc = win.document;
					root = doc.documentElement;
					add = (doc.addEventListener ? "addEventListener" : "attachEvent");
					rem = (doc.addEventListener ? "removeEventListener" : "detachEvent");
					pre = (doc.addEventListener ? "" : "on");
					
					init = function(e) 
					{
						  if (e.type === "readystatechange" && doc.readyState !== "complete") 
						  {
							  return;
						  }
						  (e.type === "load" ? win : doc)[rem](pre + e.type, init, false);
						  if (!done && (done = true)) 
						  {
							return fn.call(win, e.type || e);
						  }
					};
					
					poll = function() 
					{
					  var e;
				
					  try {
						root.doScroll("left");
					  } catch (_error) {
						e = _error;
						setTimeout(poll, 50);
						return;
					  }
					  return init("poll");
					};
					
					if (doc.readyState !== "complete") 
					{
					  if (doc.createEventObject && root.doScroll) 
					  {
						try {
						  top = !win.frameElement;
						} catch (_error) {}
						
						if (top) 
						{
						  poll();
						}
					  }
					  doc[add](pre + "DOMContentLoaded", init, false);
					  doc[add](pre + "readystatechange", init, false);
					  return win[add](pre + "load", init, false);
					}
			  };
			  /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
			  contentLoaded(window, function() 
			  {
						var checkElements, dropzone, dropzones, _i, _len, _results;
					
						if (false) 
						{
						  dropzones = document.querySelectorAll(".dropzone");
						} 
						else 
						{
							  dropzones = [];//کلیه عناصر با کلاس دراپ زون در این آرایه قرار می گیرند
							  checkElements = function(elements) 
							  {
									var el, _i, _len, _results;
							
									_results = [];
									
									for (_i = 0, _len = elements.length; _i < _len; _i++) 
									{
										el = elements[_i];
										if (/(^| )dropzone($| )/.test(el.className)) 
										{
										  _results.push(dropzones.push(el));
										} 
										else 
										{
										  _results.push(void 0);
										}
									}
									return _results;
							  };
							  checkElements(document.getElementsByTagName("div"));
							  checkElements(document.getElementsByTagName("form"));
						}
						_results = [];
						for (_i = 0, _len = dropzones.length; _i < _len; _i++) 
						{
							  dropzone = dropzones[_i];
							  
							  _results.push(new Dropzone(dropzone)); //سپس برای هر کدام از عناصر یافته شده در مرحله قبل تابع دراپ زون صدا زده می شود
						}
						return _results;
				});
			   /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
   }).call(this);

});
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
require.register("func2", function(exports, require, module){
		/**
		 * Expose `Emitter`.
		 */
	
		module.exports = Emitter;
	
		/**
		 * Initialize a new `Emitter`.
		 *
		 * @api public
		 */
		
		function Emitter(obj) 
		{
		  if (obj) return mixin(obj);
		};
		
		/**
		 * Mixin the emitter properties.
		 *
		 * @param {Object} obj
		 * @return {Object}
		 * @api private
		 */
		
		function mixin(obj) 
		{
		  for (var key in Emitter.prototype) 
		  {
			obj[key] = Emitter.prototype[key];
		  }
		  return obj;
		}
	
		/**
		 * Listen on the given `event` with `fn`.
		 *
		 * @param {String} event
		 * @param {Function} fn
		 * @return {Emitter}
		 * @api public
		 */
	
		Emitter.prototype.on = function(event, fn)
		{
		  this._callbacks = this._callbacks || {};
		  (this._callbacks[event] = this._callbacks[event] || []).push(fn);
		  return this;
		};
		
		/**
		 * Adds an `event` listener that will be invoked a single
		 * time then automatically removed.
		 *
		 * @param {String} event
		 * @param {Function} fn
		 * @return {Emitter}
		 * @api public
		 */
		
		Emitter.prototype.once = function(event, fn)
		{
		  var self = this;
		  this._callbacks = this._callbacks || {};
		
		  function on() {
			self.off(event, on);
			fn.apply(this, arguments);
		  }
		
		  fn._off = on;
		  this.on(event, on);
		  return this;
		};
	
		/**
		 * Remove the given callback for `event` or all
		 * registered callbacks.
		 *
		 * @param {String} event
		 * @param {Function} fn
		 * @return {Emitter}
		 * @api public
		 */
		
		Emitter.prototype.off =
		Emitter.prototype.removeListener =
		Emitter.prototype.removeAllListeners = function(event, fn)
		{
		  this._callbacks = this._callbacks || {};
		  var callbacks = this._callbacks[event];
		  if (!callbacks) return this;
		
		  // remove all handlers
		  if (1 == arguments.length) 
		  {
			delete this._callbacks[event];
			return this;
		  }
		
		  // remove specific handler
		  var i = callbacks.indexOf(fn._off || fn);
		  if (~i) callbacks.splice(i, 1);
		  return this;
		};
	
		/**
		 * Emit `event` with the given args.
		 *
		 * @param {String} event
		 * @param {Mixed} ...
		 * @return {Emitter}
		 */
		
		Emitter.prototype.emit = function(event)
		{
		  this._callbacks = this._callbacks || {};
		  var args = [].slice.call(arguments, 1)
			, callbacks = this._callbacks[event];
		
		  if (callbacks) {
			callbacks = callbacks.slice(0);
			for (var i = 0, len = callbacks.length; i < len; ++i) {
			  callbacks[i].apply(this, args);
			}
		  }
		
		  return this;
		};
	
		/**
		 * Return array of callbacks for `event`.
		 *
		 * @param {String} event
		 * @return {Array}
		 * @api public
		 */
	
		Emitter.prototype.listeners = function(event)
		{
		  this._callbacks = this._callbacks || {};
		  return this._callbacks[event] || [];
		};
		
		/**
		 * Check if this emitter has `event` handlers.
		 *
		 * @param {String} event
		 * @return {Boolean}
		 * @api public
		 */
	
		Emitter.prototype.hasListeners = function(event)
		{
		  return !! this.listeners(event).length;
		};

});
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		/*
		if (typeof exports == "object") 
		{
		   module.exports = require("func1");
		} 
		else if (typeof define == "function" && define.amd) 
		{
		  define(function(){ return require("func1"); });
		} 
		else 
		{*/
	  		window["Dropzone"] = require("func1");
		//}
})();