/**
 * @license Copyright (c) 2003-2018, CKSource - Frederico Knabben. All rights reserved.
 * For licensing, see LICENSE.md or https://ckeditor.com/legal/ckeditor-oss-license
 */

// Register a templates definition set named "default".
CKEDITOR.addTemplates( 'default', {
	// The name of sub folder which hold the shortcut preview images of the
	// templates.
	imagesPath: CKEDITOR.getUrl( CKEDITOR.plugins.getPath( 'templates' ) + 'templates/images/' ),

	// The templates definitions.
	templates: [
        {
            title: 'Blockquote',
            image: 'blockquote.png',
            description: 'Blockquote with or without attribution.',
            html: '<figure class="e-blockquote-wrap">' +
                    '<blockquote>Content inside a blockquote must be quoted from another source, whose address, if it has one, may be cited in the cite attribute [view source to add this attribute]. If the cite attribute is present, it must be a valid URL potentially surrounded by spaces... Attribution for the quotation, if any, must be placed outside the blockquote element.</blockquote>' +
                    '<figcaption>' +
                        '<p>—whatwg HTML Living Standard</p>' +
                    '</figcaption>' +
                '</figure>' + '&nbsp;'

        },
	    {
	        title: 'Statistics',
	        image: 'stats.png',
            description: 'Statistics widget.',
	        html: '<ul class="c-stat-list">' +
					'<li class="c-stat">' +
					'<img src="/build/images/group.png" alt="" />' + 
					'<p class="cloak-h2 is-red">1300+</p><hr><p class="is-bold">members</p></li>' +
					'<li class="c-stat">' +
					'<img src="/build/images/connect.png" alt="" />' + 
					'<p class="cloak-h2 is-red">350+</p><hr><p class="is-bold">organisations representeted across the public and private sectors</p></li>' +
					'<li class="c-stat">' +
					'<img src="/build/images/flag.png" alt="" />' + 
					'<p class="cloak-h2 is-red">60+</p><hr><p class="is-bold">cities around the world represented</p></li>' +
					'<li class="c-stat">' +
					'<img src="/build/images/present.png" alt="" />' + 
					'<p class="cloak-h2 is-red">20</p><hr><p class="is-bold">appointments to the bench</p></li>' +
	            '</ul>'
		},
		{
	        title: 'Alumni in the Media',
	        image: 'link.png',
            description: 'Alumni in the media link and text',
			html: '<ul class="unstyled">' +
					'<li><a href="/" class="is-large is-bold is-external" target="_blank">Read Janet Bolton\'s Story</a>' +
					'<p>Promoted to Associate Vice President Legal, Mergers and Acquisitions at ACME Inc.</p></li>' +
					'<li><a href="/" class="is-large is-bold is-external" target="_blank">Read Janet Bolton\'s Story</a>' +
					'<p>Promoted to Associate Vice President Legal, Mergers and Acquisitions at ACME Inc.</p></li>' +
					'<li><a href="/" class="is-large is-bold is-external" target="_blank">Read Janet Bolton\'s Story</a>' +
					'<p>Promoted to Associate Vice President Legal, Mergers and Acquisitions at ACME Inc.</p></li>' +
				'</ul>'
		}
    ]
} );
