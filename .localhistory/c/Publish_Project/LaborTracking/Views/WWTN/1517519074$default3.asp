<!--#include virtual="/dbopen.asp" -->

<%
 if Session("agtcode")="" or Session("Lcode")="" then
	Response.redirect "../default.asp"

else
	if Session("CompId")="KEUSA" then
		Response.Redirect "KE_default.asp"
	else
		if Cint(Session("agtlevel")) > 3 then
		else
			Response.Redirect "default2.asp"
		end if
	end if
	
	agtcode=Session("agtcode")

	sql="Select * from AgentTB where agtcode='"& agtcode &"'"
	set rs=db.Execute(sql)	
	
end if

if Cint(Session("agtlevel")) >= 7 then
    link1=""
    link2=""
    link3=""
    link4=""
    link5="list.asp?board=RPT_BOD"
    link6=""
    link7=""
    link8=""
    link9=""
    link10=""
    link11=""
    link12=""
else
    link1=""
    link2=""
    link3=""
    link4=""
    link5="list.asp?board=RPT_GM"
    link6=""
    link7=""
    link8=""
    link9=""
    link10=""
    link11=""
    link12=""
end if
%>
<html>
<head>
<meta http-equiv="Content-Type" content="text/html; charset=iso-8859-1">
<title>CASnet Dashboard</title>
<style type="text/css">
<!--
body {
	margin-left: 0px;
	margin-top: 0px;
	margin-right: 0px;
	margin-bottom: 0px;
}
-->
</style>
<link href="style2.css" rel="stylesheet" type="text/css">
<script type="text/javascript">
<!--
function MM_swapImgRestore() { //v3.0
  var i,x,a=document.MM_sr; for(i=0;a&&i<a.length&&(x=a[i])&&x.oSrc;i++) x.src=x.oSrc;
}
function MM_preloadImages() { //v3.0
  var d=document; if(d.images){ if(!d.MM_p) d.MM_p=new Array();
    var i,j=d.MM_p.length,a=MM_preloadImages.arguments; for(i=0; i<a.length; i++)
    if (a[i].indexOf("#")!=0){ d.MM_p[j]=new Image; d.MM_p[j++].src=a[i];}}
}

function MM_findObj(n, d) { //v4.01
  var p,i,x;  if(!d) d=document; if((p=n.indexOf("?"))>0&&parent.frames.length) {
    d=parent.frames[n.substring(p+1)].document; n=n.substring(0,p);}
  if(!(x=d[n])&&d.all) x=d.all[n]; for (i=0;!x&&i<d.forms.length;i++) x=d.forms[i][n];
  for(i=0;!x&&d.layers&&i<d.layers.length;i++) x=MM_findObj(n,d.layers[i].document);
  if(!x && d.getElementById) x=d.getElementById(n); return x;
}

function MM_swapImage() { //v3.0
  var i,j=0,x,a=MM_swapImage.arguments; document.MM_sr=new Array; for(i=0;i<(a.length-2);i+=3)
   if ((x=MM_findObj(a[i]))!=null){document.MM_sr[j++]=x; if(!x.oSrc) x.oSrc=x.src; x.src=a[i+2];}
}

function SecurityForm_submitIt()
{
	document.SecurityForm.action = "https://www.tfaforms.com/359899";	
	document.SecurityForm.target='_blank';
	document.SecurityForm.method = "POST";
	document.SecurityForm.submit();
}
//-->
</script>
</head>

<body onLoad="MM_preloadImages('images/Intranet/buttons/top_CASview.gif','images/Intranet/buttons/top_Policy.gif','images/Intranet/buttons/top_Training.gif','images/Intranet/buttons/top_Safe.gif','images/Intranet/buttons/top_Report.gif','images/Intranet/buttons/top_CC.gif','images/Intranet/buttons/top_form.gif','images/Intranet/buttons/top_Contract.gif','images/Intranet/buttons/top_Sales.gif','images/Intranet/buttons/top_Applicant.gif','images/Intranet/buttons/top_BI.gif','images/Intranet/buttons/top_Survey.gif')">
<table width="100%" border="0" cellspacing="0" cellpadding="0">
  <tr>
    <td width="220"><a href="default3.asp"><img src="images/Intranet/pages/top_logo.gif" width="220" height="79" border="0"></a></td>
    <td valign="top"><table width="100%" border="0" cellspacing="0" cellpadding="0">
      <tr>
        <td background="images/Intranet/pages/top_top_back.gif"><table width="770" border="0" cellspacing="0" cellpadding="0">
            <tr>
              <td><img src="images/transparent.gif" width="50" height="26"></td>
              <td width="80"><a href="default3.asp" class="text12 White">Home</a></td>
              <td width="60"><a href="logout.asp" class="text12 White">Log out</a></td>
              <%if Cint(Session("agtlevel")) >= 7 then%><td width="120"><a href="default4.asp" target="_blank" class="text12link White">Test Dashboard</a></td><%end if%>
            </tr>
          </table></td>
      </tr>
      <tr>
        <td valign="top" background="images/Intranet/pages/top_btm_back.gif"><table width="770" border="0" cellspacing="0" cellpadding="0">
          <tr>
            <td width="71"><img src="images/Intranet/pages/top_img1.gif" width="71" height="53"></td>
            <td><a href="default3.asp" onMouseOut="MM_swapImgRestore()" onMouseOver="MM_swapImage('Image109','','images/Intranet/buttons/top_CASview.gif',1)"><img src="images/Intranet/buttons/top_CASview0.gif" alt="CAS View" name="Image109" width="57" height="53" border="0"></a></td>
            <td><a href="mboard/list.asp?board=PP_Company" onMouseOut="MM_swapImgRestore()" onMouseOver="MM_swapImage('Image108','','images/Intranet/buttons/top_Policy.gif',1)"><img src="images/Intranet/buttons/top_Policy0.gif" alt="Policy & Procedures" name="Image108" width="45" height="53" border="0"></a></td>
            <td><a href="mboard/list.asp?board=TR_Manual" onMouseOut="MM_swapImgRestore()" onMouseOver="MM_swapImage('Image110','','images/Intranet/buttons/top_Training.gif',1)"><img src="images/Intranet/buttons/top_Training0.gif" alt="Training" name="Image110" width="53" height="53" border="0"></a></td>
            <td><a href="mboard/list.asp?board=SF_Incident" onMouseOut="MM_swapImgRestore()" onMouseOver="MM_swapImage('Image111','','images/Intranet/buttons/top_Safe.gif',1)"><img src="images/Intranet/buttons/top_Safe0.gif" alt="Safety" name="Image111" width="56" height="53" border="0"></a></td>
            <td><a href="mboard/<%=link5%>" onMouseOut="MM_swapImgRestore()" onMouseOver="MM_swapImage('Image112','','images/Intranet/buttons/top_Report.gif',1)"><img src="images/Intranet/buttons/top_Report0.gif" alt="Reports - Files" name="Image112" width="50" height="53" border="0"></a></td>
            <td><a href="default2.asp" onMouseOut="MM_swapImgRestore()" onMouseOver="MM_swapImage('Image113','','images/Intranet/buttons/top_CC.gif',1)"><img src="images/Intranet/buttons/top_CC0.gif" alt="Cash Collection" name="Image113" width="40" height="53" border="0"></a></td>
            <td><a href="mboard/list.asp?board=FT_HR" onMouseOut="MM_swapImgRestore()" onMouseOver="MM_swapImage('Image114','','images/Intranet/buttons/top_form.gif',1)"><img src="images/Intranet/buttons/top_form0.gif" alt="Forms & Templates" name="Image114" width="52" height="53" border="0"></a></td>
            <td><a href="mboard/list.asp?board=CM_Customer" onMouseOut="MM_swapImgRestore()" onMouseOver="MM_swapImage('Image115','','images/Intranet/buttons/top_Contract.gif',1)"><img src="images/Intranet/buttons/top_Contract0.gif" alt="Contract Management" name="Image115" width="57" height="53" border="0"></a></td>
            <td><a href="mboard/list.asp?board=PS_proforma" onMouseOut="MM_swapImgRestore()" onMouseOver="MM_swapImage('Image116','','images/Intranet/buttons/top_Sales.gif',1)"><img src="images/Intranet/buttons/top_Sales0.gif" alt="Prospects / Sales" name="Image116" width="52" height="53" border="0"></a></td>
            <td><a href="empForm/list.asp" onMouseOut="MM_swapImgRestore()" onMouseOver="MM_swapImage('Image117','','images/Intranet/buttons/top_Applicant.gif',1)"><img src="images/Intranet/buttons/top_Applicant0.gif" alt="Applicant Tracking" name="Image117" width="52" height="53" border="0"></a></td>
            <td><a href="#" onMouseOut="MM_swapImgRestore()" onMouseOver="MM_swapImage('Image118','','images/Intranet/buttons/top_BI.gif',1)"><img src="images/Intranet/buttons/top_BI0.gif" alt="Business Intelligence" name="Image118" width="43" height="53" border="0"></a></td>
            <td><a href="#" onMouseOut="MM_swapImgRestore()" onMouseOver="MM_swapImage('Image119','','images/Intranet/buttons/top_Survey.gif',1)"><img src="images/Intranet/buttons/top_Survey0.gif" alt="Customer Survey" name="Image119" width="48" height="53" border="0"></a></td>
            <td width="30">&nbsp;</td>
            </tr>
        </table></td>
      </tr>
    </table></td>
  </tr>
