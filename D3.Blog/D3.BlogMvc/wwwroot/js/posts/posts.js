﻿
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
        isShowVail: false
    },
    mounted: function () {
        this.$nextTick(function () {
            this.setNavBarMenu();
        });
    },
    methods: {
        setNavBarMenu: function () {
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

            $('#aside-content .write-aside').click(function() {
                var clickel = this;
                $('#aside-content .write-aside').removeClass('checked');
                $(clickel).addClass('checked');
            });//写博客页面侧边菜单

            $('#tokenfield').tokenfield({ minWidth:400});//标签输入


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
                setTimeout(function() { v.isShowVail = false }, 1000);
            }
            console.log(v.mdeditor.getHTML());
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


