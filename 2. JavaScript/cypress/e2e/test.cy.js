describe('template spec', () => {
  it('passes', () => {
    cy.visit('http://localhost:50001/users/login');
    cy.get('#Users_LoginId').click();
    cy.get('#Users_LoginId').type('administrator');
    cy.get('#Users_Password').click();
    cy.get('#Users_Password').type('teppei1084');
    cy.get('#Login').click();
    cy.url().should('contains', 'http://localhost:50001/');
    cy.get('.new').click();
    cy.get('#StandardTemplates > li:nth-child(4)').click();
    cy.get('#OpenSiteTitleDialog').click();
    cy.get('#CreateByTemplate').click();
    cy.url().should('contains', 'http://localhost:50001/items/1/index');
    cy.get('.menubox:nth-child(3) img').click();
    cy.get('#block-03').click();
    cy.url().should('contains', 'http://localhost:50001/items/1/edit');
    cy.get('#ui-id-7').click();
    cy.get('#ToEnableEditorColumns').click();
    cy.get('#UpdateCommand').click();
    cy.url().should('contains', 'http://localhost:50001/items/1/index');
    cy.url().should('contains', 'http://localhost:50001/items/1/new');
    cy.get('#Results_Title').click();
    cy.get('#Results_Title').type('{backspace}');
    cy.get('#Results_Title').type('{backspace}');
    cy.get('#Results_Title').type('{backspace}');
    cy.get('#Results_Title').type('{backspace}');
    cy.get('#Results_Title').type('{backspace}');
    cy.get('#Results_Title').type('{backspace}');
    cy.get('#Results_Title').type('{backspace}');
    cy.get('#Results_Title').type('テスト');
    cy.get('#Results_ClassA').click();
    cy.get('#Results_ClassA').type('テスト');
    cy.get('#CreateCommand').click();
    cy.url().should('contains', 'http://localhost:50001/items/2/edit');
    cy.get('.menubox:nth-child(3) img').click();
    cy.get('#block-03').click();
    cy.url().should('contains', 'http://localhost:50001/items/1/edit');
    cy.get('#OpenDeleteSiteDialogCommand').click();
    cy.get('#DeleteSiteTitle').click();
    cy.get('#DeleteSiteTitle').type('記録テーブル');
    cy.get('#Users_LoginId').click();
    cy.get('#Users_LoginId').type('administrator');
    cy.get('#Users_Password').click();
    cy.get('#Users_Password').type('teppei1084');
    cy.get('.loading').click();
    cy.url().should('contains', 'http://localhost:50001/items/0/index');
    cy.get('.menubox:nth-child(4) img').click();
    cy.get('#block-05').click();
    cy.url().should('contains', 'http://localhost:50001/users/login');
  })
})