</table>
<table width="100%" border="0" cellspacing="0" cellpadding="0">
  <tr>
    <td><img src="images/transparent.gif" width="100" height="5"></td>
  </tr>
</table>
<table width="990" border="0" cellspacing="0" cellpadding="0">
  <tr>
    <td width="7" valign="top">&nbsp;</td>
    <td width="480" valign="top"><table width="100%" border="0" cellspacing="0" cellpadding="0">
      <tr>
        <td><table width="100%" border="0" cellspacing="0" cellpadding="0">
          <tr>
            <td background="images/Intranet/titles/t_casview_back.gif"><img src="images/Intranet/titles/t_casview.gif" width="209" height="62"></td>
            <td background="images/Intranet/titles/t_casview_back.gif"><div align="right"><img src="images/Intranet/titles/t_casview_end.gif" width="18" height="62"></div></td>
          </tr>
        </table></td>
      </tr>
      <tr>
        <td bgcolor="FFFFFF"><table width="100%" border="0" cellspacing="0" cellpadding="0">
          <tr>
            <td bgcolor="FFFFFF" class="CASnet_border"><div align="center">
              <iframe src ="CASview/CASview.asp" width="480" height="377" frameborder="0" scrolling="no"></iframe>
            </div></td>
          </tr>
        </table></td>
      </tr>
    </table></td>
    <td width="15" valign="top">&nbsp;</td>
    <td valign="top"><table width="100%" border="0" cellspacing="0" cellpadding="0">
      <tr>
        <td><img src="images/transparent.gif" width="100" height="21"></td>
      </tr>
    </table>
      <table width="100%" border="0" cellspacing="0" cellpadding="0">
        <tr>
          <td><table width="100%" border="0" cellspacing="0" cellpadding="0">
              <tr>
                <td width="170" background="images/Intranet/titles/titlebar_back.gif"><img src="images/Intranet/titles/t_policy.gif" width="170" height="27"></td>
                <td width="70" background="images/Intranet/titles/titlebar_back.gif"><div align="right"><img src="images/Intranet/titles/titlebar_end.gif" width="10" height="27"></div></td>
                <td background="images/Intranet/titles/title_line_back.gif"><div align="right"><img src="images/Intranet/titles/title_line_end.gif" width="10" height="27"></div></td>
              </tr>
          </table></td>
        </tr>
        <tr>
          <td valign="top" class="CASnet_border"><table width="98%" border="0" align="center" cellpadding="0" cellspacing="0">
              <tr>
                <td width="80" valign="top"><img src="images/Intranet/pages/icon_Policy.gif" width="80" height="85"></td>
                <td valign="top"><table width="100%" border="0" cellspacing="0" cellpadding="2">
                    <tr>
                      <td><img src="images/transparent.gif" width="10" height="3"></td>
                    </tr>
                    <tr>
                      <td class="text12"><a href="mboard/list.asp?board=PP_CompMan" class="text12link black">Company Manuals</a></td>
                    </tr>
                    <tr>
                      <td class="text12"><a href="mboard/list.asp?board=PP_Company" class="text12link black">Company Policy</a></td>
                    </tr>
                    <tr>
                      <td class="text12"><a href="mboard/list.asp?board=PP_SOP" class="text12link black">SOP</a></td>
                    </tr>
                    <tr>
                      <td class="text12"><a href="mboard/list.asp?board=PP_Process" class="text12link black">Process Analysis</a></td>
                    </tr>
                    <tr>
                      <td><img src="images/transparent.gif" width="10" height="3"></td>
                    </tr>
                </table></td>
              </tr>
          </table></td>
        </tr>
      </table>
      <table width="100%" border="0" cellspacing="0" cellpadding="0">
        <tr>
          <td><img src="images/transparent.gif" width="100" height="9"></td>
        </tr>
      </table>
      <table width="100%" border="0" cellspacing="0" cellpadding="0">
        <tr>
          <td><table width="100%" border="0" cellspacing="0" cellpadding="0">
              <tr>
                <td width="197" background="images/Intranet/titles/titlebar_back.gif"><img src="images/Intranet/titles/t_training.gif" width="197" height="27"></td>
                <td width="43" background="images/Intranet/titles/titlebar_back.gif"><div align="right"><img src="images/Intranet/titles/titlebar_end.gif" width="10" height="27"></div></td>
                <td background="images/Intranet/titles/title_line_back.gif"><div align="right"><img src="images/Intranet/titles/title_line_end.gif" width="10" height="27"></div></td>
              </tr>
          </table></td>
        </tr>
        <tr>
          <td valign="top" class="CASnet_border"><table width="98%" border="0" align="center" cellpadding="0" cellspacing="0">
              <tr>
                <td width="80" valign="top"><img src="images/Intranet/pages/icon_Training.gif" width="80" height="85"></td>
                <td><table width="100%" border="0" cellspacing="0" cellpadding="2">
                    <tr>
                      <td><img src="images/transparent.gif" width="10" height="3"></td>
                    </tr>
                    <tr>
                      <td class="text12"><a href="mboard/list.asp?board=TR_Manual" class="text12link black">Manuals</a></td>
                    </tr>
                    <tr>
                      <td class="text12"><a href="mboard/list.asp?board=TR_Lesson" class="text12link black">Lesson Plans</a></td>
                    </tr>
                    <tr>
                      <td class="text12"><a href="mboard/list.asp?board=TR_Material" class="text12link black">Training Material</a></td>
                    </tr>
                    <tr>
                      <td class="text12"><a href="mboard/list.asp?board=TR_Employee" class="text12link black">Employee Orientation</a></td>
                    </tr>
                    <tr>
                      <td class="text12"><a href="mboard/list.asp?board=TR_Rules" class="text12link black">Rules &amp; Regulations</a></td>
                    </tr>
                    <tr>
                      <td class="text12"><a href="mboard/list.asp?board=TR_Audit" class="text12link black">Audits &amp; Reports</a></td>
                    </tr>
                    <%if Cint(Session("agtlevel")) >= 6 then%>
                    <tr>
                      <td class="text12"><a href="http://cbt.casusa.com/adminindex.aspx" target="_blank" class="text12link darkred">Manage CBT</a></td>
                    </tr>
                    <tr>
                      <td class="text12"><a href="http://cbt.casusa.com" target="_blank" class="text12link darkred">Take CBT</a></td>
                    </tr>
                    <tr>
                    <%end if%>
                      <td><img src="images/transparent.gif" width="10" height="3"></td>
                    </tr>
                </table></td>
              </tr>
          </table></td>
        </tr>
      </table>
      <table width="100%" border="0" cellspacing="0" cellpadding="0">
        <tr>
          <td><img src="images/transparent.gif" width="100" height="9"></td>
        </tr>
      </table>
      <table width="100%" border="0" cellspacing="0" cellpadding="0">
        <tr>
          <td><table width="100%" border="0" cellspacing="0" cellpadding="0">
              <tr>
                <td width="170" background="images/Intranet/titles/titlebar_back.gif"><img src="images/Intranet/titles/t_safety.gif" width="170" height="27"></td>
                <td width="70" background="images/Intranet/titles/titlebar_back.gif"><div align="right"><img src="images/Intranet/titles/titlebar_end.gif" width="10" height="27"></div></td>
                <td background="images/Intranet/titles/title_line_back.gif"><div align="right"><img src="images/Intranet/titles/title_line_end.gif" width="10" height="27"></div></td>
              </tr>
          </table></td>
        </tr>
        <tr>
          <td valign="top" class="CASnet_border"><table width="98%" border="0" align="center" cellpadding="0" cellspacing="0">
              <tr>
                <td width="80" valign="top"><img src="images/Intranet/pages/icon_Safety.gif" width="80" height="85"></td>
                <td><table width="100%" border="0" cellspacing="0" cellpadding="2">
                    <tr>
                      <td><img src="images/transparent.gif" width="10" height="3"></td>
                    </tr>
                    <tr>
                      <td class="text12"><a href="mboard/list.asp?board=SF_Incident" class="text12link black">Incidents</a></td>
                    </tr>
                    <tr>
                      <td class="text12"><a href="mboard/list.asp?board=SF_OSHA" class="text12link black">OSHA</a></td>
                    </tr>
                    <tr>
                      <td class="text12"><a href="mboard/list.asp?board=SF_Alert" class="text12link black">Safety Alerts</a></td>
                    </tr>
                    <tr>
                      <td class="text12"><a href="mboard/list.asp?board=SF_Report" class="text12link black">Meetings, Reports &amp; Audits</a></td>
                    </tr>
                    <tr>
                      <td class="text12"><a href="mboard/list.asp?board=SF_Manual" class="text12link black">Safety Manuals</a></td>
                    </tr>
                    <form action="" method="post" name="SecurityForm">
                    <%
					agtcode=Session("agtcode")
					SLcode=Session("Lcode")
					
					'response.Write agtcode
					'Response.End()
					
	
					sqlInfo="Select a.idnum as idnum, Max(a.agtname) as agtname, Max(a.rot) as rot, Max(a.email) as email, Max(b.Acode) as Acode, Max(b.bldgnum) as bldgnum" &_
					" from AgentTB a, Location b, AgentLocation c where a.idnum=c.idnum and b.Lcode=c.Lcode and b.Lcode='"& SLcode &"' and a.agtcode='"& agtcode &"' and not a.agtlevel is Null and a.Active=1 group by a.idnum "
					set rsInfo=db.Execute(sqlInfo)
					
					Set dbrecInfo=Server.CreateObject("ADODB.Recordset")
					dbrecInfo.CursorType=1
					
					dbrecInfo.Open "Select a.email as email from AgentTB a, Location b, AgentLocation c where a.idnum=c.idnum and b.Lcode=c.Lcode and b.Lcode='"& SLcode &"' and a.rot like '%GM%' and a.Active=1", db
					
					GMemail=""
					i=1
					Do until dbrecInfo.EOF
					
					  if i=1 or GMemail="" then
					    GMemail=dbrecInfo("email")
					  else
					  	if not IsNull(dbrecInfo("email")) and not IsEmpty(dbrecInfo("email")) and not dbrecInfo("email")="" then
					    	GMemail=GMemail + ";" + dbrecInfo("email")
						end if
					  end if
					
					dbrecInfo.MoveNext
					i=i+1
					Loop
					%>                    
                    <tr>
                      <td class="text12"><a href="javascript:void(0)" OnClick="SecurityForm_submitIt();return false;" target="_blank" class="text12link darkblue bold">Safety Forms
                        <input name="tfa_636" type="hidden" id="tfa_636" value="<%=rsInfo("agtname")%>">
						<input name="tfa_975" type="hidden" id="tfa_975" value="<%=rsInfo("rot")%>">
                        <input name="tfa_2" type="hidden" id="tfa_2" value="<%=rsInfo("Acode")%>">
                        <input name="tfa_781" type="hidden" id="tfa_781" value="B<%=rsInfo("bldgnum")%>">
                        <input name="tfa_1209" type="hidden" id="tfa_1209" value="<%=GMemail%>">
                      </a></td>
                    </tr>
                    </form>
                    <tr>
                      <td><img src="images/transparent.gif" width="10" height="3"></td>
                    </tr>
                </table></td>
              </tr>
          </table></td>
        </tr>
      </table></td>
    <td width="15" valign="top">&nbsp;</td>
    <td valign="top"><table width="100%" border="0" cellspacing="0" cellpadding="0">
      <tr>
        <td><img src="images/transparent.gif" width="100" height="21"></td>
      </tr>
    </table>
      <table width="100%" border="0" cellspacing="0" cellpadding="0">
        <tr>
          <td><table width="100%" border="0" cellspacing="0" cellpadding="0">
              <tr>
                <td width="170" background="images/Intranet/titles/titlebar_back.gif"><img src="images/Intranet/titles/t_reports.gif" width="170" height="27"></td>
                <td width="70" background="images/Intranet/titles/titlebar_back.gif"><div align="right"><img src="images/Intranet/titles/titlebar_end.gif" width="10" height="27"></div></td>
                <td background="images/Intranet/titles/title_line_back.gif"><div align="right"><img src="images/Intranet/titles/title_line_end.gif" width="10" height="27"></div></td>
              </tr>
          </table></td>
        </tr>
        <tr>
          <td valign="top" class="CASnet_border"><table width="98%" border="0" align="center" cellpadding="0" cellspacing="0">
              <tr>
                <td width="80" valign="top"><img src="images/Intranet/pages/icon_Reports.gif" width="80" height="85"></td>
                <td valign="top"><table width="100%" border="0" cellspacing="0" cellpadding="2">
                    <tr>
                      <td><img src="images/transparent.gif" width="10" height="3"></td>
                    </tr>
                    <%if Cint(Session("agtlevel")) >= 10 then%>
                    <tr>
                      <td class="text12"><a href="mboard/list.asp?board=RPT_BODonly" class="text12link black">BOD Only</a></td>
                    </tr>
                    <%end if%>
                    <%if Cint(Session("agtlevel")) >= 7 and Session("BODlevel")=1 then%>
                    <tr>
                      <td class="text12"><a href="mboard/list.asp?board=RPT_BOD" class="text12link black">BOD Reports</a></td>
                    </tr>
                    <tr>
                      <td class="text12"><a href="mboard/list.asp?board=RPT_Financial" class="text12link black">Financial Reports</a></td>
                    </tr>
                    <%end if%>
                    <tr>
                      <td class="text12"><a href="mboard/list.asp?board=RPT_GM" class="text12link black">GM Reports</a></td>
                    </tr>
                    <tr>
                      <td><img src="images/transparent.gif" width="10" height="3"></td>
                    </tr>
                </table></td>
              </tr>
          </table></td>
        </tr>
      </table>
      <table width="100%" border="0" cellspacing="0" cellpadding="0">
        <tr>
          <td><img src="images/transparent.gif" width="100" height="9"></td>
        </tr>
      </table>
      <%if Session("agtlevel") >= 4 then%>
      <table width="100%" border="0" cellspacing="0" cellpadding="0">
        <tr>
          <td><table width="100%" border="0" cellspacing="0" cellpadding="0">
            <tr>
              <td width="170" background="images/Intranet/titles/titlebar_back.gif"><img src="images/Intranet/titles/t_bi.gif" width="170" height="27"></td>
              <td width="70" background="images/Intranet/titles/titlebar_back.gif"><div align="right"><img src="images/Intranet/titles/titlebar_end.gif" width="10" height="27"></div></td>
              <td background="images/Intranet/titles/title_line_back.gif"><div align="right"><img src="images/Intranet/titles/title_line_end.gif" width="10" height="27"></div></td>
            </tr>
          </table></td>
        </tr>
        <tr>
          <td valign="top" class="CASnet_border"><table width="98%" border="0" align="center" cellpadding="0" cellspacing="0">
            <tr>
              <td width="80" valign="top"><img src="images/Intranet/pages/icon_BI.gif" width="80" height="85"></td>
              <td valign="top"><table width="100%" border="0" cellspacing="0" cellpadding="2">
                <tr>
                  <td><img src="images/transparent.gif" width="10" height="3"></td>
                </tr>
                <tr>
                  <td class="text12"><a href="Reports/default.asp" class="text12link black">HR / Flash Reports</a></td>
                </tr>
                <tr>
                  <td class="text12">KPI Reports</td>
                </tr>
                <tr>
                  <td class="text12">&nbsp;</td>
                </tr>
                
                <%if Session("agtlevel") >= 4 and rs("CFP") = 1 then%>
                <tr>
                  <td class="text12"><a href="CFPAgentManaging/View/CFPTool.aspx" class="text12link skyblue">CFP Tool</a></td>
                </tr>
                <% end if%>
                
                <tr>
                  <td><img src="images/transparent.gif" width="10" height="3"></td>
                </tr>
              </table></td>
            </tr>
          </table></td>
        </tr>
      </table>
      <%else%>
