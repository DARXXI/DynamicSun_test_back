var AdminPanel = {
    Init: function(test) {

    },
    Utils:
    {
        dateFormatter: function(date, format = 'L') {
            moment.locale("ru");
            return moment(date).format(format);
        }
    },
};  


