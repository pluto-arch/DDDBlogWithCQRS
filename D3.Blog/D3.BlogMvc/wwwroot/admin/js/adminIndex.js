
var datass = [];
Vue.prototype.$http = axios;
var app = new Vue({
    el: '#app',
    data: {
        message: 'Collapsible Sidebar Using Bootstrap 4!',
        
    },
    mounted: function () {
        this.$nextTick(function () {
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
    methods: {
    },
    watch: {
     
    }
});