&nbsp;
<%end if%>
<table width="100%" border="0" cellspacing="0" cellpadding="0">
        <tr>
          <td><img src="images/transparent.gif" width="100" height="9"></td>
        </tr>
      </table>
      <table width="100%" border="0" cellspacing="0" cellpadding="0">
        <tr>
          <td><table width="100%" border="0" cellspacing="0" cellpadding="0">
              <tr>
                <td width="170" background="images/Intranet/titles/titlebar_back.gif"><img src="images/Intranet/titles/t_forms.gif" width="170" height="27"></td>
                <td width="70" background="images/Intranet/titles/titlebar_back.gif"><div align="right"><img src="images/Intranet/titles/titlebar_end.gif" width="10" height="27"></div></td>
                <td background="images/Intranet/titles/title_line_back.gif"><div align="right"><img src="images/Intranet/titles/title_line_end.gif" width="10" height="27"></div></td>
              </tr>
          </table></td>
        </tr>
        <tr>
          <td valign="top" class="CASnet_border"><table width="98%" border="0" align="center" cellpadding="0" cellspacing="0">
              <tr>
                <td width="80" valign="top"><img src="images/Intranet/pages/icon_form.gif" width="80" height="85"></td>
                <td><table width="100%" border="0" cellspacing="0" cellpadding="2">
                    <tr>
                      <td><img src="images/transparent.gif" width="10" height="3"></td>
                    </tr>
                    <tr>
                      <td class="text12"><a href="mboard/list.asp?board=FT_HR" class="text12link black">HR Forms</a></td>
                    </tr>
                    <tr>
                      <td class="text12"><a href="mboard/list.asp?board=FT_OP" class="text12link black">Operation Forms</a></td>
                    </tr>
                    <tr>
                      <td class="text12"><a href="mboard/list.asp?board=FT_Acct" class="text12link black">Accounting Forms</a></td>
                    </tr>
                    <tr>
                      <td class="text12"><a href="mboard/list.asp?board=FT_Sales" class="text12link black">Sales Forms</a></td>
                    </tr>
                    <tr>
                      <td class="text12"><a href="mboard/list.asp?board=FT_Other" class="text12link black">Other Forms</a></td>
                    </tr>
                    <tr>
                      <td><img src="images/transparent.gif" width="10" height="3"></td>
                    </tr>
                </table></td>
              </tr>
          </table></td>
        </tr>
      </table>
      

          <!--security bulleting-->
          <table width="100%" border="0" cellspacing="0" cellpadding="0">
        <tr>
          <td><img src="images/transparent.gif" width="100" height="9"></td>
        </tr>
      </table>
      <table width="100%" border="0" cellspacing="0" cellpadding="0">
        <tr>
          <td><table width="100%" border="0" cellspacing="0" cellpadding="0">
              <tr>
                <td width="170" background="images/Intranet/titles/titlebar_back.gif"><img src="images/Intranet/titles/t_security.gif" width="170" height="27"></td>
                <td width="70" background="images/Intranet/titles/titlebar_back.gif"><div align="right"><img src="images/Intranet/titles/titlebar_end.gif" width="10" height="27"></div></td>
                <td background="images/Intranet/titles/title_line_back.gif"><div align="right"><img src="images/Intranet/titles/title_line_end.gif" width="10" height="27"></div></td>
              </tr>
          </table></td>
        </tr>
        <tr>
          <td valign="top" class="CASnet_border"><table width="98%" border="0" align="center" cellpadding="0" cellspacing="0">
              <tr>
                <td width="80" valign="top">&nbsp;</td>
                <td><table width="100%" border="0" cellspacing="0" cellpadding="2">
                    <tr>
                      <td><img src="images/transparent.gif" width="10" height="3"></td>
                    </tr>
                    <tr>
                      <td class="text12"><a href="mboard/list.asp?board=SC_Manual" class="text12link black">Manual</a></td>
                    </tr>
                    <tr>
                      <td class="text12"><a href="mboard/list.asp?board=SC_Bulletin" class="text12link black">Security Bulletin</a></td>
                    </tr>
                    <tr>
                      <td class="text12"><a href="/SecurityReport" class="text12link black">Security Report </a></td>
                    </tr>
                    <tr>
                      <td><img src="images/transparent.gif" width="10" height="3"></td>
                    </tr>
                </table></td>
              </tr>
          </table></td>
        </tr>
      </table>      
      
      </td>
  </tr>
