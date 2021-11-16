/**
 * @license Copyright (c) 2003-2015, CKSource - Frederico Knabben. All rights reserved.
 * For licensing, see LICENSE.md or http://ckeditor.com/license
 */

CKEDITOR.editorConfig = function( config ) {
    config.allowedContent = true; // To disable CKEditor ACF
    config.dialog_backgroundCoverColor = '#888888';
    //config.skin = 'moono-lisa';
    config.enterMode = CKEDITOR.ENTER_P;
    config.shiftEnterMode = CKEDITOR.ENTER_BR;
    config.entities_latin = false;
    //config.contentsCss = [ CKEDITOR.getUrl('contents.css'), './fonts/fonts.css' ];
    //config.font_names = 'CelesteSans-Bold/CelesteSans-Bold;' + config.font_names;
    // config.contentsCss = 'C:\inetpub\wwwroot\osler\alumni\Alumni_Website\OslerAlumni.Mvc\build'

    var sourceName = config.useInlineMode ? 'Sourcedialog' : 'Source';

    config.plugins += ',templates';

    config.toolbar_Standard =
    [
        [sourceName, '-'],
        ['Undo', 'Redo', '-'],
        ['Bold', 'Italic', 'Underline', 'TextColor', 'Subscript', 'Superscript', '-'],
        ['Styles'],
        ['NumberedList', 'BulletedList', 'Outdent', 'Indent', '-'],
        ['JustifyLeft', 'JustifyCenter', 'JustifyRight', 'JustifyBlock', '-'],
        ['InsertLink', 'Unlink', '-'],
        ['InsertImageOrMedia', 'QuicklyInsertImage', 'Table', 'InsertWidget', 'InsertMacro', '-'],
        ['Maximize']
    ];

    config.toolbar_Osler = config.toolbar_Default =
    [
        [sourceName, '-'],
        ['Cut', 'Copy', 'PasteText', 'PasteFromWord', '-'],
        ['Undo', 'Redo', '-'],
        ['Bold', 'Italic', 'Underline', 'TextColor', 'Subscript', 'Superscript', '-'],
        ['Styles'],
        ['NumberedList', 'BulletedList', 'Outdent', 'Indent', '-'],
        ['JustifyLeft', 'JustifyCenter', 'JustifyRight', 'JustifyBlock', '-'],
        ['InsertLink', 'Unlink', 'Anchor', '-'],
        ['InsertImageOrMedia', 'Table', 'HorizontalRule', 'InsertWidget', 'InsertMacro', '-'],
        ['Maximize'],
        ['Templates']
    ];

    config.toolbar_Full = [
        [sourceName, '-'],
        ['Cut', 'Copy', 'Paste', 'PasteText', 'PasteFromWord', 'Scayt', '-'],
        ['Undo', 'Redo', 'Find', 'Replace', 'RemoveFormat', '-'],
        ['Bold', 'Italic', 'Underline', 'Strike', 'Subscript', 'Superscript', '-'],
        ['NumberedList', 'BulletedList', 'Outdent', 'Indent', 'Blockquote', 'CreateDiv', '-'],
        ['JustifyLeft', 'JustifyCenter', 'JustifyRight', 'JustifyBlock', '-'],
        '/',
        ['InsertLink', 'Unlink', 'Anchor', '-'],
        ['InsertImageOrMedia', 'QuicklyInsertImage', 'Table', 'HorizontalRule', 'SpecialChar', '-'],
        ['InsertForms', 'InsertPolls', 'InsertRating', 'InsertYouTubeVideo', 'InsertWidget', '-'],
        ['Styles', 'Format', 'Font', 'FontSize'],
        ['TextColor', 'BGColor', '-'],
        ['InsertMacro', '-'],
        ['Maximize', 'ShowBlocks']
    ];

    config.toolbar_Basic = [
        ['Bold', 'Italic', '-', 'NumberedList', 'BulletedList', '-', 'InsertLink', 'Unlink']
    ];

    config.toolbar_BizForm = [
        ['Source', '-'],
        ['Cut', 'Copy', 'Paste', 'PasteText', 'PasteFromWord', '-'],
        ['Undo', 'Redo', 'Find', 'Replace', 'RemoveFormat', '-'],
        ['JustifyLeft', 'JustifyCenter', 'JustifyRight', 'JustifyBlock', '-'],
        ['Bold', 'Italic', 'Underline', 'Strike', 'Subscript', 'Superscript', '-'],
        ['NumberedList', 'BulletedList', 'Outdent', 'Indent', '-'],
        ['InsertLink', 'Unlink', 'Anchor', '-'],
        ['InsertImageOrMedia', 'Table', 'HorizontalRule', 'SpecialChar', '-'],
        ['Styles', 'Format', 'Font', 'FontSize'],
        ['TextColor', 'BGColor', '-'],
        ['InsertMacro', '-'],
        ['Maximize']
    ];

    config.toolbar_Forum = [
        ['Bold', 'Italic', '-', 'InsertLink', 'InsertUrl', 'InsertImageOrMedia', 'InsertImage', 'InsertQuote', '-', 'NumberedList', 'BulletedList', '-', 'TextColor', 'BGColor']
    ];

    config.toolbar_Reporting = [
        ['Source', '-'],
        ['Cut', 'Copy', 'Paste', 'PasteText', 'PasteFromWord', '-'],
        ['Undo', 'Redo', 'Find', 'Replace', 'RemoveFormat', '-'],
        ['Bold', 'Italic', 'Underline', 'Strike', 'Subscript', 'Superscript', '-'],
        ['NumberedList', 'BulletedList', 'Outdent', 'Indent', '-'],
        ['JustifyLeft', 'JustifyCenter', 'JustifyRight', 'JustifyBlock', '-'],
        ['InsertLink', 'Unlink', 'Anchor', '-'],
        ['InsertImageOrMedia', 'QuicklyInsertImage', 'Table', 'HorizontalRule', 'SpecialChar', '-'],
        ['Styles', 'Format', 'Font', 'FontSize'],
        ['TextColor', 'BGColor', '-'],
        ['InsertMacro', '-'],
        ['Maximize']
    ];

    config.toolbar_SimpleEdit = [
        ['Cut', 'Copy', 'Paste', 'PasteText', 'PasteFromWord', '-'],
        ['Undo', 'Redo', 'Find', 'Replace', 'RemoveFormat', '-'],
        ['Bold', 'Italic', 'Underline', 'Strike', 'Subscript', 'Superscript', '-'],
        ['NumberedList', 'BulletedList', 'Outdent', 'Indent', '-'],
        ['JustifyLeft', 'JustifyCenter', 'JustifyRight', 'JustifyBlock', '-'],
        ['InsertLink', 'Unlink', 'Anchor', '-'],
        ['InsertImageOrMedia', 'QuicklyInsertImage', 'Table', 'HorizontalRule', 'SpecialChar', '-'],
        ['Styles', 'Format', 'Font', 'FontSize'],
        ['TextColor', 'BGColor', '-'],
        ['Maximize']
    ];

    config.toolbar_Invoice = [
        ['Source', '-'],
        ['Cut', 'Copy', 'Paste', 'PasteText', 'PasteFromWord', 'Scayt', '-'],
        ['Undo', 'Redo', 'Find', 'Replace', 'RemoveFormat', '-'],
        ['Bold', 'Italic', 'Underline', 'Strike', 'Subscript', 'Superscript', '-'],
        ['NumberedList', 'BulletedList', 'Outdent', 'Indent', 'Blockquote', 'CreateDiv', '-'],
        ['JustifyLeft', 'JustifyCenter', 'JustifyRight', 'JustifyBlock', '-'],
        ['InsertImageOrMedia', 'Table', 'HorizontalRule', 'SpecialChar', '-'],
        ['Styles', 'Format', 'Font', 'FontSize'],
        ['TextColor', 'BGColor', '-'],
        ['InsertMacro', '-'],
        ['Maximize', 'ShowBlocks']
    ];

    config.toolbar_Group = [
	    ['Bold', 'Italic', '-', 'NumberedList', 'BulletedList', '-', 'InsertLink', 'Unlink', 'InsertGroupPolls']
    ];

    config.toolbar_Widgets = [
        ['Bold', 'Italic', '-', 'NumberedList', 'BulletedList', '-', 'InsertLink', 'Unlink', 'InsertImageOrMedia', '-'],
        ['Format', 'Font', 'FontSize'],
        ['TextColor', 'BGColor']
    ];

    config.toolbar_EmailWidgets = [
        ['Bold', 'Italic', 'Underline', '-', 'NumberedList', 'BulletedList', '-', 'PasteText', 'PasteFromWord', '-', 'InsertMacro', '-']
    ];

    config.toolbar_Consents_ShortText = [
        ['Source', '-', 'Bold', 'Italic', '-', 'NumberedList', 'BulletedList', '-', 'InsertLink', 'Unlink', '-', 'PasteText', 'PasteFromWord']
    ];

    config.toolbar_Consents_FullText = [
        ['Source', '-', 'Bold', 'Italic', '-', 'NumberedList', 'BulletedList', '-', 'InsertLink', 'Unlink', '-', 'PasteText', 'PasteFromWord'],
        ['Styles', 'Format', 'Font', 'FontSize'],
        ['TextColor', 'BGColor', '-']
    ];

    config.toolbar_Disabled = [
        ['Maximize']
    ];

    config.toolbar = config.toolbar_Osler;

    config.scayt_customerid = '1:vhwPv1-GjUlu4-PiZbR3-lgyTz1-uLT5t-9hGBg2-rs6zY-qWz4Z3-ujfLE3-lheru4-Zzxzv-kq4';
};
