/**
 * Copyright (c) 2003-2019, CKSource - Frederico Knabben. All rights reserved.
 * For licensing, see LICENSE.md or https://ckeditor.com/legal/ckeditor-oss-license
 */

// This file contains style definitions that can be used by CKEditor plugins.
//
// The most common use for it is the "stylescombo" plugin which shows the Styles drop-down
// list containing all styles in the editor toolbar. Other plugins, like
// the "div" plugin, use a subset of the styles for their features.
//
// If you do not have plugins that depend on this file in your editor build, you can simply
// ignore it. Otherwise it is strongly recommended to customize this file to match your
// website requirements and design properly.
//
// For more information refer to: https://ckeditor.com/docs/ckeditor4/latest/guide/dev_styles.html#style-rules

CKEDITOR.stylesSet.add( 'default', [
	/* Block styles */

	// These styles are already available in the "Format" drop-down list ("format" plugin),
	// so they are not needed here by default. You may enable them to avoid
	// placing the "Format" combo in the toolbar, maintaining the same features.

	{ name: 'Paragraph',		element: 'p' },
	{ name: 'Heading 1', 		element: 'h1', attributes: { 'class': 'custom-heading' } },
	{ name: 'Heading 2',		element: 'h2' },
	{ name: 'Heading 3',		element: 'h3' },
	{ name: 'Heading 4',		element: 'h4' },
	{ name: 'Heading 5',		element: 'h5' },
	{ name: 'Heading 6',		element: 'h6' },
	{ name: 'Bold Link',   element: 'a', attributes: {'class': 'is-bold is-grey'}},
	{ name: 'Button Link',      element: 'a', attributes: {'class': 'c-button c-button-primary'}},
	{ name: 'CTA Link',   element: 'a', attributes: {'class': 'c-link c-link-primary'}},
	{ name: 'CTA Link with Background',   element: 'a', attributes: {'class': 'c-link c-link-secondary'}},
	// { name: 'Preformatted Text',element: 'pre' },
	// { name: 'Address',			element: 'address' },


	/*
	{ name: 'Italic Title',		element: 'h2', styles: { 'font-style': 'italic' } },
	{ name: 'Subtitle',			element: 'h3', styles: { 'color': '#aaa', 'font-style': 'italic' } },
	{
		name: 'Special Container',
		element: 'div',
		styles: {
			padding: '5px 10px',
			background: '#eee',
			border: '1px solid #ccc'
		}
	},
	*/

	/* Inline styles */

	// These are core styles available as toolbar buttons. You may opt enabling
	// some of them in the Styles drop-down list, removing them from the toolbar.
	// (This requires the "stylescombo" plugin.)
	/*
	{ name: 'Strong',			element: 'strong', overrides: 'b' },
	{ name: 'Emphasis',			element: 'em'	, overrides: 'i' },
	{ name: 'Underline',		element: 'u' },
    */
	// { name: 'Strikethrough',	element: 'strike' },
	// { name: 'Subscript',		element: 'sub' },
	// { name: 'Superscript',		element: 'sup' },

	// { name: 'Marker',			element: 'span', attributes: { 'class': 'marker' } },

	// { name: 'Big',				element: 'big' },
	// { name: 'Small',			element: 'small' },
	// { name: 'Typewriter',		element: 'tt' },

	// { name: 'Computer Code',	element: 'code' },
	// { name: 'Keyboard Phrase',	element: 'kbd' },
	// { name: 'Sample Text',		element: 'samp' },
	// { name: 'Variable',			element: 'var' },

	// { name: 'Deleted Text',		element: 'del' },
	// { name: 'Inserted Text',	element: 'ins' },

	// { name: 'Cited Work',		element: 'cite' },
	// { name: 'Inline Quotation',	element: 'q' },

	{ name: 'Language: RTL',	element: 'span', attributes: { 'dir': 'rtl' } },
	{ name: 'Language: LTR',	element: 'span', attributes: { 'dir': 'ltr' } },

	/* Object styles */

    {
        name: 'Hide on Mobile',
        element: 'table',
        attributes: {'class': 'hide-for-small-only'}
    }

	// {
	// 	name: 'Styled image (left)',
	// 	element: 'img',
	// 	attributes: { 'class': 'left' }
	// },

	// {
	// 	name: 'Styled image (right)',
	// 	element: 'img',
	// 	attributes: { 'class': 'right' }
	// },

	// {
	// 	name: 'Compact table',
	// 	element: 'table',
	// 	attributes: {
	// 		cellpadding: '5',
	// 		cellspacing: '0',
	// 		border: '1',
	// 		bordercolor: '#ccc'
	// 	},
	// 	styles: {
	// 		'border-collapse': 'collapse'
	// 	}
	// },

	// { name: 'Borderless Table',		element: 'table',	styles: { 'border-style': 'hidden', 'background-color': '#E6E6FA' } },
	// { name: 'Square Bulleted List',	element: 'ul',		styles: { 'list-style-type': 'square' } }
] );