</table>
<table width="990" border="0" cellspacing="0" cellpadding="0">
  <tr>
    <td height="10">&nbsp;</td>
  </tr>
</table>
<table width="990" border="0" cellspacing="0" cellpadding="0">
  <tr>
    <td width="10">&nbsp;</td>
    <td valign="top"><table width="100%" border="0" cellspacing="0" cellpadding="0">
        <tr>
          <td><img src="images/transparent.gif" width="100" height="9"></td>
        </tr>
      </table>
        <table width="100%" border="0" cellspacing="0" cellpadding="0">
          <tr>
            <td><table width="100%" border="0" cellspacing="0" cellpadding="0">
                <tr>
                  <td width="170" background="images/Intranet/titles/titlebar_back.gif"><img src="images/Intranet/titles/t_contract.gif" width="170" height="27"></td>
                  <td width="70" background="images/Intranet/titles/titlebar_back.gif"><div align="right"><img src="images/Intranet/titles/titlebar_end.gif" width="10" height="27"></div></td>
                  <td background="images/Intranet/titles/title_line_back.gif"><div align="right"><img src="images/Intranet/titles/title_line_end.gif" width="10" height="27"></div></td>
                </tr>
            </table></td>
          </tr>
          <tr>
            <td valign="top" class="CASnet_border"><table width="98%" border="0" align="center" cellpadding="0" cellspacing="0">
                <tr>
                  <td width="80" valign="top"><img src="images/Intranet/pages/icon_Contract.gif" width="80" height="85"></td>
                  <td>
                  <table width="100%" border="0" cellspacing="0" cellpadding="2">
                  	  <tr>
                        <td><img src="images/transparent.gif" width="10" height="3"></td>
                      </tr>
                      <tr>
                        <td class="text12"><a href="mboard/list.asp?board=CM_Customer" class="text12link black">Customer Contract</a></td>
                      </tr>
                      <tr>
                        <td class="text12"><a href="mboard/list.asp?board=CM_Supplier" class="text12link black">Supplier Contract</a></td>
                      </tr>
                      <tr>
                        <td class="text12"><a href="mboard/list.asp?board=CM_Equipment" class="text12link black">Equipment Capital Lease</a></td>
                      </tr>
                      <tr>
                        <td class="text12"><a href="mboard/list.asp?board=CM_OPLease" class="text12link black">Operating Lease</a></td>
                      </tr>
                      <tr>
                        <td class="text12"><a href="mboard/list.asp?board=CM_Facility" class="text12link black">Facility Lease contract</a></td>
                      </tr>
                      <tr>
                        <td class="text12"><a href="mboard/list.asp?board=CM_Financial" class="text12link black">Financial contract</a></td>
                      </tr>
                      <tr>
                        <td class="text12"><a href="mboard/list.asp?board=CM_Misc" class="text12link black">Miscellaneous contract</a></td>
                      </tr>
                      <tr>
                        <td><img src="images/transparent.gif" width="10" height="3"></td>
                      </tr>
                  </table>
                  </td>
                </tr>
            </table></td>
          </tr>
    </table></td>
    <td width="10" valign="top">&nbsp;</td>
    <td valign="top"><table width="100%" border="0" cellspacing="0" cellpadding="0">
      <tr>
        <td><img src="images/transparent.gif" width="100" height="9"></td>
      </tr>
    </table>
      <table width="100%" border="0" cellspacing="0" cellpadding="0">
        <tr>
          <td><table width="100%" border="0" cellspacing="0" cellpadding="0">
              <tr>
                <td width="170" background="images/Intranet/titles/titlebar_back.gif"><img src="images/Intranet/titles/t_prospect.gif" width="170" height="27"></td>
                <td width="70" background="images/Intranet/titles/titlebar_back.gif"><div align="right"><img src="images/Intranet/titles/titlebar_end.gif" width="10" height="27"></div></td>
                <td background="images/Intranet/titles/title_line_back.gif"><div align="right"><img src="images/Intranet/titles/title_line_end.gif" width="10" height="27"></div></td>
              </tr>
          </table></td>
        </tr>
        <tr>
          <td valign="top" class="CASnet_border"><table width="98%" border="0" align="center" cellpadding="0" cellspacing="0">
              <tr>
                <td width="80" valign="top"><img src="images/Intranet/pages/icon_Prospect.gif" width="80" height="85"></td>
                <td><table width="100%" border="0" cellspacing="0" cellpadding="2">
                    <tr>
                      <td><img src="images/transparent.gif" width="10" height="3"></td>
                    </tr>
                    <tr>
                      <td class="text12"><a href="mboard/list.asp?board=PS_proforma" class="text12link black">Pro-Forma</a></td>
                    </tr>
                    <tr>
                      <td class="text12"><a href="mboard/list.asp?board=PS_RFP" class="text12link black">RFP / RFI</a></td>
                    </tr>
                    <tr>
                      <td class="text12"><a href="mboard/list.asp?board=PS_Present" class="text12link black">Presentation</a></td>
                    </tr>
                    <tr>
                      <td class="text12"><a href="mboard/list.asp?board=PS_Proposal" class="text12link black">Proposals</a></td>
                    </tr>
                    <tr>
                      <td class="text12"><a href="mboard/list.asp?board=PS_Ref" class="text12link black">Reference / Company profile</a></td>
                    </tr>
                    <tr>
                      <td class="text12"><a href="mboard/list.asp?board=PS_Material" class="text12link black">Marketing Materials</a></td>
                    </tr>
                    <tr>
                      <td class="text12">Sales Prospect Management</td>
                    </tr>
                    <tr>
                      <td><img src="images/transparent.gif" width="10" height="3"></td>
                    </tr>
                </table></td>
              </tr>
          </table></td>
        </tr>
      </table></td>
    <td width="10" valign="top">&nbsp;</td>
    <td valign="top"><table width="100%" border="0" cellspacing="0" cellpadding="0">
      <tr>
        <td><img src="images/transparent.gif" width="100" height="9"></td>
        </tr>
      </table>
      <table width="100%" border="0" cellspacing="0" cellpadding="0">
        <tr>
          <td><table width="100%" border="0" cellspacing="0" cellpadding="0">
            <tr>
              <td width="170" background="images/Intranet/titles/titlebar_back.gif"><img src="images/Intranet/titles/t_OP_tools.gif" width="170" height="27"></td>
              <td width="70" background="images/Intranet/titles/titlebar_back.gif"><div align="right"><img src="images/Intranet/titles/titlebar_end.gif" width="10" height="27"></div></td>
              <td background="images/Intranet/titles/title_line_back.gif"><div align="right"><img src="images/Intranet/titles/title_line_end.gif" width="10" height="27"></div></td>
              </tr>
          </table></td>
        </tr>
        <tr>
          <td valign="top" class="CASnet_border"><table width="98%" border="0" align="center" cellpadding="0" cellspacing="0">
            <tr>
              <td colspan="4"><img src="images/transparent.gif" width="100" height="8"></td>
              </tr>
            <tr>
              <td colspan="2" valign="top" class="text12 bold gray33">&nbsp;&nbsp;&nbsp;&nbsp;Applicant Tracking</td>
              <td colspan="2" valign="top" class="text12 bold gray33">&nbsp;&nbsp;&nbsp;&nbsp;Cash Collection</td>
              </tr>
            <tr>
              <td width="50" align="center" valign="top"><img src="images/Intranet/pages/icon_Applicant.gif" width="40" height="45"></td>
              <td valign="top"><table width="100%" border="0" cellspacing="0" cellpadding="2">
                <tr>
                  <td><img src="images/transparent.gif" width="10" height="3"></td>
                  </tr>
                <tr>
                  <td class="text12"><a href="Applicant/loc.asp" target="_blank" class="text12link black">New Application</a></td>
                  </tr>
                <tr>
                  <td class="text12"><a href="Applicant/list.asp" class="text12link black">Application Manage</a></td>
                  </tr>
                <tr>
                  <td class="text12"><a href="empForm/list.asp" class="text12link black">Old List Manage</a></td>
                  </tr>
                <tr>
                  <td class="text12"><a href="mboard/list.asp?board=HR_Job" class="text12link black">Job Descriptions</a></td>
                  </tr>
                <tr>
                  <td><img src="images/transparent.gif" width="10" height="3"></td>
                  </tr>
                </table></td>
              <td width="50" align="center" valign="top"><img src="images/Intranet/pages/icon_CC.gif" width="40" height="45"></td>
              <td valign="top"><table width="100%" border="0" cellspacing="0" cellpadding="2">
                <tr>
                  <td><img src="images/transparent.gif" width="10" height="3"></td>
                </tr>
                <tr>
                  <td class="text12"><a href="default2.asp" class="text12link black">Cash Collection</a></td>
                </tr>
                <tr>
                  <td class="text12"><a href="/JournalEntry" class="text12link black">New Journalizing</a></td>
                </tr>
                <tr>
                  <td class="text12"><a href="/ePicJournalEntry" class="text12link black"> ePic Journalizing</a> by Carrier</td>
                </tr>
                <tr>
                  <td class="text12"><a href="/ePicJournalEntry/Home/ByUser" class="text12link black"> ePic Journalizing</a> by User</td>
                </tr>
                <tr>
                <tr>
                  <td><img src="images/transparent.gif" width="10" height="4"></td>
                </tr>
              </table></td>
            </tr>
            <tr>
            <tr>
              <td colspan="2" valign="top" class="text12 bold gray33">&nbsp;&nbsp;&nbsp;&nbsp;Labor Tracking</td>
              <td colspan="2" valign="top" class="text12 bold gray33">&nbsp;&nbsp;&nbsp;&nbsp;Billing &amp; Report</td>
            </tr>
            <tr>
              <td align="center" valign="top"><img src="images/Intranet/pages/icon_Labor.gif" width="40"></td>
              <td valign="top"><table width="100%" border="0" cellspacing="0" cellpadding="2">
                <tr>
                  <td><img src="images/transparent.gif" width="10" height="3"></td>
                </tr>
                <tr>
                  <!--<td class="text12"><a href="/LaborTracking/WWT/List" class="text12link black">Weekly Workforce Tracking</a></td>-->
                    <td class="text12"><a href="/LaborTracking/ADPPayrollAnalysis" class="text12link black">ADP Payroll Analysis</a></td>
                </tr>
                <tr>
                  <td class="text12"><a href="/LaborTracking/WWT/PayrollAnalysis" class="text12link black">Payroll Analysis</a></td>
                </tr>
                <tr>
                  <td><img src="images/transparent.gif" width="10" height="3"></td>
                </tr>
              </table></td>
              <td align="center" valign="top"><img src="images/Intranet/pages/icon_billing.gif" width="40"></td>
              <td valign="top"><table width="100%" border="0" cellspacing="0" cellpadding="2">
                <tr>
                  <td><img src="images/transparent.gif" width="10" height="3"></td>
                </tr>
                <tr>
                  <td class="text12"><a href="Reports/flash/flash0.asp" class="text12link black">Flash (Tonnage) report</a></td>
                </tr>
                <tr>
                  <td class="text12"><a href="Bill_Report/index.asp" class="text12link black">Billing Stats</a></td>
                </tr>
                <tr>
                  <td><img src="images/transparent.gif" width="10" height="4"></td>
                </tr>
              </table></td>
            </tr>
          </table></td>
        </tr>
      </table>      <table width="100%" border="0" cellspacing="0" cellpadding="0">
        <tr>
          <td><img src="images/transparent.gif" width="100" height="9"></td>
        </tr>
      </table></td>
  </tr>
