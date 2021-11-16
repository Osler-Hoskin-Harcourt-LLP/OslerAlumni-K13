#####
# Sources:
# - Self-signed certificate generation (default):
#   https://docs.microsoft.com/en-us/powershell/module/pkiclient/new-selfsignedcertificate?view=win10-ps
# 
# - Self-signed certificate generation (third-party): 
#   https://gallery.technet.microsoft.com/scriptcenter/Self-signed-certificate-5920a7c6
# 
# - Copying certificate between certificate stores: 
#   https://gallery.technet.microsoft.com/Self-signed-certificate-5920a7c6/view/Discussions/2 (comment by Fisaro)
#
#####

$DestStoreScope = "LocalMachine"
$RootStoreName = "Root"
$PersonalStoreName = "My"

# NOTE: Spaces are important for exact name match (they are added automatically)
$CARootCertName = "CN=OslerAlumniCARoot, O=ecentricarts, OU=Dev, C=Canada" 
$ServerCertNames = @( `
	("osleralumni-admin-lh.ecentricarts.com", "Osler Alumni Admin Certificate"), `
	("osleralumni-lh.ecentricarts.com", "Osler Alumni Website Certificate" ) `
)

#region Certificate Authority

	# Check if the certificate with the same name already exists
	$RootStore = New-Object `
		-TypeName System.Security.Cryptography.X509Certificates.X509Store `
		-ArgumentList $RootStoreName, $DestStoreScope

	$RootStore.Open([System.Security.Cryptography.X509Certificates.OpenFlags]::ReadWrite)

	$CARootCert = $RootStore.Certificates | Where-Object -FilterScript {
		$_.Subject -eq $CARootCertName
	}

	# Create Certification Authority Root certificate
	if (!($CARootCert)) {
	
		$BasicExtension = [System.Security.Cryptography.X509Certificates.X509BasicConstraintsExtension]::new($true, $true, 0, $true)

		# Generate self-signed certificate.
		# This will store it in Personal > Certificates in the LocalMachine store.
		$CARootCert = New-SelfsignedCertificate `
			-DnsName $CARootCertName, "Osler Alumni CA Root" `
			-Provider "Microsoft Software Key Storage Provider" `
			-TextExtension @( `
				"2.5.29.19={critical} {text}ca=1&pathlength=0" ` # means Basic Contraints = Critical, CA = true, PathLength = 0
			) `
			-KeyUsage DigitalSignature, CRLSign, CertSign `
			-KeyUsageProperty All `
			-KeyExportPolicy Exportable `
			-KeyLength 4096 `
			-CertStoreLocation ("cert:\" + $DestStoreScope + "\" + $PersonalStoreName)
				
		# Copy the generated certificate to Trusted Root Certificate Authorities > Certificates 
		# in the LocalMachine store.
		$RootStore.Add($CARootCert)
	}

	$RootStore.Close()

	# Display the self-signed certificate
	$CARootCert

#endregion

#region Server Certificates

	# Check if the certificates with the same name already exist
	$PersonalStore = New-Object `
		-TypeName System.Security.Cryptography.X509Certificates.X509Store `
		-ArgumentList $PersonalStoreName, $DestStoreScope

	$PersonalStore.Open([System.Security.Cryptography.X509Certificates.OpenFlags]::ReadWrite)

	foreach ($certNames in $ServerCertNames) {
	
		$serverCert = $PersonalStore.Certificates | Where-Object -FilterScript {
			$_.Subject -eq $certNames[0]
		}

		# Create Server SSL certificate for Admin site
		If (!($serverCert)) {

			# Generate self-signed certificate.
			# This will store it in Personal > Certificates in the LocalMachine store.
			$serverCert = New-SelfsignedCertificate `
				-DnsName $certNames `
				-Signer $CARootCert `
				-TextExtension @( `
					"2.5.29.37={text}1.3.6.1.5.5.7.3.1,1.3.6.1.5.5.7.3.2" ` # means Enhanced Key Usage = "Server Authentication, Client authentication"
				) `
				-KeyUsage KeyEncipherment, DigitalSignature `
				-KeyExportPolicy Exportable `
				-KeyLength 4096 `
				-CertStoreLocation ("cert:\" + $DestStoreScope + "\" + $PersonalStoreName)
		}

		# Display the self-signed certificate
		$serverCert
	}
	
#endregion