# Frontend - Sistema Cadastral

Sistema de gerenciamento cadastral desenvolvido com Angular 18 e PrimeNG.

## 🚀 Tecnologias

- **Angular 18+**
- **PrimeNG** - Biblioteca de componentes UI
- **PrimeIcons** - Ícones
- **TypeScript**
- **SCSS**
- **RxJS**
- **Karma/Jasmine** - Testes unitários

## 📋 Funcionalidades

### CRUD Completo para:
- ✅ **Endereços** - Gerenciamento de endereços
- ✅ **Pessoas Físicas** - Cadastro de pessoas físicas (CPF, Nome, Data de Nascimento)
- ✅ **Pessoas Jurídicas** - Cadastro de pessoas jurídicas (CNPJ, Razão Social)

### Recursos Implementados:
- ✅ **Lazy Loading** - Carregamento sob demanda de módulos
- ✅ **Componentes Standalone** - Arquitetura moderna do Angular
- ✅ **Layout Responsivo** - Header, Menu e Footer separados
- ✅ **Notificações Toast** - Mensagens de sucesso e erro
- ✅ **Validação de Formulários** - Validação reativa
- ✅ **Confirmação de Exclusão** - Diálogos de confirmação
- ✅ **Comunicação com API** - Serviços HTTP integrados
- ✅ **Cobertura de Testes** - Testes unitários implementados

## 🛠️ Pré-requisitos

- Node.js (versão 18 ou superior)
- npm (versão 9 ou superior)
- Angular CLI (versão 18 ou superior)

## 📦 Instalação

```bash
# Instalar dependências
npm install

# Verificar instalação
npm list
```

## 🎯 Como Executar

### Modo Desenvolvimento

```bash
# Iniciar servidor de desenvolvimento (com proxy para API)
npm start

# O aplicativo estará disponível em http://localhost:4200
# As requisições para /api serão redirecionadas para http://localhost:5000
```

### Build de Produção

```bash
# Compilar para produção
npm run build

# Os arquivos compilados estarão em dist/frontend
```

### Modo Watch

```bash
# Compilar com watch mode
npm run watch
```

## 🧪 Testes

### Executar Testes

```bash
# Executar testes com Karma
npm test

# Executar testes com cobertura
npm run test:coverage

# A cobertura será gerada em coverage/
```

## 🔗 Rotas Implementadas

| Rota | Descrição | Lazy Loading |
|------|-----------|--------------|
| `/addresses` | Lista de endereços | ✅ |
| `/addresses/new` | Novo endereço | ✅ |
| `/addresses/edit/:id` | Editar endereço | ✅ |
| `/natural-persons` | Lista de pessoas físicas | ✅ |
| `/natural-persons/new` | Nova pessoa física | ✅ |
| `/natural-persons/edit/:id` | Editar pessoa física | ✅ |
| `/legal-persons` | Lista de pessoas jurídicas | ✅ |
| `/legal-persons/new` | Nova pessoa jurídica | ✅ |
| `/legal-persons/edit/:id` | Editar pessoa jurídica | ✅ |

## 🔌 Configuração da API

O proxy está configurado para redirecionar requisições `/api` para `http://localhost:5000`.

Para alterar o endereço da API, edite:
1. `proxy.conf.json` - URL de destino do proxy
2. `src/environments/environment.ts` - URL base da API

```typescript
// environment.ts
export const environment = {
  production: false,
  apiUrl: '/api'
};
```

## 🎨 Componentes PrimeNG Utilizados

- **Table** - Tabelas com paginação
- **Button** - Botões com ícones
- **InputText** - Campos de texto
- **Calendar** - Seletor de datas
- **Toast** - Notificações
- **ConfirmDialog** - Diálogos de confirmação

## 📝 Scripts Disponíveis

- `npm start` - Inicia servidor de desenvolvimento com proxy
- `npm run build` - Build de produção
- `npm run watch` - Build com watch mode
- `npm test` - Executa testes
- `npm run test:coverage` - Testes com cobertura

## 🤝 Integração com Backend

Para executar com a API backend:

1. Inicie a API do .NET (porta 5000)
2. Inicie o frontend: `npm start`
3. Acesse: http://localhost:4200

As requisições serão automaticamente proxy para a API.
