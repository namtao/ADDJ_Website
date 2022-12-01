<%@ Page Title="" Language="C#" AutoEventWireup="true" CodeBehind="ErrorSystem.aspx.cs" Inherits="Website.HTHTKT.Notify.NotValidPass" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>Đăng nhập hệ thống</title>
    <% Response.Write(string.Format("<link media=\"screen\" rel=\"stylesheet\" href=\"{0}?ver={1}\" />", ResolveUrl("~/css/reset.css"), Website.AppCode.Common.Ver)); %>
    <% Response.Write(string.Format("<link media=\"screen\" rel=\"stylesheet\" href=\"{0}?ver={1}\" />", ResolveUrl("~/css/style.css"), Website.AppCode.Common.Ver)); %>
    <% Response.Write(string.Format("<script src=\"{0}?ver={1}\" type=\"text/javascript\"></script>", ResolveClientUrl("~/Scripts/jquery-3.3.1.min.js"), Website.AppCode.Common.Ver)); %>
    <% Response.Write(string.Format("<link href=\"{0}?ver={1}\" type=\"text/css\" rel=\"stylesheet\" />", ResolveClientUrl("~/Content/bootstrap.min.css"), Website.AppCode.Common.Ver)); %>
    <% Response.Write(string.Format("<link href=\"{0}?ver={1}\" type=\"text/css\" rel=\"stylesheet\" />", ResolveClientUrl("~/Content/font-awesome.min.css"), Website.AppCode.Common.Ver)); %>
    <% Response.Write(string.Format("<script src=\"{0}?ver={1}\" type=\"text/javascript\"></script>", ResolveClientUrl("~/Scripts/bootstrap.min.js"), Website.AppCode.Common.Ver)); %>



       <style type="text/css">
        #banner {
            height: 90px;
            position: relative;
        }

                #banner h1 {
                color: #fff;
                font-size: 23px;
                padding: 40px 0 0 210px;
                padding-left: 28% !important;
            }

            #banner .userarea {
                position: absolute;
                right: 10px;
                top: 10px;
                color: #fff;
            }

                #banner .userarea span {
                    font-weight: bold;
                }

                #banner .userarea a {
                    color: #fff;
                }

        #boxlogin {
            margin-left: -225px;
        }

            #boxlogin .aws {
                visibility: visible !important;
                color: red;
                font-weight: normal;
            }
    </style>
    <script type="text/javascript">
        function cal() {
            var wh = $(window).height();
            var header = $("#banner").height();
            var footer = $("#footer").height();
            var shadow = $(".shadow").height();
            var content = wh - header - footer - shadow;
            $("#contain").height(content);
        }
        $(window).on({
            load: function () {
                cal();
            },
            resize: function () { cal(); }
        });
    </script>
</head>
<body>
    <form runat="server">
        <div id="banner">
            <div class="logo">
            </div>
            <div class="company">
                <h1 style="margin-top: 0px !important">CÔNG TY CỔ PHẦN CÔNG NGHỆ HÀNH CHÍNH ADDJ</h1>
            </div>
        </div>
        <div class="shadow">
        </div>

        <div class="container" id="contain">
            <div class="row" style="padding-top: 60px;">
                <div class="col-md-4 col-md-offset-4">
                    <div class="panel panel-default">
                        <div class="panel-heading">
                            <h3 class="panel-title" style="text-align: center">Lỗi hệ thống</h3>
                        </div>
                        <div class="panel-body">
                            <div class="alert alert-danger">
                                <strong>Chú ý!</strong> Lỗi hệ thống, vui lòng thử lại sau.
                            </div>
                             <div style="text-align: center">
                                <a href="javascript:history.back()" class="btn btn-primary"><span class="glyphicon glyphicon-circle-arrow-left" aria-hidden="true"></span>&nbsp;Quay về</a>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div id="footer">
            <div class="p8">
                <p>
                    Copyright 2017 by VNPTNET
                </p>
                <p>
                    Thiết kế bởi VNPTNET
                </p>
            </div>
        </div>
    </form>
</body>
</html>
