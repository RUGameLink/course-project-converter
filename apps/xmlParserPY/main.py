import xml.etree.ElementTree as ET
root = ET.parse('2.xml').getroot()
print(root)

for child in root.iter('XMI.content'):
	print(child.tag, child.attrib)
	for model in child.iter('UML:Model'):
		for packages in model.find('UML:Package').findall('UML:Namespace.ownedElement'):
			print(packages.tag, packages.attrib)

