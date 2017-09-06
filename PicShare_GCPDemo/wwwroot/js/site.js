/// <reference path="../lib/knockout/dist/knockout.js" />
/// <reference path="../lib/knockout-mapping/knockout.mapping.js" />

function PicShareViewModel() {
    var self = this;

    // Navigation info
    var popups = ["AddPic", "Slideshow"];
    self.currentPopup = ko.observable();

    // Photos
    self.pics = ko.observableArray([]);
    self.searchText = ko.observable('');
    self.searchPics = function () {
        var curSearch = self.searchText();
    };

    // Upload
    self.uploadCaption = ko.observable();

    // Data Init
    // todo: fetch initial pictures
}

ko.applyBindings(new PicShareViewModel());

$("#uploadForm").submit(function () {

    var formData = new FormData(this);

    $.post("/api/Pictures", formData, function (data) {
        alert(data);
    });

    return false;
});