</table>
<table width="990" border="0" cellspacing="0" cellpadding="0">
  <tr>
    <td height="10">&nbsp;</td>
  </tr>
</table>
<table width="990" border="0" cellspacing="0" cellpadding="0">
  <tr>
    <td width="10">&nbsp;</td>
    <td width="240" valign="top"><table width="100%" border="0" cellspacing="0" cellpadding="0">
      <tr>
        <td><img src="images/transparent.gif" width="100" height="9"></td>
      </tr>
    </table>
      <table width="100%" border="0" cellspacing="0" cellpadding="0">
        <tr>
          <td><table width="100%" border="0" cellspacing="0" cellpadding="0">
              <tr>
                <td width="170" background="images/Intranet/titles/titlebar_back.gif"><img src="images/Intranet/titles/t_survey.gif" width="170" height="27"></td>
                <td width="70" background="images/Intranet/titles/titlebar_back.gif"><div align="right"><img src="images/Intranet/titles/titlebar_end.gif" width="10" height="27"></div></td>
                <td background="images/Intranet/titles/title_line_back.gif"><div align="right"><img src="images/Intranet/titles/title_line_end.gif" width="10" height="27"></div></td>
              </tr>
          </table></td>
        </tr>
        <tr>
          <td valign="top" class="CASnet_border"><table width="98%" border="0" align="center" cellpadding="0" cellspacing="0">
              <tr>
                <td width="80" valign="top"><img src="images/Intranet/pages/icon_Survey.gif" width="80" height="85"></td>
                <td valign="top"><table width="100%" border="0" cellspacing="0" cellpadding="2">
                    <tr>
                      <td><img src="images/transparent.gif" width="10" height="3"></td>
                    </tr>
                    <tr>
                      <td class="text12">Design New  Survey Form</td>
                    </tr>
                    <tr>
                      <td class="text12">Review Survey</td>
                    </tr>
                    <tr>
                      <td class="text12">Survey Report</td>
                    </tr>
                    <tr>
                      <td><img src="images/transparent.gif" width="10" height="3"></td>
                    </tr>
                </table></td>
              </tr>
          </table></td>
        </tr>
      </table></td>
    <td width="10" valign="top">&nbsp;</td>
    <td width="240" valign="top"><table width="100%" border="0" cellspacing="0" cellpadding="0">
      <tr>
        <td><img src="images/transparent.gif" width="100" height="9"></td>
      </tr>
    </table>
      <table width="100%" border="0" cellspacing="0" cellpadding="0">
        <tr>
          <td><table width="100%" border="0" cellspacing="0" cellpadding="0">
              <tr>
                <td width="170" background="images/Intranet/titles/titlebar_back.gif" class="text12 bold"><img src="images/Intranet/titles/t_pubweb.gif" width="170" height="27"></td>
                <td width="70" background="images/Intranet/titles/titlebar_back.gif"><div align="right"><img src="images/Intranet/titles/titlebar_end.gif" width="10" height="27"></div></td>
                <td background="images/Intranet/titles/title_line_back.gif"><div align="right"><img src="images/Intranet/titles/title_line_end.gif" width="10" height="27"></div></td>
              </tr>
          </table></td>
        </tr>
        <tr>
          <td valign="top" class="CASnet_border"><table width="98%" border="0" align="center" cellpadding="0" cellspacing="0">
              <tr>
                <td width="80" valign="top"><img src="images/Intranet/pages/icon_PubWeb.gif" width="80" height="85"></td>
                <td valign="top"><table width="100%" border="0" cellspacing="0" cellpadding="2">
                    <tr>
                      <td><img src="images/transparent.gif" width="10" height="3"></td>
                    </tr>
                    <tr>
                      <td class="text12"><a href="pub_board/list.asp?board=News" class="text12link black">Company News</a></td>
                    </tr>
                    <tr>
                      <td class="text12">Contact Information</td>
                    </tr>
                    <tr>
                      <td><img src="images/transparent.gif" width="10" height="3"></td>
                    </tr>
                </table></td>
              </tr>
          </table></td>
        </tr>
      </table></td>
    <td width="10" valign="top">&nbsp;</td>
    <td width="240" valign="top"><table width="100%" border="0" cellspacing="0" cellpadding="0">
      <tr>
        <td><img src="images/transparent.gif" width="100" height="9"></td>
      </tr>
    </table>
      <table width="100%" border="0" cellspacing="0" cellpadding="0">
        <tr>
          <td><table width="100%" border="0" cellspacing="0" cellpadding="0">
            <tr>
              <td width="170" height="27" align="center" background="images/Intranet/titles/titlebar_back.gif" class="text12 bold">Manage Epic</td>
              <td width="70" background="images/Intranet/titles/titlebar_back.gif"><div align="right"><img src="images/Intranet/titles/titlebar_end.gif" width="10" height="27"></div></td>
              <td background="images/Intranet/titles/title_line_back.gif"><div align="right"><img src="images/Intranet/titles/title_line_end.gif" width="10" height="27"></div></td>
            </tr>
          </table></td>
        </tr>
        <tr>
          <td valign="top" class="CASnet_border"><table width="98%" border="0" align="center" cellpadding="0" cellspacing="0">
            <tr>
              <td width="20" valign="top">&nbsp;</td>
              <td valign="top"><table width="100%" border="0" cellspacing="0" cellpadding="2">
                <tr>
                  <td><img src="images/transparent.gif" width="10" height="3"></td>
                </tr>
                <tr>
                  <td class="text12"><a href="ePic/ePic_default.asp" class="text12link black">Manage Epic</a></td>
                </tr>
                <tr>
                  <td class="text12"><a href="App_version/list.asp" class="text12link black">Manage App Version</a></td>
                </tr>
                <tr>
                  <td class="text12">&nbsp;</td>
                </tr>
                <tr>
                  <td><img src="images/transparent.gif" width="10" height="3"></td>
                </tr>
              </table></td>
            </tr>
          </table></td>
        </tr>
    </table>
      <table width="100%" border="0" cellspacing="0" cellpadding="0">
        <tr>
          <td><table width="100%" border="0" cellspacing="0" cellpadding="0">
            <tr>
              <td width="170" height="27" align="center" background="images/Intranet/titles/titlebar_back.gif" class="text12 bold">IATA Message Control</td>
              <td width="70" background="images/Intranet/titles/titlebar_back.gif"><div align="right"><img src="images/Intranet/titles/titlebar_end.gif" width="10" height="27"></div></td>
              <td background="images/Intranet/titles/title_line_back.gif"><div align="right"><img src="images/Intranet/titles/title_line_end.gif" width="10" height="27"></div></td>
            </tr>
          </table></td>
        </tr>
        <tr>
          <td valign="top" class="CASnet_border"><table width="98%" border="0" align="center" cellpadding="0" cellspacing="0">
            <tr>
              <td width="20" valign="top">&nbsp;</td>
              <td valign="top"><table width="100%" border="0" cellspacing="0" cellpadding="2">
                <tr>
                  <td><img src="images/transparent.gif" width="10" height="3"></td>
                </tr>
                <%if Cint(Session("agtlevel"))=8 then%>
                <tr>
                  <td class="text12"><a href="IATA_MSG/Verisions/ediMaster001.asp" class="text12link black">Message Versions &amp; Addresses</a></td>
                </tr>
                <%end if%>
                <!--<tr>
                  <td class="text12"><a href="IATA_MSG/History/list.asp" class="text12link black">Message History & Manage</a></td>
                </tr>-->
                  <tr>
                  <td class="text12"><a href="/MessageHistory/Home" class="text12link black">Message History & Manage</a></td>
                </tr>
                 <tr>
                  <td class="text12"><a href="SITAAddressMgmt/" class="text12link black">SITA Address Manager</a></td>
                </tr>
                <tr>
                  <td class="text12">&nbsp;</td>
                </tr>
                <tr>
                  <td><img src="images/transparent.gif" width="10" height="3"></td>
                </tr>
              </table></td>
            </tr>
          </table></td>
        </tr>
    </table></td>
    <td width="10" valign="top">&nbsp;</td>
    <td valign="top"><table width="100%" border="0" cellspacing="0" cellpadding="0">
      <tr>
        <td><img src="images/transparent.gif" width="100" height="9"></td>
      </tr>
    </table>
      <table width="100%" border="0" cellspacing="0" cellpadding="0">
        <tr>
          <td><table width="100%" border="0" cellspacing="0" cellpadding="0">
            <tr>
              <td width="170" height="27" align="center" background="images/Intranet/titles/titlebar_back.gif" class="text12 bold">Manage Airline Portal</td>
              <td width="70" background="images/Intranet/titles/titlebar_back.gif"><div align="right"><img src="images/Intranet/titles/titlebar_end.gif" width="10" height="27"></div></td>
              <td background="images/Intranet/titles/title_line_back.gif"><div align="right"><img src="images/Intranet/titles/title_line_end.gif" width="10" height="27"></div></td>
            </tr>
          </table></td>
        </tr>
        <tr>
          <td valign="top" class="CASnet_border"><table width="98%" border="0" align="center" cellpadding="0" cellspacing="0">
            <tr>
              <td width="20" valign="top">&nbsp;</td>
              <td valign="top"><table width="100%" border="0" cellspacing="0" cellpadding="2">
                <tr>
                  <td><img src="images/transparent.gif" width="10" height="3"></td>
                </tr>
                <tr>
                  <td class="text12"><a href="AL/AL_default.asp" class="text12link black">Manage Airline Portal</a></td>
                </tr>
                <tr>
                  <td class="text12">&nbsp;</td>
                </tr>
                <tr>
                  <td><img src="images/transparent.gif" width="10" height="3"></td>
                </tr>
              </table></td>
            </tr>
          </table></td>
        </tr>
    </table>
      <%if Cint(Session("agtlevel")) >= 8 then%>
      <table width="100%" border="0" cellspacing="0" cellpadding="0">
        <tr>
          <td><table width="100%" border="0" cellspacing="0" cellpadding="0">
            <tr>
              <td width="170" height="27" align="center" background="images/Intranet/titles/titlebar_back.gif" class="text12 bold">Test Epic 1.1</td>
              <td width="70" background="images/Intranet/titles/titlebar_back.gif"><div align="right"><img src="images/Intranet/titles/titlebar_end.gif" width="10" height="27"></div></td>
              <td background="images/Intranet/titles/title_line_back.gif"><div align="right"><img src="images/Intranet/titles/title_line_end.gif" width="10" height="27"></div></td>
            </tr>
          </table></td>
        </tr>
        <tr>
          <td valign="top" class="CASnet_border"><table width="98%" border="0" align="center" cellpadding="0" cellspacing="0">
            <tr>
              <td width="20" valign="top">&nbsp;</td>
              <td valign="top"><table width="100%" border="0" cellspacing="0" cellpadding="2">
                <tr>
                  <td><img src="images/transparent.gif" width="10" height="3"></td>
                </tr>
                <tr>
                  <td class="text12"><a href="default4.asp" target="_blank" class="text12link black">New Dashboard</a></td>
                </tr>
                <tr>
                  <td><a href="http://test.casusa.com/common/FSN_default.asp" target="_blank" class="text12link black">FSN Test tool (with Test DB)</a></td>
                </tr>
              </table></td>
            </tr>
          </table></td>
        </tr>
      </table>
    <%end if%></td>
  </tr>
