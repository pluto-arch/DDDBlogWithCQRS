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
        count: 6
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
            value = 10;
            window.location.href = "/Post/PostDetails?id=" + value;
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