var datass = [
    {
        id: 1,
        text: 'asp.net core',
        content: '这是一个跨平台的框架这是一个跨平台的框架这是一个跨平台的框架这是一个跨平台的框架这是一个跨平台的框架',
        author: 'admin',
        createtime: '2018-10-02',
        count1: 123,
        count2: 231
    },
    {
        id: 2,
        text: 'asp.net core',
        content: '这是一个跨平台的框架这是一个跨平台的框架这是一个跨平台的框架这是一个跨平台的框架这是一个跨平台的框架',
        author: 'admin',
        createtime: '2018-10-02',
        count1: 123,
        count2: 231
    },
    {
        id: 3,
        text: 'asp.net core',
        content: '这是一个跨平台的框架这是一个跨平台的框架这是一个跨平台的框架这是一个跨平台的框架这是一个跨平台的框架这是一个跨平台的框架这是一个跨平台的框架',
        author: 'admin',
        createtime: '2018-10-02',
        count1: 123,
        count2: 231
    },
    {
        id: 4,
        text: 'asp.net core',
        content: '这是一个跨平台的框架这是一个跨平台的框架这是一个跨平台的框架这是一个跨平台的框架这是一个跨平台的框架这是一个跨平台的框架',
        author: 'admin',
        createtime: '2018-10-02',
        count1: 123,
        count2: 231
    },
    {
        id: 5,
        text: 'asp.net core',
        content: '这是一个跨平台的框架这是一个跨平台的框架这是一个跨平台的框架这是一个跨平台的框架这是一个跨平台的框架这是一个跨平台的框架',
        author: 'admin',
        createtime: '2018-10-02',
        count1: 123,
        count2: 231
    }, {
        id: 6,
        text: 'asp.net core',
        content: '这是一个跨平台的框架这是一个跨平台的框架这是一个跨平台的框架这是一个跨平台的框架这是一个跨平台的框架这是一个跨平台的框架这是一个跨平台的框架这是一个跨平台的框架',
        author: 'admin',
        createtime: '2018-10-02',
        count1: 123,
        count2: 231
    }
];
Vue.prototype.$http = axios;
var app = new Vue({
    el: '#app',
    data: {
        message: 'Collapsible Sidebar Using Bootstrap 4!',
        isDown: false,
        isSearch: false,
        posts: datass,
        count: 6,
        isSignUp:false
    },
    mounted: function () {
        this.$nextTick(function () {
            this.setNavBarMenu();
            toastr.options = {
                closeButton: true,                                            // 是否显示关闭按钮，（提示框右上角关闭按钮）
                debug: false,                                                    // 是否使用deBug模式
                progressBar: false,                                            // 是否显示进度条，（设置关闭的超时时间进度条）
                positionClass: "toast-top-center",              // 设置提示款显示的位置
                onclick: null,                                                     // 点击消息框自定义事件 
                showDuration: "300",                                      // 显示动画的时间
                hideDuration: "2000",                                     //  消失的动画时间
                timeOut: "2000",                                             //  自动关闭超时时间 
                extendedTimeOut: "2000",                             //  加长展示时间
                showEasing: "swing",                                     //  显示时的动画缓冲方式
                hideEasing: "linear",                                       //   消失时的动画缓冲方式
                showMethod: "fadeIn",                                   //   显示时的动画方式
                hideMethod: "fadeOut"                                   //   消失时的动画方式
            };
        });
    },
    mounted: function () {
        this.$nextTick(function () {
            this.LoadPostList();
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
                        toastr.info('出现错误：<span style="color:yellow">' + res.data + '</span>');
                    } else {
                        window.location.reload();
                    }
                }).catch(error => {
                    toastr.info('出现错误：' + error);
                });
        }
        , SignUp: function() {
            var v = this;
            v.$http.post('/Account/RegisterAsync', $('#formSignUp').serialize())
                .then(res => {
                    if (res.data.length > 0) {
                        toastr.info('出现错误：<span style="color:yellow">' + res.data + '</span>');
                    } else {
                        window.location.reload();
                    }
                }).catch(error => {
                    toastr.info('出现错误：' + error);
                });
        }
        , dropDownOpt: function () {
            //个人操作下拉
            var v = this;
            v.isDown = !v.isDown;
        }
        , loadmore: function () {
            var v = this;
            v.count++;
            $('.sk-three-bounce').show();
            $('.more').hide();
            $('.sk-child').addClass('activeLoading');
            setTimeout(function () {
                $('.sk-child').removeClass('activeLoading');
                $('.sk-three-bounce').hide();
                $('.more').show();
                //从后台加载数据
                var readdata = [
                    {
                        id: v.count,
                        text: 'asp.net core',
                        content: '这是一个跨平台的框架这是一个跨平台的框架这是一个跨平台的框架这是一个跨平台的框架这是一个跨平台的框架',
                        author: 'admin',
                        createtime: '2018-10-02',
                        count1: 123,
                        count2: 231
                    }
                ];
                readdata.forEach(function (v) {
                    datass.push(v);
                });
            },
                2000);
        }
        , optSearch: function () {
            this.isSearch = !this.isSearch;           
        }
        , showPostDetails: function (value, event) {
            value = 5;
            window.location.href = "/Post/PostDetails/" + value;
        }
        , LoadPostList: function () {
            var v = this;
            v.$http.get('/Home/GetPostList')
                .then(res => {
                    console.log(res);
                   
                }).catch(error => {
                    toastr.info('出现错误：' + error);
                });
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