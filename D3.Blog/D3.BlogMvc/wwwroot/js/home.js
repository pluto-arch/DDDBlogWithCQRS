
var datass= [
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
var vue = new Vue({
    el: '#app',
    data: {
        message: 'hello',
        accounts: 'account',
        count: 6,
        show: false,
        conn:Object,
        posts:datass

    },
    mounted: function () {
        this.$nextTick(function () {
            var v = this;
//            $('#form1').on('submit',
//                function() {
//                    v.Login();
//                    event.preventDefault(); //阻止form表单默认提交
//                });
        });
    },
    methods: {
        loadmore: function () {
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
        ,Login: function() {
            var v = this;
            v.$http.post('/Account/LoginAsync', $('#form1').serialize())
                .then(res => {
                    console.log(res);
                }).catch(error => {
                    console.log(error);
                });
        }
    }
});

