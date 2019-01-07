
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
        editor: Object
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
            /* 富文本编辑器 */
            var E = window.wangEditor;
            var editor = new E('#divDemo');
            editor.customConfig.uploadImgShowBase64 = true;
            editor.customConfig.linkImgCallback = function (url) {
                alert("插入网络图片的回调") // url 即插入图片的地址
            }
            editor.create();
            v.editor = editor;
        }
        , setNavBarMenu: function () {
            var v = this;
            $('#usr-opt').hide();
            $('body').click(function (event) {
                if ($(event.target).data("flag") != 1) {
                    v.isDown = false;
                    v.isSearch = false;
                }
            });

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
            });
            $('.scroll-to-top').click(function () {
                $('html ,body').animate({ scrollTop: 0 }, 300);
                return false;
            });

            $('[data-toggle="tooltip"]').tooltip()
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
            });
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
            if (v.editor.txt.text() == "") {
                v.isShowVail = true;
                v.vaildataMessage = "请输入内容";
                setTimeout(function () { v.isShowVail=false},1000)
            }
            switch (flag) {
                case 1:
                    //发布
                    break;
                case 2:
                    //保存
                    break;
                case 3:
                    //预览
                    break;
            }
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


