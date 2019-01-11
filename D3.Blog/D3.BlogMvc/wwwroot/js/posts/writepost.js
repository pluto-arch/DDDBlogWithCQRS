
Vue.prototype.$http = axios;
var app = new Vue({
    el: '#app',
    data: {
        message: 'Collapsible Sidebar Using Bootstrap 4!',
        isDown: false,
        isSearch: false,
        editorHtml: "",
        articleTitle: "",
        articleTag: "",
        blogType: "",
        postType: "",
        vaildataMessage: "",
        isShowVail: false,
        editor: Object,
        mdeditor: Object
    },
    mounted: function () {
        this.$nextTick(function () {
            this.setNavBarMenu();
            this.setBaseCtl();
        });
    },
    methods: {
        setBaseCtl: function () {
            var v = this;
            toastr.options.positionClass = 'toast-bottom-center';//信息提示

            /* 富文本编辑器 */
            var E = window.wangEditor;
            var editor = new E('#divDemo');
            editor.customConfig.uploadImgShowBase64 = true;
            editor.customConfig.linkImgCallback = function (url) {
                alert("插入网络图片的回调"); // url 即插入图片的地址
            }
            editor.create();
            v.editor = editor;

            /* markdown编辑器 */
            var testEditor = editormd("test-editormd",
                {
                    width: "100%",
                    height: 640,
                    path: "../../lib/markdown/lib/",
                    saveHTMLToTextarea: true
                });
            v.mdeditor = testEditor;
        }
        , setNavBarMenu: function () {
            var v = this;
            $('#usr-opt').hide();
            $('body').click(function (event) {
                if ($(event.target).data("flag") != 1) {
                    v.isDown = false;
                    v.isSearch = false;
                }
            });//点击其余地方收起

            $(window).scroll(function () {
                var topp = $(document).scrollTop();
                $('#search-posts').slideUp();
                $('#usr-opt').slideUp();
                v.isSearch = false;
                v.isDown = false;
                if (topp > 1000) {
                    $('.scroll-to-top').show();
                } else {
                    $('.scroll-to-top').hide();
                }
            });//回到顶部
            $('.scroll-to-top').click(function () {
                $('html ,body').animate({ scrollTop: 0 }, 300);
                return false;
            });

            $('[data-toggle="tooltip"]').tooltip();
            $('#dismiss, .overlay').on('click', function () {
                $('#sidebar').removeClass('active');
                $('.overlay').removeClass('active');
                $('body').css('overflow', 'auto');
            });
            $('#sidebarCollapse').on('click', function () {
                $('body').css('overflow', 'hidden');
                $('#sidebar').addClass('active');
                $('.overlay').addClass('active');
                $('.collapse.in').toggleClass('in');
                $('a[aria-expanded=true]').attr('aria-expanded', 'false');
            });
            $('#menu li a').click(function () {
                var f = this;
                $('#menu li a').each(function () {
                    this.className = this == f ? 'nav-link clickactive' : 'nav-link'
                });
            });
            $('#side-menu li').click(function () {
                var f = this;
                $('#side-menu li').each(function () {
                    this.className = this == f ? 'active' : ''
                });
            });//侧边菜单

            $('#aside-content .write-aside').click(function () {
                var clickel = this;
                $('#aside-content .write-aside').removeClass('checked');
                $(clickel).addClass('checked');
            });//写博客页面侧边菜单

            $('#tokenfield').tokenfield({ minWidth: 250 });//标签输入

        }
        , Login: function () {
            var v = this;
            v.$http.post('/Account/LoginAsync', $('#form1').serialize())
                .then(res => {
                    if (res.data.length > 0) {
                        console.log(res.data);
                        alert(res.data);
                    } else {
                        window.location.reload();//登录成功，刷新当前页面
                    }
                }).catch(error => {
                    console.log(error);
                });
        }
        , dropDownOpt: function () {
            //个人操作下拉
            var v = this;
            v.isDown = !v.isDown;
        }
        , optSearch: function () {
            this.isSearch = !this.isSearch;
        }
        , showPostDetails: function (value, event) {
            window.location.href = "/Post/PostDetails?id=" + value;
        }
        , publish: function (flag, event) {
            var v = this;
            console.log(v.mdeditor.getHTML());//md编辑器的内容
            console.log(v.editor.txt.html());//富文本编辑器的内容
            switch (flag) {
                case 1:
                    //发布
                    v.publishArticle(1);
                    break;
                case 2:
                    //保存
                    v.publishArticle(2);
                    break;
                case 3:
                    //预览
                    break;
            }
        }
        , publishArticle: function (flag) {
            var v = this;
            var isSubmit = v.setAndVaildata();
            if (isSubmit) {
                v.$http.post('/Post/WritePost', $('#formPost').serialize())
                    .then(res => {
                        if (res.data.length > 0) {
                           //有错误
                            toastr.warning('出现错误，请稍后重试');
                        } else {
                           //无错误
                            toastr.info('发布成功');
                        }
                    }).catch(error => {
                        toastr.warning('出现错误，请稍后重试');
                    });
            } else {
                
            }
        }
        , setAndVaildata: function () {
            var v = this;
            var isVail = true;
            var style = $('#test-editormd').css('display');
            var txtcontent_fu = v.editor.txt.html();//富文本编辑器内容
            var txtcontent_md = v.mdeditor.getPreviewedHTML();//md编辑器内容
            var txtmd = v.mdeditor.getMarkdown();//获取markdown内容
            if (style == 'none') {
                //使用的是富文本编辑器
                if (txtcontent_fu.trim() != "") {
                    $('#articlehtml').val(txtcontent_fu);
                } else {
                    toastr.warning('请输入文章内容');
                    isVail = false;
                }
            } else {
                //使用的是md编辑器
                if (txtcontent_md.trim() != "") {
                    $('#articlehtml').val(txtcontent_md);
                } else {
                    toastr.warning('请输入文章内容');
                    isVail = false;
                }

            }
            var value = $('#sel1').val();
            var value2 = $('#sel2').val();
            if (value == "" ||
                value2 == "") {
                toastr.warning('选择博客类型或者文章类型');
                isVail = false;
            }
            if ($('#txtTitle').val() == "") {
                toastr.warning('标题不能为空');
                isVail = false;
            }
            return isVail;
        }
    },
    watch: {
        isDown: function (newval, oldval) {
            if (newval) {
                $('#usr-opt').slideDown();
            } else {
                $('#usr-opt').slideUp();
            }
        },
        isSearch: function (newval, oldval) {
            if (newval) {
                $('#search-posts').slideDown();
            } else {
                $('#search-posts').slideUp();
            }
        }
    }
});