</table>
<table width="990" border="0" cellspacing="0" cellpadding="0">
  <tr>
    <td width="10">&nbsp;</td>
    <td width="315" valign="top"><table width="100%" border="0" cellspacing="0" cellpadding="0">
      <tr>
        <td><img src="images/transparent.gif" width="100" height="9"></td>
      </tr>
    </table>
        </td>
    <td width="15" valign="top">&nbsp;</td>
    <td width="320" valign="top"><table width="100%" border="0" cellspacing="0" cellpadding="0">
      <tr>
        <td><img src="images/transparent.gif" width="100" height="9"></td>
      </tr>
    </table></td>
    <td width="15" valign="top">&nbsp;</td>
    <td width="315" valign="top"><table width="100%" border="0" cellspacing="0" cellpadding="0">
        <tr>
          <td><img src="images/transparent.gif" width="100" height="9"></td>
        </tr>
      </table>
    </td>
  </tr>
</table>
<br><!-- #BeginLibraryItem "/Library/footer.lbi" --><table width="100%" border="0" cellspacing="0" cellpadding="0">
  <tr>
    <td width="900" background="images/pages/footer_back1.gif"><table width="100%" border="0" cellspacing="0" cellpadding="0">
      <tr>
        <td width="541"><img src="images/pages/footerimg1.gif" width="541" height="41"></td>
        <td>&nbsp;</td>
      </tr>
    </table></td>
    <td background="images/pages/footer_back1.gif">&nbsp;</td>
  </tr>
</table>
<table width="100%" border="0" cellspacing="0" cellpadding="0">
  <tr>
    <td width="900" background="images/pages/footer_back2.gif"><table width="900" border="0" cellpadding="0" cellspacing="0">
      <tr>
        <td height="61" background="images/pages/footer_back2.gif"><table width="95%" border="0" cellspacing="0" cellpadding="3">
            <tr>
              <td class="copyright"><div align="right">Copyright &copy; 2008 casnet.casusa.com, All rights are reserved</div></td>
            </tr>
        </table></td>
      </tr>
    </table></td>
    <td background="images/pages/footer_back2.gif">&nbsp;</td>
  </tr>
</table><!-- #EndLibraryItem --><p>&nbsp;</p>
</body>
</html>
