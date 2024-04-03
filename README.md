# Zipper
Aplicação em linha de comandos que consegue criar um zip de uma pasta e respetivas sub-pastas, excluindo determinadas extensões, pastas ou nomes de ficheiros.

O programa também permite que o ficheiro output possa ser gerado para uma pasta local, copiado
para uma fileshare ou enviado como anexo por Email (SMTP).
Desenvolver o programa usando as melhores práticas de POO e SOLID, bem como respetivos testes
unitários e de integração.

Requisito #1
O utilizador pode invocar a aplicação via linha de comandos passando como argumentos:
- a pasta a zipar (e.g. C:\\temp)
- o nome final do ficheiro zip (e.g. final.zip)
- uma lista de extensões a excluir (e.g. .bmp, .jpg, .txt)
- uma lista de diretórios a excluir (e.g. git, diretório)
- uma lista de ficheiros a excluir (e.g. ficheiro1, filcheiro2)
- tipo de output (e.g. localFile, filesShare, SMTP)
- parâmetros opcionais de acordo com o tipo de output (e.g. path do fileshare)
  
## Requisito #2
Todos os ficheiros e pastas devem ser incluídos no ficheiro de output num ficheiro ZIP.

## Requisito #3
Criar um design de “outputs” em que seja fácil desenvolver no futuro novos outputs.

## Requisito #4
Desenvolver testes unitários para o código atingindo o máximo de cobertura possível.

## Requisito #5
A aplicação pode ser tanto executada em .NET Core 3.1 como em .NET Framework 4.8
